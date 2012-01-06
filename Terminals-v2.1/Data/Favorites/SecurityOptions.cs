using System;
using System.Xml.Serialization;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data
{
    [Serializable]
    public class SecurityOptions
    {
        public Guid Credential { get; set; }
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

        public SecurityOptions()
        {
            this.Credential = Guid.Empty;
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
            var source = Persistance.Instance.Credentials[this.Credential];
            result.UpdateFromCredential(source);
            result.UpdateFromDefaultValues();
            return result;
        }

        internal void UpdateFromCredential(ICredentialSet source)
        {
            if (source != null)
            {
                this.Credential = source.Id;
                this.DomainName = source.Domain;
                this.UserName = source.Username;
                this.EncryptedPassword = source.SecretKey;
            }
        }

        private void UpdateFromDefaultValues()
        {
            if (string.IsNullOrEmpty(this.DomainName))
                this.DomainName = Settings.DefaultDomain;
            else
                this.DomainName = this.DomainName;

            if (string.IsNullOrEmpty(this.UserName))
                this.UserName = Settings.DefaultUsername;
            else
                this.UserName = this.UserName;

            if (string.IsNullOrEmpty(this.Password))
                this.Password = Settings.DefaultPassword;
            else
                this.Password = this.Password;
        }
    }
}
