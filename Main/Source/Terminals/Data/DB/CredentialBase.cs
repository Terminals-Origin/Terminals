using System;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal partial class CredentialBase : ICredentialBase
    {
        private PersistenceSecurity persistenceSecurity;

        public string UserName
        {
            get
            {
                return this.GetDecryptedUserName();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedUserName = String.Empty;
                else
                    this.EncryptedUserName = persistenceSecurity.EncryptPersistencePassword(value);
            }
        }

        public string Domain
        {
            get
            {
                return this.GetDecryptedDomain();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedDomain = String.Empty;
                else
                    this.EncryptedDomain = persistenceSecurity.EncryptPersistencePassword(value);
            }
        }

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

        public void UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            string secret = this.GetDecryptedPassword();
            if (!string.IsNullOrEmpty(secret))
                this.EncryptedPassword = PasswordFunctions2.EncryptPassword(secret, newKeymaterial);

            string userName = this.GetDecryptedUserName();
            if (!string.IsNullOrEmpty(userName))
                this.EncryptedPassword = PasswordFunctions2.EncryptPassword(userName, newKeymaterial);

            string domain = this.GetDecryptedDomain();
            if (!string.IsNullOrEmpty(domain))
                this.EncryptedPassword = PasswordFunctions2.EncryptPassword(domain, newKeymaterial);
        }

        private string GetDecryptedUserName()
        {
            if (!string.IsNullOrEmpty(this.EncryptedUserName))
                return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedUserName);

            return String.Empty;
        }

        private string GetDecryptedDomain()
        {
            if (!string.IsNullOrEmpty(this.EncryptedDomain))
                return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedDomain);

            return String.Empty;
        }

        private string GetDecryptedPassword()
        {
            if (!string.IsNullOrEmpty(this.EncryptedPassword))
                return persistenceSecurity.DecryptPersistencePassword(this.EncryptedPassword);

            return String.Empty;
        }

        protected void CopyTo(CredentialBase copy)
        {
            copy.EncryptedUserName = this.EncryptedUserName;
            copy.EncryptedDomain = this.EncryptedDomain;
            copy.EncryptedPassword = this.EncryptedPassword;
            copy.persistenceSecurity = this.persistenceSecurity;
        }

        internal void AssignSecurity(PersistenceSecurity persistenceSecurity)
        {
            this.persistenceSecurity = persistenceSecurity;
        }
    }
}
