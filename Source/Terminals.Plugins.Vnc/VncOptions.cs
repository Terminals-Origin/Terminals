using System;

namespace Terminals.Data
{
    [Serializable]
    public class VncOptions : ProtocolOptions
    {
        public Boolean AutoScale { get; set; }
        public Boolean ViewOnly { get; set; }
        public Int32 DisplayNumber { get; set; }

        public override ProtocolOptions Copy()
        {
            return new VncOptions
                {
                    AutoScale = this.AutoScale,
                    ViewOnly = this.ViewOnly,
                    DisplayNumber = this.DisplayNumber
                };
        }

        public override void FromCofigFavorite(IFavorite destination, FavoriteConfigurationElement source)
        {
            this.AutoScale = source.VncAutoScale;
            this.DisplayNumber = source.VncDisplayNumber;
            this.ViewOnly = source.VncViewOnly;
        }

        public override void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination)
        {
            destination.VncAutoScale = this.AutoScale;
            destination.VncDisplayNumber = this.DisplayNumber;
            destination.VncViewOnly = this.ViewOnly;
        }
    }
}
