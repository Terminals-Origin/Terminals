using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using System.Collections.Generic;
using Terminals.Data.Credentials;
using Terminals.Data.DB;

namespace Tests.SqlPersisted
{
    [TestClass]
    public class FavoritesBatchActionsTest : TestsLab
    {
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
                var guarded = new GuardedSecurity(this.PrimaryPersistence, secondaryFavorite.Security);
                Assert.AreEqual(VALIDATION_VALUE, guarded.UserName, "User name was not set properly to all favorites");
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
                var guarded = this.CreateGuardedSecurity(secondaryFavorite);
                Assert.AreEqual(VALIDATION_VALUE, guarded.Domain, "Domain name was not set properly to all favorites");
            }
        }

        [TestMethod]
        public void SetPasswordToAllFavoritesTest()
        {
            PrimaryFavorites.SetPasswordToAllFavorites(this.TestFavorites, VALIDATION_VALUE);

            foreach (IFavorite secondaryFavorite in SecondaryFavorites)
            {
                var guarded = this.CreateGuardedSecurity(secondaryFavorite);
                Assert.AreEqual(VALIDATION_VALUE, guarded.Password, "Password was not set properly to all favorites");
            }
        }

        private GuardedSecurity CreateGuardedSecurity(IFavorite favorite)
        {
            return new GuardedSecurity(this.SecondaryPersistence, favorite.Security);
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
