using System;
using System.Threading;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.TerminalServices;

namespace Terminals.Connections
{
    internal abstract class Connection : Control, IConnection
    {
        public delegate void LogHandler(string entry);

        public event LogHandler OnLog;

        public string LastError { get; set; }

        public abstract bool Connected 
        {
            get;
        }

        public IFavorite Favorite { get; set; }

        public TerminalTabControlItem TerminalTabPage { get; set; }

        public MainForm ParentForm { get; set; }

        public TerminalServer Server { get; set; }

        public bool IsTerminalServer { get; set; }

        public abstract bool Connect();

        public abstract void Disconnect();

        public void CheckForTerminalServer(IFavorite favorite)
        {
            if (favorite.Protocol == ConnectionManager.RDP)
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckForTS), favorite);

            this.IsTerminalServer = false;
        }
    
        private void CheckForTS(object state)
        {
            this.IsTerminalServer = false;
            IFavorite favorite = state as IFavorite;
            try
            {
                Thread.Sleep(3000);
                this.Server = TerminalServer.LoadServer(favorite.ServerName);
                this.IsTerminalServer = this.Server.IsATerminalServer;
            }
            catch (Exception exception)
            {
                string message = string.Format("Checked to see if {0} is a terminal server. {0} is not a terminal server",
                                               favorite.ServerName);
                Logging.Error(message, exception);
            }
        }

        protected void Log(string text)
        {
            if (this.OnLog != null)
                this.OnLog(text);
        }
        
        public abstract void ChangeDesktopSize(DesktopSize size);

        /// <summary>
        /// Because this method is called from methods, which mean end of the connection,
        /// the idea behind the refactoring is to avoid to call remove the tab control from connection.
        /// Instead we are going to fire event to the MainForm, which will do for us:
        /// - remove the tab
        /// - Close the connection, when necessary
        /// - and finally the expected Dispose of Disposable resources 
        /// After that the connection wouldnt need reference to the TabControl and MainForm.
        /// </summary>
        protected void FireConnectionClosed()
        {
            this.ParentForm.InvokeCloseTab(this.TerminalTabPage);
        }
    }
}
