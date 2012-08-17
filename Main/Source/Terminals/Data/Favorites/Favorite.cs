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

        public string Name { get; set; }

        /// <summary>
        /// Only to identify groups containing this favorite. Manipulating this property 
        /// has no efect in persistence layer
        /// </summary>
        [XmlIgnore]
        List<IGroup> IFavorite.Groups
        {
            get { return GetGroups(this); }
        }

        private string protocol = ConnectionManager.RDP;
        public String Protocol
        {
            get { return protocol; }
            set
            {
                protocol = value;
                this.protocolProperties = UpdateProtocolPropertiesByProtocol(this.protocol, this.protocolProperties);
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
        /// Gets or sets the user credits. Only for serialization puroposes.
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
                this.ResetLoadedIcon();
            }
        }

        private void ResetLoadedIcon()
        {
            if (this.toolBarIconImage != null)
            {
                this.toolBarIconImage.Dispose();
                this.toolBarIconImage = null;
            }
        }

        private Image toolBarIconImage;
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
        /// Depending on selected protocol, this should contian the protocol detailed options.
        /// Because default protocol is RDP, also this properties are RdpOptions by default.
        /// This property should be never null, use EmptyProperties to provide in not necesary case.
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
            set { protocolProperties = value; }
        }

        /// <summary>
        /// Explicit call of update properties container depending on selected protocol.
        /// </summary>
        internal static ProtocolOptions UpdateProtocolPropertiesByProtocol(string newProtocol, ProtocolOptions currentOptions)
        {
            switch (newProtocol) // Dont call this in property setter, because of serializer
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
            if (!(currentOptions is TOptions)) // prevent to reset proeprties
                return Activator.CreateInstance<TOptions>();

            return currentOptions;
        }

        private string notes;
        public String Notes
        {
            get
            {
                return DecodeFrom64(notes);
            }
            set
            {
                notes = EncodeTo64(value);
            }
        }

        [XmlIgnore]
        public string GroupNames
        {
            get
            {
                List<IGroup> groups = GetGroups(this);
                return GroupsListToString(groups);
            }
        }

        internal static string GroupsListToString(List<IGroup> groups)
        {
            var groupNames = groups.Select(group => @group.Name).ToArray();
            return string.Join(",", groupNames);
        }

        private static String EncodeTo64(String toEncode)
        {
            if (toEncode == null)
                return null;

            Byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            String returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        private static String DecodeFrom64(String encodedData)
        {
            if (encodedData == null)
                return null;

            Byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            String returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        private static List<IGroup> GetGroups(IFavorite selected)
        {
            return Persistence.Instance.Groups.GetGroupsContainingFavorite(selected.Id);
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

            // Get favorite's groups and convert groups list to a comma seperated string
            List<IGroup> groups = GetGroups(selected);
            String grps = (groups.Count > 0) ? String.Join(",", groups.ConvertAll(group => group.Name).ToArray()) : String.Empty;

            string extendedToolTip = String.Format("Groups: {1}{0}Connect to Console: {2}{0}Notes: {3}{0}",
                                                   Environment.NewLine, grps, console, selected.Notes);
            return extendedToolTip;
        }

        /// <summary>
        /// Creates new favorite filled by properties of this favorite exept Id and Groups.
        /// </summary>
        IFavorite IFavorite.Copy()
        {
            var copy = new Favorite();
            copy.DesktopShare = this.DesktopShare;
            copy.Display = this.Display.Copy();
            copy.ExecuteBeforeConnect = this.ExecuteBeforeConnect.Copy();
            copy.Name = this.Name;
            copy.NewWindow = this.NewWindow;
            copy.Notes = this.Notes;
            copy.Port = this.Port;
            copy.Protocol = this.Protocol;
            copy.security = this.security.Copy();
            copy.ServerName = this.ServerName;
            copy.ToolBarIconFile = this.ToolBarIconFile;

            copy.ProtocolProperties = this.ProtocolProperties.Copy();

            return copy;
        }

        /// <summary>
        /// Returns text compareto method values selecting property to compare
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

        void IFavorite.UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
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
