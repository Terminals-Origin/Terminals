using System;
using Terminals.Security;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Sql implementation of user credentials directly used on favorites.
    /// Remember, that this type isnt used inside protocol options.
    /// We dont use table per type mapping here, because the credential base doesnt have to be defined,
    /// if user has selected StoredCredential. CredentialBase is implemented with lazy loading, 
    /// eg. this item has in database its CredentialBase only 
    /// if some of its values to assigned has not null or empty value
    /// </summary>
    internal partial class SecurityOptions : ISecurityOptions
    {
        public string UserName
        {
            get
            {
                if (this.CredentialBase == null)
                    return null;

                return this.CredentialBase.UserName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.EnsureCredentialBase();
                    this.CredentialBase.UserName = value;
                }
            }
        }

        public string Domain
        {
            get
            {
                if (this.CredentialBase == null)
                    return null;

                return this.CredentialBase.Domain;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.EnsureCredentialBase();
                    this.CredentialBase.Domain = value;
                }
            }
        }

        public string EncryptedPassword
        {
            get
            {
                if (this.CredentialBase == null)
                    return null;

                return this.CredentialBase.EncryptedPassword;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.EnsureCredentialBase();
                    this.CredentialBase.EncryptedPassword = value;
                }
            }
        }

        string ICredentialBase.Password
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EncryptedPassword))
                    return PasswordFunctions.DecryptPassword(this.EncryptedPassword);

                return String.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedPassword = String.Empty;
                else
                    this.EncryptedPassword = PasswordFunctions.EncryptPassword(value);
            }
        }

        public Guid Credential
        {
            get
            {
                if (this.CredentialSet != null)
                    return ((ICredentialSet)this.CredentialSet).Id;

                return Guid.Empty;
            }
            set
            {
                if (value != Guid.Empty)
                {
                    var credentialToAssign = Persistence.Instance.Credentials[value] as CredentialSet;
                    if (credentialToAssign != null)
                        this.CredentialSet = credentialToAssign;
                }
                else
                {
                    this.CredentialSet = null;
                }
            }
        }

        /// <summary>
        /// LazyLoading of CredentialBase for favorites, where security isnt touched
        /// </summary>
        private void EnsureCredentialBase()
        {
            if (this.CredentialBase == null)
                this.CredentialBase = new CredentialBase();
        }

        internal SecurityOptions Copy()
        {
            return new SecurityOptions
            {
                CredentialSet = this.CredentialSet,
                Domain = this.Domain,
                EncryptedPassword = this.EncryptedPassword,
                UserName = this.UserName
            };
        }

        public ISecurityOptions GetResolvedCredentials()
        {
            SecurityOptions result = this.Copy();
            Data.SecurityOptions.ResolveCredentials(result, this.Credential);
            return result;
        }

        public void UpdateFromCredential(ICredentialSet source)
        {
            Data.SecurityOptions.UpdateFromCredential(source, this);
        } 
        
        public void UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            if (this.CredentialBase != null)
                this.CredentialBase.UpdatePasswordByNewKeyMaterial(newKeymaterial);
        }
    }
}
