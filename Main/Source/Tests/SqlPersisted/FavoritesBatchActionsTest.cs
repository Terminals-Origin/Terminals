using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using System.Collections.Generic;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    [TestClass]
    public class FavoritesBatchActionsTest : SqlTestsLab
    {
        private const string VALIDATION_VALUE = "AAA";

        private List<IFavorite> TestFavorites
        {
            get
            {
                return this.PrimaryFavorites.ToList();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
            this.CreateTestFavorites();
        }

        private void CreateTestFavorites()
        {
            List<IFavorite> testFavorites = new List<IFavorite>();
            for (int index = 0; index < 3; index++)
            {
                DbFavorite favorite = this.CreateTestFavorite();
                favorite.Name += index;
                testFavorites.Add(favorite);
            }
            
            this.PrimaryFavorites.Add(testFavorites);
        }

        [TestMethod]
        public void ApplyUserNameToAllFavoritesTest()
        {
            PrimaryFavorites.ApplyUserNameToAllFavorites(this.TestFavorites, VALIDATION_VALUE);

            foreach (IFavorite secondaryFavorite in SecondaryFavorites)
            {
                string finalUserName = secondaryFavorite.Security.UserName;
                Assert.AreEqual(VALIDATION_VALUE, finalUserName, "User name was not set properly to all favorites");
            }
        }

        [TestMethod]
        public void ApplyCredentialsToAllFavoritesTest()
        {
            ICredentialSet credentialSet = PrimaryFactory.CreateCredentialSet();
            credentialSet.Name = VALIDATION_VALUE;
            PrimaryPersistence.Credentials.Add(credentialSet);
            PrimaryFavorites.ApplyCredentialsToAllFavorites(this.TestFavorites, credentialSet);

            foreach (IFavorite secondaryFavorite in SecondaryFavorites)
            {
                Guid finalCredential = secondaryFavorite.Security.Credential;
                Assert.AreNotEqual(Guid.Empty, finalCredential, "Credential was not set properly to all favorites");
            }

            this.AssertStoredCredentialsCount();
        }

        [TestMethod]
        public void ApplyDomainNameToAllFavoritesTest()
        {
            PrimaryFavorites.ApplyDomainNameToAllFavorites(this.TestFavorites, VALIDATION_VALUE);

            foreach (IFavorite secondaryFavorite in SecondaryFavorites)
            {
                string finalDomain = secondaryFavorite.Security.Domain;
                Assert.AreEqual(VALIDATION_VALUE, finalDomain, "Domain name was not set properly to all favorites");
            }
        }

        [TestMethod]
        public void SetPasswordToAllFavoritesTest()
        {
            PrimaryFavorites.SetPasswordToAllFavorites(this.TestFavorites, VALIDATION_VALUE);

            foreach (IFavorite secondaryFavorite in SecondaryFavorites)
            {
                string finalPassword = secondaryFavorite.Security.Password;
                Assert.AreEqual(VALIDATION_VALUE, finalPassword, "Password was not set properly to all favorites");
            }
        }

        [TestMethod]
        public void SetUserNameConcurrentUpdateTest()
        {
            IFavorite sendaryFavorite = this.SecondaryFavorites.First();
            this.SecondaryFavorites.Delete(sendaryFavorite);
            PrimaryFavorites.ApplyUserNameToAllFavorites(this.TestFavorites, VALIDATION_VALUE);
        }
    }
}
