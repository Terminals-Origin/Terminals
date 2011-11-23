using System;
using System.Collections.Generic;
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
    internal static class FavoritesFactory
    {
        private const string DISCOVERED_CONNECTIONS = "Discovered Connections";

        internal static readonly String TerminalsReleasesFavoriteName = Program.Resources.GetString("TerminalsNews");

        private static Favorites PersistedFavorites
        {
            get { return Persistance.Instance.Favorites; }
        }

        internal static FavoriteConfigurationElement GetOrCreateReleaseFavorite()
        {
            List<FavoriteConfigurationElement> favorites = PersistedFavorites.GetFavorites().ToList();
            FavoriteConfigurationElement release = favorites
                .FirstOrDefault(candidate => candidate.Name == TerminalsReleasesFavoriteName);

            if (release == null)
            {
                release = new FavoriteConfigurationElement(TerminalsReleasesFavoriteName);
                release.Url = "http://terminals.codeplex.com";
                release.Tags = Program.Resources.GetString("Terminals");
                release.Protocol = ConnectionManager.HTTP;
                PersistedFavorites.AddFavorite(release);
            }
            return release;
        }

        internal static FavoriteConfigurationElement GetOrCreateQuickConnectFavorite(String server,
            Boolean ConnectToConsole, Int32 port)
        {
            FavoriteConfigurationElementCollection favorites = PersistedFavorites.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[server];
            if (favorite != null)
            {
                favorite.ConnectToConsole = ConnectToConsole;
            }
            else  //create a temporaty favorite and connect to it
            {
                favorite = new FavoriteConfigurationElement();
                favorite.ConnectToConsole = ConnectToConsole;
                favorite.ServerName = server;
                favorite.Name = server;

                if (port != 0)
                    favorite.Port = port;
            }
            return favorite;
        }

        internal static FavoriteConfigurationElement GetFavoriteUpdatedCopy(String connectionName,
            Boolean forceConsole, Boolean forceNewWindow, CredentialSet credential)
        {
            FavoriteConfigurationElementCollection favorites = PersistedFavorites.GetFavorites();
            FavoriteConfigurationElement favorite = favorites[connectionName];
            if (favorite == null)
                return null;

            favorite = (FavoriteConfigurationElement)favorite.Clone();

            if (forceConsole)
                favorite.ConnectToConsole = true;

            if (forceNewWindow)
                favorite.NewWindow = true;

            if (credential != null)
            {
                favorite.Credential = credential.Name;
                favorite.UserName = credential.Username;
                favorite.DomainName = credential.Domain;
                favorite.EncryptedPassword = credential.Password;
            }
            return favorite;
        }

        internal static FavoriteConfigurationElement CreateNewFavorite(string favoriteName, string server, int port,
            string domain, string userName)
        {
            FavoriteConfigurationElement newFavorite = new FavoriteConfigurationElement();
            newFavorite.Name = favoriteName;
            newFavorite.ServerName = server;
            newFavorite.UserName = userName;
            newFavorite.DomainName = domain;
            newFavorite.Tags = DISCOVERED_CONNECTIONS;
            newFavorite.Port = port;
            newFavorite.Protocol = ConnectionManager.GetPortName(port, true);
            return newFavorite;
        }

        internal static FavoriteConfigurationElement CreateNewFavorite(string favoriteName, string server, int port)
        {
            string name = GetHostName(server, favoriteName, port);
            string domainName = GetCurrentDomainName(server);
            return CreateNewFavorite(name, server, port, domainName, Environment.UserName);
        }

        private static string GetCurrentDomainName(string server)
        {
            if (Environment.UserDomainName != Environment.MachineName)
                return Environment.UserDomainName;

            return server;
        }

        private static string GetHostName(string server, string name, int port)
        {
            try
            {
                IPAddress address;
                if (IPAddress.TryParse(server, out address))
                    name = Dns.GetHostEntry(address).HostName;

                string portName = ConnectionManager.GetPortName(port, true);
                return string.Format("{0}_{1}", name, portName);
            }
            catch //lets not log dns lookups!
            {
                return name;
            }
        }
    }
}
