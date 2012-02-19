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
            return  new Favorite();
        }

        public IGroup CreateGroup(string groupName, List<IFavorite> favorites = null)
        {
            var newGroup = new Group();
            if (favorites != null)
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
