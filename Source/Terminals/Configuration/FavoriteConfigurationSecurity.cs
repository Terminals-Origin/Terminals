using Terminals.Data;

namespace Terminals.Configuration
{
    internal class FavoriteConfigurationSecurity
    {
        private readonly ICredentials credentials;

        private readonly FavoriteConfigurationElement favorite;

        internal FavoriteConfigurationSecurity(ICredentials credentials, FavoriteConfigurationElement favorite)
        {
            this.credentials = credentials;
            this.favorite = favorite;
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
