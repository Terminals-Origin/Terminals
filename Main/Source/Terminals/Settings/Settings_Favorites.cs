using System.Collections.Generic;
using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal partial class Settings
    {
        /// <summary>
        /// "Since version 2. only for updates. Use new persistence instead."
        /// </summary>
        internal void RemoveAllFavoritesAndTags()
        {
            DeleteFavorites();
            DeleteTags();
        }
        
        private void DeleteFavorites()
        {
            List<FavoriteConfigurationElement> favorites = GetFavorites().ToList();
            List<string> deletedTags = new List<string>();
            StartDelayedUpdate();
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                DeleteFavoriteFromSettings(favorite.Name);
                List<string> difference = DeleteTagsFromSettings(favorite.TagList);
                deletedTags.AddRange(difference);
            }

            SaveAndFinishDelayedUpdate();
        }

        private void DeleteFavoriteFromSettings(string name)
        {
            GetSection().Favorites.Remove(name);
            SaveImmediatelyIfRequested();
        }

        private void AddFavorite(FavoriteConfigurationElement favorite)
        {
            AddFavoriteToSettings(favorite);
            AddTagsToSettings(favorite.TagList);
        }

        /// <summary>
        /// Adds favorite to the database, but doesn't fire the changed event
        /// </summary>
        private void AddFavoriteToSettings(FavoriteConfigurationElement favorite)
        {
            GetSection().Favorites.Add(favorite);
            SaveImmediatelyIfRequested();
        }

        internal FavoriteConfigurationElement GetDefaultFavorite()
        {
            TerminalsConfigurationSection section = GetSection();
            if (section != null && section.DefaultFavorite.Count > 0)
                return section.DefaultFavorite[0];
            return null;
        }

        internal void SaveDefaultFavorite(FavoriteConfigurationElement favorite)
        {
            FavoriteConfigurationElementCollection defaultFav = GetSection().DefaultFavorite;
            defaultFav.Clear();
            defaultFav.Add(favorite);
            SaveImmediatelyIfRequested();
        }

        internal void RemoveDefaultFavorite()
        {
            FavoriteConfigurationElementCollection defaultFav = GetSection().DefaultFavorite;
            defaultFav.Clear();
            SaveImmediatelyIfRequested();
        }

        internal FavoriteConfigurationElementCollection GetFavorites()
        {
            TerminalsConfigurationSection section = GetSection();
            if (section != null)
                return section.Favorites;
            return null;
        }
    }
}
