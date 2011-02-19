using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Terminals.Network {
    public class Server {

        private static int _serverPort = 1216;
        private static bool _serverOnline = false;
        //static TcpListener server = new TcpListener(ServerPort);
        private static TcpListener _server = new TcpListener(IPAddress.Any, _serverPort);

        public delegate void ClientConnection(string Username, Socket Socket);
        public static event ClientConnection OnClientConnection;

        public static int ServerPort
        {
            get { return Server._serverPort; }
            //set { Server._serverPort = value; }
        }

        public static bool ServerOnline
        {
            get { return Server._serverOnline; }
            //set { Server._serverOnline = value; }
        }

        public static void Stop() 
        {
            _serverOnline = false;
        }

        public static void Start() 
        {
            _serverOnline = true;
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(StartServer), null);
        }
        public static void FinishDisconnect(Socket inSock) 
        {
            inSock.Disconnect(true);
        }
        private static void StartServer(object data)
        {
            try {
                while (_serverOnline) {
                    _server.Start();
                    Socket inSock = _server.AcceptSocket();
                    byte[] rcvd = new byte[512];
                    int rcvdLen = inSock.Receive(rcvd, rcvd.Length, 0);
                    string incoming = System.Text.Encoding.Default.GetString(rcvd);
                    if (OnClientConnection != null)
                        OnClientConnection(incoming, inSock);
                }
                _server.Stop();
            } catch (Exception exc) {
                Terminals.Logging.Log.Error("StartServer", exc);
            }
        }
    }
}
