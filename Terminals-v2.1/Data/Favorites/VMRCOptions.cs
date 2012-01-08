using System;

namespace Terminals.Data
{
    [Serializable]
    public class VMRCOptions : ProtocolOptions
    {
        public Boolean ReducedColorsMode { get; set; }
        public Boolean AdministratorMode { get; set; }

        internal override ProtocolOptions Copy()
        {
            return new VMRCOptions
                {
                    ReducedColorsMode = this.ReducedColorsMode,
                    AdministratorMode = this.AdministratorMode
                };
        }

        internal override void FromCofigFavorite(FavoriteConfigurationElement favorite)
        {
            this.AdministratorMode = favorite.VMRCAdministratorMode;
            this.ReducedColorsMode = favorite.VMRCReducedColorsMode;
        }

        internal override void ToConfigFavorite(FavoriteConfigurationElement favorite)
        {
            favorite.VMRCAdministratorMode = this.AdministratorMode;
            favorite.VMRCReducedColorsMode = this.ReducedColorsMode;
        }
    }
}
