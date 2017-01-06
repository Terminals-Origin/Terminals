using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Data;
using Terminals.Data.History;

namespace Tests.FilePersisted
{
    [TestClass]
    public class FilesHistoryTest : FilePersistedTestLab
    {
        private int historyClearReported;

        [TestMethod]
        public void HistoryDateTimeIsInUtcTest()
        {
            InjectionDateTime.SetTestDateTime();
            IConnectionHistory history = RecordHistoryItemToFilePersistence();
            DateTime recordedDate = GetRecordedDate(history);
            Assert.AreEqual(recordedDate, Moment.Now, "Correct date wasn't delivered to the history file");
        }

        private IConnectionHistory RecordHistoryItemToFilePersistence()
        {
            IFavorite favorite = this.AddFavorite();
            IConnectionHistory history = this.Persistence.ConnectionHistory;
            history.RecordHistoryItem(favorite);
            return history;
        }

        private static DateTime GetRecordedDate(IConnectionHistory history)
        {
            var historyAccesor = new PrivateObject(history);
            var recordedItems = historyAccesor.Invoke("GetGroupedByDate") as SerializableDictionary<string, SortableList<IHistoryItem>>;
            var foundItem = recordedItems.SelectMany(group => group.Value).First();
            return foundItem.Date.ToUniversalTime();
        }

        [TestMethod]
        public void ClearHistoryTest()
        {
            this.Persistence.ConnectionHistory.HistoryClear += new Action(PrimaryHistory_HistoryClear);
            InjectionDateTime.SetTestDateTime();
            IConnectionHistory history = RecordHistoryItemToFilePersistence();
            history.Clear();
            int historyItems = history.GetDateItems(HistoryIntervals.TODAY).Count;
            Assert.AreEqual(0, historyItems, "File history wasnt clear properly");
            Assert.AreEqual(1, historyClearReported, "History clear wasnt reported properly");
        }

        private void PrimaryHistory_HistoryClear()
        {
            this.historyClearReported++;
        }
    }
}
