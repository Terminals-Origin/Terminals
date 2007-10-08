using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.TerminalServices
{
    public class TerminalServer
    {
        private string server;

        public string Server
        {
            get { return server; }
            set { server = value; }
        }
        private bool isConnected;

        public bool IsConnected
        {
            get { return (ServerPointer != System.IntPtr.Zero); ; }
        }
	
        private System.IntPtr ServerPointer = System.IntPtr.Zero;
        public TerminalServer(string Server)
        {
            this.Server = Server;
        }
        public bool Connect()
        {
            ServerPointer = TerminalServicesAPI.WTSOpenServer(this.Server);
            return IsConnected;
        }
        public bool DisConnect()
        {
            if (IsConnected)
            {
                TerminalServicesAPI.WTSCloseServer(this.ServerPointer);
                this.ServerPointer = System.IntPtr.Zero;
            }
            return true;
        }
    }
}
