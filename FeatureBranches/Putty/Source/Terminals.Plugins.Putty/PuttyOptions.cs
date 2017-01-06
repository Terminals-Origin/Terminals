using System;
using Terminals.Data;

namespace Terminals.Plugins.Putty
{

    [Serializable]
    public class PuttyOptions : ProtocolOptions
    {

        public string SessionName { get; set; }
        public bool X11Forwarding { get; set; }
        public bool EnableCompression { get; set; }

        public override ProtocolOptions Copy()
        {
            return new PuttyOptions()
            {
                SessionName = this.SessionName,
                X11Forwarding = this.X11Forwarding,
                EnableCompression = this.EnableCompression
            };
        }
    }
}
