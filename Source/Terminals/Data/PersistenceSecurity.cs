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
        private readonly IPersistedSecurity persistence;

        internal string KeyMaterial { get; private set; }

        internal bool IsMasterPasswordDefined
        {
            get
            {
                return !String.IsNullOrEmpty(this.persistence.MasterPasswordHash);
            }
        }

        internal PersistenceSecurity(IPersistedSecurity persistence)
        {
            this.persistence = persistence;
            this.KeyMaterial = string.Empty;
        }

        internal Boolean IsMasterPasswordValid(string passwordToCheck)
        {
            String hashToCheck = PasswordFunctions.ComputeMasterPasswordHash(passwordToCheck);
            if (this.persistence.MasterPasswordHash == hashToCheck)
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
