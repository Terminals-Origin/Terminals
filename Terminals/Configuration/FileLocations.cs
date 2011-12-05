using System.IO;

namespace Terminals.Configuration
{
    /// <summary>
    /// Data file locations resolution under Data subdirectory
    /// </summary>
    internal static class FileLocations
    {
        /// <summary>
        /// Gets the directory name of data directory,
        /// where all files changed by user should be stored
        /// </summary>
        internal const string DATA_DIRECTORY = "Data";
        
        /// <summary>
        /// Gets the name of the directory, where log files are stored
        /// </summary>
        internal const string LOG_DIRECTORY = "Logs";

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
            EnsureDataDirectory(root);
            return root;
        }

        private static void EnsureDataDirectory(string root)
        {
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
        }
    }
}
