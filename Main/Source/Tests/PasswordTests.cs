using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Security;

namespace Tests
{
    /// <summary>
    /// Tests to ensure that passwords in version after v2.0 work
    /// and upgrade doesn't break any stored passwords
    /// </summary>
    [TestClass]
    public class PasswordV3Tests
    {
        private const string TEST_PASSWORD = "testPassword";
        
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void MasterPasswordKeyV2StillWorksTest()
        {
            // cant use persistence security here, because of initialization depends on newest algorithm
            string key1 = PasswordFunctions.CalculateMasterPasswordKey(TEST_PASSWORD);
            string key2 = PasswordFunctions.CalculateMasterPasswordKey(TEST_PASSWORD);
            Assert.AreEqual(key1, key2, "generated master password key v2 doesn't equals.");

            string encryptedPassword = PasswordFunctions.EncryptPassword(TEST_PASSWORD, key1);
            string encryptedPassword2 = PasswordFunctions.EncryptPassword(TEST_PASSWORD, key1);
            Assert.AreEqual(encryptedPassword, encryptedPassword2, "password encryption doesn't generate identical encrypted bytes");
            
            string resolvedPassowrd = PasswordFunctions.DecryptPassword(encryptedPassword, key1);
            Assert.AreEqual(TEST_PASSWORD, resolvedPassowrd, "Unable to restore password");
        }

        [TestMethod]
        public void MasterPasswordKeyV3IsUniqueTest()
        {
            string key1 = PasswordFunctions2.CalculateMasterPasswordKey(TEST_PASSWORD);
            string key2 = PasswordFunctions2.CalculateMasterPasswordKey(TEST_PASSWORD);
            Assert.AreNotEqual(key1, key2, "generated master password key always equals.");
        }

        [TestMethod]
        public void MasterPasswordSecretV3ValidationTest()
        {
            string stored = PasswordFunctions2.ComputeMasterPasswordHash(TEST_PASSWORD);
            string stored2 = PasswordFunctions2.ComputeMasterPasswordHash(TEST_PASSWORD);
            Assert.AreNotEqual(stored, stored2);

            string key1 = PasswordFunctions2.CalculateMasterPasswordKey(TEST_PASSWORD);
            string key2 = PasswordFunctions2.CalculateMasterPasswordKey(TEST_PASSWORD);
            Assert.AreNotEqual(key1, key2, "generated master password key v2 doesn't equals.");

            string encryptedPassword = PasswordFunctions2.EncryptPassword(TEST_PASSWORD, key1);
            string encryptedPassword2 = PasswordFunctions2.EncryptPassword(TEST_PASSWORD, key1);
            Assert.AreNotEqual(encryptedPassword, encryptedPassword2, "password encryption always generates identical encrypted bytes");

            string resolvedPassowrd = PasswordFunctions2.DecryptPassword(encryptedPassword, key1);
            Assert.AreEqual(TEST_PASSWORD, resolvedPassowrd, "Unable to restore password");

            bool isValid = PasswordFunctions2.MasterPasswordIsValid(TEST_PASSWORD, stored);
            Assert.IsTrue(isValid, "Unable to validate encrypted master password after version 2");
            // known shared password was stored properly as stored master password
            // we are able to decrypt it properly
            // the same stored password always generates unique encrypted bytes
        }

        [TestMethod]
        public void UpgradeV2PasswordsTest()
        {
            throw new NotImplementedException();
            // deploy test version 2.0 config file
            // check, if are able to read all passwords
            // use upgrade routine to ensure, that we are able to:
            // - validate the master password
            // - all stored passwords in favorite, credentials, RDP protocol properties are still valid
        }
    }
}