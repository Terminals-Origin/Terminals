using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.DB;
using Tests.Connections;
using Tests.FilePersisted;

namespace Tests
{
    [TestClass]
    public class PersistenceFactoryTests
    {
        // no password -> Initialize -> START
        // *password prompt ->     password valid ->  Persistence initialize -> START 
        //        (cancel) -> EXIT      (invalid) ->  *         (SQL failed) -> User wants fallbact from ->       Initialize -> START
        //                                                    (Files failed) -> EXIT                (NO) -> EXIT    (FAILED) -> EXIT

        private const string EXPECTED_PASSWORD = "ExpectedPassword";
        private bool passwordRequested;
        private bool exitCalled;
        private bool fallbackRequested;
        private PersistenceFactory factory;
        private AuthenticationPrompt authenticationPrompt;
        private Settings settings;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
            this.settings = Settings.Instance;
            this.settings.PersistenceType = FilePersistence.TYPE_ID;
            this.passwordRequested = false;
            this.exitCalled = false;
            this.fallbackRequested = false;
            this.authenticationPrompt = new AuthenticationPrompt();
            this.factory = new PersistenceFactory(Settings.Instance, TestConnectionManager.Instance, TestConnectionManager.CreateTestFavoriteIcons());
        }

        [TestMethod]
        public void FilePersistenceNoMasterPassword_Authenticate_CallsNoCallback()
        {
            this.Authenticate(string.Empty);
            Assert.IsTrue(!this.exitCalled && !this.passwordRequested && !this.fallbackRequested);
        }

        [TestMethod]
        public void SqlPersistenceNoMasterPassword_Authenticate_CallsNoCallback()
        {
            this.settings.PersistenceType = SqlPersistence.TYPE_ID;
            SqlPersisted.TestsLab.AssignDeploymentDirConnectionString(this.settings, this.TestContext.DeploymentDirectory);
            // TODO authentication fails, because test connection fails, because masterpassword has isnt saved in the DB.
            this.Authenticate(string.Empty);
            Assert.IsTrue(!this.exitCalled && !this.passwordRequested && !this.fallbackRequested);
        }

        [TestMethod]
        public void CancelPasswordPrompt_Authenticate_CallsExit()
        {
            this.authenticationPrompt.Canceled = true;
            this.Authenticate();
            Assert.IsTrue(this.exitCalled && this.passwordRequested && !this.fallbackRequested);
        }

        [TestMethod]
        public void FilePersistenceMasterPassword_Authenticate_AsksForMasterPassword()
        {
            this.authenticationPrompt.Password = EXPECTED_PASSWORD;
            this.Authenticate();
            Assert.IsTrue(!this.exitCalled && this.passwordRequested && !this.fallbackRequested);
        }

        private void Authenticate(string expectedMasterPassword = EXPECTED_PASSWORD)
        {
            this.SetupMasterPassword(expectedMasterPassword);
            Mock<IStartupUi> startupUiMock = this.SetupStartupUi();
            IPersistence persistence = this.factory.CreatePersistence();
            this.factory.AuthenticateByMasterPassword(persistence, startupUiMock.Object);
        }

        private void SetupMasterPassword(string expectedMasterPassword)
        {
            IPersistence persistence = this.factory.CreatePersistence();
            persistence.Security.UpdateMasterPassword(expectedMasterPassword);
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
