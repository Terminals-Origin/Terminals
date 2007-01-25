using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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
            editedFavorite.UserName = favorite.UserName;
            editedFavorite.RedirectDrives = favorite.RedirectDrives;
            editedFavorite.RedirectPorts = favorite.RedirectPorts;
            editedFavorite.RedirectPrinters = favorite.RedirectPrinters;
            editedFavorite.RedirectDevices = favorite.RedirectDevices;
            editedFavorite.RedirectClipboard = favorite.RedirectClipboard;
            editedFavorite.RedirectSmartCards = favorite.RedirectSmartCards;
            editedFavorite.Sounds = favorite.Sounds;
            editedFavorite.Port = favorite.Port;
            editedFavorite.DesktopShare = favorite.DesktopShare;
            editedFavorite.ExecuteBeforeConnect = favorite.ExecuteBeforeConnect;
            editedFavorite.ExecuteBeforeConnectCommand = favorite.ExecuteBeforeConnectCommand;
            editedFavorite.ExecuteBeforeConnectArgs = favorite.ExecuteBeforeConnectArgs;
            editedFavorite.ExecuteBeforeConnectInitialDirectory = favorite.ExecuteBeforeConnectInitialDirectory;
            editedFavorite.ExecuteBeforeConnectWaitForExit = favorite.ExecuteBeforeConnectWaitForExit;
            configuration.Save();
        }

        public static bool HasToolbarButton(string name)
        {
            List<string> buttons = new List<string>();
            buttons.AddRange(Settings.FavoritesToolbarButtons);
            return buttons.IndexOf(name) > -1;
        }

        public static void EditFavorite(string oldName, FavoriteConfigurationElement favorite, bool showOnToolbar)
        {
            EditFavorite(oldName, favorite);
            bool shownOnToolbar = HasToolbarButton(oldName);
            if (shownOnToolbar && !showOnToolbar)
            {
                DeleteFavoriteButton(oldName);
            }
            else if (shownOnToolbar && (oldName != favorite.Name))
            {
                EditFavoriteButton(oldName, favorite.Name);
            }
            else if (!shownOnToolbar && showOnToolbar)
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

        public static void AddFavorite(FavoriteConfigurationElement favorite, bool showOnToolbar)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).Favorites.Add(favorite);
            configuration.Save();
            if (showOnToolbar)
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

        public static bool ShowUserNameInTitle
        {
            get
            {
                return GetSection().ShowUserNameInTitle;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ShowUserNameInTitle = value;
                configuration.Save();
            }
        }

        public static bool ShowInformationToolTips
        {
            get
            {
                return GetSection().ShowInformationToolTips;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ShowInformationToolTips = value;
                configuration.Save();
            }
        }

        public static bool ShowFullInformationToolTips
        {
            get
            {
                return GetSection().ShowFullInformationToolTips;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ShowFullInformationToolTips = value;
                configuration.Save();
            }
        }

        public static string DefaultDesktopShare
        {
            get
            {
                return GetSection().DefaultDesktopShare;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).DefaultDesktopShare = value;
                configuration.Save();
            }
        }

        public static bool ExecuteBeforeConnect
        {
            get
            {
                return GetSection().ExecuteBeforeConnect;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ExecuteBeforeConnect = value;
                configuration.Save();
            }
        }

        public static string ExecuteBeforeConnectCommand
        {
            get
            {
                return GetSection().ExecuteBeforeConnectCommand;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ExecuteBeforeConnectCommand = value;
                configuration.Save();
            }
        }

        public static string ExecuteBeforeConnectArgs
        {
            get
            {
                return GetSection().ExecuteBeforeConnectArgs;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ExecuteBeforeConnectArgs = value;
                configuration.Save();
            }
        }

        public static string ExecuteBeforeConnectInitialDirectory
        {
            get
            {
                return GetSection().ExecuteBeforeConnectInitialDirectory;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ExecuteBeforeConnectInitialDirectory = value;
                configuration.Save();
            }
        }

        public static bool ExecuteBeforeConnectWaitForExit
        {
            get
            {
                return GetSection().ExecuteBeforeConnectWaitForExit;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ExecuteBeforeConnectWaitForExit = value;
                configuration.Save();
            }
        }

        private static bool? _supportsRDP6;

        public static bool SupportsRDP6
        {
            get
            {
                if (!_supportsRDP6.HasValue)
                {
                    try
                    {
                        MSTSCLib.IMsRdpClient2 rdpClient = new MSTSCLib.MsRdpClient2Class();
                        _supportsRDP6 = ((rdpClient as MSTSCLib6.IMsRdpClient5) != null);
                    }
                    catch
                    {
                        _supportsRDP6 = false;
                    }
                    
                }
                return _supportsRDP6.Value;
            }
        }
    }
}
