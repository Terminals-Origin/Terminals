using System;
using System.Collections.Generic;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Virtual menu item used to collect favorites without assigned any group
    /// </summary>
    internal class UntagedMenuItem : GroupMenuItem
    {
        /// <summary>
        /// Gets the default tag name for favorites without any tag
        /// </summary>
        private const String UNTAGGED_NODENAME = "Not grouped";

        internal override List<IFavorite> Favorites
        {
            get
            {
                var favorites = Persistence.Instance.Favorites;
                return FavoriteTreeListLoader.GetUntaggedFavorites(favorites);
            }
        }

        internal UntagedMenuItem()
            : base(UNTAGGED_NODENAME, true)
        {
        }
    }
}
