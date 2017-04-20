using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals;
using Terminals.Configuration;
using Terminals.Data;
using Tests.Connections;
using Tests.FilePersisted;

namespace Tests
{
    [TestClass]
    public class PersistenceFactoryTests
    {
        // *password prompt ->     password valid ->  Persistence initialize -> START 
        //        (cancel) -> EXIT      (invalid) ->  *         (SQL failed) -> User wants fallbact from ->       Initialize -> START
        //                                                    (Files failed) -> EXIT                (NO) -> EXIT    (FAILED) -> EXIT

        private const string EXPECTED_PASSWORD = "ExpectedPassword";
        private bool passwordRequested;
        private bool exitCalled;
        private bool fallbackRequested;
        private PersistenceFactory factory;
        private AuthenticationPrompt authenticationPrompt;

        [TestInitialize]
        public void TestInitialize()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
            this.passwordRequested = false;
            this.exitCalled = false;
            this.fallbackRequested = false;
            this.authenticationPrompt = new AuthenticationPrompt();
            this.factory = new PersistenceFactory(Settings.Instance, TestConnectionManager.Instance, TestConnectionManager.CreateTestFavoriteIcons());
        }

        [TestMethod]
        public void FilePersistenceNoMasterPassword_Authenticate_CallsNoCallback()
        {
            this.SetupMasterPassword(string.Empty);
            Mock<IStartupUi> startupUiMock = this.SetupStartupUi();
            this.Authenticate(startupUiMock);
            Assert.IsFalse(this.exitCalled || this.passwordRequested || this.fallbackRequested);
        }

        [TestMethod]
        public void CancelPasswordPrompt_Authenticate_CallsExit()
        {
            this.SetupMasterPassword();
            this.authenticationPrompt.Canceled = true;
            Mock<IStartupUi> startupUiMock = this.SetupStartupUi();
            this.Authenticate(startupUiMock);
            Assert.IsFalse(!this.exitCalled || !this.passwordRequested || this.fallbackRequested);
        }

        [TestMethod]
        public void FilePersistenceMasterPassword_Authenticate_AsksForMasterPassword()
        {
            this.SetupMasterPassword();
            this.authenticationPrompt = new AuthenticationPrompt() { Password = EXPECTED_PASSWORD };
            Mock<IStartupUi> startupUiMock = this.SetupStartupUi();
            this.Authenticate(startupUiMock);
            Assert.IsFalse(this.exitCalled || !this.passwordRequested || this.fallbackRequested);
        }

        private void SetupMasterPassword(string newMasterPassword = EXPECTED_PASSWORD)
        {
            IPersistence persistence = this.factory.CreatePersistence();
            persistence.Security.UpdateMasterPassword(newMasterPassword);
        }

        private void Authenticate(Mock<IStartupUi> startupUiMock)
        {
            IPersistence persistence = this.factory.CreatePersistence();
            this.factory.AuthenticateByMasterPassword(persistence, startupUiMock.Object);
        }

        private Mock<IStartupUi> SetupStartupUi()
        {
            var startupUiMock = new Mock<IStartupUi>();
            startupUiMock.Setup(ui => ui.Exit())
                .Callback(() => this.exitCalled = true);
            startupUiMock.Setup(ui => ui.UserWantsFallback())
                .Callback(() => this.fallbackRequested = true);
            startupUiMock.Setup(ui => ui.KnowsUserPassword(It.IsAny<bool>()))
                .Callback(() => this.passwordRequested = true)
                .Returns(this.authenticationPrompt);
            return startupUiMock;
        }
    }
}
