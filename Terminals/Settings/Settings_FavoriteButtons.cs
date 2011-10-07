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
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return null;
                return (_terminalsConfigurationSection.FavoritesButtons).ReadList().ToArray();
            }
        }

        public static void AddFavoriteButton(string name)
        {
            SysConfig.Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, name);
            SaveImmediatelyIfRequested(configuration);
        }

        public static void DeleteFavoriteButton(string name)
        {
            SysConfig.Configuration configuration = Config;
            DeleteMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, name);
            SaveImmediatelyIfRequested(configuration);
        }

        public static void EditFavoriteButton(string oldName, string newName)
        {
            SysConfig.Configuration configuration = Config;
            EditMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, oldName, newName);
            SaveImmediatelyIfRequested(configuration);
        }

        public static void CreateFavoritesToolbarButtonsList(String[] names)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).FavoritesButtons.Clear();
            SaveImmediatelyIfRequested(configuration);
            foreach (string name in names)
            {
                AddFavoriteButton(name);
            }
        }
    }
}
