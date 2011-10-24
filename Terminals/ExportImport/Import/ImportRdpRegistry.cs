using System;
using System.Collections.Generic;
using Microsoft.Win32;
using Terminals.Connections;

namespace Terminals.Integration.Import
{
    /// <summary>
    /// Loads favorites from windows registry used by the native Windows remote desktop client
    /// </summary>
    internal static class ImportRdpRegistry
    {
        /// <summary>
        /// Gets the last recently used connection registry path, relative to the HKCU
        /// HKEY_CURRENT_USER\Software\Microsoft\Terminal Server Client\Servers
        /// </summary>
        private const string REGISTRY_KEY = @"Software\Microsoft\Terminal Server Client\Servers";

        /// <summary>
        /// Reads favorites from the registry. Reads serverName, domain and user name.
        /// </summary>
        /// <returns>Not null collection of favorites. Empty collection by exception.</returns>
        internal static List<FavoriteConfigurationElement> Import()
        {
            try
            {
                RegistryKey favoritesKey = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY);
                if (favoritesKey != null)
                    return ImportFavoritesFromSubKeys(favoritesKey);
            }
            catch (Exception e)
            {
                Logging.Log.Error("Registry Import", e);
            }

            return new List<FavoriteConfigurationElement>();
        }

        private static List<FavoriteConfigurationElement> ImportFavoritesFromSubKeys(RegistryKey favoritesKey)
        {
            var registryFavorites = new List<FavoriteConfigurationElement>();
            foreach (string favoriteKeyName in favoritesKey.GetSubKeyNames())
            {
                RegistryKey favoriteKey = favoritesKey.OpenSubKey(favoriteKeyName);
                if (favoriteKey == null)
                    continue;

                registryFavorites.Add(ImportRdpKey(favoriteKey, favoriteKeyName));
            }

            return registryFavorites;
        }

        private static FavoriteConfigurationElement ImportRdpKey(RegistryKey favoriteKey, string favoriteName)
        {
            string userKey = favoriteKey.GetValue("UsernameHint").ToString();
            int slashIndex = userKey.LastIndexOf('\\');
            string domainName = userKey.Substring(0, slashIndex);
            string userName = userKey.Substring(slashIndex + 1, userKey.Length - slashIndex - 1);
            return FavoritesFactory.CreateNewFavorite(favoriteName, favoriteName,
                ConnectionManager.RDPPort, domainName, userName);
        }
    }
}
