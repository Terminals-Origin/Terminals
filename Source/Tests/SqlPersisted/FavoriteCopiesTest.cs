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
        private const string FAVORITE_SERVERNAME2 = "Second server";

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
        public void CopyFavorite()
        {
            IFavorite favorite = this.CreatePrimaryFavorite();
            IFavorite copy = favorite.Copy();
            this.PrimaryFavorites.Add(copy);
            this.AssertCopy(copy);
        }

        [TestMethod]
        public void UpdateFromFavorite()
        {
            IFavorite favoriteA = this.CreatePrimaryFavorite();
            IFavorite favoriteB = this.CreateTestFavorite();
            this.PrimaryFavorites.Add(favoriteB);
            favoriteB.UpdateFrom(favoriteA);
            this.PrimaryFavorites.Update(favoriteB);
            this.AssertCopy(favoriteB);
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

        private void AssertCopy(IFavorite copy)
        {
            int stored = this.CheckDatabase.Favorites.Count();
            Assert.AreEqual(2, stored, "Favorites count doesn't match after copy added to the persistence");
            IFavorite secondary = this.SecondaryFavorites.ToList()[1];
            Assert.AreEqual(secondary.ServerName, FAVORITE_SERVERNAME2, "Values not updated properly");

            // next method loads details of not loaded favorite from database, it shouldn't fail, even if it is a copy
            string toolTip = copy.GetToolTipText();
            Assert.IsFalse(string.IsNullOrEmpty(toolTip), "ToolTip wasn't resolved for copy of favorite");
        }
    }
}
