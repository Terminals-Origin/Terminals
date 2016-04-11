using Terminals.Data;

namespace Terminals.Common.Connections
{
    public class OptionsConversionContext
    {
        public IFavorite Favorite { get; private set; }

        public FavoriteConfigurationElement ConfigFavorite { get; private set; }

        public IGuardedCredentialFactory CredentialsFactory { get; private set; }

        public OptionsConversionContext(IGuardedCredentialFactory credentialsFactory,
            IFavorite favorite, FavoriteConfigurationElement configFavorite)
        {
            this.CredentialsFactory = credentialsFactory;
            this.Favorite = favorite;
            this.ConfigFavorite = configFavorite;
        }
    }
}