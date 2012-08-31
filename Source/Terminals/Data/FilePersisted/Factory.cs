using Terminals.Data;

namespace Terminals
{
    /// <summary>
    /// Provides unified creation of Favorites
    /// </summary>
    internal class Factory : IFactory
    {
        private readonly Groups groups;

        internal Factory(Groups groups)
        {
            this.groups = groups;
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
            newGroup.Groups = this.groups;
            return newGroup;
        }

        public ICredentialSet CreateCredentialSet()
        {
            return new CredentialSet();
        }
    }
}
