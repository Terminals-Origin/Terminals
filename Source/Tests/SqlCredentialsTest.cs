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

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
            var credentials = this.CreateTestCredentialSet();
            this.lab.Persistence.Credentials.Add(credentials);
            this.lab.Persistence.Credentials.Save();

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
            var credentials = this.CreateTestCredentialSet();
            this.lab.Persistence.Credentials.Add(credentials);
            this.lab.Persistence.Credentials.Save();

            int credentialsCountBefore = this.lab.CheckDatabase.CredentialBase
                .OfType<CredentialSet>().Count();
            this.lab.Persistence.Credentials.Remove(credentials);
            this.lab.Persistence.Credentials.Save();
            int credentialsCountAfter = this.lab.CheckDatabase.CredentialBase
                .OfType<CredentialSet>().Count();

            Assert.AreEqual(1, credentialsCountBefore, "credential wasnt added to the database");
            Assert.AreEqual(0, credentialsCountAfter, "credential wasnt added to the database");
        }

        /// <summary>
        ///A test for UpdatePasswordsByNewKeyMaterial
        ///</summary>
        [TestMethod]
        public void UpdateCredentialsPasswordsByNewKeyMaterialTest()
        {
            var credentials = this.CreateTestCredentialSet();
            this.lab.Persistence.Credentials.Add(credentials);
            this.lab.Persistence.Credentials.Save();

            this.lab.Persistence.Credentials.UpdatePasswordsByNewKeyMaterial(String.Empty);
            this.lab.Persistence.Credentials.Save();

            CredentialSet checkCredentials = this.lab.CheckDatabase.CredentialBase
                .OfType<CredentialSet>().FirstOrDefault();

            Assert.AreEqual(TEST_PASSWORD, ((ICredentialSet)checkCredentials).Password, "Passwrod lost after update of keymaterial");
        }
    }
}
