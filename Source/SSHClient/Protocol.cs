/*
 * Created by SharpDevelop.
 * User: CableJ01
 * Date: 18/01/2009
 * Time: 13:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Granados;
using System.Net.Sockets;

namespace SSHClient
{
	/// <summary>
	/// Description of Protocol.
	/// </summary>
	public class Protocol : ISSHConnectionEventReceiver, ISSHChannelEventReceiver
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
        public Granados.SSHProtocol SSHProtocol
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
        public delegate void Disconnect();
        #endregion
		#region Public Events
	    public event DataIndicate OnDataIndicated;
        public event Disconnect OnDisconnect;
		#endregion
		#region Public Constructors
		public Protocol()
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
		public void setTerminalParams(string type, int rows, int cols)
		{
			_params = new SSHConnectionParameter();
			TerminalName = type;
            TerminalHeight = rows;
            TerminalWidth = cols;
		}

		public void setProtocolParams(
			AuthMethod authMethod,
			string userName,
			string pass,
			string key,
			bool SSH1)
		{
            // each following check can force to different authentication
            authMethod = CheckUserName(userName, authMethod);
		    authMethod = CheckPublickKey(key, authMethod);
            authMethod = CheckPassword(pass, authMethod);
		    AssingAuthentization(authMethod);
            ChooseProtocolVersion(SSH1);
		    _params.EventTracer = new SShTraceLissener();
		}

	    private AuthMethod CheckUserName(String userName, AuthMethod authMethod)
        {
            if (userName == null)
                userName = "";
            this.Username = userName;

            if (userName == "") // can't do auto login without username
                return AuthMethod.KeyboardInteractive;

            return authMethod;
        }

	    private AuthMethod CheckPublickKey(String key, AuthMethod authMethod)
        {
            if (authMethod == AuthMethod.PublicKey)
            {
                if (string.IsNullOrEmpty(key))
                    return AuthMethod.Password;
                
                this.Key = SSH2UserAuthKey.FromBase64String(key).toSECSHStyle("");
            }

            return authMethod;
        }
 	    
        private AuthMethod CheckPassword(String pass, AuthMethod authMethod)
        {
            if (pass == null)
                pass = "";
            this.Password = pass; // password always has to be set: required by grandados

            if (authMethod == AuthMethod.Password && pass == "")
                return AuthMethod.KeyboardInteractive;

            return authMethod;
        } 
      
        private void AssingAuthentization(AuthMethod authMethod)
        {
            switch (authMethod)
            {
                case AuthMethod.Password:
                    this.AuthenticationType = AuthenticationType.Password;
                    break;
                case AuthMethod.PublicKey:
                    this.AuthenticationType = AuthenticationType.PublicKey;
                    break;
                
                default:
                    this.AuthenticationType = AuthenticationType.KeyboardInteractive;
                    break;                
                // granados doesnt support Host authentication
            }
        }

        private void ChooseProtocolVersion(bool SSH1)
	    {
	        if (SSH1)
	        {
	            this.SSHProtocol = SSHProtocol.SSH1;
	        }
	        else
	        {
	            this.SSHProtocol = SSHProtocol.SSH2;
	        }
	    }

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
            OnConnectionClosed();
			//Debug.WriteLine("Channel EOF");
		}
		public void OnExtendedData(int type, byte[] data) 
		{
			//Debug.WriteLine("EXTENDED DATA");
		}
		public void OnConnectionClosed() 
		{
			//Debug.WriteLine("Connection closed");.
            if (OnDisconnect != null) OnDisconnect();
		}
		public void OnUnknownMessage(byte type, byte[] data) 
		{
			//Debug.WriteLine("Unknown Message " + type);
		}
        public void OnChannelReady() 
        {
        //    _ready = true;
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
		//private bool _ready;
		public SSHChannel _pf;
		#endregion
    }
}
