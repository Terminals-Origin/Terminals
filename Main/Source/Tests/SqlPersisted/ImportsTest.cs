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
        private readonly Settings settings = Settings.Instance;

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
            settings.DatabaseMasterPassword = string.Empty;
        }

        [TestMethod]
        public void ExportImportFavoriteTest()
        {
            IPersistence persistence = this.PrimaryPersistence;
            DatabasePasswordUpdate.UpdateMastrerPassord(settings.ConnectionString, string.Empty, VALIDATION_VALUE_B);
            settings.DatabaseMasterPassword = VALIDATION_VALUE_B;
            ((SqlPersistenceSecurity)persistence.Security).UpdateDatabaseKey();
            string filePath = this.TestContext.DeploymentDirectory;
            Integrations.ImportsTest.ExportImportFavorite(persistence, filePath);
        }
    }
}