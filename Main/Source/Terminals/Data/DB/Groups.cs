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

        internal Groups(DataDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        IGroup IGroups.this[string groupName]
        {
            get
            {
                using (var database = Database.CreateInstance())
                {
                    return database.Groups
                      .FirstOrDefault(group => group.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
                }
            }
        }

        public void Add(IGroup group)
        {
            using (var database = Database.CreateInstance())
            {
                database.Groups.AddObject((Group)group);
                database.SaveImmediatelyIfRequested();
                database.Detach(group);
                this.dispatcher.ReportGroupsAdded(new List<IGroup> { group });
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
                this.dispatcher.ReportGroupsDeleted(new List<IGroup> { group });
            }
        }

        public List<IGroup> GetGroupsContainingFavorite(Guid favoriteId)
        {
            using (var database = Database.CreateInstance())
            {
                Favorite favorite = database.GetFavoriteByGuid(favoriteId);
                if (favorite != null)
                    return favorite.GetInvariantGroups();

                return new List<IGroup>();
            }
        }

        public void Rebuild()
        {
            using (var database = Database.CreateInstance())
            {
                List<IGroup> emptyGroups = this.DeleteEmptyGroupsFromDatabase(database);
                database.SaveImmediatelyIfRequested();
                this.dispatcher.ReportGroupsDeleted(emptyGroups);
            }
        }

        internal List<IGroup> DeleteEmptyGroupsFromDatabase(Database database)
        {
            List<Group> emptyGroups = this.GetEmptyGroups(database);
            database.AttachAll(emptyGroups);
            DeleteFromDatabase(database, emptyGroups);
            return emptyGroups.Cast<IGroup>().ToList();
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
            return database.Groups.ToList()
                .Where(group => group.Favorites.Count == 0).ToList();
        }

        #region IEnumerable members

        public IEnumerator<IGroup> GetEnumerator()
        {
            using (var database = Database.CreateInstance())
            {
                var groups = database.Groups.ToList();
                database.DetachAll(groups);
                return groups.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
