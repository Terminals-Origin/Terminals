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
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;

using Routrek.PKI;
using Routrek.SSHC;
using Routrek.Toolkit;

namespace Routrek.SSHCV2
{
	public sealed class SSH2Connection : SSHConnection {
	
		//packet count for transmission and reception
		private int _tSequence;

		//MAC for transmission and reception
		private MAC _tMAC;
		private SSH2PacketBuilder _packetBuilder;

		//server info
		private SSH2ConnectionInfo _cInfo;

		private bool _waitingForPortForwardingResponse;

		private KeyExchanger _asyncKeyExchanger;

		public SSH2Connection(SSHConnectionParameter param, ISSHConnectionEventReceiver r, string serverversion, string clientversion) : base(param, r) {
			_cInfo = new SSH2ConnectionInfo();
			_cInfo._serverVersionString = serverversion;
			_cInfo._clientVersionString = clientversion;
			
			_packetBuilder = new SSH2PacketBuilder(new SynchronizedSSH2PacketHandler());
		}
		internal override IByteArrayHandler PacketBuilder {
			get {
				return _packetBuilder;
			}
		}
		public override SSHConnectionInfo ConnectionInfo {
			get {
				return _cInfo;
			}
		}

		internal override AuthenticationResult Connect(AbstractSocket s) {
			_stream = s;
			
			KeyExchanger kex = new KeyExchanger(this, null);
			if(!kex.SynchronousKexExchange()) {
				_stream.Close();
				return AuthenticationResult.Failure;
			}
			//Step3 user authentication
			ServiceRequest("ssh-userauth");
			_authenticationResult = UserAuth();
			return _authenticationResult;
		}


		private void ServiceRequest(string servicename) {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_SERVICE_REQUEST);
			wr.Write(servicename);
			TransmitPacket(wr.ToByteArray());

			byte[] response = ReceivePacket().Data;
			SSH2DataReader re = new SSH2DataReader(response);
			PacketType t = re.ReadPacketType();
			if(t!=PacketType.SSH_MSG_SERVICE_ACCEPT) {
				throw new SSHException("service establishment failed "+t);
			}

			string s = Encoding.ASCII.GetString(re.ReadString());
			if(servicename!=s)
				throw new SSHException("protocol error");
		}

		private AuthenticationResult UserAuth() {
			string sn = "ssh-connection"; 

			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_USERAUTH_REQUEST);
			wr.Write(_param.UserName);
			if(_param.AuthenticationType==AuthenticationType.Password) {
				//Password authentication
				wr.Write(sn); 
				wr.Write("password");
				wr.Write(false);
				wr.Write(_param.Password);
			}
			else if(_param.AuthenticationType==AuthenticationType.KeyboardInteractive) {
				wr.Write(sn); 
				wr.Write("keyboard-interactive");
				wr.Write(""); //lang
				wr.Write(""); //submethod
			}
			else {
				//public key authentication
				SSH2UserAuthKey kp = SSH2UserAuthKey.FromSECSHStyleFile(_param.IdentityFile, _param.Password);
				SSH2DataWriter signsource = new SSH2DataWriter();
				signsource.WriteAsString(_sessionID);
				signsource.WritePacketType(PacketType.SSH_MSG_USERAUTH_REQUEST);
				signsource.Write(_param.UserName);
				signsource.Write(sn);
				signsource.Write("publickey");
				signsource.Write(true);
				signsource.Write(SSH2Util.PublicKeyAlgorithmName(kp.Algorithm));
				signsource.WriteAsString(kp.GetPublicKeyBlob());

				SSH2DataWriter signpack = new SSH2DataWriter();
				signpack.Write(SSH2Util.PublicKeyAlgorithmName(kp.Algorithm));
				signpack.WriteAsString(kp.Sign(signsource.ToByteArray()));

				wr.Write(sn);
				wr.Write("publickey");
				wr.Write(true);
				wr.Write(SSH2Util.PublicKeyAlgorithmName(kp.Algorithm));
				wr.WriteAsString(kp.GetPublicKeyBlob());
				wr.WriteAsString(signpack.ToByteArray());
			}
			TransmitPacket(wr.ToByteArray());

			_authenticationResult = ProcessAuthenticationResponse();
			if(_authenticationResult==AuthenticationResult.Failure)
				throw new SSHException(Strings.GetString("AuthenticationFailed"));
			return _authenticationResult;
		}
		private AuthenticationResult ProcessAuthenticationResponse() {
			do {
				SSH2DataReader response = new SSH2DataReader(ReceivePacket().Data);
				PacketType h = response.ReadPacketType();
				if(h==PacketType.SSH_MSG_USERAUTH_FAILURE) {
					string msg = Encoding.ASCII.GetString(response.ReadString());
					return AuthenticationResult.Failure;
				}
				else if(h==PacketType.SSH_MSG_USERAUTH_BANNER) {
					Debug.WriteLine("USERAUTH_BANNER");
				}
				else if(h==PacketType.SSH_MSG_USERAUTH_SUCCESS) {
					_packetBuilder.Handler = new CallbackSSH2PacketHandler(this);
					return AuthenticationResult.Success; //successfully exit
				}
				else if(h==PacketType.SSH_MSG_USERAUTH_INFO_REQUEST) {
					string name = Encoding.ASCII.GetString(response.ReadString());
					string inst = Encoding.ASCII.GetString(response.ReadString());
					string lang = Encoding.ASCII.GetString(response.ReadString());
					int num = response.ReadInt32();
					string[] prompts = new string[num];
					for(int i=0; i<num; i++) {
						prompts[i] = Encoding.ASCII.GetString(response.ReadString());
						bool echo = response.ReadBool();
					}
					_eventReceiver.OnAuthenticationPrompt(prompts);
					return AuthenticationResult.Prompt;
				}
				else
					throw new SSHException("protocol error: unexpected packet type "+h);
			} while(true);
		}
		public AuthenticationResult DoKeyboardInteractiveAuth(string[] input) {
			if(_param.AuthenticationType!=AuthenticationType.KeyboardInteractive)
				throw new SSHException("DoKeyboardInteractiveAuth() must be called with keyboard-interactive authentication");
			SSH2DataWriter re = new SSH2DataWriter();
			re.WritePacketType(PacketType.SSH_MSG_USERAUTH_INFO_RESPONSE);
			re.Write(input.Length);
			foreach(string t in input)
				re.Write(t);
			TransmitPacket(re.ToByteArray());

			_authenticationResult = ProcessAuthenticationResponse();
			//try again on failure
			if(_authenticationResult==AuthenticationResult.Failure) {
				SSH2DataWriter wr = new SSH2DataWriter();
				wr.WritePacketType(PacketType.SSH_MSG_USERAUTH_REQUEST);
				wr.Write(_param.UserName);
				wr.Write("ssh-connection"); 
				wr.Write("keyboard-interactive");
				wr.Write(""); //lang
				wr.Write(""); //submethod
				TransmitPacket(wr.ToByteArray());
				_authenticationResult = ProcessAuthenticationResponse();
			}
			return _authenticationResult;
		}
		
		public override SSHChannel OpenShell(ISSHChannelEventReceiver receiver) {
			//open channel
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN);
			wr.Write("session");
			int local_channel = this.RegisterChannelEventReceiver(null, receiver)._localID;

			wr.Write(local_channel);
			wr.Write(_param.WindowSize); //initial window size
			int windowsize = _param.WindowSize;
			wr.Write(_param.MaxPacketSize); //max packet size
			SSH2Channel channel = new SSH2Channel(this, ChannelType.Shell, local_channel);
			TransmitPacket(wr.ToByteArray());
			
			return channel;
		}

		public override SSHChannel ForwardPort(ISSHChannelEventReceiver receiver, string remote_host, int remote_port, string originator_host, int originator_port) {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN);
			wr.Write("direct-tcpip");
			int local_id = RegisterChannelEventReceiver(null, receiver)._localID;
			wr.Write(local_id);
			wr.Write(_param.WindowSize); //initial window size
			int windowsize = _param.WindowSize;
			wr.Write(_param.MaxPacketSize); //max packet size
			wr.Write(remote_host);
			wr.Write(remote_port);
			wr.Write(originator_host);
			wr.Write(originator_port);

			SSH2Channel channel = new SSH2Channel(this, ChannelType.ForwardedLocalToRemote, local_id);
			
			TransmitPacket(wr.ToByteArray());
			
			return channel;
		}

		public override void ListenForwardedPort(string allowed_host, int bind_port) {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_GLOBAL_REQUEST);
			wr.Write("tcpip-forward");
			wr.Write(true);
			wr.Write(allowed_host);
			wr.Write(bind_port);

			_waitingForPortForwardingResponse = true;
			TransmitPacket(wr.ToByteArray());
		}

		public override void CancelForwardedPort(string host, int port) {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_GLOBAL_REQUEST);
			wr.Write("cancel-tcpip-forward");
			wr.Write(true);
			wr.Write(host);
			wr.Write(port);
			TransmitPacket(wr.ToByteArray());
		}

		private void ProcessPortforwardingRequest(ISSHConnectionEventReceiver receiver, SSH2DataReader reader) {
			string method = Encoding.ASCII.GetString(reader.ReadString());

			int remote_channel = reader.ReadInt32();
			int window_size = reader.ReadInt32(); //skip initial window size
			int servermaxpacketsize = reader.ReadInt32();
			string host = Encoding.ASCII.GetString(reader.ReadString());
			int port = reader.ReadInt32();
			string originator_ip = Encoding.ASCII.GetString(reader.ReadString());
			int originator_port = reader.ReadInt32();
			
			PortForwardingCheckResult r = receiver.CheckPortForwardingRequest(host,port,originator_ip,originator_port);
			SSH2DataWriter wr = new SSH2DataWriter();
			if(r.allowed) {
				//send OPEN_CONFIRMATION
				SSH2Channel channel = new SSH2Channel(this, ChannelType.ForwardedRemoteToLocal, RegisterChannelEventReceiver(null, r.channel)._localID, remote_channel, servermaxpacketsize);
				wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION);
				wr.Write(remote_channel);
				wr.Write(channel.LocalChannelID);
				wr.Write(_param.WindowSize); //initial window size
				wr.Write(_param.MaxPacketSize); //max packet size
				receiver.EstablishPortforwarding(r.channel, channel);
			}
			else {
				wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE);
				wr.Write(remote_channel);
				wr.Write(r.reason_code);
				wr.Write(r.reason_message);
				wr.Write(""); //lang tag
			}
			TransmitPacket(wr.ToByteArray());
		}

		internal SSH2Packet TransmitPacket(byte[] payload) {
			lock(_tLockObject) { 
				SSH2Packet p = SSH2Packet.FromPlainPayload(payload, _tCipher==null? 8 : _tCipher.BlockSize, _param.Random);
				if(_tMAC!=null) p.CalcHash(_tMAC, _tSequence);

				_tSequence++;
				p.WriteTo(_stream, _tCipher);
				return p;
			}
		}

		//synchronous reception
		internal SSH2Packet ReceivePacket() {

			while(true) {
				SSH2Packet p = null;
				SynchronizedSSH2PacketHandler handler = (SynchronizedSSH2PacketHandler)_packetBuilder.Handler;
				if(!handler.HasPacket) {
					handler.Wait();
					if(handler.State==ReceiverState.Error)
						throw new SSHException(handler.ErrorMessage);
					else if(handler.State==ReceiverState.Closed)
						throw new SSHException("socket closed");
				}
				p = handler.PopPacket();

				SSH2DataReader r = new SSH2DataReader(p.Data);
				PacketType pt = r.ReadPacketType();
				if(pt==PacketType.SSH_MSG_IGNORE) {
					if(_eventReceiver!=null) _eventReceiver.OnIgnoreMessage(r.ReadString());
				}
				else if(pt==PacketType.SSH_MSG_DEBUG) {
					bool f = r.ReadBool();
					if(_eventReceiver!=null) _eventReceiver.OnDebugMessage(f, r.ReadString());
				}
				else
					return p;
			}
		}
		internal void AsyncReceivePacket(SSH2Packet packet) {
			try {
				ProcessPacket(packet);
			}
			catch(Exception ex) {
                //Debug.WriteLine(ex.StackTrace);
				if(!_closed)
					_eventReceiver.OnError(ex, ex.Message);
			}
		}

		private bool ProcessPacket(SSH2Packet packet) {
			SSH2DataReader r = new SSH2DataReader(packet.Data);
			PacketType pt = r.ReadPacketType();
			//Debug.WriteLine("ProcessPacket pt="+pt);

			if(pt==PacketType.SSH_MSG_DISCONNECT) {
				int errorcode = r.ReadInt32();
				//string description = Encoding.ASCII.GetString(r.ReadString());
				_eventReceiver.OnConnectionClosed();
				return false;
			}
			else if(_waitingForPortForwardingResponse) {
				if(pt!=PacketType.SSH_MSG_REQUEST_SUCCESS)
					_eventReceiver.OnUnknownMessage((byte)pt, r.Image);
				_waitingForPortForwardingResponse = false;
				return true;
			}
			else if(pt==PacketType.SSH_MSG_CHANNEL_OPEN) {
				ProcessPortforwardingRequest(_eventReceiver, r);
				return true;
			}
			else if(pt>=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION && pt<=PacketType.SSH_MSG_CHANNEL_FAILURE) {
				int local_channel = r.ReadInt32();
				ChannelEntry e = FindChannelEntry(local_channel);
				if(e!=null) //throw new SSHException("Unknown channel "+local_channel);
					((SSH2Channel)e._channel).ProcessPacket(e._receiver, pt, 5+r.Rest, r);
				else
					Debug.WriteLine("unexpected channel pt="+pt+" local_channel="+local_channel.ToString());
				return true;
			}
			else if(pt==PacketType.SSH_MSG_IGNORE) {
				_eventReceiver.OnIgnoreMessage(r.ReadString());
				return true;
			}
			else if(_asyncKeyExchanger!=null) {
				_asyncKeyExchanger.AsyncProcessPacket(packet);
				return true;
			}
			else if(pt==PacketType.SSH_MSG_KEXINIT) {
				Debug.WriteLine("Host sent KEXINIT");
				_asyncKeyExchanger = new KeyExchanger(this, _sessionID);
				_asyncKeyExchanger.AsyncProcessPacket(packet);
				return true;
			}
			else {
				_eventReceiver.OnUnknownMessage((byte)pt, r.Image);
				return false;
			}
		}

		public override void Disconnect(string msg) {
			if(_closed) return;
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_DISCONNECT);
			wr.Write(0);
			wr.Write(msg);
			wr.Write(""); //language
			TransmitPacket(wr.ToByteArray());
			_stream.Flush();
			_closed = true;
			_stream.Close();
		}

		public override void Close() {
			if(_closed) return;
			_closed = true;
			_stream.Close();
		} 
		public override void SendIgnorableData(string msg) {
			SSH2DataWriter w = new SSH2DataWriter();
			w.WritePacketType(PacketType.SSH_MSG_IGNORE);
			w.Write(msg);
			TransmitPacket(w.ToByteArray());
		}
		public void ReexchangeKeys() {
			_asyncKeyExchanger = new KeyExchanger(this, _sessionID);
			_asyncKeyExchanger.AsyncStartReexchange();
		}

		internal void LockCommunication() {
			_packetBuilder.SetSignal(false);
		}
		internal void UnlockCommunication() {
			_packetBuilder.SetSignal(true);
		}
		internal void RefreshKeys(byte[] sessionID, Cipher tc, Cipher rc, MAC tm, MAC rm) {
			_sessionID = sessionID;
			_tCipher = tc;
			_tMAC = tm;
			_packetBuilder.SetCipher(rc, _param.CheckMACError? rm : null);
			_asyncKeyExchanger = null;
		}

	}

	public class SSH2Channel : SSHChannel {
		//channel property
		protected int _windowSize;
		protected int _leftWindowSize;
		protected int _serverMaxPacketSize;

		//negotiation status
		protected int _negotiationStatus;

		public SSH2Channel(SSHConnection con, ChannelType type, int local_id) : base(con, type, local_id) {
			_windowSize = _leftWindowSize = con.Param.WindowSize;
			_negotiationStatus = type==ChannelType.Shell? 3 : type==ChannelType.ForwardedLocalToRemote? 1 : type==ChannelType.Session? 1 : 0;
		}
		public SSH2Channel(SSHConnection con, ChannelType type, int local_id, int remote_id, int maxpacketsize) : base(con, type, local_id) {
			_windowSize = _leftWindowSize = con.Param.WindowSize;
			Debug.Assert(type==ChannelType.ForwardedRemoteToLocal);
			_remoteID = remote_id;
			_serverMaxPacketSize = maxpacketsize;
			_negotiationStatus = 0;
		}

		public override void ResizeTerminal(int width, int height, int pixel_width, int pixel_height) {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
			wr.Write(_remoteID);
			wr.Write("window-change");
			wr.Write(false);
			wr.Write(width);
			wr.Write(height);
			wr.Write(pixel_width); //no graphics
			wr.Write(pixel_height);
			TransmitPacket(wr.ToByteArray());
		}
		public override void Transmit(byte[] data)	{
			//!!it is better idea that we wait a WINDOW_ADJUST if the left size is lack
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_DATA);
			wr.Write(_remoteID);
			wr.WriteAsString(data);

			TransmitPacket(wr.ToByteArray());
		}
		public override void Transmit(byte[] data, int offset, int length)	{
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_DATA);
			wr.Write(_remoteID);
			wr.WriteAsString(data, offset, length);

			TransmitPacket(wr.ToByteArray());
		}

		public override void SendEOF() {
			if(_connection.IsClosed) return;
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_EOF);
			wr.Write(_remoteID);
			TransmitPacket(wr.ToByteArray());
		}


		public override void Close() {
			if(_connection.IsClosed) return;
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_CLOSE);
			wr.Write(_remoteID);
			TransmitPacket(wr.ToByteArray());

		}

		//maybe this is SSH2 only feature
		public void SetEnvironmentVariable(string name, string value) {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
			wr.Write(_remoteID);
			wr.Write("env");
			wr.Write(false);
			wr.Write(name);
			wr.Write(value);
			TransmitPacket(wr.ToByteArray());
		}
		public void SendBreak(int time) {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
			wr.Write(_remoteID);
			wr.Write("break");
			wr.Write(true);
			wr.Write(time);
			TransmitPacket(wr.ToByteArray());
		}

		internal void ProcessPacket(ISSHChannelEventReceiver receiver, PacketType pt, int data_length, SSH2DataReader re) {

			//NOTE: the offset of 're' is next to 'receipiant channel' field
			_leftWindowSize -= data_length;
			while(_leftWindowSize <= _windowSize) {
				SSH2DataWriter adj = new SSH2DataWriter();
				adj.WritePacketType(PacketType.SSH_MSG_CHANNEL_WINDOW_ADJUST);
				adj.Write(_remoteID);
				adj.Write(_windowSize);
				TransmitPacket(adj.ToByteArray());
				_leftWindowSize += _windowSize;
				//Debug.WriteLine("Window size is adjusted to " + _leftWindowSize);
			}

			if(pt==PacketType.SSH_MSG_CHANNEL_WINDOW_ADJUST) {
				int w = re.ReadInt32();
				//Debug.WriteLine(String.Format("Window Adjust +={0}",w));
			}
			else if(_negotiationStatus!=0) { //when the negotiation is not completed
				if(_type==ChannelType.Shell)
					OpenShell(receiver, pt, re);
				else if(_type==ChannelType.ForwardedLocalToRemote)
					ReceivePortForwardingResponse(receiver, pt, re);
				else if(_type==ChannelType.Session)
					EstablishSession(receiver, pt, re);
			}
			else {
				switch(pt) {
					case PacketType.SSH_MSG_CHANNEL_DATA: {
						int len = re.ReadInt32();
						receiver.OnData(re.Image, re.Offset, len);
					}
						break;
					case PacketType.SSH_MSG_CHANNEL_EXTENDED_DATA: {
						int t = re.ReadInt32();
						byte[] data = re.ReadString();
						receiver.OnExtendedData(t, data);
					}
						break;
					case PacketType.SSH_MSG_CHANNEL_REQUEST: {
						string request = Encoding.ASCII.GetString(re.ReadString());
						bool reply = re.ReadBool();
						if(request=="exit-status") {
							int status = re.ReadInt32();
						}
						else if(reply) { //we reject unknown requests including keep-alive check
							SSH2DataWriter wr = new SSH2DataWriter();
							wr.Write((byte)PacketType.SSH_MSG_CHANNEL_FAILURE);
							wr.Write(_remoteID);
							TransmitPacket(wr.ToByteArray());
						}
					}
						break;
					case PacketType.SSH_MSG_CHANNEL_EOF:
						receiver.OnChannelEOF();
						break;
					case PacketType.SSH_MSG_CHANNEL_CLOSE:
						_connection.UnregisterChannelEventReceiver(_localID);
						receiver.OnChannelClosed();
						break;
					case PacketType.SSH_MSG_CHANNEL_FAILURE:
					case PacketType.SSH_MSG_CHANNEL_SUCCESS:
						receiver.OnMiscPacket((byte)pt, re.Image, re.Offset, re.Rest);
						break;
					default:
						receiver.OnMiscPacket((byte)pt, re.Image, re.Offset, re.Rest);
						Debug.WriteLine("Unknown Packet "+pt);
						break;
				}			

			}

		}

		private void TransmitPacket(byte[] data) {
			((SSH2Connection)_connection).TransmitPacket(data);
		}

		private void OpenShell(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader reader) {
			if(_negotiationStatus==3) {
				if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION) {
					if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE)
						receiver.OnChannelError(null, "opening channel failed; packet type="+pt);
					else {
						int errcode = reader.ReadInt32();
						string msg = Encoding.ASCII.GetString(reader.ReadString());
						receiver.OnChannelError(null, msg);
					}
					Close();
				}
				else {
					_remoteID = reader.ReadInt32();
					_serverMaxPacketSize = reader.ReadInt32();
				
					//open pty
					SSH2DataWriter wr = new SSH2DataWriter();
					wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
					wr.Write(_remoteID);
					wr.Write("pty-req");
					wr.Write(true);
					wr.Write(_connection.Param.TerminalName);
					wr.Write(_connection.Param.TerminalWidth);
					wr.Write(_connection.Param.TerminalHeight);
					wr.Write(_connection.Param.TerminalPixelWidth);
					wr.Write(_connection.Param.TerminalPixelHeight);
					wr.WriteAsString(new byte[0]);
					TransmitPacket(wr.ToByteArray());

					_negotiationStatus = 2;
				}
			}
			else if(_negotiationStatus==2) {
				if(pt!=PacketType.SSH_MSG_CHANNEL_SUCCESS) {
					receiver.OnChannelError(null, "opening pty failed");
					Close();
				}
				else {
					//open shell
					SSH2DataWriter wr = new SSH2DataWriter();
					wr.Write((byte)PacketType.SSH_MSG_CHANNEL_REQUEST);
					wr.Write(_remoteID);
					wr.Write("shell");
					wr.Write(true);
					TransmitPacket(wr.ToByteArray());

					_negotiationStatus = 1;
				}
			}
			else if(_negotiationStatus==1) {
				if(pt!=PacketType.SSH_MSG_CHANNEL_SUCCESS) {
					receiver.OnChannelError(null, "Opening shell failed: packet type="+pt.ToString());
					Close();
				}
				else {
					receiver.OnChannelReady();
					_negotiationStatus = 0; //goal!
				}
			}
			else
				Debug.Assert(false);
		}

		private void ReceivePortForwardingResponse(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader reader) {
			if(_negotiationStatus==1) {
				if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION) {
					if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE)
						receiver.OnChannelError(null, "opening channel failed; packet type="+pt);
					else {
						int errcode = reader.ReadInt32();
						string msg = Encoding.ASCII.GetString(reader.ReadString());
						receiver.OnChannelError(null, msg);
					}
					Close();
				}
				else {
					_remoteID = reader.ReadInt32();
					_serverMaxPacketSize = reader.ReadInt32();
					_negotiationStatus = 0;
					receiver.OnChannelReady();
				}
			}
			else
				Debug.Assert(false);
		}
		private void EstablishSession(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader reader) {
			if(_negotiationStatus==1) {
				if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION) {
					if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE)
						receiver.OnChannelError(null, "opening channel failed; packet type="+pt);
					else {
						int remote_id = reader.ReadInt32();
						int errcode = reader.ReadInt32();
						string msg = Encoding.ASCII.GetString(reader.ReadString());
						receiver.OnChannelError(null, msg);
					}
					Close();
				}
				else {
					_remoteID = reader.ReadInt32();
					_serverMaxPacketSize = reader.ReadInt32();
					_negotiationStatus = 0;
					receiver.OnChannelReady();
				}
			}
			else
				Debug.Assert(false);
		}
	}

	internal class KeyExchanger {
		private SSH2Connection _con;
		private SSHConnectionParameter _param;
		private SSH2ConnectionInfo _cInfo;
		//payload of KEXINIT message
		private byte[] _serverKEXINITPayload;
		private byte[] _clientKEXINITPayload;

		//true if the host sent KEXINIT first
		private bool _startedByHost;

		private ManualResetEvent _newKeyEvent;

		//status
		private enum Status {
			INITIAL,
			WAIT_KEXINIT,
			WAIT_KEXDH_REPLY,
			WAIT_NEWKEYS,
			FINISHED
		}
		private Status _status;

		private BigInteger _x;
		private BigInteger _e;
		private BigInteger _k;
		private byte[] _hash;
		private byte[] _sessionID;

		//results
		Cipher _rc;
		Cipher _tc;
		MAC _rm;
		MAC _tm;

		private void TransmitPacket(byte[] payload) {
			_con.TransmitPacket(payload);
		}

		public KeyExchanger(SSH2Connection con, byte[] sessionID) {
			_con = con;
			_param = con.Param;
			_cInfo = (SSH2ConnectionInfo)con.ConnectionInfo;
			_sessionID = sessionID;
			_status = Status.INITIAL;
		}

		public bool SynchronousKexExchange() {
			SendKEXINIT();
			ProcessKEXINIT(_con.ReceivePacket());
			SendKEXDHINIT();
			if(!ProcessKEXDHREPLY(_con.ReceivePacket())) return false;
			SendNEWKEYS();
			ProcessNEWKEYS(_con.ReceivePacket());
			return true;
		}
		public void AsyncStartReexchange() {
			_startedByHost = false;
			_status = Status.WAIT_KEXINIT;
			SendKEXINIT();
		}
		public void AsyncProcessPacket(SSH2Packet packet) {
			switch(_status) {
				case Status.INITIAL:
					_startedByHost = true;
					ProcessKEXINIT(packet);
					SendKEXINIT();
					SendKEXDHINIT();
					break;
				case Status.WAIT_KEXINIT:
					ProcessKEXINIT(packet);
					SendKEXDHINIT();
					break;
				case Status.WAIT_KEXDH_REPLY:
					ProcessKEXDHREPLY(packet);
					SendNEWKEYS();
					break;
				case Status.WAIT_NEWKEYS:
					ProcessNEWKEYS(packet);
					Debug.Assert(_status==Status.FINISHED);
					break;
			}
		}
					
		private void SendKEXINIT() {
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_KEXINIT);
			byte[] cookie = new byte[16];
			_param.Random.NextBytes(cookie);
			wr.Write(cookie);
			wr.Write("diffie-hellman-group1-sha1"); //    kex_algorithms
			wr.Write(FormatHostKeyAlgorithmDescription());            //    server_host_key_algorithms
			wr.Write(FormatCipherAlgorithmDescription());      //    encryption_algorithms_client_to_server
			wr.Write(FormatCipherAlgorithmDescription());      //    encryption_algorithms_server_to_client
			wr.Write("hmac-sha1");                  //    mac_algorithms_client_to_server
			wr.Write("hmac-sha1");                  //    mac_algorithms_server_to_client
			wr.Write("none");                       //    compression_algorithms_client_to_server
			wr.Write("none");                       //    compression_algorithms_server_to_client
			wr.Write("");                           //    languages_client_to_server
			wr.Write("");                           //    languages_server_to_client
			wr.Write(false); //Indicates whether a guessed key exchange packet follows
			wr.Write(0);       //reserved for future extension

			_clientKEXINITPayload = wr.ToByteArray();
			_status = Status.WAIT_KEXINIT;
			TransmitPacket(_clientKEXINITPayload);
		}
		private void ProcessKEXINIT(SSH2Packet packet) {
			_serverKEXINITPayload = packet.Data;
			SSH2DataReader re = new SSH2DataReader(_serverKEXINITPayload);
			byte[] head = re.Read(17); //Type and cookie
			if(head[0]!=(byte)PacketType.SSH_MSG_KEXINIT) throw new SSHException(String.Format("Server response is not SSH_MSG_KEXINIT but {0}", head[0]));
			Encoding enc = Encoding.ASCII;
			
			string kex = enc.GetString(re.ReadString());
			_cInfo._supportedKEXAlgorithms = kex;
			CheckAlgorithmSupport("keyexchange", kex, "diffie-hellman-group1-sha1");
			
			string host_key = enc.GetString(re.ReadString());
			_cInfo._supportedHostKeyAlgorithms = host_key;
			_cInfo._algorithmForHostKeyVerification = DecideHostKeyAlgorithm(host_key);
			
			string enc_cs = enc.GetString(re.ReadString());
			_cInfo._supportedCipherAlgorithms = enc_cs;
			_cInfo._algorithmForTransmittion = DecideCipherAlgorithm(enc_cs);
			
			string enc_sc = enc.GetString(re.ReadString());
			_cInfo._algorithmForReception = DecideCipherAlgorithm(enc_sc);

			string mac_cs = enc.GetString(re.ReadString());
			CheckAlgorithmSupport("mac", mac_cs, "hmac-sha1");
			
			string mac_sc = enc.GetString(re.ReadString());
			CheckAlgorithmSupport("mac", mac_sc, "hmac-sha1");
			
			string comp_cs = enc.GetString(re.ReadString());
			CheckAlgorithmSupport("compression", comp_cs, "none");
			string comp_sc = enc.GetString(re.ReadString());
			CheckAlgorithmSupport("compression", comp_sc, "none");
			
			string lang_cs = enc.GetString(re.ReadString());
			string lang_sc = enc.GetString(re.ReadString());
			bool flag = re.ReadBool();
			int reserved = re.ReadInt32();
			Debug.Assert(re.Rest==0);
			if(flag) throw new SSHException("Algorithm negotiation failed"); 
		}


		private void SendKEXDHINIT() {
			//Round1 computes and sends [e]
			byte[] sx = new byte[16];
			_param.Random.NextBytes(sx);
			_x = new BigInteger(sx);
			_e = new BigInteger(2).modPow(_x, DH_PRIME);
			SSH2DataWriter wr = new SSH2DataWriter();
			wr.WritePacketType(PacketType.SSH_MSG_KEXDH_INIT);
			wr.Write(_e);
			_status = Status.WAIT_KEXDH_REPLY;
			TransmitPacket(wr.ToByteArray());
		}
		private bool ProcessKEXDHREPLY(SSH2Packet packet) {
			//Round2 receives response
			SSH2DataReader re = new SSH2DataReader(packet.Data);
			PacketType h = re.ReadPacketType();
			if(h!=PacketType.SSH_MSG_KEXDH_REPLY) throw new SSHException(String.Format("KeyExchange response is not KEXDH_REPLY but {0}", h));
			byte[] key_and_cert = re.ReadString();
			BigInteger f = re.ReadMPInt();
			byte[] signature = re.ReadString();
			Debug.Assert(re.Rest==0);

			//Round3 calc hash H
			SSH2DataWriter wr = new SSH2DataWriter();
			_k = f.modPow(_x, DH_PRIME);
			wr = new SSH2DataWriter();
			wr.Write(_cInfo._clientVersionString);
			wr.Write(_cInfo._serverVersionString);
			wr.WriteAsString(_clientKEXINITPayload);
			wr.WriteAsString(_serverKEXINITPayload);
			wr.WriteAsString(key_and_cert);
			wr.Write(_e);
			wr.Write(f);
			wr.Write(_k);
			_hash = new SHA1CryptoServiceProvider().ComputeHash(wr.ToByteArray());

			if(!VerifyHostKey(key_and_cert, signature, _hash)) return false;

			//Debug.WriteLine("hash="+DebugUtil.DumpByteArray(hash));
			if(_sessionID==null) _sessionID = _hash;
			return true;
		}
		private void SendNEWKEYS() {
			_status = Status.WAIT_NEWKEYS;
			_newKeyEvent = new ManualResetEvent(false);
			TransmitPacket(new byte[1] {(byte)PacketType.SSH_MSG_NEWKEYS});

			//establish Ciphers
			_tc = CipherFactory.CreateCipher(SSHProtocol.SSH2, _cInfo._algorithmForTransmittion,
				DeriveKey(_k, _hash, 'C', CipherFactory.GetKeySize(_cInfo._algorithmForTransmittion)), DeriveKey(_k, _hash, 'A', CipherFactory.GetBlockSize(_cInfo._algorithmForTransmittion)));
			_rc = CipherFactory.CreateCipher(SSHProtocol.SSH2, _cInfo._algorithmForReception,
				DeriveKey(_k, _hash, 'D', CipherFactory.GetKeySize(_cInfo._algorithmForReception)), DeriveKey(_k, _hash, 'B', CipherFactory.GetBlockSize(_cInfo._algorithmForReception)));

			//establish MACs
			MACAlgorithm ma = MACAlgorithm.HMACSHA1;
			_tm = MACFactory.CreateMAC(MACAlgorithm.HMACSHA1, DeriveKey(_k, _hash, 'E', MACFactory.GetSize(ma)));
			_rm = MACFactory.CreateMAC(MACAlgorithm.HMACSHA1, DeriveKey(_k, _hash, 'F', MACFactory.GetSize(ma)));
			_newKeyEvent.Set();
		}
		private void ProcessNEWKEYS(SSH2Packet packet) {
			//confirms new key
			try {
				byte[] response = packet.Data;
				if(response.Length!=1 || response[0]!=(byte)PacketType.SSH_MSG_NEWKEYS) throw new SSHException("SSH_MSG_NEWKEYS failed");

				_newKeyEvent.WaitOne();
				_newKeyEvent.Close();
				_con.LockCommunication();
				_con.RefreshKeys(_sessionID, _tc, _rc, _tm, _rm);
				_status = Status.FINISHED;
			}
			finally {
				_con.UnlockCommunication();
			}
		}

		private bool VerifyHostKey(byte[] K_S, byte[] signature, byte[] hash) {
			SSH2DataReader re1 = new SSH2DataReader(K_S);
			string algorithm = Encoding.ASCII.GetString(re1.ReadString());
			if(algorithm!=SSH2Util.PublicKeyAlgorithmName(_cInfo._algorithmForHostKeyVerification))
				throw new SSHException("Protocol Error: Host Key Algorithm Mismatch");

			SSH2DataReader re2 = new SSH2DataReader(signature);
			algorithm = Encoding.ASCII.GetString(re2.ReadString());
			if(algorithm!=SSH2Util.PublicKeyAlgorithmName(_cInfo._algorithmForHostKeyVerification))
				throw new SSHException("Protocol Error: Host Key Algorithm Mismatch");
			byte[] sigbody = re2.ReadString();
			Debug.Assert(re2.Rest==0);

			if(_cInfo._algorithmForHostKeyVerification==PublicKeyAlgorithm.RSA)
				VerifyHostKeyByRSA(re1, sigbody, hash);
			else if(_cInfo._algorithmForHostKeyVerification==PublicKeyAlgorithm.DSA)
				VerifyHostKeyByDSS(re1, sigbody, hash);
			else
				throw new SSHException("Bad host key algorithm "+_cInfo._algorithmForHostKeyVerification);

			//ask the client whether he accepts the host key
			if(!_startedByHost && _param.KeyCheck!=null && !_param.KeyCheck(_cInfo))
				return false;
			else
				return true;
		}

		private void VerifyHostKeyByRSA(SSH2DataReader pubkey, byte[] sigbody, byte[] hash) {
			BigInteger exp = pubkey.ReadMPInt();
			BigInteger mod = pubkey.ReadMPInt();
			Debug.Assert(pubkey.Rest==0);

			//Debug.WriteLine(exp.ToHexString());
			//Debug.WriteLine(mod.ToHexString());

			RSAPublicKey pk = new RSAPublicKey(exp, mod);
			pk.VerifyWithSHA1(sigbody, new SHA1CryptoServiceProvider().ComputeHash(hash));
			_cInfo._hostkey = pk;
		}

		private void VerifyHostKeyByDSS(SSH2DataReader pubkey, byte[] sigbody, byte[] hash) {
			BigInteger p = pubkey.ReadMPInt();
			BigInteger q = pubkey.ReadMPInt();
			BigInteger g = pubkey.ReadMPInt();
			BigInteger y = pubkey.ReadMPInt();
			Debug.Assert(pubkey.Rest==0);

			//Debug.WriteLine(p.ToHexString());
			//Debug.WriteLine(q.ToHexString());
			//Debug.WriteLine(g.ToHexString());
			//Debug.WriteLine(y.ToHexString());


			DSAPublicKey pk = new DSAPublicKey(p,g,q,y);
			pk.Verify(sigbody, new SHA1CryptoServiceProvider().ComputeHash(hash));
			_cInfo._hostkey = pk;
		}

		private byte[] DeriveKey(BigInteger key, byte[] hash, char ch, int length) {
			byte[] result = new byte[length];

			SSH2DataWriter wr = new SSH2DataWriter();
			wr.Write(key);
			wr.Write(hash);
			wr.Write((byte)ch);
			wr.Write(_sessionID);
			byte[] h1 = new SHA1CryptoServiceProvider().ComputeHash(wr.ToByteArray());
			if(h1.Length >= length) {
				Array.Copy(h1, 0, result, 0, length);
				return result;
			}
			else {
				wr = new SSH2DataWriter();
				wr.Write(key);
				wr.Write(_sessionID);
				wr.Write(h1);
				byte[] h2 = new SHA1CryptoServiceProvider().ComputeHash(wr.ToByteArray());
				if(h1.Length+h2.Length >= length) {
					Array.Copy(h1, 0, result, 0, h1.Length);
					Array.Copy(h2, 0, result, h1.Length, length-h1.Length);
					return result;
				}
				else
					throw new SSHException("necessary key length is too big"); //long key is not supported
			}
		}

		private static void CheckAlgorithmSupport(string title, string data, string algorithm_name) {
			string[] t = data.Split(',');
			foreach(string s in t) {
				if(s==algorithm_name) return; //found!
			}
			throw new SSHException("Server does not support "+algorithm_name+" for "+title);
		}
		private PublicKeyAlgorithm DecideHostKeyAlgorithm(string data) {
			string[] t = data.Split(',');
			foreach(PublicKeyAlgorithm a in _param.PreferableHostKeyAlgorithms) {
				if(SSHUtil.ContainsString(t, SSH2Util.PublicKeyAlgorithmName(a))) {
					return a;
				}
			}
			throw new SSHException("The negotiation of host key verification algorithm is failed");
		}
		private CipherAlgorithm DecideCipherAlgorithm(string data) {
			string[] t = data.Split(',');
			foreach(CipherAlgorithm a in _param.PreferableCipherAlgorithms) {
				if(SSHUtil.ContainsString(t, CipherFactory.AlgorithmToSSH2Name(a))) {
					return a;
				}
			}
			throw new SSHException("The negotiation of encryption algorithm is failed");
		}
		private string FormatHostKeyAlgorithmDescription() {
			StringBuilder b = new StringBuilder();
			if(_param.PreferableHostKeyAlgorithms.Length==0) throw new SSHException("HostKeyAlgorithm is not set");
			b.Append(SSH2Util.PublicKeyAlgorithmName(_param.PreferableHostKeyAlgorithms[0]));
			for(int i=1; i<_param.PreferableHostKeyAlgorithms.Length; i++) {
				b.Append(',');
				b.Append(SSH2Util.PublicKeyAlgorithmName(_param.PreferableHostKeyAlgorithms[i]));
			}
			return b.ToString();
		}
		private string FormatCipherAlgorithmDescription() {
			StringBuilder b = new StringBuilder();
			if(_param.PreferableCipherAlgorithms.Length==0) throw new SSHException("CipherAlgorithm is not set");
			b.Append(CipherFactory.AlgorithmToSSH2Name(_param.PreferableCipherAlgorithms[0]));
			for(int i=1; i<_param.PreferableCipherAlgorithms.Length; i++) {
				b.Append(',');
				b.Append(CipherFactory.AlgorithmToSSH2Name(_param.PreferableCipherAlgorithms[i]));
			}
			return b.ToString();
		}

		/*
		 * the seed of diffie-hellman KX defined in the spec of SSH2
		 */ 
		private static BigInteger _dh_prime = null;
		private static BigInteger DH_PRIME {
			get {
				if(_dh_prime==null) {
					StringBuilder sb = new StringBuilder();
					sb.Append("FFFFFFFFFFFFFFFFC90FDAA22168C234C4C6628B80DC1CD1");
					sb.Append("29024E088A67CC74020BBEA63B139B22514A08798E3404DD");
					sb.Append("EF9519B3CD3A431B302B0A6DF25F14374FE1356D6D51C245");
					sb.Append("E485B576625E7EC6F44C42E9A637ED6B0BFF5CB6F406B7ED");
					sb.Append("EE386BFB5A899FA5AE9F24117C4B1FE649286651ECE65381");
					sb.Append("FFFFFFFFFFFFFFFF");
					_dh_prime = new BigInteger(sb.ToString(), 16);
				}
				return _dh_prime;
			}
		}

	}

}
