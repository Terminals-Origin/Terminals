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
                if (string.IsNullOrEmpty(value))
                    this.credential = Guid.Empty;
                else
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

        public SecurityOptions Copy()
        {
            var copy = new SecurityOptions{ Credential = this.Credential };
            copy.UpdateFrom(this);
            return copy;
        }

        /// <summary>
        /// Gets this credentials replaced first by Stored credential and then by default
        /// stored credentials for each value, if the value is empty
        /// </summary>
        ISecurityOptions ISecurityOptions.GetResolvedCredentials()
        {
            SecurityOptions result = this.Copy();
            ResolveCredentials(result, this.credential);
            return result;
        }

        void ISecurityOptions.UpdateFromCredential(ICredentialSet source)
        {
            UpdateFromCredential(source, this);
        }

        public static void ResolveCredentials(ISecurityOptions result, Guid credentialId)
        {
            ICredentialSet source = null; //todo Persistence.Instance.Credentials[credentialId];
            result.UpdateFromCredential(source);
            UpdateFromDefaultValues(result);
        }

        public static void UpdateFromCredential(ICredentialSet source, ISecurityOptions target)
        {
            if (source != null)
            {
                target.Credential = source.Id;
                target.Domain = source.Domain;
                target.UserName = source.UserName;
                target.EncryptedPassword = source.EncryptedPassword;
            }
        }

        private static void UpdateFromDefaultValues(ICredentialBase target)
        {
            // todo Settings settings = Settings.Instance;

            //if (string.IsNullOrEmpty(target.Domain))
            //    target.Domain = settings.DefaultDomain;

            //if (string.IsNullOrEmpty(target.UserName))
            //    target.UserName = settings.DefaultUsername;

            //if (string.IsNullOrEmpty(target.Password))
            //    target.Password = settings.DefaultPassword;
        }

        public override string ToString()
        {
            return string.Format("SecurityOptions:User='{0}',Domain='{1}'", this.UserName, this.Domain);
        }
    }
}
