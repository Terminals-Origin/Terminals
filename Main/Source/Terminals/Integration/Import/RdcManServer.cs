using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManServer : RdcManProperties 
    {
        internal string DisplayName
        {
            get
            {
                XElement name = this.PropertiesElement.Element("displayName");
                return name != null ? name.Value : string.Empty;
            }
        }

        public RdcManServer(XElement serverElement, RdcManProperties parentProperties)
            : base(serverElement, parentProperties)
        {
        }

        public override string ToString()
        {
            return string.Format("RdcManServer:Name={0}", this.DisplayName);
        }
    }
}