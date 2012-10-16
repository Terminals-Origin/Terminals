namespace Terminals.Data.DB
{
    /// <summary>
    /// Unified creation of Entity framework entities
    /// </summary>
    internal class Factory : IFactory
    {
        private readonly Groups groups;

        private readonly Favorites favorites;

        private readonly StoredCredentials credentials;

        private readonly DataDispatcher dispatcher;

        internal Factory(Groups groups, Favorites favorites, StoredCredentials credentials, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.dispatcher = dispatcher;
            this.favorites = favorites;
            this.credentials = credentials;
        }

        public IFavorite CreateFavorite()
        {
            var favorite = new Favorite();
            favorite.Display = new DisplayOptions();
            favorite.Security = new SecurityOptions();
            favorite.ExecuteBeforeConnect = new BeforeConnectExecute();
            favorite.AssignStores(this.groups, credentials);
            favorite.MarkAsNewlyCreated();
            return favorite;
        }

        public IGroup CreateGroup(string groupName)
        {
            // call this constructor doesnt fire the group changed event
            Group createdGroup = new Group(groupName);
            createdGroup.AssignStores(this.groups, this.dispatcher, this.favorites);
            return createdGroup;
        }

        public ICredentialSet CreateCredentialSet()
        {
            return new CredentialSet();
        }
    }
}
