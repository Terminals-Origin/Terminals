using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections.VNC;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Data.DB;
using Tests.Connections;
using Tests.Helpers;

namespace Tests.SqlPersisted
{
    /// <summary>
    ///This is a test class for database implementation of Favorites
    ///</summary>
    [TestClass]
    public class FavoritesTest : TestsLab
    {
        private const string IMAGE_FILE = ImageAssert.IMAGE_FILE;
        private int addedCount;
        private int updatedCount;
        private int deletedCount;

        private DbSet<DbFavorite> CheckFavorites { get { return this.CheckDatabase.Favorites; } }

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
        public void AddTwoFavorites_AddPropertiesToDatabaseAndFiresEvent()
        {
            DbFavorite favorite = this.CreateTestFavorite();
            DbFavorite favorite2 = this.CreateTestFavorite();
            int before = this.CheckFavorites.Count();
            this.PrimaryFavorites.Add(favorite);
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
        public void StoredFavorite_Delete_RemovesFromDatabaseTablesAndFiresEvent()
        {
            DbFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            // force the password to be stored to ensure
            ((IFavorite)favorite).Security.EncryptedPassword = VALIDATION_VALUE;
            this.PrimaryFavorites.Update(favorite);
            this.PrimaryFavorites.Delete(favorite);

            int after = this.CheckFavorites.Count();
            Assert.AreEqual(0, after, "Favorite wasn't deleted");
            int displayOptions = this.CheckDatabase.DisplayOptions.Count();
            Assert.AreEqual(0, displayOptions, "DisplayOptions wasn't deleted");
            int security = this.CheckDatabase.Security.Count();
            Assert.AreEqual(0, security, "Security wasn't deleted");
            int execute = this.CheckDatabase.BeforeConnectExecute.Count();
            Assert.AreEqual(0, execute, "BeforeConnectExecute wasn't deleted");
            int credentialBase = this.CheckDatabase.CredentialBase.Count();
            Assert.AreEqual(0, credentialBase, "CredentialBase wasn't deleted");
            Assert.AreEqual(1, this.deletedCount, "Event wasn't delivered");
        }

        [TestMethod]
        public void VNCFavorite_Update_SavesPropertiesToDatabaseAndFiresEvents()
        {
            IFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            TestConnectionManager.Instance.ChangeProtocol(favorite, VncConnectionPlugin.VNC);
            favorite.Display.Colors = Terminals.Colors.Bits24;
            this.PrimaryFavorites.Update(favorite);

            IFavorite target = this.SecondaryFavorites.FirstOrDefault();
            Assert.IsTrue(target.Protocol == VncConnectionPlugin.VNC, "Protocol wasn't updated");
            Assert.IsTrue(target.Display.Colors == Terminals.Colors.Bits24, "Colors property wasn't updated");

            // Because of dynamic loading, types compare is not possible.
            var testOptions = target.ProtocolProperties.GetType().FullName;
            Assert.AreEqual("Terminals.Data.VncOptions", testOptions, "Protocol properties weren't updated");
            Assert.AreEqual(1, this.updatedCount, "Event wasn't delivered");
        }

        /// <summary>
        /// Checks if Security is properly saved, if not saved in previous commit, 
        /// because this property is initialized using lazy loading
        /// </summary>
        [TestMethod]
        public void UpdateFavoriteThreeTimes_FiresEventsAndStoresSecurity()
        {
            IFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            // first time change nothing to ensure, that dummy update doesn't fail.
            // EF Security.CachedCredentials property is still null.
            this.PrimaryFavorites.Update(favorite);
            var guarded = new GuardedSecurity(this.PrimaryPersistence, favorite.Security);

            // now assign new values to security and commit it as newly added, should not fail
            guarded.UserName = VALIDATION_VALUE;
            this.PrimaryFavorites.Update(favorite);
            // try again to ensure, that second update also doesn't fail.
            guarded.UserName = VALIDATION_VALUE_B;
            this.PrimaryFavorites.Update(favorite);

            IFavorite target = this.SecondaryFavorites.FirstOrDefault();
            Assert.AreEqual(VALIDATION_VALUE_B, guarded.UserName, "Protocol properties weren't updated");
            Assert.AreEqual(3, this.updatedCount, "Event wasn't delivered");
        }

        [TestMethod]
        public void AssignGroupToStoredFavorite_Update_SavesToDatabase()
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
            Assert.AreEqual(2, targetGroupsCount, "Groups count was changed");
            Assert.AreEqual(2, this.updatedCount, "Event wasn't delivered");
        }

        [DeploymentItem(IMAGE_FILE)]
        [TestMethod]
        public void UpdateFavoriteIcon_StoresIconToDatabase()
        {
            IFavorite favorite = this.CreateTestFavorite();
            // try to access on not saved favorite
            this.PrimaryFavorites.Add(favorite);

            this.PrimaryFavorites.UpdateFavoriteIcon(favorite, IMAGE_FILE);
            this.PrimaryFavorites.Update(favorite);
            Image favoriteIcon = this.PrimaryFavorites.LoadFavoriteIcon(favorite);

            DbFavorite checkFavorite = this.CheckFavorites.FirstOrDefault();

            Assert.IsNotNull(favoriteIcon, "Icon wasn't assigned successfully");
            var loadedIcon = this.SecondaryFavorites.LoadFavoriteIcon(checkFavorite);
            Assert.IsNotNull(loadedIcon, "Icon didn't reach the database");
            ImageAssert.EqualsToExpectedIcon(this.TestContext.DeploymentDirectory, favoriteIcon);
        }

        [DeploymentItem(IMAGE_FILE)]
        [TestMethod]
        public void UpdateFavoriteIcon_DoesntFireFavoriteUpdate()
        {
            IFavorite favorite = this.CreateTestFavorite();
            this.PrimaryFavorites.Add(favorite);
            int updatesCount = 0;
            this.PrimaryPersistence.Dispatcher.FavoritesChanged += args => updatesCount++;

            this.PrimaryFavorites.UpdateFavoriteIcon(favorite, IMAGE_FILE);
   
            Assert.AreEqual(0, updatesCount, FilePersisted.FavoritesTest.UPDATE_ICON_MESSAGE);
        }

        [DeploymentItem(IMAGE_FILE)]
        [TestMethod]
        public void UpdateFavoriteIcon_DoesntStoreIconToDatabase()
        {
            IFavorite favorite = this.CreateTestFavorite();
            this.PrimaryFavorites.Add(favorite);

            this.PrimaryFavorites.UpdateFavoriteIcon(favorite, IMAGE_FILE);

            IFavorite loaded = this.SecondaryFavorites.First();
            Image loadedIcon = this.SecondaryFavorites.LoadFavoriteIcon(loaded);
            string deploymentDirectory = this.TestContext.DeploymentDirectory;
            ImageAssert.DoesntEqualsExpectedIcon(deploymentDirectory, loadedIcon);
        }

        [TestMethod]
        public void VNCProtocolToRdp_ChangeProtocol_AllowesUpdatesRdpSecurity()
        {
            IFavorite favorite = this.CreateTestFavorite();
            // now it has RdpOptions
            TestConnectionManager.Instance.ChangeProtocol(favorite, VncConnectionPlugin.VNC);
            this.PrimaryFavorites.Update(favorite);
            FilePersisted.FavoritesTest.AssertRdpSecurity(this.PrimaryPersistence, favorite);
        }
    }
}
