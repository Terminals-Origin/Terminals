using System;

namespace Terminals.Data
{
    [Serializable]
    public class VncOptions : ProtocolOptions
    {
        public Boolean AutoScale { get; set; }
        public Boolean ViewOnly { get; set; }
        public Int32 DisplayNumber { get; set; }

        internal override ProtocolOptions Copy()
        {
            return new VncOptions
                {
                    AutoScale = this.AutoScale,
                    ViewOnly = this.ViewOnly,
                    DisplayNumber = this.DisplayNumber
                };
        }

        internal override void FromCofigFavorite(FavoriteConfigurationElement favorite)
        {
            this.AutoScale = favorite.VncAutoScale;
            this.DisplayNumber = favorite.VncDisplayNumber;
            this.ViewOnly = favorite.VncViewOnly;
        }

        internal override void ToConfigFavorite(FavoriteConfigurationElement favorite)
        {
            favorite.VncAutoScale = this.AutoScale;
            favorite.VncDisplayNumber = this.DisplayNumber;
            favorite.VncViewOnly = this.ViewOnly;
        }
    }
}
