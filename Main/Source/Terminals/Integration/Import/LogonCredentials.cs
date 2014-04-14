using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class LogonCredentials : RdcManSettings<LogonCredentials>
    {
        internal string UserName
        {
            get { return this.Inherited ? this.Parent.UserName : this.PropertiesElement.GetUserName(); }
        }

        internal string Domain
        {
            get { return this.Inherited ? this.Parent.Domain : this.PropertiesElement.GetDomain(); }
        }

        internal string Password
        {
            get { return this.Inherited ? this.Parent.Password : this.PropertiesElement.GetPassword(); }
        }

        public LogonCredentials(XElement settingsElement, LogonCredentials parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}