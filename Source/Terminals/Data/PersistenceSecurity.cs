using System;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data
{
    /// <summary>
    /// Provides persistence authentication and manipulations with master password
    /// </summary>
    internal class PersistenceSecurity
    {
        private IPersistedSecurity persistence;

        internal string KeyMaterial { get; private set; }

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
            String hashToCheck = PasswordFunctions.ComputeMasterPasswordHash(passwordToCheck);
            if (Settings.MasterPasswordHash == hashToCheck)
            {
                UpdateKeyMaterial(passwordToCheck);
                return true;
            }

            return false;
        }

        private void UpdateKeyMaterial(String password)
        {
            KeyMaterial = PasswordFunctions.CalculateMasterPasswordKey(password);
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
    }
}
