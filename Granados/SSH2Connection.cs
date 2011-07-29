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
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;

using Granados.PKI;
using Granados.Crypto;
using Granados.Util;
using Granados.IO;
using Granados.IO.SSH2;

namespace Granados.SSH2
{

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public sealed class SSH2Connection : SSHConnection {
    
        //packet count for transmission and reception
        private int _tSequence;
        private Cipher _tCipher;                            //cipher for transmission
        private SSH2TransmissionPacket _transmissionPacket;
        private DataFragment _transmissionImage;

        //reception util
        private SSH2DataReader _readerForProcessPacket;

        //MAC for transmission and reception
        private MAC _tMAC;
        private SSH2PacketBuilder _packetBuilder;
        private SynchronizedPacketReceiver _packetReceiver;

        //server info
        private SSH2ConnectionInfo _cInfo;

        private bool _waitingForPortForwardingResponse;
        private bool _agentForwardConfirmed;

        private KeyExchanger _asyncKeyExchanger;
        private int _requiredResponseCount; //for keyboard-interactive authentication

        internal SSH2Connection(SSHConnectionParameter param, AbstractGranadosSocket strm, ISSHConnectionEventReceiver r, string serverversion, string clientversion) : base(param, strm, r) {
            _cInfo = new SSH2ConnectionInfo();
            _cInfo._serverVersionString = serverversion;
            _cInfo._clientVersionString = clientversion;
            
            _packetReceiver = new SynchronizedPacketReceiver(this);
            _packetBuilder = new SSH2PacketBuilder(_packetReceiver);
            _transmissionPacket = new SSH2TransmissionPacket();
            _transmissionImage = new DataFragment(null, 0, 0);
        }
        internal void SetAgentForwardConfirmed(bool value) {
            _agentForwardConfirmed = value;
        }
        internal override IDataHandler PacketBuilder {
            get {
                return _packetBuilder;
            }
        }
        public override SSHConnectionInfo ConnectionInfo {
            get {
                return _cInfo;
            }
        }

        internal override AuthenticationResult Connect() {
            //key exchange
            KeyExchanger kex = new KeyExchanger(this, null);
            if(!kex.SynchronizedKexExchange()) {
                Close();
                return AuthenticationResult.Failure;
            }

            //user authentication
            ServiceRequest("ssh-userauth");
            _authenticationResult = UserAuth();
            return _authenticationResult;
        }


        private void ServiceRequest(string servicename) {
            SSH2DataWriter wr = OpenTransmissionPacket();
            wr.WritePacketType(PacketType.SSH_MSG_SERVICE_REQUEST);
            wr.Write(servicename);
            TraceTransmissionEvent("SSH_MSG_SERVICE_REQUEST", servicename);
            TransmitPacket(wr);

            DataFragment response = ReceivePacket();
            SSH2DataReader re = new SSH2DataReader(response);
            PacketType t = re.ReadPacketType();
            if(t!=PacketType.SSH_MSG_SERVICE_ACCEPT) {
                TraceReceptionEvent(t.ToString(), "service request failed");
                throw new SSHException("service establishment failed "+t);
            }

            string s = Encoding.ASCII.GetString(re.ReadString());
            if(servicename!=s)
                throw new SSHException("protocol error");
        }

        private AuthenticationResult UserAuth() {
            const string sn = "ssh-connection";
            if(_param.AuthenticationType==AuthenticationType.KeyboardInteractive) {
                SSH2DataWriter wr = OpenTransmissionPacket();
                wr.WritePacketType(PacketType.SSH_MSG_USERAUTH_REQUEST);
                wr.Write(_param.UserName);
                wr.Write(sn);
                wr.Write("keyboard-interactive");
                wr.Write(""); //lang
                wr.Write(""); //submethod
                TraceTransmissionEvent(PacketType.SSH_MSG_USERAUTH_REQUEST, "starting keyboard-interactive authentication");
                TransmitPacket(wr);
                _authenticationResult = ProcessAuthenticationResponse();
            }
            else {
                SSH2DataWriter wr = OpenTransmissionPacket();
                wr.WritePacketType(PacketType.SSH_MSG_USERAUTH_REQUEST);
                wr.Write(_param.UserName);
                if(_param.AuthenticationType==AuthenticationType.Password) {
                    //Password authentication
                    wr.Write(sn);
                    wr.Write("password");
                    wr.Write(false);
                    wr.Write(_param.Password);
                    TraceTransmissionEvent(PacketType.SSH_MSG_USERAUTH_REQUEST, "starting password authentication");
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
                    TraceTransmissionEvent(PacketType.SSH_MSG_USERAUTH_REQUEST, "starting public key authentication");
                }
                TransmitPacket(wr);

                _authenticationResult = ProcessAuthenticationResponse();
                if(_authenticationResult==AuthenticationResult.Failure)
                    throw new SSHException(Strings.GetString("AuthenticationFailed"));
            }
            return _authenticationResult;
        }
        private AuthenticationResult ProcessAuthenticationResponse() {
            do {
                SSH2DataReader response = new SSH2DataReader(ReceivePacket());
                PacketType h = response.ReadPacketType();
                if(h==PacketType.SSH_MSG_USERAUTH_FAILURE) {
                    string msg = Encoding.ASCII.GetString(response.ReadString());
                    TraceReceptionEvent(h, "user authentication failed:" + msg);
                    return AuthenticationResult.Failure;
                }
                else if(h==PacketType.SSH_MSG_USERAUTH_BANNER) {
                    TraceReceptionEvent(h, "");
                }
                else if(h==PacketType.SSH_MSG_USERAUTH_SUCCESS) {
                    TraceReceptionEvent(h, "user authentication succeeded");
                    _packetBuilder.InnerHandler = new CallbackSSH2PacketHandler(this);
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
                    _requiredResponseCount = num;
                    return AuthenticationResult.Prompt;
                }
                else
                    throw new SSHException("protocol error: unexpected packet type "+h);
            } while(true);
        }
        public AuthenticationResult DoKeyboardInteractiveAuth(string[] input) {
            if(_param.AuthenticationType!=AuthenticationType.KeyboardInteractive)
                throw new SSHException("DoKeyboardInteractiveAuth() must be called with keyboard-interactive authentication");
            SSH2DataWriter wr = OpenTransmissionPacket();
            wr.WritePacketType(PacketType.SSH_MSG_USERAUTH_INFO_RESPONSE);
            wr.Write(input.Length);
            foreach(string t in input)
                wr.Write(t);
            TransmitPacket(wr);

            _authenticationResult = ProcessAuthenticationResponse();
            //recent OpenSSH sends SSH_MSG_USERAUTH_INFO_REQUEST with 0-length prompt array after the first negotiation
            while(_authenticationResult==AuthenticationResult.Prompt && _requiredResponseCount==0) {
                wr = OpenTransmissionPacket();
                wr.WritePacketType(PacketType.SSH_MSG_USERAUTH_INFO_RESPONSE);
                wr.Write(0);
                TransmitPacket(wr);
                _authenticationResult = ProcessAuthenticationResponse();
            }
            return _authenticationResult;
        }
        
        public override SSHChannel OpenShell(ISSHChannelEventReceiver receiver) {
            return DoExecCommandInternal(receiver, ChannelType.Shell, null, "opening shell");
        }

        // open new channel for SCP
        public override SSHChannel DoExecCommand(ISSHChannelEventReceiver receiver, string command) {
            return DoExecCommandInternal(receiver, ChannelType.ExecCommand, command, "executing " + command);
        }

        // open subsystem such as NETCONF
        public SSHChannel OpenSubsystem(ISSHChannelEventReceiver receiver, string subsystem) {
            return DoExecCommandInternal(receiver, ChannelType.Subsystem, subsystem, "subsystem " + subsystem);
        }

        //open channel
        private SSHChannel DoExecCommandInternal(ISSHChannelEventReceiver receiver, ChannelType channel_type, string command, string message) {
            SSH2DataWriter wr = OpenTransmissionPacket();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN);
            wr.Write("session");
            int local_channel = this.ChannelCollection.RegisterChannelEventReceiver(null, receiver).LocalID;

            wr.Write(local_channel);
            wr.Write(_param.WindowSize); //initial window size
            int windowsize = _param.WindowSize;
            wr.Write(_param.MaxPacketSize); //max packet size
            SSH2Channel channel = new SSH2Channel(this, channel_type, local_channel, command);
            TraceTransmissionEvent(PacketType.SSH_MSG_CHANNEL_OPEN, "executing command");
            TransmitPacket(wr);
            
            return channel;
        }

        public override SSHChannel ForwardPort(ISSHChannelEventReceiver receiver, string remote_host, int remote_port, string originator_host, int originator_port) {
            SSH2DataWriter wr = OpenTransmissionPacket();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN);
            wr.Write("direct-tcpip");
            int local_id = this.ChannelCollection.RegisterChannelEventReceiver(null, receiver).LocalID;
            wr.Write(local_id);
            wr.Write(_param.WindowSize); //initial window size
            int windowsize = _param.WindowSize;
            wr.Write(_param.MaxPacketSize); //max packet size
            wr.Write(remote_host);
            wr.Write(remote_port);
            wr.Write(originator_host);
            wr.Write(originator_port);

            SSH2Channel channel = new SSH2Channel(this, ChannelType.ForwardedLocalToRemote, local_id, null);
            
            TraceTransmissionEvent(PacketType.SSH_MSG_CHANNEL_OPEN, "opening a forwarded port : host={0} port={1}", remote_host, remote_port);
            TransmitPacket(wr);
            
            return channel;
        }

        public override void ListenForwardedPort(string allowed_host, int bind_port) {
            SSH2DataWriter wr = OpenTransmissionPacket();;
            wr.WritePacketType(PacketType.SSH_MSG_GLOBAL_REQUEST);
            wr.Write("tcpip-forward");
            wr.Write(true);
            wr.Write(allowed_host);
            wr.Write(bind_port);

            _waitingForPortForwardingResponse = true;
            TraceTransmissionEvent(PacketType.SSH_MSG_GLOBAL_REQUEST, "starting to listen to a forwarded port : host={0} port={1}", allowed_host, bind_port);
            TransmitPacket(wr);
        }

        public override void CancelForwardedPort(string host, int port) {
            SSH2DataWriter wr = OpenTransmissionPacket();;
            wr.WritePacketType(PacketType.SSH_MSG_GLOBAL_REQUEST);
            wr.Write("cancel-tcpip-forward");
            wr.Write(true);
            wr.Write(host);
            wr.Write(port);
            TraceTransmissionEvent(PacketType.SSH_MSG_GLOBAL_REQUEST, "terminating to listen to a forwarded port : host={0} port={1}", host, port);
            TransmitPacket(wr);
        }

        private void ProcessPortforwardingRequest(ISSHConnectionEventReceiver receiver, SSH2DataReader reader) {

            int remote_channel = reader.ReadInt32();
            int window_size = reader.ReadInt32(); //skip initial window size
            int servermaxpacketsize = reader.ReadInt32();
            string host = Encoding.ASCII.GetString(reader.ReadString());
            int port = reader.ReadInt32();
            string originator_ip = Encoding.ASCII.GetString(reader.ReadString());
            int originator_port = reader.ReadInt32();
            
            TraceReceptionEvent("port forwarding request", String.Format("host={0} port={1} originator-ip={2} originator-port={3}", host, port, originator_ip, originator_port));
            PortForwardingCheckResult r = receiver.CheckPortForwardingRequest(host,port,originator_ip,originator_port);
            SSH2DataWriter wr = new SSH2DataWriter();
            if(r.allowed) {
                //send OPEN_CONFIRMATION
                SSH2Channel channel = new SSH2Channel(this, ChannelType.ForwardedRemoteToLocal, this.ChannelCollection.RegisterChannelEventReceiver(null, r.channel).LocalID, remote_channel, servermaxpacketsize);
                wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION);
                wr.Write(remote_channel);
                wr.Write(channel.LocalChannelID);
                wr.Write(_param.WindowSize); //initial window size
                wr.Write(_param.MaxPacketSize); //max packet size
                receiver.EstablishPortforwarding(r.channel, channel);
                TraceTransmissionEvent("port-forwarding request is confirmed", "host={0} port={1} originator-ip={2} originator-port={3}", host, port, originator_ip, originator_port);
            }
            else {
                wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE);
                wr.Write(remote_channel);
                wr.Write(r.reason_code);
                wr.Write(r.reason_message);
                wr.Write(""); //lang tag
                TraceTransmissionEvent("port-forwarding request is rejected", "host={0} port={1} originator-ip={2} originator-port={3}", host, port, originator_ip, originator_port);
            }
            TransmitRawPayload(wr.ToByteArray());
        }

        private void ProcessAgentForwardRequest(ISSHConnectionEventReceiver receiver, SSH2DataReader reader) {
            int remote_channel = reader.ReadInt32();
            int window_size = reader.ReadInt32(); //skip initial window size
            int servermaxpacketsize = reader.ReadInt32();
            TraceReceptionEvent("agent forward request", "");

            SSH2DataWriter wr = new SSH2DataWriter();
            IAgentForward af = _param.AgentForward;
            if(_agentForwardConfirmed && af!=null && af.CanAcceptForwarding()) {
                //send OPEN_CONFIRMATION
                AgentForwardingChannel ch = new AgentForwardingChannel(af);
                SSH2Channel channel = new SSH2Channel(this, ChannelType.AgentForward, this.ChannelCollection.RegisterChannelEventReceiver(null, ch).LocalID, remote_channel, servermaxpacketsize);
                ch.SetChannel(channel);
                wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION);
                wr.Write(remote_channel);
                wr.Write(channel.LocalChannelID);
                wr.Write(_param.WindowSize); //initial window size
                wr.Write(_param.MaxPacketSize); //max packet size
                TraceTransmissionEvent("granados confirmed agent-forwarding request", "");
            }
            else {
                wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE);
                wr.Write(remote_channel);
                wr.Write(0);
                wr.Write("reject");
                wr.Write(""); //lang tag
                TraceTransmissionEvent("granados rejected agent-forwarding request", "");
            }
            TransmitRawPayload(wr.ToByteArray());
        }

        internal SSH2DataWriter OpenTransmissionPacket() {
            return _transmissionPacket.Open();
        }

        internal void TransmitPacket(SSH2DataWriter payload) {
            lock(this) {
                Debug.Assert(payload==_transmissionPacket.UnderlyingWriter);
                _transmissionPacket.Close(_tCipher, _param.Random, _tMAC, _tSequence++, _transmissionImage);
                _stream.Write(_transmissionImage.Data, _transmissionImage.Offset, _transmissionImage.Length);
            }
        }
        internal void TransmitRawPayload(byte[] payload) {
            lock(this) {
                SSH2TransmissionPacket p = new SSH2TransmissionPacket();
                SSH2DataWriter wr = p.Open();
                wr.Write(payload);
                p.Close(_tCipher, _param.Random, _tMAC, _tSequence++, _transmissionImage);
                _stream.Write(_transmissionImage.Data, _transmissionImage.Offset, _transmissionImage.Length);
            }
        }

        internal DataFragment SynchronizedTransmitPacket(SSH2DataWriter payload) {
            lock(this) {
                Debug.Assert(payload==_transmissionPacket.UnderlyingWriter);
                _transmissionPacket.Close(_tCipher, _param.Random, _tMAC, _tSequence++, _transmissionImage);
                return _packetReceiver.SendAndWaitResponse(_transmissionImage);
            }
        }
        



        //synchronous reception
        internal DataFragment ReceivePacket() {
            while(true) {
                DataFragment data = _packetReceiver.WaitResponse();

                PacketType pt = (PacketType)data.ByteAt(0); //sneak

                //filter unnecessary packet
                if(pt==PacketType.SSH_MSG_IGNORE) {
                    SSH2DataReader r = new SSH2DataReader(data);
                    r.ReadPacketType(); //skip
                    byte[] msg = r.ReadString();
                    if(_eventReceiver!=null) _eventReceiver.OnIgnoreMessage(msg);
                    TraceReceptionEvent(pt, msg);
                }
                else if(pt==PacketType.SSH_MSG_DEBUG) {
                    SSH2DataReader r = new SSH2DataReader(data);
                    r.ReadPacketType(); //skip
                    bool f = r.ReadBool();
                    byte[] msg = r.ReadString();
                    if(_eventReceiver!=null) _eventReceiver.OnDebugMessage(f, msg);
                    TraceReceptionEvent(pt, msg);
                }
                else {
                    return data;
                }
            }
        }
        internal void AsyncReceivePacket(DataFragment packet) {
            try {
                ProcessPacket(packet);
            }
            catch(Exception ex) {
                _eventReceiver.OnError(ex);
            }
        }

        private bool ProcessPacket(DataFragment packet) {
            if(_readerForProcessPacket==null)
                _readerForProcessPacket = new SSH2DataReader(packet);
            else
                _readerForProcessPacket.Recycle(packet); //avoid 'new'

            SSH2DataReader r = _readerForProcessPacket; //rename for frequently use
            PacketType pt = r.ReadPacketType();
            
            if(pt==PacketType.SSH_MSG_DISCONNECT) {
                int errorcode = r.ReadInt32();
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
                string method = Encoding.ASCII.GetString(r.ReadString());
                if(method=="forwarded-tcpip")
                    ProcessPortforwardingRequest(_eventReceiver, r);
                else if(method.StartsWith("auth-agent")) //in most cases, method is "auth-agent@openssh.com"
                    ProcessAgentForwardRequest(_eventReceiver, r);
                else {
                    SSH2DataWriter wr = new SSH2DataWriter();
                    wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE);
                    wr.Write(r.ReadInt32());
                    wr.Write(0);
                    wr.Write("unknown method");
                    wr.Write(""); //lang tag
                    TraceReceptionEvent("SSH_MSG_CHANNEL_OPEN rejected", "method={0}", method);
                }
                return true;
            }
            else if(pt>=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION && pt<=PacketType.SSH_MSG_CHANNEL_FAILURE) {
                int local_channel = r.ReadInt32();
                ChannelCollection.Entry e = this.ChannelCollection.FindChannelEntry(local_channel);
                if(e!=null) 
                    ((SSH2Channel)e.Channel).ProcessPacket(e.Receiver, pt, 5+r.Rest, r);
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
                //Debug.WriteLine("Host sent KEXINIT");
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
            if(!this.IsOpen) return;
            SSH2DataWriter wr = OpenTransmissionPacket();
            wr.WritePacketType(PacketType.SSH_MSG_DISCONNECT);
            wr.Write(0);
            wr.Write(msg);
            wr.Write(""); //language
            TransmitPacket(wr);
            //!!TODO クライアントからの切断リクエスト状態に切り替える
            Close();
        }

        public override void SendIgnorableData(string msg) {
            SSH2DataWriter w = OpenTransmissionPacket();
            w.WritePacketType(PacketType.SSH_MSG_IGNORE);
            w.Write(msg);
            TransmitPacket(w);
        }

        //Start key refresh
        public void ReexchangeKeys() {
            _asyncKeyExchanger = new KeyExchanger(this, _sessionID);
            _asyncKeyExchanger.AsyncStartReexchange();
        }

        internal void RefreshKeys(byte[] sessionID, Cipher tc, Cipher rc, MAC tm, MAC rm) {
            lock(this) { //these must change synchronously
                _sessionID = sessionID;
                _tCipher = tc;
                _tMAC = tm;
                _packetBuilder.SetCipher(rc, rm, _param.CheckMACError);
                _asyncKeyExchanger = null;
            }
        }

        //alternative version
        internal void TraceTransmissionEvent(PacketType pt, string message, params object[] args) {
            ISSHEventTracer t = _param.EventTracer;
            if(t!=null) t.OnTranmission(pt.ToString(), String.Format(message, args));
        }
        internal void TraceReceptionEvent(PacketType pt, string message, params object[] args) {
            ISSHEventTracer t = _param.EventTracer;
            if(t!=null) t.OnReception(pt.ToString(), String.Format(message, args));
        }
        internal void TraceReceptionEvent(PacketType pt, byte[] msg) {
            TraceReceptionEvent(pt.ToString(), Encoding.ASCII.GetString(msg));
        }
    }

    /**
     * Channel
     */
    /// <exclude/>
    public class SSH2Channel : SSHChannel {
        private SSH2Connection _connection;
        //channel property
        private string _command;
        private int _windowSize;
        private int _leftWindowSize;
        private int _serverMaxPacketSize;
        private int _allowedDataSize;

        //negotiation status
        private enum NegotiationStatus {
            Ready,
            WaitingChannelConfirmation,
            WaitingPtyReqConfirmation,
            WaitingShellConfirmation,
            WaitingExecCmdConfirmation,
            WaitingAuthAgentReqConfirmation
        }
        private NegotiationStatus _negotiationStatus;

        public SSH2Channel(SSH2Connection con, ChannelType type, int local_id, string command) : base(con, type, local_id) {
            _command = command;
            if(type==ChannelType.ExecCommand || type==ChannelType.Subsystem)
                Debug.Assert(command!=null); //'command' is required for ChannelType.ExecCommand
            _connection = con;
            _windowSize = _leftWindowSize = con.Param.WindowSize;
            _negotiationStatus = NegotiationStatus.WaitingChannelConfirmation;
        }

        //attach to an existing channel
        public SSH2Channel(SSH2Connection con, ChannelType type, int local_id, int remote_id, int maxpacketsize) : base(con, type, local_id) {
            _connection = con;
            _windowSize = _leftWindowSize = con.Param.WindowSize;
            Debug.Assert(type==ChannelType.ForwardedRemoteToLocal || type==ChannelType.AgentForward);
            _remoteID = remote_id;
            _serverMaxPacketSize = maxpacketsize;
            _negotiationStatus = NegotiationStatus.Ready;
        }

        public override void ResizeTerminal(int width, int height, int pixel_width, int pixel_height) {
            SSH2DataWriter wr = OpenWriter();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
            wr.Write(_remoteID);
            wr.Write("window-change");
            wr.Write(false);
            wr.Write(width);
            wr.Write(height);
            wr.Write(pixel_width); //no graphics
            wr.Write(pixel_height);
            _connection.TraceTransmissionEvent("window-change", "width={0} height={1}", width, height);
            TransmitPacket(wr);
        }
        public override void Transmit(byte[] data)	{
            //!!it is better idea that we wait a WINDOW_ADJUST if the left size is lack
            SSH2DataWriter wr = OpenWriter();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_DATA);
            wr.Write(_remoteID);
            wr.WriteAsString(data);

            TransmitPacket(wr);
        }
        public override void Transmit(byte[] data, int offset, int length)	{
            SSH2DataWriter wr = OpenWriter();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_DATA);
            wr.Write(_remoteID);
            wr.WriteAsString(data, offset, length);

            TransmitPacket(wr);
        }

        public override void SendEOF() {
            if(!_connection.IsOpen) return;
            SSH2DataWriter wr = OpenWriter();
            wr.Reset();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_EOF);
            wr.Write(_remoteID);
            _connection.TraceTransmissionEvent(PacketType.SSH_MSG_CHANNEL_EOF, "");
            TransmitPacket(wr);
        }


        public override void Close() {
            if(!_connection.IsOpen) return;
            SSH2DataWriter wr = OpenWriter();
            wr.Reset();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_CLOSE);
            wr.Write(_remoteID);
            _connection.TraceTransmissionEvent(PacketType.SSH_MSG_CHANNEL_CLOSE, "");
            TransmitPacket(wr);
        }

        //maybe this is SSH2 only feature
        public void SetEnvironmentVariable(string name, string value) {
            SSH2DataWriter wr = OpenWriter();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
            wr.Write(_remoteID);
            wr.Write("env");
            wr.Write(false);
            wr.Write(name);
            wr.Write(value);
            _connection.TraceTransmissionEvent("env", "name={0} value={1}", name, value);
            TransmitPacket(wr);
        }
        public void SendBreak(int time) {
            SSH2DataWriter wr = OpenWriter();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
            wr.Write(_remoteID);
            wr.Write("break");
            wr.Write(true);
            wr.Write(time);
            _connection.TraceTransmissionEvent("break", "time={0}", time);
            TransmitPacket(wr);
        }

        internal void ProcessPacket(ISSHChannelEventReceiver receiver, PacketType pt, int data_length, SSH2DataReader re) {
            //NOTE: the offset of 're' is next to 'receipiant channel' field

            AdjustWindowSize(pt, data_length);

            //SSH_MSG_CHANNEL_WINDOW_ADJUST comes before the complete of channel establishment
            if(pt==PacketType.SSH_MSG_CHANNEL_WINDOW_ADJUST) {
                int w = re.ReadInt32();
                //some servers may not send SSH_MSG_CHANNEL_WINDOW_ADJUST. 
                //it is dangerous to wait this message in send procedure
                _allowedDataSize += w;
                if(_connection.IsEventTracerAvailable)
                    _connection.TraceReceptionEvent("SSH_MSG_CHANNEL_WINDOW_ADJUST", "adjusted to {0} by increasing {1}", _allowedDataSize, w);
                return;
            }

            if(_negotiationStatus!=NegotiationStatus.Ready) //when the negotiation is not completed
                ProgressChannelNegotiation(receiver, pt, re);
            else 
                ProcessChannelLocalData(receiver, pt, re);
        }

        private void AdjustWindowSize(PacketType pt, int data_length) {
            _leftWindowSize -= data_length;
            if(pt==PacketType.SSH_MSG_CHANNEL_EOF || pt==PacketType.SSH_MSG_CHANNEL_CLOSE) return; //window adjust is not necessary if the channel is being closed

            // need not send window size to server when the channel is not opened.
            if (_negotiationStatus!=NegotiationStatus.Ready) return; 

            while(_leftWindowSize <= _windowSize) {
                SSH2DataWriter adj = new SSH2DataWriter();
                adj.WritePacketType(PacketType.SSH_MSG_CHANNEL_WINDOW_ADJUST);
                adj.Write(_remoteID);
                adj.Write(_windowSize);
                TransmitPayload(adj.ToByteArray());
                _leftWindowSize += _windowSize;
                if(_connection.IsEventTracerAvailable)
                    _connection.TraceTransmissionEvent("SSH_MSG_CHANNEL_WINDOW_ADJUST", "adjusted to {0} by increasing {1}", _leftWindowSize, _windowSize);
            }
        }

        //Progress the state of this channel establishment negotiation
        private void ProgressChannelNegotiation(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader re) {
            if(_type==ChannelType.Shell)
                OpenShellOrSubsystem(receiver, pt, re, "shell");
            else if(_type==ChannelType.ForwardedLocalToRemote)
                ReceivePortForwardingResponse(receiver, pt, re);
            else if(_type==ChannelType.Session)
                EstablishSession(receiver, pt, re);
            else if(_type==ChannelType.ExecCommand)  // for SCP
                ExecCommand(receiver, pt, re);
            else if(_type==ChannelType.Subsystem)
                OpenShellOrSubsystem(receiver, pt, re, "subsystem");
        }

        private void ProcessChannelLocalData(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader re) {
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
                        wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_FAILURE);
                        wr.Write(_remoteID);
                        TransmitPayload(wr.ToByteArray());
                    }
                }
                    break;
                case PacketType.SSH_MSG_CHANNEL_EOF:
                    receiver.OnChannelEOF();
                    break;
                case PacketType.SSH_MSG_CHANNEL_CLOSE:
                    _connection.ChannelCollection.UnregisterChannelEventReceiver(_localID);
                    receiver.OnChannelClosed();
                    break;
                case PacketType.SSH_MSG_CHANNEL_FAILURE:
                    receiver.OnMiscPacket((byte)pt, re.Image, re.Offset, re.Rest);
                    break;
                default:
                    receiver.OnMiscPacket((byte)pt, re.Image, re.Offset, re.Rest);
                    Debug.WriteLine("Unknown Packet "+pt);
                    break;
            }			
        }

        private SSH2DataWriter OpenWriter() {
            return _connection.OpenTransmissionPacket();
        }
        private void TransmitPacket(SSH2DataWriter writer) {
            _allowedDataSize -= writer.Length - SSH2TransmissionPacket.INITIAL_OFFSET;
            _connection.TransmitPacket(writer);
        }
        private void TransmitPayload(byte[] payload) {
            _allowedDataSize -= payload.Length;
            _connection.TransmitRawPayload(payload);
        }

        private void OpenShellOrSubsystem(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader reader, string scheme) {
            if(_negotiationStatus==NegotiationStatus.WaitingChannelConfirmation) {
                if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION) {
                    if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE)
                        receiver.OnChannelError(new SSHException("opening channel failed; packet type="+pt));
                    else {
                        int errcode = reader.ReadInt32();
                        string msg = Encoding.ASCII.GetString(reader.ReadString());
                        receiver.OnChannelError(new SSHException(msg));
                    }
                    Close();
                }
                else {
                    _remoteID = reader.ReadInt32();
                    _allowedDataSize = reader.ReadInt32();
                    _serverMaxPacketSize = reader.ReadInt32();

                    //open pty
                    SSH2DataWriter wr = new SSH2DataWriter();
                    SSHConnectionParameter param = _connection.Param;
                    wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
                    wr.Write(_remoteID);
                    wr.Write("pty-req");
                    wr.Write(true);
                    wr.Write(param.TerminalName);
                    wr.Write(param.TerminalWidth);
                    wr.Write(param.TerminalHeight);
                    wr.Write(param.TerminalPixelWidth);
                    wr.Write(param.TerminalPixelHeight);
                    wr.WriteAsString(new byte[0]);
                    if(_connection.IsEventTracerAvailable)
                        _connection.TraceTransmissionEvent(PacketType.SSH_MSG_CHANNEL_REQUEST, "pty-req", "terminal={0} width={1} height={2}", param.TerminalName, param.TerminalWidth, param.TerminalHeight);
                    TransmitPayload(wr.ToByteArray());

                    _negotiationStatus = NegotiationStatus.WaitingPtyReqConfirmation;
                }
            }
            else if(_negotiationStatus==NegotiationStatus.WaitingPtyReqConfirmation) {
                if(pt!=PacketType.SSH_MSG_CHANNEL_SUCCESS) {
                    receiver.OnChannelError(new SSHException("opening pty failed"));
                    Close();
                }
                else {
                    //agent request (optional)
                    if(_connection.Param.AgentForward!=null) {
                        SSH2DataWriter wr = new SSH2DataWriter();
                        wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
                        wr.Write(_remoteID);
                        wr.Write("auth-agent-req@openssh.com");
                        wr.Write(true);
                        _connection.TraceTransmissionEvent(PacketType.SSH_MSG_CHANNEL_REQUEST, "auth-agent-req", "");
                        TransmitPayload(wr.ToByteArray());
                        _negotiationStatus = NegotiationStatus.WaitingAuthAgentReqConfirmation;
                    }
                    else {
                        OpenScheme(scheme);
                        _negotiationStatus = NegotiationStatus.WaitingShellConfirmation;
                    }
                }
            }
            else if(_negotiationStatus==NegotiationStatus.WaitingAuthAgentReqConfirmation) {
                if(pt!=PacketType.SSH_MSG_CHANNEL_SUCCESS && pt!=PacketType.SSH_MSG_CHANNEL_FAILURE) {
                    receiver.OnChannelError(new SSHException("auth-agent-req error"));
                    Close();
                }
                else { //auth-agent-req is optional
                    _connection.SetAgentForwardConfirmed(pt==PacketType.SSH_MSG_CHANNEL_SUCCESS);
                    _connection.TraceReceptionEvent(pt, "auth-agent-req");

                    OpenScheme(scheme);
                    _negotiationStatus = NegotiationStatus.WaitingShellConfirmation;
                }
            }
            else if(_negotiationStatus==NegotiationStatus.WaitingShellConfirmation) {
                if(pt!=PacketType.SSH_MSG_CHANNEL_SUCCESS) {
                    receiver.OnChannelError(new SSHException("Opening shell failed: packet type="+pt.ToString()));
                    Close();
                }
                else {
                    receiver.OnChannelReady();
                    _negotiationStatus = NegotiationStatus.Ready; //goal!
                }
            }
        }
        private void OpenScheme(string scheme) {
            //open shell / subsystem
            SSH2DataWriter wr = new SSH2DataWriter();
            wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
            wr.Write(_remoteID);
            wr.Write(scheme);
            wr.Write(true);
            if(_command!=null)
                wr.Write(_command);
            TransmitPayload(wr.ToByteArray());
        }


        // sending "exec" service for SCP protocol.
        private void ExecCommand(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader reader) {
            if(_negotiationStatus==NegotiationStatus.WaitingChannelConfirmation) {
                if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION) {
                    if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE)
                        receiver.OnChannelError(new SSHException("opening channel failed; packet type="+pt));
                    else {
                        int errcode = reader.ReadInt32();
                        string msg = Encoding.ASCII.GetString(reader.ReadString());
                        receiver.OnChannelError(new SSHException(msg));
                    }
                    Close();
                }
                else {
                    _remoteID = reader.ReadInt32();
                    _allowedDataSize = reader.ReadInt32();
                    _serverMaxPacketSize = reader.ReadInt32();

                    // exec command
                    SSH2DataWriter wr = new SSH2DataWriter();
                    SSHConnectionParameter param = _connection.Param;
                    wr.WritePacketType(PacketType.SSH_MSG_CHANNEL_REQUEST);
                    wr.Write(_remoteID);
                    wr.Write("exec");  // "exec"
                    wr.Write(false);   // want confirm is disabled. (*)
                    wr.Write(_command);

                    if (_connection.IsEventTracerAvailable)
                        _connection.TraceTransmissionEvent("exec command","cmd={0}", _command);
                    TransmitPayload(wr.ToByteArray());

                    //confirmation is omitted
                    receiver.OnChannelReady();
                    _negotiationStatus = NegotiationStatus.Ready; //goal!
                }
            }
            else if(_negotiationStatus==NegotiationStatus.WaitingExecCmdConfirmation) {
                if(pt!=PacketType.SSH_MSG_CHANNEL_DATA) {
                    receiver.OnChannelError(new SSHException("exec command failed"));
                    Close();
                }
                else {
                    receiver.OnChannelReady();
                    _negotiationStatus = NegotiationStatus.Ready; //goal!
                }
            }
            else
                throw new SSHException("internal state error");
        }

        
        private void ReceivePortForwardingResponse(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader reader) {
            if(_negotiationStatus==NegotiationStatus.WaitingChannelConfirmation) {
                if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION) {
                    if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE)
                        receiver.OnChannelError(new SSHException("opening channel failed; packet type="+pt));
                    else {
                        int errcode = reader.ReadInt32();
                        string msg = Encoding.ASCII.GetString(reader.ReadString());
                        receiver.OnChannelError(new SSHException(msg));
                    }
                    Close();
                }
                else {
                    _remoteID = reader.ReadInt32();
                    _serverMaxPacketSize = reader.ReadInt32();
                    _negotiationStatus = NegotiationStatus.Ready;
                    receiver.OnChannelReady();
                }
            }
            else
                throw new SSHException("internal state error");
        }
        private void EstablishSession(ISSHChannelEventReceiver receiver, PacketType pt, SSH2DataReader reader) {
            if(_negotiationStatus==NegotiationStatus.WaitingChannelConfirmation) {
                if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION) {
                    if(pt!=PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE)
                        receiver.OnChannelError(new SSHException("opening channel failed; packet type="+pt));
                    else {
                        int remote_id = reader.ReadInt32();
                        int errcode = reader.ReadInt32();
                        string msg = Encoding.ASCII.GetString(reader.ReadString());
                        receiver.OnChannelError(new SSHException(msg));
                    }
                    Close();
                }
                else {
                    _remoteID = reader.ReadInt32();
                    _serverMaxPacketSize = reader.ReadInt32();
                    _negotiationStatus = NegotiationStatus.Ready;
                    receiver.OnChannelReady();
                }
            }
            else
                throw new SSHException("internal state error");
        }
    }

    /**
     * Key Exchange
     */
    internal class KeyExchanger {
        private SSH2Connection _connection;
        private SSHConnectionParameter _param;
        private SSH2ConnectionInfo _cInfo;
        //payload of KEXINIT message
        private byte[] _serverKEXINITPayload;
        private byte[] _clientKEXINITPayload;

        //true if the host sent KEXINIT first
        private bool _startedByHost;

        //asynchronously?
        private enum Mode {
            Synchronized,
            Asynchronized
        }

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
        private Cipher _rc;
        private Cipher _tc;
        private MAC _rm;
        private MAC _tm;

        public KeyExchanger(SSH2Connection con, byte[] sessionID) {
            _connection = con;
            _param = con.Param;
            _cInfo = (SSH2ConnectionInfo)con.ConnectionInfo;
            _sessionID = sessionID;
            _status = Status.INITIAL;
        }

        private SSH2DataWriter OpenWriter(Mode mode) {
            if(mode==Mode.Synchronized)
                return _connection.OpenTransmissionPacket();
            else
                return new SSH2DataWriter(); //in case of synchronized mode, we create independent SSH2DataWriter
        }
        private void TransmitRawPayload(byte[] payload) {
            _connection.TransmitRawPayload(payload);
        }
        private DataFragment SynchronizedTransmitPacket(SSH2DataWriter wr) {
            return _connection.SynchronizedTransmitPacket(wr);
        }

        private void TraceTransmissionNegotiation(PacketType pt, string msg) {
            _connection.TraceTransmissionEvent(pt.ToString(), msg);
        }
        private void TraceReceptionNegotiation(PacketType pt, string msg) {
            _connection.TraceReceptionEvent(pt.ToString(), msg);
        }
            
        public bool SynchronizedKexExchange() {
            Mode m = Mode.Synchronized;
            DataFragment response;
            //note that the KEXINIT is sent asynchronously in most cases
            SendKEXINIT(m);
            response = _connection.ReceivePacket();

            ProcessKEXINIT(response);
            response = SendKEXDHINIT(m);
            if(!ProcessKEXDHREPLY(response)) return false;
            response = SendNEWKEYS(m);
            ProcessNEWKEYS(response);
            return true;
        }
        public void AsyncStartReexchange() {
            _startedByHost = false;
            _status = Status.WAIT_KEXINIT;
            TraceTransmissionNegotiation(PacketType.SSH_MSG_KEXINIT, "starting asynchronously key exchange");
            SendKEXINIT(Mode.Asynchronized);

        }
        public void AsyncProcessPacket(DataFragment packet) {
            Mode m = Mode.Asynchronized;
            switch(_status) {
                case Status.INITIAL:
                    _startedByHost = true;
                    ProcessKEXINIT(packet);
                    SendKEXINIT(m);
                    SendKEXDHINIT(m);
                    break;
                case Status.WAIT_KEXINIT:
                    ProcessKEXINIT(packet);
                    SendKEXDHINIT(m);
                    break;
                case Status.WAIT_KEXDH_REPLY:
                    ProcessKEXDHREPLY(packet);
                    SendNEWKEYS(m);
                    break;
                case Status.WAIT_NEWKEYS:
                    ProcessNEWKEYS(packet);
                    Debug.Assert(_status==Status.FINISHED);
                    break;
            }
        }
                    
        private void SendKEXINIT(Mode mode) {
            const string kex_algorithm = "diffie-hellman-group1-sha1";
            const string mac_algorithm = "hmac-sha1";
            SSH2DataWriter wr = new SSH2DataWriter();
            wr.WritePacketType(PacketType.SSH_MSG_KEXINIT);
            byte[] cookie = new byte[16];
            _param.Random.NextBytes(cookie);
            wr.Write(cookie);
            wr.Write(kex_algorithm); //    kex_algorithms
            wr.Write(FormatHostKeyAlgorithmDescription());            //    server_host_key_algorithms
            wr.Write(FormatCipherAlgorithmDescription());      //    encryption_algorithms_client_to_server
            wr.Write(FormatCipherAlgorithmDescription());      //    encryption_algorithms_server_to_client
            wr.Write(mac_algorithm);                  //    mac_algorithms_client_to_server
            wr.Write(mac_algorithm);                  //    mac_algorithms_server_to_client
            wr.Write("none");                       //    compression_algorithms_client_to_server
            wr.Write("none");                       //    compression_algorithms_server_to_client
            wr.Write("");                           //    languages_client_to_server
            wr.Write("");                           //    languages_server_to_client
            wr.Write(false); //Indicates whether a guessed key exchange packet follows
            wr.Write(0);       //reserved for future extension

            _clientKEXINITPayload = wr.ToByteArray();
            _status = Status.WAIT_KEXINIT;
            if(_connection.IsEventTracerAvailable) {
                StringBuilder bld = new StringBuilder();
                bld.Append("kex_algorithm=");                            bld.Append(kex_algorithm);
                bld.Append("; server_host_key_algorithms=");             bld.Append(FormatHostKeyAlgorithmDescription());
                bld.Append("; encryption_algorithms_client_to_server="); bld.Append(FormatCipherAlgorithmDescription());
                bld.Append("; encryption_algorithms_server_to_client="); bld.Append(FormatCipherAlgorithmDescription());
                bld.Append("; mac_algorithms_client_to_server=");        bld.Append(mac_algorithm);
                bld.Append("; mac_algorithms_server_to_client=");        bld.Append(mac_algorithm);
                TraceTransmissionNegotiation(PacketType.SSH_MSG_KEXINIT, bld.ToString());
            }
            TransmitRawPayload(_clientKEXINITPayload);
        }
        private void ProcessKEXINIT(DataFragment packet) {
            SSH2DataReader re = null;
            do {
                _serverKEXINITPayload = packet.ToNewArray();
                re = new SSH2DataReader(_serverKEXINITPayload);
                byte[] head = re.Read(17); //Type and cookie
                PacketType pt = (PacketType)head[0];

                if(pt==PacketType.SSH_MSG_KEXINIT)
                    break; //successfully exit
                else if(pt==PacketType.SSH_MSG_IGNORE || pt==PacketType.SSH_MSG_DEBUG) { //continue
                    packet = _connection.ReceivePacket();
                }
                else
                    throw new SSHException(String.Format("Server response is not SSH_MSG_KEXINIT but {0}", head[0]));
            } while(true);

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

            if(_connection.IsEventTracerAvailable) {
                StringBuilder bld = new StringBuilder();
                bld.Append("kex_algorithm=");                            bld.Append(kex);
                bld.Append("; server_host_key_algorithms=");             bld.Append(host_key);
                bld.Append("; encryption_algorithms_client_to_server="); bld.Append(enc_cs);
                bld.Append("; encryption_algorithms_server_to_client="); bld.Append(enc_sc);
                bld.Append("; mac_algorithms_client_to_server=");        bld.Append(mac_cs);
                bld.Append("; mac_algorithms_server_to_client=");        bld.Append(mac_sc);
                bld.Append("; comression_algorithms_client_to_server="); bld.Append(comp_cs);
                bld.Append("; comression_algorithms_server_to_client="); bld.Append(comp_sc);
                TraceReceptionNegotiation(PacketType.SSH_MSG_KEXINIT, bld.ToString());
            }

            if(flag) throw new SSHException("Algorithm negotiation failed"); 
        }


        private DataFragment SendKEXDHINIT(Mode mode) {
            //Round1 computes and sends [e]
            byte[] sx = new byte[16];
            _param.Random.NextBytes(sx);
            _x = new BigInteger(sx);
            _e = new BigInteger(2).modPow(_x, DH_PRIME);
            SSH2DataWriter wr = OpenWriter(mode);
            wr.WritePacketType(PacketType.SSH_MSG_KEXDH_INIT);
            wr.Write(_e);
            _status = Status.WAIT_KEXDH_REPLY;
            TraceTransmissionNegotiation(PacketType.SSH_MSG_KEXDH_INIT, "");
            if(mode==Mode.Synchronized)
                return SynchronizedTransmitPacket(wr);
            else {
                TransmitRawPayload(wr.ToByteArray());
                return null;
            }
        }

        private bool ProcessKEXDHREPLY(DataFragment packet) {
            //Round2 receives response
            SSH2DataReader re = null;
            PacketType h;
            do {
                re = new SSH2DataReader(packet);
                h = re.ReadPacketType();
                if(h==PacketType.SSH_MSG_KEXDH_REPLY)
                    break; //successfully exit
                else if(h==PacketType.SSH_MSG_IGNORE || h==PacketType.SSH_MSG_DEBUG) { //continue
                    packet = _connection.ReceivePacket();
                }
                else
                    throw new SSHException(String.Format("KeyExchange response is not KEXDH_REPLY but {0}", h));
            } while(true);

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

            _connection.TraceReceptionEvent(h, "verifying host key");
            if(!VerifyHostKey(key_and_cert, signature, _hash)) return false;

            //Debug.WriteLine("hash="+DebugUtil.DumpByteArray(hash));
            if(_sessionID==null) _sessionID = _hash;
            return true;
        }
        private DataFragment SendNEWKEYS(Mode mode) {
            _status = Status.WAIT_NEWKEYS;
            Monitor.Enter(_connection); //lock the connection during we exchange sSH_MSG_NEWKEYS
            TransmitRawPayload(new byte[1] {(byte)PacketType.SSH_MSG_NEWKEYS});

            //establish Ciphers
            _tc = CipherFactory.CreateCipher(SSHProtocol.SSH2, _cInfo._algorithmForTransmittion,
                DeriveKey(_k, _hash, 'C', CipherFactory.GetKeySize(_cInfo._algorithmForTransmittion)), DeriveKey(_k, _hash, 'A', CipherFactory.GetBlockSize(_cInfo._algorithmForTransmittion)));
            _rc = CipherFactory.CreateCipher(SSHProtocol.SSH2, _cInfo._algorithmForReception,
                DeriveKey(_k, _hash, 'D', CipherFactory.GetKeySize(_cInfo._algorithmForReception)), DeriveKey(_k, _hash, 'B', CipherFactory.GetBlockSize(_cInfo._algorithmForReception)));

            //establish MACs
            MACAlgorithm ma = MACAlgorithm.HMACSHA1;
            _tm = MACFactory.CreateMAC(MACAlgorithm.HMACSHA1, DeriveKey(_k, _hash, 'E', MACFactory.GetSize(ma)));
            _rm = MACFactory.CreateMAC(MACAlgorithm.HMACSHA1, DeriveKey(_k, _hash, 'F', MACFactory.GetSize(ma)));
            
            if(mode==Mode.Synchronized)
                return _connection.ReceivePacket();
            else
                return null;
        }
        private void ProcessNEWKEYS(DataFragment packet) {
            //confirms new key
            if(packet.Length!=1 || packet.ByteAt(0)!=(byte)PacketType.SSH_MSG_NEWKEYS) {
                Monitor.Exit(_connection);
                throw new SSHException("SSH_MSG_NEWKEYS failed");
            }

            _connection.RefreshKeys(_sessionID, _tc, _rc, _tm, _rm);
            Monitor.Exit(_connection);
            TraceReceptionNegotiation(PacketType.SSH_MSG_NEWKEYS, "the keys are refreshed");
            _status = Status.FINISHED;
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
