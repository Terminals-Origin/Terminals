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
        private CredentialBase credentialBase;

        public string UserName
        {
            get
            {
                if (this.credentialBase != null)
                   return this.credentialBase.UserName;

                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.EnsureCredentialBase();
                    this.credentialBase.UserName = value;
                }
            }
        }

        public string Domain
        {
            get
            {
                if (this.credentialBase != null)
                    return this.credentialBase.Domain;

                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.EnsureCredentialBase();
                    this.credentialBase.Domain = value;
                }
            }
        }

        public string EncryptedPassword
        {
            get
            {
                if (this.credentialBase != null)
                    return this.credentialBase.EncryptedPassword;

                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.EnsureCredentialBase();
                    this.credentialBase.EncryptedPassword = value;
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
                // todo check the credential resolution from SQL persistence in getter and setter
                if (this.CredentialSet != null)
                    return this.CredentialSet.Guid;

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
        /// LazyLoading of CredentialBase for favorites, where security wasnt touched until now.
        /// The credential base doesnt have to be initialized, if used doent configure its properties.
        /// </summary>
        private void EnsureCredentialBase()
        {
            if (this.credentialBase == null)
            {
                this.credentialBase = new CredentialBase();
                this.CredentialBase = this.credentialBase;
            }
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
            if (this.credentialBase != null)
                this.credentialBase.UpdatePasswordByNewKeyMaterial(newKeymaterial);
        }

        internal void Attach(Database database)
        {
            database.Attach(this);
            if (this.credentialBase != null)
                database.Attach(this.credentialBase);
        }

        internal void Detach(Database database)
        {
            database.Detach(this);
            // check the reference, not local cached field
            if (this.CredentialBase != null)
                database.Detach(this.credentialBase);
        }

        internal void LoadReferences()
        {
            this.CredentialBaseReference.Load();
        }

        internal void LoadFieldsFromReferences()
        {
            this.credentialBase = this.CredentialBase;
        }

        internal void MarkAsModified(Database database)
        {
            database.MarkAsModified(this);
            if (this.credentialBase != null)
                database.MarkAsModified(this.credentialBase);
        }

        internal SecurityOptions Copy()
        {
            var copy = new SecurityOptions
                           {
                               Domain = this.Domain,
                               EncryptedPassword = this.EncryptedPassword,
                               UserName = this.UserName
                           };

            if (this.CredentialSet != null)
                copy.CredentialSet = this.CredentialSet.Copy();
            
            return copy;
        }

        public override string ToString()
        {
            if (this.credentialBase == null)
                return "SecurityOptions:Empty";
            return string.Format("SecurityOptions:User='{0}',Domain='{1}'", this.UserName, this.Domain);
        }
    }
}
