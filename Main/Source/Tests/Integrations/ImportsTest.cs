using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Forms.Controls;
using Terminals.Integration;
using Terminals.Integration.Export;
using Terminals.Integration.Import;
using Tests.Connections;
using Tests.FilePersisted;

namespace Tests.Integrations
{
    [TestClass]
    [DeploymentItem(@"..\Resources\TestData\" + DUPLICIT_ITEMS_FILE)]
    public class ImportsTest : FilePersistedTestLab
    {
        private const string DUPLICIT_ITEMS_FILE = @"Issue_21279_import_800_items.xml";

        private const string TEST_FILE = "testOne.xml";

        private const string TEST_PASSWORD = "aaa";

        private const string TEST_DOMAIN = "testDomain";

        private const string TEST_USERNAME = "testUser";

        public TestContext TestContext { get; set; }

        // following functions simulate the answer usually provided by user in UI
        private static readonly Func<int, DialogResult> rename = itemsCount => DialogResult.Yes;
        private static readonly Func<int, DialogResult> overwrite = itemsCount => DialogResult.No;

        private int PersistenceFavoritesCount
        {
            get
            {
                return this.Persistence.Favorites.Select(favorite => favorite.Name)
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .Count();
            }
        }

        private int ImportedGroupsCount
        {
            get
            {
                return this.Persistence.Groups.Select(favorite => favorite.Name)
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .Count();
            }
        }

        [TestMethod]
        public void ExportImportFavorite_ImportsFavoriteSecurity()
        {
            ExportImportFavorite(this.Persistence, this.TestContext.DeploymentDirectory);
        }

        [TestMethod]
        public void ExportImportFavorite_ImportsTsgwOptionsSecurity()
        {
            string path = this.TestContext.DeploymentDirectory;
            IFavorite importedFavorite = PerformImportExportFavorite(this.Persistence, path);
            SecurityOptions security = ((IContainsCredentials)importedFavorite.ProtocolProperties).GetSecurity();
            AssertSecurityImported(this.Persistence, security);
        }

        /// <summary>
        /// More regression test than unit test
        /// </summary>
        internal static void ExportImportFavorite(IPersistence persistence, string path)
        {
            IFavorite importedFavorite = PerformImportExportFavorite(persistence, path);
            AssertSecurityImported(persistence, importedFavorite.Security);
        }

        private static IFavorite PerformImportExportFavorite(IPersistence persistence, string path)
        {
            IFavorite favorite = CreateTestFavorite(persistence);
            ExportFavorite(favorite, persistence);
            // to preserve test against identical favorite
            persistence.Favorites.Delete(favorite);
            List<FavoriteConfigurationElement> toImport = ImportItemsFromFile(persistence, path, TEST_FILE);
            // persisted favorites are empty, strategy doesnt matter
            InvokeTheImport(toImport, persistence, rename);
            return persistence.Favorites.First();
        }

        private static void AssertSecurityImported(IPersistence persistence, ISecurityOptions security)
        {
            var guarded = new GuardedSecurity(persistence, security);
            Assert.AreEqual(TEST_USERNAME, guarded.UserName);
            Assert.AreEqual(TEST_DOMAIN, guarded.Domain);
            Assert.AreEqual(TEST_PASSWORD, guarded.Password);
        }

        private static IFavorite CreateTestFavorite(IPersistence persistence)
        {
            IFavorite favorite = persistence.Factory.CreateFavorite();
            favorite.Name = "testFavorite";
            favorite.ServerName = favorite.Name;
            SetupSecurityValues(persistence, favorite.Security);
            TsGwOptions tsgwOptions = ((RdpOptions)favorite.ProtocolProperties).TsGateway;
            tsgwOptions.UsageMethod = 1;// enable
            SetupSecurityValues(persistence, tsgwOptions.Security);
            persistence.Favorites.Add(favorite);
            return favorite;
        }

        private static void SetupSecurityValues(IPersistence persistence, ISecurityOptions security)
        {
            var guarded = new GuardedSecurity(persistence, security);
            guarded.UserName = TEST_USERNAME;
            guarded.Domain = TEST_DOMAIN;
            guarded.Password = TEST_PASSWORD;
        }

        private static void ExportFavorite(IFavorite favorite, IPersistence persistence)
        {
            FavoriteConfigurationElement favoriteElement = ModelConverterV2ToV1.ConvertToFavorite(favorite, persistence, TestConnectionManager.Instance);

            ExportOptions options = new ExportOptions
                {
                    ProviderFilter = ImportTerminals.TERMINALS_FILEEXTENSION,
                    Favorites = new List<FavoriteConfigurationElement> {favoriteElement},
                    FileName = TEST_FILE,
                    IncludePasswords = true
                };

            Exporters exporters = new Exporters(persistence, TestConnectionManager.Instance);
            exporters.Export(options);
        }

        [TestMethod]
        public void ImportingFromXmlFile_ImportsGroups()
        {
            // strategy doesnt matter
            this.ImportDuplicitFavoritesTest(rename, 2);
            const int EXPECTED_GROUPS = 23;
            Assert.AreEqual(EXPECTED_GROUPS, this.ImportedGroupsCount, "Groups were not imported from Tags config element.");
        }

        /// <summary>
        /// Tries to import duplicate items into the file persistence renaming duplicate items
        ///</summary>
        [TestMethod]
        public void ImportRenamingDuplicitFavoritesTest()
        {
            this.ImportDuplicitFavoritesTest(rename, 2);
        }

        /// <summary>
        /// Tries to import duplicate items into the file persistence overwriting duplicate items
        ///</summary>
        [TestMethod]
        public void ImportOverwritingDuplicitFavoritesTest()
        {
            this.ImportDuplicitFavoritesTest(overwrite, 1);
        }

        private void ImportDuplicitFavoritesTest(Func<int, DialogResult> strategy, int expectedSecondImportCount)
        {
            // call import first to force the persistence initialization
            List<FavoriteConfigurationElement> toImport = ImportItemsFromFile(this.Persistence, this.TestContext.DeploymentDirectory);

            // 887 obtained by manual check of the xml elements
            Assert.AreEqual(887, toImport.Count, "Some items from Import file were not identified");
            bool result = InvokeTheImport(toImport, this.Persistence, strategy);
            Assert.AreEqual(true, result, "Import wasn't successful");
            int expected = ExpectedFavoritesCount(toImport);
            Assert.AreEqual(expected, this.PersistenceFavoritesCount, "Imported favorites count doesn't match.");
            InvokeTheImport(toImport, this.Persistence, strategy);
            Assert.AreEqual(expected * expectedSecondImportCount, this.PersistenceFavoritesCount,
                "Imported favorites count doesn't match after second import");
        }

        [TestMethod]
        public void DisabledProtocol_ImportFavorite_IsntImported()
        {
            var toImport = new List<FavoriteConfigurationElement>()
            {
                new FavoriteConfigurationElement()
                {
                    Protocol = "Unknown",
                    Name = "Irrelevant",
                    ServerName = "Irrelevant"
                }
            };

            InvokeTheImport(toImport, this.Persistence, rename);
            Assert.AreEqual(0, this.PersistenceFavoritesCount, "Imports of unknown protocols are protected in validator.");
        }

        private static bool InvokeTheImport(List<FavoriteConfigurationElement> toImport, IPersistence persistence,
            Func<int, DialogResult> strategy)
        {
            var moqInterface = new TestImportUi(strategy);
            var managedImport = new ImportWithDialogs(moqInterface, persistence, TestConnectionManager.Instance);
            return managedImport.Import(toImport);
        }

        private static int ExpectedFavoritesCount(List<FavoriteConfigurationElement> toImport)
        {
            return toImport.Select(favorite => favorite.Name)
                           .Distinct(StringComparer.CurrentCultureIgnoreCase)
                           .Count();
        }

        private static List<FavoriteConfigurationElement> ImportItemsFromFile(IPersistence persistence, string path, 
            string fileName = DUPLICIT_ITEMS_FILE)
        {
            string fullFileName = Path.Combine(path, fileName);
            var importers = new Importers(persistence);
            return importers.ImportFavorites(fullFileName);
        }
    }
}
