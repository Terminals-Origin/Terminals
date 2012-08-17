using System;
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
    public class SqlCredentialsTest
    {
        private const string TEST_PASSWORD = "aaa";
        private SqlTestsLab lab;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.lab = new SqlTestsLab();
            this.lab.InitializeTestLab();
        }

        [TestCleanup]
        public void TestClose()
        {
            this.lab.ClearTestLab();
        }

        private CredentialSet CreateTestCredentialSet()
        {
            CredentialSet credentials = this.lab.Persistence.Factory.CreateCredentialSet() as CredentialSet;
            credentials.Name = "TestCredentialName";
            credentials.Domain = "TestDomain";
            credentials.UserName = "TestUserName";
            ((ICredentialSet)credentials).Password = TEST_PASSWORD;
            return credentials;
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod]
        public void AddCredentialsTest()
        {
            this.AddTestCredentialsToDatabase();

            CredentialSet checkCredentialSet = this.lab.CheckDatabase.CredentialBase
                .OfType<CredentialSet>().FirstOrDefault();

            Assert.IsNotNull(checkCredentialSet, "Credential didnt reach the database");
            Assert.AreEqual(TEST_PASSWORD, ((ICredentialSet)checkCredentialSet).Password, "Password doesnt match");
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod]
        public void RemoveCredentialsTest()
        {
            var testCredentials = this.AddTestCredentialsToDatabase();

            var credentialasDatabase = this.lab.Persistence.Credentials;
            var checkDatabase = this.lab.CheckDatabase;
            int credentialsCountBefore = checkDatabase.CredentialBase.OfType<CredentialSet>().Count();
            credentialasDatabase.Remove(testCredentials);
            credentialasDatabase.Save();
            int credentialsCountAfter = checkDatabase.CredentialBase.OfType<CredentialSet>().Count();

            int baseAfter = checkDatabase.ExecuteStoreQuery<int>("select Count(Id) from CredentialBase")
                .FirstOrDefault();

            Assert.AreEqual(1, credentialsCountBefore, "credential wasnt added to the database");
            Assert.AreEqual(0, credentialsCountAfter, "credential wasnt removed from the database");
            Assert.AreEqual(0, baseAfter, "credentialbase wasnt removed from the database");
        }

        private CredentialSet AddTestCredentialsToDatabase()
        {
            var testCredentials = this.CreateTestCredentialSet();
            var credentialasDatabase = this.lab.Persistence.Credentials;
            credentialasDatabase.Add(testCredentials);
            credentialasDatabase.Save();
            return testCredentials;
        }

        /// <summary>
        ///A test for UpdatePasswordsByNewKeyMaterial
        ///</summary>
        [TestMethod]
        public void UpdateCredentialsPasswordsByNewKeyMaterialTest()
        {
            this.AddTestCredentialsToDatabase();
            this.lab.Persistence.Security.UpdateMasterPassword(String.Empty);

            CredentialSet checkCredentials = this.lab.CheckDatabase.CredentialBase
                .OfType<CredentialSet>().FirstOrDefault();

            Assert.AreEqual(TEST_PASSWORD, ((ICredentialSet)checkCredentials).Password, "Passwrod lost after update of keymaterial");
        }
    }
}
