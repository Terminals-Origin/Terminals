using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Terminal
{
    internal class SshOptionsConverter : IOptionsConverter
    {
        public void FromCofigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as SshOptions;
            if (options != null)
            {
                FavoriteConfigurationElement source = context.ConfigFavorite;
                options.SSH1 = source.SSH1;
                options.AuthMethod = source.AuthMethod;
                options.CertificateKey = source.KeyTag;
                options.SSHKeyFile = source.SSHKeyFile;

                ConsoleOptionsConverter.FromConfigFavorite(options.Console, source);
            }
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as SshOptions;
            if (options != null)
            {
                FavoriteConfigurationElement destination = context.ConfigFavorite;
                destination.SSH1 = options.SSH1;
                destination.AuthMethod = options.AuthMethod;
                destination.KeyTag = options.CertificateKey;
                destination.SSHKeyFile = options.SSHKeyFile;

                ConsoleOptionsConverter.ToConfigFavorite(destination, options.Console);
            }
        }
    }
}