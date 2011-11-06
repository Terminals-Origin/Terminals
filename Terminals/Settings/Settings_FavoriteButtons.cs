using System;
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
                return GetSection().FavoritesButtons.ReadList().ToArray();
            }
        }

        public static void AddFavoriteButton(string name)
        {
            AddMRUItemConfigurationElement(GetSection().FavoritesButtons, name);
            SaveImmediatelyIfRequested();
        }

        public static void DeleteFavoriteButton(string name)
        {
            DeleteMRUItemConfigurationElement(GetSection().FavoritesButtons, name);
            SaveImmediatelyIfRequested();
        }

        public static void EditFavoriteButton(string oldFavoriteName, string newFavoriteName)
        {
            EditMRUItemConfigurationElement(GetSection().FavoritesButtons, oldFavoriteName, newFavoriteName);
            SaveImmediatelyIfRequested();
        }

        public static void CreateFavoritesToolbarButtonsList(String[] names)
        {
            GetSection().FavoritesButtons.Clear();
            SaveImmediatelyIfRequested();
            foreach (string name in names)
            {
                AddFavoriteButton(name);
            }
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
    }
}
