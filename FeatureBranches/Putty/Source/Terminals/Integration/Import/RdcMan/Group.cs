using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Terminals.Integration.Import.RdcMan
{
    internal class Group : Properties
    {
        private readonly XElement groupElement;

        internal IEnumerable<Group> Groups
        {
            get
            {
                return this.groupElement.GetGroupElements()
                    .Select(group => new Group(group, this));
            }
        }

        internal IEnumerable<Server> Servers
        {
            get
            {
                return this.groupElement.GetServerElements()
                    .Select(server => new Server(server, this));
            }
        }

        public Group(XElement groupElement, Properties parentProperties)
            : base(groupElement.GetPropertiesElement(), parentProperties)
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