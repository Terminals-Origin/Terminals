using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    /// <summary>
    /// Check comparisons of database favorites by version and id.
    /// Needed to check the updates from database by version stamp
    /// </summary>
    [TestClass]
    public class ComparersTest
    {
        /// <summary>
        /// Initial version after insert
        /// </summary>
        private readonly DbFavorite favoriteA = new DbFavorite { Id = 1 };

        /// <summary>
        /// After first update
        /// </summary>
        private readonly DbFavorite favoriteA2 = new DbFavorite { Id = 1, Version = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 } };

        /// <summary>
        /// After second update
        /// </summary>
        private readonly DbFavorite favoriteA3 = new DbFavorite { Id = 1, Version = new byte[] { 0, 0, 0, 0, 0, 0, 0, 2 } };

        private readonly DbFavorite favoriteB = new DbFavorite { Id = 2 };

        [TestCategory("NonSql")]
        [TestMethod]
        public void CompareByIdTest()
        {  
            var comparer = new ByIdComparer<DbFavorite>();
            bool identical = comparer.Equals(favoriteA, favoriteA);
            Assert.IsTrue(identical, "By Id compare failed");

            bool different = comparer.Equals(favoriteA, favoriteB);
            Assert.IsFalse(different);

            var listA = new List<DbFavorite> { favoriteA };
            var listB = new List<DbFavorite> { favoriteB };
            var missing = ListsHelper.GetMissingSourcesInTarget(listA, listB, comparer);
            Assert.AreEqual(1, missing[0].Id, "Missing favorite by Id wasn't found");
            var empty = ListsHelper.GetMissingSourcesInTarget(listA, new List<DbFavorite> { favoriteA }, comparer).ToList();
            Assert.AreEqual(0, empty.Count, "Favorite wasn't found in second collection");
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void CompareByVersionTest()
        {
            var comparer = new ChangedVersionComparer();
            bool identical = comparer.Equals(favoriteA, favoriteA);
            Assert.IsTrue(identical, "By Version compare failed");
            bool changed = comparer.Equals(favoriteA, favoriteA2);
            Assert.IsTrue(changed, "Initial version wasn't identified");
            changed = comparer.Equals(favoriteA, favoriteA2);
            Assert.IsTrue(changed, "First updated version wasn't identified");
            changed = comparer.Equals(favoriteA2, favoriteA3);
            Assert.IsTrue(changed, "Second updated version wasn't identified");
        }
    }
}
