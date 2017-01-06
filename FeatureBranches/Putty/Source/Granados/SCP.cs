using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Text;

namespace Granados {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processed_length"></param>
    /// <param name="total_length"></param>
    /// <exclude/>
    public delegate void SCPProgression(int processed_length, int total_length);

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum SCPCopyDirection {
        LocalToRemote,
        RemoteToLocal
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class ScpLocalSource {
        private Stream _stream;
        private bool _closed;

        public ScpLocalSource(string filename) {
            _stream = new FileStream(filename, FileMode.OpenOrCreate);
        }
        public ScpLocalSource(Stream source) {
            _stream = source;
        }

        public Stream Stream {
            get {
                return _stream;
            }
        }

        public void Close() {
            if(!_closed)
                _stream.Close();
            _closed = true;
        }
        public bool IsClosed {
            get {
                return _closed;
            }
        }
    }

    /// <summary>
    /// Fill the properties of ScpParameter object before you start the connection with SCP protocol.
    /// </summary>
    /// <exclude/>
    public class ScpParameter {
        // NECESSARY: filename included scp command string
        private string _remoteFilename;
        public string RemoteFilename {
            get {
                return _remoteFilename;
            }
            set {
                _remoteFilename = value;
            }
        }

        // NECESSARY: Local filename
        // NOTE: Store null to this variable if you want to use local memory(_iostream).
        private ScpLocalSource _localSource;
        public ScpLocalSource LocalSource {
            get {
                return _localSource;
            }
            set {
                _localSource = value;
            }
        }

        // NECESSARY: Local to Remote direction flag
        private SCPCopyDirection _direction;
        public SCPCopyDirection Direction {
            get {
                return _direction;
            }
            set {
                _direction = value;
            }
        }

        // OPTIONAL: sending file permission(DDDD: ex.0644)
        private string _permission;
        public string Permission {
            get {
                return _permission;
            }
            set {
                _permission = value;
            }
        }

        private SCPProgression _progressionDelagete;
        public SCPProgression ProgressionDelegate {
            get {
                return _progressionDelagete;
            }
            set {
                _progressionDelagete = value;
            }
        }

        // recursive copy flag(TODO)
        private bool _recursive;
        public bool Recursive {
            get {
                return _recursive;
            }
            set {
                _recursive = value;
            }
        }

        // timestamp preservation flag(TODO)
        private bool _timestamp;
        public bool Timestamp {
            get {
                return _timestamp;
            }
            set {
                _timestamp = value;
            }
        }

        // directory flag(TODO)
        private bool _directory;
        public bool Directory {
            get {
                return _directory;
            }
            set {
                _directory = value;
            }
        }

        // verbose mode flag(TODO)
        private bool _verbose;
        public bool Verbose {
            get {
                return _verbose;
            }
            set {
                _verbose = value;
            }
        }

        public ScpParameter() {
            _remoteFilename = null;
            _localSource = null;
            _permission = "0644";
            _recursive = false;
            _timestamp = false;
            _directory = false;
            _verbose = false;
        }

    }

    internal abstract class ScpChannelReceiverBase : ISSHChannelEventReceiver {
        protected SSHChannel _channel;
        protected ScpParameter _param;

        protected Exception _error;
        protected bool _ready;
        protected bool _abortFlag = false;
        protected ManualResetEvent _completeEvent;
        protected int _processedLength;
        protected int _totalLength;

        public ScpChannelReceiverBase(ScpParameter param) {
            _param = param;
            _completeEvent = new ManualResetEvent(false);
        }

        public bool Succeeded {
            get {
                return _error==null;
            }
        }
        public Exception Error {
            get {
                return _error;
            }
        }
        public ManualResetEvent CompleteEvent {
            get {
                return _completeEvent;
            }
        }

        //we must call either of SuccessfullyExit and UnsuccessfullyExit
        protected void SuccessfullyExit() {
            _error = null;
            Exit();
        }
        protected void UnsuccessfullyExit(Exception ex) {
            _error = ex;
            Exit();
        }

        private void Exit() {
            try {
                _param.LocalSource.Close();
                _channel.Close();
            } finally {
                _completeEvent.Set(); //We must not skip this Set() -- or a deadlock may happen!
            }
        }

        //the user must call Dispose() after receiving the result of the scp operation
        public virtual void Dipose() {
            _completeEvent.Close();
        }

        public abstract void Run(SSHChannel channel); //start scp operation

        //main data handler
        public abstract void OnData(byte[] data, int offset, int length);

        public void OnChannelClosed() {
            Debug.WriteLine("Channel closed");
            //_conn.AsyncReceive(this);
        }
        public void OnChannelEOF() {
            _channel.Close();
            Debug.WriteLine("Channel EOF");
        }
        public void OnExtendedData(int type, byte[] data) {
            string s = Encoding.ASCII.GetString(data);

            Debug.WriteLine("EXTENDED DATA");

            // TODO: 
        }
        public void OnUnknownMessage(byte type, byte[] data) {
            Debug.WriteLine("Unknown Message " + type);
        }
        public virtual void OnChannelReady() {
            _ready = true;
        }
        public void OnChannelError(Exception error) {
            Debug.WriteLine("Channel ERROR: " + error.Message);
        }
        public void OnMiscPacket(byte type, byte[] data, int offset, int length) {
        }

    }

    internal class ScpRemoteToLocalReceiver : ScpChannelReceiverBase {
        private enum State {
            WaitingFileInfo,
            ReceivingContent,
            Completed
        }
        private State _state;

        private ManualResetEvent _channelReadyEvent;

        public ScpRemoteToLocalReceiver(ScpParameter param) : base(param) {
            _state = State.WaitingFileInfo;
            _channelReadyEvent = new ManualResetEvent(false);
        }

        public override void Run(SSHChannel channel) {
            _channel = channel;
            _channelReadyEvent.WaitOne();
            SendResponse(); //これでやりとりがはじまる
        }

        public override void OnData(byte[] data, int offset, int length) {
            // ローカルのファイルおよびメモリへ書き込む
            try {
                if(_state==State.WaitingFileInfo) {
                    ParseFileInfo(Encoding.ASCII.GetString(data, offset, length));
                    if(_state==State.ReceivingContent)
                        SendResponse();
                }
                else if(_state==State.ReceivingContent) {
                    ProcessData(data, offset, length);
                    if(_state==State.Completed) {
                        SendResponse();
                        SuccessfullyExit();
                    }
                }
            } catch(Exception ex) {
                UnsuccessfullyExit(ex);
            }
        }

        private void SendResponse() {
            byte[] t = new byte[1];
            _channel.Transmit(t, 0, 1);
        }

        private void ParseFileInfo(string info) {
            try {
                if(info[0]!='C') 
                    throw new SSHException("mulformed file information: content=" + info);
                int l, r;
                string sperm, slen, sfile;

                l = info.IndexOf(' ');
                r = info.IndexOf(' ', l + 1);
                sperm = info.Substring(1, l);
                slen = info.Substring(l + 1, r - l - 1);
                sfile = info.Substring(r + 1);

                _totalLength = Convert.ToInt32(slen);  // file or iostream length(in byte)
                _processedLength = 0;
                _state = State.ReceivingContent;
            } catch(Exception ex) {
                throw new SSHException("mulformed file information: " + ex.Message + ",content=" + info);
            }
        }

        private void ProcessData(byte[] data, int offset, int length) {
            int next_processed = _processedLength + length;
            if(next_processed > _totalLength) { //at the end of the file content, '\0' is attched
                if(next_processed!=_totalLength+1) throw new SSHException("protocol error: too long stream");
                _state = State.Completed;
                length--;
            }

            _processedLength = next_processed;
            _param.LocalSource.Stream.Write(data, offset, length);
            if(_param.ProgressionDelegate!=null) _param.ProgressionDelegate(_processedLength, _totalLength);
        }

        public override void OnChannelReady() {
            base.OnChannelReady();
            _channelReadyEvent.Set();
        }
        public override void Dipose() {
            base.Dipose();
            _channelReadyEvent.Close();
        }
    }

    internal class ScpLocalToRemoteReceiver : ScpChannelReceiverBase {

        private enum State {
            WaitingChannelReady,
            WaitingFileInfoResponse,
            SendingContent,
            Completed,
            Failed
        }
        private State _state;
        private ManualResetEvent _responseEvent;

        public ScpLocalToRemoteReceiver(ScpParameter param) : base(param) {
            _state = State.WaitingChannelReady;
            _responseEvent = new ManualResetEvent(false);
        }

        public override void Run(SSHChannel channel) {
            _channel = channel;

            try {
                WaitAndReset();
                Debug.Assert(_state==State.Failed || _state==State.WaitingFileInfoResponse);
                if(_state==State.Failed) return;

                TransmitFileInfo();
                WaitAndReset();
                Debug.Assert(_state==State.Failed || _state==State.SendingContent);
                if(_state==State.Failed) return;

                TransmitFileContent();
                WaitAndReset();
                Debug.Assert(_state==State.Failed || _state==State.Completed);

                if(_state==State.Completed) {
                    SuccessfullyExit();
                }
            }
            catch(Exception ex) {
                UnsuccessfullyExit(ex);
            }
            finally {
                _param.LocalSource.Close();
            }
        }

        private void WaitAndReset() {
            _responseEvent.WaitOne();
            _responseEvent.Reset();
        }

        // for Local to Remote
        private void ReadResponse(byte[] data, int offset, int length) {
            byte c = data[offset];
            if (c == 0) { // All is well.
                _error = null;
            }
            else if (c == 255)
                _error = new SSHException("premature EOF");
            else
                _error = new SSHException("readResponse() error");

            if(_state==State.WaitingFileInfoResponse)
                _state = State.SendingContent;
                    
            _responseEvent.Set();
        }

        private void TransmitFileInfo() {
            // sending file permission
            string info = String.Format("C{0} {1} {2}\n", _param.Permission, _param.LocalSource.Stream.Length, _param.RemoteFilename);
            _channel.Transmit(Encoding.ASCII.GetBytes(info));
            _state = State.WaitingFileInfoResponse;
        }

        private void TransmitFileContent() {
            const int BUFSIZE = 4096; //TODO: this size should be configurable
            byte[] buf = new byte[BUFSIZE];
            Stream strm = _param.LocalSource.Stream;
            try {
                int count = 0;
                int total = (int)strm.Length;
                while(count < total) {
                    int size = strm.Read(buf, 0, BUFSIZE);
                    count += size;
                    _channel.Transmit(buf, 0, size);

                    if(_param.ProgressionDelegate!=null) _param.ProgressionDelegate(count, total);
                }

                //EOF
                buf[0] = 0;
                _channel.Transmit(buf, 0, 1);  // null terminate
                _state = State.Completed;
            } catch(Exception ex) {
                _state = State.Failed;
                UnsuccessfullyExit(ex);
            }
        }

        public override void OnChannelReady() {
            base.OnChannelReady();
            _state = State.WaitingFileInfoResponse;
            _responseEvent.Set();
        }

        public override void OnData(byte[] data, int offset, int length) {
            ReadResponse(data, offset, length);
        }

        public override void Dipose() {
            base.Dipose();
            _responseEvent.Close();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
}
