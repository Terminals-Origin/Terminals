using System;
using System.Collections.Generic;

namespace Terminals.Network
{
    internal class ShareFavoritesEventArgs: EventArgs
    {
        internal string UserName { get; set; }
        internal List<FavoriteConfigurationElement> Favorites { get; private set; }

        internal ShareFavoritesEventArgs(List<FavoriteConfigurationElement> favorites)
        {
            this.Favorites = favorites;
        }
    }
}
