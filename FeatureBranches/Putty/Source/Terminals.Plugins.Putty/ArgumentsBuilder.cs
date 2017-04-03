using System;
using System.Text;
using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    internal class ArgumentsBuilder
    {
        private readonly IGuardedSecurity credentials;
        private readonly IFavorite favorite;
        private PuttyOptions puttyOptions;

        public ArgumentsBuilder(IGuardedSecurity credentials, IFavorite favorite)
        {
            this.credentials = credentials;
            this.favorite = favorite;
        }
        private void ValidateGeneral()
        {
            if (!(favorite.Protocol == SshConnectionPlugin.SSH || favorite.Protocol == TelnetConnectionPlugin.TELNET))
                throw new ArgumentException(string.Format("Protocol {0} is not supported", favorite.Protocol));
        }

        public string Build()
        {
            var args = default(string);
            ValidateGeneral();

            puttyOptions = favorite.ProtocolProperties as PuttyOptions;

            if (favorite.Protocol == SshConnectionPlugin.SSH)
                args = BuildSsh();
            else if (favorite.Protocol == TelnetConnectionPlugin.TELNET)
                args = BuildTelnet();
            

            return args;
        }

        internal string BuildTelnet()
        {
            var args = new StringBuilder();
            var telnetOptions = puttyOptions as TelnetOptions;

            // 3.8.3.1 -load: load a saved session
            if (!string.IsNullOrEmpty(telnetOptions.SessionName))
                args.AppendFormat(" -load \"{0}\"", telnetOptions.SessionName);

            // 3.8.3.2 Selecting a protocol: -ssh, -telnet, -rlogin, -raw -serial
            args.Append(" -telnet");

            // 3.8.3.3 -v: increase verbosity
            if (telnetOptions.Verbose)
                args.Append(" -v");

            // 3.8.3.7 -P: specify a port number
            if (favorite.Port > 0)
                args.AppendFormat(" -P {0}", favorite.Port);

            args.Append(" " + favorite.ServerName);

            return args.ToString();
        }

        internal string BuildSsh()
        {
            var userName = this.credentials.UserName;
            var userPassword = this.credentials.Password;

            var args = new StringBuilder();
            var sshOptions = puttyOptions as SshOptions;

            // 3.8.3.1 -load: load a saved session
            if (!string.IsNullOrEmpty(sshOptions.SessionName))
                args.AppendFormat(" -load \"{0}\"", sshOptions.SessionName);

            // 3.8.3.2 Selecting a protocol: -ssh, -telnet, -rlogin, -raw -serial
            args.Append(" -ssh");

            // 3.8.3.3 -v: increase verbosity
            if (sshOptions.Verbose)
                args.Append(" -v");

            // 3.8.3.4 -l: specify a login name
            if (!string.IsNullOrEmpty(userName))
                args.Append(" -l " + userName);

            // 3.8.3.5 -L, -R and -D: set up port forwardings
            // not right now, supported via session config

            // 3.8.3.6 -m: read a remote command or script from a file
            // not right now

            // 3.8.3.7 -P: specify a port number
            if (favorite.Port > 0)
                args.AppendFormat(" -P {0}", favorite.Port);

            // 3.8.3.8 -pw: specify a password
            if (!string.IsNullOrEmpty(userPassword))
                args.Append(" -pw " + userPassword);

            // 3.8.3.9 -agent and -noagent: control use of Pageant for authentication
            if (sshOptions.EnablePagentAuthentication)
                args.Append(" -agent");
            else
                args.Append(" -noagent");

            // 3.8.3.10 -A and -a: control agent forwarding
            if (sshOptions.EnablePagentForwarding)
                args.Append(" -A");
            else
                args.Append(" -a");

            // 3.8.3.11 -X and -x: control X11 forwarding
            if (sshOptions.X11Forwarding)
                args.Append(" -X");
            else
                args.Append(" -x");

            // 3.8.3.15 -C: enable compression
            if (sshOptions.EnableCompression)
                args.Append(" -C");

            // 3.8.3.16 -1 and -2: specify an SSH protocol version
            if (sshOptions.SshVersion == SshVersion.SshVersion1)
                args.Append(" -1");
            else if (sshOptions.SshVersion == SshVersion.SshVersion2)
                args.Append(" -2");

            args.Append(" " + favorite.ServerName);

            return args.ToString();
        }
    }
}