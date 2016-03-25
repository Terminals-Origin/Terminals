using System;
using System.Xml.Serialization;

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

        protected void UpdateFrom(CredentialBase source)
        {
            this.EncryptedUserName = source.EncryptedUserName;
            this.EncryptedDomain = source.EncryptedDomain;
            this.EncryptedPassword = source.EncryptedPassword;
        }

        private string GetDecryptedPassword()
        {
            // todo if (!string.IsNullOrEmpty(this.EncryptedPassword))
            //    return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedPassword);

            return String.Empty;
        }
    }
}