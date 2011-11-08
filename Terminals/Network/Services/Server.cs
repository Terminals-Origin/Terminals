using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Terminals.Configuration;
using Unified;

namespace Terminals.Network
{
    internal class Server
    {
        private static TcpListener _server = new TcpListener(IPAddress.Any, ServerPort);
        internal const int ServerPort = 1216;
        public static bool ServerOnline { get; private set; }

        public static void Stop()
        {
            ServerOnline = false;
        }

        public static void Start()
        {
            ServerOnline = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartServer), null);
        }

        public static void FinishDisconnect(Socket incomingSocket)
        {
            incomingSocket.Disconnect(true);
        }

        private static void StartServer(object data)
        {
            try
            {
                while (ServerOnline)
                {
                    _server.Start();
                    Socket incomingSocket = _server.AcceptSocket();
                    byte[] received = new byte[512];
                    incomingSocket.Receive(received, received.Length, 0);
                    string userName = Encoding.Default.GetString(received);
                    SendFavorites(incomingSocket);
                }
                _server.Stop();
            }
            catch (Exception exc)
            {
                Logging.Log.Error("StartServer", exc);
            }
        }

        private static void SendFavorites(Socket incomingSocket)
        {
            ArrayList list = FavoritesToSharedList();
            Byte[] data = SharedListToBinaryData(list);
            incomingSocket.Send(data);
            FinishDisconnect(incomingSocket);
        }

        private static ArrayList FavoritesToSharedList()
        {
            var favoritesToShare = Settings.GetFavorites();
            ArrayList list = new ArrayList();
            foreach (FavoriteConfigurationElement elem in favoritesToShare)
            {
                list.Add(SharedFavorite.ConvertFromFavorite(elem));
            }
            return list;
        }

        private static Byte[] SharedListToBinaryData(ArrayList favorites)
        {
            MemoryStream favs = Serialize.SerializeBinary(favorites);

            if (favs != null && favs.Length > 0)
            {
                if (favs.CanRead && favs.Position > 0)
                    favs.Position = 0;
                Byte[] data = favs.ToArray();
                favs.Close();
                favs.Dispose();
                return data;
            }

            return new byte[0];
        }
    }
}
