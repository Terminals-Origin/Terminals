using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;
using Terminals.Data;

namespace Tests.FilePersisted
{
    [TestClass]
    public class FavoritesTest : FilePersistedTestLab
    {
        private Guid addedId;
        private Guid updatedId;
        private Guid deletedId;

        /// <summary>
        /// Notes property can store special characters, so it is Base64 encoded.
        /// </summary>
        [TestMethod]
        public void SaveLoadNotesTest()
        {
            IFavorite favorite = this.AddFavorite();
            const string SPECIAL_CHARACTERS = "čočka\r\nčočka"; // some example special characters
            favorite.Notes = SPECIAL_CHARACTERS;
            this.Persistence.Favorites.Update(favorite);
            var secondary = new FilePersistence();
            secondary.Initialize();
            IFavorite checkfavorite = secondary.Favorites.FirstOrDefault();
            Assert.AreEqual(SPECIAL_CHARACTERS, checkfavorite.Notes, "favorite notes were not saved properly");
        }

        /// <summary>
        /// Checks, if we are still able to manipulate passwords after protocol update.
        /// This is a special case for RdpOptions, which need persistence to handle Gateway credentials
        /// </summary>
        [TestMethod]
        public void UpdateProtocolTest()
        {
            IFavorite favorite = this.AddFavorite();
            // now it has RdpOptions
            favorite.Protocol = ConnectionManager.VNC;
            this.Persistence.Favorites.Update(favorite);
            AssertRdpSecurity(favorite);
        }

        /// <summary>
        /// Checks, if Gateway has still assigned persistence security, to be able work with passwords.
        /// </summary>
        internal static void AssertRdpSecurity(IFavorite favorite)
        {
            favorite.Protocol = ConnectionManager.RDP;
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            // next line shouldn't fail
            rdpOptions.TsGateway.Security.Password = "aaa";
        }

        [TestMethod]
        public void ConcurentFileUpdateTest()
        {
            IFavorite toRemove = this.AddFavorite();
            IFavorite toUpdate = this.AddFavorite();
            
            var testFileWatch = new TestFileWatch();
            var secondaryPersistence = this.InitializeSecondaryPersistence(testFileWatch);

            //do the next steps ot be able serialize otherwise paraller work of persistences refresh and dont relay on OS file changes
            //1. kick off the file changed event in background thread to let the secondary persistence reload in parallel
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) => testFileWatch.FireFileChanged()));

            //2. do the changes in primary persistence, this is not noticed in secondary persistence yet
            IFavorite added = this.UpdatePrimaryPersistence(toUpdate, toRemove);

            testFileWatch.ObservationWatch.Set(); // signal secondary persistence, that changes are ready to reload

            //3. wait till secondary persistence is done with reload 
            testFileWatch.ReleaseWatch.WaitOne(600000);

            //4. assert the results in secondary persistence
            int secondaryFavoritesCount = secondaryPersistence.Favorites.Count();
            Assert.AreEqual(2, secondaryFavoritesCount); // one updated and one added
            Assert.AreEqual(this.addedId, added.Id, "Didnt receive favorite add event");
            Assert.AreEqual(this.updatedId, toUpdate.Id, "Didnt receive favorite update event");
            Assert.AreEqual(this.deletedId, toRemove.Id, "Didnt receive favorite add event");
        }

        private FilePersistence InitializeSecondaryPersistence(TestFileWatch testFileWatch)
        {
            var secondaryPersistence = new FilePersistence(new PersistenceSecurity(), testFileWatch);
            // let the persistence load initial state
            secondaryPersistence.Initialize();
            secondaryPersistence.Dispatcher.FavoritesChanged += this.DispatcherOnFavoritesChanged;
            return secondaryPersistence;
        }

        private IFavorite UpdatePrimaryPersistence(IFavorite toUpdate, IFavorite toRemove)
        {
            toUpdate.Name = "some other value";
            IFavorites primaryFavorites = this.Persistence.Favorites;
            primaryFavorites.Update(toUpdate);
            primaryFavorites.Delete(toRemove);
            IFavorite added = this.AddFavorite();
            return added;
        }

        private void DispatcherOnFavoritesChanged(FavoritesChangedEventArgs args)
        {
            if (args.Added.Count == 1)
                this.addedId = args.Added[0].Id;
            if (args.Removed.Count == 1)
                this.deletedId = args.Removed[0].Id;
            if (args.Updated.Count == 1)
                this.updatedId = args.Updated[0].Id;
        }
    }
}
