using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Vnc
{
    internal class VncOptionsConverter : IOptionsConverter
    {
        public void FromCofigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as VncOptions;
            if (options != null)
            {
                FavoriteConfigurationElement source = context.ConfigFavorite;
                options.AutoScale = source.VncAutoScale;
                options.DisplayNumber = source.VncDisplayNumber;
                options.ViewOnly = source.VncViewOnly;
            }
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as VncOptions;
            if (options != null)
            {
                FavoriteConfigurationElement destination = context.ConfigFavorite;
                destination.VncAutoScale = options.AutoScale;
                destination.VncDisplayNumber = options.DisplayNumber;
                destination.VncViewOnly = options.ViewOnly;
            }
        }
    }
}