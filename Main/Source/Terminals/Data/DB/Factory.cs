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

        private readonly PersistenceSecurity persistenceSecurity;

        private readonly DataDispatcher dispatcher;

        internal Factory(Groups groups, Favorites favorites, StoredCredentials credentials,
            PersistenceSecurity persistenceSecurity, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.favorites = favorites;
            this.credentials = credentials;
            this.persistenceSecurity = persistenceSecurity;
            this.dispatcher = dispatcher;
        }

        public IFavorite CreateFavorite()
        {
            var favorite = new DbFavorite();
            favorite.Display = new DbDisplayOptions();
            favorite.Security = new DbSecurityOptions();
            favorite.ExecuteBeforeConnect = new DbBeforeConnectExecute();
            favorite.AssignStoreToRdpOptions(this.persistenceSecurity);
            favorite.AssignStores(this.groups, credentials, this.persistenceSecurity);
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
            var newCredential = new DbCredentialSet();
            newCredential.AssignSecurity(this.persistenceSecurity);
            return newCredential;
        }
    }
}
