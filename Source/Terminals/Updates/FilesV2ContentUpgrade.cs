using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Converters;
using Terminals.Data;
using Terminals.Forms.Controls;

namespace Terminals.Updates
{
    /// <summary>
    /// Upgrades content of credentials and config file from v2 to version 3.0
    /// </summary>
    internal class FilesV2ContentUpgrade
    {
        private readonly IPersistence persistence;

        private readonly Settings settings = Settings.Instance;

        private readonly PasswordsV2Update passwordsUpdate;

        private AuthenticationPrompt prompt;

        private readonly ConnectionManager connectionManager;

        /// <summary>
        /// Initialize new instance of the upgrade providing fresh not initialized persistence and password prompt.
        /// </summary>
        /// <param name="persistence">Not null,not authenticated, not initialized persistence</param>
        /// <param name="knowsUserPassword">Not null password prompt to obtain current master password from user</param>
        internal FilesV2ContentUpgrade(IPersistence persistence, ConnectionManager connectionManager, Func<bool, AuthenticationPrompt> knowsUserPassword)
        {
            this.persistence = persistence;
            this.connectionManager = connectionManager;

            // prevents ask for password two times
            this.passwordsUpdate = new PasswordsV2Update(retry =>
                {
                    this.prompt = knowsUserPassword(retry);
                    return this.prompt;
                });
        }

        internal void Run()
        {
            this.UpgradeCredentialsFile();
            this.UpgradeConfigFile();
        }

        private void UpgradeCredentialsFile()
        {
            string credentialsFile = settings.FileLocations.Credentials;
            if (File.Exists(credentialsFile))
            {
                string credentialsXml = File.ReadAllText(credentialsFile);
                string updatedContet = UpdateCredentialXmlTags(credentialsXml);
                updatedContet = this.passwordsUpdate.UpdateCredentialPasswords(updatedContet);
                File.WriteAllText(credentialsFile, updatedContet);
            }
        }

        private static string UpdateCredentialXmlTags(string credentialsXml)
        {
            StringBuilder credentialsText = new StringBuilder(credentialsXml);
            credentialsText.Replace("Password", "EncryptedPassword");
            credentialsText.Replace("Username", "EncryptedUserName");
            credentialsText.Replace("Domain", "EncryptedDomain");
            return credentialsText.ToString();
        }

        private void UpgradeConfigFile()
        {
            settings.StartDelayedUpdate();
            this.persistence.StartDelayedUpdate();
            string newConfigFileName = settings.FileLocations.Configuration;
            this.passwordsUpdate.UpdateConfigFilePasswords(newConfigFileName);
            // we don't have to reload now, because the file watcher is already listening
            settings.ForceReload(); // not identified why, but otherwise master password is lost
            // now already need to have persistence authenticated, otherwise we are working with wrong masterKey
            bool athenticated = this.persistence.Security.Authenticate(retry => this.prompt);
            if (athenticated)
                this.persistence.Initialize();
            this.ImportTagsFromConfigFile();
            this.MoveFavoritesFromConfigFile();
            this.MoveGroupsFromConfigFile();
            settings.RemoveAllFavoritesAndTags();
            this.ReplaceFavoriteButtonNamesByIds();
            settings.SaveAndFinishDelayedUpdate();
            persistence.Groups.Rebuild();
            // we can upgrade only file persistence, so the cast is safe.
            // Credentials Ids were newly created, and the file persistence didn't save the file.
            // We have to force the save to ensure, that upgraded favorites now point to proper credential ids.
            ((StoredCredentials)persistence.Credentials).Save();
            persistence.SaveAndFinishDelayedUpdate();
        }

        private void MoveGroupsFromConfigFile()
        {
            GroupConfigurationElementCollection configGroups = settings.GetGroups();
            foreach (GroupConfigurationElement configGroup in configGroups)
            {
                this.MoveFavoriteAliasesGroup(configGroup);
            }

            settings.ClearGroups();
        }

        private void MoveFavoriteAliasesGroup(GroupConfigurationElement configGroup)
        {
            IGroup group = FavoritesFactory.GetOrAddNewGroup(this.persistence, configGroup.Name);
            List<string> favoriteNames = configGroup.FavoriteAliases.GetFavoriteNames();
            List<IFavorite> groupFavorites = favoriteNames.Select(favoriteName => persistence.Favorites[favoriteName])
                .Where(favorite => favorite != null).ToList();
            this.persistence.Groups.AddFavorites(group, groupFavorites);
        }

        private void ReplaceFavoriteButtonNamesByIds()
        {
            string[] favoriteNames = settings.FavoriteNamesToolbarButtons;
            List<Guid> favoritesWithButton = persistence.Favorites
                .Where(favorite => favoriteNames.Contains(favorite.Name))
                .Select(candidate => candidate.Id).ToList();

            settings.UpdateFavoritesToolbarButtons(favoritesWithButton);
        }

        private void ImportTagsFromConfigFile()
        {
            foreach (string tag in settings.Tags)
            {
                var group = this.persistence.Factory.CreateGroup(tag);
                this.persistence.Groups.Add(group);
            }
        }

        private void MoveFavoritesFromConfigFile()
        {
            var tagsConvertert = new TagsConverter();

            foreach (FavoriteConfigurationElement favoriteConfigElement in settings.GetFavorites())
            {
                IFavorite favorite = ModelConverterV1ToV2.ConvertToFavorite(favoriteConfigElement, this.persistence, this.connectionManager);
                ImportWithDialogs.AddFavoriteIntoGroups(this.persistence, favorite, tagsConvertert.ResolveTagsList(favoriteConfigElement));
                this.persistence.Favorites.Add(favorite);
            }
        }
    }
}
