using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;

namespace Tests.FilePersisted
{
    public abstract class FilePersistedTestLab
    {
        private readonly Settings settings = Settings.Instance;
        internal FilePersistence Persistence { get; private set; }

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
            return new FilePersistence(new PersistenceSecurity());
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
            this.Persistence.Favorites.UpdateFavorite(favorite, new List<IGroup>() { group });
            return new Tuple<IFavorite, IGroup>(favorite, group);
        }

        internal IFavorite AddFavorite(string favoriteName = "Favorite")
        {
            IFavorite favorite = this.Persistence.Factory.CreateFavorite();
            favorite.Name = favoriteName;
            this.Persistence.Favorites.Add(favorite);
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
