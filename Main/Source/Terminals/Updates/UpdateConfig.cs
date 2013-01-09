using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms.Controls;
using Terminals.Security;

namespace Terminals.Updates
{
    /// <summary>
    /// Class containing methods to update config files after an update. 
    /// </summary>
    internal static class UpdateConfig
    {
        /// <summary>
        /// Updates config file to current version, if it isn't up to date
        /// </summary>
        internal static void CheckConfigVersionUpdate()
        {
            // If the Terminals version is not in the config or the version number in the config
            // is lower then the current assembly version, check for config updates
            if (Settings.ConfigVersion == null || Settings.ConfigVersion < Program.Info.Version)
            {
                // keep update sequence ordered!
                UpdateToVersion20();

                // After all updates change the config version to the current assembly version
                Settings.ConfigVersion = Program.Info.Version;
            }
        }

        private static void UpdateToVersion20()
        {
            // problem: Settings file is already loaded from new location,
            // but we need to check, if there is one in old location
            if (IsVersion2UpTo21() || IsConfigFileOnV2Location())
            {
                MoveFilesToVersion2Location();
                UpdateDefaultFavoriteUrl();
            }
        }

        private static bool IsVersion2UpTo21()
        {
            return Program.Info.Version >= new Version(2, 0, 0, 0) &&
                   (Settings.ConfigVersion != null && Settings.ConfigVersion < new Version(2,1,0,0));
        }

        private static void MoveFilesToVersion2Location()
        {
            // we upgrade only files, there was no SqlPersistence before
            // don't need to refresh the file location, because if the files were changed
            // are already in use, they will be reloaded
            MoveThumbsDirectory();
            // don't move the logs directory, because it is in use by Log4net library.
            MoveDataFile(FileLocations.HISTORY_FILENAME);
            MoveDataFile(FileLocations.TOOLSTRIPS_FILENAME);
            // only this is already in use if started with default location
            MoveDataFile(FileLocations.CONFIG_FILENAME);
            Settings.ForceReload();
            Func<bool, AuthenticationPrompt> knowsUserPassword = RequestPassword.KnowsUserPassword;
            var passwordsUpdate = new PasswordsV2Update(knowsUserPassword);
            UpgradeCredentialsFile(passwordsUpdate);
            UpgradeConfigFile(passwordsUpdate, knowsUserPassword);
        }

        private static void UpgradeCredentialsFile(PasswordsV2Update passwordsUpdate)
        {
            MoveDataFile(FileLocations.CREDENTIALS_FILENAME);
            string credentialsFile = Settings.FileLocations.Credentials;
            if (File.Exists(credentialsFile))
            {
                string credentialsXml = File.ReadAllText(credentialsFile);
                string updatedContet = UpdateCredentialXmlTags(credentialsXml);
                updatedContet = passwordsUpdate.UpdateCredentialPasswords(updatedContet);
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

        private static void UpgradeConfigFile(PasswordsV2Update passwordsUpdate,
            Func<bool, AuthenticationPrompt> knowsUserPassword) 
        {
            Settings.StartDelayedUpdate();
            Persistence.Instance.StartDelayedUpdate();
            string newConfigFileName = Settings.FileLocations.Configuration;
            passwordsUpdate.UpdateConfigFilePasswords(newConfigFileName);
            // we don't have to reload now, because the file watcher is already listening
            Settings.ForceReload(); // not identified why, but otherwise master password is lost
            // now already need to have persistence authenticated, otherwise we are working with wrong masterKey
            Persistence.Instance.Security.Authenticate(knowsUserPassword);
            ImportTagsFromConfigFile();
            MoveFavoritesFromConfigFile();
            MoveGroupsFromConfigFile();
            Settings.RemoveAllFavoritesAndTags();
            ReplaceFavoriteButtonNamesByIds();
            Settings.SaveAndFinishDelayedUpdate();
            Persistence.Instance.Groups.Rebuild();
            Persistence.Instance.SaveAndFinishDelayedUpdate();
        }

        private static void MoveGroupsFromConfigFile()
        {
            GroupConfigurationElementCollection configGroups = Settings.GetGroups();
            foreach (GroupConfigurationElement configGroup in configGroups)
            {
                MoveFavoriteAliasesGroup(configGroup);
            }
            
            Settings.ClearGroups();
        }

        private static void MoveFavoriteAliasesGroup(GroupConfigurationElement configGroup) 
        {
            IGroup group = FavoritesFactory.GetOrCreateGroup(configGroup.Name);
            List<string> favoriteNames = configGroup.FavoriteAliases.GetFavoriteNames();
            IFavorites favorites = Persistence.Instance.Favorites;
            List<IFavorite> groupFavorites = favoriteNames.Select(favoriteName => favorites[favoriteName])
                .Where(favorite => favorite != null).ToList();
            group.AddFavorites(groupFavorites);
        }

        private static void ReplaceFavoriteButtonNamesByIds()
        {
            string[] favoriteNames = Settings.FavoriteNamesToolbarButtons;
            List<Guid> favoritesWithButton = Persistence.Instance.Favorites
                .Where(favorite => favoriteNames.Contains(favorite.Name))
                .Select(candidate => candidate.Id).ToList();

            Settings.UpdateFavoritesToolbarButtons(favoritesWithButton);
        }

        private static void ImportTagsFromConfigFile()
        {
            IGroups groups = Persistence.Instance.Groups;
            foreach (string tag in Settings.Tags)
            {
                var group = Persistence.Instance.Factory.CreateGroup(tag);
                groups.Add(group);
            }
        }

        private static void MoveFavoritesFromConfigFile()
        {
            IFavorites favorites = Persistence.Instance.Favorites;
            foreach (FavoriteConfigurationElement favoriteConfigElement in Settings.GetFavorites())
            {
                IFavorite favorite = ModelConverterV1ToV2.ConvertToFavorite(favoriteConfigElement);
                ImportWithDialogs.AddFavoriteIntoGroups(favoriteConfigElement, favorite);
                favorites.Add(favorite);
            }
        }

        private static void MoveThumbsDirectory()
        {
            try
            {
                TryToMoveThumbsDirectory();
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Upgrade of Thumbs directory failed", exception);
            }
        }

        private static void TryToMoveThumbsDirectory()
        {
            string oldPath = GetOldDataFullPath(FileLocations.THUMBS_DIRECTORY);
            string newPath = FileLocations.ThumbsDirectoryFullPath;

            if (Directory.Exists(oldPath))
            {
                if(Directory.Exists(newPath))
                    Directory.Delete(newPath, true);

                Directory.Move(oldPath, newPath);
            }
        }

        private static void MoveDataFile(string relativePath)
        {
            try
            {
                TryToMoveDataFile(relativePath);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Upgrade of data file failed", exception);
            }
        }

        private static void TryToMoveDataFile(string relativePath)
        {
            string oldPath = GetOldDataFullPath(relativePath);
            string newPath = FileLocations.GetFullPath(relativePath);
            if (File.Exists(oldPath))
            {
                if (!File.Exists(newPath))
                {
                    File.Move(oldPath, newPath);
                }
                else
                {
                    File.Copy(oldPath, newPath, true);
                    File.Delete(oldPath);
                }
            }
        }

        private static bool IsConfigFileOnV2Location()
        {
            string oldConfigFile = GetOldDataFullPath(FileLocations.CONFIG_FILENAME);
            return File.Exists(oldConfigFile);
        }

        private static string GetOldDataFullPath(string relativePath)
        {
            return Path.Combine(Program.Info.Location, relativePath);
        }

        /// <summary>
        /// Change the Terminals URL to the correct URL used for Terminals News as of version 2.0 RC1
        /// </summary>
        private static void UpdateDefaultFavoriteUrl()
        {
            var favorites = Persistence.Instance.Favorites;
            IFavorite newsFavorite = favorites[FavoritesFactory.TerminalsReleasesFavoriteName];
            if (newsFavorite != null)
            {
                newsFavorite.ServerName = Program.Resources.GetString("TerminalsURL");
                Persistence.Instance.SaveAndFinishDelayedUpdate();
            }
        }
    }
}
