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
            throw new NotImplementedException("moved to converter");
        }

        public override void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination)
        {
            throw new NotImplementedException("moved to converter");
        }
    }
}
