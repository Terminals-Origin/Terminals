using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Connections {
    public abstract class Connection : System.Windows.Forms.Control, IConnection {
        #region IConnection Members
        public abstract bool Connected{get;}

        public delegate void LogHandler(string Entry);
        public event LogHandler OnLog;
        
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
