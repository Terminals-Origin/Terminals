using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.DB;

namespace Tests
{
    [TestClass]
    public class SqlSecurityTests : SqlTestsLab
    {
        private const string PASSWORD_A = "aaa";
        private const string PASSWORD_B = "bbb";

        public TestContext TestContext { get; set; }

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
            PrimaryFavorites.Update(testFavorite);

            Assert.AreEqual(PASSWORD_B, testFavorite.Security.Password,
                 "Favorite password doesnt match after first password update.");

            IFavorite resultFavorite = SecondaryFavorites.FirstOrDefault();
            Assert.AreEqual(PASSWORD_B, resultFavorite.Security.Password,
                "Secondary Favorite password doesnt match after Favorite password update.");
        }

        [TestMethod]
        public void TestCredentialsUpdate()
        {
            IFavorite favorite = this.CreateTestFavorite();
            PrimaryFavorites.Add(favorite);
            ICredentialSet credential = this.PrimaryFactory.CreateCredentialSet();
            credential.Name = "testCredential";
            credential.Password = PASSWORD_A;
            PrimaryPersistence.Credentials.Add(credential);

            IFavorite secondary = SecondaryFavorites.FirstOrDefault();
            Assert.AreEqual(Guid.Empty, secondary.Security.Credential, "Favorite credentails should be null");

            favorite.Security.Credential = credential.Id;
            PrimaryFavorites.Update(favorite);
            var secondaryFavorites = SecondaryFavorites as Terminals.Data.DB.Favorites;
            secondaryFavorites.RefreshCache();
            secondary = SecondaryFavorites.FirstOrDefault();
            Guid favoriteCredential = secondary.Security.Credential;
            Assert.AreNotEqual(Guid.Empty, favoriteCredential, "Favorite credentail wasnt assigned properly");
            ICredentialSet resolvedCredentials = SecondaryPersistence.Credentials[favoriteCredential];
            Assert.AreEqual(PASSWORD_A, resolvedCredentials.Password, "Favorite credentials, doesnt match");
        }

        [TestMethod]
        public void TestMasterPasswordUpdate()
        {
            this.AddFavoriteWithTestPassword();
            Database.UpdateMastrerPassord(Settings.ConnectionString, string.Empty, PASSWORD_B);
            Settings.DatabaseMasterPassword = PASSWORD_B;
            bool result = Database.TestConnection();
            Assert.IsTrue(result, "Couldnt update database master password");
            IFavorite resultFavorite = SecondaryFavorites.FirstOrDefault();
            Assert.AreEqual(PASSWORD_A, resultFavorite.Security.Password,
                "Favorite password doesnt match after database password update.");
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
