using System.Collections.Generic;
using System.Linq;
using SysConfig = System.Configuration;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        public static string[] FavoritesToolbarButtons
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

        public static void AddFavoriteButton(string name)
        {
            List<string> oldButtons = ButtonsCollection.ToList();
            AddButtonToInternCollection(name);
            FireButtonsChangedEvent(oldButtons);
        }

        private static void AddButtonToInternCollection(string name)
        {
            ButtonsCollection.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static void DeleteFavoriteButton(string name)
        {
            List<string> oldButtons = ButtonsCollection.ToList();
            ButtonsCollection.DeleteByName(name);
            SaveImmediatelyIfRequested();
            FireButtonsChangedEvent(oldButtons);
        }

        public static void EditFavoriteButton(string oldFavoriteName, string newFavoriteName)
        {
            List<string> oldButtons = ButtonsCollection.ToList();
            ButtonsCollection.EditByName(oldFavoriteName, newFavoriteName);
            SaveImmediatelyIfRequested();
            FireButtonsChangedEvent(oldButtons);
        }

        public static void UpdateFavoritesToolbarButtons(List<string> newFavoriteNames)
        {
            StartDelayedUpdate();
            List<string> oldButtons = ButtonsCollection.ToList();
            ButtonsCollection.Clear();
            foreach (string favoriteName in newFavoriteNames)
            {
                AddButtonToInternCollection(favoriteName);
            }
            SaveAndFinishDelayedUpdate();
            FireButtonsChangedEvent(oldButtons);
        }

        internal static void EditFavoriteButton(string oldFavoriteName, string newFavoriteName, bool showOnToolbar)
        {
            bool shownOnToolbar = HasToolbarButton(oldFavoriteName);
            if (shownOnToolbar && !showOnToolbar)
                DeleteFavoriteButton(oldFavoriteName);
            else if (shownOnToolbar && (oldFavoriteName != newFavoriteName))
                EditFavoriteButton(oldFavoriteName, newFavoriteName);
            else if (!shownOnToolbar && showOnToolbar)
                AddFavoriteButton(newFavoriteName);
        }

        internal static bool HasToolbarButton(string favoriteName)
        {
            return FavoritesToolbarButtons.Contains(favoriteName);
        }

        private static void FireButtonsChangedEvent(List<string> oldButtons)
        {
          List<string> newButtons = ButtonsCollection.ToList(); 
          var args = ConfigurationChangedEventArgs.CreateFromButtons(oldButtons, newButtons);
          FireConfigurationChanged(args);
        }
    }
}
