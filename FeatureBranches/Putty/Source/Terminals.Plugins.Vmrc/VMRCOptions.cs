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
    }
}
