using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;

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

        [TestMethod]
        public void ExportImportFavoriteTest()
        {
            IPersistence persistence = this.PrimaryPersistence;
            string filePath = this.TestContext.DeploymentDirectory;
            FilePersisted.ImportsTest.ExportImportFavorite(persistence, filePath);
        }
    }
}