using System;
using System.Collections.Generic;
using System.Linq;
using Terminals.Configuration;
using Unified;

namespace Terminals.Data
{
    internal class FilePersistance
    {
        internal Favorites Favorites { get; private set; }
        internal Groups Groups { get; private set; }
        internal DataDispatcher Dispatcher { get; private set; }
        private const string FILENAME = "Favorites.xml";

        internal FilePersistance(DataDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
            Load();
        }

        private void Load()
        {
            FavoritesFile file = LoadFile();
            this.Favorites = new Favorites(this.Dispatcher, file.Favorites);
            this.Groups = new Groups(this.Dispatcher, file.Groups);
            this.UpdateFavoritesInGroups(file.FavoritesInGroups);
        }

        private static FavoritesFile LoadFile()
        {
            try
            {
                string fileLocation = GetDataFileLocation();
                object file = Serialize.DeserializeXMLFromDisk(fileLocation, typeof(FavoritesFile));
                return file as FavoritesFile;
            }
            catch (Exception exception)
            {
                Logging.Log.Error("File peristance was unable to load Favorites.xml", exception);
                return new FavoritesFile();
            }
        }

        private void UpdateFavoritesInGroups(FavoritesInGroup[] favoritesInGroups)
        {
            foreach (FavoritesInGroup favoritesInGroup in favoritesInGroups)
            {
                var group = this.Groups[favoritesInGroup.GroupId];
                if (group != null)
                {
                    this.AddFavoritesToGroup(favoritesInGroup, group);
                }
            }
        }

        private void AddFavoritesToGroup(FavoritesInGroup favoritesInGroup, Group group)
        {
            foreach (Guid favoriteId in favoritesInGroup.Favorites)
            {
                var favorite = this.Favorites[favoriteId];
                if (favorite != null)
                {
                    group.AddFavorite(favorite);
                }
            }
        }

        private static string GetDataFileLocation()
        {
            return FileLocations.GetFullPath(FILENAME);
        }

        internal void Save()
        {
            try
            {
                FavoritesFile persistanceFile = CreatePersistanceFileFromCache();
                string fileLocation = GetDataFileLocation();
                Serialize.SerializeXMLToDisk(persistanceFile, fileLocation);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("File peristance was unable to save Favorites.xml", exception);
            }
        }

        private FavoritesFile CreatePersistanceFileFromCache()
        {
            return new FavoritesFile
              {
                  Favorites = this.Favorites.ToArray(),
                  Groups = this.Groups.ToArray(),
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
