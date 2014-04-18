namespace Terminals.Integration.Import
{
    internal class RdcManFavoriteImporter
    {
        private readonly RdcManServer server;

        private readonly FavoriteConfigurationElement favorite;

        public RdcManFavoriteImporter(RdcManServer server)
        {
            this.server = server;
            this.favorite = new FavoriteConfigurationElement();
        }

        internal FavoriteConfigurationElement ImportServer()
        {
            this.favorite.Name = this.server.DisplayName;
            this.favorite.ServerName = this.server.Name;
            // todo import favorite content

            return this.favorite;
        }
    }
}