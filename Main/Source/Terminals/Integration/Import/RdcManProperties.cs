using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManProperties
    {
        protected XElement PropertiesElement { get; set; }

        protected RdcManProperties Parent { get; set; }

        private bool HasParent
        {
            get { return this.Parent != null; }
        }

        internal string Name
        {
            get
            {
                XElement name = this.PropertiesElement.Element("name");
                return name != null ? name.Value : string.Empty;
            }
        }

        internal string Comment
        {
            get
            {
                XElement comment = this.PropertiesElement.Element("comment");
                return comment.Value;
            }
        }

        # region inherit="FromParent" or "None"

        internal XElement LogonCredentials
        {
            get
            {
                return this.PropertiesElement.Element("logonCredentials");
            }
        }

        internal XElement ConnectionSettings
        {
            get
            {
                return this.PropertiesElement.Element("connectionSettings");
            }
        }

        internal XElement GatewaySettings
        {
            get
            {
                return this.PropertiesElement.Element("gatewaySettings");
            }
        }

        internal XElement RemoteDesktop
        {
            get
            {
                return this.PropertiesElement.Element("remoteDesktop");
            }
        }
        
        internal XElement LocalResources
        {
            get
            {
                return this.PropertiesElement.Element("localResources");
            }
        }

        internal XElement SecuritySettings
        {
            get
            {
                return this.PropertiesElement.Element("securitySettings");
            }
        }

        internal XElement DisplaySettings
        {
            get
            {
                return this.PropertiesElement.Element("displaySettings");
            }
        }

        #endregion

        public RdcManProperties(XElement propertiesElement, RdcManProperties parent = null)
        {
            this.PropertiesElement = propertiesElement;
            this.Parent = parent;
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