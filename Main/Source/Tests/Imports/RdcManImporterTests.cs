using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Integration.Import;

namespace Tests.Imports
{
    [TestClass]
    public class RdcManImporterTests
    {
        private const string RELATIVE_PATH = @"..\Resources\TestData\";
        private const string EMPTY_FILE_NAME = "RdcManGroupsEmpty.rdg";
        private const string EMPTY_FILE = RELATIVE_PATH + EMPTY_FILE_NAME;
        private const string FULL_FILE_NAME = "RdcManGroupsAndFavorites.rdg";
        private const string FULL_FILE = RELATIVE_PATH + FULL_FILE_NAME;
        private const string SERVERSONLY_FILE_NAME = "RdcManRootServers.rdg";
        private const string SERVERSONLY_FILE = RELATIVE_PATH + SERVERSONLY_FILE_NAME;
        private const string INHERITED_FILE_NAME = "RdcManInheritedProperties.rdg";
        private const string INHERITED_FILE = RELATIVE_PATH + INHERITED_FILE_NAME;

        private const string SERVER_NAME2 = "ServerName2";

        private readonly ImportRdcMan importer = new ImportRdcMan();

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ImportInvalidFile_DoesntFail()
        {
            string file = Path.Combine(this.TestContext.DeploymentDirectory, "dummy.rdg");
            File.WriteAllText(file, string.Empty);
            this.importer.ImportFavorites(file);
        }

        [DeploymentItem(EMPTY_FILE)]
        [TestMethod]
        public void ImportEmptyFile_ReturnsNoFavorites()
        {
            string fileName = Path.Combine(this.TestContext.DeploymentDirectory, EMPTY_FILE_NAME);
            var importedItems = this.importer.ImportFavorites(fileName);
            Assert.AreEqual(0, importedItems.Count, "The empty file doesnt contain any favorites");
        }

        [DeploymentItem(FULL_FILE)]
        [TestMethod]
        public void ImportFullFile_ImportsFavorites()
        {
            string fileName = this.GetFullFileName();
            var importedItems = this.importer.ImportFavorites(fileName);
            Assert.AreEqual(0, importedItems.Count, "The full file should import 3 favorites");
        }

        [DeploymentItem(FULL_FILE)]
        [TestMethod]
        public void ReadFullDocument_ResolvesVersion()
        {
            RdcManDocument document = this.ReadFullDocument();
            Assert.IsTrue(document.IsVersion22, "Loaded document should contain version version 2.2");
        }

        [DeploymentItem(FULL_FILE)]
        [TestMethod]
        public void ReadFullDocument_ResolvesGroupA2()
        {
            RdcManDocument document = this.ReadFullDocument();
            RdcManGroup firstLevelGroup = document.Groups.First();
            RdcManGroup groupA2 = firstLevelGroup.Groups.ToList()[1];
            Assert.AreEqual("GroupA2", groupA2.Name, "First group in second level should contain GroupA2");
        }

        [DeploymentItem(FULL_FILE)]
        [TestMethod]
        public void ReadFullDocument_ResolvesServerName2()
        {
            RdcManDocument document = this.ReadFullDocument();
            RdcManGroup firstLevelGroup = document.Groups.First();
            RdcManGroup groupA2 = firstLevelGroup.Groups.First();
            RdcManServer server2 = groupA2.Servers.ToList()[1];
            const string MESSAGE = "Second server in second level group should contain ServerName2 display name";
            Assert.AreEqual(SERVER_NAME2, server2.DisplayName, MESSAGE);
        }

        [DeploymentItem(SERVERSONLY_FILE)]
        [TestMethod]
        public void ReadServersOnlyDocument_ResolvesServerName2()
        {
            string fileName = this.GetFullFileName(SERVERSONLY_FILE_NAME);
            var document = new RdcManDocument(fileName);
            RdcManServer server2 = document.Servers.ToList()[1];
            const string MESSAGE = "Second server in document root level should contain ServerName2 display name";
            Assert.AreEqual(SERVER_NAME2, server2.DisplayName, MESSAGE);
        }

        [DeploymentItem(INHERITED_FILE)]
        [TestMethod]
        public void ReadInheritedDocument_ResolvesInheritedConnectionSettings()
        {
            string fileName = this.GetFullFileName(INHERITED_FILE_NAME);
            var document = new RdcManDocument(fileName);
            var group = document.Groups.First();
            RdcManServer server2 = group.Servers.First();
            const string MESSAGE = "Server should contain not inherited port value";
            Assert.AreEqual(9999, server2.ConnectionSettings.Port, MESSAGE);
        }

        [DeploymentItem(FULL_FILE)]
        [TestMethod]
        public void ReadFullDocument_ResolvesNotInheritedConnectionSettings()
        {
            RdcManDocument document = this.ReadFullDocument();
            var group = document.Groups.First().Groups.First();
            RdcManServer server1 = group.Servers.First();
            const string MESSAGE = "Server should contain not inherited port value";
            Assert.AreEqual(4444, server1.ConnectionSettings.Port, MESSAGE);
        }

        private RdcManDocument ReadFullDocument()
        {
            string fileName = this.GetFullFileName();
            return new RdcManDocument(fileName);
        }

        private string GetFullFileName(string fileName = FULL_FILE_NAME)
        {
            return Path.Combine(this.TestContext.DeploymentDirectory, fileName);
        }
    }
}
