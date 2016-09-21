using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;

namespace Tests.SqlPersisted
{
    /// <summary>
    /// Ensure, that all deep copies of favorite work properly
    /// </summary>
    [TestClass]
    public class FavoriteCopiesTest : TestsLab
    {
        private const string FAVORITE_SERVERNAME2 = "Second_server";

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
        }

        [TestMethod]
        public void CopyFavorite_AddsToPersistence()
        {
            IFavorite source = this.CopyInPersistence();
            this.AssertCopyInSecondaryPersistence(source);
        }

        [TestMethod]
        public void CopyFavorite_CopiesProperties()
        {
            IFavorite source = this.CopyInPersistence();
            this.AssertByToolTip(source);
        }

        private IFavorite CopyInPersistence()
        {
            IFavorite source = this.CreatePrimaryFavorite();
            IFavorite copy = source.Copy();
            this.PrimaryFavorites.Add(copy);
            return source;
        }

        [TestMethod]
        public void UpdateFromFavorite_UpdatesInPersistence()
        {
            var source = this.UpdateInPersistence();
            this.AssertCopyInSecondaryPersistence(source);
        }

        [TestMethod]
        public void UpdateFromFavorite_CopiesProperties()
        {
            var source = this.UpdateInPersistence();
            this.AssertCopyInSecondaryPersistence(source);
        }

        private IFavorite UpdateInPersistence()
        {
            IFavorite source = this.CreatePrimaryFavorite();
            IFavorite copy = this.CreateTestFavorite();
            this.PrimaryFavorites.Add(copy);
            copy.UpdateFrom(source);
            this.PrimaryFavorites.Update(copy);
            return source;
        }

        /// <summary>
        /// Create favorite with custom name to be able compare copies. 
        /// Returns newly created favorite already inserted into primary store.
        /// </summary>
        private IFavorite CreatePrimaryFavorite()
        {
            IFavorite favorite = this.CreateTestFavorite();
            favorite.ServerName = FAVORITE_SERVERNAME2;
            this.PrimaryFavorites.Add(favorite);
            return favorite;
        }

        private void AssertCopyInSecondaryPersistence(IFavorite source)
        {
            int stored = this.CheckDatabase.Favorites.Count();
            Assert.AreEqual(2, stored, "Favorites count doesn't match after copy added to the persistence");
            IFavorite secondary = this.SecondaryFavorites.ToList()[1];
            Assert.AreEqual(secondary.ServerName, FAVORITE_SERVERNAME2, "Values not updated properly");
        }

        private void AssertByToolTip(IFavorite source)
        {
            IFavorite copy = this.SecondaryFavorites.ToList()[1];
            // next method loads details of not loaded favorite from database, it shouldn't fail, even if it is a copy.
            string toolTip = copy.GetToolTipText();
            string origTooltip = source.GetToolTipText();
            Assert.AreEqual(origTooltip, toolTip, "Properties to generate ToolTip weren't copied.");
        }
    }
}
