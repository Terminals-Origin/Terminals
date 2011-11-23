namespace Terminals.Data
{
    internal class FilePersistance
    {
        internal Favorites Favorites { get; private set; }
        internal FavoriteGroups Groups { get; private set; }

        internal FilePersistance(DataDispatcher dispatcher)
        {
            this.Favorites = new Favorites(dispatcher);
            this.Groups = new FavoriteGroups(dispatcher);
        }
    }
}
