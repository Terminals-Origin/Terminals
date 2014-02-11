using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManProperties
    {
        private readonly RdcManProperties parent;

        protected XElement PropertiesElement { get; private set; }
        
        private bool HasParent
        {
            get { return this.parent != null; }
        }

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

        internal XElement LogonCredentials
        {
            get
            {
                return this.PropertiesElement.GetLogonCredentialsElement();
            }
        }

        internal XElement ConnectionSettings
        {
            get
            {
                return this.PropertiesElement.GetConnectionSettingsElement();
            }
        }

        internal XElement GatewaySettings
        {
            get
            {
                return this.PropertiesElement.GetGatewaySettingsElement();
            }
        }

        internal XElement RemoteDesktop
        {
            get
            {
                return this.PropertiesElement.GetRemoteDesktopElement();
            }
        }
        
        internal XElement LocalResources
        {
            get
            {
                return this.PropertiesElement.GetLocalResourcesElement();
            }
        }

        internal XElement SecuritySettings
        {
            get
            {
                return this.PropertiesElement.GetSecuritySettingsElement();
            }
        }

        internal XElement DisplaySettings
        {
            get
            {
                return this.PropertiesElement.GetDisplaySettingsElement();
            }
        }

        #endregion

        public RdcManProperties(XElement propertiesElement, RdcManProperties parent = null)
        {
            this.PropertiesElement = propertiesElement;
            this.parent = parent;
        }

        public override string ToString()
        {
            string parentName = this.GetParentName();
            return string.Format("Properties:Name='{0}',Parent='{1}'", 
                this.Name, parentName);
        }

        private string GetParentName()
        {
            return this.HasParent ? this.parent.Name : "None";
        }
    }
}