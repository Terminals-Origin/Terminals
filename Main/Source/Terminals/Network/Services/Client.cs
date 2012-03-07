using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Unified;

namespace Terminals.Network
{
    internal delegate void ServerConnectionHandler(ShareFavoritesEventArgs args);
    
    internal class Client
    {
        public static event ServerConnectionHandler OnServerConnection;
        private static TcpClient client;

        public static void Stop()
        {
            try
            {
                if (client != null && client.Connected)
                    client.Close();
            }
            catch (Exception exception)
            {
                Logging.Log.Fatal("Client - Stop", exception);
            }
        }

        public static void Start(string server)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartClient), server);
        }

        private static void StartClient(object data)
        {
            try
            {
                string server = data as string;
                client = new TcpClient(server, Server.ServerPort);
                NetworkStream networkStream = client.GetStream();
                SendUserName(networkStream);
                ReceiveData(networkStream);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Could not connect to the remote server.  Make sure the server is online.", exception);
                MessageBox.Show("Could not connect to the remote server.  Make sure the server is online.");
            }
        }

        private static void ReceiveData(NetworkStream networkStream)
        {
            while (networkStream.CanRead)
            {
                if (networkStream.DataAvailable)
                {
                    byte[] received = new byte[4096];
                    int dataLength = networkStream.Read(received, 0, received.Length);
                    MemoryStream memoryStream = new MemoryStream(received, 0, dataLength);
                    networkStream.Close();
                    FireDataReceived(memoryStream);
                }
            }
        }

        private static void FireDataReceived(MemoryStream stream)
        {
            if (OnServerConnection != null)
            {
               SortableList<FavoriteConfigurationElement> favorites = ReceiveFavorites(stream);
               OnServerConnection(new ShareFavoritesEventArgs(favorites));
            }
        }

        private static void SendUserName(NetworkStream networkStream)
        {
            if (networkStream.CanWrite)
            {
                byte[] userName = Encoding.Default.GetBytes(Environment.UserName);
                networkStream.Write(userName, 0, userName.Length);
                networkStream.Flush();
            }
        }

        private static SortableList<FavoriteConfigurationElement> ReceiveFavorites(MemoryStream Response)
        {
            if (Response.Length != 0)
            {
                ArrayList favorites = (ArrayList)Serialize.DeSerializeBinary(Response);
                return GetReceivedFavorites(favorites);
            }

            return new SortableList<FavoriteConfigurationElement>();
        }

        private static SortableList<FavoriteConfigurationElement> GetReceivedFavorites(ArrayList favorites)
        {
            var importedFavorites = new SortableList<FavoriteConfigurationElement>();
            foreach (object item in favorites)
            {
                SharedFavorite favorite = item as SharedFavorite;
                if (favorite != null)
                    importedFavorites.Add(ImportSharedFavorite(favorite));
            }
            return importedFavorites;
        }

        private static FavoriteConfigurationElement ImportSharedFavorite(SharedFavorite favorite)
        {
            FavoriteConfigurationElement newfav = SharedFavorite.ConvertFromFavorite(favorite);
            newfav.UserName = Environment.UserName;
            newfav.DomainName = Environment.UserDomainName;
            return newfav;
        }
    }
}