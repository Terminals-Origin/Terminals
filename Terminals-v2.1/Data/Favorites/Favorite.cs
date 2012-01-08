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

        // todo REFACTORING remove redundant url property
        private string url = "http://terminals.codeplex.com";
        public String Url
        {
            get { return url; }
            set { url = value; }
        }

        private SecurityOptions security = new SecurityOptions();
        public SecurityOptions Security
        {
            get { return this.security; }
            set { this.security = value; }
        }
        
        private string toolBarIcon;
        public String ToolBarIcon
        {
            get { return toolBarIcon; }
            set { toolBarIcon = value; }
        }

        public Boolean NewWindow { get; set; }
        public String DesktopShare { get; set; }

        private BeforeConnectExecuteOptions executeBeforeConnect = new BeforeConnectExecuteOptions();
        public BeforeConnectExecuteOptions ExecuteBeforeConnect
        {
            get { return this.executeBeforeConnect; }
            set { this.executeBeforeConnect = value; }
        }

        private DisplayOptions display = new DisplayOptions();
        public DisplayOptions Display
        {
            get { return this.display; }
            set { this.display = value; }
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
                    if (!(this.protocolProperties is VncOptions)) // prevent to reset proeprties
                        this.protocolProperties = new VncOptions();
                    break;
                case ConnectionManager.VMRC:
                    if (!(this.protocolProperties is VMRCOptions))
                        this.protocolProperties = new VMRCOptions();
                    break;
                case ConnectionManager.TELNET:
                    if (!(this.protocolProperties is ConsoleOptions))
                        this.protocolProperties = new ConsoleOptions();
                    break;
                case ConnectionManager.SSH:
                    if (!(this.protocolProperties is SshOptions))
                        this.protocolProperties = new SshOptions();
                    break;
                case ConnectionManager.RDP:
                    if (!(this.protocolProperties is RdpOptions))   
                        this.protocolProperties = new RdpOptions();
                    break;
                case ConnectionManager.ICA_CITRIX:
                    if(!(this.protocolProperties is ICAOptions))
                        this.protocolProperties = new ICAOptions();
                    break;
                case ConnectionManager.HTTP:
                case ConnectionManager.HTTPS:
                default:
                    this.protocolProperties = new EmptyOptions();
                    break;
            }
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

            Byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(toEncode);
            String returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        private static String DecodeFrom64(String encodedData)
        {
            if(encodedData == null)
                return null;

            Byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            String returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public override String ToString()
        {
            string domain = this.Security.DomainName;
            if (!String.IsNullOrEmpty(domain))
                domain += "\\";

            return String.Format(@"Favorite:{0}({1})={2}{3}:{4}",
                this.Name, this.Protocol, domain, this.ServerName, this.Port);
        }

        public String GetToolTipText()
        {
            string serverName = this.Protocol == ConnectionManager.HTTP || this.Protocol == ConnectionManager.HTTPS ?
                                this.Url : this.ServerName;

            string userDisplayName = HelperFunctions.UserDisplayName(this.Security.DomainName, this.Security.UserName);
            String toolTip = String.Format("Computer: {1}{0}Port: {2}{0}User: {3}{0}",
                Environment.NewLine, serverName, this.Port, userDisplayName);

            if (Settings.ShowFullInformationToolTips)
            {
                var rdp = this.ProtocolProperties as RdpOptions;
                bool console = false;
                if(rdp != null)
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
            copy.Security = this.Security.Copy();
            copy.ServerName = this.ServerName;
            copy.ToolBarIcon = this.ToolBarIcon;
            copy.Url = this.Url;

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
