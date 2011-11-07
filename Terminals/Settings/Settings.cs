using System;
using System.Collections.Generic;
using Unified.Encryption.Hash;
using SysConfig = System.Configuration;
using System.IO;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        private static string keyMaterial = string.Empty;

        #region Terminals Version

        public static Version ConfigVersion
        {
            get
            {
                string configVersion = GetSection().ConfigVersion;
                if (configVersion != String.Empty)
                    return new Version(configVersion);

                return null;
            }

            set
            {
                GetSection().ConfigVersion = value.ToString();
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region General tab settings

        public static bool NeverShowTerminalsWindow
        {
            get
            {
                return GetSection().NeverShowTerminalsWindow;
            }

            set
            {
                GetSection().NeverShowTerminalsWindow = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool ShowUserNameInTitle
        {
            get
            {
                return GetSection().ShowUserNameInTitle;
            }

            set
            {
                GetSection().ShowUserNameInTitle = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ShowInformationToolTips = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ShowFullInformationToolTips = value;
                SaveImmediatelyIfRequested();
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
                GetSection().SingleInstance = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ShowConfirmDialog = value;
                SaveImmediatelyIfRequested();
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
                GetSection().SaveConnectionsOnClose = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool MinimizeToTray
        {
            get
            {
                return GetSection().MinimizeToTray;
            }

            set
            {
                GetSection().MinimizeToTray = value;
                SaveImmediatelyIfRequested();
            }
        }

        // Validate server names
        public static bool ForceComputerNamesAsURI
        {
            get
            {
                return GetSection().ForceComputerNamesAsURI;
            }

            set
            {
                GetSection().ForceComputerNamesAsURI = value;
                SaveImmediatelyIfRequested();
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
                GetSection().WarnOnConnectionClose = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool AutoCaseTags
        {
            get
            {
                return GetSection().AutoCaseTags;
            }

            set
            {
                GetSection().AutoCaseTags = value;
                SaveImmediatelyIfRequested();
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
                GetSection().DefaultDesktopShare = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static int PortScanTimeoutSeconds
        {
            get
            {
                return GetSection().PortScanTimeoutSeconds;
            }

            set
            {
                GetSection().PortScanTimeoutSeconds = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Execute Before Connect tab settings

        public static bool ExecuteBeforeConnect
        {
            get
            {
                return GetSection().ExecuteBeforeConnect;
            }

            set
            {
                GetSection().ExecuteBeforeConnect = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ExecuteBeforeConnectCommand = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ExecuteBeforeConnectArgs = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ExecuteBeforeConnectInitialDirectory = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ExecuteBeforeConnectWaitForExit = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Security tab settings

        internal static bool IsMasterPasswordDefined
        {
            get
            {
                return !String.IsNullOrEmpty(GetMasterPasswordHash());
            }
        }

        private static string GetMasterPasswordHash()
        {
            return GetSection().TerminalsPassword;
        }

        internal static void UpdateMasterPassword(string newPassword)
        {
            TerminalsConfigurationSection configSection = GetSection();

            UpdateAllFavoritesPasswords(configSection, newPassword);
            // start of not secured transaction. Old key is still present,
            // but passwords are already encrypted by newKey
            configSection.TerminalsPassword = newPassword;
            SaveImmediatelyIfRequested();

            // finish transaction, the passwords now reflect the new key
            UpdateKeyMaterial(newPassword);
        }

        /// <summary>
        /// During this procedure, the old master key material should be still present.
        /// This finds all stored passwords and updates them to reflect new key material.
        /// </summary>
        private static void UpdateAllFavoritesPasswords(TerminalsConfigurationSection configSection,
            string newMasterPassword)
        {
            string newKeyMaterial = GetKeyMaterial(newMasterPassword);
            configSection.UpdatePasswordsByNewKeyMaterial(newKeyMaterial);
            UpdateFavoritePasswordsByNewKeyMaterial(newKeyMaterial);
            StoredCredentials.Instance.UpdatePasswordsByNewKeyMaterial(newKeyMaterial);
        }

        internal static string KeyMaterial
        {
            get
            {
                return keyMaterial;
            }

            private set
            {
                keyMaterial = value;
            }
        }

        internal static Boolean IsMasterPasswordValid(string passwordToCheck)
        {
            String hashToCheck = Hash.GetHash(passwordToCheck, Hash.HashType.SHA512);
            if (GetMasterPasswordHash() == hashToCheck)
            {
                UpdateKeyMaterial(passwordToCheck);
                return true;
            }

            return false;
        }

        private static void UpdateKeyMaterial(String password)
        {
            KeyMaterial = GetKeyMaterial(password);
        }

        private static string GetKeyMaterial(string password)
        {
            String hashToCheck = Hash.GetHash(password, Hash.HashType.SHA512);
            return Hash.GetHash(password + hashToCheck, Hash.HashType.SHA512);
        }

        public static string DefaultDomain
        {
            get
            {
                return GetSection().DefaultDomain;
            }

            set
            {
                GetSection().DefaultDomain = value;
                SaveImmediatelyIfRequested();
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
                GetSection().DefaultUsername = value;
                SaveImmediatelyIfRequested();
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
                GetSection().DefaultPassword = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool UseAmazon
        {
            get
            {
                return GetSection().UseAmazon;
            }

            set
            {
                GetSection().UseAmazon = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string AmazonAccessKey
        {
            get
            {
                return GetSection().AmazonAccessKey;
            }

            set
            {
                GetSection().AmazonAccessKey = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string AmazonSecretKey
        {
            get
            {
                return GetSection().AmazonSecretKey;
            }

            set
            {
                GetSection().AmazonSecretKey = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string AmazonBucketName
        {
            get
            {
                return GetSection().AmazonBucketName;
            }

            set
            {
                GetSection().AmazonBucketName = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Flickr tab settings

        public static string FlickrToken
        {
            get
            {
                return GetSection().FlickrToken;
            }

            set
            {
                GetSection().FlickrToken = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Proxy tab settings

        public static bool UseProxy
        {
            get
            {
                return GetSection().UseProxy;
            }

            set
            {
                GetSection().UseProxy = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string ProxyAddress
        {
            get
            {
                return GetSection().ProxyAddress;
            }

            set
            {
                GetSection().ProxyAddress = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static int ProxyPort
        {
            get
            {
                return GetSection().ProxyPort;
            }

            set
            {
                GetSection().ProxyPort = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Screen capture tab settings

        public static bool EnableCaptureToClipboard
        {
            get
            {
                return GetSection().EnableCaptureToClipboard;
            }

            set
            {
                GetSection().EnableCaptureToClipboard = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool EnableCaptureToFolder
        {
            get
            {
                return GetSection().EnableCaptureToFolder;
            }

            set
            {
                GetSection().EnableCaptureToFolder = value;
                SaveImmediatelyIfRequested();
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
                return GetSection().AutoSwitchOnCapture;
            }

            set
            {
                GetSection().AutoSwitchOnCapture = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string CaptureRoot
        {
            get
            {
                string root = GetSection().CaptureRoot;
                if (string.IsNullOrEmpty(root))
                    root = GetDefaultCaptureRootDirectory();

                return EnsureCaptureDirectory(root);
            }

            set
            {
                GetSection().CaptureRoot = value;
                SaveImmediatelyIfRequested();
            }
        }

        private static string EnsureCaptureDirectory(string root)
        {
            try
            {
                if (!Directory.Exists(root))
                {
                    Logging.Log.Info(string.Format("Capture root folder does not exist:{0}. Lets try to create it now.", root));
                    Directory.CreateDirectory(root);
                }
            }
            catch (Exception exception)
            {
                root = GetDefaultCaptureRootDirectory();
                string logMessage = string.Format("Capture root could not be created, set it to the default value : {0}", root);
                Logging.Log.Error(logMessage, exception);
                SwitchToDefaultDirectory(root);
            }

            return root;
        }

        private static void SwitchToDefaultDirectory(string defaultRoot)
        {
            try
            {
                Directory.CreateDirectory(defaultRoot);
                CaptureRoot = defaultRoot;
            }
            catch (Exception exception)
            {
                Logging.Log.Error(@"Capture root could not be created again. Abort!", exception);
            }
        }

        private static string GetDefaultCaptureRootDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Terminals Captures");
        }

        #endregion

        #region More tab settings

        public static bool EnableFavoritesPanel
        {
            get
            {
                return GetSection().EnableFavoritesPanel;
            }

            set
            {
                GetSection().EnableFavoritesPanel = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool EnableGroupsMenu
        {
            get
            {
                return GetSection().EnableGroupsMenu;
            }

            set
            {
                GetSection().EnableGroupsMenu = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool AutoExapandTagsPanel
        {
            get
            {
                return GetSection().AutoExapandTagsPanel;
            }

            set
            {
                GetSection().AutoExapandTagsPanel = value;
                SaveImmediatelyIfRequested();
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
                GetSection().DefaultSortProperty = value.ToString();
                SaveImmediatelyIfRequested();
            }
        }

        public static bool Office2007BlackFeel
        {
            get
            {
                return GetSection().Office2007BlackFeel;
            }

            set
            {
                GetSection().Office2007BlackFeel = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool Office2007BlueFeel
        {
            get
            {
                return GetSection().Office2007BlueFeel;
            }

            set
            {
                GetSection().Office2007BlueFeel = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Vnc settings

        public static bool VncAutoScale
        {
            get
            {
                return GetSection().VncAutoScale;
            }

            set
            {
                GetSection().VncAutoScale = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static bool VncViewOnly
        {
            get
            {
                return GetSection().VncViewOnly;
            }

            set
            {
                GetSection().VncViewOnly = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static int VncDisplayNumber
        {
            get
            {
                return GetSection().VncDisplayNumber;
            }

            set
            {
                GetSection().VncDisplayNumber = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Mainform control settings

        public static int FavoritePanelWidth
        {
            get
            {
                return GetSection().FavoritePanelWidth;
            }

            set
            {
                GetSection().FavoritePanelWidth = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ShowFavoritePanel = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static ToolStripSettings ToolbarSettings
        {
            get
            {
                return ToolStripSettings.Load();
            }
            set
            {
                value.Save();
            }
        }

        public static bool ToolbarsLocked
        {
            get
            {
                return GetSection().ToolbarsLocked;
            }

            set
            {
                GetSection().ToolbarsLocked = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Startup settings

        public static string UpdateSource
        {
            get
            {
                return GetSection().UpdateSource;
            }

            set
            {
                GetSection().UpdateSource = value;
                SaveImmediatelyIfRequested();
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
                GetSection().ShowWizard = value;
                SaveImmediatelyIfRequested();
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
                GetSection().PsexecLocation = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static string SavedCredentialsLocation
        {
            get
            {
                return GetSection().SavedCredentialsLocation;
            }

            set
            {
                GetSection().SavedCredentialsLocation = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region MRU lists



        public static string[] MRUServerNames
        {
            get
            {
                return GetSection().ServersMRU.ToList().ToArray();
            }
        }

        public static string[] MRUDomainNames
        {
            get
            {
                return GetSection().DomainsMRU.ToList().ToArray();
            }
        }

        public static string[] MRUUserNames
        {
            get
            {
                return GetSection().UsersMRU.ToList().ToArray();
            }
        }

        #endregion

        #region Public

        public static string ToTitleCase(string name)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
        }

        public static void AddServerMRUItem(string name)
        {
            GetSection().ServersMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static void AddDomainMRUItem(string name)
        {
            GetSection().DomainsMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static void AddUserMRUItem(string name)
        {
            GetSection().UsersMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static void AddConnection(string name)
        {
            GetSection().SavedConnections.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public static SpecialCommandConfigurationElementCollection SpecialCommands
        {
            get
            {
                return GetSection().SpecialCommands;
            }

            set
            {
                GetSection().SpecialCommands = value;
                SaveImmediatelyIfRequested();
            }
        }

        public static void CreateSavedConnectionsList(string[] names)
        {
            GetSection().SavedConnections.Clear();
            SaveImmediatelyIfRequested();
            foreach (string name in names)
            {
                AddConnection(name);
            }
        }

        public static void ClearSavedConnectionsList()
        {
            GetSection().SavedConnections.Clear();
            SaveImmediatelyIfRequested();
        }

        public static string[] SavedConnections
        {
            get
            {
                return GetSection().SavedConnections.ToList().ToArray();
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
                    Config.Sections.Add("SSH", keys);
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

        #endregion
    }
}
