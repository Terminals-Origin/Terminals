using System;
using Terminals.Data;

namespace Terminals.Plugins.Putty
{

    [Serializable]
    public abstract class PuttyOptions : ProtocolOptions
    {
        public string SessionName { get; set; }

        public bool Verbose { get; set; }

    }
}
