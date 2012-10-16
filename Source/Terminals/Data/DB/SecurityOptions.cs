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
        internal StoredCredentials StoredCredentials { get; set; }

        private int? credentialId;

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
                return this.GetCredential();
            }
            set
            {
                this.SetCredential(value);
            }
        }

        private Guid GetCredential()
        {
            CredentialSet resolved = this.ResolveCredentailFromStore();
            if (resolved != null)
                return resolved.Guid;

            return Guid.Empty;
        }

        private CredentialSet ResolveCredentailFromStore()
        {
            if (this.credentialId != null)
                return this.StoredCredentials[this.credentialId.Value];
            
            return null;
        }

        private void SetCredential(Guid value)
        {
            if (value != Guid.Empty)
                this.SetCredentialByStoreId(value);
            else
                this.credentialId = null;
        }

        private void SetCredentialByStoreId(Guid value)
        {
            CredentialSet credentialToAssign = this.StoredCredentials[value];
            if (credentialToAssign != null)
            {
                this.credentialId = credentialToAssign.Id;
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
            this.CredentialSetReference.Load();
        }

        internal void LoadFieldsFromReferences()
        {
            this.credentialBase = this.CredentialBase;
            this.LoadFromCredentialSetReference();
        }

        private void LoadFromCredentialSetReference()
        {
            if (this.CredentialSet != null)
                this.credentialId = this.CredentialSet.Id;
            else
                this.credentialId = null;
        }

        internal void MarkAsModified(Database database)
        {
            database.MarkAsModified(this);
            if (this.credentialBase != null)
                database.MarkAsModified(this.credentialBase);

            this.UpdateCredentialSetReference();
        }

        private void UpdateCredentialSetReference()
        {
            if (this.credentialId != null)
                this.CredentialSet = this.StoredCredentials[this.credentialId.Value];
            else
                this.CredentialSet = null;
        }

        internal SecurityOptions Copy()
        {
            var copy = new SecurityOptions
                           {
                               Domain = this.Domain,
                               EncryptedPassword = this.EncryptedPassword,
                               UserName = this.UserName,
                               credentialId = this.credentialId,
                               StoredCredentials = this.StoredCredentials
                           };
            
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
