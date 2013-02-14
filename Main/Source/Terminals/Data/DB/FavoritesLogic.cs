using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Transactions;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL persisted favorites container
    /// </summary>
    internal class Favorites : IFavorites, IEnumerable<DbFavorite>
    {
        private readonly Groups groups;

        private readonly StoredCredentials credentials;

        private readonly DataDispatcher dispatcher;
        private readonly EntitiesCache<DbFavorite> cache = new EntitiesCache<DbFavorite>();

        private List<DbFavorite> Cached
        {
            get { return this.cache.ToList(); }
        }

        private bool isLoaded;

        private readonly PersistenceSecurity persistenceSecurity;

        internal Favorites(Groups groups, StoredCredentials credentials,
            PersistenceSecurity persistenceSecurity, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.credentials = credentials;
            this.persistenceSecurity = persistenceSecurity;
            this.dispatcher = dispatcher;
        }

        IFavorite IFavorites.this[Guid favoriteId]
        {
            get
            {
                this.EnsureCache();
                return this.cache.FirstOrDefault(favorite => favorite.Guid == favoriteId);
            }
        }

        IFavorite IFavorites.this[string favoriteName]
        {
            get
            {
                this.EnsureCache();
                return this.cache.FirstOrDefault(favorite =>
                            favorite.Name.Equals(favoriteName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public void Add(IFavorite favorite)
        {
            var favoritesToAdd = new List<IFavorite> { favorite };
            this.Add(favoritesToAdd);
        }

        public void Add(List<IFavorite> favorites)
        {
            try
            {
                this.TryAdd(favorites);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to add favorite to database", exception);
            }
        }

        private void TryAdd(List<IFavorite> favorites)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                List<DbFavorite> toAdd = favorites.Cast<DbFavorite>().ToList();
                this.AddAllToDatabase(database, toAdd);
                database.SaveImmediatelyIfRequested();
                database.Cache.DetachAll(toAdd);
                this.cache.Add(toAdd);
                this.dispatcher.ReportFavoritesAdded(favorites);
            }
        }

        private void AddAllToDatabase(Database database, IEnumerable<DbFavorite> favorites)
        {
            foreach (DbFavorite favorite in favorites)
            {
                database.Favorites.Add(favorite);
            }
        }

        public void Update(IFavorite favorite)
        {
            try
            {
                this.TryUpdateFavorite(favorite);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to update favorite in database", exception);
            }
        }

        private void TryUpdateFavorite(IFavorite favorite)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var toUpdate = favorite as DbFavorite;
                database.Cache.AttachFavorite(toUpdate);
                this.TrySaveAndReportFavoriteUpdate(toUpdate, database);
            }
        }

        public void UpdateFavorite(IFavorite favorite, List<IGroup> newGroups)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    // do it in transaction, because here we do more things at once
                    this.TryUpdateFavorite(favorite, newGroups);
                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to update favorite and its groups in database", exception);
            }
        }

        private void TryUpdateFavorite(IFavorite favorite, List<IGroup> newGroups)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var toUpdate = favorite as DbFavorite;
                database.Cache.AttachFavorite(toUpdate);
                List<IGroup> addedGroups = this.groups.AddToDatabase(database, newGroups);
                // commit newly created groups, otherwise we cant add into them
                database.SaveImmediatelyIfRequested();
                List<DbGroup> removedGroups = this.UpdateGroupsMembership(favorite, newGroups, database);
                database.SaveImmediatelyIfRequested();

                List<IGroup> removedToReport = this.groups.DeleteFromCache(removedGroups);
                this.dispatcher.ReportGroupsRecreated(addedGroups, removedToReport);
                this.TrySaveAndReportFavoriteUpdate(toUpdate, database);
            }
        }

        private List<DbGroup> UpdateGroupsMembership(IFavorite favorite, List<IGroup> newGroups, Database database)
        {
            List<IGroup> redundantGroups = ListsHelper.GetMissingSourcesInTarget(favorite.Groups, newGroups);
            List<IGroup> missingGroups = ListsHelper.GetMissingSourcesInTarget(newGroups, favorite.Groups);
            Data.Favorites.AddIntoMissingGroups(favorite, missingGroups);
            Data.Groups.RemoveFavoritesFromGroups(new List<IFavorite> { favorite }, redundantGroups);
            List<DbGroup> removedGroups = this.groups.DeleteEmptyGroupsFromDatabase(database);
            return removedGroups;
        }

        private void TrySaveAndReportFavoriteUpdate(DbFavorite toUpdate, Database database)
        {
            try
            {
                this.SaveAndReportFavoriteUpdated(database, toUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                this.TryToRefreshUpdatedFavorite(toUpdate, database);
            }
        }

        private void TryToRefreshUpdatedFavorite(DbFavorite toUpdate, Database database)
        {
            try
            {
                database.Entry(toUpdate).Reload();
                ((IObjectContextAdapter)database).ObjectContext.Refresh(RefreshMode.ClientWins, toUpdate);
                this.SaveAndReportFavoriteUpdated(database, toUpdate);
            }
            catch (InvalidOperationException)
            {
                this.cache.Delete(toUpdate);
                this.dispatcher.ReportFavoriteDeleted(toUpdate);
            }
        }

        private void SaveAndReportFavoriteUpdated(Database database, DbFavorite favorite)
        {
            database.Cache.MarkFavoriteAsModified(favorite);
            database.SaveImmediatelyIfRequested();
            database.Cache.DetachFavorite(favorite);
            this.cache.Update(favorite);
            this.dispatcher.ReportFavoriteUpdated(favorite);
        }

        public void Delete(IFavorite favorite)
        {
            var favoritesToDelete = new List<IFavorite> { favorite };
            Delete(favoritesToDelete);
        }

        public void Delete(List<IFavorite> favorites)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    this.TryDelete(favorites);
                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to delete favorites from database", exception);
            }
        }

        private void TryDelete(List<IFavorite> favorites)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                List<DbFavorite> favoritesToDelete = favorites.Cast<DbFavorite>().ToList();
                this.DeleteFavoritesFromDatabase(database, favoritesToDelete);
                database.SaveImmediatelyIfRequested();
                this.groups.RefreshCache();
                List<DbGroup> deletedGroups = this.groups.DeleteEmptyGroupsFromDatabase(database);
                database.SaveImmediatelyIfRequested();
                List<IGroup> groupsToReport = this.groups.DeleteFromCache(deletedGroups);
                this.dispatcher.ReportGroupsDeleted(groupsToReport);
                this.cache.Delete(favoritesToDelete);
                this.dispatcher.ReportFavoritesDeleted(favorites);
            }
        }

        private void DeleteFavoritesFromDatabase(Database database, List<DbFavorite> favorites)
        {
            // we don't have to attach the details, because they will be deleted by reference constraints
            database.Cache.AttachAll(favorites);
            DeleteAllFromDatabase(database, favorites);
        }

        private void DeleteAllFromDatabase(Database database, IEnumerable<DbFavorite> favorites)
        {
            foreach (DbFavorite favorite in favorites)
            {
                database.Favorites.Remove(favorite);
            }
        }

        public SortableList<IFavorite> ToListOrderedByDefaultSorting()
        {
            return Data.Favorites.OrderByDefaultSorting(this);
        }

        public void ApplyCredentialsToAllFavorites(List<IFavorite> selectedFavorites, ICredentialSet credential)
        {
            try
            {
                this.TryApplyCredentials(selectedFavorites, credential);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to set credentials on favorites", exception);
            }
        }

        private void TryApplyCredentials(List<IFavorite> selectedFavorites, ICredentialSet credential)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var dbFavorites = selectedFavorites.Cast<DbFavorite>().ToList();
                Data.Favorites.ApplyCredentialsToFavorites(selectedFavorites, credential);
                database.Cache.AttachAll(dbFavorites);
                // here we have to mark it modified, because caching detail properties
                // sets proper credential set reference
                database.Cache.MarkAsModified(dbFavorites);
                this.SaveAndReportFavoritesUpdated(database, dbFavorites, selectedFavorites);
            }
        }

        public void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            try
            {
                this.TrySetPassword(selectedFavorites, newPassword);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable set password to favorites", exception);
            }
        }

        private void TrySetPassword(List<IFavorite> selectedFavorites, string newPassword)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var dbFavorites = selectedFavorites.Cast<DbFavorite>().ToList();
                database.Cache.AttachAll(dbFavorites);
                Data.Favorites.SetPasswordToFavorites(selectedFavorites, newPassword);
                this.SaveAndReportFavoritesUpdated(database, dbFavorites, selectedFavorites);
            }
        }

        public void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            try
            {
                this.TryApplyDomainName(selectedFavorites, newDomainName);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to set domain name to favorites", exception);
            }
        }

        private void TryApplyDomainName(List<IFavorite> selectedFavorites, string newDomainName)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var dbFavorites = selectedFavorites.Cast<DbFavorite>().ToList();
                database.Cache.AttachAll(dbFavorites);
                Data.Favorites.ApplyDomainNameToFavorites(selectedFavorites, newDomainName);
                this.SaveAndReportFavoritesUpdated(database, dbFavorites, selectedFavorites);
            }
        }

        public void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            try
            {
                this.TryApplyUserName(selectedFavorites, newUserName);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to apply user name to favorites", exception);
            }
        }

        private void TryApplyUserName(List<IFavorite> selectedFavorites, string newUserName)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var dbFavorites = selectedFavorites.Cast<DbFavorite>().ToList();
                database.Cache.AttachAll(dbFavorites);
                Data.Favorites.ApplyUserNameToFavorites(selectedFavorites, newUserName);
                this.SaveAndReportFavoritesUpdated(database, dbFavorites, selectedFavorites);
            }
        }

        private void SaveAndReportFavoritesUpdated(Database database, List<DbFavorite> dbFavorites,
            List<IFavorite> selectedFavorites)
        {
            database.SaveImmediatelyIfRequested();
            this.cache.Update(dbFavorites);
            this.dispatcher.ReportFavoritesUpdated(selectedFavorites);
        }

        private void EnsureCache()
        {
            if (isLoaded)
                return;

            List<DbFavorite> loaded = LoadFromDatabase();
            this.cache.Add(loaded);
            this.isLoaded = true;
        }

        internal void RefreshCache()
        {
            List<DbFavorite> newlyLoaded = LoadFromDatabase(this.Cached);
            List<DbFavorite> oldFavorites = this.Cached;
            List<DbFavorite> missing = ListsHelper.GetMissingSourcesInTarget(newlyLoaded, oldFavorites);
            List<DbFavorite> redundant = ListsHelper.GetMissingSourcesInTarget(oldFavorites, newlyLoaded);
            List<DbFavorite> toUpdate = ListsHelper.GetMissingSourcesInTarget(oldFavorites, redundant);

            this.cache.Add(missing);
            this.cache.Delete(redundant);
            this.RefreshCachedItems();

            var missingToReport = missing.Cast<IFavorite>().ToList();
            var redundantToReport = redundant.Cast<IFavorite>().ToList();
            var updatedToReport = toUpdate.Cast<IFavorite>().ToList();

            this.dispatcher.ReportFavoritesAdded(missingToReport);
            this.dispatcher.ReportFavoritesDeleted(redundantToReport);
            this.dispatcher.ReportFavoritesUpdated(updatedToReport);
        }

        private void RefreshCachedItems()
        {
            foreach (DbFavorite favorite in this.cache)
            {
                favorite.ReleaseLoadedDetails();
            }
        }

        private List<DbFavorite> LoadFromDatabase()
        {
            try
            {
                return this.TryLoadFromDatabase();
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to load favorites from database", exception);
                return new List<DbFavorite>();
            }
        }

        private List<DbFavorite> TryLoadFromDatabase()
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                // to list because Linq to entities allows only cast to primitive types
                List<DbFavorite> favorites = database.Favorites.ToList();
                database.Cache.DetachAll(favorites);
                favorites.ForEach(candidate => candidate.AssignStores(this.groups, this.credentials, this.persistenceSecurity));
                return favorites;
            }
        }

        private static List<DbFavorite> LoadFromDatabase(List<DbFavorite> toRefresh)
        {
            try
            {
                return TryLoadFromDatabase(toRefresh);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to refresh favorites from database", exception);
                // force to clear cache, because we are not able to refresh
                return new List<DbFavorite>();
            }
        }

        private static List<DbFavorite> TryLoadFromDatabase(List<DbFavorite> toRefresh)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                if (toRefresh != null)
                    database.Cache.AttachAll(toRefresh);

                // to list because Linq to entities allows only cast to primitive types
                ((IObjectContextAdapter)database).ObjectContext.Refresh(RefreshMode.StoreWins, database.Favorites);
                List<DbFavorite> favorites = database.Favorites.ToList();
                database.Cache.DetachAll(favorites);
                return favorites;
            }
        }

        #region IEnumerable members

        IEnumerator<IFavorite> IEnumerable<IFavorite>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<DbFavorite> GetEnumerator()
        {
            this.EnsureCache();
            return this.cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Favorites:Cached={0}", this.cache.Count());
        }
    }
}
