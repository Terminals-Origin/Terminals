using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data.FilePersisted;
using Terminals.History;

namespace Terminals.Data
{
    internal class FilePersistence : IPersistence, IPersistedSecurity
    {
        /// <summary>
        /// Gets unique id of the persistence to be stored in settings (0)
        /// </summary>
        internal const int TYPE_ID = 0;

        public int TypeId { get { return TYPE_ID; } }

        public string Name { get { return "Files"; } }

        private readonly FileLocations fileLocations;
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
        private IDataFileWatcher fileWatcher;
        private bool delaySave;

        private readonly FavoritesFileSerializer serializer;

        private readonly SerializationContextBuilder contextBuilder;

        private List<XElement> cachedUnknownFavorites = new List<XElement>();

        /// <summary>
        /// Try to reuse current security in case of changing persistence, because user is already authenticated
        /// </summary>
        internal FilePersistence(PersistenceSecurity security, FavoriteIcons favoriteIcons, ConnectionManager connectionManager)
            : this(security, new DataFileWatcher(Settings.Instance.FileLocations.Favorites),
                  favoriteIcons, connectionManager)
        {}

        /// <summary>
        /// For testing purpose allowes to inject internaly used services
        /// </summary>
        internal FilePersistence(PersistenceSecurity security, IDataFileWatcher fileWatcher,
            FavoriteIcons favoriteIcons, ConnectionManager connectionManager)
        {
            this.fileLocations = Settings.Instance.FileLocations;
            this.serializer = new FavoritesFileSerializer(connectionManager);
            this.InitializeSecurity(security);
            this.Dispatcher = new DataDispatcher();
            this.storedCredentials = new StoredCredentials(security);
            this.groups = new Groups(this);
            this.favorites = new Favorites(this, favoriteIcons, connectionManager);
            this.connectionHistory = new ConnectionHistory(this.favorites);
            this.factory = new Factory(this.groups, this.Dispatcher,  connectionManager);
            this.contextBuilder = new SerializationContextBuilder(this.groups, this.favorites, this.Dispatcher);
            this.InitializeFileWatch(fileWatcher);
        }

        private void InitializeSecurity(PersistenceSecurity security)
        {
            this.Security = security;
            this.Security.AssignPersistence(this);
        }

        private void InitializeFileWatch(IDataFileWatcher fileWatcher)
        {
            this.fileWatcher = fileWatcher;
            this.fileWatcher.FileChanged += new EventHandler(this.FavoritesFileChanged);
            this.fileWatcher.StartObservation();
        }

        private void FavoritesFileChanged(object sender, EventArgs e)
        {
            FavoritesFile file = this.LoadFile();
            // dont report changes immediately, we have to wait till memberships are refreshed properly
            this.Dispatcher.StartDelayedUpdate();
            List<IGroup> addedGroups = this.groups.Merge(file.Groups.Cast<IGroup>().ToList());
            this.favorites.Merge(file.Favorites.Cast<IFavorite>().ToList());
            // first update also present groups assignment,
            // than send the favorite update also for present favorites
            List<IGroup> updated = this.UpdateFavoritesInGroups(file.FavoritesInGroups);
            updated = ListsHelper.GetMissingSourcesInTarget(updated, addedGroups);
            this.Dispatcher.ReportGroupsUpdated(updated);
            this.Dispatcher.EndDelayedUpdate();
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            Settings.Instance.AssignSynchronizationObject(synchronizer);
            this.connectionHistory.AssignSynchronizationObject(synchronizer);
            this.storedCredentials.AssignSynchronizationObject(synchronizer);
            this.fileWatcher.AssignSynchronizer(synchronizer);
        }

        public void StartDelayedUpdate()
        {
            this.delaySave = true;
            this.Dispatcher.StartDelayedUpdate();
        }

        public void SaveAndFinishDelayedUpdate()
        {
            this.delaySave = false;
            this.SaveImmediatelyIfRequested();
            this.Dispatcher.EndDelayedUpdate();
        }

        public bool Initialize()
        {
            this.storedCredentials.Initialize();
            FavoritesFile file = this.LoadFile();
            this.groups.AddAllToCache(file.Groups.Cast<IGroup>().ToList());
            this.favorites.AddAllToCache(file.Favorites.Cast<IFavorite>().ToList());
            this.UpdateFavoritesInGroups(file.FavoritesInGroups);
            return true;
        }

        public void UpdatePasswordsByNewMasterPassword(string newMasterKey)
        {
            this.storedCredentials.UpdatePasswordsByNewKeyMaterial(newMasterKey);
            this.favorites.UpdatePasswordsByNewMasterPassword(newMasterKey);
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
            var loaded = this.TryLoadFile();
            this.cachedUnknownFavorites = loaded.Unknown;
            this.contextBuilder.AssignServices(loaded.File);
            return loaded.File;
        }

        private SerializationContext TryLoadFile()
        {
            try
            {
                this.fileLock.WaitOne();
                string fileLocation = this.fileLocations.Favorites;
                return this.serializer.Deserialize(fileLocation);
            }
            catch (Exception exception)
            {
                Logging.Error("File persistence was unable to load Favorites.xml", exception);
                return new SerializationContext();
            }
            finally
            {
                this.fileLock.ReleaseMutex();
                Debug.WriteLine("Favorite file was loaded.");
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
            SerializationContext context = this.contextBuilder.CreateDataFromCache(this.cachedUnknownFavorites);
            this.TrySave(context);
        }

        private void TrySave(SerializationContext context)
        {
            try
            {
                this.fileLock.WaitOne();
                this.fileWatcher.StopObservation();
                string fileLocation = this.fileLocations.Favorites;
                this.serializer.Serialize(context, fileLocation);
            }
            catch (Exception exception)
            {
                Logging.Error("File persistence was unable to save Favorites.xml", exception);
            }
            finally
            {
                this.fileWatcher.StartObservation();
                this.fileLock.ReleaseMutex();
                Debug.WriteLine("Favorite file was saved.");
            }
        }
    }
}
