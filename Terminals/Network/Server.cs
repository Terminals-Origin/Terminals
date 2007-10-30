using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Terminals.Network {
    public class Server {

        public static int ServerPort = 1216;
        public delegate void ClientConnection(string Username, Socket Socket);
        public static event ClientConnection OnClientConnection;
        public static bool ServerOnline = false;
        static TcpListener server = new TcpListener(ServerPort);
        public static void Stop() {
            ServerOnline = false;
        }

        public static void Start() {
            ServerOnline = true;
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(StartServer), null);
        }
        public static void FinishDisconnect(Socket inSock) {
            inSock.Disconnect(true);
        }
        private static void StartServer(object data) {
            try {
                while (ServerOnline) {
                    server.Start();
                    Socket inSock = server.AcceptSocket();
                    byte[] rcvd = new byte[512];
                    int rcvdLen = inSock.Receive(rcvd, rcvd.Length, 0);
                    string incoming = System.Text.Encoding.Default.GetString(rcvd);
                    if (OnClientConnection != null) {
                        OnClientConnection(incoming, inSock);
                    }

                }
                server.Stop();
            } catch (Exception exc) {
                Terminals.Logging.Log.Info("", exc);
                string f = exc.ToString();
            }
        }
    }
}
