using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Vmrc
{
    internal class VmrcOptionsConverter : IOptionsConverter
    {
        public void FromCofigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as VMRCOptions;
            if (options != null)
            {
                FavoriteConfigurationElement source = context.ConfigFavorite;
                options.AdministratorMode = source.VMRCAdministratorMode;
                options.ReducedColorsMode = source.VMRCReducedColorsMode;
            }
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as VMRCOptions;
            if (options != null)
            {
                FavoriteConfigurationElement destination = context.ConfigFavorite;
                destination.VMRCAdministratorMode = options.AdministratorMode;
                destination.VMRCReducedColorsMode = options.ReducedColorsMode;
            }
        }
    }
}