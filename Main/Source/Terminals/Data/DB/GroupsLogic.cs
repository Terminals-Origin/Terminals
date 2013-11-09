using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL persisted groups container
    /// </summary>
    internal class Groups : IGroups
    {
        private DataDispatcher dispatcher;

        private Favorites favorites;

        private readonly EntitiesCache<DbGroup> cache;

        private bool isLoaded;

        private List<DbGroup> Cached
        {
            get
            {
                this.CheckCache();
                return this.cache.ToList();
            }
        }

        /// <summary>
        /// Gets cached item by its database unique identifier
        /// </summary>
        internal DbGroup this[int id]
        {
            get
            {
                this.CheckCache();
                return this.cache.FirstOrDefault(candidate => candidate.Id == id);
            }
        }

        internal Groups()
        {
            this.cache = new EntitiesCache<DbGroup>();
        }

        public void AssignStores(DataDispatcher dispatcher, Favorites favorites)
        {
            this.dispatcher = dispatcher;
            this.favorites = favorites;
        }

        IGroup IGroups.this[string groupName]
        {
            get
            {
                this.CheckCache();
                return this.cache.FirstOrDefault(group => 
                    group.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public void Add(IGroup group)
        {
            try
            {
                this.TryAdd(group);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Add, group, this, exception, "Unable to add group to database.");
            }
        }

        private void TryAdd(IGroup group)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                DbGroup toAdd = group as DbGroup;
                database.Groups.Add(toAdd);
                database.SaveImmediatelyIfRequested();
                database.Cache.Detach(toAdd);
                this.cache.Add(toAdd);
                this.dispatcher.ReportGroupsAdded(new List<IGroup> {toAdd});
            }
        }

        internal List<IGroup> AddToDatabase(Database database, List<IGroup> groups)
        {
            // not added groups don't have an identifier obtained from database
            List<IGroup> added = groups.Where(candidate => ((DbGroup)candidate).Id == 0).ToList();
            database.AddAll(added);
            List<DbGroup> toAttach = groups.Where(candidate => ((DbGroup)candidate).Id != 0).Cast<DbGroup>().ToList();
            database.Cache.AttachAll(toAttach);
            return added;
        }

        public void Update(IGroup group)
        {
            try
            {
                this.TryUpdateGroup(group);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Update, group, this, exception,
                    "Unable to update group in database");
            }

        }

        private void TryUpdateGroup(IGroup group)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var toUpdate = group as DbGroup;
                database.Cache.Attach(toUpdate);
                this.TrySaveAndReport(toUpdate, database);
            }
        }

        private void TrySaveAndReport(DbGroup toUpdate, Database database)
        {
            try
            {
                this.SaveAndReportUpdated(database, toUpdate);
            }
            catch (DbUpdateException)
            {
                this.TryToRefreshUpdated(toUpdate, database);
            }
        }

        private void TryToRefreshUpdated(DbGroup toUpdate, Database database)
        {
            try
            {
                database.RefreshEntity(toUpdate);
                this.SaveAndReportUpdated(database, toUpdate);
            }
            catch (InvalidOperationException)
            {
                this.cache.Delete(toUpdate);
                this.dispatcher.ReportGroupsDeleted(new List<IGroup>(){ toUpdate });
            }
        }

        private void SaveAndReportUpdated(Database database, DbGroup toUpdate)
        {
            database.Cache.MarkAsModified(toUpdate);
            database.SaveImmediatelyIfRequested();
            database.Cache.Detach(toUpdate);
            this.cache.Update(toUpdate);
            this.dispatcher.ReportGroupsUpdated(new List<IGroup>() { toUpdate });
        }

        public void Delete(IGroup group)
        {
            try
            {
                this.TryDelete(group);
            }
            catch (DbUpdateException) // item already removed
            {
                this.FinishGroupRemove(group);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Delete, group, this, exception, "Unable to remove group from database.");
            }
        }

        private void TryDelete(IGroup group)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var toDelete = group as DbGroup;
                List<IFavorite> changedFavorites = group.Favorites;
                this.SetChildsToRoot(database, toDelete);
                database.Groups.Attach(toDelete);
                database.Groups.Remove(toDelete);
                database.SaveImmediatelyIfRequested();
                this.FinishGroupRemove(group);
                this.dispatcher.ReportFavoritesUpdated(changedFavorites);
            }
        }

        private void SetChildsToRoot(Database database, DbGroup group)
        {
            foreach (DbGroup child in this.CachedChilds(group))
            {
                child.Parent = null;
                database.Cache.Attach(child);
                this.TrySaveAndReport(child, database);
                this.cache.Delete(child);
            }
        }

        private List<DbGroup> CachedChilds(IGroup parent)
        {
            return this.cache.Where(candidate => parent.StoreIdEquals(candidate.Parent))
                .ToList();
        }

        private void FinishGroupRemove(IGroup group)
        {
            this.cache.Delete((DbGroup)group);
            this.dispatcher.ReportGroupsDeleted(new List<IGroup> { group });
        }

        public void Rebuild()
        {
            try
            {
                this.TryRebuild();
            }
            catch (DbUpdateException)
            {
                // merge or update is not critical simply force refresh
                this.RefreshCache();
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Rebuild, this, exception, "Unable to rebuild groups.");
            }
        }

        private void TryRebuild()
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                List<DbGroup> emptyGroups = this.DeleteEmptyGroupsFromDatabase(database);
                database.SaveImmediatelyIfRequested();
                List<IGroup> toReport = this.DeleteFromCache(emptyGroups);
                this.dispatcher.ReportGroupsDeleted(toReport);
            }
        }

        /// <summary>
        /// Call this method after the changes were committed into database, 
        /// to let the cache in last state as long as possible and ensure, that the commit didn't fail.
        /// </summary>
        private List<IGroup> DeleteFromCache(List<DbGroup> emptyGroups)
        {
            this.cache.Delete(emptyGroups);
            return emptyGroups.Cast<IGroup>().ToList();
        }

        /// <summary>
        /// Doesn't remove them from cache, only from database
        /// </summary>
        private List<DbGroup> DeleteEmptyGroupsFromDatabase(Database database)
        {
            List<DbGroup> emptyGroups = this.GetEmptyGroups();
            database.Cache.AttachAll(emptyGroups);
            database.DeleteAll(emptyGroups);
            return emptyGroups;
        }

        private List<DbGroup> GetEmptyGroups()
        {
            return this.cache.Where(group => ((IGroup)group).Favorites.Count == 0)
                .ToList();
        }

        internal void RefreshCache()
        {
            List<DbGroup> newlyLoaded = this.Load(this.Cached);
            List<DbGroup> oldGroups = this.Cached;

            List<DbGroup> missing = ListsHelper.GetMissingSourcesInTarget(newlyLoaded, oldGroups);
            List<DbGroup> redundant = ListsHelper.GetMissingSourcesInTarget(oldGroups, newlyLoaded);
            this.cache.Add(missing);
            this.cache.Delete(redundant);
            this.RefreshLoaded();

            var missingToReport = missing.Cast<IGroup>().ToList();
            var redundantToReport = redundant.Cast<IGroup>().ToList();
            this.dispatcher.ReportGroupsRecreated(missingToReport, redundantToReport);
        }

        private void RefreshLoaded()
        {
            foreach (DbGroup group in this.cache)
            {
                group.ReleaseFavoriteIds();
            }
        }

        private void CheckCache()
        {
            if (isLoaded)
                return;

            List<DbGroup> loaded = LoadFromDatabase();
            this.AssignGroupsContainer(loaded);
            this.cache.Add(loaded);
            this.isLoaded = true;
        }

        private List<DbGroup> Load(List<DbGroup> toRefresh)
        {
            List<DbGroup> loaded = LoadFromDatabase(toRefresh);
            this.AssignGroupsContainer(loaded);
            return loaded;
        }

        private void AssignGroupsContainer(List<DbGroup> groups)
        {
            foreach (DbGroup group in groups)
            {
                group.AssignStores(this, this.dispatcher, this.favorites);
            }
        }

        private List<DbGroup> LoadFromDatabase()
        {
            try
            {
                return TryLoadFromDatabase();
            }
            catch (EntityException exception)
            {
                return this.dispatcher.ReportFunctionError(LoadFromDatabase, this, exception,
                    "Unable to load groups from database");
            }
        }

        private static List<DbGroup> TryLoadFromDatabase()
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                List<DbGroup> groups = database.Groups.ToList();
                database.Cache.DetachAll(groups);
                return groups;
            }
        }

        private List<DbGroup> LoadFromDatabase(List<DbGroup> toRefresh)
        {
            try
            {
                return TryLoadFromDatabase(toRefresh);
            }
            catch (EntityException exception)
            {
                return this.dispatcher.ReportFunctionError(LoadFromDatabase, toRefresh, this, exception,
                    "Unable to refresh groups from database");
            }
        }

        private static List<DbGroup> TryLoadFromDatabase(List<DbGroup> toRefresh)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                if (toRefresh != null)
                    database.Cache.AttachAll(toRefresh);

                ((IObjectContextAdapter)database).ObjectContext.Refresh(RefreshMode.StoreWins, database.Groups);
                List<DbGroup> groups = database.Groups.Include("ParentGroup").ToList();
                database.Cache.DetachAll(groups);
                return groups;
            }
        }

        internal List<DbGroup> GetGroupsContainingFavorite(int favoriteId)
        {
            this.CheckCache();
            return this.cache.Where(candidate => candidate.ContainsFavorite(favoriteId))
                .ToList();
        }

        #region IEnumerable members

        public IEnumerator<IGroup> GetEnumerator()
        {
            this.CheckCache();
            return this.cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Groups:Cached={0}", this.cache.Count());
        }
    }
}
