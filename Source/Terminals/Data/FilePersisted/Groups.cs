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
    internal class Groups : IGroups, IFavoriteGroups
    {
        private readonly DataDispatcher dispatcher;
        private readonly FilePersistence persistence;
        private readonly Dictionary<Guid, IGroup> cache;

        internal Groups(FilePersistence persistence)
        {
            this.persistence = persistence;
            this.dispatcher = persistence.Dispatcher;
            this.cache = new Dictionary<Guid,IGroup>();
        }

        private bool AddToCache(Group group)
        {
            if (group == null || this.cache.ContainsKey(group.Id))
                return false;

            this.cache.Add(group.Id, group);
            return true;
        }

        internal List<IGroup> AddAllToCache(IEnumerable<IGroup> groups)
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

        private List<IGroup> DeleteFromCache(List<IGroup> groups)
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

        private bool DeleteFromCache(Group group)
        {
            if (this.IsNotCached(group))
                return false;

            this.cache.Remove(group.Id);
            return true;
        }

        private List<IGroup> GetEmptyGroups()
        {
            return this.cache.Values
                .Where(group => group.Favorites.Count == 0)
                .ToList();
        }

        internal List<IGroup> Merge(List<IGroup> newGroups)
        {
            List<IGroup> oldGroups = this.ToList();
            List<IGroup> addedGroups = ListsHelper.GetMissingSourcesInTarget(newGroups, oldGroups);
            List<IGroup> deletedGroups = ListsHelper.GetMissingSourcesInTarget(oldGroups, newGroups);
            addedGroups = this.AddAllToCache(addedGroups);
            this.dispatcher.ReportGroupsAdded(addedGroups);
            deletedGroups = this.DeleteFromCache(deletedGroups);
            this.dispatcher.ReportGroupsDeleted(deletedGroups);
            return addedGroups;
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
        }

        internal static void RemoveFavoritesFromGroups(List<IFavorite> favoritesToRemove, IEnumerable<IGroup> groups)
        {
            foreach (IGroup group in groups)
            {
                group.RemoveFavorites(favoritesToRemove);
            }
        }

        #region IGroups members

        /// <summary>
        /// Gets group by its name. If there are more than one with this name returns the first found.
        /// If there is no group with such name, returns null. Search isn't case sensitive.
        /// Use this only to identify, if group with required name isn't already present,
        /// to prevent name duplicities.
        /// </summary>
        public IGroup this[string groupName]
        {
            get
            {
                return this.cache.Values
                    .FirstOrDefault(group => group.Name
                        .Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        /// <summary>
        /// Gets a group by its unique identifier. Returns null, if the identifier is unknown.
        /// </summary>
        internal IGroup this[Guid groupId]
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
            if (AddToCache(group as Group))
            {
                this.dispatcher.ReportGroupsAdded(new List<IGroup> {group});
                this.persistence.SaveImmediatelyIfRequested();
            }
        }

        public void Update(IGroup group)
        {
            if (this.UpdateInCache(group as Group))
            {
                this.dispatcher.ReportGroupsUpdated(new List<IGroup> {group});
                this.persistence.SaveImmediatelyIfRequested();
            }
        }

        private bool UpdateInCache(Group group)
        {
            if (this.IsNotCached(group))
                return false;

            this.cache[group.Id] = group;
            return true;
        }

        private bool IsNotCached(Group group)
        {
            return group == null || !this.cache.ContainsKey(group.Id);
        }

        public void Delete(IGroup group)
        {
            var toRemove = group as Group;
            if (DeleteFromCache(toRemove))
            {
                List<IFavorite> changedFavorites = group.Favorites;
                this.RemoveChildGroupsParent(toRemove);
                this.dispatcher.ReportGroupsDeleted(new List<IGroup> {group});
                this.dispatcher.ReportFavoritesUpdated(changedFavorites);
                this.persistence.SaveImmediatelyIfRequested();
            }
        }

        private void RemoveChildGroupsParent(Group group)
        {
            List<IGroup> childs = this.GetChildGroups(group);
            SetParentToRoot(childs);
            this.dispatcher.ReportGroupsUpdated(childs);
        }

        private static void SetParentToRoot(List<IGroup> childs)
        {
            foreach (IGroup child in childs)
            {
                child.Parent = null;
            }
        }

        private List<IGroup> GetChildGroups(Group group)
        {
            // Search by id, because the parent already cant be obtained from cache
            return this.cache.Values.Where(candidate => group.Id == ((Group)candidate).Parent)
                .ToList();
        }

        public List<IGroup> GetGroupsContainingFavorite(Guid favoriteId)
        {
            return this.cache.Values.Where(group => group.Favorites
                    .Select(favorite => favorite.Id).Contains(favoriteId))
                .ToList();
        }

        public void Rebuild()
        {
            List<IGroup> emptyGroups = this.GetEmptyGroups();
            this.DeleteFromCache(emptyGroups);
            this.dispatcher.ReportGroupsDeleted(emptyGroups);
            this.persistence.SaveImmediatelyIfRequested();
        }

        public void AddFavorite(IGroup toUpdate, IFavorite favorite)
        {
            this.AddFavorites(toUpdate, new List<IFavorite>() { favorite });
        }

        public void AddFavorites(IGroup toUpdate, List<IFavorite> favorites)
        {
            var group = toUpdate as Group;
            group.AddFavoritesToCache(favorites);
            this.ReportGroupChanged(toUpdate);
            this.persistence.SaveImmediatelyIfRequested();
        }

        private void ReportGroupChanged(IGroup group)
        {
            this.dispatcher.ReportGroupsUpdated(new List<IGroup> { group });
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
