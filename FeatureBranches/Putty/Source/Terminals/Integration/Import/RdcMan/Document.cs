using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Terminals.Integration.Import.RdcMan
{
    internal class Document
    {
        private readonly XElement root;

        private readonly Properties properties;

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

        internal IEnumerable<Group> Groups
        {
            get
            {
                return this.File.GetGroupElements()
                    .Select(group => new Group(group, this.properties));
            }
        }

        internal IEnumerable<Server> Servers
        {
            get
            {
                return this.File.GetServerElements()
                    .Select(server => new Server(server, this.properties));
            }
        }

        private XElement File
        {
            get
            {
                return this.root.GetFileElement();
            }
        }

        public Document(string fileName)
        {
            var document = XDocument.Load(fileName);
            this.root = document.Root;
            XElement propertiesElement = this.File.GetPropertiesElement();
            this.properties = new Properties(propertiesElement);
        }

        public override string ToString()
        {
            return string.Format("RdcManDocument:Is2.2={0},Groups={1},Servers={2}",
                this.IsVersion22, this.Groups.Count(), this.Servers.Count());
        }
    }
}