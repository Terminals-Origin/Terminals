using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Security;

namespace Terminals.Updates
{
    /// <summary>
    /// Updates all passwords stored using PasswordFunctions by passwords encrypted by PasswordFunctionsV2
    /// </summary>
    internal class PasswordsV2Update
    {
        private readonly bool isAuthenticated;

        private readonly string oldKey;

        private readonly string newKey;

        /// <summary>
        /// Hold in memory to be able resolve master password key salt and update the stored master password
        /// as last step in the update procedure.
        /// </summary>
        private readonly string storedMasterPassword;

        internal PasswordsV2Update(Func<bool, AuthenticationPrompt> knowsUserPassword)
        {
            AuthenticationPrompt authenticationResult = knowsUserPassword(false);
            string masterPassword = authenticationResult.Password;
            isAuthenticated = PasswordFunctions.MasterPasswordIsValid(masterPassword, Settings.MasterPasswordHash);
            if (!isAuthenticated)
            {
                Logging.Log.Debug("Application was not able to upgrade your passwords to version 2, because your master password isn't valid.");
                return;
            }

            this.oldKey = PasswordFunctions.CalculateMasterPasswordKey(masterPassword);
            this.storedMasterPassword = PasswordFunctions2.ComputeStoredMasterPasswordKey(masterPassword);
            this.newKey = PasswordFunctions2.CalculateMasterPasswordKey(masterPassword, this.storedMasterPassword);
        }

        internal string UpdateCredentialPasswords(string credentialsFileContent)
        {
            if (!isAuthenticated)
                return credentialsFileContent;

            XDocument credentialsDocument = XDocument.Parse(credentialsFileContent);
            IEnumerable<XElement> credentials = credentialsDocument.Root.Descendants("CredentialSet");
            foreach (XElement credential in credentials)
            {
                this.MigratePasswordElement(credential, "Password");
                this.MigrateNotEncryptedElement(credential, "Username");
                this.MigrateNotEncryptedElement(credential, "Domain");
            }
            return credentialsDocument.ToString();
        }

        private void MigratePasswordElement(XElement element, string elementName)
        {
            XElement passwordElement = element.Element(elementName);
            passwordElement.Value = this.MigratePassword(passwordElement.Value);
        }

        private void MigrateNotEncryptedElement(XElement element, string elementName)
        {
            XElement plainTextElement = element.Element(elementName);
            plainTextElement.Value = PasswordFunctions2.EncryptPassword(plainTextElement.Value, this.newKey);
        }

        internal void UpdateConfigFilePasswords(string fullConfigFileName)
        {
            if (!isAuthenticated)
                return;

            var configFile = XDocument.Load(fullConfigFileName);
            this.UpdateFavorites(configFile);
            this.UpdateSettingsPasswords(configFile);
            configFile.Save(fullConfigFileName);
        }

        private void UpdateFavorites(XDocument configFile)
        {
            List<XElement> favorites = FindAllFavoriteElements(configFile);
            foreach (XElement favorite in favorites)
            {
                this.UpgradeFavorite(favorite);
            }
        }

        private static List<XElement> FindAllFavoriteElements(XDocument configFile)
        {
            return configFile.Root.Descendants("favorites").Descendants("add").ToList();
        }

        private void UpgradeFavorite(XElement favorite)
        {
            this.MigrateNotEncryptedAttribute(favorite, "userName");
            this.MigrateNotEncryptedAttribute(favorite, "domainName");
            this.MigrateNotEncryptedAttribute(favorite, "tsgwUsername");
            this.MigrateNotEncryptedAttribute(favorite, "tsgwDomain");
            this.MigratePasswordAttribute(favorite, "encryptedPassword");
            this.MigratePasswordAttribute(favorite, "tsgwPassword");
        }

        private void UpdateSettingsPasswords(XDocument configFile)
        {
            var settingsElement = configFile.Root.Element("settings");
            this.MigratePasswordAttribute(settingsElement, "encryptedDefaultPassword");
            this.MigratePasswordAttribute(settingsElement, "encryptedAmazonAccessKey");
            this.MigratePasswordAttribute(settingsElement, "encryptedAmazonSecretKey");
            this.MigrateNotEncryptedAttribute(settingsElement, "defaultDomain");
            this.MigrateNotEncryptedAttribute(settingsElement, "defaultUsername");
            
            XAttribute masterPassword = settingsElement.Attribute("terminalsPassword");
            masterPassword.Value = this.storedMasterPassword;
        }

        private void MigratePasswordAttribute(XElement element, string attributeName)
        {
            XAttribute passwordAttribute = element.Attribute(attributeName);
            passwordAttribute.Value = this.MigratePassword(passwordAttribute.Value);
        }

        private string MigratePassword(string oldPassword)
        {
            string securityPassword = PasswordFunctions.DecryptPassword(oldPassword, this.oldKey);
            return PasswordFunctions2.EncryptPassword(securityPassword, this.newKey);
        }

        private void MigrateNotEncryptedAttribute(XElement element, string attributeName)
        {
            XAttribute plainTextAttribute = element.Attribute(attributeName);
            plainTextAttribute.Value = PasswordFunctions2.EncryptPassword(plainTextAttribute.Value, this.newKey);
        }
    }
}
