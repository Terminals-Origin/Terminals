using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Terminals.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TabControl;
using WalburySoftware;
using Routrek.SSHC;
using Org.Mentalis.Network.ProxySocket;

namespace Terminals.Connections
{
    public class TerminalConnection : Connection
    {
        #region Connection Members
        private bool connected = false;
        public override bool Connected { get { return connected; } }
        public override void ChangeDesktopSize(Terminals.DesktopSize Size)
        {
        }

        private TerminalEmulator term;
        private ProxySocket client;        	

        public override bool Connect()
        {
            string protocol = "unknown";
            try
            {
                Terminals.Logging.Log.Info("Connecting to a "+Favorite.Protocol+" Connection");
                term = new TerminalEmulator();

                Controls.Add(term);
                term.BringToFront();
                this.BringToFront();

                term.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;
                term.Dock = DockStyle.Fill;

                term.BackColor = Color.FromName(Favorite.ConsoleBackColor);
                term.Font = FontParser.ParseFontName(Favorite.ConsoleFont);
                term.ForeColor = Color.FromName(Favorite.ConsoleTextColor);

                term.Rows = Favorite.ConsoleRows;
                term.Columns = Favorite.ConsoleCols;

                client = new ProxySocket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                client.ProxyUser="";
                client.ProxyPass="";
                client.Connect(Favorite.ServerName,Favorite.Port);
                
                string domainName = Favorite.DomainName;
                if(domainName == null || domainName == "") domainName = Settings.DefaultDomain;


                if (Favorite.Protocol == "Telnet")
                {
                    protocol = "Telnet";
                    TcpProtocol t = new TcpProtocol(new NetworkStream(client));
                    TelnetProtocol p = new TelnetProtocol();
                    t.OnDataIndicated += p.IndicateData;
	            	t.OnDisconnect += this.OnDisconnected;
                    p.TerminalType = term.TerminalType;
	            	p.Username=Favorite.UserName;
	            	p.Password = Favorite.Password;
	            	p.OnDataIndicated += term.IndicateData;
	            	p.OnDataRequested += t.RequestData;
	            	term.OnDataRequested += p.RequestData;
	                connected = client.Connected;
                }
                else
                {
	                SSHProtocol p = new SSHProtocol();
                    p.TerminalName = term.TerminalType;
                    p.TerminalHeight = term.Rows;
                    p.TerminalWidth = term.Width;
	            	p.OnDataIndicated += term.IndicateData;
	            	term.OnDataRequested += p.RequestData;

	                AuthMethod authMethod = Favorite.AuthMethod;

	                string userName = Favorite.UserName;
	                if(userName==null)
		            	userName="";
                    if (userName == "") // can't do auto login without username
                        authMethod = AuthMethod.KeyboardInteractive;
	            	p.Username=userName;
                    
                    if(authMethod==AuthMethod.PublicKey)
                    {
                	    string tag = Favorite.KeyTag;
                        if (tag == null || tag == "")
                        {
                            authMethod = AuthMethod.Password;
                        }
                        else
                        {
                            SSHKeyElement e = Settings.SSHKeys[tag];
                            if (e != null && e.key != null)
                            {
                                p.AuthenticationType = AuthenticationType.PublicKey;
                                p.Key = e.key.ToString();
                            }
                            else
                            {
                                authMethod = AuthMethod.Password;
                            }
                        }
                    }
                    if(authMethod==AuthMethod.Password)
                    {
	                    string pass = Favorite.Password;
	                    if(pass==null)
	                	    pass="";
	                    if(pass=="")
	                    {
	                	    p.AuthenticationType=AuthenticationType.KeyboardInteractive;
	                    }
	                    else
	                    {
	                	    p.AuthenticationType=AuthenticationType.Password;
		            	    p.Password=pass;
	                    }
                    }
                    if (Favorite.SSH1)
                    {
                        protocol = "SSH1";
                        p.Protocol = Routrek.SSHC.SSHProtocol.SSH1;
                    }
                    else
                    {
                        protocol = "SSH2";
                        p.Protocol = Routrek.SSHC.SSHProtocol.SSH2;
                    }
                    p.Connect(client);
                    connected = true; // SSH will throw if fails
                }
                term.Focus();
                return true;
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Fatal("Connecting to "+protocol+" Connection", exc);
                return false;
            }
        }

        void OnDisconnected()
        {
            Terminals.Logging.Log.Fatal(this.Favorite.Protocol + " Connection Lost" + this.Favorite.Name);
            this.connected = false;
 /* TODO - this is getting called from the TcpProtocol read thread - sort it.
            TabControlItem selectedTabPage = (TabControlItem)(this.Parent);
            bool wasSelected = selectedTabPage.Selected;
            ParentForm.tcTerminals.RemoveTab(selectedTabPage);
            ParentForm.tcTerminals_TabControlItemClosed(null, EventArgs.Empty);
            if(wasSelected)
                NativeApi.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
            ParentForm.UpdateControls();
            */
        }

        public override void Disconnect()
        {
            try
            {
                client.Close();
            }
            catch(Exception e)
            {
                Terminals.Logging.Log.Info("", e);
            }
        }

        #endregion

    }
	public class SSHProtocol : ISSHConnectionEventReceiver, ISSHChannelEventReceiver
	{
		#region Public Properties
		public string Username
		{
			set
			{
				_params.UserName = value;
			}
		}
		public string Password
		{
			set
			{
				_params.Password = value;
			}
		}
		public AuthenticationType AuthenticationType
		{
			set
			{
				_params.AuthenticationType = value;
			}
		}
        public string Key
        {
            set
            {
            	// tunnel this through to modified key classes!
            	// rule will be if password is set but IdentifyFile
            	// is null
            	// then password is a base64 ProtectedData key
            	_params.Password = value;
            }
        }
        public Routrek.SSHC.SSHProtocol Protocol
        {
            set
            {
            	_params.Protocol = value;
            }
        }
        public string TerminalName
        {
            set
            {
            	_params.TerminalName = value;
            }
        }
        public int TerminalWidth
        {
            set
            {
            	_params.TerminalWidth = value;
            }
        }
        public int TerminalHeight
        {
            set
            {
            	_params.TerminalHeight = value;
            }
        }
		#endregion
		#region Public Enums
		#endregion
		#region Public Fields
		#endregion
		#region Public Delegates
		public delegate void DataIndicate(byte[] data);
		#endregion
		#region Public Events
	    public event DataIndicate OnDataIndicated;
		#endregion
		#region Public Constructors
		public SSHProtocol()
		{
			_params = new SSHConnectionParameter();
            _params.KeyCheck = delegate(SSHConnectionInfo info) {
                //byte[] h = info.HostKeyMD5FingerPrint();
                //foreach(byte b in h) Debug.Write(String.Format("{0:x2} ", b));
                return true;
            };
		}
		#endregion
		#region Public Methods
		public void RequestData (byte[] data)
		{
			_pf.Transmit(data, 0, data.Length);
		}
		public void Connect (Socket s)
		{
			_params.WindowSize = 0x1000;
			_conn = SSHConnection.Connect(_params, this, s);
			_pf = _conn.OpenShell(this);
			SSHConnectionInfo ci = _conn.ConnectionInfo;
		}
		
		public void OnData(byte[] data, int offset, int length)
		{
			if(OnDataIndicated!=null)
			{
				byte[] obuf = new byte[length];
				System.Array.Copy(data, offset, obuf, 0, obuf.Length);
				OnDataIndicated(obuf);
			}
		}

		public void OnDebugMessage(bool always_display, byte[] data) 
		{
            //Debug.WriteLine("DEBUG: "+ Encoding.Default.GetString(data));
		}

		public void OnIgnoreMessage(byte[] data)
		{
            //Debug.WriteLine("Ignore: "+ Encoding.Default.GetString(data));
		}

		public void OnAuthenticationPrompt(string[] msg)
		{
			//Debug.WriteLine("Auth Prompt "+msg[0]);
		}

		public void OnError(Exception error) 
		{
			//Debug.WriteLine("ERROR: "+ msg);
		}
		public void OnChannelError(Exception error)
		{
			//Debug.WriteLine("Channel ERROR: "+ error.Message);
		}
		public void OnChannelClosed()
		{
			//Debug.WriteLine("Channel closed");
			_conn.Disconnect("");
			//_conn.AsyncReceive(this);
		}

		public void OnChannelEOF()
		{
			_pf.Close();
			//Debug.WriteLine("Channel EOF");
		}
		public void OnExtendedData(int type, byte[] data) 
		{
			//Debug.WriteLine("EXTENDED DATA");
		}
		public void OnConnectionClosed() 
		{
			//Debug.WriteLine("Connection closed");
		}
		public void OnUnknownMessage(byte type, byte[] data) 
		{
			//Debug.WriteLine("Unknown Message " + type);
		}
		public void OnChannelReady() 
		{
			_ready = true;
		}

		public void OnChannelError(Exception error, string msg) 
		{
			//Debug.WriteLine("Channel ERROR: "+ msg);
		}

        public void OnError(Exception error, string msg)
        {
        }

        public void OnMiscPacket(byte type, byte[] data, int offset, int length) 
		{
		}

		public PortForwardingCheckResult CheckPortForwardingRequest(string host, int port, string originator_host, int originator_port) 
		{
			PortForwardingCheckResult r = new PortForwardingCheckResult();
			r.allowed = true;
			r.channel = this;
			return r;
		}
		public void EstablishPortforwarding(ISSHChannelEventReceiver rec, SSHChannel channel) 
		{
			_pf = channel;
		}
		#endregion
		#region Public Overrides
		#endregion
		#region Private Enums
		#endregion
		#region Private Fields
		private SSHConnectionParameter _params;
		private SSHConnection _conn;
		private bool _ready;
		public SSHChannel _pf;
		#endregion

    }
}
