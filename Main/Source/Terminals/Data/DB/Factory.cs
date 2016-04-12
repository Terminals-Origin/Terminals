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

        internal Factory(Groups groups, Favorites favorites, 
            StoredCredentials credentials, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.favorites = favorites;
            this.credentials = credentials;
            this.dispatcher = dispatcher;
        }

        public IFavorite CreateFavorite()
        {
            return CreateFavorite(this.groups, this.credentials, this.dispatcher);
        }

        internal static DbFavorite CreateFavorite(Groups groups, StoredCredentials credentials, DataDispatcher dispatcher)
        {
            var favorite = new DbFavorite();
            favorite.Display = new DbDisplayOptions();
            favorite.Security = new DbSecurityOptions();
            favorite.ExecuteBeforeConnect = new DbBeforeConnectExecute();
            favorite.AssignStores(groups, credentials, dispatcher);
            favorite.MarkAsNewlyCreated();
            return favorite;
        }

        public IGroup CreateGroup(string groupName)
        {
            // call this constructor doesn't fire the group changed event
            DbGroup createdGroup = new DbGroup(groupName);
            createdGroup.AssignStores(this.groups, this.dispatcher, this.favorites);
            return createdGroup;
        }

        public ICredentialSet CreateCredentialSet()
        {
            return new DbCredentialSet();
        }
    }
}
