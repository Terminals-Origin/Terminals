using System;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data
{
    /// <summary>
    /// Provides persistence authentication and manipulations with master password.
    /// Doesn't distinguish between application and persistence master password.
    /// </summary>
    internal class PersistenceSecurity
    {
        private IPersistedSecurity persistence;

        protected string KeyMaterial { get; private set; }

        protected virtual string PersistenceKeyMaterial { get { return this.KeyMaterial; }  }

        internal bool IsMasterPasswordDefined
        {
            get
            {
                return !String.IsNullOrEmpty(Settings.MasterPasswordHash);
            }
        }

        internal PersistenceSecurity()
        {
            this.KeyMaterial = string.Empty;
        }

        /// <summary>
        /// Creates new instance initializating internal state from sourceSecurity.
        /// </summary>
        /// <param name="sourceSecurity">Not null current security instance to initialize from.</param>
        internal PersistenceSecurity(PersistenceSecurity sourceSecurity)
        {
            this.KeyMaterial = sourceSecurity.KeyMaterial;
        }

        internal void AssignPersistence(IPersistedSecurity persistence)
        {
            this.persistence = persistence;
        }

        internal bool Authenticate(Func<bool, AuthenticationPrompt> knowsUserPassword)
        {
            bool authenticated = this.AuthenticateIfRequired(knowsUserPassword);
            if (authenticated)
              this.persistence.Initialize();

            return authenticated;
        }

        private bool AuthenticateIfRequired(Func<bool, AuthenticationPrompt> knowsUserPassword)
        {
            if (this.IsMasterPasswordDefined)
                return this.PromptUserToAuthenticate(knowsUserPassword);

            return true;
        }

        private bool PromptUserToAuthenticate(Func<bool, AuthenticationPrompt> knowsUserPassword)
        {
            AuthenticationPrompt promptResults = knowsUserPassword(false);
            bool authenticated = IsUserPaswordValid(promptResults);

            while (!(promptResults.Canceled || authenticated))
            {
                promptResults = knowsUserPassword(true);
                authenticated = this.IsUserPaswordValid(promptResults);
            }
            return authenticated;
        }

        private bool IsUserPaswordValid(AuthenticationPrompt promptResults)
        {
            if (!promptResults.Canceled)
                return this.IsMasterPasswordValid(promptResults.Password);

            return false;
        }

        private Boolean IsMasterPasswordValid(string passwordToCheck)
        {
            bool isValid = PasswordFunctions.MasterPasswordIsValid(passwordToCheck, Settings.MasterPasswordHash);
            if (isValid)
            {
                UpdateKeyMaterial(passwordToCheck);
                return true;
            }

            return false;
        }

        private void UpdateKeyMaterial(String password)
        {
            this.KeyMaterial = PasswordFunctions.CalculateMasterPasswordKey(password);
        }

        internal void UpdateMasterPassword(string newPassword)
        {
            this.persistence.UpdatePasswordsByNewMasterPassword(newPassword);

            // start of not secured transaction. Old key is still present,
            // but passwords are already encrypted by newKey
            Settings.UpdateConfigurationPasswords(newPassword);

            // finish transaction, the passwords now reflect the new key
            UpdateKeyMaterial(newPassword);
        }

        internal string DecryptPassword(string encryptedPassword)
        {
            return PasswordFunctions.DecryptPassword(encryptedPassword, this.KeyMaterial);
        }

        internal string EncryptPassword(string decryptedPassword)
        {
            return PasswordFunctions.EncryptPassword(decryptedPassword, this.KeyMaterial);
        }

        internal string DecryptPersistencePassword(string encryptedPassword)
        {
            return PasswordFunctions.DecryptPassword(encryptedPassword, this.PersistenceKeyMaterial);
        }

        internal string EncryptPersistencePassword(string decryptedPassword)
        {
            return PasswordFunctions.EncryptPassword(decryptedPassword, this.PersistenceKeyMaterial);
        }
    }
}
