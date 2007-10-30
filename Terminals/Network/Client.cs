using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;


namespace Terminals.Network {
    public class Client {

        public static int ClientPort = 1215;
        public delegate void ServerConnection(System.IO.MemoryStream Response);
        public static event ServerConnection OnServerConnection;
        static TcpClient client;
        public static bool ClientOnline = false;
        public static void Stop() {
            ClientOnline = false;
            client.Close();
        }
        public static void Start(string server) {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(StartClient), server);
        }
        private static void StartClient(object data) {
            try {
                string server = (string)data;
                client = new TcpClient(server, Server.ServerPort);
                NetworkStream nStm = client.GetStream();
                if (nStm.CanWrite) {
                    byte[] dta = System.Text.Encoding.Default.GetBytes(System.Environment.UserName);
                    nStm.Write(dta, 0, (int)dta.Length);
                    nStm.Flush();
                }
                while (nStm.CanRead) {
                    if (nStm.DataAvailable) {
                        byte[] rcvd = new byte[4096];
                        int rcvdCount = nStm.Read(rcvd, 0, rcvd.Length);
                        System.IO.MemoryStream stm = new System.IO.MemoryStream(rcvd, 0, rcvdCount);
                        nStm.Close();
                        if (OnServerConnection != null) {
                            OnServerConnection(stm);
                        }

                    }
                }

            } catch (Exception exc) {
                Terminals.Logging.Log.Info("", exc);
                System.Windows.Forms.MessageBox.Show("Could not connect to the remote server.  Make sure the server is online.");
            }

        }

    }
}
