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

namespace Tests.SqlPersisted
{
    [TestClass]
    public class ConcurentUpdatesTest : TestsLab
    {
        private IFavorite updatedFavorite;
        private bool addEventCatched;
        private bool updateEventCatched;
        private bool removedEventCatched;

        private const string TEST_NAME = "second";

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
        }

        [TestCleanup]
        public void TestClose()
        {
          // do not clean up the store here, because of waiting for background threads
          // this.lab.ClearTestLab();
        }

        [TestMethod]
        public void PeriodicalUpdatesTest()
        {
            this.AddFavoriteToPrimaryPersistence();
            this.AddFavoriteToPrimaryPersistence();
            
            // refresh interval is set to 30 sec. by default
            this.PrimaryPersistence.RefreshInterval = 2;
            // assign event handler before another changes to catch all of them
            this.PrimaryPersistence.Dispatcher.FavoritesChanged +=
                new FavoritesChangedEventHandler(this.OnPrimaryStoreFavoritesChanged);

            this.MakeChangesOnSecondaryPersistence();

            ISynchronizeInvoke control = new Control();
            this.PrimaryPersistence.AssignSynchronizationObject(control);
            
            Thread.Sleep(3000);

            Assert.IsTrue(this.addEventCatched, "Favorite added event wasn't received");
            Assert.IsTrue(this.updateEventCatched, "Favorite updated event wasn't received");
            Assert.IsTrue(this.removedEventCatched, "Favorite removed event wasn't received");
            Assert.AreEqual(TEST_NAME, this.updatedFavorite.Name, "The updated favorite wasn't refreshed");
        }

        private void MakeChangesOnSecondaryPersistence()
        {
            List<IFavorite> secondary = this.SecondaryFavorites.ToList();
            this.SecondaryFavorites.Delete(secondary[0]);
            var favoriteA = secondary[1] as DbFavorite;
            favoriteA.Name = TEST_NAME;
            this.SecondaryFavorites.Update(favoriteA);

            IFavorite favoriteB = this.SecondaryPersistence.Factory.CreateFavorite();
            favoriteB.Name = FAVORITE_NAME;
            favoriteB.ServerName = FAVORITE_SERVERNAME;
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
        public void SaveOnAlreadyUpdatedFavoriteTest()
        {
            DbFavorite favoriteA = this.AddFavoriteToPrimaryPersistence();
            var favoriteB = this.SecondaryFavorites.FirstOrDefault() as DbFavorite;
            this.PrimaryPersistence.Dispatcher.FavoritesChanged +=
                new FavoritesChangedEventHandler(this.OnUpdateAlreadyUpdatedFavoritesChanged);

            this.UpdateFavorite(favoriteA, this.PrimaryFavorites);
            this.UpdateFavorite(favoriteB, this.SecondaryFavorites);
            // last update wins
            this.UpdateFavorite(favoriteA, this.PrimaryFavorites);
            // now the stored name is "Test Changed Changed", because third update replaced previous change
            this.SecondaryFavorites.Delete(favoriteB);
            // next update shouldn't fail on deleted favorite
            this.UpdateFavorite(favoriteA, this.PrimaryFavorites);
            Assert.IsTrue(this.removedEventCatched, "Persistence didn't catched the already removed favorite update event.");
        }

        private void OnUpdateAlreadyUpdatedFavoritesChanged(FavoritesChangedEventArgs args)
        {
            if (args.Removed.Count > 0)
                this.removedEventCatched = true;
        }

        private void UpdateFavorite(DbFavorite toUpdate, IFavorites targetPersistence)
        {
            toUpdate.Name += " Changed";
            targetPersistence.Update(toUpdate);
        }

        [TestMethod]
        public void DeleteFavoriteWhenDisconnectedTest()
        {
            DbFavorite favoriteA = this.AddFavoriteToPrimaryPersistence();
            // simulate disconnection using invalid connection string for next call
            Settings.ConnectionString = @"Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\fake.mdf;Integrated Security=True;User Instance=True";
            this.PrimaryPersistence.Dispatcher.ErrorOccurred += new EventHandler<DataErrorEventArgs>(this.DispatcherErrorOccurred);
            // first call throws a connection exception, which is reported and results in reset connection string
            this.PrimaryFavorites.Delete(favoriteA);
            // second call throws a concurrency exception, which should be handled
            this.PrimaryFavorites.Delete(favoriteA);

            Assert.IsTrue(removedEventCatched, "Disconnected state wasn't reported");
        }

        private void DispatcherErrorOccurred(object sender, DataErrorEventArgs e)
        {
            SetDeploymentDirConnectionString();
            removedEventCatched = true;
            Assert.IsFalse(e.CallStackFull, "Dispatcher error reported full call stack");
        }
    }
}
