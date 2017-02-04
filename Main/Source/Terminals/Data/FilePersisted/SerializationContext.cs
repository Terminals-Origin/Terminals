using System.Collections.Generic;
using System.Xml.Linq;

namespace Terminals.Data.FilePersisted
{
    internal class SerializationContext
    {
        internal FavoritesFile File { get; private set; }

        public List<XElement> Unknown { get; private set; }

        public SerializationContext()
            : this(new FavoritesFile(), new List<XElement>())
        {
        }

        public SerializationContext(FavoritesFile file, List<XElement> unknownFavorites)
        {
            this.File = file;
            this.Unknown = unknownFavorites;
        }
    }
}