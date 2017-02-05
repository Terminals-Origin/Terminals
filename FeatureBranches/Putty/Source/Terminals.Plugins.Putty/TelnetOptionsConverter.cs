using Terminals.Common.Connections;

namespace Terminals.Plugins.Putty
{
    internal class TelnetOptionsConverter : OptionsConverterTemplate<TelnetOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, TelnetOptions options)
        {
            options.SessionName = source.TelnetSessionName;
            options.Verbose = source.TelnetVerbose;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, TelnetOptions options)
        {
            destination.TelnetSessionName = options.SessionName;
            destination.TelnetVerbose = options.Verbose;
        }
    }
}
