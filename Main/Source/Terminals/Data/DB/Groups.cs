using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL persisted groups container
    /// </summary>
    internal class Groups : IGroups
    {
        private readonly DataDispatcher dispatcher;

        private readonly EntitiesCache<Group> cache;

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

        internal Groups(DataDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.cache = new EntitiesCache<Group>();
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
            List<Group> emptyGroups = this.GetEmptyGroups(database);
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

        private List<Group> GetEmptyGroups(Database database)
        {
            // not optimized access to database. Weeknes: Current state doesnt have to reflect the cache
            List<Group> loaded = database.Groups.ToList()
                .Where(group => group.Favorites.Count == 0).ToList();
            this.AssignGroupsContainer(loaded);
            return loaded;
        }

        private void CheckCache()
        {
            if (!this.cache.IsEmpty)
                return;
            
            List<Group> loaded = LoadFromDatabase();
            this.AssignGroupsContainer(loaded);
            this.cache.Add(loaded);
        }

        private void AssignGroupsContainer(List<Group> groups)
        {
            foreach (Group group in groups)
            {
                group.AssignStores(this, this.dispatcher);
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
