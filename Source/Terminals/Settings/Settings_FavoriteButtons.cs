using System;
using System.Collections.Generic;
using System.Linq;
using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        internal static Guid[] FavoritesToolbarButtons
        {
            get
            {
                return GetSection().FavoritesButtons.ToList()
                    .Select(id => new Guid(id))
                    .ToArray();
            }
        }

        /// <summary>
        /// For backward compatibility with older version than 2.0 for imports.
        /// </summary>
        public static string[] FavoriteNamesToolbarButtons
        {
            get
            {
                return GetSection().FavoritesButtons.ToList().ToArray();
            }
        }

        private static MRUItemConfigurationElementCollection ButtonsCollection
        {
            get { return GetSection().FavoritesButtons; }
        }

        internal static void AddFavoriteButton(Guid favoriteId)
        {
            AddButtonToInternCollection(favoriteId);
            FireButtonsChangedEvent();
        }

        private static void AddButtonToInternCollection(Guid favoriteId)
        {
            ButtonsCollection.AddByName(favoriteId.ToString());
            SaveImmediatelyIfRequested();
        }

        internal static void UpdateFavoritesToolbarButtons(List<Guid> newFavoriteIds)
        {
            StartDelayedUpdate();
            ButtonsCollection.Clear();
            foreach (Guid favoriteId in newFavoriteIds)
            {
                AddButtonToInternCollection(favoriteId);
            }
            SaveAndFinishDelayedUpdate();
            FireButtonsChangedEvent();
        }

        internal static void EditFavoriteButton(Guid oldFavoriteId, Guid newFavoriteId, bool showOnToolbar)
        {
            DeleteFavoriteButton(oldFavoriteId);

            bool hasToolbarButton = HasToolbarButton(newFavoriteId);
            if (hasToolbarButton && !showOnToolbar)
                DeleteFavoriteButton(newFavoriteId);
            else if (showOnToolbar)
                AddFavoriteButton(newFavoriteId);
        }

        private static void DeleteFavoriteButton(Guid favoriteId)
        {
            ButtonsCollection.DeleteByName(favoriteId.ToString());
            SaveImmediatelyIfRequested();
            FireButtonsChangedEvent();
        }

        internal static bool HasToolbarButton(Guid favoriteId)
        {
            return FavoritesToolbarButtons.Contains(favoriteId);
        }

        private static void FireButtonsChangedEvent()
        {
          var args = ConfigurationChangedEventArgs.CreateFromButtons();
          FireConfigurationChanged(args);
        }
    }
}
