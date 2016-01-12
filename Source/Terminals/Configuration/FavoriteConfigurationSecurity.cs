using System;
using Terminals.Data;

namespace Terminals.Configuration
{
    internal class FavoriteConfigurationSecurity
    {
        private readonly PersistenceSecurity security;
        private readonly ICredentials credentials;
        private readonly FavoriteConfigurationElement favorite;

        internal FavoriteConfigurationSecurity(IPersistence persistence, FavoriteConfigurationElement favorite)
        {
            this.security = persistence.Security;
            this.credentials = persistence.Credentials;
            this.favorite = favorite;
        }

        internal String TsgwPassword
        {
            get
            {
                return this.security.DecryptPassword(this.favorite.TsgwEncryptedPassword);
            }
            set
            {
                this.favorite.TsgwEncryptedPassword = this.security.EncryptPassword(value);
            }
        }

        internal string ResolveDomainName()
        {
            ICredentialSet cred = this.credentials[this.favorite.Credential];
            if (cred != null)
                return cred.Domain;

            return this.favorite.DomainName;
        }

        public string ResolveUserName()
        {
            if (!string.IsNullOrEmpty(this.favorite.Credential))
            {
                ICredentialSet cred = this.credentials[this.favorite.Credential];
                if (cred != null)
                    return cred.UserName;
            }

            return this.favorite.UserName;
        }
    }
}
