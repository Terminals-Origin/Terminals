using System;
using System.IO;
using Terminals.Configuration;
using Terminals.Data;
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
            // todo and then extract favorites and Tags from old config
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
            FavoriteConfigurationElement newsFavorite = favorites.GetOneFavorite(FavoritesFactory.TerminalsReleasesFavoriteName);
            if (newsFavorite != null)
            {
                newsFavorite.Url = Program.Resources.GetString("TerminalsURL");
                favorites.SaveDefaultFavorite(newsFavorite);
            }
        }
    }
}
