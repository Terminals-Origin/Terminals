using System;
using System.Collections.Generic;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals
{
    /// <summary>
    /// Provides unified creation of Favorites
    /// </summary>
    internal class Factory : IFactory
    {
        internal Factory() { }

        public IFavorite CreateFavorite()
        {
            return new Favorite();
        }

        public IGroup CreateGroup(string groupName, List<IFavorite> favorites = null)
        {
            if (favorites == null)
                return new Group(groupName, new List<IFavorite>());

            return new Group(groupName, favorites);
        }

        public ICredentialSet CreateCredentialSet()
        {
            return new CredentialSet();
        }
    }
}
