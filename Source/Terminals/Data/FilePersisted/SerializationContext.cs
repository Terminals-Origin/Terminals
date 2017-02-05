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

        public override string ToString()
        {
            int favoritesCount = this.File.Favorites.Length;
            int unknownCount = this.Unknown.Count;
            return string.Format("SerializationContext:Unknowns='{0}',Known='{1}'", unknownCount, favoritesCount);
        }
    }
}