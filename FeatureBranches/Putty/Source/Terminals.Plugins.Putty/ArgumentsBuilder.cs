using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    internal class ArgumentsBuilder
    {
        private readonly IGuardedSecurity credentials;
        private readonly IFavorite favorite;

        public ArgumentsBuilder(IGuardedSecurity credentials, IFavorite favorite)
        {
            this.credentials = credentials;
            this.favorite = favorite;
        }

        public string Build()
        {
            var userName = this.credentials.UserName;
            var userPassword = this.credentials.Password;

            var puttyOptions = favorite.ProtocolProperties as PuttyOptions;

            var args = string.Empty;

            // 3.8.3.1 -load: load a saved session
            if (!string.IsNullOrEmpty(puttyOptions.SessionName))
                args += string.Format(" -load \"{0}\"", puttyOptions.SessionName);

            // 3.8.3.2 Selecting a protocol: -ssh, -telnet, -rlogin, -raw -serial
            args += " -ssh";

            // -l: specify a login name
            if (!string.IsNullOrEmpty(userName))
                args += " -l " + userName;

            // 3.8.3.5 -L, -R and -D: set up port forwardings
            // not right now


            // 3.8.3.7 -P: specify a port number
            args += " -P " + favorite.Port;

            // 3.8.3.8 -pw: specify a password
            if (!string.IsNullOrEmpty(userPassword))
                args += " -pw " + userPassword; 

            // 3.8.3.11 -X and -x: control X11 forwarding
            if (puttyOptions.X11Forwarding)
                args += " -X ";
            else
                args += " -x ";

            // 3.8.3.11 -X and -x: control X11 forwarding
            if (puttyOptions.EnableCompression)
                args += " -C ";


            args += " " + favorite.ServerName;

            return args;

        }
    }
}