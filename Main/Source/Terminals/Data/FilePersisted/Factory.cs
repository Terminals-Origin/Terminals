using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals
{
    /// <summary>
    /// Provides unified creation of Favorites
    /// </summary>
    internal class Factory : IFactory
    {
        private readonly Groups groups;

        private readonly DataDispatcher dispatcher;

        private readonly ConnectionManager connectionManager;

        internal Factory(Groups groups, DataDispatcher dispatcher, ConnectionManager connectionManager)
        {
            this.groups = groups;
            this.dispatcher = dispatcher;
            this.connectionManager = connectionManager;
        }

        public IFavorite CreateFavorite()
        {
            var newItem = new Favorite();
            newItem.AssignStores(this.groups);
            connectionManager.ChangeProtocol(newItem, KnownConnectionConstants.RDP);
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
            return new CredentialSet();
        }
    }
}
