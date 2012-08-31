using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Terminals.Configuration;
using Terminals.History;
using Terminals.Security;
using Unified;

namespace Terminals.Data
{
    internal class FilePersistence : IPersistence, IPersistedSecurity
    {
        private readonly Favorites favorites;
        public IFavorites Favorites
        {
            get
            {
                return this.favorites;
            }
        }

        private readonly Groups groups;
        internal Groups GroupsStore
        {
            get { return this.groups; }
        }

        public IGroups Groups
        {
            get
            {
                return this.groups;
            }
        }

        private readonly Factory factory;
        public IFactory Factory
        {
            get { return this.factory; }
        }

        private readonly StoredCredentials storedCredentials;
        public ICredentials Credentials
        {
            get { return this.storedCredentials; }
        }

        private readonly ConnectionHistory connectionHistory;
        public IConnectionHistory ConnectionHistory
        {
            get { return this.connectionHistory; }
        }

        public DataDispatcher Dispatcher { get; private set; }

        public PersistenceSecurity Security { get; private set; }

        private readonly Mutex fileLock = new Mutex(false, "Terminals.CodePlex.com.FilePersistence");
        private DataFileWatcher fileWatcher;
        private bool delaySave;

        internal FilePersistence() : this(new PersistenceSecurity())
        {}

        /// <summary>
        /// Try to reuse current security in case of changing peristence, because user is already authenticated
        /// </summary>
        internal FilePersistence(PersistenceSecurity security)
        {
            this.InitializeSecurity(security);
            this.Dispatcher = new DataDispatcher();
            this.storedCredentials = new StoredCredentials();
            this.groups = new Groups(this);
            this.favorites = new Favorites(this);
            this.connectionHistory = new ConnectionHistory(this.favorites);
            this.factory = new Factory(this.groups, this.Dispatcher);
            this.InitializeFileWatch();
        }

        private void InitializeSecurity(PersistenceSecurity security)
        {
            this.Security = security;
            this.Security.AssignPersistence(this);
        }

        private void InitializeFileWatch()
        {
            this.fileWatcher = new DataFileWatcher(Settings.FileLocations.Favorites);
            this.fileWatcher.FileChanged += new EventHandler(this.FavoritesFileChanged);
            this.fileWatcher.StartObservation();
        }

        private void FavoritesFileChanged(object sender, EventArgs e)
        {
            FavoritesFile file = this.LoadFile();
            this.groups.Merge(file.Groups.Cast<IGroup>().ToList());
            this.favorites.Merge(file.Favorites.Cast<IFavorite>().ToList());
            // first update also present groups assignment,
            // than send the favorite update also for present favorites
            IList<IGroup> updated = this.UpdateFavoritesInGroups(file.FavoritesInGroups);
            this.Dispatcher.ReportGroupsUpdated(updated);
            // Simple update without ensuring, if the favorite was changes or not - possible porformance issue);
            this.Dispatcher.ReportFavoritesUpdated(new List<IFavorite>(this.favorites));
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            Settings.AssignSynchronizationObject(synchronizer);
            this.connectionHistory.AssignSynchronizationObject(synchronizer);
            this.storedCredentials.AssignSynchronizationObject(synchronizer);
            this.fileWatcher.AssignSynchronizer(synchronizer);
        }

        public void StartDelayedUpdate()
        {
            this.delaySave = true;
        }

        public void SaveAndFinishDelayedUpdate()
        {
            this.delaySave = false;
            this.SaveImmediatelyIfRequested();
        }

        public void Initialize()
        {
            FavoritesFile file = this.LoadFile();
            this.groups.Add(file.Groups.Cast<IGroup>().ToList());
            this.favorites.AddAllToCache(file.Favorites.Cast<IFavorite>().ToList());
            this.UpdateFavoritesInGroups(file.FavoritesInGroups);
        }

        public void UpdatePasswordsByNewMasterPassword(string newMasterPassword)
        {
            string newKeyMaterial = PasswordFunctions.CalculateMasterPasswordKey(newMasterPassword);
            this.storedCredentials.UpdatePasswordsByNewKeyMaterial(newKeyMaterial);
            this.favorites.UpdatePasswordsByNewMasterPassword(newKeyMaterial);
            this.SaveImmediatelyIfRequested();
        }

        internal void SaveImmediatelyIfRequested()
        {
            if (!this.delaySave)
            {
                this.Save();
            }
        }

        private FavoritesFile LoadFile()
        {
            try
            {
                this.fileLock.WaitOne();
                return this.LoadFileContent();
            }
            catch (Exception exception)
            {
                Logging.Log.Error("File peristance was unable to load Favorites.xml", exception);
                return new FavoritesFile();
            }
            finally
            {
                this.fileLock.ReleaseMutex();
                Debug.WriteLine("Favorite file was loaded.");
            }
        }

        private FavoritesFile LoadFileContent()
        {
            string fileLocation = Settings.FileLocations.Favorites;
            object fileContent = Serialize.DeserializeXMLFromDisk(fileLocation, typeof(FavoritesFile));
            var file = fileContent as FavoritesFile;
            if (file == null)
                file = new FavoritesFile();
            this.AssignGroupsToFileContent(file);
            return file;
        }

        private void AssignGroupsToFileContent(FavoritesFile file)
        {
            this.AssignGroupsToGroups(file.Favorites);
            this.AssignGroupsToFavorites(file.Groups);
        }

        private void AssignGroupsToFavorites(Group[] fileGroups)
        {
            foreach (Group group in fileGroups)
            {
                group.AssignStores(this.groups, this.Dispatcher);
            }
        }

        private void AssignGroupsToGroups(Favorite[] fileFavorites)
        {
            foreach (Favorite favorite in fileFavorites)
            {
                favorite.Groups = this.groups;
            }
        }

        private List<IGroup> UpdateFavoritesInGroups(FavoritesInGroup[] favoritesInGroups)
        {
            List<IGroup> updatedGroups = new List<IGroup>();

            foreach (FavoritesInGroup favoritesInGroup in favoritesInGroups)
            {
                var group = this.groups[favoritesInGroup.GroupId] as Group;
                bool groupUpdated = this.UpdateFavoritesInGroup(group, favoritesInGroup.Favorites);
                if (groupUpdated)
                    updatedGroups.Add(group);
            }

            return updatedGroups;
        }

        private bool UpdateFavoritesInGroup(Group group, Guid[] favoritesInGroup)
        {
            if (group != null)
            {
                List<IFavorite> newFavorites = this.GetFavoritesInGroup(favoritesInGroup);
                return group.UpdateFavorites(newFavorites);
            }

            return false;
        }

        private List<IFavorite> GetFavoritesInGroup(Guid[] favoritesInGroup)
        {
            return this.Favorites
                .Where(favorite => favoritesInGroup.Contains(favorite.Id))
                .ToList();
        }

        private void Save()
        {
            try
            {
                this.fileLock.WaitOne();
                this.fileWatcher.StopObservation();
                FavoritesFile persistenceFile = this.CreatePersistenceFileFromCache();
                string fileLocation = Settings.FileLocations.Favorites;
                Serialize.SerializeXMLToDisk(persistenceFile, fileLocation);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("File peristance was unable to save Favorites.xml", exception);
            }
            finally
            {
                this.fileWatcher.StartObservation();
                this.fileLock.WaitOne();
                Debug.WriteLine("Favorite file was saved.");
            }
        }

        private FavoritesFile CreatePersistenceFileFromCache()
        {
            return new FavoritesFile
              {
                  Favorites = this.Favorites.Cast<Favorite>().ToArray(),
                  Groups = this.Groups.Cast<Group>().ToArray(),
                  FavoritesInGroups = this.GetFavoriteInGroups()
              };
        }

        private FavoritesInGroup[] GetFavoriteInGroups()
        {
            List<FavoritesInGroup> references = new List<FavoritesInGroup>();
            foreach (Group group in this.Groups)
            {
                FavoritesInGroup groupReferences = group.GetGroupReferences();
                references.Add(groupReferences);
            }

            return references.ToArray();
        }
    }
}
