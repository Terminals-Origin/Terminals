using System;

namespace Terminals.Data
{
    [Serializable]
    public class VncOptions
    {
        public Boolean AutoScale { get; set; }
        public Boolean ViewOnly { get; set; }
        public Int32 DisplayNumber { get; set; }
    }
}
