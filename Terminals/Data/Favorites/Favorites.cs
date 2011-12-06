using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Terminals.Configuration;

namespace Terminals.Data
{
    internal class Favorites : IEnumerable<Favorite>
    {
        private DataDispatcher dispatcher;
        private Dictionary<Guid, Favorite> cache;

        internal Favorites(DataDispatcher dispatcher, Favorite[] favoritesSource)
        {
            this.dispatcher = dispatcher;
            this.cache = favoritesSource.ToDictionary(favorite => favorite.Id);
        }

        public Favorites() {}

        public Favorite this[Guid favoriteId]
        {
            get
            {
                if (this.cache.ContainsKey(favoriteId))
                    return this.cache[favoriteId];
                return null;
            }
        }

        public void Add(Favorite favorite)
        {
            if (!this.cache.ContainsKey(favorite.Id))
                this.cache.Add(favorite.Id, favorite);
        }

        #region IEnumerable members

        public IEnumerator<Favorite> GetEnumerator()
        {
            return this.cache.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Obsolete

        public FavoriteConfigurationElementCollection GetFavorites()
        {
            return Settings.GetFavorites();
        }

        internal SortableList<FavoriteConfigurationElement> GetSortedFavoritesByTag(string tag)
        {
            return Settings.GetSortedFavoritesByTag(tag);
        }

        public FavoriteConfigurationElement GetOneFavorite(string favoriteName)
        {
            return Settings.GetOneFavorite(favoriteName);
        }

        public void AddFavorite(FavoriteConfigurationElement favorite)
        {
            Settings.AddFavorite(favorite);
        }

        public void AddFavorites(List<FavoriteConfigurationElement> favorites)
        {
            Settings.AddFavorites(favorites);
        }

        public void EditFavorite(string oldName, FavoriteConfigurationElement favorite)
        {
            Settings.EditFavorite(oldName, favorite);
        }

        public void DeleteFavorite(string name)
        {
            Settings.DeleteFavorite(name);
        }

        public void DeleteFavorites(List<FavoriteConfigurationElement> favorites)
        {
           Settings.DeleteFavorites(favorites); 
        }

        public FavoriteConfigurationElement GetDefaultFavorite()
        {
            return Settings.GetDefaultFavorite();
        }

        public void SaveDefaultFavorite(FavoriteConfigurationElement favorite)
        {
            Settings.SaveDefaultFavorite(favorite);
        }

        public void RemoveDefaultFavorite()
        {
            Settings.RemoveDefaultFavorite();
        }

        public void ApplyCredentialsForAllSelectedFavorites(
            List<FavoriteConfigurationElement> selectedFavorites, string credentialName)
        {
            Settings.ApplyCredentialsForAllSelectedFavorites(selectedFavorites, credentialName);
        }

        public void SetPasswordToAllSelectedFavorites(
            List<FavoriteConfigurationElement> selectedFavorites, string newPassword)
        {
            Settings.SetPasswordToAllSelectedFavorites(selectedFavorites, newPassword);
        }

        public void ApplyDomainNameToAllSelectedFavorites(
            List<FavoriteConfigurationElement> selectedFavorites, string newDomainName)
        {
            Settings.ApplyDomainNameToAllSelectedFavorites(selectedFavorites, newDomainName);
        }

        public void ApplyUserNameToAllSelectedFavorites(
            List<FavoriteConfigurationElement> selectedFavorites, string newUserName)
        {
            Settings.ApplyUserNameToAllSelectedFavorites(selectedFavorites, newUserName);
        }

        #endregion
    }
}
