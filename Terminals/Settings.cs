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
        public static void RebuildTagIndex()
        {
            foreach(string Tag in Settings.Tags)
            {
                Settings.DeleteTag(Tag);
            }

            FavoriteConfigurationElementCollection favs = Settings.GetFavorites();

            foreach(FavoriteConfigurationElement fav in favs)
            {
                foreach(string tag in fav.TagList)
                {
                    Settings.AddTag(tag);
                }
            }
        }

        private static Configuration GetConfiguration()
        {
            string configFile = Terminals.Program.ConfigurationFileLocation;

            if(!System.IO.File.Exists(configFile))
            {
                string templateConfigFile = global::Terminals.Properties.Resources.Terminals;
                using(System.IO.StreamWriter sr = new StreamWriter(configFile))
                {
                    sr.Write(templateConfigFile);
                }
            }
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = null;

            try
            {
                config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                if (System.IO.File.Exists(configFile))
                {
                    string newGUID = System.Guid.NewGuid().ToString();
                    string folder = System.IO.Path.GetDirectoryName(configFile);
                    //back it up before we do anything
                    System.IO.File.Copy(configFile, System.IO.Path.Combine(folder, string.Format("Terminals-{1}-{0}.config", newGUID, System.DateTime.Now.ToFileTime())));

                    //now delete it
                    System.IO.File.Delete(configFile);
                }
                string templateConfigFile = global::Terminals.Properties.Resources.Terminals;
                using(System.IO.StreamWriter sr = new StreamWriter(configFile))
                {
                    sr.Write(templateConfigFile);
                }
                config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            }
            return config;
        }
        private static Configuration ImportConfiguration(string Filename)
        {
            Configuration c = null;// = new TerminalsConfiguration();

            string templateConfigFile = global::Terminals.Properties.Resources.Terminals;

            //get a temp filename to hold the current settings which are failing
            string tempFile = System.IO.Path.GetTempFileName();
            //delete the zerobyte file which is created by default
            if (System.IO.File.Exists(tempFile)) System.IO.File.Delete(tempFile);
            //move the error file to the temp file
            System.IO.File.Move(Filename, tempFile);

            //if its still hanging around, kill it
            if (System.IO.File.Exists(Filename)) System.IO.File.Delete(Filename);
            //write out the template to work from
            using (System.IO.StreamWriter sr = new StreamWriter(Filename))
            {
                sr.Write(templateConfigFile);
            }
            //load up the templated config file
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = Filename;
            c = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            //get a list of the properties on the Settings object (static props)
            System.Reflection.PropertyInfo[] propList = typeof(Settings).GetProperties();

            //read all the xml from the erroring file
            string xml = System.IO.File.ReadAllText(tempFile);
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);
            //get the settings root
            string settingsRoot = "/configuration/settings";
            System.Xml.XmlNode root = doc.SelectSingleNode(settingsRoot);
            try
            {
                //for each setting's attribute
                foreach (System.Xml.XmlAttribute att in root.Attributes)
                {
                    //scan for the related property if any
                    try
                    {
                        foreach (System.Reflection.PropertyInfo info in propList)
                        {
                            try
                            {
                                if (info.Name.ToLower() == att.Name.ToLower())
                                {
                                    //found a matching property, try to set it
                                    string val = att.Value;
                                    info.SetValue(null, System.Convert.ChangeType(val, info.PropertyType), null);
                                    break;
                                }
                            }
                            catch (Exception exc)
                            { //ignore the error
                                Terminals.Logging.Log.Info("", exc);
                            }

                        }
                    }
                    catch (Exception exc)
                    { //ignore the error
                        Terminals.Logging.Log.Info("", exc);
                    }
                }
            }
            catch (Exception exc)
            { //ignore the error
                Terminals.Logging.Log.Info("", exc);
            }

            string favorites = "/configuration/settings/favorites/add";
            System.Xml.XmlNodeList favs = doc.SelectNodes(favorites);

            try
            {
                foreach (System.Xml.XmlNode fav in favs)
                {
                    try
                    {
                        FavoriteConfigurationElement newFav = new FavoriteConfigurationElement();
                        foreach (System.Xml.XmlAttribute att in fav.Attributes)
                        {
                            try
                            {
                                foreach (System.Reflection.PropertyInfo info in newFav.GetType().GetProperties())
                                {
                                    try
                                    {
                                        if (info.Name.ToLower() == att.Name.ToLower())
                                        {
                                            //found a matching property, try to set it
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
                                    { //ignore the error
                                        Terminals.Logging.Log.Info("", exc);
                                    }

                                }
                            }
                            catch (Exception exc)
                            { //ignore the error
                                Terminals.Logging.Log.Info("", exc);
                            }

                        }
                        Settings.AddFavorite(newFav, false);
                    }
                    catch (Exception exc)
                    { //ignore the error
                        Terminals.Logging.Log.Info("", exc);
                    }

                }
            }
            catch (Exception exc)
            { //ignore the error
                Terminals.Logging.Log.Info("", exc);
            }


            return c;
        }

        private static TerminalsConfigurationSection GetSection()
        {
            Configuration configuration = Config;
            TerminalsConfigurationSection c = null;
            try
            {
                c = (TerminalsConfigurationSection)configuration.GetSection("settings");
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                try
                {
                    //kick into the import routine
                    configuration = ImportConfiguration(Terminals.Program.ConfigurationFileLocation);
                    configuration = GetConfiguration();
                    c = (TerminalsConfigurationSection)configuration.GetSection("settings");
                    if(configuration != null) {
                        System.Windows.Forms.MessageBox.Show("Terminals was able to automatically upgrade your existing connections.");
                    }
                }
                catch(Exception importException)
                {
                    Terminals.Logging.Log.Info("", importException);
                    System.Windows.Forms.MessageBox.Show(string.Format("Terminals was NOT able to automatically upgrade your existing connections.\r\nError:{0}",importException.Message));
                    //if(System.IO.File.Exists(configuration.FilePath)) System.IO.File.Delete(configuration.FilePath);
                    //configuration = GetConfiguration();
                    //c = (TerminalsConfigurationSection)configuration.GetSection("settings");
                }
            }
            return c;
        }

        private static TerminalsConfigurationSection GetSection(Configuration configuration)
        {
            return (TerminalsConfigurationSection)configuration.GetSection("settings");
        }
        public static string ToTitleCase(string Name)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Name.ToLower());
        }

        private static void AddMRUItemConfigurationElement(MRUItemConfigurationElementCollection configurationElementCollection, string name)
        {
            MRUItemConfigurationElement configurationElement = configurationElementCollection.ItemByName(name);
            if(configurationElement == null)
            {
                configurationElementCollection.Add(new MRUItemConfigurationElement(name));
            }
        }

        private static void DeleteMRUItemConfigurationElement(MRUItemConfigurationElementCollection configurationElementCollection, string name)
        {
            MRUItemConfigurationElement configurationElement = configurationElementCollection.ItemByName(name);
            if(configurationElement != null)
            {
                configurationElementCollection.Remove(name);
            }
        }

        private static void EditMRUItemConfigurationElement(MRUItemConfigurationElementCollection configurationElementCollection, string oldName, string newName)
        {
            MRUItemConfigurationElement configurationElement = configurationElementCollection.ItemByName(oldName);
            if(configurationElement != null)
            {
                configurationElementCollection[oldName].Name = newName;
            }
        }

        public static void AddServerMRUItem(string name)
        {
            Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).ServersMRU, name);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void AddDomainMRUItem(string name)
        {
            Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).DomainsMRU, name);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void AddUserMRUItem(string name)
        {
            Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).UsersMRU, name);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void AddFavoriteButton(string name)
        {
            Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, name);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void DeleteFavoriteButton(string name)
        {
            Configuration configuration = Config;
            DeleteMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, name);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void EditFavoriteButton(string oldName, string newName)
        {
            Configuration configuration = Config;
            EditMRUItemConfigurationElement(GetSection(configuration).FavoritesButtons, oldName, newName);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void AddTag(string tag)
        {
            if(Settings.AutoCaseTags) tag = ToTitleCase(tag);

            Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).Tags, tag);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void DeleteTag(string tag)
        {
            if(Settings.AutoCaseTags) tag = ToTitleCase(tag);
            Configuration configuration = Config;
            DeleteMRUItemConfigurationElement(GetSection(configuration).Tags, tag);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void CreateFavoritesToolbarButtonsList(string[] names)
        {
            Configuration configuration = Config;
            GetSection(configuration).FavoritesButtons.Clear();
            if(!DelayConfigurationSave) configuration.Save();
            foreach(string name in names)
            {
                AddFavoriteButton(name);
            }
        }

        public static void AddConnection(string name)
        {
            Configuration configuration = Config;
            AddMRUItemConfigurationElement(GetSection(configuration).SavedConnections, name);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static SpecialCommandConfigurationElementCollection SpecialCommands
        {
            get
            {
                Configuration configuration = Config;
                return GetSection(configuration).SpecialCommands;
            }
            set
            {
                Configuration configuration = Config;
                GetSection(configuration).SpecialCommands = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }


        public static void CreateSavedConnectionsList(string[] names)
        {
            Configuration configuration = Config;
            GetSection(configuration).SavedConnections.Clear();
            if(!DelayConfigurationSave) configuration.Save();
            foreach(string name in names)
            {
                AddConnection(name);
            }
        }

        public static void ClearSavedConnectionsList()
        {
            Configuration configuration = Config;
            GetSection(configuration).SavedConnections.Clear();
            if(!DelayConfigurationSave) configuration.Save();
        }

        private static List<string> ReadList(MRUItemConfigurationElementCollection configurationElementCollection)
        {
            List<string> list = new List<string>();
            foreach(MRUItemConfigurationElement configurationElement in configurationElementCollection)
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

        public enum SortProperties { ServerName, ConnectionName, Protocol, None }
        public static SortedDictionary<string, FavoriteConfigurationElement> GetSortedFavorites(SortProperties SortProperty) {
            FavoriteConfigurationElementCollection favlist = GetFavorites();
            if(favlist != null) {
                SortedDictionary<string, FavoriteConfigurationElement> favs = new SortedDictionary<string, FavoriteConfigurationElement>();
                int counter = 0;
                foreach(FavoriteConfigurationElement fav in favlist) {
                    string key = new string('a', counter);
                    switch(SortProperty) {
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
            if(section != null) return section.Favorites;
            return null;
        }

        public static void EditFavorite(string oldName, FavoriteConfigurationElement favorite)
        {
            Configuration configuration = Config;
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
            editedFavorite.DisableWallPaper = favorite.DisableWallPaper;

            editedFavorite.DisableCursorBlinking = favorite.DisableCursorBlinking;
            editedFavorite.DisableCursorShadow = favorite.DisableCursorShadow;
            editedFavorite.DisableFullWindowDrag = favorite.DisableFullWindowDrag;
            editedFavorite.DisableMenuAnimations = favorite.DisableMenuAnimations;
            editedFavorite.DisableTheming = favorite.DisableTheming;


            editedFavorite.Tags = favorite.Tags;
            if(!DelayConfigurationSave) configuration.Save();
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
            if(shownOnToolbar && !showOnToolbar)
            {
                DeleteFavoriteButton(oldName);
            }
            else if(shownOnToolbar && (oldName != favorite.Name))
            {
                EditFavoriteButton(oldName, favorite.Name);
            }
            else if(!shownOnToolbar && showOnToolbar)
            {
                AddFavoriteButton(favorite.Name);
            }
        }

        public static void DeleteFavorite(string name)
        {
            Configuration configuration = Config;
            GetSection(configuration).Favorites.Remove(name);
            if(!DelayConfigurationSave) configuration.Save();
            DeleteFavoriteButton(name);
        }

        public static void AddFavorite(FavoriteConfigurationElement favorite, bool showOnToolbar)
        {
            Configuration configuration = Config;
            GetSection(configuration).Favorites.Add(favorite);
            if(!DelayConfigurationSave) configuration.Save();
            if(showOnToolbar)
            {
                AddFavoriteButton(favorite.Name);
            }
            else
            {
                DeleteFavoriteButton(favorite.Name);
            }

            if(favorite.Tags != null && favorite.Tags.Trim() != "")
            {
                foreach(string tag in favorite.TagList)
                {
                    AddTag(tag);
                }
            }

        }

        public static GroupConfigurationElementCollection GetGroups()
        {
            return GetSection().Groups;
        }

        public static void DeleteGroup(string name)
        {
            Configuration configuration = Config;
            GetSection(configuration).Groups.Remove(name);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static void AddGroup(GroupConfigurationElement group)
        {
            Configuration configuration = Config;
            GetSection(configuration).Groups.Add(group);
            if(!DelayConfigurationSave) configuration.Save();
        }

        public static bool ShowUserNameInTitle
        {
            get
            {
                return GetSection().ShowUserNameInTitle;
            }
            set
            {
                Configuration configuration = Config;
                GetSection(configuration).ShowUserNameInTitle = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).PortScanTimeoutSeconds = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).DefaultDomain = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static string UpdateSource {
            get {
                return GetSection().UpdateSource;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).UpdateSource = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).DefaultUsername = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static bool UseProxy {
            get {
                return GetSection().UseProxy;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).UseProxy = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static string ProxyAddress {
            get {
                return GetSection().ProxyAddress;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).ProxyAddress = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static int ProxyPort {
            get {
                return GetSection().ProxyPort;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).ProxyPort = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).DefaultPassword = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).AutoCaseTags = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).UseAmazon = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).AmazonAccessKey = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static bool VncAutoScale
        {
            get
            {
                return GetSection().VncAutoScale;
            }
            set
            {
                Configuration configuration = Config;
                GetSection(configuration).VncAutoScale = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).VncViewOnly = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).VncDisplayNumber = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).AmazonSecretKey = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }




        public static string FlickrToken {
            get {
                return GetSection().FlickrToken;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).FlickrToken = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static int FavoritePanelWidth {
            get {
                return GetSection().FavoritePanelWidth;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).FavoritePanelWidth = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static string TerminalsPassword
        {
            get
            {
                return GetSection().TerminalsPassword;
            }
            set
            {
                Configuration configuration = Config;
                GetSection(configuration).TerminalsPassword = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ForceComputerNamesAsURI = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static bool MinimizeToTray {
            get {
                return GetSection().MinimizeToTray;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).MinimizeToTray = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static SortProperties DefaultSortProperty {
            get {
                TerminalsConfigurationSection config = GetSection();
                if(config != null) {
                    string dsp = config.DefaultSortProperty;
                    SortProperties prop = (SortProperties)System.Enum.Parse(typeof(SortProperties), dsp);
                    return prop;
                }
                return SortProperties.ConnectionName;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).DefaultSortProperty = value.ToString();
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        
        public static bool EnableGroupsMenu {
            get {
                return GetSection().EnableGroupsMenu;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).EnableGroupsMenu = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static bool EnableFavoritesPanel {
            get {
                return GetSection().EnableFavoritesPanel;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).EnableFavoritesPanel = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).WarnOnConnectionClose = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ShowInformationToolTips = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ShowFullInformationToolTips = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).DefaultDesktopShare = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnect = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnectCommand = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnectArgs = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnectInitialDirectory = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ExecuteBeforeConnectWaitForExit = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ShowConfirmDialog = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).SaveConnectionsOnClose = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }


        private static bool? _supportsRDP6;

        public static bool SupportsRDP6
        {
            get
            {
                if(!_supportsRDP6.HasValue)
                {
                    try
                    {
                        MSTSCLib.IMsRdpClient2 rdpClient = new MSTSCLib.MsRdpClient2Class();
                        _supportsRDP6 = ((rdpClient as MSTSCLib6.IMsRdpClient5) != null);
                    }
                    catch(Exception exc) 
                    {
                        Terminals.Logging.Log.Info("", exc);
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
                    root = @".\CaptureRoot";
                    CaptureRoot = root;
                }
                if (!System.IO.Directory.Exists(root))
                {

                    Logging.Log.Info("Capture root folder does not exist:" + root + ". Lets try to create it now.");
                    try
                    {
                        System.IO.Directory.CreateDirectory(root);
                    }
                    catch (Exception exc)
                    {
                        Logging.Log.Info(@"Capture root could not be created, set it to the default value : .\CaptureRoot", exc);
                        root = @".\CaptureRoot";
                        try
                        {
                            System.IO.Directory.CreateDirectory(root);
                        }
                        catch (Exception exc1)
                        {
                            Logging.Log.Info(@"Capture root could not be created again.  Abort!", exc1);
                        }
                    }
                    CaptureRoot = root;

                }
                return root;
            }
            set
            {
                Configuration configuration = Config;
                GetSection(configuration).CaptureRoot = value;
                if (!DelayConfigurationSave) configuration.Save();
            }
        }

        private static Configuration config=null;
        public static Configuration Config
        {
            get
            {
                if (config == null) config = GetConfiguration();
                return config;
            }
        }
        public static bool DelayConfigurationSave = false;

        public static string ToolStripSettingsFile = "ToolStrip.settings";
        public static ToolStripSettings ToolbarSettings {
            get {
                ToolStripSettings settings = null;
                if(System.IO.File.Exists(ToolStripSettingsFile)) {
                    string s = System.IO.File.ReadAllText(ToolStripSettingsFile);
                    settings = ToolStripSettings.LoadFromString(s);
                }
                return settings;
            }
            set {
                System.IO.File.WriteAllText(ToolStripSettingsFile, value.ToString());
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
                Configuration configuration = Config;
                GetSection(configuration).Office2007BlueFeel = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).Office2007BlackFeel = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static bool AutoExapandTagsPanel
        {
            get {
                return GetSection().AutoExapandTagsPanel;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).AutoExapandTagsPanel = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
        public static bool ToolbarsLocked {
            get {
                return GetSection().ToolbarsLocked;
            }
            set {
                Configuration configuration = Config;
                GetSection(configuration).ToolbarsLocked = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).ShowWizard = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).AutoSwitchOnCapture = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).SingleInstance = value;
                if(!DelayConfigurationSave) configuration.Save();
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
                Configuration configuration = Config;
                GetSection(configuration).PsexecLocation = value;
                if(!DelayConfigurationSave) configuration.Save();
            }
        }
    }
    public class ToolStripSettings : SerializableSortedDictionary<int, ToolStripSetting> {
        public ToolStripSettings() { }
        public static ToolStripSettings LoadFromString(string Settings) {
            return (ToolStripSettings)Unified.Serialize.DeSerializeXML(Settings, typeof(ToolStripSettings), false);
        }
        public override string ToString() {
            string val = "";
            using(System.IO.MemoryStream stm = Unified.Serialize.SerializeXML(this, typeof(ToolStripSettings), false)) {
                if(stm != null) {
                    if(stm.CanSeek && stm.Position > 0) stm.Position = 0;
                    using(System.IO.StreamReader sr = new StreamReader(stm)) {
                        val = sr.ReadToEnd();                        
                    }
                }
            }
            return val;
        }
    }
    public class ToolStripSetting {
        private string name;

        public string Name {
            get { return name; }
            set { name = value; }
        }

        private bool visible;

        public bool Visible {
            get { return visible; }
            set { visible = value; }
        }

        private int row;

        public int Row {
            get { return row; }
            set { row = value; }
        }
	
	
        private string dock;

        public string Dock {
            get { return dock; }
            set { dock = value; }
        }

        private int left;

        public int Left {
            get { return left; }
            set { left = value; }
        }

        private int top;

        public int Top {
            get { return top; }
            set { top = value; }
        }
	
	
	
    }
}