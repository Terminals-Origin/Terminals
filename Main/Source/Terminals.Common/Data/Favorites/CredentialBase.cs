using System;
using System.Xml.Serialization;
//using Terminals.Security;

namespace Terminals.Data
{
    /// <summary>
    /// General properties, which represent user authentication values
    /// </summary>
    [Serializable]
    public class CredentialBase : ICredentialBase
    {
        public string EncryptedUserName { get; set; }

        public string EncryptedDomain { get; set; }

        [XmlIgnore]
        String ICredentialBase.Domain
        {
            get { return Domain; }
            set { Domain = value; }
        }
        
        [XmlIgnore]
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
                //else
                // todo    this.EncryptedDomain = persistenceSecurity.EncryptPersistencePassword(value);
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
        public String Password
        {
            get
            {
                return this.GetDecryptedPassword();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedPassword = String.Empty;
                //else
                // todo    this.EncryptedPassword = persistenceSecurity.EncryptPersistencePassword(value);
            }
        }

       // private PersistenceSecurity persistenceSecurity;

        protected void UpdateFrom(CredentialBase source)
        {
            this.EncryptedUserName = source.EncryptedUserName;
            this.EncryptedDomain = source.EncryptedDomain;
            this.EncryptedPassword = source.EncryptedPassword;
            //this.persistenceSecurity = source.persistenceSecurity;
        }

        // todo internal void AssignStore(PersistenceSecurity persistenceSecurity)
        //{
        //    this.persistenceSecurity = persistenceSecurity;
        //}

        private string GetDecryptedUserName()
        {
            // todo if (!string.IsNullOrEmpty(this.EncryptedUserName))
            //    return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedUserName);

            return String.Empty;
        }

        private string GetDecryptedDomain()
        {
            // todo if (!string.IsNullOrEmpty(this.EncryptedDomain))
            //    return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedDomain);

            return String.Empty;
        }

        private string GetDecryptedPassword()
        {
            // todo if (!string.IsNullOrEmpty(this.EncryptedPassword))
            //    return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedPassword);

            return String.Empty;
        }
    }
}