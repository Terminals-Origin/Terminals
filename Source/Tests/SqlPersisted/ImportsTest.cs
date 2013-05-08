using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    /// <summary>
    /// Export import into the SQL persistence
    /// </summary>
    [TestClass]
    public class ImportsTest : TestsLab
    {
        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
            Settings.DatabaseMasterPassword = string.Empty;
        }

        [TestMethod]
        public void ExportImportFavoriteTest()
        {
            IPersistence persistence = this.PrimaryPersistence;
            DatabasePasswordUpdate.UpdateMastrerPassord(Settings.ConnectionString, string.Empty, VALIDATION_VALUE_B);
            Settings.DatabaseMasterPassword = VALIDATION_VALUE_B;
            ((SqlPersistenceSecurity)persistence.Security).UpdateDatabaseKey();
            string filePath = this.TestContext.DeploymentDirectory;
            FilePersisted.ImportsTest.ExportImportFavorite(persistence, filePath);
        }
    }
}