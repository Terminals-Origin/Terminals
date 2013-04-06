using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terminals.Configuration;
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

        private readonly PasswordsV2Update passwordsUpdate;

        private AuthenticationPrompt prompt;

        /// <summary>
        /// Initialize new instance of the upgrade providing fresh not initialized persistence and password prompt.
        /// </summary>
        /// <param name="persistence">Not null,not authenticated, not initialized persistence</param>
        /// <param name="knowsUserPassword">Not null password prompt to obtain current master password from user</param>
        internal FilesV2ContentUpgrade(IPersistence persistence, Func<bool, AuthenticationPrompt> knowsUserPassword)
        {
            this.persistence = persistence;

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
            string credentialsFile = Settings.FileLocations.Credentials;
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
            Settings.StartDelayedUpdate();
            this.persistence.StartDelayedUpdate();
            string newConfigFileName = Settings.FileLocations.Configuration;
            this.passwordsUpdate.UpdateConfigFilePasswords(newConfigFileName);
            // we don't have to reload now, because the file watcher is already listening
            Settings.ForceReload(); // not identified why, but otherwise master password is lost
            // now already need to have persistence authenticated, otherwise we are working with wrong masterKey
            this.persistence.Security.Authenticate(retry => this.prompt);
            this.ImportTagsFromConfigFile();
            this.MoveFavoritesFromConfigFile();
            this.MoveGroupsFromConfigFile();
            Settings.RemoveAllFavoritesAndTags();
            this.ReplaceFavoriteButtonNamesByIds();
            Settings.SaveAndFinishDelayedUpdate();
            persistence.Groups.Rebuild();
            persistence.SaveAndFinishDelayedUpdate();
        }

        private void MoveGroupsFromConfigFile()
        {
            GroupConfigurationElementCollection configGroups = Settings.GetGroups();
            foreach (GroupConfigurationElement configGroup in configGroups)
            {
                this.MoveFavoriteAliasesGroup(configGroup);
            }

            Settings.ClearGroups();
        }

        private void MoveFavoriteAliasesGroup(GroupConfigurationElement configGroup)
        {
            IGroup group = FavoritesFactory.GetOrAddNewGroup(configGroup.Name);
            List<string> favoriteNames = configGroup.FavoriteAliases.GetFavoriteNames();
            List<IFavorite> groupFavorites = favoriteNames.Select(favoriteName => persistence.Favorites[favoriteName])
                .Where(favorite => favorite != null).ToList();
            group.AddFavorites(groupFavorites);
        }

        private void ReplaceFavoriteButtonNamesByIds()
        {
            string[] favoriteNames = Settings.FavoriteNamesToolbarButtons;
            List<Guid> favoritesWithButton = persistence.Favorites
                .Where(favorite => favoriteNames.Contains(favorite.Name))
                .Select(candidate => candidate.Id).ToList();

            Settings.UpdateFavoritesToolbarButtons(favoritesWithButton);
        }

        private void ImportTagsFromConfigFile()
        {
            foreach (string tag in Settings.Tags)
            {
                var group = this.persistence.Factory.CreateGroup(tag);
                this.persistence.Groups.Add(group);
            }
        }

        private void MoveFavoritesFromConfigFile()
        {
            foreach (FavoriteConfigurationElement favoriteConfigElement in Settings.GetFavorites())
            {
                IFavorite favorite = ModelConverterV1ToV2.ConvertToFavorite(favoriteConfigElement, this.persistence);
                ImportWithDialogs.AddFavoriteIntoGroups(favoriteConfigElement, favorite);
                this.persistence.Favorites.Add(favorite);
            }
        }
    }
}
