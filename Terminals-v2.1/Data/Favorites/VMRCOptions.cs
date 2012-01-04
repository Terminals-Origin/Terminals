using System;

namespace Terminals.Data
{
    [Serializable]
    public class VMRCOptions : ICloneable
    {
        public Boolean ReducedColorsMode { get; set; }
        public Boolean AdministratorMode { get; set; }

        public object Clone()
        {
            return new VMRCOptions
                {
                    ReducedColorsMode = this.ReducedColorsMode,
                    AdministratorMode = this.AdministratorMode
                };
        }
    }
}
