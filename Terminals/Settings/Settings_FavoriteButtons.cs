using System;
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

        public static void EditFavoriteButton(string oldName, string newName)
        {
            EditMRUItemConfigurationElement(GetSection().FavoritesButtons, oldName, newName);
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
    }
}
