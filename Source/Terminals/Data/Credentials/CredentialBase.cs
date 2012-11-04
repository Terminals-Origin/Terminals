using System;
using System.Xml.Serialization;
using Terminals.Security;

namespace Terminals.Data
{
    /// <summary>
    /// General properties, which represent user authentication values
    /// </summary>
    [Serializable]
    public class CredentialBase : ICredentialBase
    {
        public string UserName { get; set; }
        public string Domain { get; set; }
        public string EncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the password String in not ecrypted form
        /// </summary>
        [XmlIgnore]
        String ICredentialBase.Password
        {
            get { return Password; }
            set { Password = value; }
        }

        [XmlIgnore]
        internal String Password
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EncryptedPassword))
                    return persistenceSecurity.DecryptPersistencePassword(this.EncryptedPassword);

                return String.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedPassword = String.Empty;
                else
                    this.EncryptedPassword = persistenceSecurity.EncryptPersistencePassword(value);
            }
        }

        private PersistenceSecurity persistenceSecurity;

        protected void UpdateFrom(CredentialBase source)
        {
            this.UserName = source.UserName;
            this.Domain = source.Domain;
            this.EncryptedPassword = source.EncryptedPassword;
            this.persistenceSecurity = source.persistenceSecurity;
        }

        internal void AssignStore(PersistenceSecurity persistenceSecurity)
        {
            this.persistenceSecurity = persistenceSecurity;
        }

        void ICredentialBase.UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            UpdatePasswordByNewKeyMaterial(newKeymaterial);
        }

        internal void UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            string secret = ((ICredentialBase)this).Password;
            if (!string.IsNullOrEmpty(secret))
                this.EncryptedPassword = PasswordFunctions.EncryptPassword(secret, newKeymaterial);            
        }
    }
}