using System;
using System.Collections.Generic;
using System.Linq;
using Terminals.Data;

namespace Terminals.Forms
{
    /// <summary>
    /// Set of parameters required to open new connection
    /// </summary>
    internal class ConnectionDefinition
    {
        internal bool HasFavorites
        {
            get { return this.Favorites.Any(); }
        }

        /// <summary>
        /// Gets not null list of favorites where to connect
        /// </summary>
        internal IEnumerable<IFavorite> Favorites { get; private set; }

        /// <summary>
        /// Gets optional argument for possiblity to override Console session type.
        /// It is relevant for RDP connections only.
        /// </summary>
        internal bool? ForceConsole { get; private set; }

        /// <summary>
        /// Gets optional argument for possiblity to open connection in a new window.
        /// It is relevant for RDP connections only.
        /// </summary>
        internal bool? ForceNewWindow { get; private set; }

        /// <summary>
        /// Gets optional argument for possibility to override favorites authentication properties.
        /// </summary>
        internal ICredentialSet Credentials { get; private set; }

        /// <summary>
        /// Creates new instance of parameters required to open new connection
        /// </summary>
        /// <param name="favoriteName">Not null favorite name, where to connect</param>
        /// <param name="forceConsole">For RDP only. If defined, will replace the RDP Console option</param>
        /// <param name="forceNewWindow">If defined, will replace the favorite option to open connection in new window</param>
        internal ConnectionDefinition(string favoriteName, bool? forceConsole = null, bool? forceNewWindow = null)
            : this(new List<string>() { favoriteName }, forceConsole, forceNewWindow)
        {
        }

        /// <summary>
        /// Creates new instance of parameters required to open new connection
        /// </summary>
        /// <param name="favoriteNames">Not null colleciton of favorite names, where to connect</param>
        /// <param name="forceConsole">For RDP only. If defined, will replace the RDP Console option</param>
        /// <param name="forceNewWindow">If defined, will replace the favorite option to open connection in new window</param>
        /// <param name="credentials">If defined, replaces the favorite authentication informations</param>
        internal ConnectionDefinition(IEnumerable<string> favoriteNames, bool? forceConsole = null, bool? forceNewWindow = null, ICredentialSet credentials = null)
        {
            var favorites = Persistence.Instance.Favorites
                .Where(favorite => favoriteNames.Contains(favorite.Name, StringComparer.InvariantCultureIgnoreCase));
            this.AssignFields(favorites, forceConsole, forceNewWindow, credentials);
        }

        /// <summary>
        /// Creates new instance of parameters required to open new connection
        /// </summary>
        /// <param name="favorites">Not null colleciton of favorites, where to connect</param>
        /// <param name="forceConsole">For RDP only. If defined, will replace the RDP Console option</param>
        /// <param name="forceNewWindow">If defined, will replace the favorite option to open connection in new window</param>
        /// <param name="credentials">If defined, replaces the favorite authentication informations</param>
        internal ConnectionDefinition(IEnumerable<IFavorite> favorites, bool? forceConsole = null, bool? forceNewWindow = null, ICredentialSet credentials = null)
        {
            this.AssignFields(favorites, forceConsole, forceNewWindow, credentials);
        }

        private void AssignFields(IEnumerable<IFavorite> favorites, bool? forceConsole, bool? forceNewWindow, ICredentialSet credentials)
        {
            this.Favorites = favorites.ToList(); // evaluate imediately
            this.ForceConsole = forceConsole;
            this.ForceNewWindow = forceNewWindow;
            this.Credentials = credentials;
        }
    }
}