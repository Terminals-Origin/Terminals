using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.History;
using Favorite = Terminals.Data.DB.Favorite;

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

        [Description("If this test failes, the reason may be the delay to the database server.")]
        [TestMethod]
        public void HistoryTest()
        {
            Favorite favorite = this.AddFavoriteToPrimaryPersistence();

            // thats not a mystake, we want to check, if two fast tryes are ignored,
            // but it may fail, if the response from the server doesnt reflect our requirement
            // todo make this test 100% successful
            PrimaryHistory.RecordHistoryItem(favorite);
            PrimaryHistory.RecordHistoryItem(favorite);
            var expectedCount = this.GetExpectedHistoryCount();

            Assert.AreEqual(2, historyRecordedCount, "Recorded history wasnt reported");
            // to preserve duplicit times, when creating new entry in database, only one should be recorded
            Assert.AreEqual(1, expectedCount, "History wasnt stored into database");

            this.PrimaryFavorites.Delete(favorite);
            var expectedCountAfter = this.GetExpectedHistoryCount();
            Assert.AreEqual(0, expectedCountAfter, "Favorite history wasnt deleted from database");
        }

        private int GetExpectedHistoryCount()
        {
            return this.CheckDatabase
                .ExecuteStoreQuery<int>("select Count(FavoriteId) from History")
                .FirstOrDefault();
        }
    }
}
