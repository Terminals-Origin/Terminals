using System;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Sql implementation of user credentials directly used on favorites.
    /// Remember, that this type isn't used inside protocol options.
    /// We don't use table per type mapping here, because the credential base doesn't have to be defined,
    /// if user has selected StoredCredential. CredentialBase is implemented with lazy loading, 
    /// e.g. this item has in database its CredentialBase only 
    /// if some of its values to assigned has not null or empty value
    /// </summary>
    internal partial class DbSecurityOptions : ISecurityOptions
    {
        private PersistenceSecurity persistenceSecurity;

        private StoredCredentials storedCredentials;

        /// <summary>
        /// reference to the associated credentials by its ID.
        /// Is resolved by request from storedCredentials field.
        /// </summary>
        private int? credentialId;

        /// <summary>
        /// Distinguish between newly created CachedCredentials or loaded
        /// </summary>
        internal bool NewCachedCredentials { get; private set; }

        /// <summary>
        /// Cached base properties. Loaded from reference or assigned by creation only.
        /// </summary>
        internal DbCredentialBase CachedCredentials { get; private set; }

        public string EncryptedUserName
        {
            get
            {
                if (this.CachedCredentials != null)
                    return this.CachedCredentials.EncryptedUserName;

                return null;
            }
            set
            {
                if (this.CanCommitSecuredValue(value))
                {
                    this.EnsureCredentialBase();
                    this.CachedCredentials.EncryptedUserName = value;
                }
            }
        }

        string ICredentialBase.UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EncryptedUserName))
                    return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedUserName);

                return String.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedUserName = String.Empty;
                else
                    this.EncryptedUserName = this.persistenceSecurity.EncryptPersistencePassword(value);
            }
        }

        public string EncryptedDomain
        {
            get
            {
                if (this.CachedCredentials != null)
                    return this.CachedCredentials.EncryptedDomain;

                return null;
            }
            set
            {
                if (this.CanCommitSecuredValue(value))
                {
                    this.EnsureCredentialBase();
                    this.CachedCredentials.EncryptedDomain = value;
                }
            }
        }

        string ICredentialBase.Domain
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EncryptedDomain))
                    return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedDomain);

                return String.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedDomain = String.Empty;
                else
                    this.EncryptedDomain = this.persistenceSecurity.EncryptPersistencePassword(value);
            }
        }

        public string EncryptedPassword
        {
            get
            {
                if (this.CachedCredentials != null)
                    return this.CachedCredentials.EncryptedPassword;

                return null;
            }
            set
            {
                if (this.CanCommitSecuredValue(value))
                {
                    this.EnsureCredentialBase();
                    this.CachedCredentials.EncryptedPassword = value;
                }
            }
        }

        string ICredentialBase.Password
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EncryptedPassword))
                    return this.persistenceSecurity.DecryptPersistencePassword(this.EncryptedPassword);

                return String.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.EncryptedPassword = String.Empty;
                else
                    this.EncryptedPassword = this.persistenceSecurity.EncryptPersistencePassword(value);
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

        private bool CanCommitSecuredValue(string newValue)
        {
            // don't force store of empty value, if Security is not present in database
            // but allow clear, if security was already used
            return !string.IsNullOrEmpty(newValue) ||
                   (string.IsNullOrEmpty(newValue) && this.CachedCredentials != null);
        }

        internal void AssignStores(StoredCredentials storedCredentials, PersistenceSecurity persistenceSecurity)
        {
            this.storedCredentials = storedCredentials;
            this.persistenceSecurity = persistenceSecurity;
        }

        private Guid GetCredential()
        {
            DbCredentialSet resolved = this.ResolveCredentailFromStore();
            if (resolved != null)
                return resolved.Guid;

            return Guid.Empty;
        }

        internal DbCredentialSet ResolveCredentailFromStore()
        {
            if (this.credentialId != null)
                return this.storedCredentials[this.credentialId.Value];
            
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
            DbCredentialSet credentialToAssign = this.storedCredentials[value];
            if (credentialToAssign != null)
            {
                this.credentialId = credentialToAssign.Id;
            }
        }

        /// <summary>
        /// LazyLoading of CredentialBase for favorites, where security wasn't touched until now.
        /// The credential base doesn't have to be initialized, if used doesn't configure its properties.
        /// </summary>
        private void EnsureCredentialBase()
        {
            if (this.CachedCredentials == null)
            {
                this.CachedCredentials = new DbCredentialBase();
                this.CachedCredentials.AssignSecurity(this.persistenceSecurity);
                this.CredentialBase = this.CachedCredentials;
                this.NewCachedCredentials = true;
            }
        }

        public ISecurityOptions GetResolvedCredentials()
        {
            DbSecurityOptions result = this.Copy();
            SecurityOptions.ResolveCredentials(result, this.Credential);
            return result;
        }

        public void UpdateFromCredential(ICredentialSet source)
        {
            SecurityOptions.UpdateFromCredential(source, this);
        } 
        
        public void UpdatePasswordByNewKeyMaterial(string newKeymaterial)
        {
            if (this.CachedCredentials != null)
                this.CachedCredentials.UpdatePasswordByNewKeyMaterial(newKeymaterial);
        }

        internal void LoadReferences(Database database)
        {
            var securityEntry = database.Entry(this);
            securityEntry.Reference(so => so.CredentialBase).Load();
            securityEntry.Reference(so => so.CredentialSet).Load();
        }

        internal void LoadFieldsFromReferences()
        {
            this.CachedCredentials = this.CredentialBase;
            if (this.CachedCredentials != null)
                this.CachedCredentials.AssignSecurity(this.persistenceSecurity);
            this.LoadFromCredentialSetReference();
        }

        private void LoadFromCredentialSetReference()
        {
            if (this.CredentialSet != null)
                this.credentialId = this.CredentialSet.Id;
            else
                this.credentialId = null;
        }

        internal void UpdateCredentialSetReference()
        {
            if (this.credentialId != null)
                this.CredentialSet = this.storedCredentials[this.credentialId.Value];
            else
                this.CredentialSet = null;
        }

        internal DbSecurityOptions Copy()
        {
            var copy = new DbSecurityOptions();
            // newCachedCredentials doesn't have to be assigned here, because it depends on the values copied
            copy.UpdateFrom(this);
            return copy;
        }

        internal void UpdateFrom(DbSecurityOptions source)
        {
            this.EncryptedUserName = source.EncryptedUserName;
            this.EncryptedDomain = source.EncryptedDomain;
            this.EncryptedPassword = source.EncryptedPassword;
            this.credentialId = source.credentialId;
            this.storedCredentials = source.storedCredentials;
            this.persistenceSecurity = source.persistenceSecurity;
        }

        public override string ToString()
        {
            if (this.CachedCredentials == null)
                return "SecurityOptions:Empty";
            var security = this as ICredentialBase;
            return string.Format("SecurityOptions:User='{0}',Domain='{1}'", security.UserName, security.Domain);
        }

        /// <summary>
        /// Because of CachedCredentialBase lazy loading, we have to mark the property as initially saved.
        /// </summary>
        internal void Save()
        {
            this.NewCachedCredentials = false;
        }
    }
}
