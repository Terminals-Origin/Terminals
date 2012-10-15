using System;
using System.Data.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using CredentialSet = Terminals.Data.DB.CredentialSet;

namespace Tests
{
    /// <summary>
    ///This is a test class for database implementation of StoredCredentials
    ///</summary>
    [TestClass]
    public class SqlCredentialsTest : SqlTestsLab
    {
        private const string TEST_PASSWORD = "aaa";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private ICredentials PrimaryCredentials
        {
            get
            {
                return this.PrimaryPersistence.Credentials;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
        }

        private CredentialSet CreateTestCredentialSet()
        {
            CredentialSet credentials = this.PrimaryFactory.CreateCredentialSet() as CredentialSet;
            credentials.Name = "TestCredentialName";
            credentials.Domain = "TestDomain";
            credentials.UserName = "TestUserName";
            ((ICredentialSet)credentials).Password = TEST_PASSWORD;
            return credentials;
        }

        [TestMethod]
        public void AddCredentialsTest()
        {
            this.AddTestCredentialsToDatabase();

            CredentialSet checkCredentialSet = this.CheckDatabaseCredentials.FirstOrDefault();

            Assert.IsNotNull(checkCredentialSet, "Credential didnt reach the database");
            Assert.AreEqual(TEST_PASSWORD, ((ICredentialSet)checkCredentialSet).Password, "Password doesnt match");
        }

        private ObjectQuery<CredentialSet> CheckDatabaseCredentials
        {
            get
            {
                return this.CheckDatabase.CredentialBase
                    .OfType<CredentialSet>();
            }
        }

        [TestMethod]
        public void RemoveCredentialsTest()
        {
            var testCredentials = this.AddTestCredentialsToDatabase();

            int credentialsCountBefore = this.CheckDatabaseCredentials.Count();
            PrimaryCredentials.Remove(testCredentials);
            PrimaryCredentials.Save();
            int credentialsCountAfter = this.CheckDatabaseCredentials.Count();

            int baseAfter = this.CheckDatabase.ExecuteStoreQuery<int>("select Count(Id) from CredentialBase")
                .FirstOrDefault();

            Assert.AreEqual(1, credentialsCountBefore, "credential wasnt added to the database");
            Assert.AreEqual(0, credentialsCountAfter, "credential wasnt removed from the database");
            Assert.AreEqual(0, baseAfter, "credentialbase wasnt removed from the database");
        }

        private CredentialSet AddTestCredentialsToDatabase()
        {
            var testCredentials = this.CreateTestCredentialSet();
            PrimaryCredentials.Add(testCredentials);
            PrimaryCredentials.Save();
            return testCredentials;
        }

        /// <summary>
        ///A test for UpdatePasswordsByNewKeyMaterial
        ///</summary>
        [TestMethod]
        public void UpdateCredentialsPasswordsByNewKeyMaterialTest()
        {
            this.AddTestCredentialsToDatabase();
            this.PrimaryPersistence.Security.UpdateMasterPassword(String.Empty);

            CredentialSet checkCredentials = this.CheckDatabaseCredentials.FirstOrDefault();
            Assert.AreEqual(TEST_PASSWORD, ((ICredentialSet)checkCredentials).Password, "Passwrod lost after update of keymaterial");
        }
    }
}
