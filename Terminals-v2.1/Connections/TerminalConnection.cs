using System;
using System.Net.Sockets;
using Terminals.Configuration;
using Terminals.Converters;
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
                Logging.Log.Info(String.Format("Connecting to a {0} Connection", Favorite.Protocol));
                term = new TerminalEmulator();

                Controls.Add(term);
                term.BringToFront();
                this.BringToFront();

                term.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;
                term.Dock = System.Windows.Forms.DockStyle.Fill;

                AssignTerminalCollors();
                term.Font = FontParser.FromString(Favorite.ConsoleFont);

                term.Rows = Favorite.ConsoleRows;
                term.Columns = Favorite.ConsoleCols;

                this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.client.Connect(Favorite.ServerName, Favorite.Port);

                String password = String.IsNullOrEmpty(Favorite.Password) ? Settings.DefaultPassword : Favorite.Password;
                String userName = String.IsNullOrEmpty(Favorite.UserName) ? Settings.DefaultUsername : Favorite.UserName;

                switch (Favorite.Protocol)
                {
                    case ConnectionManager.TELNET:
                        {
                           protocol = ConfigureTelnetConnection(userName, password);
                        }
                        break;

                    case ConnectionManager.SSH:
                        {
                            protocol = ConfigureSshConnection(userName, password);
                        }
                        break;
                }

                this.term.Focus();
                return true;
            }
            catch (Exception exc)
            {
                Logging.Log.Fatal(String.Format("Connecting to {0} Connection", protocol), exc);
                return false;
            }
        }

        private string ConfigureTelnetConnection(string userName, string password)
        {
            TcpProtocol tcpProtocol = new TcpProtocol(new NetworkStream(this.client));
            TelnetProtocol p = new TelnetProtocol();
            tcpProtocol.OnDataIndicated += p.IndicateData;
            tcpProtocol.OnDisconnect += this.OnDisconnected;
            p.TerminalType = this.term.TerminalType;
            p.Username = userName;
            p.Password = password;
            p.OnDataIndicated += this.term.IndicateData;
            p.OnDataRequested += tcpProtocol.RequestData;
            this.term.OnDataRequested += p.RequestData;
            this.connected = this.client.Connected;

            return ConnectionManager.TELNET;
        }

        private string ConfigureSshConnection(string userName, string password)
        {
            this.sshProtocol = new SSHClient.Protocol();
            this.sshProtocol.setTerminalParams(term.TerminalType, term.Rows, term.Columns);
            this.sshProtocol.OnDataIndicated += term.IndicateData;
            this.sshProtocol.OnDisconnect += this.OnDisconnected;
            term.OnDataRequested += this.sshProtocol.RequestData;

            String key = String.Empty;
            SSHClient.KeyConfigElement keyConfigElement = Settings.SSHKeys.Keys[Favorite.KeyTag];

            if (keyConfigElement != null)
                key = keyConfigElement.Key;

            this.sshProtocol.setProtocolParams(Favorite.AuthMethod, userName, password, key, Favorite.SSH1);

            this.sshProtocol.Connect(client);
            this.connected = true; // SSH will throw if fails
            return (Favorite.SSH1) ? "SSH1" : "SSH2";
        }

        private void AssignTerminalCollors()
        {
            this.term.BackColor = ColorParser.FromString(this.Favorite.ConsoleBackColor);
            this.term.ForeColor = ColorParser.FromString(this.Favorite.ConsoleTextColor);
            this.term.BlinkColor = ColorParser.FromString(this.Favorite.ConsoleCursorColor);
        }

        private void OnDisconnected()
        {
            Logging.Log.Fatal(String.Format("{0} Connection Lost {1}", this.Favorite.Protocol, this.Favorite.Name));
            this.connected = false;
            if (this.ParentForm.InvokeRequired)
            {
                InvokeCloseTabPage d = new InvokeCloseTabPage(this.CloseTabPage);
                this.Invoke(d, new object[] { this.Parent });
            }
            else
            {
                this.CloseTabPage(this.Parent);
            }
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
                Logging.Log.Error("Disconnect", e);
            }
        }

        #endregion
    }
}
