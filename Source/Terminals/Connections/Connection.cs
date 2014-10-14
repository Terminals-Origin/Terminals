using System;
using System.Threading;
using System.Windows.Forms;
using TabControl;
using System.Runtime.InteropServices;
using Terminals.Data;
using Terminals.Native;
using Terminals.TerminalServices;

namespace Terminals.Connections
{
    internal abstract class Connection : Control, IConnection
    {
        #region IConnection Members
        public string LastError { get; set; }
        private TerminalServer server;
        private bool isTerminalServer = false;
        private IFavorite favorite;
        private TerminalTabControlItem terminalTabPage;
        private MainForm parentForm;
        
        internal delegate void InvokeCloseTabPage(TabControlItem tabPage);
        
        public delegate void TerminalServerStateDiscovery(IFavorite favorite, bool isTerminalServer, TerminalServer server);
        
        public event TerminalServerStateDiscovery OnTerminalServerStateDiscovery;

        public delegate void LogHandler(string entry);

        public event LogHandler OnLog;

        public abstract bool Connect();

        public abstract void Disconnect();

        public abstract bool Connected 
        {
            get;
        }

        public TerminalServer Server
        {
            get
            {
                return this.server;
            }
            
            set
            {
                this.server = value;
            }
        }

        public bool IsTerminalServer
        {
            get
            {
                return this.isTerminalServer;
            }

            set
            {
                this.isTerminalServer = value;
            }
        }
    
        private void CheckForTS(object state)
        {
            this.isTerminalServer = false;
            IFavorite favorite = state as IFavorite;
            try
            {
                Thread.Sleep(3000);
                this.server = TerminalServer.LoadServer(favorite.ServerName);
                this.isTerminalServer = this.server.IsATerminalServer;
            }
            catch (Exception exception)
            {
                string message = string.Format("Checked to see if {0} is a terminal server. {0} is not a terminal server",
                                               favorite.ServerName);
                Logging.Error(message, exception);
            }

            if (this.OnTerminalServerStateDiscovery != null)
                this.OnTerminalServerStateDiscovery(favorite, this.isTerminalServer, this.server);
        }

        public void CheckForTerminalServer(IFavorite favorite)
        {
            if (favorite.Protocol == ConnectionManager.RDP)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.CheckForTS), favorite);
            }

            this.isTerminalServer = false;
        }

        protected void Log(string text)
        {
            if (this.OnLog != null)
                this.OnLog(text);
        }
        
        public abstract void ChangeDesktopSize(DesktopSize size);

        public IFavorite Favorite 
        {
            get
            {
                return this.favorite;
            }
            
            set
            {
                this.favorite = value;
            }
        }
        
        public TerminalTabControlItem TerminalTabPage
        {
            get
            {
                return this.terminalTabPage;
            }
            
            set
            {
                this.terminalTabPage = value;
            }
        }

        public MainForm ParentForm
        {
            get
            {
                return this.parentForm;
            }

            set
            {
                this.parentForm = value;
            }
        }

        internal void CloseTabPage(object tabObject)
        {
            var tabPage = tabObject as TabControlItem;
            if (tabPage == null)
                return;
            
            bool wasSelected = tabPage.Selected;
            this.ParentForm.RemoveTabPage(tabPage);
            if (wasSelected)
                this.ParentForm.OnLeavingFullScreen();

            this.ParentForm.UpdateControls();
        }

        #endregion
    }
}
