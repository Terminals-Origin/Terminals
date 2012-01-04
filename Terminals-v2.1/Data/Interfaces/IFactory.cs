using System;

namespace Terminals.Data
{
    /// <summary>
    /// Provides access to create persisted item from one place
    /// </summary>
    internal interface IFactory
    {
        /// <summary>
        /// Gets group with required groupName or creates new group which isnt added to persistance yet.
        /// </summary>
        /// <param name="groupName">Name of the group to search in persistance.</param>
        /// <returns>Not null value of Group obtained from persistance or newly created group</returns>
        IGroup GetOrCreateGroup(string groupName);

        /// <summary>
        /// Creates new empty, not configured group.
        /// </summary>
        /// <param name="groupName">New name to assign</param>
        /// <returns>Not null, newly created group</returns>
        IGroup CreateGroup(string groupName);

        /// <summary>
        /// Creates new, not configured instance of connection favorite.
        /// </summary>
        /// <returns>Not null newly created instance</returns>
        IFavorite CreateFavorite();

        /// <summary>
        /// Gets connection favorite, with name of Terminals release constant.
        /// </summary>
        /// <returns>Not null, configured instance of connection favorite,
        /// which points to the terminals web site</returns>
        IFavorite GetOrCreateReleaseFavorite();

        /// <summary>
        /// Gets persisted favorite, if there is a favorite named by server parameter.
        /// If no favorite is found creates new favorite, which is configured by paramter properties
        /// and point to RDP server.
        /// </summary>
        /// <param name="server">the RDP server name</param>
        /// <param name="connectToConsole">Flag used for ConnectToConsole RDP option</param>
        /// <param name="port">Number of port, which RDP service is lisening on server "server"</param>
        /// <returns></returns>
        IFavorite GetOrCreateQuickConnectFavorite(String server, Boolean connectToConsole, Int32 port);
    }
}
