using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Vnc
{
    internal class VncOptionsConverter : OptionsConverterTemplate<VncOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, VncOptions options)
        {
            options.AutoScale = source.VncAutoScale;
            options.DisplayNumber = source.VncDisplayNumber;
            options.ViewOnly = source.VncViewOnly;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, VncOptions options)
        {
            destination.VncAutoScale = options.AutoScale;
            destination.VncDisplayNumber = options.DisplayNumber;
            destination.VncViewOnly = options.ViewOnly;
        }
    }
}