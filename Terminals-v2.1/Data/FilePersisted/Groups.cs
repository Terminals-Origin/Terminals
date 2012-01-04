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

        internal List<IGroup> Add(List<IGroup> groups)
        {
            var added = new List<IGroup>();
            if (groups == null)
                return added;
            
            foreach (Group group in groups)
            {
                if (AddToCache(group))
                    added.Add(group);
            }

            return added;
        }

        private List<IGroup> Delete(List<IGroup> groups)
        {
            var deleted = new List<IGroup>();
            if (groups == null)
                return deleted;
            
            foreach (Group group in groups)
            {
                if (DeleteFromCache(group))
                    deleted.Add(group);
            }

            return deleted;
        }

        private bool DeleteFromCache(IGroup group)
        {
            if (group == null || !this.cache.ContainsKey(group.Id))
                return false;

            this.cache.Remove(group.Id);
            return true;
        }

        internal List<IGroup> DeleteEmptyGroupsFromCache()
        {
            List<IGroup> emptyGroups = this.GetEmptyGroups();
            Delete(emptyGroups);
            return emptyGroups;
        }

        private List<IGroup> GetEmptyGroups()
        {
            return this.cache.Values
                .Where(group => group.Favorites.Count == 0)
                .ToList();
        }

        internal void Merge(List<IGroup> newGroups)
        {
            List<IGroup> oldGroups = this.ToList();
            List<IGroup> addedGroups = ListsHelper.GetMissingSourcesInTarget(newGroups, oldGroups);
            List<IGroup> deletedGroups = ListsHelper.GetMissingSourcesInTarget(oldGroups, newGroups);
            addedGroups = Add(addedGroups);
            this.dispatcher.ReportGroupsAdded(addedGroups);
            deletedGroups = Delete(deletedGroups);
            this.dispatcher.ReportGroupsDeleted(deletedGroups);
        }

        /// <summary>
        /// Deletes all favorites from all groups and removes empty groups after that.
        /// Also fires removed groups event.
        /// </summary>
        internal void DeleteFavoritesFromAllGroups(List<IFavorite> favoritesToRemove)
        {
            if (favoritesToRemove == null)
                return;

            RemoveFavoritesFromGroups(favoritesToRemove, this);
            DeleteEmptyGroups();
        }

        internal static void RemoveFavoritesFromGroups(List<IFavorite> favoritesToRemove, IEnumerable<IGroup> groups)
        {
            foreach (IGroup group in groups)
            {
                group.RemoveFavorites(favoritesToRemove);
            }
        }

        private void DeleteEmptyGroups()
        {
            var emptyGroups = this.DeleteEmptyGroupsFromCache();
            this.dispatcher.ReportGroupsDeleted(emptyGroups);
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

        public void Rebuild()
        {
            DeleteEmptyGroups();
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
