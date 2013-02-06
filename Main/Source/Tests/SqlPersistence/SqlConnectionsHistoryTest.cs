using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.DB;
using Terminals.History;

namespace Tests
{
    /// <summary>
    ///This is a test class for database implementation of connection history
    ///</summary>
    [TestClass]
    public class SqlConnectionsHistoryTest : SqlTestsLab
    {
        private int historyRecordedCount;

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
            historyRecordedCount = 0;
            this.PrimaryHistory.OnHistoryRecorded += new HistoryRecorded(ConnectionHistory_OnHistoryRecorded);
        }

        private IConnectionHistory PrimaryHistory { get { return this.PrimaryPersistence.ConnectionHistory; } }

        private void ConnectionHistory_OnHistoryRecorded(HistoryRecordedEventArgs args)
        {
            historyRecordedCount++;
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
        }

        [Description("If this test fails, the reason may be the delay to the database server.")]
        [TestMethod]
        public void HistoryTest()
        {
            DbFavorite favorite = this.AddFavoriteToPrimaryPersistence();

            // that's not a mistake, we want to check, if two fast tries are ignored,
            // but it may fail, if the response from the server doesn't reflect our requirement
            // todo make this test 100% successful
            PrimaryHistory.RecordHistoryItem(favorite);
            PrimaryHistory.RecordHistoryItem(favorite);
            var expectedCount = this.GetExpectedHistoryCount();

            Assert.AreEqual(2, historyRecordedCount, "Recorded history wasn't reported");
            // to preserve duplicate times, when creating new entry in database, only one should be recorded
            Assert.AreEqual(1, expectedCount, "History wasn't stored into database");

            this.PrimaryFavorites.Delete(favorite);
            var expectedCountAfter = this.GetExpectedHistoryCount();
            Assert.AreEqual(0, expectedCountAfter, "Favorite history wasn't deleted from database");
        }

        private int GetExpectedHistoryCount()
        {
            return this.CheckDatabase
                .Database.SqlQuery<int>("select Count(FavoriteId) from History")
                .FirstOrDefault();
        }
    }
}
