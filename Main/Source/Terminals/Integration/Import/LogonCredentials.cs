using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class LogonCredentials : RdcManSettings<LogonCredentials>
    {
        internal string UserName
        {
            get { return this.PropertiesElement.GetUserName(); }
        }

        internal string Domain
        {
            get { return this.PropertiesElement.GetDomain(); }
        }

        internal string Password
        {
            get { return this.PropertiesElement.GetPassword(); }
        }

        public LogonCredentials(XElement settingsElement, LogonCredentials parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}