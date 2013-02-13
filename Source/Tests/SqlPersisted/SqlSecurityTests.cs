using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    [TestClass]
    public class SqlSecurityTests : SqlTestsLab
    {
        private const string PASSWORD_A = "aaa";
        private const string PASSWORD_B = "bbb";

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
        }

        [TestCleanup]
        public void TestClose()
        {
            Settings.DatabaseMasterPassword = string.Empty;
            this.CheckDatabase.UpdateMasterPassword(string.Empty);
        }
        
        [TestMethod]
        public void TestPasswordUpdate()
        {
            IFavorite testFavorite = this.AddFavoriteWithTestPassword();
            testFavorite.Security.Password = PASSWORD_B;
            this.PrimaryFavorites.Update(testFavorite);

            Assert.AreEqual(PASSWORD_B, testFavorite.Security.Password,
                 "Favorite password doesn't match after first password update.");

            IFavorite resultFavorite = this.SecondaryFavorites.FirstOrDefault();
            Assert.AreEqual(PASSWORD_B, resultFavorite.Security.Password,
                "Secondary Favorite password doesn't match after Favorite password update.");
        }

        [TestMethod]
        public void TestCredentialsUpdate()
        {
            IFavorite favorite = this.CreateTestFavorite();
            this.PrimaryFavorites.Add(favorite);
            ICredentialSet credential = this.PrimaryFactory.CreateCredentialSet();
            credential.Name = "testCredential";
            credential.Password = PASSWORD_A;
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
            Assert.AreEqual(PASSWORD_A, resolvedCredentials.Password, "Favorite credentials, doesn't match");
            this.AssertStoredCredentialsCount();
        }

        [TestMethod]
        public void TestMasterPasswordUpdate()
        {
            var newFavorite = this.AddFavoriteWithTestPassword();
            var rdpOptions = newFavorite.ProtocolProperties as RdpOptions;
            rdpOptions.TsGateway.Security.Password = PASSWORD_A;
            this.PrimaryFavorites.Update(newFavorite);
            DatabasePasswordUpdate.UpdateMastrerPassord(Settings.ConnectionString, string.Empty, PASSWORD_B);
            Settings.DatabaseMasterPassword = PASSWORD_B;
            bool result = DatabaseConnections.TestConnection();
            Assert.IsTrue(result, "Couldn't update database master password");
            
            // the secondary persistence has to reflect the database password change
            ((SqlPersistenceSecurity)this.SecondaryPersistence.Security).UpdateDatabaseKey();
            IFavorite resultFavorite = this.SecondaryFavorites.FirstOrDefault();
            Assert.AreEqual(PASSWORD_A, resultFavorite.Security.Password,
                "Favorite password doesn't match after database password update.");
            
            var secondaryRdpOptions = resultFavorite.ProtocolProperties as RdpOptions;
            Assert.AreEqual(PASSWORD_A, secondaryRdpOptions.TsGateway.Security.Password,
                "Favorite TS gateway password doesn't match after database password update.");
        }

        private IFavorite AddFavoriteWithTestPassword()
        {
            IFavorite testFavorite = this.CreateTestFavorite();
            testFavorite.Security.Password = PASSWORD_A;
            this.PrimaryFavorites.Add(testFavorite);
            return testFavorite;
        }
    }
}
