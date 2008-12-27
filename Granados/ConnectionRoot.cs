/* ---------------------------------------------------------------------------
 *
 * Copyright (c) Routrek Networks, Inc.    All Rights Reserved..
 * 
 * This file is a part of the Granados SSH Client Library that is subject to
 * the license included in the distributed package.
 * You may not use this file except in compliance with the license.
 * 
 * ---------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Routrek.Crypto;
using Routrek.PKI;
using Routrek.SSHCV1;
using Routrek.SSHCV2;

namespace Routrek.SSHC
{
	public abstract class SSHConnection {

		internal AbstractSocket _stream;
		internal ISSHConnectionEventReceiver _eventReceiver;
		
		protected byte[] _sessionID;
		internal Cipher _tCipher; //transmission
		//internal Cipher _rCipher; //reception
		protected SSHConnectionParameter _param;

		protected object _tLockObject = new Object();

		protected bool _closed;

		protected bool _autoDisconnect;

		protected AuthenticationResult _authenticationResult;

		protected SSHConnection(SSHConnectionParameter param, ISSHConnectionEventReceiver receiver) {
			_param = (SSHConnectionParameter)param.Clone();
			_eventReceiver = receiver;
			_channel_entries = new ArrayList(16);
			_autoDisconnect = true;
		}

		public abstract SSHConnectionInfo ConnectionInfo { get; }
		
		/**
		* returns true if any data from server is available
		*/
		public bool Available {
			get {
				if(_closed)
					return false;
				else
					return _stream.DataAvailable;
			}
		}

		public SSHConnectionParameter Param {
			get {
				return _param;
			}
		}
		public AuthenticationResult AuthenticationResult {
			get {
				return _authenticationResult;
			}
		}

		internal abstract IByteArrayHandler PacketBuilder {
			get ;
		}

		public ISSHConnectionEventReceiver EventReceiver {
			get {
				return _eventReceiver;
			}
		}
		public bool IsClosed {
			get {
				return _closed;
			}
		}
		public bool AutoDisconnect {
			get {
				return _autoDisconnect;
			}
			set {
				_autoDisconnect = value;
			}
		}


		internal abstract AuthenticationResult Connect(AbstractSocket target);

		/**
		* terminates this connection
		*/
		public abstract void Disconnect(string msg);

		/**
		* opens a pseudo terminal
		*/
		public abstract SSHChannel OpenShell(ISSHChannelEventReceiver receiver);

		/**
		 * forwards the remote end to another host
		 */ 
		public abstract SSHChannel ForwardPort(ISSHChannelEventReceiver receiver, string remote_host, int remote_port, string originator_host, int originator_port);

		/**
		 * listens a connection on the remote end
		 */ 
		public abstract void ListenForwardedPort(string allowed_host, int bind_port);

		/**
		 * cancels binded port
		 */ 
		public abstract void CancelForwardedPort(string host, int port);

		/**
		* closes socket directly.
		*/
		public abstract void Close();

		
		public abstract void SendIgnorableData(string msg);


		/**
		 * opens another SSH connection via port-forwarded connection
		 */ 
		public SSHConnection OpenPortForwardedAnotherConnection(SSHConnectionParameter param, ISSHConnectionEventReceiver receiver, string host, int port) {
			ProtocolNegotiationHandler pnh = new ProtocolNegotiationHandler(param);
			ChannelSocket s = new ChannelSocket(pnh);

			SSHChannel ch = ForwardPort(s, host, port, "localhost", 0);
			s.SSHChennal = ch;
			return SSHConnection.Connect(param, receiver, pnh, s);
		}

		//channel id support
		protected class ChannelEntry {
			public int _localID;
			public ISSHChannelEventReceiver _receiver;
			public SSHChannel _channel;
		}

		protected ArrayList _channel_entries;
		protected int _channel_sequence;
		protected ChannelEntry FindChannelEntry(int id) {
			for(int i=0; i<_channel_entries.Count; i++) {
				ChannelEntry e = (ChannelEntry)_channel_entries[i];
				if(e._localID==id) return e;
			}
			return null;
		}
		protected ChannelEntry RegisterChannelEventReceiver(SSHChannel ch, ISSHChannelEventReceiver r) {
			lock(this) {
				ChannelEntry e = new ChannelEntry();
				e._channel = ch;
				e._receiver = r;
				e._localID = _channel_sequence++;

				for(int i=0; i<_channel_entries.Count; i++) {
					if(_channel_entries[i]==null) {
						_channel_entries[i] = e;
						return e;
					}
				}
				_channel_entries.Add(e);
				return e;
			}
		}
		internal void RegisterChannel(int local_id, SSHChannel ch) {
			FindChannelEntry(local_id)._channel = ch;
		}
		internal void UnregisterChannelEventReceiver(int id) {
			lock(this) {
				foreach(ChannelEntry e in _channel_entries) {
					if(e._localID==id) {
						_channel_entries.Remove(e);
						break;
					}
				}
				if(this.ChannelCount==0 && _autoDisconnect) Disconnect(""); //auto close
			}
		}
		public virtual int ChannelCount {
			get {
				int r = 0;
				for(int i=0; i<_channel_entries.Count; i++) {
					if(_channel_entries[i]!=null) r++;
				}
				return r;
			}
		}

		
		//establishes a SSH connection in subject to ConnectionParameter
		public static SSHConnection Connect(SSHConnectionParameter param, ISSHConnectionEventReceiver receiver, Socket underlying_socket) {
			if(param.UserName==null) throw new InvalidOperationException("UserName property is not set");
			if(param.Password==null) throw new InvalidOperationException("Password property is not set");

			ProtocolNegotiationHandler pnh = new ProtocolNegotiationHandler(param);
			PlainSocket s = new PlainSocket(underlying_socket, pnh);
			s.RepeatAsyncRead();
			return ConnectMain(param, receiver, pnh, s);
		}
		internal static SSHConnection Connect(SSHConnectionParameter param, ISSHConnectionEventReceiver receiver, ProtocolNegotiationHandler pnh, AbstractSocket s) {
			if(param.UserName==null) throw new InvalidOperationException("UserName property is not set");
			if(param.Password==null) throw new InvalidOperationException("Password property is not set");

			return ConnectMain(param, receiver, pnh, s);
		}
		private static SSHConnection ConnectMain(SSHConnectionParameter param, ISSHConnectionEventReceiver receiver, ProtocolNegotiationHandler pnh, AbstractSocket s) {
			pnh.Wait();

			if(pnh.State!=ReceiverState.Ready) throw new SSHException(pnh.ErrorMessage);

			string sv = pnh.ServerVersion;

			SSHConnection con = null;
			if(param.Protocol==SSHProtocol.SSH1)
				con = new SSH1Connection(param, receiver, sv, SSHUtil.ClientVersionString(param.Protocol));
			else
				con = new SSH2Connection(param, receiver, sv, SSHUtil.ClientVersionString(param.Protocol));

			s.SetHandler(con.PacketBuilder);
			SendMyVersion(s, param);

			if(con.Connect(s)!=AuthenticationResult.Failure)
				return con;
			else {
				s.Close();
				return null;
			}
		}

		private static void SendMyVersion(AbstractSocket stream, SSHConnectionParameter param) {
			string cv = SSHUtil.ClientVersionString(param.Protocol);
			if(param.Protocol==SSHProtocol.SSH1)
				cv += param.SSH1VersionEOL;
			else
				cv += "\r\n";
			byte[] data = Encoding.ASCII.GetBytes(cv);
			stream.Write(data, 0, data.Length);
		}
	}

	public enum ChannelType {
		Session,
		Shell,
		ForwardedLocalToRemote,
		ForwardedRemoteToLocal
	}

	public abstract class SSHChannel {
		protected ChannelType _type;
		protected int _localID;
		protected int _remoteID;
		protected SSHConnection _connection;

		protected SSHChannel(SSHConnection con, ChannelType type, int local_id) {
			con.RegisterChannel(local_id, this);
			_connection = con;
			_type = type;
			_localID = local_id;
		}

		public int LocalChannelID {
			get {
				return _localID;
			}
		}
		public int RemoteChannelID {
			get {
				return _remoteID;
			}
		}
		public SSHConnection Connection {
			get {
				return _connection;
			}
		}
		public ChannelType Type {
			get {
				return _type;
			}
		}

		/**
		 * resizes the size of terminal
		 */
		public abstract void ResizeTerminal(int width, int height, int pixel_width, int pixel_height);

		/**
		* transmits channel data 
		*/
		public abstract void Transmit(byte[] data);

		/**
		* transmits channel data 
		*/
		public abstract void Transmit(byte[] data, int offset, int length);

		/**
		 * sends EOF(SSH2 only)
		 */
		public abstract void SendEOF();

		/**
		 * closes this channel
		 */
		public abstract void Close();


	}
}
