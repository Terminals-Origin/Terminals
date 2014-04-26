using System.Xml.Linq;

namespace Terminals.Integration.Import.RdcMan
{
    internal class Properties : Settings<Properties>
    {
        internal string Name
        {
            get
            {
                XElement name = this.PropertiesElement.GetNameElement();
                return name != null ? name.Value : string.Empty;
            }
        }

        internal string Comment
        {
            get
            {
                XElement comment = this.PropertiesElement.GetCommentElement();
                return comment.Value;
            }
        }

        # region Inheritable properties inherit="FromParent" or "None"

        internal LogonCredentials LogonCredentials
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetLogonCredentialsElement();
                var parentSettings = this.HasParent ? this.Parent.LogonCredentials : null;
                return new LogonCredentials(settingsElement, parentSettings);
            }
        }

        internal ConnectionSettings ConnectionSettings
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetConnectionSettingsElement();
                var parentSettings = this.HasParent ? this.Parent.ConnectionSettings : null;
                return new ConnectionSettings(settingsElement, parentSettings);
            }
        }

        internal GatewaySettings GatewaySettings
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetGatewaySettingsElement();
                var parentSettings = this.HasParent ? this.Parent.GatewaySettings : null;
                return new GatewaySettings(settingsElement, parentSettings);
            }
        }

        internal RemoteDesktop RemoteDesktop
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetRemoteDesktopElement();
                var parentSettings = this.HasParent ? this.Parent.RemoteDesktop : null;
                return new RemoteDesktop(settingsElement, parentSettings);
            }
        }
        
        internal LocalResources LocalResources
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetLocalResourcesElement();
                var parentSettings = this.HasParent ? this.Parent.LocalResources : null;
                return new LocalResources(settingsElement, parentSettings);
            }
        }

        #endregion

        public Properties(XElement propertiesElement, Properties parent = null)
            :base(propertiesElement, parent)
        {
        }

        public override string ToString()
        {
            string parentName = this.GetParentName();
            return string.Format("Properties:Name='{0}',Parent='{1}'", 
                this.Name, parentName);
        }

        private string GetParentName()
        {
            return this.HasParent ? this.Parent.Name : "None";
        }
    }
}