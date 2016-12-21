using Terminals.Data.Interfaces;

namespace Terminals.Data
{
    /// <summary>
    /// Provides access to create persisted item from one place
    /// </summary>
    internal interface IFactory
    {
        /// <summary>
        /// Creates new, not configured instance of connection favorite. Does not add it to the persistence.
        /// </summary>
        /// <returns>Not null newly created instance</returns>
        IFavorite CreateFavorite();

        /// <summary>
        /// Creates new empty, not configured group. Does not add it to the persistence.
        /// </summary>
        /// <param name="groupName">New name to assign</param>
        /// <returns>
        /// Not null, newly created group
        /// </returns>
        IGroup CreateGroup(string groupName); 
       
        /// <summary>
        /// Creates new empty credentials item. Does not add it to the persistence.
        /// </summary>
        /// <returns>Not null newly created instance</returns>
        ICredentialSet CreateCredentialSet();

        /// <summary>
        /// Creates data validator conform with its persistence rules.
        /// </summary>
        /// <returns>Not null newly created instance</returns>
        IDataValidator CreateValidator();
    }
}
