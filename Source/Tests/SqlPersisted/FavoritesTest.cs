using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    /// <summary>
    ///This is a test class for database implementation of Favorites
    ///</summary>
    [TestClass]
    public class FavoritesTest : TestsLab
    {
        private int addedCount;
        private int updatedCount;
        private int deletedCount;

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
            this.PrimaryPersistence.Dispatcher.FavoritesChanged += new FavoritesChangedEventHandler(this.DispatcherFavoritesChanged);
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
        }

        private void DispatcherFavoritesChanged(FavoritesChangedEventArgs args)
        {
            this.addedCount += args.Added.Count;
            this.deletedCount += args.Removed.Count;
            this.updatedCount += args.Updated.Count;
        }

        [TestMethod]
        public void AddFavoriteTest()
        {
            DbFavorite favorite = this.CreateTestFavorite();
            DbFavorite favorite2 = this.CreateTestFavorite();
            int before = this.CheckFavorites.Count();
            this.PrimaryPersistence.Favorites.Add(favorite);
            this.PrimaryFavorites.Add(favorite2);

            int after = this.CheckFavorites.Count();
            string protocolProperties = this.CheckDatabase.GetProtocolPropertiesByFavorite(favorite.Id);
            IFavorite checkFavorite = this.SecondaryPersistence.Favorites.FirstOrDefault();

            Assert.AreNotEqual(before, after, -2, "Favorites didn't reach the database");
            Assert.IsTrue(!string.IsNullOrEmpty(protocolProperties), "Protocol properties are null");
            Assert.IsNotNull(checkFavorite.Security, "Security is null");
            Assert.IsNotNull(checkFavorite.Display, "Display is null");
            Assert.IsNotNull(checkFavorite.ExecuteBeforeConnect, "ExecuteBeforeConnect is null");
            Assert.AreEqual(2, this.addedCount, "Event wasn't delivered");
        }

        [TestMethod]
        public void DeleteFavoriteTest()
        {
            DbFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            this.PrimaryFavorites.Delete(favorite);

            int after = this.CheckFavorites.Count();
            Assert.AreEqual(0, after, "Favorite wasn't deleted");
            int displayOptions = this.CheckDatabase.DisplayOptions.Count();
            Assert.AreEqual(0, displayOptions, "DisplayOptions wasn't deleted");
            int security = this.CheckDatabase.Security.Count();
            Assert.AreEqual(0, security, "Security wasn't deleted");
            int execute = this.CheckDatabase.BeforeConnectExecute.Count();
            Assert.AreEqual(0, execute, "BeforeConnectExecute wasn't deleted");
            Assert.AreEqual(1, this.deletedCount, "Event wasn't delivered");
        }

        private DbSet<DbFavorite> CheckFavorites { get { return this.CheckDatabase.Favorites; } }

        [TestMethod]
        public void UpdateFavoriteTest()
        {
            IFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            favorite.Protocol = ConnectionManager.VNC;
            favorite.Display.Colors = Terminals.Colors.Bits24;
            this.PrimaryFavorites.Update(favorite);

            IFavorite target = this.SecondaryFavorites.FirstOrDefault();
            Assert.IsTrue(target.Protocol == ConnectionManager.VNC, "Protocol wasn't updated");
            Assert.IsTrue(target.Display.Colors == Terminals.Colors.Bits24, "Colors property wasn't updated");

            var testOptions = target.ProtocolProperties as VncOptions;
            Assert.IsNotNull(testOptions, "Protocol properties weren't updated");
            Assert.AreEqual(1, this.updatedCount, "Event wasn't delivered");
        }

        [TestMethod]
        public void UpdateFavoriteWithGroupsTest()
        {
            IFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            IGroup groupToDelete = this.PrimaryFactory.CreateGroup("TestGroupToDelete");
            this.PrimaryPersistence.Groups.Add(groupToDelete);
            this.PrimaryFavorites.UpdateFavorite(favorite, new List<IGroup> { groupToDelete });
            IGroup groupToAdd = this.PrimaryFactory.CreateGroup("TestGroupToAdd");
            this.PrimaryFavorites.UpdateFavorite(favorite, new List<IGroup> { groupToAdd });

            DbFavorite checkFavorite = this.CheckFavorites.Include(f => f.Groups).FirstOrDefault();
            Assert.AreEqual(1, checkFavorite.Groups.Count, "Child group is missing");
            IGroup group = checkFavorite.Groups.FirstOrDefault();
            Assert.IsTrue(group.Name == "TestGroupToAdd", "wrong merge of groups");
            int targetGroupsCount = this.CheckDatabase.Groups.Count();
            Assert.AreEqual(1, targetGroupsCount, "Empty groups weren't deleted");
            Assert.AreEqual(2, this.updatedCount, "Event wasn't delivered");
        }

        [TestMethod]
        public void LoadSaveFavoriteIconsTest()
        {
            IFavorite favorite = this.CreateTestFavorite();
            // try to access on not saved favorite
            favorite.ToolBarIconFile = @"Data\ControlPanel.png";
            Image favoriteIcon = favorite.ToolBarIconImage;
            this.PrimaryFavorites.Add(favorite);
            DbFavorite checkFavorite = this.CheckFavorites.FirstOrDefault();

            Assert.IsNotNull(favoriteIcon, "Icon wasn't assigned successfully");
            Assert.IsNotNull(checkFavorite.ToolBarIconImage, "Icon didn't reach the database");
        }
    }
}
