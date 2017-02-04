using System.Collections.Generic;
using System.Xml.Linq;

namespace Terminals.Data.FilePersisted
{
    internal class SerializationContext
    {
        internal FavoritesFile File { get; private set; }

        public List<XElement> Unknown { get; private set; }

        public SerializationContext(FavoritesFile file)
        {
            this.File = file;
            this.Unknown = new List<XElement>();
        }
    }
}