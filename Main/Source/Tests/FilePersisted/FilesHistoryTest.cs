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
    }
}
