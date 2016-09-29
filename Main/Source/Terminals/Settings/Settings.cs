using System;
using System.Linq;
using System.Threading;
using Terminals.Common.Configuration;
using Terminals.Data;
using Terminals.Security;

namespace Terminals.Configuration
{
    internal partial class Settings : IConnectionSettings, IMRUSettings
    {
        public Version ConfigVersion
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

        #region General tab settings

        public bool NeverShowTerminalsWindow
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

        public bool ShowUserNameInTitle
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

        public bool ShowInformationToolTips
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

        public bool ShowFullInformationToolTips
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

        public bool SingleInstance
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

        public bool ShowConfirmDialog
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

        public bool SaveConnectionsOnClose
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

        public bool MinimizeToTray
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
        public bool ForceComputerNamesAsURI
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

        public bool WarnOnConnectionClose
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

        public bool AutoCaseTags
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

        public string DefaultDesktopShare
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

        public int PortScanTimeoutSeconds
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

        public bool ExecuteBeforeConnect
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

        public string ExecuteBeforeConnectCommand
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

        public string ExecuteBeforeConnectArgs
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

        public string ExecuteBeforeConnectInitialDirectory
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

        public bool ExecuteBeforeConnectWaitForExit
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

        #region Security

        /// <summary>
        /// Gets or sets the stored master password key, getter and setter don't make any encryption.
        /// </summary>
        internal string MasterPasswordHash
        {
            get
            {
                return GetSection().TerminalsPassword;
            }
            private set
            {
                GetSection().TerminalsPassword = value;
            }
        }

        /// <summary>
        /// This updates all stored passwords and assigns new key material in config section.
        /// </summary>
        internal void UpdateConfigurationPasswords(string newMasterKey, string newStoredMasterKey)
        {
            MasterPasswordHash = newStoredMasterKey;
            UpdateStoredPasswords(newMasterKey);
            SaveImmediatelyIfRequested();
        }

        private void UpdateStoredPasswords(string newKeyMaterial)
        {
            TerminalsConfigurationSection configSection = GetSection();
            configSection.EncryptedDefaultPassword = PasswordFunctions2.EncryptPassword(DefaultPassword, newKeyMaterial);
            configSection.EncryptedAmazonAccessKey = PasswordFunctions2.EncryptPassword(AmazonAccessKey, newKeyMaterial);
            configSection.EncryptedAmazonSecretKey = PasswordFunctions2.EncryptPassword(AmazonSecretKey, newKeyMaterial);
            configSection.EncryptedConnectionString = PasswordFunctions2.EncryptPassword(ConnectionString, newKeyMaterial);
            configSection.DatabaseMasterPasswordHash = PasswordFunctions2.EncryptPassword(DatabaseMasterPassword, newKeyMaterial);
        }

        #endregion

        #region Security tab settings

        public string DefaultDomain
        {
            get
            {
                string encryptedDefaultDomain = GetSection().DefaultDomain;
                return PersistenceSecurity.DecryptPassword(encryptedDefaultDomain);
            }
            set
            {
                GetSection().DefaultDomain = PersistenceSecurity.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        public string DefaultUsername
        {
            get
            {
                string encryptedDefaultUserName = GetSection().DefaultUsername;
                return PersistenceSecurity.DecryptPassword(encryptedDefaultUserName);
            }
            set
            {
                GetSection().DefaultUsername = PersistenceSecurity.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        internal string DefaultPassword
        {
            get
            {
                string encryptedDefaultPassword = GetSection().EncryptedDefaultPassword;
                return PersistenceSecurity.DecryptPassword(encryptedDefaultPassword);
            }
            set
            {
                GetSection().EncryptedDefaultPassword = PersistenceSecurity.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        /// <summary>
        /// Gets or sets authentication instance used to encrypt and decrypt secured settings.
        /// Set only initialized and authenticated instance before access to any data. 
        /// </summary>
        internal PersistenceSecurity PersistenceSecurity { get; set; }

        internal bool UseAmazon
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

        internal string AmazonAccessKey
        {
            get
            {
                string encryptedAmazonAccessKey = GetSection().EncryptedAmazonAccessKey;
                return PersistenceSecurity.DecryptPassword(encryptedAmazonAccessKey);
            }

            set
            {
                GetSection().EncryptedAmazonAccessKey = PersistenceSecurity.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        internal string AmazonSecretKey
        {
            get
            {
                string encryptedAmazonSecretKey = GetSection().EncryptedAmazonSecretKey;
                return PersistenceSecurity.DecryptPassword(encryptedAmazonSecretKey);
            }

            set
            {
                GetSection().EncryptedAmazonSecretKey = PersistenceSecurity.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        internal string AmazonBucketName
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

        public string FlickrToken
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

        public bool UseProxy
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

        public string ProxyAddress
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

        public int ProxyPort
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

        public bool EnableCaptureToClipboard
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

        public bool EnableCaptureToFolder
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

        internal bool EnabledCaptureToFolderAndClipBoard
        {
            get { return EnableCaptureToClipboard || EnableCaptureToFolder; }
        }

        public bool AutoSwitchOnCapture
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

        public string CaptureRoot
        {
            get
            {
                string root = GetSection().CaptureRoot;
                if (String.IsNullOrEmpty(root))
                    root = FileLocations.DefaultCaptureRootDirectory;

                return root;
            }

            set
            {
                GetSection().CaptureRoot = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region More tab settings

        public bool RestoreWindowOnLastTerminalDisconnect
        {
            get
            {
                return GetSection().RestoreWindowOnLastTerminalDisconnect;
            }

            set
            {
                GetSection().RestoreWindowOnLastTerminalDisconnect = value;
                SaveImmediatelyIfRequested();
            }
        }

        public bool EnableFavoritesPanel
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

        public bool EnableGroupsMenu
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

        public bool AutoExapandTagsPanel
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

        public SortProperties DefaultSortProperty
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

        public bool Office2007BlackFeel
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

        public bool Office2007BlueFeel
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

        public bool VncAutoScale
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

        public bool VncViewOnly
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

        public int VncDisplayNumber
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

        public int FavoritePanelWidth
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

        public bool ShowFavoritePanel
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

        public bool ToolbarsLocked
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

        public string UpdateSource
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

        public bool ShowWizard
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

        public string PsexecLocation
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

        public string SavedCredentialsLocation
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

        public string SavedFavoritesFileLocation
        {
            get
            {
                return GetSection().SavedFavoritesFileLocation;
            }

            set
            {
                GetSection().SavedFavoritesFileLocation = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region MRU lists

        public string[] MRUServerNames
        {
            get
            {
                return GetSection().ServersMRU.ToSortedArray();
            }
        }

        public string[] MRUDomainNames
        {
            get
            {
                return GetSection().DomainsMRU.ToSortedArray();
            }
        }

        public string[] MRUUserNames
        {
            get
            {
                return GetSection().UsersMRU.ToSortedArray();
            }
        }

        public string[] SavedSearches
        {
            get
            {
                return GetSection().SearchesMRU.ToSortedArray();
            }
            set
            {
                var newSearches = new MRUItemConfigurationElementCollection(value.Distinct());
                GetSection().SearchesMRU = newSearches;
            }
        }

        #endregion

        #region Tags/Favorite lists Settings

        public string ExpandedFavoriteNodes
        {
            get
            {
                return GetSection().ExpandedFavoriteNodes;
            }

            set
            {
                GetSection().ExpandedFavoriteNodes = value;
                SaveImmediatelyIfRequested();
            }
        }

        public string ExpandedHistoryNodes
        {
            get
            {
                return GetSection().ExpandedHistoryNodes;
            }

            set
            {
                GetSection().ExpandedHistoryNodes = value;
                SaveImmediatelyIfRequested();
            }
        }
        
        #endregion

        #region Persistence File/Sql database

        /// <summary>
        /// Gets or sets encrypted entity framework connection string
        /// </summary>
        internal string ConnectionString
        {
            get
            {
                string encryptedConnectionString = GetSection().EncryptedConnectionString;
                return this.PersistenceSecurity.DecryptPassword(encryptedConnectionString);
            }

            set
            {
                GetSection().EncryptedConnectionString = this.PersistenceSecurity.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        /// <summary>
        /// Gets or sets bidirectional encrypted database password. We need it in unencrypted form 
        /// to be able authenticate against the database and don't prompt user for it.
        /// </summary>
        internal string DatabaseMasterPassword
        {
            get
            {
                string databaseMasterPasswordHash = GetSection().DatabaseMasterPasswordHash;
                return this.PersistenceSecurity.DecryptPassword(databaseMasterPasswordHash);
            }
            set
            {
                GetSection().DatabaseMasterPasswordHash = this.PersistenceSecurity.EncryptPassword(value);
                SaveImmediatelyIfRequested();
            }
        }

        /// <summary>
        /// Gets or sets the value identifying the persistence.
        /// 0 by default - file persisted data, 1 - SQL database
        /// </summary>
        internal byte PersistenceType
        {
            get
            {
                return GetSection().PersistenceType;
            }

            set
            {
                GetSection().PersistenceType = value;
                SaveImmediatelyIfRequested();
            }
        }

        #endregion

        #region Public

        public void AddServerMRUItem(string name)
        {
            GetSection().ServersMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public void AddDomainMRUItem(string name)
        {
            GetSection().DomainsMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public void AddUserMRUItem(string name)
        {
            GetSection().UsersMRU.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public void AddConnection(string name)
        {
            GetSection().SavedConnections.AddByName(name);
            SaveImmediatelyIfRequested();
        }

        public SpecialCommandConfigurationElementCollection SpecialCommands
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

        public void CreateSavedConnectionsList(string[] names)
        {
            GetSection().SavedConnections.Clear();
            SaveImmediatelyIfRequested();
            foreach (string name in names)
            {
                AddConnection(name);
            }
        }

        public void ClearSavedConnectionsList()
        {
            GetSection().SavedConnections.Clear();
            SaveImmediatelyIfRequested();
        }

        public string[] SavedConnections
        {
            get
            {
                return GetSection().SavedConnections.ToList().ToArray();
            }
        }

        public KeysSection SSHKeys
        {
            get
            {
                KeysSection keys = Config.Sections["SSH"] as KeysSection;
                if (keys == null)
                {
                    // The section wasn't found, so add it.
                    keys = new KeysSection();
                    Config.Sections.Add("SSH", keys);
                }

                return keys;
            }
        }

        internal FormsSection Forms
        {
            get
            {
                FormsSection formsSection = Config.Sections[FormsSection.FORMS] as FormsSection;
                if (formsSection == null)
                {
                    // The section wasn't found, so add it.
                    formsSection = new FormsSection();
                    Config.Sections.Add(FormsSection.FORMS, formsSection);
                }

                return formsSection;
            }
        }

        #endregion

        public bool AskToReconnect
        {
            get
            {
                return GetSection().AskToReconnect;
            }
            set
            {
                GetSection().AskToReconnect = value;
                SaveImmediatelyIfRequested();
            }
        }
    }
}
