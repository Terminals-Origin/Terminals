using System;

namespace Terminals.Data
{
    /// <summary>
    /// Used as "clone able" in favorite protocol properties
    /// </summary>
    [Serializable]
    public abstract class ProtocolOptions
    {
        public abstract ProtocolOptions Copy();
    }
}
