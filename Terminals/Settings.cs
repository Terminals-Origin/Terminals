using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Terminals
{
    public static class Settings
    {
        private static Configuration GetConfiguration()
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = Path.GetDirectoryName(Application.ExecutablePath) + @"\Terminals.config";
            return ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }

        private static TerminalsConfigurationSection GetSection()
        {
            Configuration configuration = GetConfiguration();
            return (TerminalsConfigurationSection)configuration.GetSection("settings");
        }

        private static TerminalsConfigurationSection GetSection(Configuration configuration)
        {
            return (TerminalsConfigurationSection)configuration.GetSection("settings");
        }

        private static void AddMRUItemConfigurationElement(MRUItemConfigurationElementCollection configurationElementCollection, string name)
        {
            MRUItemConfigurationElement configurationElement = configurationElementCollection.ItemByName(name);
            if (configurationElement == null)
            {
                configurationElementCollection.Add(new MRUItemConfigurationElement(name));
            }
        }

        private static void DeleteMRUItemConfigurationElement(MRUItemConfigurationElementCollection configurationElementCollection, string name)
        {
            MRUItemConfigurationElement configurationElement = configurationElementCollection.ItemByName(name);
            if (configurationElement != null)
            {
                configurationElementCollection.Remove(name);
            }
        }

        private static void EditMRUItemConfigurationElement(MRUItemConfigurationElementCollection configurationElementCollection, string oldName, string newName)
        {
            MRUItemConfigurationElement configurationElement = configurationElementCollection.ItemByName(oldName);
            if (configurationElement != null)
            {
                configurationElementCollection[oldName].Name = newName;
            }
        }

        public static void AddServerMRUItem(string name)
        {
            Configuration configuration = GetConfiguration();
            AddMRUItemConfigurationElement(GetSection(configuration).ServersMRU, name);
            configuration.Save();
        }

        public static void AddDomainMRUItem(string name)
        {
            Configuration configuration = GetConfiguration();
            AddMRUItemConfigurationElement(GetSection(configuration).DomainsMRU, name);
            configuration.Save();
        }

        public static void AddUserMRUItem(string name)
        {
            Configuration configuration = GetConfiguration();
            AddMRUItemConfigurationElement(GetSection(configuration).UsersMRU, name);
            configuration.Save();
        }

        public static void AddFavoriteButton(string name)
        {
            Configuration configuration = GetConfiguration();
            AddMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, name);
            configuration.Save();
        }

        public static void DeleteFavoriteButton(string name)
        {
            Configuration configuration = GetConfiguration();
            DeleteMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, name);
            configuration.Save();
        }

        public static void EditFavoriteButton(string oldName, string newName)
        {
            Configuration configuration = GetConfiguration();
            EditMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, oldName, newName);
            configuration.Save();
        }

        public static void CreateFavoritesToolbarButtonsList(string[] names)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).FavoritesButtons.Clear();
            configuration.Save();
            foreach (string name in names)
            {
                AddFavoriteButton(name);
            }
        }

        private static List<string> ReadList(MRUItemConfigurationElementCollection configurationElementCollection)
        {
            List<string> list = new List<string>();
            foreach (MRUItemConfigurationElement configurationElement in configurationElementCollection)
            {
                list.Add(configurationElement.Name);
            }
            return list;
        }

        public static string[] MRUServerNames
        {
            get
            {
                return ReadList(GetSection().ServersMRU).ToArray();
            }
        }

        public static string[] MRUDomainNames
        {
            get
            {
                return ReadList(GetSection().DomainsMRU).ToArray();
            }
        }

        public static string[] MRUUserNames
        {
            get
            {
                return ReadList(GetSection().UsersMRU).ToArray();
            }
        }

        public static string[] FavoritesToolbarButtons
        {
            get
            {
                return ReadList(GetSection().FavoritesButtons).ToArray();
            }
        }

        public static FavoriteConfigurationElementCollection GetFavorites()
        {
            return GetSection().Favorites;
        }

        public static void EditFavorite(string oldName, FavoriteConfigurationElement favorite)
        {
            Configuration configuration = GetConfiguration();
            TerminalsConfigurationSection section = GetSection(configuration);
            FavoriteConfigurationElement editedFavorite = section.Favorites[oldName];
            editedFavorite.Colors = favorite.Colors;
            editedFavorite.ConnectToConsole = favorite.ConnectToConsole;
            editedFavorite.DesktopSize = favorite.DesktopSize;
            editedFavorite.DomainName = favorite.DomainName;
            editedFavorite.EncryptedPassword = favorite.EncryptedPassword;
            editedFavorite.Name = favorite.Name;
            editedFavorite.ServerName = favorite.ServerName;
            bool shownOnToolbar = editedFavorite.ShowOnToolbar;
            editedFavorite.ShowOnToolbar = favorite.ShowOnToolbar;
            editedFavorite.UserName = favorite.UserName;
            configuration.Save();
            if (shownOnToolbar && !favorite.ShowOnToolbar)
            {
                DeleteFavoriteButton(oldName);
            }
            else if (shownOnToolbar && (oldName != favorite.Name))
            {
                EditFavoriteButton(oldName, favorite.Name);
            }
            else if (!shownOnToolbar && favorite.ShowOnToolbar)
            {
                AddFavoriteButton(favorite.Name);
            }
        }

        public static void DeleteFavorite(string name)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).Favorites.Remove(name);
            configuration.Save();
            DeleteFavoriteButton(name);
        }

        public static void AddFavorite(FavoriteConfigurationElement favorite)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).Favorites.Add(favorite);
            configuration.Save();
            if (favorite.ShowOnToolbar)
            {
                AddFavoriteButton(favorite.Name);
            }
        }

        public static GroupConfigurationElementCollection GetGroups()
        {
            return GetSection().Groups;
        }

        public static void DeleteGroup(string name)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).Groups.Remove(name);
            configuration.Save();
        }

        public static void AddGroup(GroupConfigurationElement group)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).Groups.Add(group);
            configuration.Save();
        }
    }
}
