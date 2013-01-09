using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Security;
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
        private bool isAuthenticated;

        private string oldKey;

        private string newKey;

        /// <summary>
        /// Hold in memory to be able resolve master password key salt and update the stored master password
        /// as last step in the update procedure.
        /// </summary>
        private string storedMasterPassword;
        
        internal PasswordsV2Update(Func<bool, AuthenticationPrompt> knowsUserPassword)
        {
            // we cant use PersistenceSecurity, because it already works on new PasswordFunctions2 methods
            var authentication = new AuthenticationSequence(this.IsMasterPasswordValid, knowsUserPassword);
            authentication.AuthenticateIfRequired();
            if (!this.isAuthenticated)
            {
                const string MESSAGE = "Application was not able to upgrade your passwords to version 2, because your master password isn't valid.";
                throw new SecurityAccessDeniedException(MESSAGE);
            }
        }

        private bool IsMasterPasswordValid(string masterPassword)
        {
            this.isAuthenticated = PasswordFunctions.MasterPasswordIsValid(masterPassword, Settings.MasterPasswordHash);
            if (this.isAuthenticated)
            {
                this.oldKey = PasswordFunctions.CalculateMasterPasswordKey(masterPassword);
                this.storedMasterPassword = PasswordFunctions2.CalculateStoredMasterPasswordKey(masterPassword);
                this.newKey = PasswordFunctions2.CalculateMasterPasswordKey(masterPassword, this.storedMasterPassword);
            }
            return this.isAuthenticated;
        }

        internal string UpdateCredentialPasswords(string credentialsFileContent)
        {
            if (!this.isAuthenticated)
                return credentialsFileContent;

            XDocument credentialsDocument = XDocument.Parse(credentialsFileContent);
            IEnumerable<XElement> credentials = credentialsDocument.Root.Descendants("CredentialSet");
            foreach (XElement credential in credentials)
            {
                this.MigratePasswordElement(credential, "EncryptedPassword");
                this.MigrateNotEncryptedElement(credential, "EncryptedUserName");
                this.MigrateNotEncryptedElement(credential, "EncryptedDomain");
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
            if (!this.isAuthenticated)
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
            // attributes userName, domainName, tsgwUsername and tsgwDomain cant be upgraded, 
            // because rest of the application understands them unencrypted
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
