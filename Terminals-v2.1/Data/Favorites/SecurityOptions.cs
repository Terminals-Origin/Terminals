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
            var source = Persistance.Instance.Credentials[this.credential];
            ((ISecurityOptions)result).UpdateFromCredential(source);
            UpdateFromDefaultValues(result);
            return result;
        }

        void ISecurityOptions.UpdateFromCredential(ICredentialSet source)
        {
            UpdateFromCredential(source, this);
        }

        internal static void UpdateFromCredential(ICredentialSet source, ISecurityOptions target)
        {
            if (source != null)
            {
                target.Credential = source.Id;
                target.Domain = source.Domain;
                target.UserName = source.UserName;
                target.EncryptedPassword = source.EncryptedPassword;
            }
        }

        internal static void UpdateFromDefaultValues(ICredentialBase target)
        {
            if (string.IsNullOrEmpty(target.Domain))
                target.Domain = Settings.DefaultDomain;

            if (string.IsNullOrEmpty(target.UserName))
                target.UserName = Settings.DefaultUsername;

            if (string.IsNullOrEmpty(target.Password))
                target.Password = Settings.DefaultPassword;
        }
    }
}
