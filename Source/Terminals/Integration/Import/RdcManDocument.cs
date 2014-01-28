using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManDocument
    {
        private readonly XElement root;

        /// <summary>
        /// Gets true, if the file contains version element with value "2.2".
        /// Other versions were not tested.
        /// </summary>
        internal bool IsVersion22
        {
            get
            {
                XElement version = this.root.Element("version");
                return version != null && version.Value == "2.2";
            }
        }

        internal RdcManProperties Properties
        {
            get
            {
                XElement current = this.File.Element("properties");
                return new RdcManProperties(current);
            }
        }

        internal IEnumerable<RdcManGroup> Groups
        {
            get
            {
                return this.File.Elements("group")
                    .Select(group => new RdcManGroup(group));
            }
        }

        private XElement File
        {
            get
            {
                return this.root.Element("file");
            }
        }

        public RdcManDocument(XElement root)
        {
            this.root = root;
        }

        public override string ToString()
        {
            return string.Format("RdcManDocument:Is2.2={0},Groups={1}",
                this.IsVersion22, this.Groups.Count());
        }
    }
}