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
        private Dictionary<Guid, IFavorite> cache;

        internal Favorites(FilePersistance persistance, IFavorite[] favoritesSource)
        {
            this.persistance = persistance;
            this.dispatcher = persistance.Dispatcher;
            this.cache = favoritesSource.ToDictionary(favorite => favorite.Id);
        }

        private bool AddToCache(IFavorite favorite)
        {
            if (favorite == null || this.cache.ContainsKey(favorite.Id))
                return false;

            this.cache.Add(favorite.Id, favorite);
            return true;
        }

        private bool DeleteFromCache(IFavorite favorite)
        {
            if (favorite == null || !this.cache.ContainsKey(favorite.Id))
                return false;

            this.cache.Remove(favorite.Id);
            return true;
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
                   .FirstOrDefault(favorite => favorite.Name == favoriteName);
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

            var added = new List<IFavorite>();
            foreach (IFavorite favorite in favorites)
            {
                if (AddToCache(favorite))
                    added.Add(favorite);
            }

            this.dispatcher.ReportFavoritesAdded(added);
            if (added.Count > 0)
                this.persistance.SaveImmediatelyIfRequested();
        }

        public void Update(IFavorite favorite)
        {
            if (favorite == null || !this.cache.ContainsKey(favorite.Id))
                return;

            this.cache[favorite.Id] = favorite;
            this.dispatcher.ReportFavoriteUpdated(favorite);
            this.persistance.SaveImmediatelyIfRequested();
        }

        public void Delete(IFavorite favorite)
        {
            if (DeleteFromCache(favorite))
            {
                this.dispatcher.ReportFavoriteDeleted(favorite);
                this.persistance.SaveImmediatelyIfRequested();
            }
        }

        public void Delete(List<IFavorite> favorites)
        {
            if (favorites == null)
                return;

            var deleted = new List<IFavorite>();
            foreach (IFavorite favorite in favorites)
            {
                if (DeleteFromCache(favorite))
                    deleted.Add(favorite);
            }

            this.dispatcher.ReportFavoritesDeleted(deleted);
            if(deleted.Count > 0)
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
