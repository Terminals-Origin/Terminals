﻿using System;
using System.Collections.Generic;

namespace Terminals.Data
{
    /// <summary>
    /// Part of application data store, which handles favorites groups collection
    /// </summary>
    internal interface IGroups: IEnumerable<IGroup>
    {
        /// <summary>
        /// Gets group by its name. If there are more than one with this name returns the first found.
        /// If there is no group with such name, returns null. Search isnt case sensitive.
        /// Use this only to identify, if group with required name isnt already present,
        /// to prevent name duplicities.
        /// </summary>
        IGroup this[string groupName] { get; }

        /// <summary>
        /// Gets the stored group identified by its unique identifier.
        /// If no group exists with provided identifier, than returns null.
        /// </summary>
        IGroup this[Guid groupId] { get; }

        /// <summary>
        /// Adds group in the persistance, if no group with its identifier exists.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void Add(IGroup group);

        /// <summary>
        /// Removes the group from persistance, if it is present.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void Delete(IGroup group);

        List<IGroup> GetGroupsContainingFavorite(Guid favoriteId);

        /// <summary>
        /// Updates favorite in all groups defined by groupNames collection.
        /// If favorite is present in group not listed in groupNames it will be removed,
        /// if it isnt present, but is listed in groupNames, then it will be add to the group.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        /// <param name="favorite">Not null item to update.</param>
        /// <param name="groups">New collection of groups in which the favorite should be listed.</param>
        void UpdateFavoriteInGroups(IFavorite favorite, List<IGroup> groups);

        /// <summary>
        /// Removes all empty groups from persistance.
        /// Saves changes, if delayed save isnt required.
        /// </summary>
        void Rebuild();
    }
}
