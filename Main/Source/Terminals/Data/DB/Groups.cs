using System;
using System.Collections;
using System.Collections.Generic;
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

        private readonly EntitiesCache<Group> cache;

        private bool isLoaded;

        internal List<Group> Cached
        {
            get
            {
                this.CheckCache();
                return this.cache.ToList();
            }
        }

        /// <summary>
        /// Gets cached item by tis database unique identifier
        /// </summary>
        internal Group this[int id]
        {
            get
            {
                this.CheckCache();
                return this.cache.FirstOrDefault(candidate => candidate.Id == id);
            }
        }

        internal Groups()
        {
            this.cache = new EntitiesCache<Group>();
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
            using (var database = Database.CreateInstance())
            {
                Group toAdd = group as Group;
                database.Groups.AddObject(toAdd);
                database.SaveImmediatelyIfRequested();
                database.Detach(toAdd);
                this.cache.Add(toAdd);
                this.dispatcher.ReportGroupsAdded(new List<IGroup> { toAdd });
            }
        }

        internal List<IGroup> AddToDatabase(Database database, List<IGroup> groups)
        {
            // not added groups dont have an identifier obtained from database
            List<IGroup> added = groups.Where(candidate => ((Group)candidate).Id == 0).ToList();
            AddAllToDatabase(database, added);
            List<Group> toAttach = groups.Where(candidate => ((Group)candidate).Id != 0).Cast<Group>().ToList();
            database.AttachAll(toAttach);
            return added;
        }

        private static void AddAllToDatabase(Database database, List<IGroup> added)
        {
            foreach (Group group in added)
            {
                database.Groups.AddObject(group);
            }
        }

        public void Delete(IGroup group)
        {
            using (var database = Database.CreateInstance())
            {
                var toDelete = group as Group;
                database.Attach(toDelete);
                database.Groups.DeleteObject(toDelete);
                database.SaveImmediatelyIfRequested();
                this.cache.Delete(toDelete);
                this.dispatcher.ReportGroupsDeleted(new List<IGroup> { group });
            }
        }

        public void Rebuild()
        {
            using (var database = Database.CreateInstance())
            {
                List<Group> emptyGroups = this.DeleteEmptyGroupsFromDatabase(database);
                database.SaveImmediatelyIfRequested();
                List<IGroup> toReport = this.DeleteFromCache(emptyGroups);
                this.dispatcher.ReportGroupsDeleted(toReport);
            }
        }

        /// <summary>
        /// Call this method after the changes were commited into database, 
        /// to let the cache in last state as long as possible and ensure, that the commit didnt fail.
        /// </summary>
        internal List<IGroup> DeleteFromCache(List<Group> emptyGroups)
        {
            this.cache.Delete(emptyGroups);
            return emptyGroups.Cast<IGroup>().ToList();
        }

        /// <summary>
        /// Doesnt remove them from cache, only fro database
        /// </summary>
        internal List<Group> DeleteEmptyGroupsFromDatabase(Database database)
        {
            List<Group> emptyGroups = this.GetEmptyGroups();
            database.AttachAll(emptyGroups);
            DeleteFromDatabase(database, emptyGroups);
            return emptyGroups;
        }

        private void DeleteFromDatabase(Database database, IEnumerable<Group> groups)
        {
            foreach (Group group in groups)
            {
                database.Groups.DeleteObject(group);
            }
        }

        private List<Group> GetEmptyGroups()
        {
            return this.cache.Where(group => ((IGroup)group).Favorites.Count == 0)
                .ToList();
        }

        internal void RefreshCache()
        {
            List<Group> newlyLoaded = this.Load(this.Cached);
            List<Group> oldGroups = this.Cached;

            List<Group> missing = ListsHelper.GetMissingSourcesInTarget(newlyLoaded, oldGroups);
            List<Group> redundant = ListsHelper.GetMissingSourcesInTarget(oldGroups, newlyLoaded);
            this.cache.Add(missing);
            this.cache.Delete(redundant);
            this.RefreshLoaded();

            var missingToReport = missing.Cast<IGroup>().ToList();
            var redundantToReport = redundant.Cast<IGroup>().ToList();
            this.dispatcher.ReportGroupsRecreated(missingToReport, redundantToReport);
        }

        private void RefreshLoaded()
        {
            foreach (Group group in this.cache)
            {
                group.ReleaseFavoriteIds();
            }
        }

        private void CheckCache()
        {
            if (isLoaded)
                return;

            List<Group> loaded = LoadFromDatabase();
            this.AssignGroupsContainer(loaded);
            this.cache.Add(loaded);
            this.isLoaded = true;
        }

        private List<Group> Load(List<Group> toRefresh)
        {
            List<Group> loaded = LoadFromDatabase(toRefresh);
            this.AssignGroupsContainer(loaded);
            return loaded;
        }

        private void AssignGroupsContainer(List<Group> groups)
        {
            foreach (Group group in groups)
            {
                group.AssignStores(this, this.dispatcher, this.favorites);
            }
        }

        private static List<Group> LoadFromDatabase()
        {
            using (var database = Database.CreateInstance())
            {
                List<Group> groups = database.Groups.ToList();
                database.DetachAll(groups);
                return groups;
            }
        }

        private static List<Group> LoadFromDatabase(List<Group> toRefresh)
        {
            using (var database = Database.CreateInstance())
            {
                if (toRefresh != null)
                    database.AttachAll(toRefresh);

                database.Refresh(RefreshMode.StoreWins, database.Groups);
                List<Group> groups = database.Groups.ToList();
                database.DetachAll(groups);
                return groups;
            }
        }

        internal List<Group> GetGroupsContainingFavorite(int favoriteId)
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
