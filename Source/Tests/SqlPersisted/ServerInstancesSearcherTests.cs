using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    [TestClass]
    public class ServerInstancesSearcherTests
    {
        private const string TEST_SERVER_NAME = "ServerA";

        [TestCategory("NonSql")]
        [TestMethod]
        public void SearchAcceptsNull()
        {
            var searcher = new ServerInstancesSearcher(() => null);
            Task<List<string>> foundTask = searcher.FindSqlServerInstancesAsync();
            var assertTask = foundTask.ContinueWith(t => Assert.IsNotNull(t.Result, "Search engine should accept null table"));
            assertTask.Wait();
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void SearchNeverReturnsNull()
        {
            var searcher = new ServerInstancesSearcher(CreateEmptyTable);
            Task<List<string>> foundTask = searcher.FindSqlServerInstancesAsync();
            var assertTask = foundTask.ContinueWith(t => Assert.IsNotNull(t.Result, "Search engine should never return null"));
            assertTask.Wait();
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void SearchFormatsFullNameProperly()
        {
            var searcher = new ServerInstancesSearcher(() => CreateWithItem("InstanceA"));
            Task<List<string>> foundTask = searcher.FindSqlServerInstancesAsync();
            Task assertTask = foundTask.ContinueWith(AssertFullNameFormatsProperly);
            assertTask.Wait();
        }

        private static void AssertFullNameFormatsProperly(Task<List<string>> foundTask)
        {
            string found = foundTask.Result[0];
            Assert.IsNotNull("ServerA\\InstanceA", found, "Search engine dint parse full name properly");
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void SearchFormatsServerNameProperly()
        {
            var searcher = new ServerInstancesSearcher(() => CreateWithItem(string.Empty));
            Task<List<string>> foundTask = searcher.FindSqlServerInstancesAsync();
            Task assertTask = foundTask.ContinueWith(AssertServerNameFormatsProperly);
            assertTask.Wait();
        }

        private static void AssertServerNameFormatsProperly(Task<List<string>> foundTask)
        {
            string found = foundTask.Result[0];
            Assert.IsNotNull(TEST_SERVER_NAME, found, "Search engine dint parse server name properly");
        }

        private static DataTable CreateWithItem(string instanceName)
        {
            var table = CreateEmptyTable();
            var itemRow = table.NewRow();
            itemRow[ServerInstancesSearcher.SERVER_NAME_COLUMN] = TEST_SERVER_NAME;
            itemRow[ServerInstancesSearcher.SERVER_NAME_COLUMN] = instanceName;
            table.Rows.Add(itemRow);
            return table;
        }

        private static DataTable CreateEmptyTable()
        {
            var instancesTable = new DataTable();
            instancesTable.Columns.Add(ServerInstancesSearcher.SERVER_NAME_COLUMN);
            instancesTable.Columns.Add(ServerInstancesSearcher.INSTANCE_NAME_COLUMN);

            return instancesTable;
        }
    }
}
