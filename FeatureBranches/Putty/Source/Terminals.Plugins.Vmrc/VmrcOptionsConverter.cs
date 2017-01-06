using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Vmrc
{
    internal class VmrcOptionsConverter : OptionsConverterTemplate<VMRCOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, VMRCOptions options)
        {
            options.AdministratorMode = source.VMRCAdministratorMode;
            options.ReducedColorsMode = source.VMRCReducedColorsMode;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, VMRCOptions options)
        {
            destination.VMRCAdministratorMode = options.AdministratorMode;
            destination.VMRCReducedColorsMode = options.ReducedColorsMode;
        }
    }
}