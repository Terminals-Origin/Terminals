using System;
using System.Collections.Generic;

namespace Terminals.Data
{
    /// <summary>
    /// Set of connection favorites used in simillar meaning like directories in operation system.
    /// Allows logical organization of favorites.
    /// </summary>
    internal interface IGroup
    {
        /// <summary>
        /// Gets or sets the unique identifier of the group.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of group in which this group is listed.
        /// By default set to Guid.Empty, which means, that it isnt listed no where 
        /// and will appear as one of root folders in first level of favorites tree.
        /// </summary>
        Guid Parent { get; set; }

        /// <summary>
        /// Gets or sets the unique name of the set.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets list of connection favorites listed in this group. 
        /// To change this collection use AddFavorite or RemoveFavorite methods.
        /// </summary>
        List<IFavorite> Favorites { get; }

        /// <summary>
        /// Adds required favorite into this set. It would be added only, 
        /// if there is no favorite with the same id yet.
        /// </summary>
        /// <param name="favorite">Not null already persisted instance to add.</param>
        void AddFavorite(IFavorite favorite);

        /// <summary>
        /// Adds all required favorites into this set. It would be added only, 
        /// if there is no favorite with the same id yet.
        /// </summary>
        /// <param name="favorite">Not null already persisted instance to add.</param>
        void AddFavorites(List<IFavorite> favorite);

        /// <summary>
        /// Removes required favorite from set of favorites. If there is no favorite with the same Id,
        /// than nothing happens.
        /// </summary>
        /// <param name="favorite">Not null instance to remove. This instance doesnt have to be peristed</param>
        void RemoveFavorite(IFavorite favorite);

        /// <summary>
        /// Removes all required favorites from set of favorites. If there is no favorite with the same Id,
        /// than nothing happens.
        /// </summary>
        /// <param name="favorite">Not null instance to remove. This instance doesnt have to be peristed</param>
        void RemoveFavorites(List<IFavorite> favorite);
    }
}
