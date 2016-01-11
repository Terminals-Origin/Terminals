using Terminals.Data;

namespace Terminals.Configuration
{
    internal class FavoriteConfigurationSecurity
    {
        private readonly ICredentials credentials;

        internal FavoriteConfigurationSecurity(ICredentials credentials)
        {
            this.credentials = credentials;
        }

        internal string ResolveDomainName(FavoriteConfigurationElement favorite)
        {
            ICredentialSet cred = this.credentials[favorite.Credential];
            if (cred != null)
                return cred.Domain;

            return favorite.DomainName;
        }
    }
}
