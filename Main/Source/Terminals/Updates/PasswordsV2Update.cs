using System;
using System.Collections.Generic;
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
        private readonly bool isAuthenticated;

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
            this.isAuthenticated = authentication.AuthenticateIfRequired();
            if (!this.isAuthenticated)
            {
                const string MESSAGE = "Application was not able to upgrade your passwords to version 2, because your master password isn't valid.";
                throw new SecurityAccessDeniedException(MESSAGE);
            }

            this.CheckEmptyMasterPassword();
        }

        private void CheckEmptyMasterPassword()
        {
            // we have to do it manually, because if master password isn't defined, 
            // authentication sequence doesn't call IsMasterPasswordValid to assign the fields
            if (this.isAuthenticated && string.IsNullOrEmpty(this.oldKey))
            {
                this.AssignFieldsByOldMasterPassword(string.Empty);
            }
        }

        private bool IsMasterPasswordValid(string masterPassword)
        {
            bool isValid = PasswordFunctions.MasterPasswordIsValid(masterPassword, Settings.Instance.MasterPasswordHash);
            if (isValid)
            {
                this.AssignFieldsByOldMasterPassword(masterPassword);
            }
            return isValid;
        }

        private void AssignFieldsByOldMasterPassword(string masterPassword)
        {
            this.oldKey = PasswordFunctions.CalculateMasterPasswordKey(masterPassword);
            this.storedMasterPassword = PasswordFunctions2.CalculateStoredMasterPasswordKey(masterPassword);
            this.newKey = PasswordFunctions2.CalculateMasterPasswordKey(masterPassword, this.storedMasterPassword);
        }

        internal string UpdateCredentialPasswords(string credentialsFileContent)
        {
            if (!this.isAuthenticated)
                return credentialsFileContent;

            XDocument credentialsDocument = XDocument.Parse(credentialsFileContent);
            if (credentialsDocument.Root == null)
                return credentialsFileContent;

            IEnumerable<XElement> credentials = credentialsDocument.Root.Descendants("CredentialSet");
            this.UpgradeAllCredentials(credentials);
            return credentialsDocument.ToString();
        }

        private void UpgradeAllCredentials(IEnumerable<XElement> credentials)
        {
            foreach (XElement credential in credentials)
            {
                this.MigratePasswordElement(credential, "EncryptedPassword");
                this.MigrateNotEncryptedElement(credential, "EncryptedUserName");
                this.MigrateNotEncryptedElement(credential, "EncryptedDomain");
            }
        }

        private void MigratePasswordElement(XElement element, string elementName)
        {
            XElement passwordElement = element.Element(elementName);
            if (passwordElement == null)
                return;

            passwordElement.Value = this.MigratePassword(passwordElement.Value);
        }

        private void MigrateNotEncryptedElement(XElement element, string elementName)
        {
            XElement plainTextElement = element.Element(elementName);
            if (plainTextElement == null)
                return;

            plainTextElement.Value = PasswordFunctions2.EncryptPassword(plainTextElement.Value, this.newKey);
        }

        internal void UpdateConfigFilePasswords(string fullConfigFileName)
        {
            if (!this.isAuthenticated)
                return;

            var configFile = XDocument.Load(fullConfigFileName);
            if (configFile.Root == null)
                return;

            this.UpdateFavorites(configFile.Root);
            this.UpdateSettingsPasswords(configFile);
            configFile.Save(fullConfigFileName);
        }

        private void UpdateFavorites(XElement rootElement)
        {
            List<XElement> favorites = FindAllFavoriteElements(rootElement);
            foreach (XElement favorite in favorites)
            {
                this.UpgradeFavorite(favorite);
            }
        }

        private static List<XElement> FindAllFavoriteElements(XElement rootElement)
        {
            return rootElement.Descendants("favorites").Descendants("add").ToList();
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
            if (configFile.Root == null)
                return;

            var settingsElement = configFile.Root.Element("settings");
            if (settingsElement == null)
                return;

            this.UpdateSettingsElement(settingsElement);
            this.ConfirmMasterPassword(settingsElement);
        }

        private void UpdateSettingsElement(XElement settingsElement)
        {
            this.MigratePasswordAttribute(settingsElement, "encryptedDefaultPassword");
            this.MigratePasswordAttribute(settingsElement, "encryptedAmazonAccessKey");
            this.MigratePasswordAttribute(settingsElement, "encryptedAmazonSecretKey");
            this.MigrateNotEncryptedAttribute(settingsElement, "defaultDomain");
            this.MigrateNotEncryptedAttribute(settingsElement, "defaultUsername");
        }

        private void ConfirmMasterPassword(XElement settingsElement)
        {
            XAttribute masterPassword = settingsElement.Attribute("terminalsPassword");
            if (masterPassword == null)
                return;

            masterPassword.Value = this.storedMasterPassword;
        }

        private void MigratePasswordAttribute(XElement element, string attributeName)
        {
            XAttribute passwordAttribute = element.Attribute(attributeName);
            if (passwordAttribute == null)
                return;

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
            if (plainTextAttribute == null)
                return;

            plainTextAttribute.Value = PasswordFunctions2.EncryptPassword(plainTextAttribute.Value, this.newKey);
        }
    }
}
