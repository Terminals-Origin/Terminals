using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Terminals.Data;
using SysConfig = System.Configuration;
using System.IO;
using System.Linq;
using Unified;

namespace Terminals.Configuration
{
    internal sealed class StoredCredentials
    {
        /// <summary>
        /// Gets default name of the credentials file.
        /// </summary>
        internal const string FILE_NAME = "Credentials.xml";

        private static string FullFileName
        {
            get
            {
                return FileLocations.GetFullPath(FILE_NAME);
            }
        }

        private List<CredentialSet> cache;
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

        private Mutex fileLock = new Mutex(false, "Terminals.CodePlex.com.Credentials");
        private DataFileWatcher fileWatcher;
        internal event EventHandler CredentialsChanged;

        #region Thread safe singleton

        /// <summary>
        /// Gets the singleton instance with cached credentials
        /// </summary>
        public static StoredCredentials Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private static class Nested
        {
            internal static readonly StoredCredentials instance = new StoredCredentials();
        }

        #endregion

        /// <summary>
        /// Prevents creating from other class
        /// </summary>
        private StoredCredentials()
        {
            this.cache = new List<CredentialSet>();
            string configFileName = GetEnsuredCredentialsFileLocation();
            InitializeFileWatch();

            if (File.Exists(configFileName))
                LoadStoredCredentials(configFileName);
            else
                Save();
        }

        private void InitializeFileWatch()
        {
            fileWatcher = new DataFileWatcher(FullFileName);
            fileWatcher.FileChanged += new EventHandler(CredentialsFileChanged);
            fileWatcher.StartObservation();
        }

        private void CredentialsFileChanged(object sender, EventArgs e)
        {
            LoadStoredCredentials(GetEnsuredCredentialsFileLocation());
            if (CredentialsChanged != null)
                CredentialsChanged(this, new EventArgs());
        }

        internal void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            fileWatcher.AssignSynchronizer(synchronizer);
        }

        private void LoadStoredCredentials(string configFileName)
        {
            List<CredentialSet> loaded = LoadFile(configFileName);
            if (loaded != null)
            {
                this.cache.Clear();
                this.cache.AddRange(loaded);
            }
        }

        private List<CredentialSet> LoadFile(string configFileName)
        {
            try
            {
                fileLock.WaitOne();
                object loadedObj = Serialize.DeserializeXMLFromDisk(configFileName, typeof(List<CredentialSet>));
                return loadedObj as List<CredentialSet>;
            }
            catch (Exception exception)
            {
                string errorMessage = String.Format("Load credentials from {0} failed.", configFileName);
                Logging.Log.Error(errorMessage, exception);
                return new List<CredentialSet>();
            }
            finally
            {
                fileLock.ReleaseMutex();
            }
        }

        internal void Save()
        {
            try
            {
                fileLock.WaitOne();
                fileWatcher.StopObservation();
                string fileName = GetEnsuredCredentialsFileLocation();
                Serialize.SerializeXMLToDisk(cache, fileName);
            }
            catch (Exception exception)
            {
                string errorMessage = string.Format("Save credentials to {0} failed.", 
                    GetEnsuredCredentialsFileLocation());
                Logging.Log.Error(errorMessage, exception);
            }
            finally
            {
                fileWatcher.StartObservation();
                fileLock.ReleaseMutex();
            }
        }

        private static string GetEnsuredCredentialsFileLocation()
        {
            // TODO not configurable location of credentials file (Jiri Pokorny, 08.07.2011)
            string fileLocation = Settings.SavedCredentialsLocation;
            if (string.IsNullOrEmpty(fileLocation))
            {
                Settings.SavedCredentialsLocation = FullFileName;
            }

            return fileLocation;
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
