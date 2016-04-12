using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    /// <summary>
    ///This is a test class for database implementation of StoredCredentials
    ///</summary>
    [TestClass]
    public class CredentialsTest : TestsLab
    {
        private ICredentials PrimaryCredentials
        {
            get
            {
                return this.PrimaryPersistence.Credentials;
            }
        }

        private IQueryable<DbCredentialSet> CheckDatabaseCredentials
        {
            get
            {
                return this.CheckDatabase.CredentialBase
                    .OfType<DbCredentialSet>();
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

        private DbCredentialSet CreateTestCredentialSet()
        {
            DbCredentialSet credentials = this.PrimaryFactory.CreateCredentialSet() as DbCredentialSet;
            credentials.Name = "TestCredentialName";
            var guarded = new GuardedCredential(credentials, this.PrimaryPersistence.Security);
            guarded.Domain = "TestDomain";
            guarded.UserName = "TestUserName";
            guarded.Password = VALIDATION_VALUE;
            return credentials;
        }

        [TestMethod]
        public void AddCredentialsTest()
        {
            this.AddTestCredentialsToDatabase();

            var checkCredentialSet = this.SecondaryPersistence.Credentials.FirstOrDefault() as DbCredentialSet;
            string resolvedPassword = this.ResolveVerifiedPassword(checkCredentialSet);
            Assert.IsNotNull(checkCredentialSet, "Credential didn't reach the database");
            Assert.AreEqual(VALIDATION_VALUE, resolvedPassword, "Password doesn't match");
        }

        [TestMethod]
        public void RemoveCredentialsTest()
        {
            var testCredentials = this.AddTestCredentialsToDatabase();

            int credentialsCountBefore = this.CheckDatabaseCredentials.Count();
            this.PrimaryCredentials.Remove(testCredentials);
            int credentialsCountAfter = this.CheckDatabaseCredentials.Count();

            int baseAfter = this.CheckDatabase.Database
                .SqlQuery<int>("select Count(Id) from CredentialBase")
                .FirstOrDefault();

            Assert.AreEqual(1, credentialsCountBefore, "credential wasn't added to the database");
            Assert.AreEqual(0, credentialsCountAfter, "credential wasn't removed from the database");
            Assert.AreEqual(0, baseAfter, "credential base wasn't removed from the database");
        }

        private DbCredentialSet AddTestCredentialsToDatabase()
        {
            var testCredentials = this.CreateTestCredentialSet();
            this.PrimaryCredentials.Add(testCredentials);
            return testCredentials;
        }

        /// <summary>
        ///A test for UpdatePasswordsByNewKeyMaterial
        ///</summary>
        [TestMethod]
        public void UpdateCredentialsPasswordsByNewKeyMaterialTest()
        {
            // this is the only one test, which plays with different master passwords
            Settings.Instance.PersistenceSecurity = this.PrimaryPersistence.Security;
            this.AddTestCredentialsToDatabase();
            this.PrimaryPersistence.Security.UpdateMasterPassword(VALIDATION_VALUE_B);

            ICredentialSet checkCredentials = this.SecondaryPersistence.Credentials.FirstOrDefault();
            string resolvedPassword = this.ResolveVerifiedPassword(checkCredentials);
            Assert.AreEqual(VALIDATION_VALUE, resolvedPassword, "Password lost after update of key material");
        }

        private string ResolveVerifiedPassword(ICredentialSet checkCredentialSet)
        {
            var guarded = new GuardedCredential(checkCredentialSet, this.SecondaryPersistence.Security);
            return guarded.Password;
        }
    }
}
