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
        private bool isAuthenticated;

        private IPersistedSecurity persistence;

        protected string KeyMaterial { get; private set; }

        protected virtual string PersistenceKeyMaterial { get { return this.KeyMaterial; }  }

        internal bool IsMasterPasswordDefined
        {
            get
            {
                return AuthenticationSequence.IsMasterPasswordDefined();
            }
        }

        internal PersistenceSecurity()
        {
            this.KeyMaterial = string.Empty;
        }

        /// <summary>
        /// Creates new instance and initializes internal state from sourceSecurity.
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
            // don't prompt user third time for password when upgrading passwords from v2
            if (this.isAuthenticated)
                return true;
            
            var authentication = new AuthenticationSequence(this.IsMasterPasswordValid, knowsUserPassword);
            this.isAuthenticated = authentication.AuthenticateIfRequired();
            if (this.isAuthenticated)
              this.persistence.Initialize();

            return this.isAuthenticated;
        }

        private Boolean IsMasterPasswordValid(string passwordToCheck)
        {
            string storedMasterPassword = Settings.MasterPasswordHash;
            bool isValid = PasswordFunctions2.MasterPasswordIsValid(passwordToCheck, storedMasterPassword);
            if (isValid)
            {
                this.KeyMaterial = PasswordFunctions2.CalculateMasterPasswordKey(passwordToCheck, storedMasterPassword);
                return true;
            }

            return false;
        }

        internal void UpdateMasterPassword(string newPassword)
        {
            string storedMasterPassword = PasswordFunctions2.CalculateStoredMasterPasswordKey(newPassword);
            string newMasterKey = PasswordFunctions2.CalculateMasterPasswordKey(newPassword, storedMasterPassword);

            // start of not secured transaction. Old key is still present,
            // but passwords are already encrypted by new Key
            this.persistence.UpdatePasswordsByNewMasterPassword(newMasterKey);
            Settings.UpdateConfigurationPasswords(newMasterKey, storedMasterPassword);
            // finish transaction, the passwords now reflect the new key
            this.KeyMaterial = newMasterKey;
        }

        /// <summary>
        /// Use only to resolve settings file passwords
        /// </summary>
        internal string DecryptPassword(string encryptedPassword)
        {
            return PasswordFunctions2.DecryptPassword(encryptedPassword, this.KeyMaterial);
        }

        /// <summary>
        /// Use only to store settings file passwords
        /// </summary>
        internal string EncryptPassword(string decryptedPassword)
        {
            return PasswordFunctions2.EncryptPassword(decryptedPassword, this.KeyMaterial);
        }

        /// <summary>
        /// Use only to resolve all other passwords except settings file passwords
        /// </summary>
        internal string DecryptPersistencePassword(string encryptedPassword)
        {
            return PasswordFunctions2.DecryptPassword(encryptedPassword, this.PersistenceKeyMaterial);
        }

        /// <summary>
        /// Use only to store all other passwords except settings file passwords
        /// </summary>
        internal string EncryptPersistencePassword(string decryptedPassword)
        {
            return PasswordFunctions2.EncryptPassword(decryptedPassword, this.PersistenceKeyMaterial);
        }
    }
}
