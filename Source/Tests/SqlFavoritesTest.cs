using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
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
    public class SqlFavoritesTest
    {
        private TestContext testContextInstance;
        private DB.SqlPersistance persistence;
        private DB.DataBase checkDatabase;
        private int addedCount;
        private int updatedCount;
        private int deletedCount;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Settings.FileLocations.AssignCustomFileLocations(string.Empty, string.Empty, string.Empty);
            Settings.ConnectionString = DB.DataBase.DEVELOPMENT_CONNECTION_STRING;
            persistence = new DB.SqlPersistance();
            persistence.Dispatcher.FavoritesChanged += new FavoritesChangedEventHandler(Dispatcher_FavoritesChanged);
            checkDatabase = DB.DataBase.CreateDatabaseInstance();
        }

        [TestCleanup]
        public void TestClose()
        {
            const string deleteCommand = @"DELETE FROM ";
            string favoritesTable = checkDatabase.Favorites.EntitySet.Name;
            checkDatabase.ExecuteStoreCommand(deleteCommand + favoritesTable);
            string beforeConnectTable = checkDatabase.BeforeConnectExecute.EntitySet.Name;
            checkDatabase.ExecuteStoreCommand(deleteCommand + beforeConnectTable);
            string securityTable = checkDatabase.Security.EntitySet.Name;
            checkDatabase.ExecuteStoreCommand(deleteCommand + securityTable);
            string displayOptionsTable = checkDatabase.DisplayOptions.EntitySet.Name;
            checkDatabase.ExecuteStoreCommand(deleteCommand + displayOptionsTable);
            string groupsTable = checkDatabase.DisplayOptions.EntitySet.Name;
            checkDatabase.ExecuteStoreCommand(deleteCommand + groupsTable);
            checkDatabase.ExecuteStoreCommand(@"DELETE FROM FavoritesInGroup");

            //persistence.Dispatcher.FavoritesChanged -= new FavoritesChangedEventHandler(Dispatcher_FavoritesChanged);
        }

        private Favorite CreateTestFavorite()
        {
            var favorite = this.persistence.Factory.CreateFavorite() as Favorite;
            favorite.Name = "test";
            favorite.ServerName = "test server";
            return favorite;
        }

        private void Dispatcher_FavoritesChanged(FavoritesChangedEventArgs args)
        {
            addedCount = args.Added.Count;
            deletedCount = args.Removed.Count;
            updatedCount = args.Updated.Count;
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod]
        public void AddFavoriteTest()
        {
            Favorite favorite = this.CreateTestFavorite();
            int before = this.checkDatabase.Favorites.Count();
            persistence.Favorites.Add(favorite);

            int after = checkDatabase.Favorites.Count();
            string protocolProperties = checkDatabase.GetFavoriteProtocolProperties(favorite.Id).FirstOrDefault();
            IFavorite checkFavorite = checkDatabase.Favorites.ToList().FirstOrDefault();

            Assert.AreNotEqual(before, after, "Favorite didnt reach the database");
            Assert.IsTrue(!string.IsNullOrEmpty(protocolProperties), "Protocol properties are null");
            Assert.IsNotNull(checkFavorite.Security, "Security is null");
            Assert.IsNotNull(checkFavorite.Display, "Display is null");
            Assert.IsNotNull(checkFavorite.ExecuteBeforeConnect, "ExecuteBeforeConnect is null");
            Assert.AreEqual(1, addedCount, "Event wasnt delivered");
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod]
        public void DeleteFavoriteTest()
        {
            Favorite favorite = this.CreateTestFavorite();
            persistence.Favorites.Add(favorite);
            persistence.Favorites.Delete(favorite);

            int after = checkDatabase.Favorites.Count();
            Assert.AreEqual(0, after, "Favorite wasnt deleted");
            int displayOptions = checkDatabase.DisplayOptions.Count();
            Assert.AreEqual(0, displayOptions, "DisplayOptions wasnt deleted");
            int security = checkDatabase.Security.Count();
            Assert.AreEqual(0, security, "Security wasnt deleted");
            int execute = checkDatabase.BeforeConnectExecute.Count();
            Assert.AreEqual(0, execute, "BeforeConnectExecute wasnt deleted");
            Assert.AreEqual(1, deletedCount, "Event wasnt delivered");
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod]
        public void UpdateFavoriteTest()
        {
            Favorite favorite = this.CreateTestFavorite();
            persistence.Favorites.Add(favorite);
            favorite.Protocol = ConnectionManager.VNC;
            persistence.Favorites.Update(favorite);

            Favorite target = checkDatabase.Favorites.FirstOrDefault();
            Assert.IsTrue(target.Protocol == ConnectionManager.VNC, "Protocol wasnt updated");
            var testOptions = target.ProtocolProperties as VncOptions;
            Assert.IsNotNull(testOptions, "Protocol properties werent updated");
            Assert.AreEqual(1, updatedCount, "Event wasnt delivered");
        }

        /// <summary>
        ///A test for UpdateFavorite
        ///</summary>
        [TestMethod]
        public void UpdateFavoriteWithGroupsTest()
        {
            IFavorite favorite = this.CreateTestFavorite();
            persistence.Favorites.Add(favorite);
            IGroup groupToDelete = persistence.Factory.CreateGroup("TestGroupToDelete", new List<IFavorite> { favorite });
            persistence.Groups.Add(groupToDelete);
            IGroup groupToAdd = persistence.Factory.CreateGroup("TestGroupToAdd", new List<IFavorite>());
            persistence.Favorites.UpdateFavorite(favorite, new List<IGroup> { groupToAdd });

            Favorite checkFavorite = checkDatabase.Favorites.FirstOrDefault();
            Assert.IsTrue(checkFavorite.Groups.Count == 1, "Groups count differs");
            DB.Group group = checkFavorite.Groups.FirstOrDefault();
            Assert.IsTrue(group.Name == "TestGroupToAdd", "wrong merge of groups");
            Assert.AreEqual(1, updatedCount, "Event wasnt delivered");
        }
    }
}
