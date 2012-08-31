using System.Collections.Generic;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Unified creation of Entity framework entities
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
            var favorite = new Favorite();
            favorite.MarkAsNewlyCreated();
            favorite.Display = new DisplayOptions();
            favorite.Security = new SecurityOptions();
            favorite.ExecuteBeforeConnect = new BeforeConnectExecute();

            return favorite;
        }

        public IGroup CreateGroup(string groupName)
        {
            // call this constructor doesnt fire the group changed event
            Group createdGroup = new Group(groupName);
            createdGroup.Groups = this.groups;
            return createdGroup;
        }

        public ICredentialSet CreateCredentialSet()
        {
            return new CredentialSet();
        }
    }
}
