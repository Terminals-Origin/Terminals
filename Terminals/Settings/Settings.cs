using System;
using System.Collections.Generic;
using SysConfig = System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Terminals.Configuration
{
    internal static class Settings
    {
        private static SysConfig.Configuration _config = null;
        private static bool _delayConfigurationSave = false;
        private static string ToolStripSettingsFile = "ToolStrip.settings.config";
        private static TerminalsConfigurationSection _terminalsConfigurationSection;
        private static string keyMaterial = string.Empty;



        #region Terminals Version

        public static Version ConfigVersion
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return null;
                if (_terminalsConfigurationSection.ConfigVersion != String.Empty)
                    return new Version(_terminalsConfigurationSection.ConfigVersion);
                else
                    return null;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ConfigVersion = value.ToString();
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region General tab settings

        public static bool NeverShowTerminalsWindow
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.NeverShowTerminalsWindow;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).NeverShowTerminalsWindow = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }


        public static bool ShowUserNameInTitle
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ShowUserNameInTitle;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ShowUserNameInTitle = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool ShowInformationToolTips
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ShowInformationToolTips;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ShowInformationToolTips = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool ShowFullInformationToolTips
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ShowFullInformationToolTips;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ShowFullInformationToolTips = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool SingleInstance
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.SingleInstance;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).SingleInstance = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool ShowConfirmDialog
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ShowConfirmDialog;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ShowConfirmDialog = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool SaveConnectionsOnClose
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.SaveConnectionsOnClose;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).SaveConnectionsOnClose = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool MinimizeToTray
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.MinimizeToTray;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).MinimizeToTray = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        // Validate server names
        public static bool ForceComputerNamesAsURI
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ForceComputerNamesAsURI;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ForceComputerNamesAsURI = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool WarnOnConnectionClose
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.WarnOnConnectionClose;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).WarnOnConnectionClose = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool AutoCaseTags
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.AutoCaseTags;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).AutoCaseTags = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string DefaultDesktopShare
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.DefaultDesktopShare;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).DefaultDesktopShare = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static int PortScanTimeoutSeconds
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return 0;
                return _terminalsConfigurationSection.PortScanTimeoutSeconds;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).PortScanTimeoutSeconds = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region Execute Before Connect tab settings

        public static bool ExecuteBeforeConnect
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ExecuteBeforeConnect;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnect = value;
                if (!_delayConfigurationSave)
                    configuration.Save();
            }
        }

        public static string ExecuteBeforeConnectCommand
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.ExecuteBeforeConnectCommand;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnectCommand = value;
                if (!_delayConfigurationSave)
                    configuration.Save();
            }
        }

        public static string ExecuteBeforeConnectArgs
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.ExecuteBeforeConnectArgs;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnectArgs = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string ExecuteBeforeConnectInitialDirectory
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.ExecuteBeforeConnectInitialDirectory;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnectInitialDirectory = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool ExecuteBeforeConnectWaitForExit
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ExecuteBeforeConnectWaitForExit;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnectWaitForExit = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region Security tab settings

        public static string TerminalsPassword
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.TerminalsPassword;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).TerminalsPassword = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string KeyMaterial
        {
            get
            {
                return keyMaterial;
            }

            set
            {
                keyMaterial = value;
            }
        }

        public static string DefaultDomain
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.DefaultDomain;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).DefaultDomain = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string DefaultUsername
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.DefaultUsername;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).DefaultUsername = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string DefaultPassword
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.DefaultPassword;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).DefaultPassword = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool UseAmazon
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.UseAmazon;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).UseAmazon = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string AmazonAccessKey
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.AmazonAccessKey;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).AmazonAccessKey = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string AmazonSecretKey
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.AmazonSecretKey;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).AmazonSecretKey = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string AmazonBucketName
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.AmazonBucketName;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).AmazonBucketName = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region Flickr tab settings

        public static string FlickrToken
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.FlickrToken;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).FlickrToken = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region Proxy tab settings

        public static bool UseProxy
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.UseProxy;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).UseProxy = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string ProxyAddress
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.ProxyAddress;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ProxyAddress = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static int ProxyPort
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return 0;
                return _terminalsConfigurationSection.ProxyPort;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ProxyPort = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region Screen capture tab settings

        public static bool EnableCaptureToClipboard
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.EnableCaptureToClipboard;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).EnableCaptureToClipboard = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool EnableCaptureToFolder
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.EnableCaptureToFolder;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).EnableCaptureToFolder = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        internal static bool EnabledCaptureToFolderAndClipBoard
        {
          get { return EnableCaptureToClipboard && EnableCaptureToFolder; }
        }

        public static bool AutoSwitchOnCapture
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.AutoSwitchOnCapture;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).AutoSwitchOnCapture = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string CaptureRoot
        {
            get
            {
                string root;
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection != null && !string.IsNullOrEmpty(_terminalsConfigurationSection.CaptureRoot))
                    root = _terminalsConfigurationSection.CaptureRoot;
                else
                    root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Terminals Captures");

                CaptureRoot = root;

                if (!Directory.Exists(root))
                {
                    Logging.Log.Info("Capture root folder does not exist:" + root + ". Lets try to create it now.");
                    try
                    {
                        Directory.CreateDirectory(root);
                    }
                    catch (Exception exc)
                    {
                        Logging.Log.Error(@"Capture root could not be created, set it to the default value : .\CaptureRoot", exc);
                        try
                        {
                            Directory.CreateDirectory(root);
                        }
                        catch (Exception exc1)
                        {
                            Logging.Log.Error(@"Capture root could not be created again.  Abort!", exc1);
                        }
                    }

                    CaptureRoot = root;
                }

                return root;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).CaptureRoot = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region More tab settings

        public static bool EnableFavoritesPanel
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.EnableFavoritesPanel;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).EnableFavoritesPanel = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool EnableGroupsMenu
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.EnableGroupsMenu;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).EnableGroupsMenu = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool AutoExapandTagsPanel
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.AutoExapandTagsPanel;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).AutoExapandTagsPanel = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static SortProperties DefaultSortProperty
        {
            get
            {
                TerminalsConfigurationSection config = GetSection();
                if (config != null)
                {
                    string dsp = config.DefaultSortProperty;
                    SortProperties prop = (SortProperties)Enum.Parse(typeof(SortProperties), dsp);
                    return prop;
                }

                return SortProperties.ConnectionName;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).DefaultSortProperty = value.ToString();
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool Office2007BlackFeel
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.Office2007BlackFeel;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).Office2007BlackFeel = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool Office2007BlueFeel
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.Office2007BlueFeel;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).Office2007BlueFeel = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region Vnc settings

        public static bool VncAutoScale
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.VncAutoScale;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).VncAutoScale = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool VncViewOnly
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.VncViewOnly;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).VncViewOnly = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static int VncDisplayNumber
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return 0;
                return _terminalsConfigurationSection.VncDisplayNumber;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).VncDisplayNumber = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region Mainform control settings

        public static int FavoritePanelWidth
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return 0;
                return _terminalsConfigurationSection.FavoritePanelWidth;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).FavoritePanelWidth = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool ShowFavoritePanel
        {
            get
            {
                return GetSection().ShowFavoritePanel;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ShowFavoritePanel = value;
                if (!DelayConfigurationSave) configuration.Save();
            }
        }

        public static ToolStripSettings ToolbarSettings
        {
            get
            {
                ToolStripSettings settings = null;
                if (File.Exists(ToolStripSettingsFile))
                {
                    string s = File.ReadAllText(ToolStripSettingsFile);
                    settings = ToolStripSettings.LoadFromString(s);
                }

                return settings;
            }

            set
            {
                File.WriteAllText(ToolStripSettingsFile, value.ToString());
            }
        }

        public static bool ToolbarsLocked
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ToolbarsLocked;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ToolbarsLocked = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string[] FavoritesToolbarButtons
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return null;
                return ReadList(_terminalsConfigurationSection.FavoritesButtons).ToArray();
            }
        }

        #endregion

        #region Startup settings

        public static string UpdateSource
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.UpdateSource;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).UpdateSource = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static bool ShowWizard
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return false;
                return _terminalsConfigurationSection.ShowWizard;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).ShowWizard = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static string PsexecLocation
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return string.Empty;
                return _terminalsConfigurationSection.PsexecLocation;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).PsexecLocation = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        #endregion

        #region Favorites

        public static void EditFavorite(string oldName, FavoriteConfigurationElement favorite, bool showOnToolbar)
        {
            EditFavorite(oldName, favorite);
            bool shownOnToolbar = HasToolbarButton(oldName);
            if (shownOnToolbar && !showOnToolbar)
                DeleteFavoriteButton(oldName);
            else if (shownOnToolbar && (oldName != favorite.Name))
                EditFavoriteButton(oldName, favorite.Name);
            else if (!shownOnToolbar && showOnToolbar)
                AddFavoriteButton(favorite.Name);
        }

        public static void DeleteFavorite(string name)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).Favorites.Remove(name);
            if (!_delayConfigurationSave)
                configuration.Save();
            DeleteFavoriteButton(name);
        }

        public static void AddFavorite(FavoriteConfigurationElement favorite, bool showOnToolbar)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).Favorites.Add(favorite);
            if (!_delayConfigurationSave)
                configuration.Save();

            if (showOnToolbar)
                AddFavoriteButton(favorite.Name);
            else
                DeleteFavoriteButton(favorite.Name);

            if (!string.IsNullOrEmpty(favorite.Tags))
            {
                foreach (string tag in favorite.TagList)
                {
                    AddTag(tag);
                }
            }
        }

        public static FavoriteConfigurationElement GetDefaultFavorite()
        {
            TerminalsConfigurationSection section = GetSection();
            if (section != null && section.DefaultFavorite.Count > 0)
                return section.DefaultFavorite[0];
            return null;
        }

        public static void SaveDefaultFavorite(FavoriteConfigurationElement favorite)
        {
            SysConfig.Configuration configuration = Config;
            FavoriteConfigurationElementCollection defaultFav = GetSection(configuration).DefaultFavorite;
            defaultFav.Clear();
            defaultFav.Add(favorite);
            if (!_delayConfigurationSave)
                configuration.Save();
        }

        public static void RemoveDefaultFavorite()
        {
            SysConfig.Configuration configuration = Config;
            FavoriteConfigurationElementCollection defaultFav = GetSection(configuration).DefaultFavorite;
            defaultFav.Clear();
            if (!_delayConfigurationSave)
                configuration.Save();
        }

        public static SortedDictionary<string, FavoriteConfigurationElement> GetSortedFavorites(SortProperties SortProperty)
        {
            FavoriteConfigurationElementCollection favlist = GetFavorites();
            if (favlist != null)
            {
                SortedDictionary<string, FavoriteConfigurationElement> favs = new SortedDictionary<string, FavoriteConfigurationElement>();
                int counter = 0;
                foreach (FavoriteConfigurationElement fav in favlist)
                {
                    string key = new string('a', counter);
                    switch (SortProperty)
                    {
                        case SortProperties.ConnectionName:
                            favs.Add(fav.Name + key, fav);
                            break;
                        case SortProperties.Protocol:
                            favs.Add(fav.Protocol + key, fav);
                            break;
                        case SortProperties.ServerName:
                            favs.Add(fav.ServerName + key, fav);
                            break;
                        case SortProperties.None:
                            favs.Add(key, fav);
                            break;
                        default:
                            break;
                    }

                    counter++;
                }

                return favs;
            }

            return null;
        }

        public static FavoriteConfigurationElementCollection GetFavorites()
        {
            TerminalsConfigurationSection section = GetSection();
            if (section != null)
                return section.Favorites;
            return null;
        }

        public static FavoriteConfigurationElement GetOneFavorite(string connectionName)
        {
            return GetFavorites()[connectionName];
        }

        public static void EditFavorite(string oldName, FavoriteConfigurationElement favorite)
        {
            if (favorite == null)
                return;

            SysConfig.Configuration configuration = Config;
            TerminalsConfigurationSection section = GetSection(configuration);
            section.Favorites[oldName] = (FavoriteConfigurationElement)favorite.Clone();

            if (!_delayConfigurationSave)
                configuration.Save();
        }

        #endregion

        #region Groups

        public static GroupConfigurationElementCollection GetGroups()
        {
            _terminalsConfigurationSection = GetSection();
            if (_terminalsConfigurationSection == null)
                return null;
            return _terminalsConfigurationSection.Groups;
        }

        public static void DeleteGroup(string name)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).Groups.Remove(name);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void AddGroup(GroupConfigurationElement group)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).Groups.Add(group);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static string[] Tags
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return null;
                return ReadList(_terminalsConfigurationSection.Tags).ToArray();
            }
        }

        #endregion

        #region MRU lists

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
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return null;
                _terminalsConfigurationSection = GetSection();
                return ReadList(_terminalsConfigurationSection.ServersMRU).ToArray();
            }
        }

        public static string[] MRUDomainNames
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return null;
                _terminalsConfigurationSection = GetSection();
                return ReadList(_terminalsConfigurationSection.DomainsMRU).ToArray();
            }
        }

        public static string[] MRUUserNames
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return null;
                return ReadList(_terminalsConfigurationSection.UsersMRU).ToArray();
            }
        }

        #endregion

        #region Public

        public static bool DelayConfigurationSave
        {
            get { return _delayConfigurationSave; }
            set { _delayConfigurationSave = value; }
        }

        public static void Save()
        {
            SysConfig.Configuration configuration = Config;
            configuration.Save();
        }

        public static void SaveAs(String fileName, SysConfig.ConfigurationSaveMode saveMode = SysConfig.ConfigurationSaveMode.Modified,
                                  Boolean forceSaveAll = false)
        {
            SysConfig.Configuration configuration = Config;
            configuration.SaveAs(fileName, saveMode, forceSaveAll);
        }

        public static void RebuildTagIndex()
        {
            foreach (string Tag in Tags)
            {
                DeleteTag(Tag);
            }

            FavoriteConfigurationElementCollection favs = GetFavorites();

            foreach (FavoriteConfigurationElement fav in favs)
            {
                foreach (string tag in fav.TagList)
                {
                    AddTag(tag);
                }
            }
        }

        public static string ToTitleCase(string Name)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Name.ToLower());
        }

        public static void AddServerMRUItem(string name)
        {
            SysConfig.Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).ServersMRU, name);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void AddDomainMRUItem(string name)
        {
            SysConfig.Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).DomainsMRU, name);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void AddUserMRUItem(string name)
        {
            SysConfig.Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).UsersMRU, name);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void AddFavoriteButton(string name)
        {
            SysConfig.Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, name);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void DeleteFavoriteButton(string name)
        {
            SysConfig.Configuration configuration = Config;
            DeleteMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, name);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void EditFavoriteButton(string oldName, string newName)
        {
            SysConfig.Configuration configuration = Config;
            EditMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, oldName, newName);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void AddTag(string tag)
        {
            if (AutoCaseTags) tag = ToTitleCase(tag);

            SysConfig.Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).Tags, tag);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void DeleteTag(string tag)
        {
            if (AutoCaseTags) tag = ToTitleCase(tag);
            SysConfig.Configuration configuration = Config;
            DeleteMRUItemConfigurationElement(GetSection(configuration).Tags, tag);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static void CreateFavoritesToolbarButtonsList(string[] names)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).FavoritesButtons.Clear();
            if (!_delayConfigurationSave) configuration.Save();
            foreach (string name in names)
            {
                AddFavoriteButton(name);
            }
        }

        public static void AddConnection(string name)
        {
            SysConfig.Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).SavedConnections, name);
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static SpecialCommandConfigurationElementCollection SpecialCommands
        {
            get
            {
                SysConfig.Configuration configuration = Config;
                return GetSection(configuration).SpecialCommands;
            }

            set
            {
                SysConfig.Configuration configuration = Config;
                GetSection(configuration).SpecialCommands = value;
                if (!_delayConfigurationSave) configuration.Save();
            }
        }

        public static void CreateSavedConnectionsList(string[] names)
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).SavedConnections.Clear();
            if (!_delayConfigurationSave) configuration.Save();
            foreach (string name in names)
            {
                AddConnection(name);
            }
        }

        public static void ClearSavedConnectionsList()
        {
            SysConfig.Configuration configuration = Config;
            GetSection(configuration).SavedConnections.Clear();
            if (!_delayConfigurationSave) configuration.Save();
        }

        public static string[] SavedConnections
        {
            get
            {
                _terminalsConfigurationSection = GetSection();
                if (_terminalsConfigurationSection == null)
                    return null;
                return ReadList(_terminalsConfigurationSection.SavedConnections).ToArray();
            }
        }

        public static SSHClient.KeysSection SSHKeys
        {
            get
            {
                SSHClient.KeysSection keys = Config.Sections["SSH"] as SSHClient.KeysSection;
                if (keys == null)
                {
                    // The section wasn't found, so add it.
                    keys = new SSHClient.KeysSection();
                    _config.Sections.Add("SSH", keys);
                }

                return keys;
            }

            ////set
            ////{
            ////    Configuration configuration = Config;
            ////    configuration.Sections["SSH"] = value;
            ////    if(!DelayConfigurationSave) configuration.Save();
            ////}
        }

        public static bool HasToolbarButton(string name)
        {
            List<string> buttons = new List<string>();
            buttons.AddRange(FavoritesToolbarButtons);
            return buttons.IndexOf(name) > -1;
        }

        internal static SysConfig.Configuration Config
        {
            get
            {
                if (_config == null)
                    _config = GetConfiguration();
                return _config;
            }
        }

        #endregion

        #region Private

        private static SysConfig.Configuration GetConfiguration()
        {
            string configFile = Program.ConfigurationFileLocation;

            if (!File.Exists(configFile))
            {
                string templateConfigFile = Properties.Resources.Terminals;
                using (StreamWriter sr = new StreamWriter(configFile))
                {
                    sr.Write(templateConfigFile);
                }
            }

            SysConfig.ExeConfigurationFileMap configFileMap = new SysConfig.ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            SysConfig.Configuration config = null;

            try
            {
                config = SysConfig.ConfigurationManager.OpenMappedExeConfiguration(configFileMap, SysConfig.ConfigurationUserLevel.None);
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Get Configuration", exc);
                if (File.Exists(configFile))
                {
                    string newGUID = Guid.NewGuid().ToString();
                    string folder = Path.GetDirectoryName(configFile);

                    // back it up before we do anything
                    File.Copy(configFile, Path.Combine(folder, string.Format("Terminals-{1}-{0}.config", newGUID, DateTime.Now.ToFileTime())));

                    // now delete it
                    File.Delete(configFile);
                }

                string templateConfigFile = Properties.Resources.Terminals;
                using (StreamWriter sr = new StreamWriter(configFile))
                {
                    sr.Write(templateConfigFile);
                }

                config = SysConfig.ConfigurationManager.OpenMappedExeConfiguration(configFileMap, SysConfig.ConfigurationUserLevel.None);
            }

            return config;
        }

        private static void MoveAndDeleteFile(string fileName, string tempFileName)
        {
            // delete the zerobyte file which is created by default
            if (File.Exists(tempFileName))
                File.Delete(tempFileName);

            // move the error file to the temp file
            File.Move(fileName, tempFileName);

            // if its still hanging around, kill it
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        private static SysConfig.Configuration ImportConfiguration(string filename)
        {
            // get a temp filename to hold the current settings which are failing
            string tempFile = Path.GetTempFileName();

            MoveAndDeleteFile(filename, tempFile);
                        
            // write out the template to work from
            using (StreamWriter sr = new StreamWriter(filename))
            {
                sr.Write(Properties.Resources.Terminals);
            }
            
            // load up the templated config file
            SysConfig.ExeConfigurationFileMap configFileMap = new SysConfig.ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = filename;
            SysConfig.Configuration c = SysConfig.ConfigurationManager.OpenMappedExeConfiguration(configFileMap, SysConfig.ConfigurationUserLevel.None);

            // get a list of the properties on the Settings object (static props)
            PropertyInfo[] propList = typeof(Settings).GetProperties();

            // read all the xml from the erroring file
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(tempFile));
            
            // get the settings root
            XmlNode root = doc.SelectSingleNode("/configuration/settings");
            try
            {
                // for each setting's attribute
                foreach (XmlAttribute att in root.Attributes)
                {
                    // scan for the related property if any
                    try
                    {
                        foreach (PropertyInfo info in propList)
                        {
                            try
                            {
                                if (info.Name.ToLower() == att.Name.ToLower())
                                {
                                    // found a matching property, try to set it
                                    string val = att.Value;
                                    info.SetValue(null, System.Convert.ChangeType(val, info.PropertyType), null);
                                    break;
                                }
                            }
                            catch (Exception exc)
                            { // ignore the error
                                Terminals.Logging.Log.Error("Remapping Settings Inner", exc);
                            }
                        }
                    }
                    catch (Exception exc)
                    { // ignore the error
                        Terminals.Logging.Log.Error("Remapping Settings Outer", exc);
                    }
                }
            }
            catch (Exception exc)
            { // ignore the error
                Terminals.Logging.Log.Error("Remapping Settings Outer Try", exc);
            }

            XmlNodeList favs = doc.SelectNodes("/configuration/settings/favorites/add");
            try
            {
                foreach (XmlNode fav in favs)
                {
                    try
                    {
                        FavoriteConfigurationElement newFav = new FavoriteConfigurationElement();
                        foreach (XmlAttribute att in fav.Attributes)
                        {
                            try
                            {
                                foreach (PropertyInfo info in newFav.GetType().GetProperties())
                                {
                                    try
                                    {
                                        if (info.Name.ToLower() == att.Name.ToLower())
                                        {
                                            // found a matching property, try to set it
                                            string val = att.Value;
                                            if (info.PropertyType.IsEnum)
                                            {
                                                info.SetValue(newFav, System.Enum.Parse(info.PropertyType, val), null);
                                            }
                                            else
                                            {
                                                info.SetValue(newFav, System.Convert.ChangeType(val, info.PropertyType), null);
                                            }

                                            break;
                                        }
                                    }
                                    catch (Exception exc)
                                    { // ignore the error
                                        Terminals.Logging.Log.Error("Remapping Favorites 1", exc);
                                    }
                                }
                            }
                            catch (Exception exc)
                            { // ignore the error
                                Terminals.Logging.Log.Error("Remapping Favorites 2", exc);
                            }
                        }

                        Settings.AddFavorite(newFav, false);
                    }
                    catch (Exception exc)
                    { // ignore the error
                        Terminals.Logging.Log.Error("Remapping Favorites 3", exc);
                    }
                }
            }
            catch (Exception exc)
            { // ignore the error
                Terminals.Logging.Log.Error("Remapping Favorites 4", exc);
            }

            return c;
        }

        internal static TerminalsConfigurationSection GetSection()
        {
            SysConfig.Configuration configuration = Config;
            TerminalsConfigurationSection c = null;
            try
            {
                c = (TerminalsConfigurationSection)configuration.GetSection("settings");
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("telnet"))
                {
                    MessageBox.Show("You need to replace telnetrows, telnetcols, telnetfont, telnetbackcolor, "
                    + "telnettextcolor, telnetcursorcolor with consolerows, consolecols, consolefont, consolebackcolor, "
                    + "consoletextcolor, consolecursorcolor");
                    return null;
                }

                Logging.Log.Info("Telnet Section Failed", exc);
                
                try
                {
                    // kick into the import routine
                    configuration = ImportConfiguration(Program.ConfigurationFileLocation);
                    configuration = GetConfiguration();
                    c = (TerminalsConfigurationSection)configuration.GetSection("settings");
                    if (configuration != null)
                        MessageBox.Show("Terminals was able to automatically upgrade your existing connections.");
                }
                catch (Exception importException)
                {
                    Logging.Log.Info("Trying to import connections failed", importException);
                    MessageBox.Show(string.Format("Terminals was NOT able to automatically upgrade your existing connections.\r\nError:{0}", importException.Message));
                }
            }

            return c;
        }

        private static TerminalsConfigurationSection GetSection(SysConfig.Configuration configuration)
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

        #endregion
    }
}
