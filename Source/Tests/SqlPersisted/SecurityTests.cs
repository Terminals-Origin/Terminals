using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    [TestClass]
    public class SecurityTests : TestsLab
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
            settings.DatabaseMasterPassword = string.Empty;
            this.CheckDatabase.UpdateMasterPassword(string.Empty);
        }
        
        [TestMethod]
        public void PasswordUpdateTest()
        {
            IFavorite testFavorite = this.AddFavoriteWithTestPassword();
            this.UpdateWithNewSecuredValue(testFavorite, VALIDATION_VALUE_B);
            this.AssertFavoriteSecuredValues(testFavorite, VALIDATION_VALUE_B);
        }

        [TestMethod]
        public void PasswordClearTest()
        {
            IFavorite testFavorite = this.AddFavoriteWithTestPassword();
            this.UpdateWithNewSecuredValue(testFavorite, VALIDATION_VALUE_B);
            this.UpdateWithNewSecuredValue(testFavorite, string.Empty);
            this.AssertFavoriteSecuredValues(testFavorite, string.Empty);
        }

        private void UpdateWithNewSecuredValue(IFavorite testFavorite, string newValue)
        {
            var guarded = new GuardedSecurity(this.PrimaryPersistence.Security, testFavorite.Security);
            guarded.UserName = newValue;
            testFavorite.Security.Password = newValue;
            testFavorite.Security.Domain = newValue;
            this.PrimaryFavorites.Update(testFavorite);
        }

        private void AssertFavoriteSecuredValues(IFavorite testFavorite, string expectedValue)
        {
            AssertSecurityValues(testFavorite, expectedValue);
            IFavorite resultFavorite = this.SecondaryFavorites.FirstOrDefault();
            AssertSecurityValues(resultFavorite, expectedValue);
        }

        private void AssertSecurityValues(IFavorite testFavorite, string expectedValue)
        {
            var guarded = new GuardedSecurity(this.PrimaryPersistence.Security, testFavorite.Security);
            Assert.AreEqual(expectedValue, testFavorite.Security.Password, "Favorite password doesn't match after update.");
            Assert.AreEqual(expectedValue, guarded.UserName, "Favorite user name doesn't match after update.");
            Assert.AreEqual(expectedValue, testFavorite.Security.Domain, "Favorite user name doesn't match after update.");
        }

        [TestMethod]
        public void CredentialsUpdateTest()
        {
            IFavorite favorite = this.CreateTestFavorite();
            this.PrimaryFavorites.Add(favorite);
            ICredentialSet credential = this.PrimaryFactory.CreateCredentialSet();
            credential.Name = "testCredential";
            credential.Password = VALIDATION_VALUE;
            this.PrimaryPersistence.Credentials.Add(credential);

            IFavorite secondary = this.SecondaryFavorites.FirstOrDefault();
            Assert.AreEqual(Guid.Empty, secondary.Security.Credential, "Favorite credentials should be null");

            favorite.Security.Credential = credential.Id;
            this.PrimaryFavorites.Update(favorite);
            var secondaryFavorites = this.SecondaryFavorites as Terminals.Data.DB.Favorites;
            secondaryFavorites.RefreshCache();
            secondary = this.SecondaryFavorites.FirstOrDefault();
            Guid favoriteCredential = secondary.Security.Credential;
            Assert.AreNotEqual(Guid.Empty, favoriteCredential, "Favorite credential wasn't assigned properly");
            ICredentialSet resolvedCredentials = this.SecondaryPersistence.Credentials[favoriteCredential];
            Assert.AreEqual(VALIDATION_VALUE, resolvedCredentials.Password, "Favorite credentials, doesn't match");
            this.AssertStoredCredentialsCount();
        }

        [TestMethod]
        public void MasterPasswordUpdateTest()
        {
            var newFavorite = this.AddFavoriteWithTestPassword();
            var rdpOptions = newFavorite.ProtocolProperties as RdpOptions;
            rdpOptions.TsGateway.Security.Password = VALIDATION_VALUE;
            this.PrimaryFavorites.Update(newFavorite);
            DatabasePasswordUpdate.UpdateMastrerPassord(settings.ConnectionString, string.Empty, VALIDATION_VALUE_B);
            settings.DatabaseMasterPassword = VALIDATION_VALUE_B;
            bool result = DatabaseConnections.TestConnection();
            Assert.IsTrue(result, "Couldn't update database master password");
            
            // the secondary persistence has to reflect the database password change
            ((SqlPersistenceSecurity)this.SecondaryPersistence.Security).UpdateDatabaseKey();
            IFavorite resultFavorite = this.SecondaryFavorites.FirstOrDefault();
            Assert.AreEqual(VALIDATION_VALUE, resultFavorite.Security.Password,
                "Favorite password doesn't match after database password update.");
            
            var secondaryRdpOptions = resultFavorite.ProtocolProperties as RdpOptions;
            Assert.AreEqual(VALIDATION_VALUE, secondaryRdpOptions.TsGateway.Security.Password,
                "Favorite TS gateway password doesn't match after database password update.");
        }

        private IFavorite AddFavoriteWithTestPassword()
        {
            IFavorite testFavorite = this.CreateTestFavorite();
            testFavorite.Security.Password = VALIDATION_VALUE;
            this.PrimaryFavorites.Add(testFavorite);
            return testFavorite;
        }
    }
}
