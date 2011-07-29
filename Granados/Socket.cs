using System;
using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Granados.Util;

namespace Granados.IO
{
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum SocketStatus {
        Ready,            
        Negotiating,       //preparing for connection
        RequestingClose,   //the client is requesting termination
        Closed,            //closed
        Unknown
    }

    //interface to receive data through AbstractGranadosSocket asynchronously
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IDataHandler {
        void OnData(DataFragment data);
        void OnClosed();
        void OnError(Exception error);
    }

    //System.IO.SocketÇ∆IChannelEventReceiverÇíäè€âªÇ∑ÇÈ
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public abstract class AbstractGranadosSocket {
        protected IDataHandler _handler;
        protected SocketStatus _socketStatus;

        protected AbstractGranadosSocket(IDataHandler h) {
            _handler = h;
            _single = new byte[1];
            _socketStatus = SocketStatus.Unknown;
        }

        public SocketStatus SocketStatus {
            get {
                return _socketStatus;
            }
        }
        public IDataHandler DataHandler {
            get {
                return _handler;
            }
        }

        public void SetHandler(IDataHandler h) {
            _handler = h;
        }

        internal abstract void Write(byte[] data, int offset, int length);

        private byte[] _single;
        internal void WriteByte(byte data) {
            _single[0] = data;
            Write(_single, 0, 1);
        }

        internal abstract void Close();
        internal abstract bool DataAvailable { get; }
    }

    // base class for processing data and passing another IDataHandler
    internal abstract class FilterDataHandler : IDataHandler {
        protected IDataHandler _inner_handler;

        public FilterDataHandler(IDataHandler inner_handler) {
            _inner_handler = inner_handler;
        }
        public IDataHandler InnerHandler {
            get {
                return _inner_handler;
            }
            set {
                _inner_handler = value;
            }
        }

        public abstract void OnData(DataFragment data);

        public virtual void OnClosed() {
            _inner_handler.OnClosed();
        }
        public virtual void OnError(Exception error) {
            _inner_handler.OnError(error);
        }
    }

    //Handler for receiving the response synchronously
    internal abstract class SynchronizedDataHandler : IDataHandler {

        private AbstractGranadosSocket _socket;
        private ManualResetEvent _event;
        private Queue _results;

        public SynchronizedDataHandler(AbstractGranadosSocket socket) {
            _socket = socket;
            _event = new ManualResetEvent(false);
            _results = new Queue();
        }

        public void Close() {
            _event.Close();
        }

        public void OnData(DataFragment data) {
            lock(_socket) {
                OnDataInLock(data);
            }
        }
        public void OnClosed() {
            lock(_socket) {
                OnClosedInLock();
            }
        }
        public void OnError(Exception error) {
            lock(_socket) {
                OnErrorInLock(error);
            }
        }

        protected abstract void OnDataInLock(DataFragment data);
        protected virtual void OnClosedInLock() {
            SetFailureResult(new SSHException("the connection is closed with unexpected condition."));
        }
        protected virtual void OnErrorInLock(Exception error) {
            SetFailureResult(error);
        }

        //Set the response
        protected void SetSuccessfulResult(DataFragment data) {
            _results.Enqueue(data.Isolate());
            _event.Set();
        }
        protected void SetFailureResult(Exception error) {
            _results.Enqueue(error);
            _event.Set();
        }

        //Send request and wait response
        public DataFragment SendAndWaitResponse(DataFragment data) {
            //this lock is important
            lock(_socket) {
                Debug.Assert(_results.Count==0);
                _event.Reset();
                if(data.Length>0) _socket.Write(data.Data, data.Offset, data.Length);
            }

            _event.WaitOne();
            Debug.Assert(_results.Count>0);
            return Dequeue();
        }

        //asynchronously data exchange
        public DataFragment WaitResponse() {
            lock(_socket) {
                if(_results.Count>0)
                    return Dequeue(); //we have data already 
                else
                    _event.Reset();
            }

            _event.WaitOne();
            return Dequeue();
        }

        //Pop the data from the queue
        private DataFragment Dequeue() {
            lock(_socket) {
                object t = _results.Dequeue();
                Debug.Assert(t!=null);

                DataFragment d = t as DataFragment;
                if(d!=null)
                    return d;
                else {
                    Exception e = t as Exception;
                    Debug.Assert(e!=null);
                    throw e;
                }
            }
        }
    }



    // Connecting to SSH daemon
    internal class VersionExchangeHandler : SynchronizedDataHandler {
        private SSHConnectionParameter _param;
        private string _serverVersion;

        public VersionExchangeHandler(SSHConnectionParameter param, AbstractGranadosSocket socket) : base(socket) {
            _param = param;
        }

        public string ServerVersion {
            get {
                return _serverVersion;
            }
        }

        protected override void OnDataInLock(DataFragment data) {
            try {
                //the specification claims the version string ends with CRLF, however some servers send LF only
                if(data.Length<=2 || data.Data[data.Offset+data.Length-1]!=0x0A) throw new SSHException(Strings.GetString("NotSSHServer"));
                //Debug.WriteLine(String.Format("receiveServerVersion len={0}",len));

                //this Trim() is necessary for computing hash in the host key authentication stage
                string sv = Encoding.ASCII.GetString(data.Data, data.Offset, data.Length).Trim();

                //check compatibility
                int a = sv.IndexOf('-');
                if(a==-1) ThrowUnexpectedFormatException(sv);
                int b = sv.IndexOf('-', a+1);
                if(b==-1) ThrowUnexpectedFormatException(sv);
                int comma = sv.IndexOf('.', a, b-a);
                if(comma==-1) ThrowUnexpectedFormatException(sv);

                int major = Int32.Parse(sv.Substring(a+1, comma-a-1));
                int minor = Int32.Parse(sv.Substring(comma+1, b-comma-1));

                if(_param.Protocol==SSHProtocol.SSH1) {
                    if(major!=1) ThrowVersionMismatchException(sv, SSHProtocol.SSH1);
                }
                else {
                    if(major>=3 || major<=0 || (major==1 && minor!=99)) ThrowVersionMismatchException(sv, SSHProtocol.SSH2);
                }

                _serverVersion = sv;
                this.SetSuccessfulResult(data);
            }
            catch(Exception ex) {
                this.SetFailureResult(ex);
            }
        }
        private void ThrowSSHException(string msg) {
            throw new SSHException(msg);
        }
        private void ThrowUnexpectedFormatException(string version) {
            ThrowSSHException("Format of server version is invalid:[" + version + "]");
        }
        private void ThrowVersionMismatchException(string version, SSHProtocol client) {
            StringBuilder bld = new StringBuilder();
            bld.Append("The protocol version of the server [");
            bld.Append(version);
            bld.Append("] is not compatible with ");
            bld.Append(client.ToString());
            ThrowSSHException(bld.ToString());
        }

    }

    //directly notification to synchronized
    internal class SynchronizedPacketReceiver : SynchronizedDataHandler {

        private SSHConnection _connection;

        public SynchronizedPacketReceiver(SSHConnection c) : base(c.UnderlyingStream) {
            _connection = c;
        }

        protected override void OnDataInLock(DataFragment data) {
            this.SetSuccessfulResult(data);
        }
    }

    // GranadosSocket on an underlying .NET socket
    internal class PlainSocket : AbstractGranadosSocket {
        private Socket _socket;
        private DataFragment _data;

        private AsyncCallback _callback;

        internal PlainSocket(Socket s, IDataHandler h) : base(h) {
            _socket = s;
            Debug.Assert(_socket.Connected);
            _socketStatus = SocketStatus.Ready;

            _data = new DataFragment(0x1000);
            _callback = new AsyncCallback(RepeatCallback);
        }
        
        internal override void Write(byte[] data, int offset, int length) {
            _socket.Send(data, offset, length, SocketFlags.None);
        }

        internal override void Close() {
            if(_socketStatus!=SocketStatus.Closed) {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socketStatus = SocketStatus.Closed;
            }
        }
        
        internal void RepeatAsyncRead() {
            _socket.BeginReceive(_data.Data, 0, _data.Capacity, SocketFlags.None, _callback, null);
        }

        internal override bool DataAvailable {
            get {
                return _socket.Available>0;
            }
        }

        private void RepeatCallback(IAsyncResult result) {
            try {
                int n = _socket.EndReceive(result);
                if(n > 0) {
                    _data.SetLength(0, n);
                    _handler.OnData(_data);
                    if(_socketStatus!=SocketStatus.Closed)
                        RepeatAsyncRead();
                }
                else if(n < 0) {
                    //in case of Win9x, EndReceive() returns 0 every 288 seconds even if no data is available
                    RepeatAsyncRead();
                }
                else
                    _handler.OnClosed();
            }
            catch(Exception ex) {
                if((ex is SocketException) && ((SocketException)ex).ErrorCode==995) {
                    //in case of .NET1.1 on Win9x, EndReceive() changes the behavior. it throws SocketException with an error code 995. 
                    RepeatAsyncRead();
                }
                else if(_socketStatus!=SocketStatus.Closed)
                    _handler.OnError(ex);
            }
        }
    }

    // GranadosSocket on an underlying another SSH channel
    internal class ChannelSocket : AbstractGranadosSocket, ISSHChannelEventReceiver {
        private SSHChannel _channel;
        private DataFragment _fragment;

        internal ChannelSocket(IDataHandler h) : base(h) {
        }
        internal SSHChannel SSHChennal {
            get {
                return _channel;
            }
            set {
                _channel = value;
                _socketStatus = SocketStatus.Negotiating;
            }
        }

        internal override void Write(byte[] data, int offset, int length) {
            if(_socketStatus!=SocketStatus.Ready) throw new SSHException("channel not ready");
            _channel.Transmit(data, offset, length);
        }
        internal override bool DataAvailable {
            get {
                //Note: this may be not correct
                return _channel.Connection.Available;
            }
        }

        internal override void Close() {
            if(_socketStatus!=SocketStatus.Ready) throw new SSHException("channel not ready");

            _channel.Close();
            if(_channel.Connection.ChannelCollection.Count<=1) //close last channel
                _channel.Connection.Close();
        }

        public void OnData(byte[] data, int offset, int length) {
            if(_fragment==null)
                _fragment = new DataFragment(data, offset, length);
            else
                _fragment.Init(data, offset, length);

            _handler.OnData(_fragment);
        }

        public void OnChannelEOF() {
            _handler.OnClosed();
        }

        public void OnChannelError(Exception error) {
            _handler.OnError(error);
        }

        public void OnChannelClosed() {
            _handler.OnClosed();
        }

        public void OnChannelReady() {
            _socketStatus = SocketStatus.Ready;
        }

        public void OnExtendedData(int type, byte[] data) {
            //!!handle data
        }
        public void OnMiscPacket(byte type, byte[] data, int offset, int length) {
            //!!handle data
        }
    }
}
