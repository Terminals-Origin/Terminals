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
        internal FilePersistence Persistence { get; private set; }

        [TestInitialize]
        public void InitializeTestLab()
        {
            SetDefaultFileLocations();
            File.Delete(Settings.FileLocations.Favorites);
            this.Persistence = new FilePersistence();
            this.Persistence.Initialize();
        }

        internal static void SetDefaultFileLocations()
        {
            Settings.FileLocations.AssignCustomFileLocations(string.Empty, string.Empty, string.Empty);
        }

        internal Tuple<IFavorite, IGroup> AddFavoriteWithGroup(string groupName)
        {
            IFavorite favorite = this.AddFavorite();
            IGroup group = this.Persistence.Factory.CreateGroup(groupName);
            group.AddFavorite(favorite);
            this.Persistence.Favorites.UpdateFavorite(favorite, new List<IGroup>() { group });
            return new Tuple<IFavorite, IGroup>(favorite, group);
        }

        internal IFavorite AddFavorite()
        {
            IFavorite favorite = this.Persistence.Factory.CreateFavorite();
            this.Persistence.Favorites.Add(favorite);
            return favorite;
        }
    }
}
