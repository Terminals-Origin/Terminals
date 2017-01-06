using System;
using System.Xml.Linq;

namespace Terminals.Integration.Import.RdcMan
{
    internal class GatewaySettings : Settings<GatewaySettings>
    {
        internal bool IsEnabled
        {
            get
            {
                Func<bool> getParentValue = () => this.Parent.IsEnabled;
                Func<bool> getElementValue = this.PropertiesElement.IsEnabled;
                return this.ResolveValue(getParentValue, getElementValue);
            }
        }

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

        internal string HostName
        {
            get
            {
                Func<string> getParentValue = () => this.Parent.HostName;
                Func<string> getElementValue = this.PropertiesElement.GetTsGwHostName;
                return this.ResolveValue(getParentValue, getElementValue, string.Empty);
            }
        }

        internal int LogonMethod
        {
            get
            {
                Func<int> getParentValue = () => this.Parent.LogonMethod;
                Func<int> getElementValue = this.PropertiesElement.GetLogonMethod;
                return this.ResolveValue(getParentValue, getElementValue);
            }
        }

        public GatewaySettings(XElement settingsElement, GatewaySettings parent = null)
            : base(settingsElement, parent)
        {
        }
    }
}