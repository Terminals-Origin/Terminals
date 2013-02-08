using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms.Controls;
using Terminals.Integration;

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

        private static int PersistenceFavoritesCount
        {
            get
            {
                return Persistence.Instance.Favorites.Select(favorite => favorite.Name)
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .Count();
            }
        }

        private static int ImportedGroupsCount
        {
            get
            {
                return Persistence.Instance.Groups.Select(favorite => favorite.Name)
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .Count();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            SetDefaultFileLocations();
            List<IFavorite> current = Persistence.Instance.Favorites.ToList();
            Persistence.Instance.Favorites.Delete(current);
        }

        internal static void SetDefaultFileLocations()
        {
            Settings.FileLocations.AssignCustomFileLocations(string.Empty, string.Empty, string.Empty);
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
            int expectedGroups = Persistence.Instance.Groups.Count();
            Assert.AreEqual(expectedGroups, ImportedGroupsCount, "Imported groups count doesn't match.");
        }

        private object InvokeTheImport(List<FavoriteConfigurationElement> toImport, Func<int, DialogResult> strategy)
        {
            var managedImport = new ImportWithDialogs(null);
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
