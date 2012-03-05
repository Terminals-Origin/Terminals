using System;
using System.Collections.Generic;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Unified creation of Entity framework entities
    /// </summary>
    internal class Factory : IFactory
    {
        public IFavorite CreateFavorite()
        {
            var favorite = new Favorite();
            favorite.Display = new DisplayOptions();
            favorite.Security = new SecurityOptions();
            favorite.ExecuteBeforeConnect = new BeforeConnectExecute();

            return favorite;
        }

        public IGroup CreateGroup(string groupName, List<IFavorite> favorites = null)
        {
            // call this constructor doesnt fire the group changed event
            if (favorites == null)
                return new Group(groupName, new List<IFavorite>());

            return new Group(groupName, favorites);
        }

        public ICredentialSet CreateCredentialSet()
        {
            throw new NotImplementedException();
            //return new Data.DB.CredentialSet();
        }
    }
}
