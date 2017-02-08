using System.Collections.Generic;
using System.Xml.Linq;

namespace Terminals.Data.FilePersisted
{
    internal class UnknonwPluginElements
    {
        public List<XElement> Favorites { get; private set; }

        public Dictionary<string, List<XElement>> GroupMembership { get; private set; }

        public UnknonwPluginElements()
            : this(new List<XElement>(), new Dictionary<string, List<XElement>>())
        {
        }

        public UnknonwPluginElements(List<XElement> favorites, Dictionary<string, List<XElement>> groupMembership)
        {
            this.Favorites = favorites;
            this.GroupMembership = groupMembership;
        }
    }
}