using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManDocument
    {
        private readonly XElement root;

        private readonly RdcManProperties properties;

        /// <summary>
        /// Gets true, if the file contains version element with value "2.2".
        /// Other versions were not tested.
        /// </summary>
        internal bool IsVersion22
        {
            get
            {
                XElement version = this.root.GetVersionElement();
                return version != null && version.Value == "2.2";
            }
        }

        internal IEnumerable<RdcManGroup> Groups
        {
            get
            {
                return this.File.Elements("group")
                    .Select(group => new RdcManGroup(group, this.properties));
            }
        }

        internal IEnumerable<RdcManServer> Servers
        {
            get
            {
                return this.File.GetServerElements()
                    .Select(server => new RdcManServer(server, this.properties));
            }
        }

        private XElement File
        {
            get
            {
                return this.root.GetFileElement();
            }
        }

        public RdcManDocument(string fileName)
        {
            XDocument document = XDocument.Load(fileName);
            this.root = document.Root;
            XElement propertiesElement = this.File.Element("properties");
            this.properties = new RdcManProperties(propertiesElement);
        }

        public override string ToString()
        {
            return string.Format("RdcManDocument:Is2.2={0},Groups={1},Servers={2}",
                this.IsVersion22, this.Groups.Count(), this.Servers.Count());
        }
    }
}