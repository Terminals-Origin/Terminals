using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Network;

namespace Terminals.Data
{
    [Serializable]
    public class Favorite : IFavorite
    {
        private Guid id = Guid.NewGuid();
        [XmlAttribute("id")]
        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Gets or sets its associated groups container. Used to resolve associated groups membership.
        /// </summary>
        internal Groups Groups { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Only to identify groups containing this favorite. Manipulating this property 
        /// has no effect in persistence layer
        /// </summary>
        [XmlIgnore]
        List<IGroup> IFavorite.Groups
        {
            get { return this.GetGroups(); }
        }

        private string protocol = ConnectionManager.RDP;
        public String Protocol
        {
            get { return protocol; }
            set
            {
                protocol = value;
                this.protocolProperties = UpdateProtocolPropertiesByProtocol(this.protocol, this.protocolProperties);
                AssignStoreToRdpOptions(this.ProtocolProperties, this.persistenceSecurity);
            }
        }

        private int port = ConnectionManager.RDPPort;
        public Int32 Port
        {
            get { return port; }
            set
            {
                port = value;
            }
        }

        public string ServerName { get; set; }

        private SecurityOptions security = new SecurityOptions();
        /// <summary>
        /// Gets or sets the user credits. Only for serialization purposes.
        /// General access is done by interface property
        /// </summary>
        public SecurityOptions Security
        {
            get { return this.security; }
            set { this.security = value; }
        }

        ISecurityOptions IFavorite.Security
        {
            get { return this.security; }
        }

        private string toolBarIconFile;
        public string ToolBarIconFile
        {
            get
            {
                return this.toolBarIconFile;
            }
            set
            {
                this.toolBarIconFile = value;
                // don't dispose the previous image here, because it can be loaded from default shared FavoriteIcons
                this.toolBarIconImage = null;
            }
        }

        private Image toolBarIconImage;
        [XmlIgnore]
        public Image ToolBarIconImage
        {
            get
            {
                // cache the image to safe the resources
                if(this.toolBarIconImage == null)
                    this.toolBarIconImage = FavoriteIcons.GetFavoriteIcon(this);
                return this.toolBarIconImage;
            }
        }

        public Boolean NewWindow { get; set; }
        public String DesktopShare { get; set; }

        private PersistenceSecurity persistenceSecurity;

        private IBeforeConnectExecuteOptions executeBeforeConnect = new BeforeConnectExecuteOptions();
        /// <summary>
        /// Only for serialization
        /// </summary>
        public BeforeConnectExecuteOptions ExecuteBeforeConnect
        {
            get { return this.executeBeforeConnect as BeforeConnectExecuteOptions; }
            set { this.executeBeforeConnect = value; }
        }

        IBeforeConnectExecuteOptions IFavorite.ExecuteBeforeConnect
        {
            get { return this.executeBeforeConnect; }
        }

        private IDisplayOptions display = new DisplayOptions();
        /// <summary>
        /// Only for serialization.
        /// </summary>
        public DisplayOptions Display
        {
            get { return this.display as DisplayOptions; }
            set { this.display = value; }
        }

        IDisplayOptions IFavorite.Display
        {
            get { return this.display; }
        }

        private ProtocolOptions protocolProperties = new RdpOptions();
        /// <summary>
        /// Depending on selected protocol, this should contain the protocol detailed options.
        /// Because default protocol is RDP, also this properties are RdpOptions by default.
        /// This property should be never null, use EmptyProperties to provide in not necessary case.
        /// </summary>
        [XmlElement(typeof(RdpOptions))]
        [XmlElement(typeof(VncOptions))]
        [XmlElement(typeof(VMRCOptions))]
        [XmlElement(typeof(SshOptions))]
        [XmlElement(typeof(ConsoleOptions))]
        [XmlElement(typeof(ICAOptions))]
        [XmlElement(typeof(WebOptions))]
        [XmlElement(typeof(EmptyOptions))]
        public ProtocolOptions ProtocolProperties
        {
            get { return protocolProperties; }
            // setter should be used for deserialization only
            set { protocolProperties = value; }
        }

        /// <summary>
        /// Explicit call of update properties container depending on selected protocol.
        /// </summary>
        internal static ProtocolOptions UpdateProtocolPropertiesByProtocol(string newProtocol, ProtocolOptions currentOptions)
        {
            switch (newProtocol) // Don't call this in property setter, because of serializer
            {
                case ConnectionManager.VNC:
                    return SwitchPropertiesIfNotTheSameType<VncOptions>(currentOptions);
                case ConnectionManager.VMRC:
                    return SwitchPropertiesIfNotTheSameType<VMRCOptions>(currentOptions);
                case ConnectionManager.TELNET:
                    return SwitchPropertiesIfNotTheSameType<ConsoleOptions>(currentOptions);
                case ConnectionManager.SSH:
                    return SwitchPropertiesIfNotTheSameType<SshOptions>(currentOptions);
                case ConnectionManager.RDP:
                    return SwitchPropertiesIfNotTheSameType<RdpOptions>(currentOptions);
                case ConnectionManager.ICA_CITRIX:
                    return SwitchPropertiesIfNotTheSameType<ICAOptions>(currentOptions);
                case ConnectionManager.HTTP:
                case ConnectionManager.HTTPS:
                    return SwitchPropertiesIfNotTheSameType<WebOptions>(currentOptions);
                default:
                    return new EmptyOptions();
            }
        }

        private static ProtocolOptions SwitchPropertiesIfNotTheSameType<TOptions>(ProtocolOptions currentOptions)
            where TOptions: ProtocolOptions
        {
            if (!(currentOptions is TOptions)) // prevent to reset properties
                return Activator.CreateInstance<TOptions>();

            return currentOptions;
        }

        private string notes;
        string IFavorite.Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }

        /// <summary>
        /// Gets or sets the notes property as Base64 encoded string. This is used only for xml serialization.
        /// </summary>
        public String Notes
        {
            get
            {
                return EncodeTo64(this.notes);
            }
            set
            {
                this.notes = DecodeFrom64(value);
            }
        }

        [XmlIgnore]
        public string GroupNames
        {
            get
            {
                List<IGroup> groups = this.GetGroups();
                return GroupsListToString(groups);
            }
        }

        internal static string GroupsListToString(List<IGroup> groups)
        {
            if (groups.Count == 0)
                return string.Empty;

            string[] groupNames = groups.Select(group => @group.Name).ToArray();
            return string.Join(",", groupNames);
        }

        private static String EncodeTo64(String toEncode)
        {
            if (toEncode == null)
                return null;

            Byte[] toEncodeAsBytes = Encoding.Unicode.GetBytes(toEncode);
            String returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        private static String DecodeFrom64(String encodedData)
        {
            try
            {
                return TryDecodeFrom64(encodedData);
            }
            catch (FormatException)
            {
                // the text wasnt encoded, so the original (issue when upgrading from older file)
                return encodedData;
            }
        }

        private static string TryDecodeFrom64(string encodedData)
        {
            if (encodedData == null)
                return null;

            Byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            String returnValue = Encoding.Unicode.GetString(encodedDataAsBytes);
            return returnValue;
        }

        private List<IGroup> GetGroups()
        {
            return this.Groups.GetGroupsContainingFavorite(this.Id);
        }

        public String GetToolTipText()
        {
            return GetToolTipText(this);
        }

        internal static String GetToolTipText(IFavorite selected)
        {
            string userDisplayName = HelperFunctions.UserDisplayName(selected.Security.Domain, selected.Security.UserName);
            String toolTip = String.Format("Computer: {1}{0}Port: {2}{0}User: {3}{0}",
                Environment.NewLine, selected.ServerName, selected.Port, userDisplayName);

            if (Settings.ShowFullInformationToolTips)
            {
                var extendedToolTip = CreateExtendedToolTip(selected);
                toolTip += extendedToolTip;
            }

            return toolTip;
        }

        private static string CreateExtendedToolTip(IFavorite selected)
        {
            var rdp = selected.ProtocolProperties as RdpOptions;
            bool console = false;
            if (rdp != null)
                console = rdp.ConnectToConsole;

            return String.Format("Groups: {1}{0}Connect to Console: {2}{0}Notes: {3}{0}",
                                 Environment.NewLine, selected.GroupNames, console, selected.Notes);
        }

        /// <summary>
        /// Creates new favorite filled by properties of this favorite except Id and Groups.
        /// </summary>
        IFavorite IFavorite.Copy()
        {
            var copy = new Favorite();
            copy.UpdateFrom(this);
            return copy;
        }

        void IFavorite.UpdateFrom(IFavorite source)
        {
            var sourceFavorite = source as Favorite;
            if (sourceFavorite == null)
                return;
            this.UpdateFrom(sourceFavorite);
        }

        private void UpdateFrom(Favorite source)
        {
            // we do not call AssignStores here, because they will be copied together with the child properties
            this.Groups = source.Groups;
            this.DesktopShare = source.DesktopShare;
            this.Display = source.Display.Copy();
            this.ExecuteBeforeConnect = source.ExecuteBeforeConnect.Copy();
            this.Name = source.Name;
            this.NewWindow = source.NewWindow;
            this.Notes = source.Notes;
            this.Port = source.Port;
            this.Protocol = source.Protocol;
            this.security = source.security.Copy();
            this.ServerName = source.ServerName;
            this.ToolBarIconFile = source.ToolBarIconFile;

            this.ProtocolProperties = source.ProtocolProperties.Copy();
        }

        /// <summary>
        /// Returns text compare to method values selecting property to compare
        /// depending on Settings default sort property value
        /// </summary>
        /// <param name="target">not null favorite to compare with</param>
        /// <returns>result of String CompareTo method</returns>
        int IFavorite.CompareByDefaultSorting(IFavorite target)
        {
            return CompareByDefaultSorting(this, target);
        }

        internal static int CompareByDefaultSorting(IFavorite source, IFavorite target)
        {
            switch (Settings.DefaultSortProperty)
            {
                case SortProperties.ServerName:
                    return source.ServerName.CompareTo(target.ServerName);
                case SortProperties.Protocol:
                    return source.Protocol.CompareTo(target.Protocol);
                case SortProperties.ConnectionName:
                    return source.Name.CompareTo(target.Name);
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Replaces stored password by new one created from newKeyMaterial in underlying store.
        /// </summary>
        /// <param name="newKeyMaterial">New shared key used to encrypt passwords in the store</param>
        internal void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            this.security.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
            UpdatePasswordsInProtocolProperties(this.protocolProperties, newKeyMaterial);
        }

        internal static void UpdatePasswordsInProtocolProperties(ProtocolOptions protocolProperties, string newKeyMaterial)
        {
            RdpOptions rdpOptions = protocolProperties as RdpOptions;
            if (rdpOptions != null)
            {
                SecurityOptions tsGatewaySecurity = rdpOptions.TsGateway.Security;
                tsGatewaySecurity.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
            }
        }

        bool IStoreIdEquals<IFavorite>.StoreIdEquals(IFavorite oponent)
        {
            var oponentFavorite = oponent as Favorite;
            if (oponentFavorite == null)
                return false;

            return oponentFavorite.Id == this.Id;
        }

        public int GetStoreIdHash()
        {
            return this.Id.GetHashCode();
        }

        internal void AssignStores(PersistenceSecurity persistenceSecurity, Groups groups)
        {
            this.persistenceSecurity = persistenceSecurity;
            this.Groups = groups;
            this.Security.AssignStore(persistenceSecurity);
            AssignStoreToRdpOptions(this.ProtocolProperties, persistenceSecurity);
        }

        internal static void AssignStoreToRdpOptions(ProtocolOptions protocolOptions, PersistenceSecurity persistenceSecurity)
        {
            var rdpOptions = protocolOptions as RdpOptions;
            if (rdpOptions != null)
            {
                rdpOptions.AssignStore(persistenceSecurity);
            }
        }

        public override String ToString()
        {
            return ToString(this);
        }

        internal static string ToString(IFavorite favorite)
        {
            string domain = favorite.Security.Domain;
            if (!String.IsNullOrEmpty(domain))
                domain += "\\";

            return String.Format(@"Favorite:{0}({1})={2}{3}:{4}",
                favorite.Name, favorite.Protocol, domain, favorite.ServerName, favorite.Port);
        }
    }
}
