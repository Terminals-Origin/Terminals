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
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Granados.PKI;
using Granados.Util;
using Granados.Crypto;
using Granados.IO;
using Granados.IO.SSH1;

namespace Granados.SSH1
{
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public sealed class SSH1Connection : SSHConnection {
    
        private const int AUTH_NOT_REQUIRED = 0;
        private const int AUTH_REQUIRED = 1;

        private SSH1ConnectionInfo _cInfo;
        private int _shellID;

        private SynchronizedPacketReceiver _packetReceiver;
        private SSH1PacketBuilder _packetBuilder;
        private bool _executingShell;
        private Cipher _tCipher;                            //cipher for transmission

        // exec command for SCP
        //private bool _executingExecCmd = false;

        public SSH1Connection(SSHConnectionParameter param, AbstractGranadosSocket s, ISSHConnectionEventReceiver er, string serverversion, string clientversion) : base(param, s, er) {
            _cInfo = new SSH1ConnectionInfo();
            _cInfo._serverVersionString = serverversion;
            _cInfo._clientVersionString = clientversion;
            _shellID = -1;
            _packetReceiver = new SynchronizedPacketReceiver(this);
            _packetBuilder = new SSH1PacketBuilder(_packetReceiver);
        }
        public override SSHConnectionInfo ConnectionInfo {
            get {
                return _cInfo;
            }
        }
        internal override IDataHandler PacketBuilder {
            get {
                return _packetBuilder;
            }
        }

        internal override AuthenticationResult Connect() {
            
            // Phase1 receives server keys
            ReceiveServerKeys();
            if(_param.KeyCheck!=null && !_param.KeyCheck(_cInfo)) {
                _stream.Close();
                return AuthenticationResult.Failure;
            }

            // Phase2 generates session key
            byte[] session_key = GenerateSessionKey();
            
            // Phase3 establishes the session key
            SendSessionKey(session_key);
            InitCipher(session_key);
            ReceiveKeyConfirmation();
            
            // Phase4 user authentication
            SendUserName(_param.UserName);
            if(ReceiveAuthenticationRequirement()==AUTH_REQUIRED) {
                if(_param.AuthenticationType==AuthenticationType.Password) {
                    SendPlainPassword();
                } else if(_param.AuthenticationType==AuthenticationType.PublicKey) {
                    DoRSAChallengeResponse();
                }
                bool auth = ReceiveAuthenticationResult();
                if(!auth) throw new SSHException(Strings.GetString("AuthenticationFailed"));

            }
            
            if(_authenticationResult!=AuthenticationResult.Failure) {
                _packetBuilder.InnerHandler = new CallbackSSH1PacketHandler(this);
            }
            return AuthenticationResult.Success;
        }

        internal void Transmit(SSH1Packet p) {
            lock(this) {
                p.WriteTo(_stream, _tCipher);
            }
        }

        public override void Disconnect(string msg) {
            if(!this.IsOpen) return;
            SSH1DataWriter w = new SSH1DataWriter();
            w.Write(msg);
            SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_MSG_DISCONNECT, w.ToByteArray());
            p.WriteTo(_stream, _tCipher);
            base.Close();
        }

        public override void SendIgnorableData(string msg) {
            SSH1DataWriter w = new SSH1DataWriter();
            w.Write(msg);
            SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_MSG_IGNORE, w.ToByteArray());
            Transmit(p);
        }

    
        private void ReceiveServerKeys() {
            DataFragment packet = ReceivePacket();
            SSH1DataReader reader = new SSH1DataReader(packet);
            PacketType pt = reader.ReadPacketType();

            if(pt!=PacketType.SSH_SMSG_PUBLIC_KEY) throw new SSHException("unexpected SSH SSH1Packet type " + pt, reader.ReadAll());
            
            _cInfo._serverinfo = new SSHServerInfo(reader); 
            _cInfo._hostkey = new RSAPublicKey(_cInfo._serverinfo.host_key_public_exponent, _cInfo._serverinfo.host_key_public_modulus);
            
            //read protocol support parameters
            int protocol_flags = reader.ReadInt32();
            int supported_ciphers_mask = reader.ReadInt32();
            _cInfo.SetSupportedCipherAlgorithms(supported_ciphers_mask);
            int supported_authentications_mask = reader.ReadInt32();
            //Debug.WriteLine(String.Format("ServerOptions {0} {1} {2}", protocol_flags, supported_ciphers_mask, supported_authentications_mask));

            if(reader.Rest>0) throw new SSHException("data length mismatch", reader.ReadAll());
            
            bool found = false;
            foreach(CipherAlgorithm a in _param.PreferableCipherAlgorithms) {
                if(a!=CipherAlgorithm.Blowfish && a!=CipherAlgorithm.TripleDES)
                    continue;
                else if(a==CipherAlgorithm.Blowfish && (supported_ciphers_mask & (1 << (int)CipherAlgorithm.Blowfish))==0)
                    continue; 
                else if(a==CipherAlgorithm.TripleDES && (supported_ciphers_mask & (1 << (int)CipherAlgorithm.TripleDES))==0)
                    continue; 

                _cInfo._algorithmForReception = _cInfo._algorithmForTransmittion = a;  
                found = true;
                break;
            }

            if(!found) 
                throw new SSHException(String.Format(Strings.GetString("ServerNotSupportedX"), "Blowfish/TripleDES"));

            if(_param.AuthenticationType==AuthenticationType.Password && (supported_authentications_mask & (1 << (int)AuthenticationType.Password))==0)
                throw new SSHException(String.Format(Strings.GetString("ServerNotSupportedPassword")), reader.ReadAll());
            if(_param.AuthenticationType==AuthenticationType.PublicKey && (supported_authentications_mask & (1 << (int)AuthenticationType.PublicKey))==0)
                throw new SSHException(String.Format(Strings.GetString("ServerNotSupportedRSA")), reader.ReadAll());
            
            TraceReceptionEvent(pt, "received server key");
        }
    
        private byte[] GenerateSessionKey() {
            //session key(256bits)
            byte[] session_key = new byte[32];
            _param.Random.NextBytes(session_key); 
            
            return session_key;
        }
    
        private void SendSessionKey(byte[] session_key) {
            try
            {
                //step1 XOR with session_id
                byte[] working_data = new byte[session_key.Length];
                byte[] session_id = CalcSessionID();
                Array.Copy(session_key, 0, working_data, 0, session_key.Length);
                for(int i=0; i<session_id.Length; i++) working_data[i] ^= session_id[i];

                //step2 decrypts with RSA
                RSAPublicKey first_encryption;
                RSAPublicKey second_encryption;
                SSHServerInfo si = _cInfo._serverinfo;
                int first_key_bytelen, second_key_bytelen;
                if(si.server_key_bits < si.host_key_bits)
                {
                    first_encryption  = new RSAPublicKey(si.server_key_public_exponent, si.server_key_public_modulus);
                    second_encryption = new RSAPublicKey(si.host_key_public_exponent, si.host_key_public_modulus);
                    first_key_bytelen = (si.server_key_bits+7)/8;
                    second_key_bytelen = (si.host_key_bits+7)/8;
                }
                else
                {
                    first_encryption  = new RSAPublicKey(si.host_key_public_exponent, si.host_key_public_modulus);
                    second_encryption = new RSAPublicKey(si.server_key_public_exponent, si.server_key_public_modulus);
                    first_key_bytelen = (si.host_key_bits+7)/8;
                    second_key_bytelen = (si.server_key_bits+7)/8;
                }

                BigInteger first_result = RSAUtil.PKCS1PadType2(new BigInteger(working_data), first_key_bytelen, _param.Random).modPow(first_encryption.Exponent, first_encryption.Modulus);
                BigInteger second_result = RSAUtil.PKCS1PadType2(first_result, second_key_bytelen, _param.Random).modPow(second_encryption.Exponent, second_encryption.Modulus);

                //output
                SSH1DataWriter writer = new SSH1DataWriter();
                writer.Write((byte)_cInfo._algorithmForTransmittion);
                writer.Write(si.anti_spoofing_cookie);
                writer.Write(second_result);
                writer.Write(0); //protocol flags

                //send
                TraceTransmissionEvent(PacketType.SSH_CMSG_SESSION_KEY, "sent encrypted session-keys");
                SSH1Packet packet = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_SESSION_KEY, writer.ToByteArray());
                packet.WriteTo(_stream);

                _sessionID = session_id;

            }
            catch(Exception e)
            {
                if(e is IOException)
                    throw (IOException)e;
                else
                {
                    string t = e.StackTrace;
                    throw new SSHException(e.Message); //IOException以外はみなSSHExceptionにしてしまう
                }
            }
        }
    
        private void ReceiveKeyConfirmation() {
            DataFragment packet = ReceivePacket();
            if(SneakPacketType(packet)!=PacketType.SSH_SMSG_SUCCESS)
                throw new SSHException("unexpected packet type [" + SneakPacketType(packet).ToString() +"] at ReceiveKeyConfirmation()");
        }
    
        private int ReceiveAuthenticationRequirement() {
            DataFragment packet = ReceivePacket();
            PacketType pt = SneakPacketType(packet);
            if(pt==PacketType.SSH_SMSG_SUCCESS)
                return AUTH_NOT_REQUIRED;
            else if(pt==PacketType.SSH_SMSG_FAILURE)
                return AUTH_REQUIRED;  
            else
                throw new SSHException("unexpected type " + pt);
        }
    
        private void SendUserName(string username) {
            SSH1DataWriter writer = new SSH1DataWriter();
            writer.Write(username);
            SSH1Packet SSH1Packet = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_USER, writer.ToByteArray());
            SSH1Packet.WriteTo(_stream, _tCipher);
            TraceTransmissionEvent(PacketType.SSH_CMSG_USER, "sent user name");
        }
        private void SendPlainPassword() {
            SSH1DataWriter writer = new SSH1DataWriter();
            writer.Write(_param.Password);
            SSH1Packet SSH1Packet = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_AUTH_PASSWORD, writer.ToByteArray());
            SSH1Packet.WriteTo(_stream, _tCipher);
            TraceTransmissionEvent(PacketType.SSH_CMSG_AUTH_PASSWORD, "sent password");
        }

        //RSA authentication
        private void DoRSAChallengeResponse() {
            //read key
            SSH1UserAuthKey key = new SSH1UserAuthKey(_param.IdentityFile, _param.Password);
            SSH1DataWriter w = new SSH1DataWriter();
            w.Write(key.PublicModulus);
            SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_AUTH_RSA, w.ToByteArray());
            p.WriteTo(_stream, _tCipher);
            TraceTransmissionEvent(PacketType.SSH_CMSG_AUTH_RSA, "RSA challenge-reponse");

            DataFragment response = ReceivePacket();
            SSH1DataReader reader = new SSH1DataReader(response);
            PacketType pt = reader.ReadPacketType();
            if(pt==PacketType.SSH_SMSG_FAILURE)
                throw new SSHException(Strings.GetString("ServerRefusedRSA"));
            else if(pt!=PacketType.SSH_SMSG_AUTH_RSA_CHALLENGE)
                throw new SSHException(String.Format(Strings.GetString("UnexpectedResponse"), pt));
            TraceReceptionEvent(PacketType.SSH_SMSG_AUTH_RSA_CHALLENGE, "received challenge");

            //creating challenge
            BigInteger challenge = key.decryptChallenge(reader.ReadMPInt());
            byte[] rawchallenge = RSAUtil.StripPKCS1Pad(challenge, 2).getBytes();

            //building response
            MemoryStream bos = new MemoryStream();
            bos.Write(rawchallenge, 0, rawchallenge.Length); //!!mindtermでは頭が０かどうかで変なハンドリングがあった
            bos.Write(_sessionID, 0, _sessionID.Length);
            byte[] reply = new MD5CryptoServiceProvider().ComputeHash(bos.ToArray());

            w = new SSH1DataWriter();
            w.Write(reply);
            p = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_AUTH_RSA_RESPONSE, w.ToByteArray());
            p.WriteTo(_stream, _tCipher);
            TraceReceptionEvent(PacketType.SSH_CMSG_AUTH_RSA_RESPONSE, "received response");
        }

        private bool ReceiveAuthenticationResult() {
            DataFragment packet = ReceivePacket();
            SSH1DataReader r = new SSH1DataReader(packet);
            PacketType type = r.ReadPacketType();
            TraceReceptionEvent(type, "user authentication response");
            if(type==PacketType.SSH_MSG_DEBUG) {
                //Debug.WriteLine("receivedd debug message:"+Encoding.ASCII.GetString(r.ReadString()));
                return ReceiveAuthenticationResult();
            }
            else if(type==PacketType.SSH_SMSG_SUCCESS)
                return true;
            else if(type==PacketType.SSH_SMSG_FAILURE)
                return false;
            else
                throw new SSHException("unexpected type: " + type);
        }

        // sending exec command for SCP
        // TODO: まだ実装中です
        public override SSHChannel DoExecCommand(ISSHChannelEventReceiver receiver, string command) {
            //_executingExecCmd = true;
            SendExecCommand();
            return null;
        }

        private void SendExecCommand() {
            Debug.WriteLine("EXEC COMMAND");
            string cmd = _execCmd;
            SSH1DataWriter writer = new SSH1DataWriter();
            writer.Write(cmd);
            SSH1Packet SSH1Packet = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_EXEC_CMD, writer.ToByteArray());
            SSH1Packet.WriteTo(_stream, _tCipher);
            TraceTransmissionEvent(PacketType.SSH_CMSG_EXEC_CMD, "exec command: cmd={0}", cmd);
        }

        public override SSHChannel OpenShell(ISSHChannelEventReceiver receiver) {
            if(_shellID!=-1)
                throw new SSHException("A shell is opened already");
            _shellID = _channel_collection.RegisterChannelEventReceiver(null, receiver).LocalID;
            SendRequestPTY();
            _executingShell = true;
            return new SSH1Channel(this, ChannelType.Shell, _shellID);
        }

        private void SendRequestPTY() {
            SSH1DataWriter writer = new SSH1DataWriter();
            writer.Write(_param.TerminalName);
            writer.Write(_param.TerminalHeight);
            writer.Write(_param.TerminalWidth);
            writer.Write(_param.TerminalPixelWidth);
            writer.Write(_param.TerminalPixelHeight);
            writer.Write(new byte[1]); //TTY_OP_END
            SSH1Packet SSH1Packet = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_REQUEST_PTY, writer.ToByteArray());
            SSH1Packet.WriteTo(_stream, _tCipher);
            TraceTransmissionEvent(PacketType.SSH_CMSG_REQUEST_PTY, "open shell: terminal={0} width={1} height={2}", _param.TerminalName, _param.TerminalWidth, _param.TerminalHeight);
        }
    
        private void ExecShell() {
            //System.out.println("EXEC SHELL");
            SSH1Packet SSH1Packet = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_EXEC_SHELL);
            SSH1Packet.WriteTo(_stream, _tCipher);
        }

        public override SSHChannel ForwardPort(ISSHChannelEventReceiver receiver, string remote_host, int remote_port, string originator_host, int originator_port) {
            if(_shellID==-1) {
                ExecShell();
                _shellID = _channel_collection.RegisterChannelEventReceiver(null, new SSH1DummyReceiver()).LocalID;
            }
 
            int local_id = _channel_collection.RegisterChannelEventReceiver(null, receiver).LocalID;

            SSH1DataWriter writer = new SSH1DataWriter();
            writer.Write(local_id); //channel id is fixed to 0
            writer.Write(remote_host);
            writer.Write(remote_port);
            //originator is specified only if SSH_PROTOFLAG_HOST_IN_FWD_OPEN is specified
            //writer.Write(originator_host);
            SSH1Packet SSH1Packet = SSH1Packet.FromPlainPayload(PacketType.SSH_MSG_PORT_OPEN, writer.ToByteArray());
            SSH1Packet.WriteTo(_stream, _tCipher);
            TraceTransmissionEvent(PacketType.SSH_MSG_PORT_OPEN, "open forwarded port: host={0} port={1}", remote_host, remote_port);

            return new SSH1Channel(this, ChannelType.ForwardedLocalToRemote, local_id);
        }

        public override void ListenForwardedPort(string allowed_host, int bind_port) {
            SSH1DataWriter writer = new SSH1DataWriter();
            writer.Write(bind_port);
            writer.Write(allowed_host);
            writer.Write(0);
            SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_PORT_FORWARD_REQUEST, writer.ToByteArray());
            p.WriteTo(_stream, _tCipher);
            TraceTransmissionEvent(PacketType.SSH_CMSG_PORT_FORWARD_REQUEST, "start to listening to remote port: host={0} port={1}", allowed_host, bind_port);

            if(_shellID==-1) {
                ExecShell();
                _shellID = _channel_collection.RegisterChannelEventReceiver(null, new SSH1DummyReceiver()).LocalID;
            }

        }
        public override void CancelForwardedPort(string host, int port) {
            throw new NotSupportedException("not implemented");
        }

        private void ProcessPortforwardingRequest(ISSHConnectionEventReceiver receiver, SSH1DataReader reader) {
            int server_channel = reader.ReadInt32();
            string host = Encoding.ASCII.GetString(reader.ReadString());
            int port = reader.ReadInt32();

            SSH1DataWriter writer = new SSH1DataWriter();
            PortForwardingCheckResult result = receiver.CheckPortForwardingRequest(host, port, "", 0);
            if(result.allowed) {
                int local_id = _channel_collection.RegisterChannelEventReceiver(null, result.channel).LocalID;
                _eventReceiver.EstablishPortforwarding(result.channel, new SSH1Channel(this, ChannelType.ForwardedRemoteToLocal, local_id, server_channel));

                writer.Write(server_channel);
                writer.Write(local_id);
                SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION, writer.ToByteArray());
                p.WriteTo(_stream, _tCipher);
            }
            else {
                writer.Write(server_channel);
                SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_MSG_CHANNEL_OPEN_FAILURE, writer.ToByteArray());
                p.WriteTo(_stream, _tCipher);
            }
        }

        private byte[] CalcSessionID() {
            MemoryStream bos = new MemoryStream();
            SSHServerInfo si = _cInfo._serverinfo;
            byte[] h = si.host_key_public_modulus.getBytes(); 
            byte[] s = si.server_key_public_modulus.getBytes();
            //System.out.println("len h="+h.Length);
            //System.out.println("len s="+s.Length);
            
            int off_h = (h[0]==0? 1 : 0);
            int off_s = (s[0]==0? 1 : 0);
            bos.Write(h, off_h, h.Length-off_h);
            bos.Write(s, off_s, s.Length-off_s);
            bos.Write(si.anti_spoofing_cookie, 0, si.anti_spoofing_cookie.Length);
            
            byte[] session_id = new MD5CryptoServiceProvider().ComputeHash(bos.ToArray());
            //System.out.println("sess-id-len=" + session_id.Length);
            return session_id;
        }
    
        //init ciphers
        private void InitCipher(byte[] session_key) {
            _tCipher = CipherFactory.CreateCipher(SSHProtocol.SSH1, _cInfo._algorithmForTransmittion, session_key);
            Cipher rc = CipherFactory.CreateCipher(SSHProtocol.SSH1, _cInfo._algorithmForReception, session_key);
            _packetBuilder.SetCipher(rc, _param.CheckMACError);
        }

        private DataFragment ReceivePacket() {
            while(true) {
                DataFragment data = _packetReceiver.WaitResponse();

                PacketType pt = (PacketType)data.ByteAt(0); //shortcut
                if(pt==PacketType.SSH_MSG_IGNORE) {
                    SSH1DataReader r = new SSH1DataReader(data);
                    r.ReadPacketType();
                    if(_eventReceiver!=null) _eventReceiver.OnIgnoreMessage(r.ReadString());
                }
                else if(pt==PacketType.SSH_MSG_DEBUG) {
                    SSH1DataReader r = new SSH1DataReader(data);
                    r.ReadPacketType();
                    if(_eventReceiver!=null) _eventReceiver.OnDebugMessage(false, r.ReadString());
                }
                else
                    return data;
            }
        }
    
        internal void AsyncReceivePacket(DataFragment data) {
            try {
                int len = 0, channel = 0;
                SSH1DataReader re = new SSH1DataReader(data);
                PacketType pt = re.ReadPacketType();
                switch(pt) {
                    case PacketType.SSH_SMSG_STDOUT_DATA:
                        len = re.ReadInt32();
                        _channel_collection.FindChannelEntry(_shellID).Receiver.OnData(re.Image, re.Offset, len);
                        break;
                    case PacketType.SSH_SMSG_STDERR_DATA: {
                        _channel_collection.FindChannelEntry(_shellID).Receiver.OnExtendedData((int)PacketType.SSH_SMSG_STDERR_DATA, re.ReadString());
                    }
                        break;
                    case PacketType.SSH_MSG_CHANNEL_DATA:
                        channel = re.ReadInt32();
                        len = re.ReadInt32();
                        _channel_collection.FindChannelEntry(channel).Receiver.OnData(re.Image, re.Offset, len);
                        break;
                    case PacketType.SSH_MSG_PORT_OPEN:
                        ProcessPortforwardingRequest(_eventReceiver, re);
                        break;
                    case PacketType.SSH_MSG_CHANNEL_CLOSE: {
                        channel = re.ReadInt32();
                        ISSHChannelEventReceiver r = _channel_collection.FindChannelEntry(channel).Receiver;
                        _channel_collection.UnregisterChannelEventReceiver(channel);
                        r.OnChannelClosed();
                    }
                        break;
                    case PacketType.SSH_MSG_CHANNEL_CLOSE_CONFIRMATION:
                        channel = re.ReadInt32();
                        break;
                    case PacketType.SSH_MSG_DISCONNECT:
                        _eventReceiver.OnConnectionClosed();
                        break;
                    case PacketType.SSH_SMSG_EXITSTATUS:
                        _channel_collection.FindChannelEntry(_shellID).Receiver.OnChannelClosed();
                        break;
                    case PacketType.SSH_MSG_DEBUG:
                        _eventReceiver.OnDebugMessage(false, re.ReadString());
                        break;
                    case PacketType.SSH_MSG_IGNORE:
                        _eventReceiver.OnIgnoreMessage(re.ReadString());
                        break;
                    case PacketType.SSH_MSG_CHANNEL_OPEN_CONFIRMATION: {
                        int local = re.ReadInt32();
                        int remote = re.ReadInt32();
                        _channel_collection.FindChannelEntry(local).Receiver.OnChannelReady();
                    }
                        break;
                    case PacketType.SSH_SMSG_SUCCESS:
                        if(_executingShell) {
                            ExecShell();
                            _channel_collection.FindChannelEntry(_shellID).Receiver.OnChannelReady();
                            _executingShell = false;
                        }
                        break;
                    default:
                        _eventReceiver.OnUnknownMessage((byte)pt, re.ReadAll());
                        break;
                }
            }
            catch(Exception ex) {
                _eventReceiver.OnError(ex);
            }
        }

        private PacketType SneakPacketType(DataFragment data) {
            return (PacketType)data.ByteAt(0);
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

    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class SSH1Channel : SSHChannel {

        private SSH1Connection _connection;
        
        public SSH1Channel(SSH1Connection con, ChannelType type, int local_id) : base(con, type, local_id) {
            _connection = con;
        }
        public SSH1Channel(SSH1Connection con, ChannelType type, int local_id, int remote_id) : base(con, type, local_id) {
            _connection = con;
            _remoteID = remote_id;
        }

        /**
         * resizes the size of terminal
         */
        public override void ResizeTerminal(int width, int height, int pixel_width, int pixel_height) {
            SSH1DataWriter writer = new SSH1DataWriter();
            writer.Write(height);
            writer.Write(width);
            writer.Write(pixel_width);
            writer.Write(pixel_height);
            SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_WINDOW_SIZE, writer.ToByteArray());
            Transmit(p);
        }

        /**
        * transmits channel data 
        */
        public override void Transmit(byte[] data) {
            SSH1DataWriter wr = new SSH1DataWriter();
            if(_type==ChannelType.Shell) {
                wr.WriteAsString(data);
                SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_STDIN_DATA, wr.ToByteArray());
                Transmit(p);
            }
            else {
                wr.Write(_remoteID);
                wr.WriteAsString(data);
                SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_MSG_CHANNEL_DATA, wr.ToByteArray());
                Transmit(p);
            }
        }
        /**
        * transmits channel data 
        */
        public override void Transmit(byte[] data, int offset, int length) {
            SSH1DataWriter wr = new SSH1DataWriter();
            if(_type==ChannelType.Shell) {
                wr.WriteAsString(data, offset, length);
                SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_STDIN_DATA, wr.ToByteArray());
                Transmit(p);
            }
            else {
                wr.Write(_remoteID);
                wr.WriteAsString(data, offset, length);
                SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_MSG_CHANNEL_DATA, wr.ToByteArray());
                Transmit(p);
            }
        }

        public override void SendEOF() {
        }


        /**
         * closes this channel
         */
        public override void Close() {
            if(!_connection.IsOpen) return;

            if(_type==ChannelType.Shell) {
                SSH1DataWriter wr2 = new SSH1DataWriter();
                wr2.Write(_remoteID);
                SSH1Packet p2 = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_EOF, wr2.ToByteArray());
                Transmit(p2);
            }

            SSH1DataWriter wr = new SSH1DataWriter();
            wr.Write(_remoteID);
            SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_MSG_CHANNEL_CLOSE, wr.ToByteArray());
            Transmit(p);
        }

        private void Transmit(SSH1Packet p) {
            _connection.Transmit(p);
        }

    }

    //if port forwardings are performed without a shell, we use SSH1DummyChannel to receive shell data
    internal class SSH1DummyReceiver : ISSHChannelEventReceiver {
        public void OnData(byte[] data, int offset, int length) {
        }
        public void OnExtendedData(int type, byte[] data) {
        }
        public void OnChannelClosed() {
        }
        public void OnChannelEOF() {
        }
        public void OnChannelReady() {
        }
        public void OnChannelError(Exception error) {
        }
        public void OnMiscPacket(byte packet_type, byte[] data, int offset, int length) {
        }
    }
}
