using System;

namespace Terminals.Data
{
    [Serializable]
    public class VMRCOptions : ProtocolOptions
    {
        public Boolean ReducedColorsMode { get; set; }
        public Boolean AdministratorMode { get; set; }

        public override ProtocolOptions Copy()
        {
            return new VMRCOptions
                {
                    ReducedColorsMode = this.ReducedColorsMode,
                    AdministratorMode = this.AdministratorMode
                };
        }

        public override void FromCofigFavorite(IFavorite destination, FavoriteConfigurationElement source)
        {
            this.AdministratorMode = source.VMRCAdministratorMode;
            this.ReducedColorsMode = source.VMRCReducedColorsMode;
        }

        public override void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination)
        {
            destination.VMRCAdministratorMode = this.AdministratorMode;
            destination.VMRCReducedColorsMode = this.ReducedColorsMode;
        }
    }
}
