using System.Collections.Generic;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Unified creation of Entity framework entities
    /// </summary>
    internal class Factory : IFactory
    {
        private Database database;

        internal Factory(Database database)
        {
            this.database = database;
        }

        public IFavorite CreateFavorite()
        {
            var favorite = new Favorite();
            favorite.Database = this.database;
            favorite.Display = new DisplayOptions();
            favorite.Security = new SecurityOptions();
            favorite.ExecuteBeforeConnect = new BeforeConnectExecute();

            return favorite;
        }

        public IGroup CreateGroup(string groupName, List<IFavorite> favorites = null)
        {
            if (favorites == null)
                favorites = new List<IFavorite>();
            
            // call this constructor doesnt fire the group changed event
            Group createdGroup = new Group(groupName, favorites);
            createdGroup.Database = this.database;
            return createdGroup;
        }

        public ICredentialSet CreateCredentialSet()
        {
            return new CredentialSet();
        }
    }
}
