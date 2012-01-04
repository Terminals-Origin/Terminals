using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data
{
    /// <summary>
    /// In previous versions Groups and Tags.
    /// Now both features are solved here.
    /// </summary>
    internal class Groups : IGroups
    {
        private DataDispatcher dispatcher;
        private FilePersistance persistance;
        private Dictionary<Guid, IGroup> cache;

        internal Groups(FilePersistance persistance, IGroup[] groups)
        {
            this.persistance = persistance;
            this.dispatcher = persistance.Dispatcher;
            this.cache = groups.ToDictionary(group => group.Id);
        }

        private bool AddToCache(IGroup group)
        {
            if (group == null || this.cache.ContainsKey(group.Id))
                return false;

            this.cache.Add(group.Id, group);
            return true;
        }

        private void Add(List<IGroup> groups)
        {
            if (groups == null)
                return;

            var added = new List<IGroup>();
            foreach (Group group in groups)
            {
                if (AddToCache(group))
                    added.Add(group);
            }

            this.dispatcher.ReportGroupsAdded(added);
        }

        private void Delete(List<IGroup> groups)
        {
            if (groups == null)
                return;

            var deleted = new List<IGroup>();
            foreach (Group group in groups)
            {
                if (DeleteFromCache(group))
                    deleted.Add(group);
            }

            this.dispatcher.ReportGroupsDeleted(deleted);
        }

        private bool DeleteFromCache(IGroup group)
        {
            if (group == null || !this.cache.ContainsKey(group.Id))
                return false;

            this.cache.Remove(group.Id);
            return true;
        }

        internal void Merge(List<IGroup> newGroups)
        {
            List<IGroup> oldGroups = this.ToList();
            List<IGroup> addedGroups = ListsHelper.GetMissingSourcesInTarget(newGroups, oldGroups);
            List<IGroup> deletedTags = ListsHelper.GetMissingSourcesInTarget(oldGroups, newGroups);
            Add(addedGroups);
            Delete(deletedTags);
        }

        private static void UpdateFavoriteGroup(List<IGroup> groups, IGroup group, IFavorite favorite)
        {
            bool newGroupsContainGroup = groups.Contains(group);
            bool containsFavorite = group.Favorites.Contains(favorite);
            if (newGroupsContainGroup && !containsFavorite)
            {
                group.AddFavorite(favorite);
            }

            if (!newGroupsContainGroup && containsFavorite)
            {
                group.RemoveFavorite(favorite);
            }
        }

        private List<IGroup> GetEmptyGroups()
        {
            return this.cache.Values
                .Where(group => group.Favorites.Count == 0)
                .ToList();
        }

        #region IGroups members

        /// <summary>
        /// Gets a group by its name searching case sensitive. Returns null, if no group is found.
        /// </summary>
        public IGroup this[string groupName]
        {
            get
            {
                return this.cache.Values.Where(group => group.Name.Equals(groupName))
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a group by its unique identifier. Returns null, if the identifier is unknown.
        /// </summary>
        public IGroup this[Guid groupId]
        {
            get
            {
                if (this.cache.ContainsKey(groupId))
                    return this.cache[groupId];

                return null;
            }
        }

        public void Add(IGroup group)
        {
            if (AddToCache(group))
            {
                this.dispatcher.ReportGroupsAdded(new List<IGroup> {group});
                this.persistance.SaveImmediatelyIfRequested();
            }
        }

        public void Delete(IGroup group)
        {
            if (DeleteFromCache(group))
            {
                this.dispatcher.ReportGroupsDeleted(new List<IGroup> {group});
                this.persistance.SaveImmediatelyIfRequested();
            }
        }

        public List<IGroup> GetGroupsContainingFavorite(Guid favoriteId)
        {
            return this.cache.Values.Where(group => group.Favorites
                    .Select(favorite => favorite.Id).Contains(favoriteId))
                .ToList();
        }

        public void UpdateFavoriteInGroups(IFavorite favorite, List<IGroup> groups)
        {
            // First create new groups, which arent in persistance yet
            this.Add(groups);

            foreach (IGroup group in this)
            {
                UpdateFavoriteGroup(groups, group, favorite);
            }

            Rebuild();
            this.dispatcher.ReportFavoriteUpdated(favorite);
            this.persistance.SaveImmediatelyIfRequested();
        }

        public void Rebuild()
        {
            List<IGroup> emptyGroups = this.GetEmptyGroups();
            foreach (Group emptyGroup in emptyGroups)
            {
                this.cache.Remove(emptyGroup.Id);
            }

            this.dispatcher.ReportGroupsDeleted(emptyGroups);
            this.persistance.SaveImmediatelyIfRequested();
        }

        #endregion

        #region IEnumerable members

        public IEnumerator<IGroup> GetEnumerator()
        {
            return this.cache.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
