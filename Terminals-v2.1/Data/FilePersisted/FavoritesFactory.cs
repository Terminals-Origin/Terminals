using System;
using System.Linq;
using System.Net;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals
{
    /// <summary>
    /// Provides unified creation of Favorites
    /// </summary>
    internal class FavoritesFactory : IFactory
    {
        internal static readonly String terminalsReleasesFavoriteName = Program.Resources.GetString("TerminalsNews");
        internal static string TerminalsReleasesFavoriteName
        {
            get { return terminalsReleasesFavoriteName; }
        }

        private IFavorites favorites;
        private IGroups groups;

        internal FavoritesFactory(Groups groups, Favorites favorites)
        {
            this.groups = groups;
            this.favorites = favorites;
        }

        public IFavorite GetOrCreateReleaseFavorite()
        {
            IFavorite release = this.favorites
                .FirstOrDefault(candidate => candidate.Name == TerminalsReleasesFavoriteName);

            if (release == null)
            {
                release = CreateFavorite();
                release.Name = TerminalsReleasesFavoriteName;
                release.ServerName = "terminals.codeplex.com";
                release.Protocol = ConnectionManager.HTTP;
                this.favorites.Add(release);

                string terminalsGroupName = Program.Resources.GetString("Terminals");
                IGroup group = GetOrCreateGroup(terminalsGroupName);
                group.AddFavorite(release);
            }
            return release;
        }

        public IFavorite CreateFavorite()
        {
            return new Favorite();
        }

        public IGroup GetOrCreateGroup(string groupName)
        {
            IGroup group = groups[groupName];
            if (group == null)
            {
                group = CreateGroup(groupName);
                groups.Add(group);
            }

            return group;
        }

        public IGroup CreateGroup(string groupName)
        {
            return new Group { Name = groupName };
        }

        public IFavorite GetOrCreateQuickConnectFavorite(String server,
            Boolean connectToConsole, Int32 port)
        {
            IFavorite favorite = this.favorites[server];
            if (favorite == null) //create a temporaty favorite and connect to it
            {
                favorite = CreateFavorite();
                favorite.ServerName = server;
                favorite.Name = server;

                if (port != 0)
                    favorite.Port = port;
            }

            var rdpProperties = favorite.ProtocolProperties as RdpOptions;
            if (rdpProperties != null)
                rdpProperties.ConnectToConsole = connectToConsole;

            return favorite;
        }

        public ICredentialSet CreateCredentialSet()
        {
            return new CredentialSet();
        }
    }
}
