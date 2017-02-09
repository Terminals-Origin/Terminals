namespace Terminals.Data.FilePersisted
{
    internal class SerializationContext
    {
        internal FavoritesFile File { get; private set; }

        internal UnknonwPluginElements Unknown { get; private set; }

        public SerializationContext()
            : this(new FavoritesFile(), new UnknonwPluginElements())
        {
        }

        public SerializationContext(FavoritesFile file, UnknonwPluginElements unknown)
        {
            this.File = file;
            this.Unknown = unknown;
        }

        public override string ToString()
        {
            int favoritesCount = this.File.Favorites.Length;
            int unknownCount = this.Unknown.Favorites.Count;
            return string.Format("SerializationContext:Unknowns='{0}',Known='{1}'", unknownCount, favoritesCount);
        }
    }
}