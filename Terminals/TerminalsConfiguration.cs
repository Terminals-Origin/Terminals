using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{
    public enum DesktopSize { x640 = 0, x800, x1024, FitToWindow, FullScreen, AutoScale };
    public enum Colors { Bits8 = 0, Bit16, Bits24, Bits32 };
    public enum RemoteSounds { Redirect = 0, PlayOnServer = 1, DontPlay = 2 };
    
    public class TerminalsConfigurationSection : ConfigurationSection
    {
        public TerminalsConfigurationSection()
        {

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
        public bool ShowUserNameInTitle {
            get {
                return (bool)this["showUserNameInTitle"];
            }
            set {
                this["showUserNameInTitle"] = value;
            }
        }

    [ConfigurationProperty("portScanTimeoutSeconds", DefaultValue=5)]
        public int PortScanTimeoutSeconds {
            get {
                int timeout = 5;
                if (this["portScanTimeoutSeconds"] != null && this["portScanTimeoutSeconds"].ToString() != string.Empty) {
                    int.TryParse(this["portScanTimeoutSeconds"].ToString(),out timeout);
                }
                return timeout;
            }
            set {
                this["portScanTimeoutSeconds"] = value;
            }
        }

        [ConfigurationProperty("minimizeToTray", DefaultValue = true)]
        public bool MinimizeToTray {
            get {
                if (this["minimizeToTray"] == null || this["minimizeToTray"].ToString() == string.Empty) return true;
                bool min = true;
                bool.TryParse(this["minimizeToTray"].ToString(), out min);
                return min;
            }
            set {
                this["minimizeToTray"] = value;
            }
        }
        [ConfigurationProperty("terminalsPassword", DefaultValue = "")]
        public string TerminalsPassword {
            get {
                return Convert.ToString(this["terminalsPassword"]);
            }
            set {
                this["terminalsPassword"] = "";
                //hash the password
                if (value != string.Empty) {
                    this["terminalsPassword"] = Unified.Encryption.Hash.Hash.GetHash(value, Unified.Encryption.Hash.Hash.HashType.SHA512);
                }
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
    }
    
    public class MRUItemConfigurationElementCollection : ConfigurationElementCollection
    {
        public MRUItemConfigurationElementCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MRUItemConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new MRUItemConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((MRUItemConfigurationElement)element).Name;
        }

        public new string AddElementName
        {
            get
            {
                return base.AddElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string ClearElementName
        {
            get
            {
                return base.ClearElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string RemoveElementName
        {
            get
            {
                return base.RemoveElementName;
            }
        }

        public new int Count
        {
            get
            {
                return base.Count;
            }
        }

        public MRUItemConfigurationElement this[int index]
        {
            get
            {
                return (MRUItemConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public MRUItemConfigurationElement this[string Name]
        {
            get
            {
                return (MRUItemConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(MRUItemConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public MRUItemConfigurationElement ItemByName(string name)
        {
            return (MRUItemConfigurationElement)BaseGet(name);
        }

        public void Add(MRUItemConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(MRUItemConfigurationElement item)
        {
            if (BaseIndexOf(item) >= 0)
                BaseRemove(item.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
    
    public class MRUItemConfigurationElement : ConfigurationElement
    {
        public MRUItemConfigurationElement()
        {

        }

        public MRUItemConfigurationElement(string name)
        {
            Name = name;
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
    
    public class FavoriteConfigurationElementCollection : ConfigurationElementCollection
    {
        public FavoriteConfigurationElementCollection()
            : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FavoriteConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new FavoriteConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FavoriteConfigurationElement)element).Name;
        }

        public FavoriteConfigurationElement this[int index]
        {
            get
            {
                return (FavoriteConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public FavoriteConfigurationElement this[string Name]
        {
            get
            {
                return (FavoriteConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(FavoriteConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public void Add(FavoriteConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(FavoriteConfigurationElement item)
        {
            if (BaseIndexOf(item) >= 0)
                BaseRemove(item.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
    
    public class FavoriteConfigurationElement : ConfigurationElement
    {
        public FavoriteConfigurationElement()
        {

        }

        public FavoriteConfigurationElement(string name)
        {
            Name = name;
        }
        [ConfigurationProperty("vmrcreducedcolorsmode", IsRequired = true, DefaultValue=false)]
        public bool VMRCReducedColorsMode {
            get {
                return (bool)this["vmrcreducedcolorsmode"];
            }
            set {
                this["vmrcreducedcolorsmode"] = value;
            }
        }
        [ConfigurationProperty("telnet", IsRequired = true, DefaultValue = true)]
        public bool Telnet {
            get {
                return (bool)this["telnet"];
            }
            set {
                this["telnet"] = value;
            }
        }
        [ConfigurationProperty("telnetrows", IsRequired = true, DefaultValue = 33)]
        public int TelnetRows {
            get {
                return (int)this["telnetrows"];
            }
            set {
                this["telnetrows"] = value;
            }
        }
        [ConfigurationProperty("telnetcols", IsRequired = true, DefaultValue = 110)]
        public int TelnetCols {
            get {
                return (int)this["telnetcols"];
            }
            set {
                this["telnetcols"] = value;
            }
        }
        [ConfigurationProperty("vmrcadministratormode", IsRequired = true, DefaultValue = false)]
        public bool VMRCAdministratorMode {
            get {
                return (bool)this["vmrcadministratormode"];
            }
            set {
                this["vmrcadministratormode"] = value;
            }
        }
        [ConfigurationProperty("protocol", IsRequired = true, DefaultValue = "RDP")]
        public string Protocol {
            get {
                return (string)this["protocol"];
            }
            set {
                this["protocol"] = value;
            }
        }
        [ConfigurationProperty("telnetfont", IsRequired = true, DefaultValue = "[Font: Name=Microsoft Sans Serif, Size=10]")]
        public string TelnetFont {
            get {
                return (string)this["telnetfont"];
            }
            set {
                this["telnetfont"] = value;
            }
        }
        [ConfigurationProperty("telnetbackcolor", IsRequired = true, DefaultValue = "Black")]
        public string TelnetBackColor {
            get {
                return (string)this["telnetbackcolor"];
            }
            set {
                this["telnetbackcolor"] = value;
            }
        }

        [ConfigurationProperty("telnettextcolor", IsRequired = true, DefaultValue = "White")]
        public string TelnetTextColor {
            get {
                return (string)this["telnettextcolor"];
            }
            set {
                this["telnettextcolor"] = value;
            }
        }
        [ConfigurationProperty("telnetcursorcolor", IsRequired = true, DefaultValue = "Green")]
        public string TelnetCursorColor {
            get {
                return (string)this["telnetcursorcolor"];
            }
            set {
                this["telnetcursorcolor"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("serverName", IsRequired = true)]
        public string ServerName
        {
            get
            {
                return (string)this["serverName"];
            }
            set
            {
                this["serverName"] = value;
            }
        }

        [ConfigurationProperty("domainName", IsRequired = true)]
        public string DomainName
        {
            get
            {
                return (string)this["domainName"];
            }
            set
            {
                this["domainName"] = value;
            }
        }

        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get
            {
                return (string)this["userName"];
            }
            set
            {
                this["userName"] = value;
            }
        }

        [ConfigurationProperty("encryptedPassword", IsRequired = true)]
        public string EncryptedPassword
        {
            get
            {
                return (string)this["encryptedPassword"];
            }
            set
            {
                this["encryptedPassword"] = value;
            }
        }

        public string Password
        {
            get
            {
                return Functions.DecryptPassword(EncryptedPassword);
            }
            set
            {
                EncryptedPassword = Functions.EncryptPassword(value);
            }
        }

        [ConfigurationProperty("connectToConsole", IsRequired = true)]
        public bool ConnectToConsole
        {
            get
            {
                return (bool)this["connectToConsole"];
            }
            set
            {
                this["connectToConsole"] = value;
            }
        }

        [ConfigurationProperty("desktopSize", IsRequired = true, DefaultValue = DesktopSize.FitToWindow)]
        public DesktopSize DesktopSize
        {
            get
            {
                return (DesktopSize)this["desktopSize"];
            }
            set
            {
                this["desktopSize"] = value;
            }
        }

        [ConfigurationProperty("colors", IsRequired = true, DefaultValue = Colors.Bits32)]
        public Colors Colors
        {
            get
            {
                return (Colors)this["colors"];
            }
            set
            {
                this["colors"] = value;
            }
        }

        [ConfigurationProperty("sounds", DefaultValue = RemoteSounds.DontPlay)]
        public RemoteSounds Sounds
        {
            get
            {
                return (RemoteSounds)this["sounds"];
            }
            set
            {
                this["sounds"] = value;
            }
        }

        [ConfigurationProperty("redirectDrives")]
        public bool RedirectDrives
        {
            get
            {
                return (bool)this["redirectDrives"];
            }
            set
            {
                this["redirectDrives"] = value;
            }
        }

        [ConfigurationProperty("redirectPorts")]
        public bool RedirectPorts
        {
            get
            {
                return (bool)this["redirectPorts"];
            }
            set
            {
                this["redirectPorts"] = value;
            }
        }

        [ConfigurationProperty("redirectPrinters")]
        public bool RedirectPrinters
        {
            get
            {
                return (bool)this["redirectPrinters"];
            }
            set
            {
                this["redirectPrinters"] = value;
            }
        }

        [ConfigurationProperty("redirectSmartCards")]
        public bool RedirectSmartCards
        {
            get
            {
                return (bool)this["redirectSmartCards"];
            }
            set
            {
                this["redirectSmartCards"] = value;
            }
        }

        [ConfigurationProperty("redirectClipboard", DefaultValue = true)]
        public bool RedirectClipboard
        {
            get
            {
                return (bool)this["redirectClipboard"];
            }
            set
            {
                this["redirectClipboard"] = value;
            }
        }

        [ConfigurationProperty("redirectDevices")]
        public bool RedirectDevices
        {
            get
            {
                return (bool)this["redirectDevices"];
            }
            set
            {
                this["redirectDevices"] = value;
            }
        }

        [ConfigurationProperty("port", DefaultValue = 3389)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("desktopShare")]
        public string DesktopShare
        {
            get
            {
                return (string)this["desktopShare"];
            }
            set
            {
                this["desktopShare"] = value;
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

        [ConfigurationProperty("showDesktopBackground")]
        public bool ShowDesktopBackground
        {
            get
            {
                return (bool)this["showDesktopBackground"];
            }
            set
            {
                this["showDesktopBackground"] = value;
            }
        }

        [ConfigurationProperty("tags")]
        public string Tags
        {
            get
            {
                return (string)this["tags"];
            }
            set
            {
                this["tags"] =  value;
            }
        }

        public List<string> TagList
        {
            get
            {
                List<string> tagList = new List<string>();
                string[] splittedTags = Tags.Split(',');
                if (!((splittedTags.Length == 1) && (String.IsNullOrEmpty(splittedTags[0]))))
                {
                    foreach (string tag in splittedTags)
                    {
                        tagList.Add(tag);
                    }
                }
                return tagList;
            }
        }

        public int PerformanceFlags
        {
            get
            {
                int result = 0;
                if (!ShowDesktopBackground)
                    result = result | 1;
                //TODO: Add more performance flags
                return result;
            }
        }
    }
    
    public class GroupConfigurationElementCollection : ConfigurationElementCollection
    {
        public GroupConfigurationElementCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new GroupConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new GroupConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((GroupConfigurationElement)element).Name;
        }

        public new string AddElementName
        {
            get
            {
                return base.AddElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string ClearElementName
        {
            get
            {
                return base.ClearElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string RemoveElementName
        {
            get
            {
                return base.RemoveElementName;
            }
        }

        public new int Count
        {
            get
            {
                return base.Count;
            }
        }

        public GroupConfigurationElement this[int index]
        {
            get
            {
                return (GroupConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public GroupConfigurationElement this[string Name]
        {
            get
            {
                return (GroupConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(GroupConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public GroupConfigurationElement ItemByName(string name)
        {
            return (GroupConfigurationElement)BaseGet(name);
        }

        public void Add(GroupConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(GroupConfigurationElement item)
        {
            if (BaseIndexOf(item) >= 0)
                BaseRemove(item.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
    
    public class FavoriteAliasConfigurationElementCollection : ConfigurationElementCollection
    {
        public FavoriteAliasConfigurationElementCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FavoriteAliasConfigurationElement();
        }


        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new FavoriteAliasConfigurationElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((FavoriteAliasConfigurationElement)element).Name;
        }

        public new string AddElementName
        {
            get
            {
                return base.AddElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string ClearElementName
        {
            get
            {
                return base.ClearElementName;
            }
            set
            {
                base.AddElementName = value;
            }
        }

        public new string RemoveElementName
        {
            get
            {
                return base.RemoveElementName;
            }
        }

        public new int Count
        {
            get
            {
                return base.Count;
            }
        }

        public FavoriteAliasConfigurationElement this[int index]
        {
            get
            {
                return (FavoriteAliasConfigurationElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public FavoriteAliasConfigurationElement this[string Name]
        {
            get
            {
                return (FavoriteAliasConfigurationElement)BaseGet(Name);
            }
        }

        public int IndexOf(FavoriteAliasConfigurationElement item)
        {
            return BaseIndexOf(item);
        }

        public FavoriteAliasConfigurationElement ItemByName(string name)
        {
            return (FavoriteAliasConfigurationElement)BaseGet(name);
        }

        public void Add(FavoriteAliasConfigurationElement item)
        {
            BaseAdd(item);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(FavoriteAliasConfigurationElement item)
        {
            if (BaseIndexOf(item) >= 0)
                BaseRemove(item.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
    
    public class FavoriteAliasConfigurationElement : ConfigurationElement
    {
        public FavoriteAliasConfigurationElement()
        {

        }

        public FavoriteAliasConfigurationElement(string name)
        {
            Name = name;
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
    
    public class GroupConfigurationElement : ConfigurationElement
    {
        public GroupConfigurationElement()
        {

        }

        public GroupConfigurationElement(string name)
        {
            Name = name;
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("favoriteAliases")]
        [ConfigurationCollection(typeof(FavoriteAliasConfigurationElementCollection))]
        public FavoriteAliasConfigurationElementCollection FavoriteAliases
        {
            get
            {
                return (FavoriteAliasConfigurationElementCollection)this["favoriteAliases"];
            }
            set
            {
                this["favoriteAliases"] = value;
            }
        }
    }
}
