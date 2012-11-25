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
        /// Removes the group from persistence, if it is present.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Delete(IGroup group);

        /// <summary>
        /// Removes all empty groups from persistence.
        /// Saves changes, if delayed save isn't required.
        /// </summary>
        void Rebuild();
    }
}
