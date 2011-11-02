using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SysConfig = System.Configuration;
using System.IO;
using System.Linq;
using Unified;

namespace Terminals.Configuration
{
    internal class StoredCredentials
    {
        /// <summary>
        /// Gets default name of the credentials file.
        /// </summary>
        private const string CONFIG_FILE = "Credentials.xml";

        private List<CredentialSet> cache;

        /// <summary>
        /// Prevents creating from other class
        /// </summary>
        private StoredCredentials()
        {
            this.cache = new List<CredentialSet>();
            string configFileName = GetEnsuredCredentialsFileLocation();

            if (File.Exists(configFileName))
            {
                LoadStoredCredentials(configFileName);
            }
            else
            {
                Save();
            }
        }

        private static StoredCredentials instance;
        /// <summary>
        /// Gets the singleton instance with cached credentials
        /// </summary>
        internal static StoredCredentials Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StoredCredentials();
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets the not null collection containing stored credentials
        /// </summary>
        internal List<CredentialSet> Items
        {
            get
            {
                // prevent manipulation directly with this list
                return cache.ToList();
            }
        }

        private void LoadStoredCredentials(string configFileName)
        {
            object loadedObj = Serialize.DeserializeXMLFromDisk(configFileName, typeof(List<CredentialSet>));
            List<CredentialSet> loaded = loadedObj as List<CredentialSet>;
            if (loaded != null)
            {
                this.cache.Clear();
                this.cache.AddRange(loaded);
            }
        }

        internal void Save()
        {
            string fileName = GetEnsuredCredentialsFileLocation();
            Serialize.SerializeXMLToDisk(cache, fileName);
        }

        private static string GetEnsuredCredentialsFileLocation()
        {
            // TODO not configurable location of credentials file (Jiri Pokorny, 08.07.2011)
            string fileLocation = Settings.SavedCredentialsLocation;
            if (string.IsNullOrEmpty(fileLocation))
            {
                //UpgradeFileLocationFromPreviousVersion();
                fileLocation = CONFIG_FILE; // GetFullFilePath();
                Settings.SavedCredentialsLocation = fileLocation;
            }

            return fileLocation;
        }

        private static string GetFullFilePath()
        {
            string directory = GetEnsuredConfigDirectory();
            return directory + CONFIG_FILE;
        }

        private static string GetEnsuredConfigDirectory()
        {
            SysConfig.Configuration config = SysConfig.ConfigurationManager.OpenExeConfiguration(
              SysConfig.ConfigurationUserLevel.PerUserRoamingAndLocal);
            string directory = Path.GetDirectoryName(config.FilePath);

            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch
            {
                Logging.Log.Error("Couldn't create directory for credentials configuration file.");
            }
            return directory + "\\";
        }

        /// <summary>
        /// Moves credentials file from application directory to the user directory
        /// </summary>
        private static void UpgradeFileLocationFromPreviousVersion()
        {
            try
            {
                string fromPath = Application.StartupPath + "\\" + CONFIG_FILE;
                string toPath = GetFullFilePath();
                if (File.Exists(fromPath) && !File.Exists(toPath)) // dont overwrite file, if it is already there
                {
                    File.Move(fromPath, toPath);
                }
            }
            catch(Exception exception)
            {
               Logging.Log.Error("Couldnt move credentials file from application directory to the user directory.", exception);
            }
        }

        /// <summary>
        /// Gets a credential by its name from cached credentials.
        /// This method isnt case sensitive. If no item matches, returns null.
        /// </summary>
        /// <param name="name">name of an item to search</param>
        internal CredentialSet GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            name = name.ToLower();
            return this.Items.FirstOrDefault(candidate => candidate.Name.ToLower() == name);
        }

        internal void Remove(CredentialSet toRemove)
        {
            cache.Remove(toRemove);
        }

        internal void Add(CredentialSet toAdd)
        {
            if (String.IsNullOrEmpty(toAdd.Name))
                return;

            cache.Add(toAdd);
        }

        internal void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
          foreach (CredentialSet credentials in cache)
          {
            credentials.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
          }

          Save();
        }
    }
}
