using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.Credentials;
using Tests.FilePersisted;
using Tests.Helpers;

namespace Tests.Passwords
{
    [TestClass]
    public class GuardedSecurityTests
    {
        private const string PASSWORD = "TestPassword";

        private Mock<IPersistence> persistenceMock;

        private ISecurityOptions original;

        [TestMethod]
        public void ValidParams_ResolveCredentials_ReturnsNotOriginalInstance()
        {
            var guarded = new GuardedSecurity(this.persistenceMock.Object, original);
            var result = guarded.GetResolvedCredentials();
            result.Password = "Irrelevant";
            const string MESSAGE = "Changing the resolved instance password cant affect the original. We return new instance.";
            Assert.AreNotEqual(original.EncryptedPassword, result.EncryptedPassword, MESSAGE);
        }

        [TestMethod]
        public void SavedPassword_ResolveCredentials_ReturnsSavedPassword()
        {
            var guarded = new GuardedSecurity(this.persistenceMock.Object, original);
            guarded.Password = PASSWORD;
            var result = guarded.GetResolvedCredentials();
            const string MESSAGE = "Localy saved password has always precedence over default password or stored credential.";
            Assert.AreEqual(original.EncryptedPassword, result.EncryptedPassword, MESSAGE);
        }

        [TestMethod]
        public void CredentialSet_ResolveCredentials_ReturnsCredentialValues()
        {
            ICredentialSet credential = this.SetupCredential();
            original.Credential = credential.Id;
            IGuardedSecurity result = this.ResolveCredentials(original);
            const string MESSAGE = "When credential set is defined, its pasword is used to connect.";
            Assert.AreEqual(credential.EncryptedPassword, result.EncryptedPassword, MESSAGE);
        }

        [TestMethod]
        public void OnlyDefaultPassword_ResolveCredentials_ReturnsDefaultPassword()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
            Settings.Instance.DefaultPassword = PASSWORD;
            IGuardedSecurity result = this.ResolveCredentials(original);
            const string MESSAGE = "If no values are defined in stored password or credentials, they are resolved from default settings.";
            Settings.Instance.DefaultPassword = string.Empty;
            Assert.AreEqual(PASSWORD, result.Password, MESSAGE);
        }

        private ICredentialSet SetupCredential()
        {
            var credential = new CredentialSet();
            var guardedSec = new GuardedCredential(credential, this.persistenceMock.Object.Security);
            guardedSec.Password = PASSWORD;
            var credentials = new Mock<ICredentials>();
            credentials.Setup(cr => cr[It.IsAny<Guid>()]).Returns(credential);
            this.persistenceMock.SetupGet(p => p.Credentials).Returns(credentials.Object);
            return credential;
        }

        private IGuardedSecurity ResolveCredentials(ISecurityOptions original)
        {
            var guarded = new GuardedSecurity(this.persistenceMock.Object, original);
            return  guarded.GetResolvedCredentials();
        }

        [TestInitialize]
        public void Setup()
        {
            this.persistenceMock = TestMocksFactory.CreatePersistence();
            Settings.Instance.PersistenceSecurity = this.persistenceMock.Object.Security;
            this.original = new SecurityOptions() { EncryptedPassword = string.Empty };
        }
    }
}
