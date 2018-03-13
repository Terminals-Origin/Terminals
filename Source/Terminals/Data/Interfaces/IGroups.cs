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
        /// If there is no group with such name, returns null. Search isn't case sensitive.
        /// Use this only to identify, if group with required name isn't already present,
        /// to prevent name duplicities.
        /// </summary>
        IGroup this[string groupName] { get; }

        /// <summary>
        /// Adds group in the persistence, if no group with its identifier exists.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Add(IGroup group);

        /// <summary>
        /// Commits group changes like rename or assignment of new parent.
        /// Doesnt handle changes in favorites members chip. For such changes see IFavorites.Update method.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Update(IGroup group);

        /// <summary>
        /// Removes the group from persistence, if it is present.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Delete(IGroup group);

        /// <summary>
        /// Removes all empty groups from persistence.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Rebuild();

        /// <summary>
        /// Adds all required favorites into this set. It would be added only, 
        /// if there is no favorite with the same id yet.
        /// </summary>
        /// <param name="toUpdate">Not null target group wher to add the favorites</param>
        /// <param name="favorites">Not null already persisted instance to add.</param>
        void AddFavorites(IGroup toUpdate, List<IFavorite> favorites);

        /// <summary>
        /// Adds required favorite into this set. It would be added only, 
        /// if there is no favorite with the same id yet.
        /// </summary>
        /// <param name="toUpdate">Not null target group wher to add the favorites</param>
        /// <param name="favorite">Not null already persisted instance to add.</param>
        void AddFavorite(IGroup toUpdate, IFavorite favorite);
    }
}