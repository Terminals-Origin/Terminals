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

        public static FavoriteConfigurationElementCollection GetFavorites()
        {
            return GetSection().Favorites;
        }

        public static void DeleteFavorite(string name)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).Favorites.Remove(name);
            configuration.Save();
        }

        public static void AddFavorite(FavoriteConfigurationElement favorite)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).Favorites.Add(favorite);
            configuration.Save();
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
