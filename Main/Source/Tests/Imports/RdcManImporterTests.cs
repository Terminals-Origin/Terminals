using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Integration.Import;

namespace Tests.Imports
{
    [TestClass]
    public class RdcManImporterTests
    {
        private const string EMPTY_FILE = "RdcManGroupsEmpty.rdg";
        private const string FULL_FILE = "RdcManGroupsAndFavorites.rdg";
        
        private readonly ImportRdcMan importer = new ImportRdcMan();

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ImportInvalidFile_DoesntFail()
        {
            string file = Path.Combine(this.TestContext.DeploymentDirectory, "dummy.rdg");
            File.WriteAllText(file, string.Empty);
            this.importer.ImportFavorites(file);
        }

        [DeploymentItem(@"..\Resources\TestData\" + EMPTY_FILE)]
        [TestMethod]
        public void ImportEmptyFile_ReturnsNoFavorites()
        {
            string file = Path.Combine(this.TestContext.DeploymentDirectory, EMPTY_FILE);
            var importedItems = this.importer.ImportFavorites(file);
            Assert.AreEqual(0, importedItems.Count, "The empty file doesnt contain any favorites");
        }

        [DeploymentItem(@"..\Resources\TestData\" + FULL_FILE)]
        [TestMethod]
        public void ImportFullFile_ImportsFavorites()
        {
            string file = Path.Combine(this.TestContext.DeploymentDirectory, FULL_FILE);
            var importedItems = this.importer.ImportFavorites(file);
            Assert.AreEqual(0, importedItems.Count, "The full file should import 3 favorites");
        }
    }
}
