using System;
using System.Collections.Generic;
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
        /// has no efect in persistance layer
        /// </summary>
        [XmlIgnore]
        List<IGroup> IFavorite.Groups
        {
            get { return GetGroups(); }
        }

        private List<IGroup> GetGroups()
        {
            return Persistance.Instance.Groups.GetGroupsContainingFavorite(this.Id);
        }

        private string protocol = ConnectionManager.RDP;
        public String Protocol
        {
            get { return protocol; }
            set
            {
                protocol = value;
                UpdateProtocolPropertiesByProtocol();
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

        private string serverName;
        public String ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        private ISecurityOptions security = new SecurityOptions();
        /// <summary>
        /// Gets or sets the user credits. Only for serialization puroposes.
        /// General access is done by interface property
        /// </summary>
        public SecurityOptions Security
        {
            get { return this.security as SecurityOptions; }
            set { this.security = value; }
        }

        ISecurityOptions IFavorite.Security
        {
            get { return this.security; }
        }

        private string toolBarIcon;
        public String ToolBarIcon
        {
            get { return toolBarIcon; }
            set { toolBarIcon = value; }
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
        private void UpdateProtocolPropertiesByProtocol()
        {
            switch (this.protocol) // Dont call this in property setter, because of serializer
            {
                case ConnectionManager.VNC:
                    SwitchPropertiesIfNotTheSameType<VncOptions>();
                    break;
                case ConnectionManager.VMRC:
                    SwitchPropertiesIfNotTheSameType<VMRCOptions>();
                    break;
                case ConnectionManager.TELNET:
                    SwitchPropertiesIfNotTheSameType<ConsoleOptions>();
                    break;
                case ConnectionManager.SSH:
                    SwitchPropertiesIfNotTheSameType<SshOptions>();
                    break;
                case ConnectionManager.RDP:
                    SwitchPropertiesIfNotTheSameType<RdpOptions>();
                    break;
                case ConnectionManager.ICA_CITRIX:
                    SwitchPropertiesIfNotTheSameType<ICAOptions>();
                    break;
                case ConnectionManager.HTTP:
                case ConnectionManager.HTTPS:
                    SwitchPropertiesIfNotTheSameType<WebOptions>();
                    break;
                default:
                    this.protocolProperties = new EmptyOptions();
                    break;
            }
        }

        private void SwitchPropertiesIfNotTheSameType<TOptions>()
            where TOptions: ProtocolOptions
        {
            if (!(this.protocolProperties is TOptions)) // prevent to reset proeprties
                this.protocolProperties = Activator.CreateInstance<TOptions>();
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
                var groupNames = GetGroups().Select(group => group.Name).ToArray();
                return string.Join(",", groupNames);
            }
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

        public override String ToString()
        {
            string domain = this.security.Domain;
            if (!String.IsNullOrEmpty(domain))
                domain += "\\";

            return String.Format(@"Favorite:{0}({1})={2}{3}:{4}",
                this.Name, this.Protocol, domain, this.ServerName, this.Port);
        }

        public String GetToolTipText()
        {
            string userDisplayName = HelperFunctions.UserDisplayName(this.security.Domain, this.security.UserName);
            String toolTip = String.Format("Computer: {1}{0}Port: {2}{0}User: {3}{0}",
                Environment.NewLine, this.ServerName, this.Port, userDisplayName);

            if (Settings.ShowFullInformationToolTips)
            {
                var rdp = this.ProtocolProperties as RdpOptions;
                bool console = false;
                if (rdp != null)
                    console = rdp.ConnectToConsole;

                toolTip += String.Format("Groups: {1}{0}Connect to Console: {2}{0}Notes: {3}{0}",
                    Environment.NewLine, GetGroups(), console, this.Notes);
            }

            return toolTip;
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
            copy.ToolBarIcon = this.ToolBarIcon;

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
            switch (Settings.DefaultSortProperty)
            {
                case SortProperties.ServerName:
                    return this.ServerName.CompareTo(target.ServerName);
                case SortProperties.Protocol:
                    return this.Protocol.CompareTo(target.Protocol);
                case SortProperties.ConnectionName:
                    return this.Name.CompareTo(target.Name);
                default:
                    return -1;
            }
        }

        void IFavorite.UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            this.security.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
            RdpOptions rdpOptions = this.ProtocolProperties as RdpOptions;
            if (rdpOptions != null)
            {
                SecurityOptions tsGatewaySecurity = rdpOptions.TsGateway.Security;
                tsGatewaySecurity.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
            }
        }

        public override bool Equals(object favorite)
        {
            Favorite oponent = favorite as Favorite;
            if (oponent == null)
                return false;

            return this.Id.Equals(oponent.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
