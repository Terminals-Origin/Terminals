using System;
using System.Xml.Serialization;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data
{
    [Serializable]
    public class SecurityOptions
    {
        public String Credential { get; set; }
        public string DomainName { get; set; }
        public string UserName { get; set; }
        public String EncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets the password String in not ecrypted form
        /// </summary>
        [XmlIgnore]
        internal String Password
        {
            get
            {
                return PasswordFunctions.DecryptPassword(EncryptedPassword);
            }
            set
            {
                EncryptedPassword = PasswordFunctions.EncryptPassword(value);
            }
        }

        internal SecurityOptions Copy()
        {
            return new SecurityOptions
                {
                    Credential = this.Credential,
                    DomainName = this.DomainName,
                    Password = this.Password,
                    UserName = this.UserName
                };
        }

        /// <summary>
        /// Gets this credentails replaced first by Stored credential and then by default
        /// stored credentials for each value, if the value is empty
        /// </summary>
        internal SecurityOptions GetResolvedCredentials()
        {
            var result = new SecurityOptions();
            ResolveFromCredential(result);
            ResolveDefaultValues(result);
            return result;
        }

        private void ResolveFromCredential(SecurityOptions result)
        {
            CredentialSet storedCredential = Persistance.Instance.Credentials.GetByName(this.Credential);
            if (storedCredential !=  null)
            {
                result.DomainName = storedCredential.Domain;
                result.UserName = storedCredential.Username;
                result.EncryptedPassword = storedCredential.SecretKey;
            }
        }

        private static void ResolveDefaultValues(SecurityOptions result)
        {
            if (string.IsNullOrEmpty(result.DomainName))
                result.DomainName = Settings.DefaultDomain;

            if (string.IsNullOrEmpty(result.UserName))
                result.UserName = Settings.DefaultUsername;

            if (string.IsNullOrEmpty(result.Password))
                result.Password = Settings.DefaultPassword;
        }
    }
}
