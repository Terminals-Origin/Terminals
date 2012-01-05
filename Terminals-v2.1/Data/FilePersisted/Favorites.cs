using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Terminals.Configuration;

namespace Terminals.Data
{
    internal class Favorites : IFavorites
    {
        private DataDispatcher dispatcher;
        private FilePersistance persistance;
        private Groups groups;
        private Dictionary<Guid, IFavorite> cache;

        internal Favorites(FilePersistance persistance, IFavorite[] favoritesSource)
        {
            this.persistance = persistance;
            this.dispatcher = persistance.Dispatcher;
            this.groups = this.persistance.GroupsStore;
            this.cache = favoritesSource.ToDictionary(favorite => favorite.Id);
        }

        private bool AddToCache(IFavorite favorite)
        {
            if (favorite == null || this.cache.ContainsKey(favorite.Id))
                return false;

            this.cache.Add(favorite.Id, favorite);
            return true;
        }

        private List<IFavorite> AddAllToCache(List<IFavorite> favorites)
        {
            var added = new List<IFavorite>();
            foreach (IFavorite favorite in favorites)
            {
                if (this.AddToCache(favorite))
                    added.Add(favorite);
            }
            return added;
        }

        private bool DeleteFromCache(IFavorite favorite)
        {
            if (IsNotCached(favorite))
                return false;

            this.cache.Remove(favorite.Id);
            return true;
        }

        private List<IFavorite> DeleteAllFavoritesFromCache(List<IFavorite> favorites)
        {
            var deleted = new List<IFavorite>();
            foreach (IFavorite favorite in favorites)
            {
                if (this.DeleteFromCache(favorite))
                    deleted.Add(favorite);
            }
            return deleted;
        }

        private bool UpdateInCache(IFavorite favorite)
        {
            if (IsNotCached(favorite))
                return false;

            this.cache[favorite.Id] = favorite;
            return true;
        }

        private bool IsNotCached(IFavorite favorite)
        {
            return favorite == null || !this.cache.ContainsKey(favorite.Id);
        }

        internal void Merge(List<IFavorite> newFavorites)
        {
            var oldFavorites = this.ToList();
            List<IFavorite> missingFavorites = ListsHelper.GetMissingSourcesInTarget(newFavorites, oldFavorites);
            List<IFavorite> redundantFavorites = ListsHelper.GetMissingSourcesInTarget(oldFavorites, newFavorites);
            Add(missingFavorites);
            Delete(redundantFavorites);
        }

        internal static SortableList<IFavorite> OrderByDefaultSorting(List<IFavorite> source)
        {
            IOrderedEnumerable<IFavorite> sorted;
            switch (Settings.DefaultSortProperty)
            {
                case SortProperties.ServerName:
                    sorted = source.OrderBy(favorite => favorite.ServerName)
                        .ThenBy(favorite => favorite.Name);
                    break;

                case SortProperties.Protocol:
                    sorted = source.OrderBy(favorite => favorite.Protocol)
                        .ThenBy(favorite => favorite.Name);
                    break;
                case SortProperties.ConnectionName:
                    sorted = source.OrderBy(favorite => favorite.Name);
                    break;
                default:
                    return new SortableList<IFavorite>(source);
            }

            return new SortableList<IFavorite>(sorted);
        }

        private void UpdateFavoriteInGroups(IFavorite favorite, List<IGroup> newGroups)
        {
            var oldGroups = this.groups.GetGroupsContainingFavorite(favorite.Id);
            List<IGroup> addedGroups = this.AddIntoMissingGroups(favorite, newGroups, oldGroups);
            List<IGroup> removedGroups = this.RemoveFromRedundantGroups(favorite, newGroups, oldGroups);
            this.dispatcher.ReportGroupsRecreated(addedGroups, removedGroups);
        }

        private List<IGroup> AddIntoMissingGroups(IFavorite favorite, List<IGroup> newGroups, List<IGroup> oldGroups)
        {
            // First create new groups, which arent in persistance yet
            var addedGroups = this.groups.Add(newGroups);
            List<IGroup> missingGroups = ListsHelper.GetMissingSourcesInTarget(newGroups, oldGroups);
            AddIntoMissingGroups(favorite, missingGroups);
            return addedGroups;
        }

        private static void AddIntoMissingGroups(IFavorite favorite, List<IGroup> missingGroups)
        {
            foreach (IGroup group in missingGroups)
            {
                group.AddFavorite(favorite);
            }
        }

        private List<IGroup> RemoveFromRedundantGroups(IFavorite favorite,
            List<IGroup> newGroups, List<IGroup> oldGroups)
        {
            List<IGroup> redundantGroups = ListsHelper.GetMissingSourcesInTarget(oldGroups, newGroups);
            Groups.RemoveFavoritesFromGroups(new List<IFavorite> { favorite }, redundantGroups);
            return groups.DeleteEmptyGroupsFromCache();
        }

        #region IFavorites members

        public IFavorite this[Guid favoriteId]
        {
            get
            {
                if (this.cache.ContainsKey(favoriteId))
                    return this.cache[favoriteId];
                return null;
            }
        }

        public IFavorite this[string favoriteName]
        {
            get
            {
                return this.cache.Values
                    .FirstOrDefault(favorite => favorite.Name
                        .Equals(favoriteName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public void Add(IFavorite favorite)
        {
            if (AddToCache(favorite))
            {
                this.dispatcher.ReportFavoriteAdded(favorite);
                this.persistance.SaveImmediatelyIfRequested();
            }
        }

        public void Add(List<IFavorite> favorites)
        {
            if (favorites == null)
                return;

            List<IFavorite> added = AddAllToCache(favorites);
            this.dispatcher.ReportFavoritesAdded(added);
            if (added.Count > 0)
                this.persistance.SaveImmediatelyIfRequested();
        }

        public void Update(IFavorite favorite)
        {
            if (!UpdateInCache(favorite))
                return;

            this.dispatcher.ReportFavoriteUpdated(favorite);
            this.persistance.SaveImmediatelyIfRequested();
        }

        public void UpdateFavorite(IFavorite favorite, List<IGroup> newGroups)
        {
            if (!UpdateInCache(favorite))
                return;

            UpdateFavoriteInGroups(favorite, newGroups);
            this.dispatcher.ReportFavoriteUpdated(favorite);
            this.persistance.SaveImmediatelyIfRequested();
        }

        public void Delete(IFavorite favorite)
        {
            if (DeleteFromCache(favorite))
            {
                var favoritesToRemove = new List<IFavorite> { favorite };
                this.groups.DeleteFavoritesFromAllGroups(favoritesToRemove);
                this.dispatcher.ReportFavoriteDeleted(favorite);
                this.persistance.SaveImmediatelyIfRequested();
            }
        }

        public void Delete(List<IFavorite> favorites)
        {
            if (favorites == null)
                return;

            List<IFavorite> deleted = DeleteAllFavoritesFromCache(favorites);
            this.groups.DeleteFavoritesFromAllGroups(deleted);
            this.dispatcher.ReportFavoritesDeleted(deleted);
            this.persistance.SaveImmediatelyIfRequested();
        }

        public SortableList<IFavorite> ToList()
        {
            return new SortableList<IFavorite>(this.cache.Values);
        }

        public SortableList<IFavorite> ToListOrderedByDefaultSorting()
        {
            var source = this.ToList();
            return OrderByDefaultSorting(source);
        }

        public void ApplyCredentialsToAllFavorites(List<IFavorite> selectedFavorites, string credentialName)
        {
            foreach (Favorite favorite in selectedFavorites)
            {
                favorite.Security.Credential = credentialName;
                this.dispatcher.ReportFavoriteUpdated(favorite);
            }

            this.persistance.SaveImmediatelyIfRequested();
        }

        public void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            foreach (Favorite favorite in selectedFavorites)
            {
                favorite.Security.Password = newPassword;
                this.dispatcher.ReportFavoriteUpdated(favorite);
            }

            this.persistance.SaveImmediatelyIfRequested();
        }

        public void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            foreach (Favorite favorite in selectedFavorites)
            {
                favorite.Security.DomainName = newDomainName;
                this.dispatcher.ReportFavoriteUpdated(favorite);
            }

            this.persistance.SaveImmediatelyIfRequested();
        }

        public void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            foreach (Favorite favorite in selectedFavorites)
            {
                favorite.Security.DomainName = newUserName;
                this.dispatcher.ReportFavoriteUpdated(favorite);
            }

            this.persistance.SaveImmediatelyIfRequested();
        }

        #endregion

        #region IEnumerable members

        public IEnumerator<IFavorite> GetEnumerator()
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
