using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;

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
            using (var database = Database.CreateInstance())
            {
                List<DbFavorite> toAdd = favorites.Cast<DbFavorite>().ToList();
                AddAllToDatabase(database, toAdd);
                database.SaveImmediatelyIfRequested();
                database.DetachAll(toAdd);
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
            using (var database = Database.CreateInstance())
            {
                var toUpdate = favorite as DbFavorite;
                if (toUpdate != null)
                {
                    database.AttachFavorite(toUpdate);
                    this.TrySaveAndReportFavoriteUpdate(toUpdate, database);
                }
            }
        }

        public void UpdateFavorite(IFavorite favorite, List<IGroup> newGroups)
        {
            using (var database = Database.CreateInstance())
            {
                var toUpdate = favorite as DbFavorite;
                if (toUpdate == null)
                    return;

                database.AttachFavorite(toUpdate);
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
            Data.Groups.RemoveFavoritesFromGroups(new List<IFavorite> {favorite}, redundantGroups);
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
            favorite.MarkAsModified(database);
            database.SaveImmediatelyIfRequested();
            database.DetachFavorite(favorite);
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
            using (var database = Database.CreateInstance())
            {
                List<DbFavorite> favoritesToDelete = favorites.Cast<DbFavorite>().ToList();
                DeleteFavoritesFromDatabase(database, favoritesToDelete);
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
            database.AttachAll(favorites);
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
            using (var database = Database.CreateInstance())
            {
                Data.Favorites.ApplyCredentialsToFavorites(selectedFavorites, credential);
                SaveAndReportFavoritesUpdated(database, selectedFavorites);
            }
        }

        private void SaveAndReportFavoritesUpdated(Database database, List<IFavorite> selectedFavorites)
        {
            database.SaveImmediatelyIfRequested();
            List<DbFavorite> toUpdate = selectedFavorites.Cast<DbFavorite>().ToList();
            this.cache.Update(toUpdate);
            this.dispatcher.ReportFavoritesUpdated(selectedFavorites);
        }

        public void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            using (var database = Database.CreateInstance())
            {
                Data.Favorites.SetPasswordToFavorites(selectedFavorites, newPassword);
                SaveAndReportFavoritesUpdated(database, selectedFavorites);
            }
        }

        public void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            using (var database = Database.CreateInstance())
            {
                Data.Favorites.ApplyDomainNameToFavorites(selectedFavorites, newDomainName);
                SaveAndReportFavoritesUpdated(database, selectedFavorites);
            }
        }

        public void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            using (var database = Database.CreateInstance())
            {
                Data.Favorites.ApplyUserNameToFavorites(selectedFavorites, newUserName);
                SaveAndReportFavoritesUpdated(database, selectedFavorites);
            }
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
            using (var database = Database.CreateInstance())
            {
                // to list because Linq to entities allows only cast to primitive types
                List<DbFavorite> favorites = database.Favorites.ToList();
                database.DetachAll(favorites);
                favorites.ForEach(candidate => candidate.AssignStores(this.groups, this.credentials, this.persistenceSecurity));
                return favorites;
            }
        }

        private static List<DbFavorite> LoadFromDatabase(List<DbFavorite> toRefresh)
        {
            using (var database = Database.CreateInstance())
            {
                if (toRefresh != null)
                    database.AttachAll(toRefresh);

                // to list because Linq to entities allows only cast to primitive types
                ((IObjectContextAdapter)database).ObjectContext.Refresh(RefreshMode.StoreWins, database.Favorites);
                List<DbFavorite> favorites = database.Favorites.ToList();
                database.DetachAll(favorites);
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
