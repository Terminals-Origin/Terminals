using Terminals.Connections;
using Terminals.Data;
using Terminals.Data.Interfaces;
using Terminals.Data.Validation;

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
            var favorite = new Favorite();
            favorite.AssignStores(this.groups);
            this.connectionManager.SetDefaultProtocol(favorite);
            return favorite;
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

        public IDataValidator CreateValidator()
        {
            return new FileValidations(this.connectionManager);
        }
    }
}
