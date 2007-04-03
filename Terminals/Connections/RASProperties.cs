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

        private void UpdateStats() {
            lbDetails1.Items.Clear();
            this.BringToFront();
            if (this.Connected) {
                ConnectionStats stats = new ConnectionStats();
                ras1.ConnectionStatistics(ref stats);
                AddDetailsText("Connection Status", "Connected");
                AddDetailsText("Alignment Errors", stats.AlignmentErrors);
                AddDetailsText("Bps", stats.Bps);
                AddDetailsText("Bytes Received", stats.BytesReceived);
                AddDetailsText("Bytes Transmitted", stats.BytesTransmitted);
                AddDetailsText("Compression Ratio In", stats.CompressionRatioIn);
                AddDetailsText("Compression Ratio Out", stats.CompressionRatioOut);
                AddDetailsText("Connection Duration", stats.ConnectionDuration);
                AddDetailsText("Crc Errors", stats.CrcErrors);
                AddDetailsText("Frames Received", stats.FramesReceived);
                AddDetailsText("Frames Transmitted", stats.FramesTransmitted);
                AddDetailsText("Framing Errors", stats.FramingErrors);
                AddDetailsText("Timeout Errors", stats.TimeoutErrors);
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
