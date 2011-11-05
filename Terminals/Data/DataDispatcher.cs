using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Terminals.Configuration;
using Terminals.History;

namespace Terminals.Data
{
    /// <summary>
    /// Infroms about changes in Tags collection
    /// </summary>
    /// <param name="args">Not null container reporting removed and added Tags</param>
    internal delegate void TagsChangedEventHandler(TagsChangedArgs args);

    /// <summary>
    /// Informs about changes in favorites collection.
    /// </summary>
    /// <param name="args">Not null container, reporting Added, removed, and updated favorites</param>
    internal delegate void FavoritesChangedEventHandler(FavoritesChangedEventArgs args);

    /// <summary>
    /// Central point, which distributes informations about changes in Tags and Favorites collections
    /// </summary>
    internal sealed class DataDispatcher
    {
        #region Thread safe singleton with lazy loading

        private DataDispatcher()
        {
            Settings.ConfigFileReloaded += new ConfigFileReloadedHandler(OnConfigFileReloaded);
        }

        /// <summary>
        /// Gets the thread safe singleton instance of the dispatcher
        /// </summary>
        public static DataDispatcher Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private static class Nested
        {
            internal static readonly DataDispatcher instance = new DataDispatcher();
        }

        #endregion

        internal event TagsChangedEventHandler TagsChanged;
        internal event FavoritesChangedEventHandler FavoritesChanged;

        /// <summary>
        /// Because filewatcher is created before the main form in GUI thread.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        internal static void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            Settings.AssignSynchronizationObject(synchronizer);
            ConnectionHistory.Instance.AssignSynchronizationObject(synchronizer);
            StoredCredentials.Instance.AssignSynchronizationObject(synchronizer);
        }

        private void OnConfigFileReloaded(ConfigFileChangedEventArgs args)
        {
            MergeTags(args);
            MergeFavorites(args);
            // 3. Report settings change
        }

        private void MergeTags(ConfigFileChangedEventArgs args)
        {
            List<string> oldTags = args.Old.Tags.ReadList();
            List<string> newTags = args.New.Tags.ReadList();
            List<string> deletedTags = ListStringHelper.GetMissingSourcesInTarget(oldTags, newTags);
            List<string> addedTags = ListStringHelper.GetMissingSourcesInTarget(newTags, oldTags);
            var tagsArgs = new TagsChangedArgs(addedTags, deletedTags);
            FireTagsChanged(tagsArgs);
        }

        private void MergeFavorites(ConfigFileChangedEventArgs args)
        {
            var oldFavorites = args.Old.Favorites.ToList();
            var newFavorites = args.New.Favorites.ToList();
            List<FavoriteConfigurationElement> missingFavorites = GetMissingFavorites(newFavorites, oldFavorites);
            List<FavoriteConfigurationElement> redundantFavorites = GetMissingFavorites(oldFavorites, newFavorites);

            var favoriteArgs = new FavoritesChangedEventArgs();
            favoriteArgs.Added.AddRange(missingFavorites);
            favoriteArgs.Removed.AddRange(redundantFavorites);
            FireFavoriteChanges(favoriteArgs);
        }

        internal static List<FavoriteConfigurationElement> GetMissingFavorites(
            List<FavoriteConfigurationElement> newFavorites,
            List<FavoriteConfigurationElement> oldFavorites)
        {
            return newFavorites.Where(
                newFavorite => oldFavorites.FirstOrDefault(oldFavorite => oldFavorite.Name == newFavorite.Name) == null)
                .ToList();
        }

        internal void ReportFavoriteAdded(FavoriteConfigurationElement addedFavorite)
        {
            var args = new FavoritesChangedEventArgs();
            args.Added.Add(addedFavorite);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoritesAdded(List<FavoriteConfigurationElement> addedFavorites)
        {
            var args = new FavoritesChangedEventArgs();
            args.Added.AddRange(addedFavorites);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoriteUpdated(string oldName, FavoriteConfigurationElement changedFavorite)
        {
            var args = new FavoritesChangedEventArgs();
            args.Updated.Add(oldName, changedFavorite);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoriteDeleted(FavoriteConfigurationElement deletedFavorite)
        {
            var args = new FavoritesChangedEventArgs();
            args.Removed.Add(deletedFavorite);
            FireFavoriteChanges(args);
        }

        internal void ReportFavoritesDeleted(List<FavoriteConfigurationElement> deletedFavorites)
        {
            var args = new FavoritesChangedEventArgs();
            args.Removed.AddRange(deletedFavorites);
            FireFavoriteChanges(args);
        }

        private void FireFavoriteChanges(FavoritesChangedEventArgs args)
        {
            Debug.WriteLine(args.ToString());
            if (this.FavoritesChanged != null && !args.IsEmpty)
            {
                this.FavoritesChanged(args);
            }
        }

        internal void ReportTagsAdded(List<String> addedsTag)
        {
            var args = new TagsChangedArgs();
            args.Added.AddRange(addedsTag);
            FireTagsChanged(args);
        }

        internal void ReportTagsDeleted(List<String> deletedTags)
        {
            var args = new TagsChangedArgs();
            args.Removed.AddRange(deletedTags);
            FireTagsChanged(args);
        }

        internal void ReportTagsRecreated(List<String> addedTags, List<String> deletedTags)
        {
            var args = new TagsChangedArgs(addedTags, deletedTags);
            FireTagsChanged(args);
        }

        private void FireTagsChanged(TagsChangedArgs args)
        {
            Debug.WriteLine(args.ToString());
            if (this.TagsChanged != null && !args.IsEmpty)
            {
                this.TagsChanged(args);
            }
        }
    }
}
