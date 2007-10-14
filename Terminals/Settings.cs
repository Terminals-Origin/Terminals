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
            string configFile = Terminals.MainForm.ConfigurationFileLocation;
            if(!System.IO.File.Exists(configFile)) {
                string templateConfigFile = global::Terminals.Properties.Resources.Terminals;
                using(System.IO.StreamWriter sr = new StreamWriter(configFile)) {
                    sr.Write(templateConfigFile);
                }
            }
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = null;
            try {
                config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            } catch (Exception exc) {
                if (System.IO.File.Exists(configFile)) System.IO.File.Delete(configFile);
                string templateConfigFile = global::Terminals.Properties.Resources.Terminals;
                using (System.IO.StreamWriter sr = new StreamWriter(configFile)) {
                    sr.Write(templateConfigFile);
                }
                config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            }
            return config;
        }

        private static TerminalsConfigurationSection GetSection()
        {
            Configuration configuration = GetConfiguration();
            TerminalsConfigurationSection c=null;
            try {
                c = (TerminalsConfigurationSection)configuration.GetSection("settings");
            } catch (Exception exc) {
                if (System.IO.File.Exists(configuration.FilePath)) System.IO.File.Delete(configuration.FilePath);
                configuration = GetConfiguration();
                c = (TerminalsConfigurationSection)configuration.GetSection("settings");
            }
            return c;
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

        public static void AddTag(string tag)
        {
            Configuration configuration = GetConfiguration();
            AddMRUItemConfigurationElement(GetSection(configuration).Tags, tag);
            configuration.Save();
        }

        public static void DeleteTag(string tag)
        {
            Configuration configuration = GetConfiguration();
            DeleteMRUItemConfigurationElement(GetSection(configuration).Tags, tag);
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

        public static void AddConnection(string name)
        {
            Configuration configuration = GetConfiguration();
            AddMRUItemConfigurationElement(GetSection(configuration).SavedConnections, name);
            configuration.Save();
        }

        public static SpecialCommandConfigurationElementCollection SpecialCommands
        {
            get
            {
                Configuration configuration = GetConfiguration();
                return GetSection(configuration).SpecialCommands;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).SpecialCommands = value;
                configuration.Save();
            }
        }


        public static void CreateSavedConnectionsList(string[] names)
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).SavedConnections.Clear();
            configuration.Save();
            foreach (string name in names)
            {
                AddConnection(name);
            }
        }

        public static void ClearSavedConnectionsList()
        {
            Configuration configuration = GetConfiguration();
            GetSection(configuration).SavedConnections.Clear();
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

        public static string[] FavoritesToolbarButtons
        {
            get
            {
                return ReadList(GetSection().FavoritesButtons).ToArray();
            }
        }

        public static string[] SavedConnections
        {
            get
            {
                return ReadList(GetSection().SavedConnections).ToArray();
            }
        }

        public static string[] Tags
        {
            get
            {
                return ReadList(GetSection().Tags).ToArray();
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
            editedFavorite.VMRCAdministratorMode = favorite.VMRCAdministratorMode;
            editedFavorite.VMRCReducedColorsMode = favorite.VMRCReducedColorsMode;
            editedFavorite.Telnet = favorite.Telnet;
            editedFavorite.TelnetRows = favorite.TelnetRows;
            editedFavorite.TelnetCols = favorite.TelnetCols;
            editedFavorite.TelnetFont = favorite.TelnetFont;
            editedFavorite.TelnetCursorColor = favorite.TelnetCursorColor;
            editedFavorite.TelnetTextColor = favorite.TelnetTextColor;
            editedFavorite.TelnetBackColor = favorite.TelnetBackColor;
            editedFavorite.Protocol = favorite.Protocol;
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
            editedFavorite.ShowDesktopBackground = favorite.ShowDesktopBackground;
            editedFavorite.Tags = favorite.Tags;
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
        public static int PortScanTimeoutSeconds {
            get {
                return GetSection().PortScanTimeoutSeconds;
            }
            set {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).PortScanTimeoutSeconds = value;
                configuration.Save();
            }
        }
        public static string DefaultDomain
        {
            get
            {
                return GetSection().DefaultDomain;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).DefaultDomain = value;
                configuration.Save();
            }
        }
        public static string DefaultUsername
        {
            get
            {
                return GetSection().DefaultUsername;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).DefaultUsername = value;
                configuration.Save();
            }
        }
        public static string DefaultPassword
        {
            get
            {
                return GetSection().DefaultPassword;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).DefaultPassword = value;
                configuration.Save();
            }
        }

        public static string TerminalsPassword {
            get {
                return GetSection().TerminalsPassword;
            }
            set {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).TerminalsPassword = value;
                configuration.Save();
            }
        }
        public static bool ForceComputerNamesAsURI
        {
            get
            {
                return GetSection().ForceComputerNamesAsURI;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ForceComputerNamesAsURI = value;
                configuration.Save();
            }
        }
        public static bool MinimizeToTray {
            get {
                return GetSection().MinimizeToTray;
            }
            set {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).MinimizeToTray = value;
                configuration.Save();
            }
        }
        public static bool WarnOnConnectionClose
        {
            get
            {
                return GetSection().WarnOnConnectionClose;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).WarnOnConnectionClose = value;
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

        public static bool ShowConfirmDialog
        {
            get
            {
                return GetSection().ShowConfirmDialog;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ShowConfirmDialog = value;
                configuration.Save();
            }
        }

        public static bool SaveConnectionsOnClose
        {
            get
            {
                return GetSection().SaveConnectionsOnClose;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).SaveConnectionsOnClose = value;
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
        public static string CaptureRoot
        {
            get
            {
                string root = GetSection().CaptureRoot;
                if(root == null || root == "")
                {
                    string asmFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    root = System.IO.Path.Combine(asmFolder, "CaptureRoot");
                    CaptureRoot = root;
                }
                if(!System.IO.Directory.Exists(root))
                {
                    System.IO.Directory.CreateDirectory(root);
                }
                return root;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).CaptureRoot = value;
                configuration.Save();
            }
        }


        public static bool ShowWizard
        {
            get
            {
                return GetSection().ShowWizard;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).ShowWizard = value;
                configuration.Save();
            }
        }
        public static bool AutoSwitchOnCapture
        {
            get
            {
                return GetSection().AutoSwitchOnCapture;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).AutoSwitchOnCapture = value;
                configuration.Save();
            }
        }
        public static bool SingleInstance
        {
            get
            {
                return GetSection().SingleInstance;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).SingleInstance = value;
                configuration.Save();
            }
        }

        public static string PsexecLocation
        {
            get
            {
                return GetSection().PsexecLocation;
            }
            set
            {
                Configuration configuration = GetConfiguration();
                GetSection(configuration).PsexecLocation = value;
                configuration.Save();
            }
        }
    }
}
