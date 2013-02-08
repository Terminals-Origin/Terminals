using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Data;

namespace Tests.FilePersisted
{
    [TestClass]
    public class FilesHistoryTest
    {
        [TestMethod]
        public void HistoryDateTimeIsInUtcTest()
        {
            var latesttime = DateTime.Now;
            IConnectionHistory history = RecordHistoryItemToFilePersistence();
            DateTime recordedDate = GetRecordedDate(history);
            bool savedIdentical = Math.Abs((latesttime - recordedDate).TotalHours) < 1;
            Assert.IsTrue(savedIdentical, "Correct date wasn't delivered to the history file");
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
            return recordedItems.SelectMany(group => group.Value).First().Date;
        }
    }
}
