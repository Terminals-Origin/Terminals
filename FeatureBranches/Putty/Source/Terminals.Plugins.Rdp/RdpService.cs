using System;
using System.Threading;
using Terminals.Data;
using Terminals.TerminalServices;

namespace Terminals.Connections
{
    internal class RdpService
    {
        public TerminalServer Server { get; private set; }

        public bool IsTerminalServer { get; private set; }

        public void CheckForTerminalServer(IFavorite favorite)
        {
            this.IsTerminalServer = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckForTS), favorite);
        }

        private void CheckForTS(object state)
        {
            IFavorite favorite = state as IFavorite;
            try
            {
                Thread.Sleep(3000);
                this.Server = TerminalServer.LoadServer(favorite.ServerName);
                this.IsTerminalServer = this.Server.IsATerminalServer;
            }
            catch (Exception exception)
            {
                const string MESSAGE = "Checked to see if {0} is a terminal server. {0} is not a terminal server";
                string message = String.Format(MESSAGE, favorite.ServerName);
                Logging.Error(message, exception);
            }
        }
    }
}