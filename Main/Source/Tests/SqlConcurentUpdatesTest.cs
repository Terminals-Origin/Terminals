using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.DB;
using Terminals.Security;
using Favorite = Terminals.Data.DB.Favorite;

namespace Tests
{
    [TestClass]
    public class SqlConcurentUpdatesTest : SqlTestsLab
    {
        private IFavorite updatedFavorite;
        private bool addEventCatched;
        private bool updateEventCatched;
        private bool removedEventCatched;

        private const string TEST_NAME = "second";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
        }

        [TestCleanup]
        public void TestClose()
        {
            Settings.DatabaseMasterPassword = string.Empty;
            Database.UpdateMastrerPassord(Settings.ConnectionString, string.Empty);

            // do not clean up the store here, because of waiting for background threads
            // this.lab.ClearTestLab();
        }

        [TestMethod]
        public void TestPeriodicalUpdates()
        {
            this.AddFavoriteToPrimaryPersistence();
            this.AddFavoriteToPrimaryPersistence();

            // assign event handler before another changes to catch all of them
            this.PrimaryPersistence.Dispatcher.FavoritesChanged +=
                new FavoritesChangedEventHandler(this.OnPrimaryStoreFavoritesChanged);

            this.MakeChangesOnSecondaryPersistence();

            ISynchronizeInvoke control = new Control();
            this.PrimaryPersistence.AssignSynchronizationObject(control);
            // refresh interval is set to 2 sec. by default
            Thread.Sleep(10000);

            Assert.IsTrue(this.addEventCatched, "Favorite added event wasnt received");
            Assert.IsTrue(this.updateEventCatched, "Favorite updated event wasnt received");
            Assert.IsTrue(this.removedEventCatched, "Favorite removed event wasnt received");
            Assert.AreEqual(TEST_NAME, this.updatedFavorite.Name, "The updated favorite wasnt refreshed");
        }

        private void MakeChangesOnSecondaryPersistence()
        {
            List<IFavorite> secondary = this.SecondaryFavorites.ToList();
            this.SecondaryFavorites.Delete(secondary[0]);
            var favoriteA = secondary[1] as Favorite;
            favoriteA.Name = TEST_NAME;
            this.SecondaryFavorites.Update(favoriteA);

            IFavorite favoriteB = this.SecondaryPersistence.Factory.CreateFavorite();
            favoriteB.Name = "test";
            favoriteB.ServerName = "test server";
            this.SecondaryFavorites.Add(favoriteB);
        }

        private void OnPrimaryStoreFavoritesChanged(FavoritesChangedEventArgs args)
        {
            var updated = args.Updated.FirstOrDefault();
            if (updated != null)
                this.updatedFavorite = updated;

            this.addEventCatched |= args.Added.Count > 0;
            this.updateEventCatched |= args.Updated.Count > 0;
            this.removedEventCatched |= args.Removed.Count > 0;
        }

        [TestMethod]
        public void TestSaveOnAlreadyUpdatedFavorite()
        {
            Favorite favoriteA = this.AddFavoriteToPrimaryPersistence();
            var favoriteB = this.SecondaryFavorites.FirstOrDefault() as Favorite;
            this.PrimaryPersistence.Dispatcher.FavoritesChanged +=
                new FavoritesChangedEventHandler(this.OnUpdateAlreadyUpdatedFavoritesChanged);

            this.UpdateFavorite(favoriteA, this.PrimaryFavorites);
            this.UpdateFavorite(favoriteB, this.SecondaryFavorites);
            // last update wins
            this.UpdateFavorite(favoriteA, this.PrimaryFavorites);
            this.SecondaryFavorites.Delete(favoriteB);
            // next update shouldnt fail on deleted favorite
            this.UpdateFavorite(favoriteA, this.PrimaryFavorites);
            Assert.IsTrue(this.removedEventCatched, "Persistence didnt catched the already removed favorite update event.");
        }

        private void OnUpdateAlreadyUpdatedFavoritesChanged(FavoritesChangedEventArgs args)
        {
            if (args.Removed.Count > 0)
                this.removedEventCatched = true;
        }

        private void UpdateFavorite(Favorite toUpdate, IFavorites targetPersistence)
        {
            toUpdate.Name += " Changed";
            targetPersistence.Update(toUpdate);
        }

        [TestMethod]
        public void TestMasterPasswordUpdate()
        {
            const string dummyPassword = "bbb";
            Database.UpdateMastrerPassord(Settings.ConnectionString, dummyPassword);
            Settings.DatabaseMasterPassword = dummyPassword;
            bool result = Database.TestConnection();
            Assert.IsTrue(result, "Couldnt update database master password");
        }
    }
}
