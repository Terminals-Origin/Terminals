using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Security;
using Terminals.Updates;

namespace Tests.Passwords
{
    /// <summary>
    /// Tests to ensure that passwords after upgrade of v2.0 work
    /// and upgrade doesn't break any stored passwords
    /// </summary>
    [TestClass]
    public class PasswordUpgradesTests
    {
        private const string TESTDATA_DIRECTORY = @"..\Resources\TestData\";
        private const string EMPTY_CONFIG_FILE = "EmptyTerminals.config";
        private const string EMPTY_CREDENTIALS_FILE = "EmptyCredentials.xml";
        private const string NOMASTER_CONFIG_FILE = "NoMasterTerminals.config";
        private const string NOMASTER_CREDENTIALS_FILE = "NoMasterCredentials.xml";
        private const string SECURED_CONFIG_FILE = "SecuredTerminals.config";
        private const string SECURED_CREDENTIALS_FILE = "SecuredCredentials.xml";

        private readonly Settings settings = Settings.Instance;
        /// <summary>
        /// User name and password encrypted in test credential file
        /// </summary>
        private const string TEST_PASSWORD = "TestUser";
        
        public TestContext TestContext { get; set; }

        private bool askedForPassword;

        [TestCategory("NonSql")]
        [DeploymentItem(TESTDATA_DIRECTORY + EMPTY_CONFIG_FILE)]
        [DeploymentItem(TESTDATA_DIRECTORY + EMPTY_CREDENTIALS_FILE)]
        [TestMethod]
        public void V2UpgradeEmptyConfigTest()
        {
            this.UpgradePasswordsTestInitialize(EMPTY_CONFIG_FILE, EMPTY_CREDENTIALS_FILE);
            // simply nothing to upgrade, procedure shouldn't fail.
            this.RunUpgrade();
            Assert.IsFalse(askedForPassword, "Empty config file shouldn't ask for password");
        }

        [TestCategory("NonSql")]
        [DeploymentItem(TESTDATA_DIRECTORY + NOMASTER_CONFIG_FILE)]
        [DeploymentItem(TESTDATA_DIRECTORY + NOMASTER_CREDENTIALS_FILE)]
        [TestMethod]
        public void V2UpgradeNoMasterPasswordConfigTest()
        {
            this.UpgradePasswordsTestInitialize(NOMASTER_CONFIG_FILE, NOMASTER_CREDENTIALS_FILE);
            // simply nothing to upgrade, procedure shouldn't fail.
            IPersistence persistence = this.RunUpgrade();
            Assert.IsFalse(askedForPassword, "Config file shouldn't ask for password");
            bool masterStillValid = PasswordFunctions2.MasterPasswordIsValid(string.Empty, settings.MasterPasswordHash);
            Assert.IsTrue(masterStillValid, "Master password upgrade failed.");
            AssertUserAndCredential(persistence);
        }

        [TestCategory("NonSql")]
        [DeploymentItem(TESTDATA_DIRECTORY + SECURED_CONFIG_FILE)]
        [DeploymentItem(TESTDATA_DIRECTORY + SECURED_CREDENTIALS_FILE)]
        [TestMethod]
        public void V2UpgradePasswordsTest()
        {
            this.UpgradePasswordsTestInitialize(SECURED_CONFIG_FILE, SECURED_CREDENTIALS_FILE, "favoritesSecured.xml");
            IPersistence persistence = this.RunUpgrade();

            bool masterStillValid = PasswordFunctions2.MasterPasswordIsValid(PasswordTests.MASTERPASSWORD, settings.MasterPasswordHash);
            Assert.IsTrue(masterStillValid, "Master password upgrade failed.");
            AssertUserAndCredential(persistence);
            AssertFavoriteCredentialSet(persistence);
        }

        private static void AssertUserAndCredential(IPersistence persistence)
        {
            // we don't have to authenticate, because it was already done by upgrade
            IFavorite favorite = persistence.Favorites.First();
            var guardedSecurity = new GuardedCredential(favorite.Security, persistence.Security);
            Assert.AreEqual(PasswordTests.USERPASSWORD, guardedSecurity.Password, "Upgrade favorite password failed.");

            ICredentialSet credential = persistence.Credentials.First();
            var guarded = new GuardedCredential(credential, persistence.Security);
            Assert.AreEqual(TEST_PASSWORD, guarded.UserName, "Credential user name upgrade failed.");
            Assert.AreEqual(TEST_PASSWORD, guarded.Password, "Credential password upgrade failed.");
        }

        private static void AssertFavoriteCredentialSet(IPersistence persistence)
        {
            IFavorite favoriteWithCredentials = persistence.Favorites.ToList()[1];
            var credentialsId = favoriteWithCredentials.Security.Credential;
            ICredentialSet credential = persistence.Credentials[credentialsId];
            Assert.IsNotNull(credential, "Favorite referenced credential was lost");
        }

        private IPersistence RunUpgrade()
        {
            var persistence = new FilePersistence();
            var contentUpgrade = new FilesV2ContentUpgrade(persistence, GetMasterPassword);
            contentUpgrade.Run();
            settings.ForceReload(); // because we changed its file, while upgrading
            return persistence;
        }

        private AuthenticationPrompt GetMasterPassword(bool retry)
        {
            Assert.IsFalse(askedForPassword, "Upgrade asks for password second time");
            askedForPassword = true;
            // simulate user prompt for master password
            return new AuthenticationPrompt { Password = PasswordTests.MASTERPASSWORD };
        }

        /// <summary>
        /// because of concurrently running tests, we have to isolate all the files,
        /// otherwise multiple test are writing to the same file and tests fail (specially favorites file).
        /// </summary>
        private void UpgradePasswordsTestInitialize(string configFile, string credentialsFile,
            string favoritesFile = FileLocations.FAVORITES_FILENAME)
        {
            string configFileName = this.CreateFullTestFileName(configFile);
            string favoritesFileName = this.CreateFullTestFileName(favoritesFile);
            string credentialsFileName = this.CreateFullTestFileName(credentialsFile);
            // remove source control read only attribute
            File.SetAttributes(configFileName, FileAttributes.Normal);
            File.SetAttributes(credentialsFileName, FileAttributes.Normal);

            // we have to force all values to test deployment directory,
            // because upgrade works with fully configured files structure
            settings.FileLocations.AssignCustomFileLocations(configFileName, favoritesFileName, credentialsFileName);
            // when running multiple tests, there is may be already old configuration
            settings.ForceReload();
        }

        private string CreateFullTestFileName(string fileName)
        {
            string deplymentDir = this.TestContext.DeploymentDirectory;
            return Path.Combine(deplymentDir, fileName);
        }
    }
}
