using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Data;
using Terminals.Forms.Controls;
using Terminals.Integration;
using Terminals.Integration.Export;
using Terminals.Integration.Import;

namespace Tests.FilePersisted
{
    [TestClass]
    public class ImportsTest : FilePersistedTestLab
    {
        private const string DUPLICIT_ITEMS_FILE = @"Issue_21279_import_800_items.xml";

        private const string TEST_FILE = "testOne.xml";

        private const string TEST_PASSWORD = "aaa";

        private const string TEST_DOMAIN = "testDomain";

        private const string TEST_USERNAME = "testUser";

        public TestContext TestContext { get; set; }

        // following functions simulate the answer usually provided by user in UI
        private readonly Func<int, DialogResult> rename = itemsCount => DialogResult.Yes;
        private readonly Func<int, DialogResult> overwrite = itemsCount => DialogResult.No;

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
        public void ExportImportFavoriteTest()
        {
            IFavorite favorite = this.CreateTestFavorite();
            this.ExportFavorite(favorite);
            // to preserve test against identical favorite
            this.Persistence.Favorites.Delete(favorite);
            List<FavoriteConfigurationElement> toImport = this.ImportItemsFromFile(TEST_FILE);
            // persisted favorites are empty, strategy doesnt matter
            this.InvokeTheImport(toImport, this.rename);
            var importedSecurity = this.Persistence.Favorites.First().Security;
            Assert.AreEqual(TEST_USERNAME, importedSecurity.UserName);
            Assert.AreEqual(TEST_DOMAIN, importedSecurity.Domain);
            Assert.AreEqual(TEST_PASSWORD, importedSecurity.Password);
        }

        private IFavorite CreateTestFavorite()
        {
            IFavorite favorite = this.AddFavorite();
            favorite.Name = "testFavorite";
            favorite.ServerName = favorite.Name;
            var security = favorite.Security;
            security.UserName = TEST_USERNAME;
            security.Domain = TEST_DOMAIN;
            security.Password = TEST_PASSWORD;
            this.Persistence.Favorites.Update(favorite);
            return favorite;
        }

        private void ExportFavorite(IFavorite favorite)
        {
            FavoriteConfigurationElement favoriteElement = ModelConverterV2ToV1.ConvertToFavorite(favorite, this.Persistence);

            ExportOptions options = new ExportOptions
                {
                    ProviderFilter = ImportTerminals.TERMINALS_FILEEXTENSION,
                    Favorites = new List<FavoriteConfigurationElement> {favoriteElement},
                    FileName = TEST_FILE,
                    IncludePasswords = true
                };
            Integrations.Exporters.Export(options);
        }

        /// <summary>
        /// Tries to import duplicate items into the file persistence renaming duplicate items
        ///</summary>
        [TestMethod]
        public void ImportRenamingDuplicitFavoritesTest()
        {
            this.ImportDuplicitFavoritesTest(this.rename, 2);
        }

        /// <summary>
        /// Tries to import duplicate items into the file persistence overwriting duplicate items
        ///</summary>
        [TestMethod]
        public void ImportOverwritingDuplicitFavoritesTest()
        {
            this.ImportDuplicitFavoritesTest(this.overwrite, 1);
        }

        private void ImportDuplicitFavoritesTest(Func<int, DialogResult> strategy, int expectedSecondImportCount)
        {
            // call import first to force the persistence initialization
            List<FavoriteConfigurationElement> toImport = this.ImportItemsFromFile();

            // 887 obtained by manual check of the xml elements
            Assert.AreEqual(887, toImport.Count, "Some items from Import file were not identified");
            object result = this.InvokeTheImport(toImport, strategy);
            Assert.AreEqual(true, result, "Import wasn't successful");
            int expected = ExpectedFavoritesCount(toImport);
            Assert.AreEqual(expected, this.PersistenceFavoritesCount, "Imported favorites count doesn't match.");
            this.InvokeTheImport(toImport, strategy);
            Assert.AreEqual(expected * expectedSecondImportCount, this.PersistenceFavoritesCount,
                "Imported favorites count doesn't match after second import");
            int expectedGroups = this.Persistence.Groups.Count();
            Assert.AreEqual(expectedGroups, this.ImportedGroupsCount, "Imported groups count doesn't match.");
        }

        private object InvokeTheImport(List<FavoriteConfigurationElement> toImport, Func<int, DialogResult> strategy)
        {
            var managedImport = new ImportWithDialogs(null, this.Persistence);
            var privateObject = new PrivateObject(managedImport);
            return privateObject.Invoke("ImportPreservingNames", new object[] { toImport, strategy });
        }

        private static int ExpectedFavoritesCount(List<FavoriteConfigurationElement> toImport)
        {
            return toImport.Select(favorite => favorite.Name)
                           .Distinct(StringComparer.CurrentCultureIgnoreCase)
                           .Count();
        }

        private List<FavoriteConfigurationElement> ImportItemsFromFile(string fileName = DUPLICIT_ITEMS_FILE)
        {
            string fullFileName = Path.Combine(this.TestContext.DeploymentDirectory, fileName);
            return Integrations.Importers.ImportFavorites(fullFileName);
        }
    }
}
