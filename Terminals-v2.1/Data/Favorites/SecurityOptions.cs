using System;
using Terminals.Configuration;

namespace Terminals.Data
{
    [Serializable]
    public class SecurityOptions : CredentialBase, ISecurityOptions
    {
        private Guid credential = Guid.Empty;
        /// <summary>
        /// Gets or sets the credential unique identifier in text form.
        /// Only for serialization to prevent serialization of empty ids.
        /// </summary>
        public string Credential
        {
            get
            {
                if (this.credential == Guid.Empty)
                    return null;

                return this.credential.ToString();
            }
            set
            {
                this.credential = new Guid(value);
            }
        }

        Guid ISecurityOptions.Credential
        {
            get { return this.credential; }
            set { this.credential = value; }
        }

        public SecurityOptions()
        {
        }

        ISecurityOptions ISecurityOptions.Copy()
        {
            return Copy();
        }

        internal SecurityOptions Copy()
        {
            return new SecurityOptions
            {
                Credential = this.Credential,
                Domain = this.Domain,
                EncryptedPassword = this.EncryptedPassword,
                UserName = this.UserName
            };
        }

        /// <summary>
        /// Gets this credentails replaced first by Stored credential and then by default
        /// stored credentials for each value, if the value is empty
        /// </summary>
        ISecurityOptions ISecurityOptions.GetResolvedCredentials()
        {
            var result = new SecurityOptions();
            var source = Persistance.Instance.Credentials[this.Credential];
            ((ISecurityOptions)result).UpdateFromCredential(source);
            result.UpdateFromDefaultValues();
            return result;
        }

        void ISecurityOptions.UpdateFromCredential(ICredentialSet source)
        {
            if (source != null)
            {
                this.credential = source.Id;
                this.Domain = source.Domain;
                this.UserName = source.UserName;
                this.EncryptedPassword = source.EncryptedPassword;
            }
        }

        private void UpdateFromDefaultValues()
        {
            if (string.IsNullOrEmpty(this.Domain))
                this.Domain = Settings.DefaultDomain;

            if (string.IsNullOrEmpty(this.UserName))
                this.UserName = Settings.DefaultUsername;

            if (string.IsNullOrEmpty(this.Password))
                this.Password = Settings.DefaultPassword;
        }
    }
}
