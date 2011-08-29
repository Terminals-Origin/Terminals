using System;
using System.Collections.Generic;

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

        private DataDispatcher() { }

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
            System.Diagnostics.Debug.WriteLine(args.ToString());
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
            System.Diagnostics.Debug.WriteLine(args.ToString());
            if (this.TagsChanged != null && !args.IsEmpty)
            {
                this.TagsChanged(args);
            }
        }
    }
}
