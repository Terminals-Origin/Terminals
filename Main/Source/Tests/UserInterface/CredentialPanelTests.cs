using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Data;
using Terminals.Forms.Controls;

namespace Tests.UserInterface
{
    [TestClass]
    public class CredentialPanelTests
    {
        private const string EXPECTED_USER_NAME = "EXPECTED_USER_NAME";
        private const string EXPECTED_DOMAIN = "EXPECTED_DOMAIN";
        private const string EXPECTED_PASSWORD = "EXPECTED_PASSWORD";

        private readonly CredentialsPanel credentialPanel = new CredentialsPanel();

        private ISecurityOptions security;

        [TestInitialize]
        public void Setup()
        {
            this.security = this.CreateSecurity();
            this.security.UserName = EXPECTED_USER_NAME;
            this.security.Domain = EXPECTED_DOMAIN;
            this.security.Password = EXPECTED_PASSWORD;
            this.security.EncryptedPassword = EXPECTED_PASSWORD;
        }

        [TestMethod]
        public void Domain_LoadDirectlyAndSave_ReturnsOriginalValue()
        {
            this.credentialPanel.LoadDirectlyFrom(this.security);
            ISecurityOptions result = CreateSecurity();
            this.credentialPanel.SaveUserAndDomain(result);
            Assert.AreEqual(EXPECTED_DOMAIN, result.Domain, "Loaded domain has to be saved back in Save");
        }

        [TestMethod]
        public void UserName_LoadDirectlyAndSave_ReturnsOriginalValue()
        {
            this.credentialPanel.LoadDirectlyFrom(this.security);
            ISecurityOptions result = CreateSecurity();
            this.credentialPanel.SaveUserAndDomain(result);
            Assert.AreEqual(EXPECTED_USER_NAME, result.UserName, "Loaded username has to be saved back in Save");
        }

        [TestMethod]
        public void ValidPassword_LoadfromSavePassword_ReturnsOriginalValue()
        {
            this.credentialPanel.LoadFrom(this.security);
            ISecurityOptions result = CreateSecurity();
            this.credentialPanel.SavePassword(result);
            Assert.AreEqual(EXPECTED_PASSWORD, result.Password, "Loaded valid password has to be saved back in Save");
        }

        private ISecurityOptions CreateSecurity()
        {
            var mockSecurity = new Mock<ISecurityOptions>();
            mockSecurity.SetupAllProperties();
            return mockSecurity.Object;
        }
    }
}
