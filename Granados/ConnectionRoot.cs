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
using System.Net.Sockets;
using System.Text;
using Granados.SSH1;
using Granados.SSH2;
using Granados.IO;
using Granados.Util;
using System.Diagnostics;

namespace Granados
{
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public abstract class SSHConnection {

        protected SSHConnectionParameter _param;              //parameters supplied by the user
        protected ChannelCollection _channel_collection;      //channels
        protected AbstractGranadosSocket _stream;             //underlying socket
        protected ISSHConnectionEventReceiver _eventReceiver; //outgoing interface for this connection
        protected byte[] _sessionID;                          //session ID
        protected bool _autoDisconnect;                       //if this is true, this connection will be closed with the last channel
        protected AuthenticationResult _authenticationResult; //authentication result

        // for scp
        protected string _execCmd;        // exec command string
        protected bool _execCmdWaitFlag;  // wait response flag for sending exec command to server

        protected SSHConnection(SSHConnectionParameter param, AbstractGranadosSocket strm, ISSHConnectionEventReceiver receiver) {
            _param = (SSHConnectionParameter)param.Clone();
            _stream = strm;
            _eventReceiver = receiver;
            _channel_collection = new ChannelCollection();
            _autoDisconnect = true;
            _execCmd = null;
            _execCmdWaitFlag = true;
        }


        ///abstract properties

        // stream->packet converter
        internal abstract IDataHandler PacketBuilder {
            get ;
        }

        // connection information such as algorithm names
        public abstract SSHConnectionInfo ConnectionInfo {
            get;
        }

        ///  paramters
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
        public ISSHConnectionEventReceiver EventReceiver {
            get {
                return _eventReceiver;
            }
            set {
                _eventReceiver = value;
            }
        }
        public SocketStatus SocketStatus {
            get {
                return _stream.SocketStatus;
            }
        }
        public bool IsOpen {
            get {
                return _stream.SocketStatus==SocketStatus.Ready && _authenticationResult==AuthenticationResult.Success;
            }
        }
        public bool IsEventTracerAvailable {
            get {
                return _param.EventTracer!=null;
            }
        }
        internal AbstractGranadosSocket UnderlyingStream {
            get {
                return _stream;
            }
        }
        internal ChannelCollection ChannelCollection {
            get {
                return _channel_collection;
            }
        }

        //configurable properties
        public bool AutoDisconnect {
            get {
                return _autoDisconnect;
            }
            set {
                _autoDisconnect = value;
            }
        }
        
        
        //returns true if any data from server is available
        public bool Available {
            get {
                if(_stream.SocketStatus!=SocketStatus.Ready)
                    return false;
                else
                    return _stream.DataAvailable;
            }
        }


        /**
         * internal procedure for opening connection
         */
        internal abstract AuthenticationResult Connect();

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


        public void ExecuteSCP(ScpParameter scp_param) {
            // making command string for scp
            string cmd = String.Format("scp {0} {1}", scp_param.Direction==SCPCopyDirection.LocalToRemote? "-t" : "-f", scp_param.RemoteFilename);
            ScpChannelReceiverBase receiver_base;

            if(scp_param.Direction==SCPCopyDirection.LocalToRemote) {
                ScpLocalToRemoteReceiver receiver = new ScpLocalToRemoteReceiver(scp_param);
                receiver_base = receiver;
                // exec command
                receiver.Run(DoExecCommand(receiver, cmd));
            }
            else {
                ScpRemoteToLocalReceiver receiver = new ScpRemoteToLocalReceiver(scp_param);
                receiver_base = receiver;
                // exec command
                receiver.Run(DoExecCommand(receiver, cmd));
                //Note: asynchronously operation should be supported
                receiver.CompleteEvent.WaitOne();
            }

            Debug.Assert(scp_param.LocalSource.IsClosed); //the local source must be closed regardless of the transmission

            receiver_base.Dipose();

            if(!receiver_base.Succeeded) throw receiver_base.Error;
        }


        public abstract SSHChannel DoExecCommand(ISSHChannelEventReceiver receiver, string command);

        /**
        * closes socket directly.
        */
        public void Close() {
            if(_stream.SocketStatus==SocketStatus.Closed || _stream.SocketStatus==SocketStatus.RequestingClose) return;
            _stream.Close();
        } 


        /**
         * sends ignorable data: the server may record the message into the log
         */
        public abstract void SendIgnorableData(string msg);


        /**
         * opens another SSH connection via port-forwarded connection
         */ 
        public SSHConnection OpenPortForwardedAnotherConnection(SSHConnectionParameter param, ISSHConnectionEventReceiver receiver, string host, int port) {
            ChannelSocket s = new ChannelSocket(null);
            SSHChannel ch = ForwardPort(s, host, port, "localhost", 0);
            s.SSHChennal = ch;
            VersionExchangeHandler pnh = new VersionExchangeHandler(param, s);
            s.SetHandler(pnh);

            return ConnectMain(param, receiver, pnh, s);
        }


        /**
         * open a new SSH connection via the .NET socket
         */
        public static SSHConnection Connect(SSHConnectionParameter param, ISSHConnectionEventReceiver receiver, Socket underlying_socket) {
            if(param.UserName==null) throw new InvalidOperationException("UserName property is not set");
            if(param.AuthenticationType!=AuthenticationType.KeyboardInteractive && param.Password==null) throw new InvalidOperationException("Password property is not set");

            PlainSocket s = new PlainSocket(underlying_socket, null);
            VersionExchangeHandler pnh = new VersionExchangeHandler(param, s);
            s.SetHandler(pnh);
            s.RepeatAsyncRead();
            return ConnectMain(param, receiver, pnh, s);
        }

        private static SSHConnection ConnectMain(SSHConnectionParameter param, ISSHConnectionEventReceiver receiver, VersionExchangeHandler pnh, AbstractGranadosSocket s) {
            DataFragment data = pnh.WaitResponse();
            string sv = pnh.ServerVersion;

            SSHConnection con = null;
            if(param.Protocol==SSHProtocol.SSH1)
                con = new SSH1Connection(param, s, receiver, sv, SSHUtil.ClientVersionString(param.Protocol));
            else
                con = new SSH2Connection(param, s, receiver, sv, SSHUtil.ClientVersionString(param.Protocol));

            con.TraceReceptionEvent("server version-string", sv.Trim());
            pnh.Close();
            s.SetHandler(con.PacketBuilder);
            con.SendMyVersion(param);

            if(con.Connect()!=AuthenticationResult.Failure) {
                return con;
            }
            else {
                s.Close();
                return null;
            }
        }

        private void SendMyVersion(SSHConnectionParameter param) {
            string cv = SSHUtil.ClientVersionString(param.Protocol);
            string cv2 = cv + param.VersionEOL;
            byte[] data = Encoding.ASCII.GetBytes(cv2);
            _stream.Write(data, 0, data.Length);
            TraceTransmissionEvent("client version-string", cv);
        }

        internal void TraceTransmissionEvent(string name, string message, params object[] args) {
            ISSHEventTracer t = _param.EventTracer;
            if(t!=null) t.OnTranmission(name, String.Format(message, args));
        }
        internal void TraceReceptionEvent(string name, string message, params object[] args) {
            ISSHEventTracer t = _param.EventTracer;
            if(t!=null) t.OnReception(name, String.Format(message, args));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum ChannelType {
        Session,
        Shell,
        ForwardedLocalToRemote,
        ForwardedRemoteToLocal,
        ExecCommand,  // for scp
        Subsystem,
        AgentForward
    }

    /**
     * the base class for SSH channels
     */
    /// <exclude/>
    public abstract class SSHChannel {
        protected ChannelType _type;
        protected int _localID;
        protected int _remoteID;
        private SSHConnection _connection;

        protected SSHChannel(SSHConnection con, ChannelType type, int local_id) {
            con.ChannelCollection.RegisterChannel(local_id, this);
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
