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
using Tests.FilePersisted;

namespace Tests
{
    [TestClass]
    public class ImportsTest
    {
        private const string DUPLICIT_ITEMS_FILE = @"Issue_21279_import_800_items.xml";

        public TestContext TestContext { get; set; }

        // following functions simulate the answer usually provided by user in UI
        private readonly Func<int, DialogResult> rename = itemsCount => DialogResult.Yes;
        private readonly Func<int, DialogResult> overwrite = itemsCount => DialogResult.No;

        private IPersistence persistence;

        private int PersistenceFavoritesCount
        {
            get
            {
                return this.persistence.Favorites.Select(favorite => favorite.Name)
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .Count();
            }
        }

        private int ImportedGroupsCount
        {
            get
            {
                return this.persistence.Groups.Select(favorite => favorite.Name)
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .Count();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
            this.persistence = new FilePersistence();
            List<IFavorite> current = this.persistence.Favorites.ToList();
            this.persistence.Favorites.Delete(current);
        }

        /// <summary>
        /// Tries to import duplicate items into the file persistence renaming duplicate items
        ///</summary>
        [TestMethod]
        public void ImportRenamingDuplicitFavoritesTest()
        {
            ImportDuplicitFavoritesTest(this.rename, 2);
        }

        /// <summary>
        /// Tries to import duplicate items into the file persistence overwriting duplicate items
        ///</summary>
        [TestMethod]
        public void ImportOverwritingDuplicitFavoritesTest()
        {
            ImportDuplicitFavoritesTest(this.overwrite, 1);
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
            Assert.AreEqual(expected, PersistenceFavoritesCount, "Imported favorites count doesn't match.");
            this.InvokeTheImport(toImport, strategy);
            Assert.AreEqual(expected * expectedSecondImportCount, PersistenceFavoritesCount,
                "Imported favorites count doesn't match after second import");
            int expectedGroups = this.persistence.Groups.Count();
            Assert.AreEqual(expectedGroups, ImportedGroupsCount, "Imported groups count doesn't match.");
        }

        private object InvokeTheImport(List<FavoriteConfigurationElement> toImport, Func<int, DialogResult> strategy)
        {
            var managedImport = new ImportWithDialogs(null, this.persistence);
            var privateObject = new PrivateObject(managedImport);
            return privateObject.Invoke("ImportPreservingNames", new object[] { toImport, strategy });
        }

        private static int ExpectedFavoritesCount(List<FavoriteConfigurationElement> toImport)
        {
            return toImport.Select(favorite => favorite.Name)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .Count();
        }

        private List<FavoriteConfigurationElement> ImportItemsFromFile()
        {
            string fullFileName = Path.Combine(this.TestContext.DeploymentDirectory, DUPLICIT_ITEMS_FILE);
            return Integrations.Importers.ImportFavorites(fullFileName);
        }
    }
}
