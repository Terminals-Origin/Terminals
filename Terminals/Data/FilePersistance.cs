using System;
using System.Collections.Generic;
using System.Linq;
using Terminals.Connections;
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
            //var file = Load();
            this.Favorites = new Favorites(dispatcher, new List<Favorite>());
            this.Groups = new Groups(dispatcher, new List<Group>());
            this.Dispatcher = dispatcher;
        }

        private static string GetDataFileLocation()
        {
            string root = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            System.IO.Directory.CreateDirectory(root + @"\Data\");
            return String.Format(@"{0}\Data\{1}",root , FILENAME);
        }

        internal void Save()
        {
            try
            {
                FavoritesFile persistanceFile = new FavoritesFile
                    {
                        Favorites = this.Favorites.Cache.Values.ToList(),
                        Groups = this.Groups.Cache
                    };

                string fileLocation = GetDataFileLocation();
                Serialize.SerializeXMLToDisk(persistanceFile, fileLocation);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("File peristance was unable to save Favorites.xml", exception);
            }
        }

        private FavoritesFile Load()
        {
            try
            {
                string fileLocation = GetDataFileLocation();
                object file = Serialize.DeserializeXMLFromDisk(fileLocation, typeof(FavoritesFile));
                return file as FavoritesFile;
            }
            catch (Exception exception)
            {
                Logging.Log.Error("File peristance was unable to save Favorites.xml", exception);
                return new FavoritesFile();
            }
        }
    }
}
