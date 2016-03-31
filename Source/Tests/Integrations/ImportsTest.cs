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

        [TestCategory("NonSql")]
        [TestMethod]
        public void ExportImportFavoriteTest()
        {
            IPersistence persistence = this.Persistence;
            ExportImportFavorite(persistence, this.TestContext.DeploymentDirectory);
        }

        /// <summary>
        /// More regression test than unit test
        /// </summary>
        internal static void ExportImportFavorite(IPersistence persistence, string path)
        {
            IFavorite favorite = CreateTestFavorite(persistence);
            ExportFavorite(favorite, persistence);
            // to preserve test against identical favorite
            persistence.Favorites.Delete(favorite);
            List<FavoriteConfigurationElement> toImport = ImportItemsFromFile(persistence, path, TEST_FILE);
            // persisted favorites are empty, strategy doesnt matter
            InvokeTheImport(toImport, persistence, rename);
            var importedSecurity = persistence.Favorites.First().Security;
            var guarded = new GuardedSecurity(persistence.Security, importedSecurity);
            Assert.AreEqual(TEST_USERNAME, guarded.UserName);
            Assert.AreEqual(TEST_DOMAIN, guarded.Domain);
            Assert.AreEqual(TEST_PASSWORD, guarded.Password);
        }

        private static IFavorite CreateTestFavorite(IPersistence persistence)
        {
            IFavorite favorite = persistence.Factory.CreateFavorite();
            favorite.Name = "testFavorite";
            favorite.ServerName = favorite.Name;
            var security = favorite.Security;
            var guarded = new GuardedSecurity(persistence.Security, security);
            guarded.UserName = TEST_USERNAME;
            guarded.Domain = TEST_DOMAIN;
            guarded.Password = TEST_PASSWORD;
            persistence.Favorites.Add(favorite);
            return favorite;
        }

        private static void ExportFavorite(IFavorite favorite, IPersistence persistence)
        {
            FavoriteConfigurationElement favoriteElement = ModelConverterV2ToV1.ConvertToFavorite(favorite, persistence);

            ExportOptions options = new ExportOptions
                {
                    ProviderFilter = ImportTerminals.TERMINALS_FILEEXTENSION,
                    Favorites = new List<FavoriteConfigurationElement> {favoriteElement},
                    FileName = TEST_FILE,
                    IncludePasswords = true
                };

            Exporters exporters = Terminals.Integration.Integrations.CreateExporters(persistence);
            exporters.Export(options);
        }

        [TestCategory("NonSql")]
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
        [TestCategory("NonSql")]
        [TestMethod]
        public void ImportRenamingDuplicitFavoritesTest()
        {
            this.ImportDuplicitFavoritesTest(rename, 2);
        }

        /// <summary>
        /// Tries to import duplicate items into the file persistence overwriting duplicate items
        ///</summary>
        [TestCategory("NonSql")]
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

        private static bool InvokeTheImport(List<FavoriteConfigurationElement> toImport, IPersistence persistence,
            Func<int, DialogResult> strategy)
        {
            var moqInterface = new TestImportUi(strategy);
            var managedImport = new ImportWithDialogs(moqInterface, persistence);
            bool success = managedImport.Import(toImport);
            return success;
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
            var importers = Terminals.Integration.Integrations.CreateImporters(persistence);
            return importers.ImportFavorites(fullFileName);
        }
    }
}
