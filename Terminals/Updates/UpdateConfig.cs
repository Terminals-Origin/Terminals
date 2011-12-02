using System;
using System.IO;
using System.Windows.Forms;
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
                // todo MoveFilesToVersion2Location();
                UpdateDefaultFavoriteUrl();
            }
        }

        private static void MoveFilesToVersion2Location()
        {
            // dont need to refresh the file location, because if the files were changed are are
            // already in use, they will be reloaded
            string root = Settings.GetDataRootDirectoryFullPath();
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            MoveThumbsDirectory();
            MoveDataFile(ConnectionHistory.FILENAME);
            MoveDataFile(ToolStripSettings.FLE_NAME);
            // only this is already in use if started with default location
            MoveDataFile(Settings.CONFIG_FILE_NAME);

            // and then extract favorites and Tags from old config
        }

        private static void MoveThumbsDirectory()
        {
            string oldPath = GetOldDataFullPath(SpecialCommandsWizard.THUMBS_DIRECTORY);
            string newPath = GetNewDataFullPath(SpecialCommandsWizard.THUMBS_DIRECTORY);
            if(Directory.Exists(oldPath))
                Directory.Move(oldPath, newPath);
        }

        private static void MoveDataFile(string relativePath) 
        {
            string oldPath = GetOldDataFullPath(relativePath);
            string newPath = GetNewDataFullPath(relativePath);
            if(File.Exists(oldPath))
                File.Move(oldPath, newPath);
        }

        private static string GetNewDataFullPath(string relativePath)
        {
            string dataRoot = Settings.GetDataRootDirectoryFullPath();
            return Path.Combine(dataRoot, relativePath);
        }

        private static string GetOldDataFullPath(string relativePath)
        {
            string root = Path.GetDirectoryName(Application.ExecutablePath);
            return Path.Combine(root, relativePath);
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
