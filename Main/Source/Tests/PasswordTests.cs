using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Security;
using Terminals.Updates;

namespace Tests
{
    /// <summary>
    /// Tests to ensure that passwords in version after v2.0 work
    /// and upgrade doesn't break any stored passwords
    /// </summary>
    [TestClass]
    public class PasswordTests
    {
        private const string USERPASSWORD = "testPassword";
        private const string MASTERPASSWORD = "testMaster";

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void V1MasterPasswordValidationTest()
        {
            // cant use persistence security here, because of initialization depends on newest algorithm
            string masterHash = PasswordFunctions.ComputeMasterPasswordHash(MASTERPASSWORD);
            string masterHash2 = PasswordFunctions.ComputeMasterPasswordHash(MASTERPASSWORD);
            Assert.AreEqual(masterHash, masterHash2);

            bool isValid = PasswordFunctions.MasterPasswordIsValid(MASTERPASSWORD, masterHash);
            Assert.IsTrue(isValid, "Couldn't validate stored master password v1");
        }

        [TestMethod]
        public void V1MasterPasswordIsUniqueTest()
        {
            string key1 = PasswordFunctions.CalculateMasterPasswordKey(MASTERPASSWORD);
            string key2 = PasswordFunctions.CalculateMasterPasswordKey(MASTERPASSWORD);
            Assert.AreEqual(key1, key2, "generated master password key v1 doesn't equals.");
        }

        [TestMethod]
        public void V1PasswordsUniqueEncryptionTest()
        {
            string key = PasswordFunctions.CalculateMasterPasswordKey(MASTERPASSWORD);
            string encryptedPassword = PasswordFunctions.EncryptPassword(USERPASSWORD, key);
            string encryptedPassword2 = PasswordFunctions.EncryptPassword(USERPASSWORD, key);
            Assert.AreEqual(encryptedPassword, encryptedPassword2, "password encryption v1 doesn't generate identical encrypted bytes");
        }

        [TestMethod]
        public void V1PasswordsEncryptDecryptTest()
        {
            PasswordsEncryptDecryptCheck(CheckEncryptDecryptPasswordV1);

            string masterKey = PasswordFunctions.CalculateMasterPasswordKey(MASTERPASSWORD);
            string decryptedPassword = PasswordFunctions.DecryptPassword(string.Empty, masterKey);
            Assert.AreEqual(string.Empty, decryptedPassword, "Decryption of empty stored password failed");
        }

        private static string CheckEncryptDecryptPasswordV1(string password, string masterPassword)
        {
            string masterKey = PasswordFunctions.CalculateMasterPasswordKey(masterPassword);
            string encryptedPassword = PasswordFunctions.EncryptPassword(password, masterKey);
            string decryptedPassword = PasswordFunctions.DecryptPassword(encryptedPassword, masterKey);
            Assert.AreEqual(password, decryptedPassword, "Unable to decrypt V1 password");
            return encryptedPassword;
        }

        [TestMethod]
        public void V2MasterPasswordValidationTest()
        {
            string stored = PasswordFunctions2.CalculateStoredMasterPasswordKey(MASTERPASSWORD);
            string stored2 = PasswordFunctions2.CalculateStoredMasterPasswordKey(MASTERPASSWORD);
            Assert.AreNotEqual(stored, stored2);

            bool isValid = PasswordFunctions2.MasterPasswordIsValid(MASTERPASSWORD, stored);
            Assert.IsTrue(isValid, "Unable to validate encrypted master password version 2");
        }

        [TestMethod]
        public void V2MasterPasswordIsUniqueTest()
        {
            string key1 = CalculateNewMasterPasswordKey(MASTERPASSWORD);
            string key2 = CalculateNewMasterPasswordKey(MASTERPASSWORD);
            Assert.AreNotEqual(key1, key2, "generated master password key always equals.");

            var accessor = new PrivateType(typeof(PasswordFunctions2));
            byte[] keySalt = (byte[])accessor.InvokeStatic("CreateRandomKeySalt", new Object[] {});
            key1 = CalculateV2Key(accessor, keySalt);
            key2 = CalculateV2Key(accessor, keySalt);
            Assert.AreEqual(key1, key2, "generated master password doesn't equal.");
        }

        private static string CalculateV2Key(PrivateType accessor, byte[] keySalt)
        {
            object[] parameters = new object[] {MASTERPASSWORD, keySalt};
            return accessor.InvokeStatic("CalculateMasterPasswordKey", parameters).ToString();
        }

        [TestMethod]
        public void V2PasswordsUniqueEncryptionTest()
        {
            string key = CalculateNewMasterPasswordKey(MASTERPASSWORD);
            string encryptedPassword = PasswordFunctions2.EncryptPassword(USERPASSWORD, key);
            string encryptedPassword2 = PasswordFunctions2.EncryptPassword(USERPASSWORD, key);
            Assert.AreNotEqual(encryptedPassword, encryptedPassword2, "password encryption always generates identical encrypted bytes");
        }

        [DeploymentItem(@"..\Resources\TestData\SecuredTerminals.config")]
        [DeploymentItem(@"..\Resources\TestData\SecuredCredentials.xml")]
        [TestMethod]
        public void V2UpgradePasswordsTest()
        {
            this.UpgradePasswordsTestInitialize();
            this.RunUpgrade();

            bool masterStillValid = PasswordFunctions2.MasterPasswordIsValid(MASTERPASSWORD, Settings.MasterPasswordHash);
            Assert.IsTrue(masterStillValid, "Master password upgrade failed.");

            // don't use the application SingleTon static instance, because other test fail, when running tests in batch
            IPersistence persistence = new FilePersistence();
            persistence.Security.Authenticate(GetMasterPassword);
            
            IFavorite favorite = persistence.Favorites.First();
            String favoritePassword = favorite.Security.Password;
            Assert.AreEqual(USERPASSWORD, favoritePassword, "Upgrade favorite password failed.");

            ICredentialSet credential = persistence.Credentials.First();
            Assert.AreEqual(credential.UserName, credential.Password, "Credential password upgrade failed.");
        }

        private void RunUpgrade()
        {
            // simulate last two method calls in UpdateConfig class
            var passwordsUpdate = new PasswordsV2Update(GetMasterPassword);
            var updateConfig = new PrivateType(typeof(UpdateConfig));
            updateConfig.InvokeStatic("UpgradeCredentialsFile", new object[] { passwordsUpdate });
            // because of needed method group conversion (method group is not convertible to object)
            object knowsUserPassword = (Func<bool, AuthenticationPrompt>)GetMasterPassword;
            updateConfig.InvokeStatic("UpgradeConfigFile", new object[] { passwordsUpdate, knowsUserPassword });
            Settings.ForceReload(); // because we changed its file, while upgrading
        }

        private AuthenticationPrompt GetMasterPassword(bool retry)
        {
            // simulate user prompt for master password
            return new AuthenticationPrompt {Password = MASTERPASSWORD};
        }

        private void UpgradePasswordsTestInitialize()
        {
            string configFileName = this.GetFullTestConfigFileName();
            string favoritesFileName = this.CreateFullTestFileName(FileLocations.FAVORITES_FILENAME);
            string credentialsFileName = this.GetFullTestCredentialsFileName();
            // remove source control read only attribute
            File.SetAttributes(configFileName, FileAttributes.Normal);
            File.SetAttributes(credentialsFileName, FileAttributes.Normal);

            // we have to force all values to test deployment directory,
            // because upgrade works with fully configured files structure
            Settings.FileLocations.AssignCustomFileLocations(configFileName, favoritesFileName, credentialsFileName);
            // when running multiple tests, there is may be already old configuration
            Settings.ForceReload();
        }

        private string GetFullTestConfigFileName()
        {
            return this.CreateFullTestFileName("SecuredTerminals.config");
        }

        private string GetFullTestCredentialsFileName()
        {
            return this.CreateFullTestFileName("SecuredCredentials.xml");
        }

        private string CreateFullTestFileName(string sourceConfigFile)
        {
            string deplymentDir = this.TestContext.DeploymentDirectory;
            return Path.Combine(deplymentDir, sourceConfigFile);
        }

        [TestMethod]
        public void V2PasswordsEncryptDecryptTest()
        {
            PasswordsEncryptDecryptCheck(CheckEncryptDecryptPasswordV2);

            string masterKey = CalculateNewMasterPasswordKey(MASTERPASSWORD);
            string decryptedPassword = PasswordFunctions2.DecryptPassword(string.Empty, masterKey);
            Assert.AreEqual(string.Empty, decryptedPassword, "Decryption of empty stored password failed");
        }

        private static string CheckEncryptDecryptPasswordV2(string password, string masterPassword)
        {
            string masterKey = CalculateNewMasterPasswordKey(masterPassword);
            string encryptedPassword = PasswordFunctions2.EncryptPassword(password, masterKey);
            string decryptedPassword = PasswordFunctions2.DecryptPassword(encryptedPassword, masterKey);
            Assert.AreEqual(password, decryptedPassword, "Unable to decrypt password");
            return encryptedPassword;
        }

        private static string CalculateNewMasterPasswordKey(string password)
        {
            string storedKey = PasswordFunctions2.CalculateStoredMasterPasswordKey(password);
            return PasswordFunctions2.CalculateMasterPasswordKey(password, storedKey);
        }

        private static void PasswordsEncryptDecryptCheck(Func<string, string, string> checkEncryptDecryptPassword)
        {
            checkEncryptDecryptPassword(USERPASSWORD, MASTERPASSWORD);
            string encryptedPassword = checkEncryptDecryptPassword(USERPASSWORD, string.Empty);
            Assert.AreNotEqual(string.Empty, encryptedPassword, "Encrypted password by empty master password isn't valid");
            encryptedPassword = checkEncryptDecryptPassword(string.Empty, MASTERPASSWORD);
            Assert.AreEqual(string.Empty, encryptedPassword, "Encrypted empty user password isn't valid");
        }
    }
}