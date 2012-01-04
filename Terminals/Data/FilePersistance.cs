using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Terminals.Configuration;
using Unified;

namespace Terminals.Data
{
    internal class FilePersistance
    {
        internal Favorites Favorites { get; private set; }
        internal Groups Groups { get; private set; }
        internal DataDispatcher Dispatcher { get; private set; }

        private Mutex fileLock = new Mutex(false, "Terminals.CodePlex.com.FilePersistance");
        private DataFileWatcher fileWatcher;
        private const string FILENAME = "Favorites.xml";

        internal FilePersistance(DataDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
            InitializeFileWatch();
            Load();
        }

        private void InitializeFileWatch()
        {
            fileWatcher = new DataFileWatcher(GetDataFileLocation());
            fileWatcher.FileChanged += new EventHandler(FavoritesFileChanged);
            fileWatcher.StartObservation();
        }

        private void FavoritesFileChanged(object sender, EventArgs e)
        {
            // todo merge the situation and call dispatcher to fire data events
        }

        /// <summary>
        /// Because filewatcher is created before the main form in GUI thread.
        /// This lets to fire the file system watcher events in GUI thread. 
        /// </summary>
        internal void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            fileWatcher.AssignSynchronizer(synchronizer);
        }

        private void Load()
        {
            FavoritesFile file = LoadFile();
            this.Favorites = new Favorites(this.Dispatcher, file.Favorites);
            this.Groups = new Groups(this.Dispatcher, file.Groups);
            // this.UpdateFavoritesInGroups(file.FavoritesInGroups);
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

        //private void UpdateFavoritesInGroups(FavoritesInGroup[] favoritesInGroups)
        //{
        //    foreach (FavoritesInGroup favoritesInGroup in favoritesInGroups)
        //    {
        //        var group = this.Groups[favoritesInGroup.GroupId];
        //        if (group != null)
        //        {
        //            this.AddFavoritesToGroup(favoritesInGroup, group);
        //        }
        //    }
        //}

        //private void AddFavoritesToGroup(FavoritesInGroup favoritesInGroup, Group group)
        //{
        //    foreach (Guid favoriteId in favoritesInGroup.Favorites)
        //    {
        //        var favorite = this.Favorites[favoriteId];
        //        if (favorite != null)
        //        {
        //            group.AddFavorite(favorite);
        //        }
        //    }
        //}

        private static string GetDataFileLocation()
        {
            return FileLocations.GetFullPath(FILENAME);
        }

        internal void Save()
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
                  Favorites = this.Favorites.ToArray(),
                  Groups = this.Groups.ToArray(),
                  // FavoritesInGroups = GetFavoriteInGroups()
              };
        }

        //private FavoritesInGroup[] GetFavoriteInGroups()
        //{
        //    List<FavoritesInGroup> references = new List<FavoritesInGroup>();
        //    foreach (Group group in this.Groups)
        //    {
        //        FavoritesInGroup groupReferences = group.GetGroupReferences();
        //        references.Add(groupReferences);
        //    }

        //    return references.ToArray();
        //}
    }
}
