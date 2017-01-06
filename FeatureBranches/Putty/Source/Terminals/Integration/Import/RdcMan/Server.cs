using System.Xml.Linq;

namespace Terminals.Integration.Import.RdcMan
{
    internal class Server : Properties 
    {
        internal string DisplayName
        {
            get
            {
                XElement name = this.PropertiesElement.GetDisplayNameElement();
                return name != null ? name.Value : string.Empty;
            }
        }

        public Server(XElement serverElement, Properties parentProperties)
            : base(serverElement, parentProperties)
        {
        }

        public override string ToString()
        {
            return string.Format("RdcManServer:Name={0}", this.DisplayName);
        }
    }
}