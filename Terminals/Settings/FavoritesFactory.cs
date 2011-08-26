using System;
using System.Collections.Generic;
using System.Linq;
using Terminals.Configuration;
using Terminals.Connections;

namespace Terminals
{
    /// <summary>
    /// Provides unified creation of Favorites
    /// </summary>
    internal static class FavoritesFactory
    {
        internal static readonly String TerminalsReleasesFavoriteName = Program.Resources.GetString("TerminalsNews");

        internal static FavoriteConfigurationElement GetOrCreateReleaseFavorite()
        {
            List<FavoriteConfigurationElement> favorites = Settings.GetFavorites().ToList();
            FavoriteConfigurationElement release = favorites
                .FirstOrDefault(candidate => candidate.Name == TerminalsReleasesFavoriteName);

            if (release == null)
            {
                release = new FavoriteConfigurationElement(TerminalsReleasesFavoriteName);
                release.Url = "http://terminals.codeplex.com";
                release.Tags = Program.Resources.GetString("Terminals");
                release.Protocol = ConnectionManager.HTTP;
                Settings.AddFavorite(release, false);
            }
            return release;
        }

        internal static FavoriteConfigurationElement GetOrCreateQuickConnectFavorite(String server,
            Boolean ConnectToConsole, Int32 port)
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
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
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
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
    }
}
