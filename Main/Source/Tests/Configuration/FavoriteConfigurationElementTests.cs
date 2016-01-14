using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Configuration;
using Terminals.Data;
using Tests.FilePersisted;

namespace Tests.Configuration
{
    [TestClass]
    public class FavoriteConfigurationElementTests : FilePersistedTestLab
    {
        private const string EXPECTED_USER = "EXPECTED_USER";

        private const string MESSAGE = "Serialization should work for all protected properties.";
        private const string EXPECTEDPASSWORD = "EXPECTEDPASSWORD";
        const string CREDENTIAL_NAME = "CREDENTIAL_NAME";
        private const string EXPECTED_DOMAIN = "EXPECTED_DOMAIN";

        [TestMethod]
        public void NewValue_SetGetTsgwPassword_ReturnsSavedValue()
        {
            FavoriteConfigurationSecurity favoriteSecurity = this.CreateFavoriteConfigurationSecurity();
            favoriteSecurity.TsgwPassword = EXPECTEDPASSWORD;
            Assert.AreEqual(EXPECTEDPASSWORD, favoriteSecurity.TsgwPassword, MESSAGE);
        }

        [TestMethod]
        public void NewValue_SetGetPassword_ReturnsSavedValue()
        {
            var security = this.CreateFavoriteConfigurationSecurity();
            security.Password = EXPECTEDPASSWORD;
            Assert.AreEqual(EXPECTEDPASSWORD, security.Password, MESSAGE);
        }

        [TestMethod]
        public void WithCredential_GetPassword_ReturnsCredentialPassword()
        {
            var security = this.CreateFavoriteConfigurationSecurity();
            Assert.AreEqual(EXPECTEDPASSWORD, security.Password, "Password is primary resolved from Credential.");
        }

        [TestMethod]
        public void NewValue_SetGetDomainName_ReturnsSavedValue()
        {
            FavoriteConfigurationElement favorite = this.CreateFavorite();
            favorite.DomainName = EXPECTED_DOMAIN;
            Assert.AreEqual(EXPECTED_DOMAIN, favorite.DomainName, MESSAGE);
        }

        [TestMethod]
        public void WithCredential_SetGetDomainName_ReturnsEmpty()
        {
            FavoriteConfigurationElement favorite = CreteFavoriteWithCredential();
            Assert.AreEqual(string.Empty, favorite.DomainName, MESSAGE);
        }

        [TestMethod]
        public void WithCredential_ResolveDomain_ReturnsCredentialDomain()
        {
            FavoriteConfigurationSecurity favoriteSecurity = this.CreateFavoriteConfigurationSecurity();
            string resolved = favoriteSecurity.ResolveDomainName();
            Assert.AreEqual(EXPECTED_DOMAIN, resolved, "Domain is primary resolved from Credential.");
        }

        [TestMethod]
        public void NewValue_SetGetUserName_ReturnsSavedValue()
        {
            FavoriteConfigurationElement favorite = this.CreateFavorite();
            favorite.UserName = EXPECTED_USER;
            Assert.AreEqual(EXPECTED_USER, favorite.UserName, MESSAGE);
        }

        [TestMethod]
        public void WithCredential_SetGetUserName_ReturnsEmpty()
        {
            FavoriteConfigurationElement favorite = CreteFavoriteWithCredential();
            Assert.AreEqual(string.Empty, favorite.UserName, MESSAGE);
        }

        [TestMethod]
        public void WithCredential_ResolveUserName_ReturnsCredentialUserName()
        {
            FavoriteConfigurationSecurity favoriteSecurity = this.CreateFavoriteConfigurationSecurity();
            Assert.AreEqual(EXPECTED_USER, favoriteSecurity.ResolveUserName(), "UserName is primary resolved from Credential.");
        }

        private FavoriteConfigurationSecurity CreateFavoriteConfigurationSecurity()
        {
            FavoriteConfigurationElement favorite = this.CreteFavoriteWithCredential();
            return new FavoriteConfigurationSecurity(this.Persistence, favorite);
        }

        private FavoriteConfigurationElement CreteFavoriteWithCredential()
        {
            CreateCredentialInPersistence();
            var favorite = CreateFavorite();
            favorite.Credential = CREDENTIAL_NAME;
            return favorite;
        }

        private FavoriteConfigurationElement CreateFavorite()
        {
            return new FavoriteConfigurationElement(this.Persistence.Security, this.Persistence.Credentials);
        }

        private void CreateCredentialInPersistence()
        {
            // because of internal usage in Favorite we have to reference the singleton
            var persistence = this.Persistence;
            var credential = new CredentialSet();
            credential.AssignStore(persistence.Security);
            credential.Name = CREDENTIAL_NAME;
            credential.Password = EXPECTEDPASSWORD;
            credential.Domain = EXPECTED_DOMAIN;
            credential.UserName = EXPECTED_USER;
            persistence.Credentials.Add(credential);
        }
    }
}
