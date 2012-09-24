using System.Collections.Generic;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;
using Terminals.Data;
using DB = Terminals.Data.DB;
using Favorite = Terminals.Data.DB.Favorite;

namespace Tests
{
    /// <summary>
    ///This is a test class for database implementation of Favorites
    ///</summary>
    [TestClass]
    public class SqlFavoritesTest : SqlTestsLab
    {
        private int addedCount;
        private int updatedCount;
        private int deletedCount;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
            this.PrimaryPersistence.Dispatcher.FavoritesChanged += new FavoritesChangedEventHandler(Dispatcher_FavoritesChanged);
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
        }

        private void Dispatcher_FavoritesChanged(FavoritesChangedEventArgs args)
        {
            addedCount += args.Added.Count;
            deletedCount += args.Removed.Count;
            updatedCount += args.Updated.Count;
        }

        [TestMethod]
        public void AddFavoriteTest()
        {
            Favorite favorite = this.CreateTestFavorite();
            Favorite favorite2 = this.CreateTestFavorite();
            int before = this.CheckFavorites.Count();
            this.PrimaryPersistence.Favorites.Add(favorite);
            this.PrimaryFavorites.Add(favorite2);

            int after = this.CheckFavorites.Count();
            string protocolProperties = this.CheckDatabase.GetFavoriteProtocolProperties(favorite.Id).FirstOrDefault();
            IFavorite checkFavorite = this.SecondaryPersistence.Favorites.FirstOrDefault();

            Assert.AreNotEqual(before, after, -2, "Favorites didnt reach the database");
            Assert.IsTrue(!string.IsNullOrEmpty(protocolProperties), "Protocol properties are null");
            Assert.IsNotNull(checkFavorite.Security, "Security is null");
            Assert.IsNotNull(checkFavorite.Display, "Display is null");
            Assert.IsNotNull(checkFavorite.ExecuteBeforeConnect, "ExecuteBeforeConnect is null");
            Assert.AreEqual(2, addedCount, "Event wasnt delivered");
        }

        [TestMethod]
        public void DeleteFavoriteTest()
        {
            Favorite favorite = this.AddFavoriteToPrimaryPersistence();
            this.PrimaryFavorites.Delete(favorite);

            int after = this.CheckFavorites.Count();
            Assert.AreEqual(0, after, "Favorite wasnt deleted");
            int displayOptions = this.CheckDatabase.DisplayOptions.Count();
            Assert.AreEqual(0, displayOptions, "DisplayOptions wasnt deleted");
            int security = this.CheckDatabase.Security.Count();
            Assert.AreEqual(0, security, "Security wasnt deleted");
            int execute = this.CheckDatabase.BeforeConnectExecute.Count();
            Assert.AreEqual(0, execute, "BeforeConnectExecute wasnt deleted");
            Assert.AreEqual(1, deletedCount, "Event wasnt delivered");
        }

        private ObjectSet<Favorite> CheckFavorites { get { return this.CheckDatabase.Favorites; } }

        [TestMethod]
        public void UpdateFavoriteTest()
        {
            IFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            favorite.Protocol = ConnectionManager.VNC;
            favorite.Display.Colors = Terminals.Colors.Bits24;
            this.PrimaryFavorites.Update(favorite);

            IFavorite target = this.SecondaryFavorites.FirstOrDefault();
            Assert.IsTrue(target.Protocol == ConnectionManager.VNC, "Protocol wasnt updated");
            Assert.IsTrue(target.Display.Colors == Terminals.Colors.Bits24, "Colors property wasnt updated");

            var testOptions = target.ProtocolProperties as VncOptions;
            Assert.IsNotNull(testOptions, "Protocol properties werent updated");
            Assert.AreEqual(1, updatedCount, "Event wasnt delivered");
        }

        [TestMethod]
        public void UpdateFavoriteWithGroupsTest()
        {
            IFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            IGroup groupToDelete = PrimaryFactory.CreateGroup("TestGroupToDelete");
            this.PrimaryPersistence.Groups.Add(groupToDelete);
            this.PrimaryFavorites.UpdateFavorite(favorite, new List<IGroup> { groupToDelete });
            IGroup groupToAdd = PrimaryFactory.CreateGroup("TestGroupToAdd");
            this.PrimaryFavorites.UpdateFavorite(favorite, new List<IGroup> { groupToAdd });

            Favorite checkFavorite = this.CheckFavorites.FirstOrDefault();
            Assert.AreEqual(1, checkFavorite.Groups.Count, "Child group is missing");
            DB.Group group = checkFavorite.Groups.FirstOrDefault();
            Assert.IsTrue(group.Name == "TestGroupToAdd", "wrong merge of groups");
            int targetGroupsCount = this.CheckDatabase.Groups.Count();
            Assert.AreEqual(1, targetGroupsCount, "Empty groups wern't deleted");
            Assert.AreEqual(2, updatedCount, "Event wasnt delivered");
        }

        [TestMethod]
        public void LoadSaveFavoriteIconsTest()
        {
            IFavorite favorite = this.CreateTestFavorite();
            // try to access on not saved favorite
            favorite.ToolBarIconFile = @"Data\ControlPanel.png";
            Image favoriteIcon = favorite.ToolBarIconImage;
            this.PrimaryFavorites.Add(favorite);
            Favorite checkFavorite = this.CheckFavorites.FirstOrDefault();

            Assert.IsNotNull(favoriteIcon, "Icon wasnt assigned successfully");
            Assert.IsNotNull(checkFavorite.ToolBarIconImage, "Icon didnt reach the database");
        }
    }
}
