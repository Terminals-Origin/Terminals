using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Terminals.Configuration
{
    /// <summary>
    /// Data file locations resolution under Data subdirectory
    /// </summary>
    internal sealed class FileLocations
    {
        private static readonly string PROFILE_DATA_DIRECTORY = 
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private const string PROFILE_PATH = @"Robert_Chartier\Terminals\";

        /// <summary>
        /// Gets the directory name of data directory,
        /// where all files changed by user should be stored
        /// </summary>
        private const string DATA_DIRECTORY = "Data";

        /// <summary>
        /// Gets directory name of the commands thumb images location
        /// </summary>
        internal const string THUMBS_DIRECTORY = "Thumbs";

        /// <summary>
        /// Gets default name of the credentials file.
        /// </summary>
        internal const string CREDENTIALS_FILENAME = "Credentials.xml";

        /// <summary>
        /// Gets default name of the favorites file.
        /// </summary>
        internal const string FAVORITES_FILENAME = "Favorites.xml";

        /// <summary>
        /// Gets the file name of stored history values
        /// </summary>
        internal const string HISTORY_FILENAME = "History.xml";

        /// <summary>
        /// Gets the name of custom user options configuration file
        /// </summary>
        internal const string CONFIG_FILENAME = "Terminals.config";

        /// <summary>
        /// Gets the file name of xml config file, where toolbar positions are stored
        /// </summary>
        internal const string TOOLSTRIPS_FILENAME = "ToolStrip.settings.config";

        internal static string ControlPanelImage
        {
            get { return Path.Combine(DATA_DIRECTORY, @"Thumbs\ControlPanel.png"); }
        }

        internal static string LastUpdateCheck
        {
            get { return GetFullPath("LastUpdateCheck.txt"); }
        }

        internal static string WriteAccessLock
        {
            get { return GetFullPath("WriteAccessCheck.txt"); }
        }

        internal static string LogDirectory
        {
            get
            {
                // dont take if from other paths, because it can be changed by some one else.
                string log4NetFilePath = "Terminals.log4net.config";
                XDocument configFile = XDocument.Load(log4NetFilePath);
                XAttribute fileAttribute = SelectFileElement(configFile);
                return Path.GetDirectoryName(fileAttribute.Value);
            }
        }

        private static XAttribute SelectFileElement(XDocument configFile)
        {
          return configFile.Descendants("file")
              .Attributes("value")
              .FirstOrDefault();
        }

        internal static string ThumbsDirectoryFullPath
        {
            get { return GetFullPath(THUMBS_DIRECTORY); }
        }

        internal static string ToolStripsFullFileName
        {
            get { return GetFullPath(TOOLSTRIPS_FILENAME); }
        }

        internal static string HistoryFullFileName
        {
            get { return GetFullPath(HISTORY_FILENAME); }
        }

        internal string Configuration { get; private set; }
        
        internal string Favorites { get; private set; }

        internal string Credentials { get; private set; }

        /// <summary>
        /// Sets custom file locations for general data files.
        /// All paths have to be set to absolute file path,  otherwise are ignored.
        /// You have to call this method only once at startup before files are loaded,
        /// otherwise their usage isnt consistent.
        /// </summary>
        internal void AssignCustomFileLocations(string configurationFullPath,
            string favoritesFullPath, string credentialsFullPath)
        {
            // we dont have to assign to file filewatchers, they arent initialized yet
            // we dont have to check if files exist, we recreate them
            AssignConfigurationFile(configurationFullPath);
            AssignFavoritesFile(favoritesFullPath);
            AssignCredentialsFile(credentialsFullPath);
        }

        private void AssignConfigurationFile(string configurationFullPath)
        {
            if (string.IsNullOrEmpty(configurationFullPath))
                this.Configuration = GetFullPath(CONFIG_FILENAME);
            else
                this.Configuration = configurationFullPath;
        }

        private void AssignFavoritesFile(string favoritesFullPath)
        {
            if (string.IsNullOrEmpty(favoritesFullPath))
            {
                this.Favorites = Settings.SavedFavoritesFileLocation;
                if (string.IsNullOrEmpty(this.Favorites))
                    this.Favorites = GetFullPath(FAVORITES_FILENAME);
            }
            else
            {
                this.Favorites = favoritesFullPath;
            }
        }

        private void AssignCredentialsFile(string credentialsFullPath)
        {
            if (string.IsNullOrEmpty(credentialsFullPath))
            {
                this.Credentials = Settings.SavedCredentialsLocation;
                if (string.IsNullOrEmpty(this.Credentials))
                    this.Credentials = GetFullPath(CREDENTIALS_FILENAME);
            }
            else
            {
                this.Credentials = credentialsFullPath;
            }
        }

        /// <summary>
        /// Gets the full file path to the required file or directory in application data directory.
        /// </summary>
        /// <param name="relativePath">The relative path to the file from data directory.</param>
        internal static string GetFullPath(string relativePath)
        {
            string root = GetDataRootDirectoryFullPath();
            return Path.Combine(root, relativePath);
        }

        private static string GetDataRootDirectoryFullPath()
        {
            string root = Path.Combine(Program.Info.Location, DATA_DIRECTORY);
            bool localInstallation = !Properties.Settings.Default.Portable;
            if (localInstallation)
                root = GetProfileDataDirectoryPath();

            EnsureDataDirectory(root);
            return root;
        }

        private static string GetProfileDataDirectoryPath()
        {
            string relativeDataPath = PROFILE_PATH + DATA_DIRECTORY;
            return Path.Combine(PROFILE_DATA_DIRECTORY, relativeDataPath);
        }

        private static void EnsureDataDirectory(string root)
        {
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
        }

        internal static string FormatThumbFileName(string fileName)
        {
            return string.Format(@"{0}\{1}\{2}.jpg", DATA_DIRECTORY, THUMBS_DIRECTORY, fileName);
        }

        internal static void EnsureImagesDirectory()
        {
            EnsureDataDirectory(ThumbsDirectoryFullPath);
        }
    }
}
