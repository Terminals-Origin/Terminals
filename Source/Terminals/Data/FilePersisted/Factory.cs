using Terminals.Data;

namespace Terminals
{
    /// <summary>
    /// Provides unified creation of Favorites
    /// </summary>
    internal class Factory : IFactory
    {
        private readonly PersistenceSecurity persistenceSecurity;

        private readonly Groups groups;

        private readonly DataDispatcher dispatcher;

        internal Factory(PersistenceSecurity persistenceSecurity, Groups groups, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.dispatcher = dispatcher;
            this.persistenceSecurity = persistenceSecurity;
        }

        public IFavorite CreateFavorite()
        {
            var newItem = new Favorite();
            newItem.AssignStores(this.persistenceSecurity, this.groups);
            return newItem;
        }

        public IGroup CreateGroup(string groupName)
        {
            var newGroup = new Group(groupName);
            newGroup.AssignStores(this.groups, this.dispatcher);
            return newGroup;
        }

        public ICredentialSet CreateCredentialSet()
        {
            var newCredentials = new CredentialSet();
            newCredentials.AssignStore(persistenceSecurity);
            return newCredentials;
        }
    }
}
