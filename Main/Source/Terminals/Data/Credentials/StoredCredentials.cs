using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Terminals.Configuration;
using SysConfig = System.Configuration;
using System.IO;
using System.Linq;
using Unified;

namespace Terminals.Data
{
    internal sealed class StoredCredentials: ICredentials
    {
        private readonly PersistenceSecurity persistenceSecurity;
        private readonly List<ICredentialSet> cache;
        private readonly Mutex fileLock = new Mutex(false, "Terminals.CodePlex.com.Credentials");
        private DataFileWatcher fileWatcher;

        private string FileFullName
        {
            get { return Settings.FileLocations.Credentials; }
        }

        internal StoredCredentials(PersistenceSecurity persistenceSecurity)
        {
            this.persistenceSecurity = persistenceSecurity;
            this.cache = new List<ICredentialSet>();
            string configFileName = this.FileFullName;
            InitializeFileWatch();

            if (File.Exists(configFileName))
                LoadStoredCredentials(configFileName);
            else
                Save();
        }

        private void InitializeFileWatch()
        {
            fileWatcher = new DataFileWatcher(FileFullName);
            fileWatcher.FileChanged += new EventHandler(CredentialsFileChanged);
            fileWatcher.StartObservation();
        }

        private void CredentialsFileChanged(object sender, EventArgs e)
        {
            LoadStoredCredentials(FileFullName);
            if (CredentialsChanged != null)
                CredentialsChanged(this, new EventArgs());
        }

        internal void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            fileWatcher.AssignSynchronizer(synchronizer);
        }

        private void LoadStoredCredentials(string configFileName)
        {
            List<ICredentialSet> loaded = LoadFile(configFileName);
            if (loaded != null)
            {
                this.cache.Clear();
                this.cache.AddRange(loaded);
            }
        }

        private List<ICredentialSet> LoadFile(string configFileName)
        {
            try
            {
                fileLock.WaitOne();
                return DeserializeFileContent(configFileName);
            }
            catch (Exception exception)
            {
                string errorMessage = String.Format("Load credentials from {0} failed.", configFileName);
                Logging.Log.Error(errorMessage, exception);
                return new List<ICredentialSet>();
            }
            finally
            {
                fileLock.ReleaseMutex();
                Debug.WriteLine("Credentials file Loaded.");
            }
        }

        private List<ICredentialSet> DeserializeFileContent(string configFileName)
        {
            object loadedObj = Serialize.DeserializeXMLFromDisk(configFileName, typeof (List<CredentialSet>));
            var loadedItems = loadedObj as List<CredentialSet>;
            foreach (CredentialSet credentialSet in loadedItems)
            {
                credentialSet.AssignStore(this.persistenceSecurity);
            }

            return loadedItems.Cast<ICredentialSet>().ToList();
        }

        #region ICredentials

        public event EventHandler CredentialsChanged;

        public ICredentialSet this[Guid id]
        {
            get
            {
                return this.cache.FirstOrDefault(candidate => candidate.Id.Equals(id));
            }
        }

        /// <summary>
        /// Gets a credential by its name from cached credentials.
        /// This method isnt case sensitive. If no item matches, returns null.
        /// </summary>
        /// <param name="name">name of an item to search</param>
        public ICredentialSet this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    return null;

                return this.cache.FirstOrDefault(candidate => candidate.Name
                   .Equals(name, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public void Add(ICredentialSet toAdd)
        {
            if (String.IsNullOrEmpty(toAdd.Name))
                return;

            this.cache.Add(toAdd);
            this.Save();
        }

        public void Remove(ICredentialSet toRemove)
        {
            this.cache.Remove(toRemove);
            this.Save();
        }

        public void Update(ICredentialSet toUpdate)
        {
            var oldItem = this[toUpdate.Id];
            if (oldItem != null)
                this.cache.Remove(oldItem);
            this.cache.Add(toUpdate);
            Save();
        }

        public void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            foreach (ICredentialSet credentials in cache)
            {
                credentials.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
            }

            Save();
        }

        private void Save()
        {
            try
            {
                this.fileLock.WaitOne();
                this.fileWatcher.StopObservation();
                this.SaveToFile();
            }
            catch (Exception exception)
            {
                string errorMessage = string.Format("Save credentials to {0} failed.", FileFullName);
                Logging.Log.Error(errorMessage, exception);
            }
            finally
            {
                this.fileWatcher.StartObservation();
                this.fileLock.ReleaseMutex();
            }
        }

        private void SaveToFile()
        {
            List<CredentialSet> fileContent = this.cache.Cast<CredentialSet>().ToList();
            Serialize.SerializeXMLToDisk(fileContent, FileFullName);
            Debug.WriteLine("Credentials file saved.");
        }

        #endregion

        #region IEnumerable members

        public IEnumerator<ICredentialSet> GetEnumerator()
        {
            return this.cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
