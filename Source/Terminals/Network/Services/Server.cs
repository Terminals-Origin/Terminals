using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Terminals.Data;
using Unified;

namespace Terminals.Network
{
    internal class Server
    {
        private readonly IPersistence persistence;

        private readonly TcpListener server = new TcpListener(IPAddress.Any, SERVER_PORT);
        internal const int SERVER_PORT = 1216;
        public bool ServerOnline { get; private set; }

        public Server(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        public void Stop()
        {
            ServerOnline = false;
        }

        public void Start()
        {
            ServerOnline = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartServer), null);
        }

        private static void FinishDisconnect(Socket incomingSocket)
        {
            incomingSocket.Disconnect(true);
        }

        private void StartServer(object data)
        {
            try
            {
                while (ServerOnline)
                {
                    this.server.Start();
                    Socket incomingSocket = this.server.AcceptSocket();
                    byte[] received = new byte[512];
                    incomingSocket.Receive(received, received.Length, 0);
                    string userName = Encoding.Default.GetString(received);
                    SendFavorites(incomingSocket);
                }
                this.server.Stop();
            }
            catch (Exception exc)
            {
                Logging.Error("StartServer", exc);
            }
        }

        private void SendFavorites(Socket incomingSocket)
        {
            ArrayList list = FavoritesToSharedList();
            Byte[] data = SharedListToBinaryData(list);
            incomingSocket.Send(data);
            FinishDisconnect(incomingSocket);
        }

        private ArrayList FavoritesToSharedList()
        {
            IFavorites favoritesToShare = this.persistence.Favorites;
            ArrayList list = new ArrayList();
            
            foreach (IFavorite favorite in favoritesToShare)
            {
                FavoriteConfigurationElement configFavorite = ModelConverterV2ToV1.ConvertToFavorite(favorite, persistence);
                list.Add(SharedFavorite.ConvertFromFavorite(this.persistence, configFavorite));
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
