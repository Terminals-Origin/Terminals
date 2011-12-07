using System;
using System.Collections.Generic;
using SysConfig = System.Configuration;
using System.Linq;
using Terminals.Data;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        /// <summary>
        /// Gets the default tag name for favorites without any tag
        /// </summary>
        internal const String UNTAGGED_NODENAME = "Untagged";

        [Obsolete("Use new persistance instead.")]
        public static void EditFavorite(string oldName, FavoriteConfigurationElement favorite)
        {
            if (favorite == null)
                return;

            StartDelayedUpdate();
            EditFavoriteInSettings(favorite, oldName);

            SaveAndFinishDelayedUpdate();
            DataDispatcher.Instance.ReportFavoriteUpdated(oldName, favorite);
        }

        [Obsolete("Use new persistance instead.")]
        internal static void ApplyCredentialsForAllSelectedFavorites(
            List<FavoriteConfigurationElement> selectedFavorites, string credentialName)
        {
            StartDelayedUpdate();
            foreach (var favorite in selectedFavorites)
            {
                favorite.Credential = credentialName;
                UpdateFavorite(favorite.Name, favorite);
            }
            SaveAndFinishDelayedUpdate();
        }

        [Obsolete("Use new persistance instead.")]
        internal static void SetPasswordToAllSelectedFavorites(
            List<FavoriteConfigurationElement> selectedFavorites, string newPassword)
        {
            StartDelayedUpdate();
            foreach (FavoriteConfigurationElement favorite in selectedFavorites)
            {
                favorite.Password = newPassword;
                UpdateFavorite(favorite.Name, favorite);
            }
            SaveAndFinishDelayedUpdate();
        }

        [Obsolete("Use new persistance instead.")]
        internal static void ApplyDomainNameToAllSelectedFavorites(
            List<FavoriteConfigurationElement> selectedFavorites, string newDomainName)
        {
            StartDelayedUpdate();
            foreach (FavoriteConfigurationElement favorite in selectedFavorites)
            {
                favorite.DomainName = newDomainName;
                UpdateFavorite(favorite.Name, favorite);
            }
            SaveAndFinishDelayedUpdate();
        }

        [Obsolete("Use new persistance instead.")]
        internal static void ApplyUserNameToAllSelectedFavorites(
          List<FavoriteConfigurationElement> selectedFavorites, string newUserName)
        {
            StartDelayedUpdate();
            foreach (FavoriteConfigurationElement favorite in selectedFavorites)
            {
                favorite.UserName = newUserName;
                UpdateFavorite(favorite.Name, favorite);
            }
            SaveAndFinishDelayedUpdate();
        }

        /// <summary>
        /// Replaces the favorite stored with <paramref name="oldName"/> by copy of the <paramref name="favorite"/>
        /// </summary>
        /// <param name="oldName">favorite.Name value, before the favorite was changed</param>
        /// <param name="favorite">Not null updated connection favorite</param>
        private static void UpdateFavorite(string oldName, FavoriteConfigurationElement favorite)
        {
            EditFavoriteInSettings(favorite, oldName);
            DataDispatcher.Instance.ReportFavoriteUpdated(oldName, favorite);
        }

        private static void EditFavoriteInSettings(FavoriteConfigurationElement favorite, string oldName)
        {
            TerminalsConfigurationSection section = GetSection();
            section.Favorites[oldName] = favorite.Clone() as FavoriteConfigurationElement;
            SaveImmediatelyIfRequested();
        }

        [Obsolete("Use new persistance instead.")]
        public static void DeleteFavorite(string name)
        {
            FavoriteConfigurationElement favoriteToDelete = GetOneFavorite(name);
            DeleteFavoriteFromSettings(name);
            List<String> deletedTags = DeleteTagsFromSettings(favoriteToDelete.TagList);
            DataDispatcher.Instance.ReportTagsDeleted(deletedTags);
            DataDispatcher.Instance.ReportFavoriteDeleted(favoriteToDelete);
        }

        private static void DeleteFavoriteFromSettings(string name)
        {
            GetSection().Favorites.Remove(name);
            SaveImmediatelyIfRequested();
            DeleteFavoriteButton(name);
        }

        [Obsolete("Use new persistance instead.")]
        internal static void DeleteFavorites(List<FavoriteConfigurationElement> favorites)
        {
            if (favorites == null || favorites.Count == 0)
                return;

            List<String> deletedTags = new List<string>();
            StartDelayedUpdate();
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                DeleteFavoriteFromSettings(favorite.Name);
                List<String> difference = DeleteTagsFromSettings(favorite.TagList);
                deletedTags.AddRange(difference);
            }

            SaveAndFinishDelayedUpdate();
            DataDispatcher.Instance.ReportTagsDeleted(deletedTags);
            DataDispatcher.Instance.ReportFavoritesDeleted(favorites);
        }

        [Obsolete("Use new persistance instead.")]
        public static void AddFavorite(FavoriteConfigurationElement favorite)
        {
            AddFavoriteToSettings(favorite);
            List<String> addedTags = AddTagsToSettings(favorite.TagList);
            DataDispatcher.Instance.ReportTagsAdded(addedTags);
            DataDispatcher.Instance.ReportFavoriteAdded(favorite);
        }

        /// <summary>
        /// Adds favorite to the database, but doesnt fire the changed event
        /// </summary>
        private static void AddFavoriteToSettings(FavoriteConfigurationElement favorite)
        {
            GetSection().Favorites.Add(favorite);
            SaveImmediatelyIfRequested();
        }

        /// <summary>
        /// Adds all favorites as new in the configuration as a batch and saves configuration after all are imported.
        /// </summary>
        /// <param name="favorites">Not null collection of favorites to import</param>
        [Obsolete("Use new persistance instead.")]
        internal static void AddFavorites(List<FavoriteConfigurationElement> favorites)
        {
            if (favorites == null || favorites.Count == 0)
                return;

            StartDelayedUpdate();
            List<String> tagsToAdd = new List<string>();
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                AddFavoriteToSettings(favorite);
                List<String> difference = ListStringHelper.GetMissingSourcesInTarget(favorite.TagList, tagsToAdd);
                tagsToAdd.AddRange(difference);
            }

            List<String> addedTags = AddTagsToSettings(tagsToAdd);
            SaveAndFinishDelayedUpdate();

            DataDispatcher.Instance.ReportTagsAdded(addedTags);
            DataDispatcher.Instance.ReportFavoritesAdded(favorites);
        }

        [Obsolete("Use new persistance instead.")]
        public static FavoriteConfigurationElement GetDefaultFavorite()
        {
            TerminalsConfigurationSection section = GetSection();
            if (section != null && section.DefaultFavorite.Count > 0)
                return section.DefaultFavorite[0];
            return null;
        }

        [Obsolete("Use new persistance instead.")]
        public static void SaveDefaultFavorite(FavoriteConfigurationElement favorite)
        {
            FavoriteConfigurationElementCollection defaultFav = GetSection().DefaultFavorite;
            defaultFav.Clear();
            defaultFav.Add(favorite);
            SaveImmediatelyIfRequested();
        }

        [Obsolete("Use new persistance instead.")]
        public static void RemoveDefaultFavorite()
        {
            FavoriteConfigurationElementCollection defaultFav = GetSection().DefaultFavorite;
            defaultFav.Clear();
            SaveImmediatelyIfRequested();
        }

        [Obsolete("Use new persistance instead.")]
        public static FavoriteConfigurationElementCollection GetFavorites()
        {
            TerminalsConfigurationSection section = GetSection();
            if (section != null)
                return section.Favorites;
            return null;
        }

        /// <summary>
        /// Gets all favorites, which contain required tag in not sorted collection.
        /// If, the tag is empty, than returns "Untagged" favorites.
        /// </summary>
        private static List<FavoriteConfigurationElement> GetFavoritesByTag(String tag)
        {
            if (String.IsNullOrEmpty(tag) || tag == UNTAGGED_NODENAME)
            {
                return GetFavorites().ToList()
                    .Where(favorite => String.IsNullOrEmpty(favorite.Tags))
                    .ToList();
            }

            return GetFavorites().ToList()
                .Where(favorite => favorite.TagList.Contains(tag, StringComparer.CurrentCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets all favorites, which contain required tag in collection sorted by default sort property.
        /// If, the tag is empty, than returns "Untagged" favorites.
        /// </summary>
        [Obsolete("Use new persistance instead.")]
        internal static SortableList<FavoriteConfigurationElement> GetSortedFavoritesByTag(string  tag)
        {
            var tagFavorites = GetFavoritesByTag(tag);
            return FavoriteConfigurationElementCollection.OrderByDefaultSorting(tagFavorites);
        }

        [Obsolete("Use new persistance instead.")]
        public static FavoriteConfigurationElement GetOneFavorite(string connectionName)
        {
            return GetFavorites()[connectionName];
        }

        private static void UpdateFavoritePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            foreach (FavoriteConfigurationElement favorite in GetFavorites())
            {
                favorite.UpdatePasswordsByNewKeyMaterial(newKeyMaterial);
            }
        }
    }
}
