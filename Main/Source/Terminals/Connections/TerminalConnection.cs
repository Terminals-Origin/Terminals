using System;
using System.Net.Sockets;
using Terminals.Configuration;
using Terminals.Converters;
using Terminals.Data;
using WalburySoftware;

namespace Terminals.Connections
{
    internal class TerminalConnection : Connection
    {
        #region Fields

        private Boolean connected = false;
        private TerminalEmulator term;
        private Socket client;
        private SSHClient.Protocol sshProtocol;

        #endregion

        #region Connection Members

        public override Boolean Connected
        {
            get
            {
                return connected;
            }
        }

        public override void ChangeDesktopSize(DesktopSize Size)
        {
        }

        public override Boolean Connect()
        {
            String protocol = "unknown";
            try
            {
                Logging.Info(String.Format("Connecting to a {0} Connection", Favorite.Protocol));
                term = new TerminalEmulator();

                Controls.Add(term);
                term.BringToFront();
                this.BringToFront();

                term.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;
                term.Dock = System.Windows.Forms.DockStyle.Fill;

                ConsoleOptions consoleOptions = this.GetConsoleOptionsFromFavorite();
                AssignTerminalCollors(consoleOptions);
                term.Font = FontParser.FromString(consoleOptions.Font);
                term.Rows = consoleOptions.Rows;
                term.Columns = consoleOptions.Columns;

                this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.client.Connect(Favorite.ServerName, Favorite.Port);

                ISecurityOptions security = this.Favorite.Security.GetResolvedCredentials();
                switch (Favorite.Protocol)
                {
                    case ConnectionManager.TELNET:
                        {
                            protocol = ConfigureTelnetConnection(security);
                        }
                        break;

                    case ConnectionManager.SSH:
                        {
                            protocol = ConfigureSshConnection(security);
                        }
                        break;
                }

                this.term.Focus();
                return true;
            }
            catch (Exception exc)
            {
                Logging.Fatal(String.Format("Connecting to {0} Connection", protocol), exc);
                LastError = exc.Message;
                return false;
            }
        }

        private ConsoleOptions GetConsoleOptionsFromFavorite()
        {
            return GetConsoleOptions(this.Favorite);
        }

        internal static ConsoleOptions GetConsoleOptions(IFavorite favorite)
        {
            var consoleOptions = favorite.ProtocolProperties as ConsoleOptions;
            if (consoleOptions != null)
                return consoleOptions;

            var sshOptions = favorite.ProtocolProperties as SshOptions;
            if (sshOptions != null)
                return sshOptions.Console;

            return null;
        }

        private string ConfigureTelnetConnection(ISecurityOptions security)
        {
            TcpProtocol tcpProtocol = new TcpProtocol(new NetworkStream(this.client));
            TelnetProtocol p = new TelnetProtocol();
            tcpProtocol.OnDataIndicated += p.IndicateData;
            tcpProtocol.OnDisconnect += this.OnDisconnected;
            p.TerminalType = this.term.TerminalType;
            p.Username = security.UserName;
            p.Password = security.Password;
            p.OnDataIndicated += this.term.IndicateData;
            p.OnDataRequested += tcpProtocol.RequestData;
            this.term.OnDataRequested += p.RequestData;
            this.connected = this.client.Connected;

            return ConnectionManager.TELNET;
        }

        private string ConfigureSshConnection(ISecurityOptions security)
        {
            this.sshProtocol = new SSHClient.Protocol();
            this.sshProtocol.setTerminalParams(term.TerminalType, term.Rows, term.Columns);
            this.sshProtocol.OnDataIndicated += term.IndicateData;
            this.sshProtocol.OnDisconnect += this.OnDisconnected;
            term.OnDataRequested += this.sshProtocol.RequestData;

            String key = String.Empty;
            var options = this.Favorite.ProtocolProperties as SshOptions;
            SSHClient.KeyConfigElement keyConfigElement = Settings.Instance.SSHKeys.Keys[options.CertificateKey];

            if (keyConfigElement != null)
                key = keyConfigElement.Key;
            
            this.sshProtocol.setProtocolParams(options.AuthMethod, security.UserName, security.Password, key, options.SSH1, options.SSHKeyFile);

            this.sshProtocol.Connect(client);
            this.connected = true; // SSH will throw if fails
            return (options.SSH1) ? "SSH1" : "SSH2";
        }

        private void AssignTerminalCollors(ConsoleOptions consoleOptions)
        {
            this.term.BackColor = ColorParser.FromString(consoleOptions.BackColor);
            this.term.ForeColor = ColorParser.FromString(consoleOptions.TextColor);
            this.term.BlinkColor = ColorParser.FromString(consoleOptions.CursorColor);
        }

        private void OnDisconnected()
        {
            Logging.Fatal(String.Format("{0} Connection Lost {1}", this.Favorite.Protocol, this.Favorite.Name));
            this.connected = false;
            this.FireDisconnected();
        }

        public override void Disconnect()
        {
            try
            {
                this.client.Close();
                if (this.sshProtocol != null)
                    this.sshProtocol.OnConnectionClosed();
            }
            catch (Exception e)
            {
                Logging.Error("Disconnect", e);
            }
        }

        #endregion
    }
}
