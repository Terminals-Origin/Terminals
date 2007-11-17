using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Connections {
    public abstract class Connection : System.Windows.Forms.Control, IConnection {
        #region IConnection Members
        public abstract bool Connected{get;}

        public delegate void LogHandler(string Entry);
        public event LogHandler OnLog;

        public delegate void TerminalServerStateDiscovery(FavoriteConfigurationElement Favorite, bool IsTerminalServer, TerminalServices.TerminalServer Server);
        public event TerminalServerStateDiscovery OnTerminalServerStateDiscovery;

        private TerminalServices.TerminalServer server;

        public TerminalServices.TerminalServer Server {
            get { return server; }
            set { server = value; }
        }
        private bool isTerminalServer = false;
        public bool IsTerminalServer {
            get { return isTerminalServer; }
            set { isTerminalServer = value; }
        }
	
        private void CheckForTS(object state) {
            isTerminalServer = false;
            FavoriteConfigurationElement host = (FavoriteConfigurationElement)state;
            try {
                System.Threading.Thread.Sleep(3000);
                server = TerminalServices.TerminalServer.LoadServer(host.ServerName);
                isTerminalServer = server.IsATerminalServer;
            } catch(Exception Exc) {
                Terminals.Logging.Log.Info(string.Format("checked to see if {0} is a terminal server.  {0} is not a terminal server", host.ServerName));
            }
            if(OnTerminalServerStateDiscovery != null) OnTerminalServerStateDiscovery(host, isTerminalServer, server);
        }
        public void CheckForTerminalServer(FavoriteConfigurationElement Fav) {
            if(Fav.Protocol == "RDP") {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(CheckForTS), (object)Fav);
            }
            isTerminalServer = false;
        }


        protected void Log(string Text) {
            if (OnLog != null) OnLog(Text);
        }
        public abstract void ChangeDesktopSize(Terminals.DesktopSize Size);

        FavoriteConfigurationElement favorite;
        public FavoriteConfigurationElement Favorite { get { return favorite; } set { favorite = value; } }
        TerminalTabControlItem terminalTabPage;
        public TerminalTabControlItem TerminalTabPage{get{return terminalTabPage;}set{terminalTabPage=value;}}

        private MainForm parentForm;

        public MainForm ParentForm
        {
            get { return parentForm; }
            set { parentForm = value; }
        }

        public abstract bool Connect();

        public abstract void Disconnect();

        #endregion

        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.Connection_ControlAdded);
            this.ResumeLayout(false);

        }

        private void Connection_ControlAdded(object sender, System.Windows.Forms.ControlEventArgs e) {

        }
    }
}
