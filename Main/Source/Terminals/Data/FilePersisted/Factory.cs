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

        public IGroup CreateGroup(string groupName)
        {
            return new Group(groupName);
        }

        public ICredentialSet CreateCredentialSet()
        {
            return new CredentialSet();
        }
    }
}
