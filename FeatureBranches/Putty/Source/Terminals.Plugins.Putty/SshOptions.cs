using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    public class SshOptions : PuttyOptions
    {
        public bool X11Forwarding { get; set; }
        public bool EnableCompression { get; set; }

        public override ProtocolOptions Copy()
        {
            return new SshOptions()
            {
                SessionName = this.SessionName,
                Verbose = this.Verbose,
                X11Forwarding = this.X11Forwarding,
                EnableCompression = this.EnableCompression
            };
        }

    }
}
