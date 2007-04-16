using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FalafelSoftware;
using FalafelSoftware.TransPort;

namespace Terminals.Connections {
    public partial class RASProperties : System.Windows.Forms.UserControl, Connections.IConnection {

        RASConnection rASConnection;
        public RASConnection RASConnection {
            get {
                return rASConnection;
            }
            set {
                rASConnection = value;
                rASConnection.OnLog += new Connection.LogHandler(rASConnection_OnLog);
            }
        }
        public void ChangeDesktopSize(Terminals.DesktopSize Size) {
        }

        System.Windows.Forms.Timer t;
        System.Windows.Forms.MethodInvoker logMiv;
        public RASProperties() {
            InitializeComponent();
            t = new Timer();
            t.Interval = 500;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
            logMiv = new MethodInvoker(UpdateLog);
        }
        void UpdateLog() {
            LogListBox.TopIndex = LogListBox.Items.Add(Entry);
        }
        void t_Tick(object sender, EventArgs e) {
            UpdateStats();
        }

        #region IConnection Members
        
        public TerminalTabControlItem TerminalTabPage {
            get {
                return RASConnection.TerminalTabPage;
            }
            set {
                RASConnection.TerminalTabPage = value;
            }
        }

        public FavoriteConfigurationElement Favorite {
            get {
                return RASConnection.Favorite;
            }
            set {
                RASConnection.Favorite = value;
            }
        }

        private MainForm parentForm;

        public MainForm ParentForm
        {
            get { return parentForm; }
            set { parentForm = value; }
        }

        public bool Connect() {
            return RASConnection.Connect();
        }

        public void Disconnect() {
            t.Stop();
            RASConnection.Disconnect();
        }

        public bool Connected {
            get { return RASConnection.Connected; }
        }
        System.DateTime connectedTime = DateTime.MinValue;
        private void UpdateStats() {
            lbDetails1.Items.Clear();
            this.BringToFront();
            if (this.Connected) {
                if (connectedTime == DateTime.MinValue) connectedTime = DateTime.Now;
                RASENTRY entry = new RASENTRY();
                rASConnection.ras.GetEntry(rASConnection.ras.EntryName, ref entry);
                AddDetailsText("Connection Status", "Connected");
                AddDetailsText("Host", entry.LocalPhoneNumber);
                AddDetailsText("IP Address", rASConnection.ras.IPAddress());
                System.TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - connectedTime.Ticks);
                AddDetailsText("Connection Duration:", string.Format("{0} Days, {1} Hours, {2} Minutes, {3} Seconds", ts.Days, ts.Hours, ts.Minutes, ts.Seconds));

            } else {
                AddDetailsText("Connection Status", "Not Connected");
            }
        }
        private void AddDetailsText(string caption, object item) {
            if (item != null)
                lbDetails1.TopIndex = lbDetails1.Items.Add(caption + ":  " + item.ToString());
        }
        string Entry = "";
        void rASConnection_OnLog(string Entry) {
            this.Entry = Entry;
            lock (logMiv) {
                this.Invoke(logMiv);
            }
        }

        #endregion
    }
}
