using System;
using Terminals.Configuration;

namespace Terminals.Data
{
    [Serializable]
    public class SecurityOptions : CredentialBase, ISecurityOptions
    {
        public Guid Credential { get; set; }

        public SecurityOptions()
        {
            this.Credential = Guid.Empty;
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
                this.Credential = source.Id;
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
