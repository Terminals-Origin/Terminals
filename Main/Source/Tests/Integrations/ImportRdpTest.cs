using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Integration.Import;
using Tests.Imports;

namespace Tests.Integrations
{
    [TestClass]
    public class ImportRdpTest
    {
        private const string FILE_NAME = "toImport.rdp";

        private static string fileName;

        private static List<FavoriteConfigurationElement> imported;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetupImporter(TestContext context)
        {
            // we want to import only once for all tests
            imported = ImportFileContent(context, ExportRdpTest.EXPECTED_CONTENT);
        }

        private static List<FavoriteConfigurationElement> ImportFileContent(TestContext context, string content)
        {
            fileName = Path.Combine(context.TestDeploymentDir, FILE_NAME);
            File.WriteAllText(fileName, content);
            var importer = new ImportRDP();
            return ((IImport)importer).ImportFavorites(fileName);
        }

        [TestMethod]
        public void RdpFile_Imports_OnlyOneFavorite()
        {
            Assert.AreEqual(1, imported.Count, "Rdp file always contains only one favorite");
        }

        [TestMethod]
        public void RdpFile_Imports_CorrectPort()
        {
            int importedPort = imported[0].Port;
            Assert.AreEqual(ExportRdpTest.CUSTOM_PORT, importedPort, "Content of the imported file wasnt parsed properly");
        }

        [TestMethod]
        public void ServerNameWithPort_Imports_CorrectPort()
        {
            string modifiedContent = CreatePortInServerName();
            var fromModified = ImportFileContent(this.TestContext, modifiedContent);
            int importedPort = fromModified[0].Port;
            Assert.AreEqual(3333, importedPort, "Sever name field can also contain port");
        }

        private static string CreatePortInServerName()
        {
            string modifiedContent = ExportRdpTest.EXPECTED_CONTENT
                .Replace("full address:s:ExportRdp", "full address:s:ExportRdp:3333");
            // otherwise the port is overriden
            return modifiedContent.Replace("server port:i:4444\r\n", string.Empty);
        }
    }
}
