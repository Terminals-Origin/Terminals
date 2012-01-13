using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms.Controls;
using Terminals.History;
using Terminals.Wizard;

namespace Terminals.Updates
{
    /// <summary>
    /// Class containing methods to update config files after an update. 
    /// </summary>
    internal static class UpdateConfig
    {
        /// <summary>
        /// Updates config file to current version, if it isnt up to date
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
            if (Program.Info.Version >= new Version(2, 0, 0, 0))
            {
                MoveFilesToVersion2Location();
                UpdateDefaultFavoriteUrl();
            }
        }

        private static void MoveFilesToVersion2Location()
        {
            // dont need to refresh the file location, because if the files were changed
            // are already in use, they will be reloaded
            MoveThumbsDirectory();
            // dont move the logs directory, because it is in use by Log4net library.
            MoveDataFile(ConnectionHistory.FILE_NAME);
            MoveDataFile(ToolStripSettings.FLE_NAME);
            MoveDataFile(StoredCredentials.FILE_NAME);
            UpgradeConfigFile();
        }

        private static void UpgradeConfigFile() 
        {
            // only this is already in use if started with default location
            MoveDataFile(Settings.CONFIG_FILE_NAME);
            Settings.ForceReload();
            Settings.StartDelayedUpdate();
            Persistance.Instance.StartDelayedUpdate();
            ImportTagsFromConfigFile();
            MoveFavoritesFromConfigFile();
            MoveGroupsFromConfigFile();
            Settings.DeleteFavorites(Settings.GetFavorites().ToList());
            Settings.DeleteTags(Settings.Tags.ToList());
            ReplaceFavoriteButtonNamesByIds();
            Settings.SaveAndFinishDelayedUpdate();
            Persistance.Instance.Groups.Rebuild();
            Persistance.Instance.SaveAndFinishDelayedUpdate();
        }

        private static void MoveGroupsFromConfigFile()
        {
            var configGroups = Settings.GetGroups();
            foreach (GroupConfigurationElement configGroup in configGroups)
            {
                MoveFavoriteAliasesGroup(configGroup);
            }
            
            configGroups.Clear();
        }

        private static void MoveFavoriteAliasesGroup(GroupConfigurationElement configGroup) 
        {
            IFactory factory = Persistance.Instance.Factory;
            IGroup group = factory.GetOrCreateGroup(configGroup.Name);
            List<string> favoriteNames = configGroup.FavoriteAliases.GetFavoriteNames();
            IFavorites favorites = Persistance.Instance.Favorites;
            List<IFavorite> groupFavorites = favoriteNames.Select(favoriteName => favorites[favoriteName])
                .Where(favorite => favorite != null).ToList();
            group.AddFavorites(groupFavorites);
        }

        private static void ReplaceFavoriteButtonNamesByIds()
        {
            string[] favoriteNames = Settings.FavoriteNamesToolbarButtons;
            List<Guid> favoritesWithButton = Persistance.Instance.Favorites
                .Where(favorite => favoriteNames.Contains(favorite.Name))
                .Select(candidate => candidate.Id).ToList();

            Settings.UpdateFavoritesToolbarButtons(favoritesWithButton);
        }

        private static void ImportTagsFromConfigFile()
        {
            IGroups groups = Persistance.Instance.Groups;
            foreach (var tag in Settings.Tags)
            {
                var group = Persistance.Instance.Factory.CreateGroup(tag);
                groups.Add(group);
            }
        }

        private static void MoveFavoritesFromConfigFile()
        {
            IFavorites favorites = Persistance.Instance.Favorites;
            foreach (FavoriteConfigurationElement favoriteConfigElement in Settings.GetFavorites())
            {
                var favorite = ModelConverterV1ToV2.ConvertToFavorite(favoriteConfigElement);
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
            string oldPath = GetOldDataFullPath(SpecialCommandsWizard.THUMBS_DIRECTORY);
            string newPath = FileLocations.GetFullPath(SpecialCommandsWizard.THUMBS_DIRECTORY);

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

        private static string GetOldDataFullPath(string relativePath)
        {
            return Path.Combine(Program.Info.Location, relativePath);
        }

        /// <summary>
        /// Change the Terminals URL to the correct URL used for Terminals News as of version 2.0 RC1
        /// </summary>
        private static void UpdateDefaultFavoriteUrl()
        {
            var favorites = Persistance.Instance.Favorites;
            IFavorite newsFavorite = favorites[FavoritesFactory.TerminalsReleasesFavoriteName];
            if (newsFavorite != null)
            {
                newsFavorite.ServerName = Program.Resources.GetString("TerminalsURL");
                Persistance.Instance.SaveAndFinishDelayedUpdate();
            }
        }
    }
}
