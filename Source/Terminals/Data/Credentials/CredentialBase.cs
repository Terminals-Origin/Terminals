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
                    return PersistenceSecurity.DecryptPersistencePassword(this.EncryptedPassword);

                return String.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedPassword = String.Empty;
                else
                    this.EncryptedPassword = PersistenceSecurity.EncryptPersistencePassword(value);
            }
        }

        private PersistenceSecurity PersistenceSecurity
        {
            get { return Persistence.Instance.Security; }
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