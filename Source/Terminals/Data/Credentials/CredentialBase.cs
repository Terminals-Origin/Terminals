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
        public string EncryptedUserName { get; set; }

        [XmlIgnore]
        String ICredentialBase.UserName
        {
            get { return UserName; }
            set { UserName = value; }
        }

        // Not everything is access using interface, e.g. TSGW options inside RdpOptions,
        // so we encapsulate the internal properties UserName, Domain and Password
        [XmlIgnore]
        internal string UserName
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

        public string EncryptedDomain { get; set; }

        [XmlIgnore]
        String ICredentialBase.Domain
        {
            get { return Domain; }
            set { Domain = value; }
        }
        
        [XmlIgnore]
        internal string Domain
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
        
        public string EncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the password String in not encrypted form
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

        private PersistenceSecurity persistenceSecurity;

        protected void UpdateFrom(CredentialBase source)
        {
            this.EncryptedUserName = source.EncryptedUserName;
            this.EncryptedDomain = source.EncryptedDomain;
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
            string userName = this.GetDecryptedUserName();
            if (!string.IsNullOrEmpty(userName))
                this.EncryptedUserName = PasswordFunctions2.EncryptPassword(userName, newKeymaterial);

            string domain = this.GetDecryptedDomain();
            if (!string.IsNullOrEmpty(domain))
                this.EncryptedDomain = PasswordFunctions2.EncryptPassword(domain, newKeymaterial);

            string secret = this.GetDecryptedPassword();
            if (!string.IsNullOrEmpty(secret))
                this.EncryptedPassword = PasswordFunctions2.EncryptPassword(secret, newKeymaterial);
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
                return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedPassword);

            return String.Empty;
        }
    }
}