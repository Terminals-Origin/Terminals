using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Terminal
{
    internal class SshOptionsConverter : OptionsConverterTemplate<SshOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, SshOptions options)
        {
            options.SSH1 = source.SSH1;
            options.AuthMethod = source.AuthMethod;
            options.CertificateKey = source.KeyTag;
            options.SSHKeyFile = source.SSHKeyFile;

            ConsoleOptionsConverter.FromConfigFavorite(options.Console, source);
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, SshOptions options)
        {
            destination.SSH1 = options.SSH1;
            destination.AuthMethod = options.AuthMethod;
            destination.KeyTag = options.CertificateKey;
            destination.SSHKeyFile = options.SSHKeyFile;

            ConsoleOptionsConverter.ToConfigFavorite(options.Console, destination);
        }
    }
}