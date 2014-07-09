using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Integration.Export;

namespace Tests.Imports
{
    [TestClass]
    public class ExportRdpTest
    {
        private const string FILE_NAME = "ExportRdp"; 

        internal const int CUSTOM_PORT = 4444;

        internal const string EXPECTED_CONTENT = @"full address:s:ExportRdp
server port:i:4444
username:s:
domain:s:
session bpp:i:32
screen mode id:i:0
connect to console:i:0
disable wallpaper:i:0
redirectsmartcards:i:0
redirectcomports:i:0
redirectprinters:i:0
gatewayhostname:s:
gatewayusagemethod:i:0
audiomode:i:2
compression:i:0
allow font smoothing:i:0
redirectclipboard:i:1
keyboardhook:i:0
displayconnectionbar:i:0
disable menu anims:i:0
disable themes:i:0
disable full window drag:i:0
allow desktop composition:i:0
disable cursor setting:i:0
bitmapcachepersistenable:i:0
redirectposdevices:i:0
gatewaycredentialssource:i:0
LoadBalanceInfo:s:
";

        private readonly ExportRdp exporter = new ExportRdp();

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void InitializePersistence()
        {
            // because of Persistence singleton usage inside of the FavoriteConfigurationElement
            FilePersisted.FilePersistedTestLab.SetDefaultFileLocations();
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void TwoFavorites_Exports_TwoFiles()
        {
            ExportOptions options = this.CrateExportOptionsWithTwoFavorites();
            this.exporter.Export(options);
            bool firstExists = File.Exists(this.GetExportedFileName("MultipleExport_1"));
            bool secondExists = File.Exists(this.GetExportedFileName("MultipleExport_2"));
            Assert.IsTrue(firstExists && secondExists, "Export of two favorites has to create two corresponding rdp files");
        }

        private ExportOptions CrateExportOptionsWithTwoFavorites()
        {
            List<FavoriteConfigurationElement> favorites = CreateTwoFavorites();
            const string MULTIPLEEXPORT = "MultipleExport";
            string fileName = this.GetExportedFileName(MULTIPLEEXPORT);
            return new ExportOptions() {Favorites = favorites, FileName = fileName};
        }

        private static List<FavoriteConfigurationElement> CreateTwoFavorites()
        {
            var favoriteA = new FavoriteConfigurationElement() {Name = "1"};
            var favoriteB = new FavoriteConfigurationElement() {Name = "2"};
            return new List<FavoriteConfigurationElement>() {favoriteA, favoriteB};
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void Favorite_Exports_ValidText()
        {
            string exportedFileName = this.GetExportedFileName(FILE_NAME);
            ExportOptions exportOptions = CreateExportOptions(exportedFileName);
            exporter.Export(exportOptions);
            string exported = File.ReadAllText(exportedFileName);
            Assert.AreEqual(EXPECTED_CONTENT, exported, "Exported RDP file content doesnt contain all properties");
            Trace.WriteLine(this.TestContext.TestDeploymentDir);
        }

        private string GetExportedFileName(string fileName)
        {
            return Path.Combine(this.TestContext.TestDeploymentDir, fileName + ".rdp");
        }

        private static ExportOptions CreateExportOptions(string exportedFileName)
        {
            var favorite = new FavoriteConfigurationElement() {Name = FILE_NAME, ServerName = FILE_NAME, Port = CUSTOM_PORT};
            var favorites = new List<FavoriteConfigurationElement>() {favorite};
            return new ExportOptions() {Favorites = favorites, FileName = exportedFileName};
        }
    }
}
