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
            if (string.IsNullOrEmpty(favorite.ServerName)) 
                throw new ArgumentException("ServerName mus't be provided.");

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

            // -l: specify a login name
            if (!string.IsNullOrEmpty(userName))
                args.Append(" -l " + userName);

            // 3.8.3.5 -L, -R and -D: set up port forwardings
            // not right now


            // 3.8.3.7 -P: specify a port number
            if (favorite.Port > 0)
                args.AppendFormat(" -P {0}", favorite.Port);

            // 3.8.3.8 -pw: specify a password
            if (!string.IsNullOrEmpty(userPassword))
                args.Append(" -pw " + userPassword);

            // 3.8.3.11 -X and -x: control X11 forwarding
            if (sshOptions.X11Forwarding)
                args.Append(" -X");
            else
                args.Append(" -x");

            // 3.8.3.11 -X and -x: control X11 forwarding
            if (sshOptions.EnableCompression)
                args.Append(" -C");

            args.Append(" " + favorite.ServerName);

            return args.ToString();
        }
    }
}