using System;
using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class LogonCredentials : RdcManSettings<LogonCredentials>
    {
        internal string UserName
        {
            get
            {
                Func<string> getParentValue = () => this.Parent.UserName;
                Func<string> getElementValue = this.PropertiesElement.GetUserName;
                return this.ResolveValue(getParentValue, getElementValue, string.Empty);
            }
        }

        internal string Domain
        {
            get
            {
                Func<string> getParentValue = () => this.Parent.Domain;
                Func<string> getElementValue = this.PropertiesElement.GetDomain;
                return this.ResolveValue(getParentValue, getElementValue, string.Empty);
            }
        }

        internal string Password
        {
            get
            {
                Func<string> getParentValue = () => this.Parent.Password;
                Func<string> getElementValue = this.PropertiesElement.GetPassword;
                return this.ResolveValue(getParentValue, getElementValue, string.Empty);
            }
        }

        public LogonCredentials(XElement settingsElement, LogonCredentials parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}