using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{

    public class TerminalsConfigurationSection : ConfigurationSection
    {
        public TerminalsConfigurationSection()
        {

        }

        [ConfigurationProperty("specialCommands")]
        [ConfigurationCollection(typeof(SpecialCommandConfigurationElement))]
        public SpecialCommandConfigurationElementCollection SpecialCommands
        {
            get
            {
                return (SpecialCommandConfigurationElementCollection)this["specialCommands"];
            }
            set
            {
                this["specialCommands"] = value;
            }
        }



        [ConfigurationProperty("serversMRUList")]
        [ConfigurationCollection(typeof(MRUItemConfigurationElementCollection))]
        public MRUItemConfigurationElementCollection ServersMRU
        {
            get
            {
                return (MRUItemConfigurationElementCollection)this["serversMRUList"];
            }
            set
            {
                this["serversMRUList"] = value;
            }
        }

        [ConfigurationProperty("domainsMRUList")]
        [ConfigurationCollection(typeof(MRUItemConfigurationElementCollection))]
        public MRUItemConfigurationElementCollection DomainsMRU
        {
            get
            {
                return (MRUItemConfigurationElementCollection)this["domainsMRUList"];
            }
            set
            {
                this["domainsMRUList"] = value;
            }
        }

        [ConfigurationProperty("usersMRUList")]
        [ConfigurationCollection(typeof(MRUItemConfigurationElementCollection))]
        public MRUItemConfigurationElementCollection UsersMRU
        {
            get
            {
                return (MRUItemConfigurationElementCollection)this["usersMRUList"];
            }
            set
            {
                this["usersMRUList"] = value;
            }
        }

        [ConfigurationProperty("favorites")]
        [ConfigurationCollection(typeof(FavoriteConfigurationElementCollection))]
        public FavoriteConfigurationElementCollection Favorites
        {
            get
            {
                return (FavoriteConfigurationElementCollection)this["favorites"];
            }
            set
            {
                this["favorites"] = value;
            }
        }

        [ConfigurationProperty("favoritesButtonsList")]
        [ConfigurationCollection(typeof(MRUItemConfigurationElementCollection))]
        public MRUItemConfigurationElementCollection FavoritesButtons
        {
            get
            {
                return (MRUItemConfigurationElementCollection)this["favoritesButtonsList"];
            }
            set
            {
                this["favoritesButtonsList"] = value;
            }
        }

        [ConfigurationProperty("groups")]
        [ConfigurationCollection(typeof(GroupConfigurationElementCollection))]
        public GroupConfigurationElementCollection Groups
        {
            get
            {
                return (GroupConfigurationElementCollection)this["groups"];
            }
            set
            {
                this["groups"] = value;
            }
        }


        [ConfigurationProperty("tags")]
        [ConfigurationCollection(typeof(MRUItemConfigurationElementCollection))]
        public MRUItemConfigurationElementCollection Tags
        {
            get
            {
                return (MRUItemConfigurationElementCollection)this["tags"];
            }
            set
            {
                this["tags"] = value;
            }
        }

        [ConfigurationProperty("showUserNameInTitle")]
        public bool ShowUserNameInTitle
        {
            get
            {
                return (bool)this["showUserNameInTitle"];
            }
            set
            {
                this["showUserNameInTitle"] = value;
            }
        }

        [ConfigurationProperty("portScanTimeoutSeconds", DefaultValue = 5)]
        public int PortScanTimeoutSeconds
        {
            get
            {
                int timeout = 5;
                if(this["portScanTimeoutSeconds"] != null && this["portScanTimeoutSeconds"].ToString() != string.Empty)
                {
                    int.TryParse(this["portScanTimeoutSeconds"].ToString(), out timeout);
                }
                return timeout;
            }
            set
            {
                this["portScanTimeoutSeconds"] = value;
            }
        }

        [ConfigurationProperty("minimizeToTray", DefaultValue = true)]
        public bool MinimizeToTray {
            get {
                if(this["minimizeToTray"] == null || this["minimizeToTray"].ToString() == string.Empty) return true;
                bool min = true;
                bool.TryParse(this["minimizeToTray"].ToString(), out min);
                return min;
            }
            set {
                this["minimizeToTray"] = value;
            }
        }
        [ConfigurationProperty("enableFavoritesPanel", DefaultValue = true)]
        public bool EnableFavoritesPanel {
            get {
                if(this["enableFavoritesPanel"] == null || this["enableFavoritesPanel"].ToString() == string.Empty) return true;
                bool min = true;
                bool.TryParse(this["enableFavoritesPanel"].ToString(), out min);
                return min;
            }
            set {
                this["enableFavoritesPanel"] = value;
            }
        }
        [ConfigurationProperty("enableGroupsMenu", DefaultValue = true)]
        public bool EnableGroupsMenu {
            get {
                if(this["enableGroupsMenu"] == null || this["enableGroupsMenu"].ToString() == string.Empty) return true;
                bool min = true;
                bool.TryParse(this["enableGroupsMenu"].ToString(), out min);
                return min;
            }
            set {
                this["enableGroupsMenu"] = value;
            }
        }
        [ConfigurationProperty("warnOnConnectionClose", DefaultValue = true)]
        public bool WarnOnConnectionClose
        {
            get
            {
                if(this["warnOnConnectionClose"] == null || this["warnOnConnectionClose"].ToString() == string.Empty) return true;
                bool min = true;
                bool.TryParse(this["warnOnConnectionClose"].ToString(), out min);
                return min;
            }
            set
            {
                this["warnOnConnectionClose"] = value;
            }
        }
        [ConfigurationProperty("forceComputerNamesAsURI", DefaultValue = true)]
        public bool ForceComputerNamesAsURI
        {
            get
            {
                if(this["forceComputerNamesAsURI"] == null || this["forceComputerNamesAsURI"].ToString() == string.Empty) return true;
                bool min = true;
                bool.TryParse(this["forceComputerNamesAsURI"].ToString(), out min);
                return min;
            }
            set
            {
                this["forceComputerNamesAsURI"] = value;
            }
        }

        [ConfigurationProperty("favoritePanelWidth", DefaultValue = 300)]
        public int FavoritePanelWidth {
            get {
                if(this["favoritePanelWidth"] != null)
                    return (int)this["favoritePanelWidth"];
                else
                    return 300;
            }
            set {
                this["favoritePanelWidth"] = value;
            }
        }

        [ConfigurationProperty("flickrToken", DefaultValue = "")]
        public string FlickrToken {
            get {
                return Convert.ToString(this["flickrToken"]);
            }
            set {
                this["flickrToken"] = value;
            }
        }

        [ConfigurationProperty("terminalsPassword", DefaultValue = "")]
        public string TerminalsPassword
        {
            get
            {
                return Convert.ToString(this["terminalsPassword"]);
            }
            set
            {
                this["terminalsPassword"] = "";
                //hash the password
                if(value != string.Empty)
                {
                    this["terminalsPassword"] = Unified.Encryption.Hash.Hash.GetHash(value, Unified.Encryption.Hash.Hash.HashType.SHA512);
                }
            }
        }
        
        [ConfigurationProperty("defaultDomain", IsRequired=false)]
        public string DefaultDomain
        {
            get
            {
                return (string)this["defaultDomain"];
            }
            set
            {
                this["defaultDomain"] = value;
            }
        }

        [ConfigurationProperty("updateSource", IsRequired = false)]
        public string UpdateSource
        {
            get
            {
                if(this["updateSource"] == null || (this["updateSource"] as string)=="") {
                    this["updateSource"] = @"http://tools.mscorlib.com/Terminals/TerminalsUpdates.xml";
                }
                return (string)this["updateSource"];
            }
            set
            {
                this["updateSource"] = value;
            }
        }


        [ConfigurationProperty("defaultUsername", IsRequired = false)]
        public string DefaultUsername
        {
            get
            {
                return (string)this["defaultUsername"];
            }
            set
            {
                this["defaultUsername"] = value;
            }
        }

        [ConfigurationProperty("encryptedDefaultPassword", IsRequired = false)]
        public string EncryptedDefaultPassword
        {
            get
            {
                return (string)this["encryptedDefaultPassword"];
            }
            set
            {
                this["encryptedDefaultPassword"] = value;
            }
        }

        public string DefaultPassword
        {
            get
            {
                return Functions.DecryptPassword(EncryptedDefaultPassword);
            }
            set
            {
                EncryptedDefaultPassword = Functions.EncryptPassword(value);
            }
        }



        [ConfigurationProperty("useProxy")]
        public bool UseProxy {
            get {
                return (bool)this["useProxy"];
            }
            set {
                this["useProxy"] = value;
            }
        }


        [ConfigurationProperty("proxyAddress")]
        public string ProxyAddress {
            get {
                return (string)this["proxyAddress"];
            }
            set {
                this["proxyAddress"] = value;
            }
        }


        [ConfigurationProperty("proxyPort")]
        public int ProxyPort {
            get {
                return (int)this["proxyPort"];
            }
            set {
                this["proxyPort"] = value;
            }
        }




        [ConfigurationProperty("showInformationToolTips")]
        public bool ShowInformationToolTips
        {
            get
            {
                return (bool)this["showInformationToolTips"];
            }
            set
            {
                this["showInformationToolTips"] = value;
            }
        }

        [ConfigurationProperty("showFullInformationToolTips")]
        public bool ShowFullInformationToolTips
        {
            get
            {
                return (bool)this["showFullInformationToolTips"];
            }
            set
            {
                this["showFullInformationToolTips"] = value;
            }
        }

        [ConfigurationProperty("defaultDesktopShare")]
        public string DefaultDesktopShare
        {
            get
            {
                return (string)this["defaultDesktopShare"];
            }
            set
            {
                this["defaultDesktopShare"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnect")]
        public bool ExecuteBeforeConnect
        {
            get
            {
                return (bool)this["executeBeforeConnect"];
            }
            set
            {
                this["executeBeforeConnect"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectCommand")]
        public string ExecuteBeforeConnectCommand
        {
            get
            {
                return (string)this["executeBeforeConnectCommand"];
            }
            set
            {
                this["executeBeforeConnectCommand"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectArgs")]
        public string ExecuteBeforeConnectArgs
        {
            get
            {
                return (string)this["executeBeforeConnectArgs"];
            }
            set
            {
                this["executeBeforeConnectArgs"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectInitialDirectory")]
        public string ExecuteBeforeConnectInitialDirectory
        {
            get
            {
                return (string)this["executeBeforeConnectInitialDirectory"];
            }
            set
            {
                this["executeBeforeConnectInitialDirectory"] = value;
            }
        }

        [ConfigurationProperty("executeBeforeConnectWaitForExit")]
        public bool ExecuteBeforeConnectWaitForExit
        {
            get
            {
                return (bool)this["executeBeforeConnectWaitForExit"];
            }
            set
            {
                this["executeBeforeConnectWaitForExit"] = value;
            }
        }

        [ConfigurationProperty("showConfirmDialog")]
        public bool ShowConfirmDialog
        {
            get
            {
                return (bool)this["showConfirmDialog"];
            }
            set
            {
                this["showConfirmDialog"] = value;
            }
        }

        [ConfigurationProperty("saveConnectionsOnClose")]
        public bool SaveConnectionsOnClose
        {
            get
            {
                return (bool)this["saveConnectionsOnClose"];
            }
            set
            {
                this["saveConnectionsOnClose"] = value;
            }
        }

        [ConfigurationProperty("savedConnectionsList")]
        [ConfigurationCollection(typeof(MRUItemConfigurationElementCollection))]
        public MRUItemConfigurationElementCollection SavedConnections
        {
            get
            {
                return (MRUItemConfigurationElementCollection)this["savedConnectionsList"];
            }
            set
            {
                this["savedConnectionsList"] = value;
            }
        }

        [ConfigurationProperty("singleInstance")]
        public bool SingleInstance
        {
            get
            {
                return (bool)this["singleInstance"];
            }
            set
            {
                this["singleInstance"] = value;
            }
        }

        [ConfigurationProperty("autoSwitchOnCapture", DefaultValue = true)]
        public bool AutoSwitchOnCapture
        {
            get
            {
                return (bool)this["autoSwitchOnCapture"];
            }
            set
            {
                this["autoSwitchOnCapture"] = value;
            }
        }

        [ConfigurationProperty("autoExapandTagsPanel", DefaultValue = false)]
        public bool AutoExapandTagsPanel
        {
            get
            {
                return (bool)this["autoExapandTagsPanel"];
            }
            set
            {
                this["autoExapandTagsPanel"] = value;
            }
        }

        [ConfigurationProperty("office2007BlueFeel", DefaultValue = false)]
        public bool Office2007BlueFeel
        {
            get
            {
                return (bool)this["office2007BlueFeel"];
            }
            set
            {
                this["office2007BlueFeel"] = value;
            }
        }

        [ConfigurationProperty("office2007BlackFeel", DefaultValue = false)]
        public bool Office2007BlackFeel
        {
            get
            {
                return (bool)this["office2007BlackFeel"];
            }
            set
            {
                this["office2007BlackFeel"] = value;
            }
        }
        
        [ConfigurationProperty("toolbarsLocked", DefaultValue = true)]
        public bool ToolbarsLocked {
            get {
                return (bool)this["toolbarsLocked"];
            }
            set {
                this["toolbarsLocked"] = value;
            }
        }
        [ConfigurationProperty("showWizard", DefaultValue = true)]
        public bool ShowWizard
        {
            get
            {
                return (bool)this["showWizard"];
            }
            set
            {
                this["showWizard"] = value;
            }
        }
        [ConfigurationProperty("captureRoot")]
        public string CaptureRoot
        {
            get
            {
                return (string)this["captureRoot"];
            }
            set
            {
                this["captureRoot"] = value;
            }
        }


        [ConfigurationProperty("psexecLocation")]
        public string PsexecLocation
        {
            get
            {
                return (string)this["psexecLocation"];
            }
            set
            {
                this["psexecLocation"] = value;
            }
        }
    }

}