using System;
using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Rdp
{
    internal class RdpOptionsConverter : IOptionsConverter
    {
        public void FromCofigFavorite(OptionsConversionContext context)
        {
            var protocolOptions = context.Favorite.ProtocolProperties as RdpOptions;
            if (protocolOptions != null)
            {
                this.ToTsgwOptions(protocolOptions.TsGateway, context.ConfigFavorite);
            }
        }

        private void ToTsgwOptions(TsGwOptions destination, FavoriteConfigurationElement source)
        {
            destination.CredentialSource = source.TsgwCredsSource;
            destination.HostName = source.TsgwHostname;
            destination.SeparateLogin = source.TsgwSeparateLogin;
            destination.UsageMethod = source.TsgwUsageMethod;

            // todo this.Security.Domain = favorite.TsgwDomain;
            destination.Security.EncryptedPassword = source.TsgwEncryptedPassword;
            // todo this.Security.UserName = favorite.TsgwUsername;
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            throw new NotImplementedException();
        }
    }
}