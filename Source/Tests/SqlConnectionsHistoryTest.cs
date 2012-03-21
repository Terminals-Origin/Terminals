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
    public class SqlConnectionsHistoryTest
    {
        private SqlTestsLab lab;
        private int historyRecordedCount;

        [TestInitialize]
        public void TestInitialize()
        {
            this.lab = new SqlTestsLab();
            this.lab.InitializeTestLab();
            historyRecordedCount = 0;
            this.lab.Persistence.ConnectionHistory.OnHistoryRecorded += new HistoryRecorded(ConnectionHistory_OnHistoryRecorded);
        }

        private void ConnectionHistory_OnHistoryRecorded(HistoryRecordedEventArgs args)
        {
            historyRecordedCount++;
        }

        [TestCleanup]
        public void TestClose()
        {
            this.lab.ClearTestLab();
        }

        [TestMethod]
        public void HistoryTest()
        {
            Favorite favorite = this.lab.CreateTestFavorite();
            this.lab.Persistence.Favorites.Add(favorite);

            IConnectionHistory history = this.lab.Persistence.ConnectionHistory;
            history.RecordHistoryItem(favorite);
            history.RecordHistoryItem(favorite);
            var expectedCountBefore = this.GetExpectedHistoryCount();

            // to preserve duplicit times, when creating new entry in database
            Assert.AreEqual(historyRecordedCount, 2, "Recorded history wasnt reported");
            Assert.AreEqual(expectedCountBefore, 1, "History wasnt stored into database");

            this.lab.Persistence.Favorites.Delete(favorite);
            var expectedCountAfter = this.GetExpectedHistoryCount();
            Assert.AreEqual(expectedCountAfter, 0, "Favorite history wasnt deleted from database");
        }

        private int GetExpectedHistoryCount()
        {
            return this.lab.CheckDatabase
                .ExecuteStoreQuery<int>("select Count(FavoriteId) from History")
                .FirstOrDefault();
        }
    }
}
