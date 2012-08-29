using System;
using System.Collections.Generic;
using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        /// <summary>
        /// Gets the default tag name for favorites without any tag
        /// </summary>
        internal const String UNTAGGED_NODENAME = "Untagged";

        [Obsolete("Since version 2. only for updates. Use new persistence instead.")]
        internal static void RemoveAllFavoritesAndTags()
        {
            DeleteFavorites();
            DeleteTags();
        }
        
        private static void DeleteFavorites()
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

        private static void DeleteFavoriteFromSettings(string name)
        {
            GetSection().Favorites.Remove(name);
            SaveImmediatelyIfRequested();
        }

        private static void AddFavorite(FavoriteConfigurationElement favorite)
        {
            AddFavoriteToSettings(favorite);
            AddTagsToSettings(favorite.TagList);
        }

        /// <summary>
        /// Adds favorite to the database, but doesnt fire the changed event
        /// </summary>
        private static void AddFavoriteToSettings(FavoriteConfigurationElement favorite)
        {
            GetSection().Favorites.Add(favorite);
            SaveImmediatelyIfRequested();
        }

        [Obsolete("Since version 2. only for updates. Use new persistence instead.")]
        internal static FavoriteConfigurationElement GetDefaultFavorite()
        {
            TerminalsConfigurationSection section = GetSection();
            if (section != null && section.DefaultFavorite.Count > 0)
                return section.DefaultFavorite[0];
            return null;
        }

        [Obsolete("Since version 2. only for updates. Use new persistence instead.")]
        internal static void SaveDefaultFavorite(FavoriteConfigurationElement favorite)
        {
            FavoriteConfigurationElementCollection defaultFav = GetSection().DefaultFavorite;
            defaultFav.Clear();
            defaultFav.Add(favorite);
            SaveImmediatelyIfRequested();
        }

        [Obsolete("Since version 2. only for updates. Use new persistence instead.")]
        internal static void RemoveDefaultFavorite()
        {
            FavoriteConfigurationElementCollection defaultFav = GetSection().DefaultFavorite;
            defaultFav.Clear();
            SaveImmediatelyIfRequested();
        }

        internal static FavoriteConfigurationElementCollection GetFavorites()
        {
            TerminalsConfigurationSection section = GetSection();
            if (section != null)
                return section.Favorites;
            return null;
        }
    }
}
