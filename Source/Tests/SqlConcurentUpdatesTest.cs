using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.DB;
using Favorite = Terminals.Data.DB.Favorite;

namespace Tests
{
    [TestClass]
    public class SqlConcurentUpdatesTest
    {
        private SqlTestsLab lab;

        private SqlPersistence secondaryPersistence;

        private IFavorite updatedFavorite;
        private bool eventCatched;

        private const string TEST_NAME = "second";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private IFavorites PrimaryFavorites
        {
            get
            {
                return this.lab.Persistence.Favorites;
            }
        }

        private IFavorites SecondaryFavorites
        {
            get
            {
                return this.secondaryPersistence.Favorites;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.lab = new SqlTestsLab();
            this.lab.InitializeTestLab();
            secondaryPersistence = new SqlPersistence();
            this.secondaryPersistence.Initialize();
        }

        [TestCleanup]
        public void TestClose()
        {
            // do not clean up the store here, because of waiting for background threads
            // this.lab.ClearTestLab();
        }

        [TestMethod]
        public void TestPeriodicalUpdates()
        {
            Favorite favoriteA = this.lab.CreateTestFavorite();
            this.PrimaryFavorites.Add(favoriteA);

            // assign event handler before another changes to catch all of them
            this.lab.Persistence.Dispatcher.FavoritesChanged +=
                new FavoritesChangedEventHandler(this.OnPrimaryStoreFavoritesChanged);

            var favoriteB = this.SecondaryFavorites.FirstOrDefault() as Favorite;
            favoriteB.Name = TEST_NAME;
            this.secondaryPersistence.Favorites.Update(favoriteB);

            ISynchronizeInvoke control = new Control();
            this.lab.Persistence.AssignSynchronizationObject(control);
            // refresh interval is set to 2 sec. by default
            Thread.Sleep(10000);

            Assert.IsTrue(this.eventCatched, "Data changed event wasnt received");
            Assert.AreEqual(this.updatedFavorite.Name, TEST_NAME, "The updated favorite wasnt refreshed");
        }

        private void OnPrimaryStoreFavoritesChanged(FavoritesChangedEventArgs args)
        {
            this.updatedFavorite = args.Updated.FirstOrDefault();
            this.eventCatched = true;
        }

        [TestMethod]
        public void TestSaveOnAlreadyUpdatedFavorite()
        {
            Favorite favoriteA = this.lab.CreateTestFavorite();
            this.PrimaryFavorites.Add(favoriteA);

            var favoriteB = this.SecondaryFavorites.FirstOrDefault() as Favorite;
            favoriteB.Name = TEST_NAME;
            this.SecondaryFavorites.Update(favoriteB);

            UpdateFirstFavorite(favoriteA);
            this.SecondaryFavorites.Delete(favoriteB);
            UpdateFirstFavorite(favoriteA);
        }

        private void UpdateFirstFavorite(Favorite favoriteA)
        {
            favoriteA.Name = "Second 2";
            this.PrimaryFavorites.Update(favoriteA);
        }
    }
}
