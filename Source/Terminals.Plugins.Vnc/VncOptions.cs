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
    }
}
