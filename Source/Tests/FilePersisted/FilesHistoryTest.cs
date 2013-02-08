using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Data;
using Terminals.Data.History;

namespace Tests.FilePersisted
{
    [TestClass]
    public class FilesHistoryTest
    {
        [TestMethod]
        public void HistoryDateTimeIsInUtcTest()
        {
            InjectionDateTime.SetTestDateTime();
            IConnectionHistory history = RecordHistoryItemToFilePersistence();
            DateTime recordedDate = GetRecordedDate(history);
            Assert.AreEqual(recordedDate, Moment.Now, "Correct date wasn't delivered to the history file");
        }

        private static IConnectionHistory RecordHistoryItemToFilePersistence()
        {
            ImportsTest.SetDefaultFileLocations();
            var persistence = new FilePersistence();
            IFavorite favorite = persistence.Factory.CreateFavorite();
            persistence.Favorites.Add(favorite);
            var history = persistence.ConnectionHistory;
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
