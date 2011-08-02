using System;
using System.Drawing;
using System.Net.Sockets;
using Terminals.Configuration;
using Terminals.Forms;
using WalburySoftware;

namespace Terminals.Connections
{
    public class TerminalConnection : Connection
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

        public override void ChangeDesktopSize(Terminals.DesktopSize Size)
        {
        }
                
        public override Boolean Connect()
        {
            String protocol = "unknown";
            try
            {
                Terminals.Logging.Log.Info(String.Format("Connecting to a {0} Connection", Favorite.Protocol));
                term = new TerminalEmulator();

                Controls.Add(term);
                term.BringToFront();
                this.BringToFront();

                term.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;
                term.Dock = System.Windows.Forms.DockStyle.Fill;

                term.BackColor = Color.FromName(Favorite.ConsoleBackColor);
                term.Font = FontParser.FromString(Favorite.ConsoleFont);
                term.ForeColor = Color.FromName(Favorite.ConsoleTextColor);

                term.Rows = Favorite.ConsoleRows;
                term.Columns = Favorite.ConsoleCols;

                this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.client.Connect(Favorite.ServerName, Favorite.Port);

                String domainName = String.IsNullOrEmpty(Favorite.DomainName) ? Settings.DefaultDomain : Favorite.DomainName;
                String password = String.IsNullOrEmpty(Favorite.Password) ? Settings.DefaultPassword : Favorite.Password;
                String userName = String.IsNullOrEmpty(Favorite.UserName) ? Settings.DefaultUsername : Favorite.UserName;

                switch (Favorite.Protocol)
                {
                    case "Telnet":
                        protocol = "Telnet";
                        TcpProtocol t = new TcpProtocol(new NetworkStream(this.client));
                        TelnetProtocol p = new TelnetProtocol();
                        t.OnDataIndicated += p.IndicateData;
                        t.OnDisconnect += this.OnDisconnected;
                        p.TerminalType = term.TerminalType;
                        p.Username = userName;
                        p.Password = password;
                        p.OnDataIndicated += term.IndicateData;
                        p.OnDataRequested += t.RequestData;
                        term.OnDataRequested += p.RequestData;
                        this.connected = this.client.Connected;
                        break;

                    case "SSH":
                        this.sshProtocol = new SSHClient.Protocol();
                        this.sshProtocol.setTerminalParams(term.TerminalType, term.Rows, term.Columns);
                        this.sshProtocol.OnDataIndicated += term.IndicateData;
                        this.sshProtocol.OnDisconnect += this.OnDisconnected;
                        term.OnDataRequested += this.sshProtocol.RequestData;

                        String key = String.Empty;
                        SSHClient.KeyConfigElement e = Settings.SSHKeys.Keys[Favorite.KeyTag];

                        if (e != null)
                            key = e.Key;

                        this.sshProtocol.setProtocolParams(Favorite.AuthMethod, userName, password, key, Favorite.SSH1);
                        protocol = (Favorite.SSH1) ? "SSH1" : "SSH2";

                        this.sshProtocol.Connect(client);
                        this.connected = true; // SSH will throw if fails
                        break;
                }

                this.term.Focus();
                return true;
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Fatal(String.Format("Connecting to {0} Connection", protocol), exc);
                return false;
            }
        }

        private void OnDisconnected()
        {
            Terminals.Logging.Log.Fatal(String.Format("{0} Connection Lost {1}", this.Favorite.Protocol, this.Favorite.Name));
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
                Terminals.Logging.Log.Error("Disconnect", e);
            }
        }

        #endregion
    }
}
