using System;
using System.Collections.Generic;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal class FavoritesFoundEventArgs : EventArgs
    {
        internal List<IFavorite> Favorites { get; private set; }

        internal FavoritesFoundEventArgs(List<IFavorite> favorites)
        {
            this.Favorites = favorites;
        }
    }
}