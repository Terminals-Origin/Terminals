using System;
using System.Net.Sockets;

namespace Terminals.Scanner
{
    internal class ConnectionState
    {
        internal Int32 Port { get; private set; }
        internal TcpClient Client { get; private set; }
        internal Boolean Done { get; set; }

        internal ConnectionState(Int32 port, TcpClient client)
        {
            this.Port = port;
            this.Client = client;
        }
    }
}
