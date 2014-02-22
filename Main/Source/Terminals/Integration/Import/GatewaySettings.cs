using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class GatewaySettings : RdcManSettings<GatewaySettings>
    {
        internal string UserName
        {
            get
            {
                return this.PropertiesElement.GetUserName();
            }
        }

        internal string Domain
        {
            get
            {
                return this.PropertiesElement.GetDomain();
            }
        }

        internal string HostName
        {
            get
            {
                return this.PropertiesElement.GetTsGwHostName();
            }
        }

        internal int LogonMethod
        {
            get
            {
                return this.PropertiesElement.GetLogonMethod();
            }
        }

        public GatewaySettings(XElement settingsElement, GatewaySettings parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}