using System;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal partial class CredentialBase : ICredentialBase
    {
        private PersistenceSecurity persistenceSecurity;

        string ICredentialBase.Password
        {
            get
            {
                return this.GetDecryptedPassword();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedPassword = String.Empty;
                else
                    this.EncryptedPassword = persistenceSecurity.EncryptPersistencePassword(value);
            }
        }

        private string GetDecryptedPassword()
        {
            if (!string.IsNullOrEmpty(this.EncryptedPassword))
                return persistenceSecurity.DecryptPersistencePassword(this.EncryptedPassword);

            return String.Empty;
        }

        public void UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            string secret = this.GetDecryptedPassword();
            if (!string.IsNullOrEmpty(secret))
                this.EncryptedPassword = PasswordFunctions.EncryptPassword(secret, newKeymaterial);
        }

        protected void CopyTo(CredentialBase copy)
        {
            copy.UserName = this.UserName;
            copy.Domain = this.Domain;
            copy.EncryptedPassword = this.EncryptedPassword;
            copy.persistenceSecurity = this.persistenceSecurity;
        }

        internal void AssignSecurity(PersistenceSecurity persistenceSecurity)
        {
            this.persistenceSecurity = persistenceSecurity;
        }
    }
}
