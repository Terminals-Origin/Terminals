using Terminals.Common.Connections;
using Terminals.Common.Converters;

namespace Terminals.Plugins.WinBox
{
    internal class WinBoxOptionsConverter : IOptionsConverter
    {
        public void FromConfigFavorite(OptionsConversionContext context)
        {
            UrlConverter.UpdateFavoriteUrl(context.Favorite, context.ConfigFavorite.Url);
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            context.ConfigFavorite.Url = UrlConverter.ExtractAbsoluteUrl(context.Favorite);
        }
    }
}
