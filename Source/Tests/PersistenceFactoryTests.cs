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
        // *passowrd prompt ->     password valid ->  Persistence initialize -> START 
        //        (cancel) -> EXIT      (invalid) ->  *         (SQL failed) -> User wants fallbact from ->       Initialize -> START
        //                                                    (Files failed) -> EXIT                (NO) -> EXIT    (FAILED) -> EXIT
        private bool passwordRequested;
        private bool exitCalled;
        private bool fallbackRequested;

        private AuthenticationPrompt authenticationPrompt;

        [TestInitialize]
        public void TestInitialize()
        {
            this.passwordRequested = false;
            this.exitCalled = false;
            this.fallbackRequested = false;
            this.authenticationPrompt = new AuthenticationPrompt();
        }

        [TestMethod]
        public void FilePersistenceNoMasterPassword_Authenticate_CallNoCallback()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
            Mock<IStartupUi> startupUiMock = this.SetupStartupUi();
            var factory = new PersistenceFactory(Settings.Instance, TestConnectionManager.Instance, TestConnectionManager.CreateTestFavoriteIcons());
            IPersistence persistence = factory.CreatePersistence();

            factory.AuthenticateByMasterPassword(persistence, startupUiMock.Object);

            Assert.IsFalse(this.exitCalled || this.passwordRequested || this.fallbackRequested);
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
