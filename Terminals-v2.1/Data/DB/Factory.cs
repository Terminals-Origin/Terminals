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
            var newGroup = new Group();
            newGroup.Name = groupName;
            if (favorites != null)
                // todo stack overflaw in case of Untagged group
                newGroup.AddFavorites(favorites);

            return newGroup;
        }

        public ICredentialSet CreateCredentialSet()
        {
            throw new NotImplementedException();
            //return new Data.DB.CredentialSet();
        }
    }
}
