using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.DB;
using Terminals.Data.History;
using Terminals.History;
using Tests.FilePersisted;

namespace Tests.SqlPersisted
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
            this.historyRecordedCount = 0;
            this.PrimaryHistory.OnHistoryRecorded += new HistoryRecorded(this.ConnectionHistory_OnHistoryRecorded);
        }

        private IConnectionHistory PrimaryHistory { get { return this.PrimaryPersistence.ConnectionHistory; } }

        private void ConnectionHistory_OnHistoryRecorded(HistoryRecordedEventArgs args)
        {
            this.historyRecordedCount++;
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
        }

        [Description("If this test fails, the reason may be the delay to the database server.")]
        [TestMethod]
        public void HistoryCommitTest()
        {
            DbFavorite favorite = this.AddFavoriteToPrimaryPersistence();

            // that's not a mistake, we want to check, if two fast tries are ignored
            this.PrimaryHistory.RecordHistoryItem(favorite);
            this.PrimaryHistory.RecordHistoryItem(favorite);
            int expectedCount = this.GetExpectedHistoryCount();

            Assert.AreEqual(2, this.historyRecordedCount, "Recorded history wasn't reported");
            // to preserve duplicate times, when creating new entry in database, only one should be recorded
            Assert.AreEqual(1, expectedCount, "History wasn't stored into database");

            this.PrimaryFavorites.Delete(favorite);
            int expectedCountAfter = this.GetExpectedHistoryCount();
            Assert.AreEqual(0, expectedCountAfter, "Favorite history wasn't deleted from database");
        }

        private int GetExpectedHistoryCount()
        {
            return this.CheckDatabase
                .Database.SqlQuery<int>("select Count(FavoriteId) from History")
                .FirstOrDefault();
        }

        [TestMethod]
        public void HistoryEqualsOnDifferentTimeZones()
        {
            InjectionDateTime.SetTestDateTime();
            DbFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            this.PrimaryHistory.RecordHistoryItem(favorite);
            // may fail about midnight, ignore this case
            IFavorite resultFavorite = this.SecondaryPersistence.ConnectionHistory
                .GetDateItems(HistoryIntervals.TODAY)
                .First();

            Assert.IsNotNull(resultFavorite, "History favorite wasn't resolved");
            var recordedHistory = this.CheckDatabase.Database.SqlQuery<DateTime>("select Date from History")
                .First();
            Assert.AreEqual(recordedHistory, Moment.Now, "Correct date wasn't delivered to the store");
        }
    }
}
