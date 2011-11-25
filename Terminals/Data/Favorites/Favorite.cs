using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Terminals.Connections;

namespace Terminals.Data
{
    [Serializable]
    public class Favorite
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

        private List<Guid> groups = new List<Guid>();
        public List<Guid> Groups
        {
            get { return groups; }
        }

        private string protocol = ConnectionManager.RDP;
        public String Protocol
        {
            get { return protocol; }
            set { protocol = value; }
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

        // todo remove redundant url property
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

        private BeforeConnectExecuteOptions beforeConnectExecute = new BeforeConnectExecuteOptions();
        public BeforeConnectExecuteOptions BeforeConnectExecute
        {
            get { return this.beforeConnectExecute; }
            set { this.beforeConnectExecute = value; }
        }

        private DisplayOptions display = new DisplayOptions();
        public DisplayOptions Display
        {
            get { return this.display; }
            set { this.display = value; }
        }

        private object protocolProperties = new RdpOptions();
        /// <summary>
        /// Depending on selected protocol, this should contian the protocol detailed options.
        /// Because default protocol is RDP, also this properties are RdpOptions by default.
        /// </summary>
        [XmlElement(typeof(RdpOptions))]
        [XmlElement(typeof(ConsoleOptions))]
        [XmlElement(typeof(VncOptions))]
        [XmlElement(typeof(VMRCOptions))]
        [XmlElement(typeof(SshOptions))]
        [XmlElement(typeof(ICAOptions))]
        public object ProtocolProperties
        {
            get { return protocolProperties; }
            set { protocolProperties = value; }
        }

        internal void UpdateProtocolPropertiesByProtocol()
        {
            switch (this.protocol)
            {
                case ConnectionManager.VNC:
                    this.protocolProperties = new VncOptions();
                    break;
                case ConnectionManager.VMRC:
                    this.protocolProperties = new VMRCOptions();
                    break;
                case ConnectionManager.TELNET:
                    this.protocolProperties = new ConsoleOptions();
                    break;
                case ConnectionManager.RDP:
                    this.protocolProperties = new RdpOptions();
                    break;
                case ConnectionManager.ICA_CITRIX:
                    this.protocolProperties = new ICAOptions();
                    break;
                case ConnectionManager.HTTP:
                case ConnectionManager.HTTPS:
                default:
                    this.protocolProperties = null;
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
    }
}
