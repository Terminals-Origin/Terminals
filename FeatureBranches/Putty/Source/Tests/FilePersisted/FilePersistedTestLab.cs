using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
using Tests.Connections;

namespace Tests.FilePersisted
{
    public abstract class FilePersistedTestLab
    {
        private readonly Settings settings = Settings.Instance;

        internal FilePersistence Persistence { get; private set; }

        internal IFavorites Favorites { get { return this.Persistence.Favorites; } }


        [TestInitialize]
        public void InitializeTestLab()
        {
            SetDefaultFileLocations();
            File.Delete(settings.FileLocations.Favorites);
            this.Persistence = CreateFilePersistence();
            this.Persistence.Initialize();
        }

        internal static FilePersistence CreateFilePersistence()
        {
            return CreateFilePersistence(new TestFileWatch());
        }

        internal static FilePersistence CreateFilePersistence(IDataFileWatcher fileWatcher)
        {
            var icons = TestConnectionManager.CreateTestFavoriteIcons();
            return new FilePersistence(new PersistenceSecurity(), fileWatcher, icons, TestConnectionManager.Instance);
        }

        internal static void SetDefaultFileLocations()
        {
            Settings.Instance.FileLocations.AssignCustomFileLocations(string.Empty, string.Empty, string.Empty);
        }

        internal Tuple<IFavorite, IGroup> AddFavoriteWithGroup(string groupName)
        {
            IFavorite favorite = this.AddFavorite();
            IGroup group = this.Persistence.Factory.CreateGroup(groupName);
            group.AddFavorite(favorite);
            this.Favorites.UpdateFavorite(favorite, new List<IGroup>() { group });
            return new Tuple<IFavorite, IGroup>(favorite, group);
        }

        internal IFavorite AddFavorite(string favoriteName = "Favorite")
        {
            IFavorite favorite = this.Persistence.Factory.CreateFavorite();
            favorite.Name = favoriteName;
            this.Favorites.Add(favorite);
            return favorite;
        }

        internal IGroup AddNewGroup(string groupName)
        {
            IGroup newGroup = this.Persistence.Factory.CreateGroup(groupName);
            this.Persistence.Groups.Add(newGroup);
            return newGroup;
        }
    }
}
