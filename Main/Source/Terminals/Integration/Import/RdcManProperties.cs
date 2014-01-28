using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManProperties
    {
        private readonly XElement current;

        private readonly XElement parent;

        private bool HasParent
        {
            get { return this.parent != null; }
        }

        public RdcManProperties(XElement current, XElement parent = null)
        {
            this.current = current;
            this.parent = parent;
        }

        internal string Name
        {
            get
            {
                XElement name = this.current.Element("name");
                return name != null ? name.Value : string.Empty;
            }
        }

        internal string Comment
        {
            get
            {
                XElement comment = this.current.Element("comment");
                return comment.Value;
            }
        }

        # region inherit="FromParent" or "None"

        internal XElement LogonCredentials
        {
            get
            {
                return this.current.Element("logonCredentials");
            }
        }

        internal XElement ConnectionSettings
        {
            get
            {
                return this.current.Element("connectionSettings");
            }
        }

        internal XElement GatewaySettings
        {
            get
            {
                return this.current.Element("gatewaySettings");
            }
        }

        internal XElement RemoteDesktop
        {
            get
            {
                return this.current.Element("remoteDesktop");
            }
        }
        
        internal XElement LocalResources
        {
            get
            {
                return this.current.Element("localResources");
            }
        }

        internal XElement SecuritySettings
        {
            get
            {
                return this.current.Element("securitySettings");
            }
        }

        internal XElement DisplaySettings
        {
            get
            {
                return this.current.Element("displaySettings");
            }
        }

        #endregion

        public override string ToString()
        {
            string parentName = this.GetParentName();
            return string.Format("Properties:Name='{0}',Parent='{1}'", 
                this.Name, parentName);
        }

        private string GetParentName()
        {
            return this.HasParent ? this.parent.Name.ToString() : "None";
        }
    }
}