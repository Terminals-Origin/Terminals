using System;
using System.Net;
using Terminals.Configuration;
using Terminals.Connections;

namespace Terminals.Data
{
    internal static class ConfigFavoritesFactory
    {
        private const string DISCOVERED_CONNECTIONS = "Discovered Connections";

        private static IFavorites PersistedFavorites
        {
            get { return Persistance.Instance.Favorites; }
        }

        internal static IFavorite GetFavoriteUpdatedCopy(String connectionName,
            Boolean forceConsole, Boolean forceNewWindow, CredentialSet credential)
        {
            IFavorite favorite = PersistedFavorites[connectionName];
            if (favorite == null)
                return null;

            favorite = favorite.Copy();

            if (forceConsole)
            {
                var rdpOptions = favorite.ProtocolProperties as RdpOptions;
                if (rdpOptions != null)
                    rdpOptions.ConnectToConsole = true;
            }

            if (forceNewWindow)
                favorite.NewWindow = true;

            if (credential != null)
            {
                var security = favorite.Security;
                security.Credential = credential.Name;
                security.UserName = credential.Username;
                security.DomainName = credential.Domain;
                security.EncryptedPassword = credential.Password;
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
