using Terminals.Common.Connections;

namespace Terminals.Plugins.Putty
{
    internal class PuttyOptionsConverter : OptionsConverterTemplate<PuttyOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, PuttyOptions options)
        {
            options.SessionName = source.PuttySessionName;
            options.X11Forwarding = source.PuttyX11Forwarding;
            options.EnableCompression = source.EnableCompression;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, PuttyOptions options)
        {
            destination.PuttySessionName = options.SessionName;
            destination.PuttyX11Forwarding = options.X11Forwarding;
            destination.PuttyEnableCompression = options.EnableCompression;
        }
    }
}
