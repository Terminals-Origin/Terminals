using System;
using System.IO;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Security;
using Terminals.Services;

namespace Terminals.Updates
{
    internal class V20Upgrade : IVersionUpgrade
    {
        private readonly Settings settings;

        private readonly IPersistence persistence;

        private readonly ConnectionManager connectionManager;

        internal V20Upgrade(Settings settings, IPersistence persistence, ConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
            this.persistence = persistence;
            this.settings = settings;
        }

        public bool NeedExecute()
        {
            // problem: Settings file is already loaded from new location,
            // but we need to check, if there is one in old location
            return this.IsVersion2UpTo21() || IsConfigFileOnV2Location();
        }

        public void Upgrade()
        {
            this.MoveFilesToVersion2Location();
            this.UpdateDefaultFavoriteUrl();
        }

        private bool IsVersion2UpTo21()
        {
            return Program.Info.Version >= new Version(2, 0, 0, 0) &&
                   (this.settings.ConfigVersion != null && this.settings.ConfigVersion < new Version(2, 1, 0, 0));
        }

        private void MoveFilesToVersion2Location()
        {
            // we upgrade only files, there was no SqlPersistence before
            // don't need to refresh the file location, because if the files were changed
            // are already in use, they will be reloaded
            MoveThumbsDirectory();
            // don't move the logs directory, because it is in use by Log4net library.
            MoveDataFile(FileLocations.HISTORY_FILENAME);
            MoveDataFile(FileLocations.TOOLSTRIPS_FILENAME);
            MoveDataFile(FileLocations.CREDENTIALS_FILENAME);
            // only this is already in use if started with default location
            MoveDataFile(FileLocations.CONFIG_FILENAME);
            this.settings.ForceReload();
            var contentUpgrade = new FilesV2ContentUpgrade(this.persistence, this.connectionManager, RequestPassword.KnowsUserPassword);
            contentUpgrade.Run();
        }

        private static void MoveThumbsDirectory()
        {
            try
            {
                TryToMoveThumbsDirectory();
            }
            catch (Exception exception)
            {
                Logging.Error("Upgrade of Thumbs directory failed", exception);
            }
        }

        private static void TryToMoveThumbsDirectory()
        {
            string oldPath = GetOldDataFullPath(FileLocations.THUMBS_DIRECTORY);
            string newPath = FileLocations.ThumbsDirectoryFullPath;

            if (Directory.Exists(oldPath))
            {
                if (Directory.Exists(newPath))
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
                Logging.Error("Upgrade of data file failed", exception);
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
        private void UpdateDefaultFavoriteUrl()
        {
            IFavorite newsFavorite = this.persistence.Favorites[FavoritesFactory.TerminalsReleasesFavoriteName];
            if (newsFavorite != null)
            {
                newsFavorite.ServerName = ExternalLinks.TerminalsReleasesUrl;
                this.persistence.Favorites.Update(newsFavorite);
            }
        }
    }
}