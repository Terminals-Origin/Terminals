using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    /// <summary>
    /// https://the.earth.li/~sgtatham/putty/0.67/htmldoc/Chapter3.html
    /// </summary>
    public class SshOptions : PuttyOptions
    {
        /// <summary>
        /// 3.8.3.9 -agent and -noagent: control use of Pageant for authentication
        /// </summary>
        public bool EnablePagentAuthentication { get; set; }
        /// <summary>
        /// 3.8.3.10 -A and -a: control agent forwarding
        /// </summary>
        public bool EnablePagentForwarding { get; set; }
        /// <summary>
        /// 3.8.3.11 -X and -x: control X11 forwarding
        /// </summary>
        public bool X11Forwarding { get; set; }
        /// <summary>
        /// 3.8.3.15 -C: enable compression
        /// </summary>
        public bool EnableCompression { get; set; }
        /// <summary>
        /// 3.8.3.16 -1 and -2: specify an SSH protocol version
        /// </summary>
        public SshVersion SshVersion { get; set; }


        public override ProtocolOptions Copy()
        {
            return new SshOptions()
            {
                SessionName = this.SessionName,
                Verbose = this.Verbose,
                EnablePagentAuthentication = this.EnablePagentAuthentication,
                EnablePagentForwarding = this.EnablePagentForwarding,
                X11Forwarding = this.X11Forwarding,
                EnableCompression = this.EnableCompression,
                SshVersion = this.SshVersion
            };
        }

    }
}
