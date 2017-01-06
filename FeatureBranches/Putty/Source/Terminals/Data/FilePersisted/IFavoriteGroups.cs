using System;
using System.Collections.Generic;

namespace Terminals.Data
{
    /// <summary>
    /// Favorite accessor to its groups.
    /// </summary>
    internal interface IFavoriteGroups
    {
        /// <summary>
        /// Returns not null collection of groups, the favorite belonges to.
        /// </summary>
        List<IGroup> GetGroupsContainingFavorite(Guid favoriteId);
    }
}