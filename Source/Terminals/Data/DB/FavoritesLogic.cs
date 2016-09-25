using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using Terminals.Connections;

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

        private readonly FavoritesBatchActions batchActions;

        private readonly ConnectionManager connectionManager;

        internal Favorites(Groups groups, StoredCredentials credentials,
            PersistenceSecurity persistenceSecurity, DataDispatcher dispatcher, ConnectionManager connectionManager)
        {
            this.groups = groups;
            this.credentials = credentials;
            this.dispatcher = dispatcher;
            this.connectionManager = connectionManager;
            this.batchActions = new FavoritesBatchActions(this, this.cache, this.dispatcher, persistenceSecurity);
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
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Add, favorites, this, exception,
                    "Unable to add favorite to database.");
            }
        }

        private void TryAdd(List<IFavorite> favorites)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                List<DbFavorite> toAdd = favorites.Cast<DbFavorite>().ToList();
                database.AddAll(toAdd);
                database.SaveImmediatelyIfRequested();
                database.Cache.DetachAll(toAdd);
                this.cache.Add(toAdd);
                this.dispatcher.ReportFavoritesAdded(favorites);
            }
        }

        public void Update(IFavorite favorite)
        {
            try
            {
                this.TryUpdateFavorite(favorite);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Update, favorite, this, exception,
                    "Unable to update favorite in database");
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
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(UpdateFavorite, favorite, newGroups, this, exception,
                    "Unable to update favorite and its groups in database.");
            }
        }

        private void TryUpdateFavorite(IFavorite favorite, List<IGroup> newGroups)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var toUpdate = favorite as DbFavorite;
                database.Cache.AttachFavorite(toUpdate);
                List<IGroup> addedGroups = database.AddToDatabase(newGroups);
                // commit newly created groups, otherwise we cant add into them
                database.SaveImmediatelyIfRequested();
                UpdateGroupsMembership(favorite, newGroups);
                database.SaveImmediatelyIfRequested();

                this.dispatcher.ReportGroupsAdded(addedGroups);
                this.TrySaveAndReportFavoriteUpdate(toUpdate, database);
            }
        }

        private static void UpdateGroupsMembership(IFavorite favorite, List<IGroup> newGroups)
        {
            List<IGroup> redundantGroups = ListsHelper.GetMissingSourcesInTarget(favorite.Groups, newGroups);
            List<IGroup> missingGroups = ListsHelper.GetMissingSourcesInTarget(newGroups, favorite.Groups);
            Data.Favorites.AddIntoMissingGroups(favorite, missingGroups);
            Data.Groups.RemoveFavoritesFromGroups(new List<IFavorite> { favorite }, redundantGroups);
        }

        private void TrySaveAndReportFavoriteUpdate(DbFavorite toUpdate, Database database)
        {
            try
            {
                this.SaveAndReportFavoriteUpdated(database, toUpdate);
            }
            catch (DbUpdateException)
            {
                this.TryToRefreshUpdatedFavorite(toUpdate, database);
            }
        }

        private void TryToRefreshUpdatedFavorite(DbFavorite toUpdate, Database database)
        {
            try
            {
                database.RefreshEntity(toUpdate);
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
                this.TryDeleteInTransaction(favorites);
            }
            catch (DbUpdateException) // item already removed
            {
                var toRemove = favorites.Cast<DbFavorite>().ToList();
                this.FinishRemove(favorites, toRemove);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Delete, favorites, this, exception,
                                                  "Unable to delete favorites from database");
            }
        }

        private void TryDeleteInTransaction(List<IFavorite> favorites)
        {
            using (var transaction = new TransactionScope())
            {
                this.TryDelete(favorites);
                transaction.Complete();
            }
        }

        private void TryDelete(List<IFavorite> favorites)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                List<DbFavorite> favoritesToDelete = favorites.Cast<DbFavorite>().ToList();
                List<DbCredentialBase> redundantCredentialBase = SelectRedundantCredentialBase(favoritesToDelete);
                this.DeleteFavoritesFromDatabase(database, favoritesToDelete);
                database.SaveImmediatelyIfRequested();
                database.RemoveRedundantCredentialBase(redundantCredentialBase);
                database.SaveImmediatelyIfRequested();
                this.groups.RefreshCache();
                this.FinishRemove(favorites, favoritesToDelete);
            }
        }

        private static List<DbCredentialBase> SelectRedundantCredentialBase(IEnumerable<DbFavorite> favorites)
        {
            return favorites.Where(f => f.Details.CredentialBaseToRemove != null)
                            .Select(f => f.Details.CredentialBaseToRemove)
                            .ToList();
        }

        private void FinishRemove(List<IFavorite> favorites, List<DbFavorite> favoritesToDelete)
        {
            this.cache.Delete(favoritesToDelete);
            this.dispatcher.ReportFavoritesDeleted(favorites);
        }

        private void DeleteFavoritesFromDatabase(Database database, List<DbFavorite> favorites)
        {
            // we don't have to attach the details, because they will be deleted by reference constraints
            database.Cache.AttachAll(favorites);
            database.DeleteAll(favorites);
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
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(ApplyCredentialsToAllFavorites, selectedFavorites, credential,
                     this, exception, "Unable to set credentials on favorites.");
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
                this.batchActions.SaveAndReportFavoritesUpdated(database, dbFavorites, selectedFavorites);
            }
        }

        public void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            this.batchActions.SetPasswordToFavorites(selectedFavorites, newPassword);
        }

        public void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            this.batchActions.ApplyDomainNameToFavorites(selectedFavorites, newDomainName);
        }

        public void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            this.batchActions.ApplyUserNameToFavorites(selectedFavorites, newUserName);
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
            List<DbFavorite> newlyLoaded = LoadFromDatabase();
            List<DbFavorite> oldFavorites = this.Cached;
            List<DbFavorite> missing = ListsHelper.GetMissingSourcesInTarget(newlyLoaded, oldFavorites, new ByIdComparer<DbFavorite>());
            List<DbFavorite> redundant = ListsHelper.GetMissingSourcesInTarget(oldFavorites, newlyLoaded, new ByIdComparer<DbFavorite>());
            List<DbFavorite> toUpdate = newlyLoaded.Intersect(oldFavorites, new ChangedVersionComparer()).ToList();

            this.cache.Add(missing);
            this.cache.Delete(redundant);
            this.cache.Update(toUpdate);
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
            catch (EntityException exception)
            {
                return this.dispatcher.ReportFunctionError(LoadFromDatabase, this, exception,
                    "Unable to load favorites from database.");
            }
        }

        private List<DbFavorite> TryLoadFromDatabase()
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                // to list because Linq to entities allows only cast to primitive types
                List<DbFavorite> favorites = database.Favorites.ToList();
                database.Cache.DetachAll(favorites);
                favorites.ForEach(this.PrepareFavorite);
                return favorites;
            }
        }

        private void PrepareFavorite(DbFavorite favorite)
        {
            favorite.AssignStores(this.groups, this.credentials, this.dispatcher);
            // not real change, but synchronizing loaded properties to empty state, before details are loaded from DB.
            var correctOptions = this.connectionManager.UpdateProtocolPropertiesByProtocol(favorite.Protocol, new EmptyOptions());
            favorite.ChangeProtocol(favorite.Protocol, correctOptions);
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
