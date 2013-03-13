using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Data;
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
        public TestContext TestContext { get; set; }

        [DeploymentItem(@"..\Resources\TestData\EmptyTerminals.config")]
        [DeploymentItem(@"..\Resources\TestData\EmptyCredentials.xml")]
        [TestMethod]
        public void V2UpgradeEmptyConfigTest()
        {
            this.UpgradePasswordsTestInitialize("EmptyTerminals.config", "EmptyCredentials.xml");
            // simply nothing to upgrade, procedure shouldn't fail.
            this.RunUpgrade(new FilePersistence());
        }

        [DeploymentItem(@"..\Resources\TestData\SecuredTerminals.config")]
        [DeploymentItem(@"..\Resources\TestData\SecuredCredentials.xml")]
        [TestMethod]
        public void V2UpgradePasswordsTest()
        {
            this.UpgradePasswordsTestInitialize("SecuredTerminals.config", "SecuredCredentials.xml");
            IPersistence persistence = new FilePersistence();
            this.RunUpgrade(persistence);

            bool masterStillValid = PasswordFunctions2.MasterPasswordIsValid(PasswordTests.MASTERPASSWORD, Settings.MasterPasswordHash);
            Assert.IsTrue(masterStillValid, "Master password upgrade failed.");

            // don't use the application SingleTon static instance, because other test fail, when running tests in batch
            persistence.Security.Authenticate(GetMasterPassword);

            IFavorite favorite = persistence.Favorites.First();
            String favoritePassword = favorite.Security.Password;
            Assert.AreEqual(PasswordTests.USERPASSWORD, favoritePassword, "Upgrade favorite password failed.");

            ICredentialSet credential = persistence.Credentials.First();
            Assert.AreEqual(credential.UserName, credential.Password, "Credential password upgrade failed.");
        }

        private void RunUpgrade(IPersistence persistence)
        {
            var contentUpgrade = new FilesV2ContentUpgrade(persistence, GetMasterPassword);
            contentUpgrade.Run();
            Settings.ForceReload(); // because we changed its file, while upgrading
        }

        private AuthenticationPrompt GetMasterPassword(bool retry)
        {
            // simulate user prompt for master password
            return new AuthenticationPrompt { Password = PasswordTests.MASTERPASSWORD };
        }

        private void UpgradePasswordsTestInitialize(string configFile, string credentialsFile)
        {
            string configFileName = this.CreateFullTestFileName(configFile);
            string favoritesFileName = this.CreateFullTestFileName(FileLocations.FAVORITES_FILENAME);
            string credentialsFileName = this.CreateFullTestFileName(credentialsFile);
            // remove source control read only attribute
            File.SetAttributes(configFileName, FileAttributes.Normal);
            File.SetAttributes(credentialsFileName, FileAttributes.Normal);

            // we have to force all values to test deployment directory,
            // because upgrade works with fully configured files structure
            Settings.FileLocations.AssignCustomFileLocations(configFileName, favoritesFileName, credentialsFileName);
            // when running multiple tests, there is may be already old configuration
            Settings.ForceReload();
        }

        private string CreateFullTestFileName(string sourceConfigFile)
        {
            string deplymentDir = this.TestContext.DeploymentDirectory;
            return Path.Combine(deplymentDir, sourceConfigFile);
        }
    }
}
