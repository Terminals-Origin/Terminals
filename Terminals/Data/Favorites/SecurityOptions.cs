using System;
using System.Xml.Serialization;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data
{
    [Serializable]
    public class SecurityOptions
    {
        private string domainName;
        public String DomainName
        {
            get
            {
                CredentialSet cred = StoredCredentials.Instance.GetByName(Credential);
                if (cred != null)
                    return cred.Domain;

                return domainName;
            }
            set
            {
                domainName = value;
            }
        }

        private string userName;
        public String UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(Credential))
                {
                    CredentialSet cred = StoredCredentials.Instance.GetByName(Credential);
                    if (cred != null)
                        return cred.Username;
                }

                return userName;
            }
            set
            {
                userName = value;
            }
        }

        public String EncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the password String in not ecrypted form
        /// </summary>
        [XmlIgnore]
        internal String Password
        {
            get
            {
                CredentialSet cred = StoredCredentials.Instance.GetByName(Credential);
                if (cred != null)
                    return cred.SecretKey;

                return PasswordFunctions.DecryptPassword(EncryptedPassword);
            }
            set
            {
                EncryptedPassword = PasswordFunctions.EncryptPassword(value);
            }
        }

        public String Credential { get; set; }
    }
}
