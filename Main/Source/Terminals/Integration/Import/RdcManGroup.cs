using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManGroup : RdcManProperties
    {
        private readonly XElement groupElement;

        internal IEnumerable<RdcManGroup> Groups
        {
            get
            {
                return this.groupElement.Elements("group")
                    .Select(group => new RdcManGroup(group, this));
            }
        }

        internal IEnumerable<RdcManServer> Servers
        {
            get
            {
                return this.groupElement.Elements("server")
                    .Select(server => new RdcManServer(server, this));
            }
        }

        public RdcManGroup(XElement groupElement, RdcManProperties parentProperties)
            : base(groupElement.Element("properties"), parentProperties)
        {
            this.groupElement = groupElement;
        }

        public override string ToString()
        {
            return string.Format("RdcManGroup:Name={0},Groups={1},Servers={2}",
                this.Name, this.Groups.Count(), this.Servers.Count());
        }
    }
}