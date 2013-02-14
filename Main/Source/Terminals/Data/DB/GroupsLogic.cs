using System;
using System.Collections;
using System.Collections.Generic;
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

        internal List<DbGroup> Cached
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
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to add group to database " + group, exception);
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
            AddAllToDatabase(database, added);
            List<DbGroup> toAttach = groups.Where(candidate => ((DbGroup)candidate).Id != 0).Cast<DbGroup>().ToList();
            database.Cache.AttachAll(toAttach);
            return added;
        }

        private static void AddAllToDatabase(Database database, List<IGroup> added)
        {
            foreach (DbGroup group in added)
            {
                database.Groups.Add(group);
            }
        }

        public void Delete(IGroup group)
        {
            try
            {
                this.TryDelete(group);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to remove from database " + group, exception);
            }
        }

        private void TryDelete(IGroup group)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var toDelete = group as DbGroup;
                database.Groups.Attach(toDelete);
                database.Groups.Remove(toDelete);
                database.SaveImmediatelyIfRequested();
                this.cache.Delete(toDelete);
                this.dispatcher.ReportGroupsDeleted(new List<IGroup> {group});
            }
        }

        public void Rebuild()
        {
            try
            {
                this.TryRebuild();
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to rebuild groups", exception);
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
        internal List<IGroup> DeleteFromCache(List<DbGroup> emptyGroups)
        {
            this.cache.Delete(emptyGroups);
            return emptyGroups.Cast<IGroup>().ToList();
        }

        /// <summary>
        /// Doesn't remove them from cache, only fro database
        /// </summary>
        internal List<DbGroup> DeleteEmptyGroupsFromDatabase(Database database)
        {
            List<DbGroup> emptyGroups = this.GetEmptyGroups();
            database.Cache.AttachAll(emptyGroups);
            DeleteFromDatabase(database, emptyGroups);
            return emptyGroups;
        }

        private void DeleteFromDatabase(Database database, IEnumerable<DbGroup> groups)
        {
            foreach (DbGroup group in groups)
            {
                database.Groups.Remove(group);
            }
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

        private static List<DbGroup> LoadFromDatabase()
        {
            try
            {
                return TryLoadFromDatabase();
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to load groups from database", exception);
                return new List<DbGroup>();
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

        private static List<DbGroup> LoadFromDatabase(List<DbGroup> toRefresh)
        {
            try
            {
                return TryLoadFromDatabase(toRefresh);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Unable to refresh groups from database", exception);
                return new List<DbGroup>();
            }
        }

        private static List<DbGroup> TryLoadFromDatabase(List<DbGroup> toRefresh)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                if (toRefresh != null)
                    database.Cache.AttachAll(toRefresh);

                ((IObjectContextAdapter)database).ObjectContext.Refresh(RefreshMode.StoreWins, database.Groups);
                List<DbGroup> groups = database.Groups.ToList();
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
