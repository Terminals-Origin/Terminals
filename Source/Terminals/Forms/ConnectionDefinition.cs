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
        /// Gets new favorite name to be created instaed of opening connection.
        /// This happens in a case favorite with required name is not present in persistence.
        /// </summary>
        internal string NewFavorite { get; private set; }

        /// <summary>
        /// Creates new instance of parameters required to open new connection
        /// </summary>
        /// <param name="favorite">Not null favorite, where to connect</param>
        /// <param name="forceConsole">For RDP only. If defined, will replace the RDP Console option</param>
        /// <param name="forceNewWindow">If defined, will replace the favorite option to open connection in new window</param>
        /// <param name="credentials">If defined, replaces the favorite authentication informations</param>
        internal ConnectionDefinition(IFavorite favorite, bool? forceConsole = null,
            bool? forceNewWindow = null, ICredentialSet credentials = null)
            : this(new List<IFavorite>() { favorite }, forceConsole, forceNewWindow, credentials)
        {
        }

        /// <summary>
        /// Creates new instance of parameters required to open new connection
        /// </summary>
        /// <param name="favorites">Not null colleciton of favorites, where to connect</param>
        /// <param name="forceConsole">For RDP only. If defined, will replace the RDP Console option</param>
        /// <param name="forceNewWindow">If defined, will replace the favorite option to open connection in new window</param>
        /// <param name="credentials">If defined, replaces the favorite authentication informations</param>
        internal ConnectionDefinition(IEnumerable<IFavorite> favorites, bool? forceConsole = null,
            bool? forceNewWindow = null, ICredentialSet credentials = null, string newFavorite = "")
        {
            this.Favorites = favorites.ToList(); // evaluate imediately
            this.ForceConsole = forceConsole;
            this.ForceNewWindow = forceNewWindow;
            this.Credentials = credentials;
            this.NewFavorite = newFavorite;
        }

        public override string ToString()
        {
            return string.Format("ConenctionDefinition:Favorites={0},Console={1},NewWindow={2},NewFavorite={3}",
                this.Favorites.Count(), this.ForceConsole, this.ForceNewWindow, this.NewFavorite);
        }
    }
}