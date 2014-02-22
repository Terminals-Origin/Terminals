using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManProperties : RdcManSettings<RdcManProperties>
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

        # region inherit="FromParent" or "None"

        internal LogonCredentials LogonCredentials
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetLogonCredentialsElement();
                return new LogonCredentials(settingsElement, this.Parent.LogonCredentials);
            }
        }

        internal ConnectionSettings ConnectionSettings
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetConnectionSettingsElement();
                return new ConnectionSettings(settingsElement, this.Parent.ConnectionSettings);
            }
        }

        internal GatewaySettings GatewaySettings
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetGatewaySettingsElement();
                return new GatewaySettings(settingsElement, this.Parent.GatewaySettings);
            }
        }

        internal RdcManRemoteDesktop RemoteDesktop
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetRemoteDesktopElement();
                return new RdcManRemoteDesktop(settingsElement, this.Parent.RemoteDesktop);
            }
        }
        
        internal LocalResources LocalResources
        {
            get
            {
                var settingsElement = this.PropertiesElement.GetLocalResourcesElement();
                return new LocalResources(settingsElement, this.Parent.LocalResources);
            }
        }

        #endregion

        public RdcManProperties(XElement propertiesElement, RdcManProperties parent = null)
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