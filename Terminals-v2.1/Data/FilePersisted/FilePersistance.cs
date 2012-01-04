﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Terminals.Configuration;
using Terminals.History;
using Unified;

namespace Terminals.Data
{
    internal class FilePersistance : IPersistance
    {
        private Favorites favorites;
        public IFavorites Favorites
        {
            get
            {
                return this.favorites;
            }
        }

        private Groups groups;
        public IGroups Groups
        {
            get
            {
                return this.groups;
            }
        }

        private FavoritesFactory factory;
        public IFactory Factory
        {
            get { return this.factory; }
        }

        public ConnectionHistory ConnectionHistory { get; private set; }
        public StoredCredentials Credentials { get; private set; }
        public DataDispatcher Dispatcher { get; private set; }

        private Mutex fileLock = new Mutex(false, "Terminals.CodePlex.com.FilePersistance");
        private DataFileWatcher fileWatcher;
        private const string FILENAME = "Favorites.xml";
        private bool delaySave = false;

        internal FilePersistance()
        {
            this.Dispatcher = new DataDispatcher();
            this.ConnectionHistory = new ConnectionHistory();
            this.Credentials = new StoredCredentials();
            InitializeFileWatch();
            Load();
            this.factory = new FavoritesFactory(this.groups, this.favorites);
        }

        private void InitializeFileWatch()
        {
            fileWatcher = new DataFileWatcher(GetDataFileLocation());
            fileWatcher.FileChanged += new EventHandler(FavoritesFileChanged);
            fileWatcher.StartObservation();
        }

        private void FavoritesFileChanged(object sender, EventArgs e)
        {
            FavoritesFile file = LoadFile();
            this.groups.Merge(file.Groups.Cast<IGroup>().ToList());
            this.favorites.Merge(file.Favorites.Cast<IFavorite>().ToList());
            // first update also present groups assignment,
            // than send the favorite update also for present favorites
            this.UpdateFavoritesInGroups(file.FavoritesInGroups);
            // Simple update without ensuring, if the favorite was changes or not - possible porformance issue
            this.Dispatcher.ReportFavoritesUpdated(this.favorites.ToList());
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            Settings.AssignSynchronizationObject(synchronizer);
            this.ConnectionHistory.AssignSynchronizationObject(synchronizer);
            this.Credentials.AssignSynchronizationObject(synchronizer);
            this.fileWatcher.AssignSynchronizer(synchronizer);
        }

        public void StartDelayedUpdate()
        {
            delaySave = true;
        }

        public void SaveAndFinishDelayedUpdate()
        {
            delaySave = false;
            SaveImmediatelyIfRequested();
        }

        internal void SaveImmediatelyIfRequested()
        {
            if (!delaySave)
            {
                Save();
            }
        }

        private void Load()
        {
            FavoritesFile file = LoadFile();
            this.favorites = new Favorites(this, file.Favorites);
            this.groups = new Groups(this, file.Groups);
            this.UpdateFavoritesInGroups(file.FavoritesInGroups);
        }

        private FavoritesFile LoadFile()
        {
            try
            {
                fileLock.WaitOne();
                string fileLocation = GetDataFileLocation();
                object file = Serialize.DeserializeXMLFromDisk(fileLocation, typeof(FavoritesFile));
                return file as FavoritesFile;
            }
            catch (Exception exception)
            {
                Logging.Log.Error("File peristance was unable to load Favorites.xml", exception);
                return new FavoritesFile();
            }
            finally
            {
                fileLock.ReleaseMutex();
            }
        }

        private void UpdateFavoritesInGroups(FavoritesInGroup[] favoritesInGroups)
        {
            foreach (FavoritesInGroup favoritesInGroup in favoritesInGroups)
            {
                var group = this.Groups[favoritesInGroup.GroupId] as Group;
                this.UpdateFavoritesInGroup(group, favoritesInGroup.Favorites);
            }
        }

        private void UpdateFavoritesInGroup(Group group, Guid[] favoritesInGroup)
        {
            if (group != null)
            {
                List<IFavorite> newFavorites = GetFavoritesInGroup(favoritesInGroup);
                group.UpdateFavorites(newFavorites);
            }
        }

        private List<IFavorite> GetFavoritesInGroup(Guid[] favoritesInGroup)
        {
            return this.Favorites
                .Where(favorite => favoritesInGroup.Contains(favorite.Id))
                .ToList();
        }

        private static string GetDataFileLocation()
        {
            return FileLocations.GetFullPath(FILENAME);
        }

        private void Save()
        {
            try
            {
                fileLock.WaitOne();
                fileWatcher.StopObservation();
                FavoritesFile persistanceFile = CreatePersistanceFileFromCache();
                string fileLocation = GetDataFileLocation();
                Serialize.SerializeXMLToDisk(persistanceFile, fileLocation);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("File peristance was unable to save Favorites.xml", exception);
            }
            finally
            {
                fileWatcher.StartObservation();
                fileLock.WaitOne();
            }
        }

        private FavoritesFile CreatePersistanceFileFromCache()
        {
            return new FavoritesFile
              {
                  Favorites = this.Favorites.Cast<Favorite>().ToArray(),
                  Groups = this.Groups.Cast<Group>().ToArray(),
                  FavoritesInGroups = GetFavoriteInGroups()
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
