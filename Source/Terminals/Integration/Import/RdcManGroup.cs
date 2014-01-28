using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    internal class RdcManGroup
    {
        private readonly XElement group;

        public RdcManGroup(XElement group)
        {
            this.group = group;
        }
    }
}