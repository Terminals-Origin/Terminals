using Terminals.Common.Connections;

namespace Terminals.Plugins.Putty
{
    internal class SshOptionsConverter : OptionsConverterTemplate<SshOptions>, IOptionsConverter
    {

        protected override void FromConfigFavorite(FavoriteConfigurationElement source, SshOptions options)
        {
            options.SessionName = source.SshSessionName;
            options.Verbose = source.SshVerbose;
            options.EnablePagentAuthentication = source.SshEnablePagentAuthentication;
            options.EnablePagentForwarding = source.SshEnablePagentForwarding;
            options.X11Forwarding = source.SshX11Forwarding;
            options.EnableCompression = source.SshEnableCompression;
            options.SshVersion = (SshVersion) source.SshVersion;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, SshOptions options)
        {
            destination.SshSessionName = options.SessionName;
            destination.SshVerbose = options.Verbose;
            destination.SshEnablePagentAuthentication = options.EnablePagentAuthentication;
            destination.SshEnablePagentForwarding = options.EnablePagentForwarding;
            destination.SshX11Forwarding = options.X11Forwarding;
            destination.SshEnableCompression = options.EnableCompression;
            destination.SshVersion = (byte) options.SshVersion;
        }
    }
}
