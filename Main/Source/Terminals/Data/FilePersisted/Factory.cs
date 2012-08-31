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

        internal Factory(Groups groups, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.dispatcher = dispatcher;
        }

        public IFavorite CreateFavorite()
        {
            var newItem = new Favorite();
            newItem.Groups = this.groups;
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
