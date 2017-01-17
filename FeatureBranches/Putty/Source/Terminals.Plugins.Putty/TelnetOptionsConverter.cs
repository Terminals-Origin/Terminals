using Terminals.Common.Connections;

namespace Terminals.Plugins.Putty
{
    internal class TelnetOptionsConverter : OptionsConverterTemplate<TelnetOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, TelnetOptions options)
        {
            options.SessionName = source.PuttySessionName;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, TelnetOptions options)
        {
            destination.PuttySessionName = options.SessionName;
        }
    }
}
