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
        private DataBase dataBase;
        private DataDispatcher dispatcher;

        internal Groups(DataBase dataBase, DataDispatcher dispatcher)
        {
            this.dataBase = dataBase;
            this.dispatcher = dispatcher;
        }

        IGroup IGroups.this[string groupName]
        {
            get
            {
                return this.dataBase.Groups
                    .FirstOrDefault(group => group.Name
                    .Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        IGroup IGroups.this[Guid groupId]
        {
            get
            {
                return this.dataBase.Groups
                .FirstOrDefault(candidate => candidate.Guid == groupId);
            }
        }

        public void Add(IGroup group)
        {
            if (!AddToDatabase(group))
                return;
            this.dispatcher.ReportGroupsAdded(new List<IGroup> { group });
            this.dataBase.SaveImmediatelyIfRequested();
        }

        private bool AddToDatabase(IGroup group)
        {
            if (this.dataBase.Groups.ToList().Contains(group))
                return false;

            this.dataBase.Groups.AddObject((Group)group);
            return true;
        }

        internal List<IGroup> AddToDatabase(List<IGroup> groups)
        {
            var added = new List<IGroup>();
            if (groups == null)
                return added;

            foreach (Group group in groups)
            {
                if (AddToDatabase(group))
                    added.Add(group);
            }

            return added;
        }

        public void Delete(IGroup group)
        {
            DeleteFromDatabase(group);
            this.dispatcher.ReportGroupsDeleted(new List<IGroup> { group });
            this.dataBase.SaveImmediatelyIfRequested();
        }

        private void DeleteFromDatabase(IGroup group)
        {
            var candidate = group as Group;
            DeleteFromDatabase(candidate);
        }

        private void DeleteFromDatabase(Group group)
        {
            this.dataBase.Groups.DeleteObject(group);
        }

        public List<IGroup> GetGroupsContainingFavorite(Guid favoriteId)
        {
            Favorite favorite = this.dataBase.GetFavoriteByGuid(favoriteId);
            if (favorite != null)
                return favorite.GetInvariantGroups();

            return new List<IGroup>();
        }

        public void Rebuild()
        {
            DeleteEmptyGroups();
            this.dataBase.SaveImmediatelyIfRequested();
        }

        private void DeleteEmptyGroups()
        {
            var emptyGroups = this.DeleteEmptyGroupsFromDataBase();
            this.dispatcher.ReportGroupsDeleted(emptyGroups);
        }

        internal List<IGroup> DeleteEmptyGroupsFromDataBase()
        {
            List<Group> emptyGroups = this.GetEmptyGroups();
            Delete(emptyGroups);
            return emptyGroups.Cast<IGroup>().ToList();
        }

        private void Delete(List<Group> emptyGroups)
        {
            foreach (var group in emptyGroups)
            {
                DeleteFromDatabase(group);
            }
        }

        private List<Group> GetEmptyGroups()
        {
            return this.dataBase.Groups.ToList()
                .Where(group => group.Favorites.Count == 0)
                .ToList();
        }

        #region IEnumerable members

        public IEnumerator<IGroup> GetEnumerator()
        {
            return this.dataBase.Groups
                .ToList()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
