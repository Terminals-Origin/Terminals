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
        private const string DUPLICIT_ITEMS_FILE = @"Data\Issue_21279_import_800_items.xml";

        public TestContext TestContext { get; set; }

        // folowing functions simulate the answer usualy provided by user in UI
        private readonly Func<int, DialogResult> rename = itemsCount => DialogResult.Yes;

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

        /// <summary>
        /// Tries to import duplict items into the file persistence
        ///</summary>
        [TestMethod]
        public void ImportDuplicitFavoritesTest()
        {
            // call import first to force the persistence initialization
            List<FavoriteConfigurationElement> favoritesToImport = this.ImportItemsFromFile();

            // 887 obtained by manual check of the xml elements
            Assert.AreEqual(887, favoritesToImport.Count, "Some items from Import file were not identified");
            object result = this.InvokeTheImport(favoritesToImport);
            Assert.AreEqual(true, result, "Import wasnt successfull");
            int expected = ExpectedFavoritesCount(favoritesToImport);
            Assert.AreEqual(expected, PersistenceFavoritesCount, "Imported favorites count doesnt match.");
            this.InvokeTheImport(favoritesToImport);
            Assert.AreEqual(expected * 2, PersistenceFavoritesCount, "Imported favorites count doesnt match after second import");
            int expectedGroups = Persistence.Instance.Groups.Count();
            Assert.AreEqual(expectedGroups, ImportedGroupsCount, "Imported groups count doesnt match.");
        }

        private object InvokeTheImport(List<FavoriteConfigurationElement> favoritesToImport)
        {
            var managedImport = new ImportWithDialogs(null);
            var privateObject = new PrivateObject(managedImport);
            return privateObject.Invoke("ImportPreservingNames", new object[] { favoritesToImport, this.rename });
        }

        private static int ExpectedFavoritesCount(List<FavoriteConfigurationElement> favoritesToImport)
        {
            return favoritesToImport.Select(favorite => favorite.Name)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .Count();
        }

        private List<FavoriteConfigurationElement> ImportItemsFromFile()
        {
            Settings.FileLocations.AssignCustomFileLocations(string.Empty, string.Empty, string.Empty);
            string fullFileName = Path.Combine(this.TestContext.TestDir, DUPLICIT_ITEMS_FILE);
            return Integrations.Importers.ImportFavorites(fullFileName);
        }
    }
}
