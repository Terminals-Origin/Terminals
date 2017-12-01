using System;
using System.Windows.Forms;
using FalafelSoftware.TransPort;
using Terminals.Data;

namespace Terminals.Connections
{
    internal partial class RASProperties : UserControl
    {
        public string LastError { get; set; }
        private RASConnection rASConnection;
        private Timer timer;
        private MethodInvoker logMiv;
        DateTime connectedTime = DateTime.MinValue;
        string Entry = string.Empty;

        public RASConnection RASConnection
        {
            get
            {
                return this.rASConnection;
            }

            set
            {
                this.rASConnection = value;
                this.rASConnection.OnLog += new Connection.LogHandler(this.rASConnection_OnLog);
            }
        }

        public TerminalServices.TerminalServer Server
        {
            get
            {
                return null;
            }
        }

        public bool IsTerminalServer
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        public RASProperties()
        {
            InitializeComponent();
            this.timer = new Timer();
            this.timer.Interval = 500;
            this.timer.Tick += new EventHandler(this.timer_Tick);
            this.timer.Start();
            this.logMiv = new MethodInvoker(this.UpdateLog);
        }

        private void UpdateLog()
        {
            this.LogListBox.TopIndex = this.LogListBox.Items.Add(Entry);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.UpdateStats();
        }

        #region IConnection Members

        public IFavorite Favorite
        {
            get
            {
                return RASConnection.Favorite;
            }

            set
            {
                RASConnection.Favorite = value;
            }
        }

        public bool Connect()
        {
            return RASConnection.Connect();
        }

        public void Disconnect()
        {
            this.timer.Stop();
            RASConnection.Disconnect();
        }

        public bool Connected
        {
            get
            {
                return RASConnection.Connected;
            }
        }

        public void ChangeDesktopSize(DesktopSize size)
        {
        }

        private void UpdateStats()
        {
            this.lbDetails1.Items.Clear();
            this.BringToFront();
            if (this.Connected)
            {
                if (this.connectedTime == DateTime.MinValue)
                    this.connectedTime = DateTime.Now;

                RASENTRY entry = new RASENTRY();
                this.rASConnection.ras.GetEntry(rASConnection.ras.EntryName, ref entry);
                this.AddDetailsText("Connection Status", "Connected");
                this.AddDetailsText("Host", entry.LocalPhoneNumber);
                this.AddDetailsText("IP Address", this.rASConnection.ras.IPAddress());
                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - this.connectedTime.Ticks);
                this.AddDetailsText("Connection Duration:", string.Format("{0} Days, {1} Hours, {2} Minutes, {3} Seconds", ts.Days, ts.Hours, ts.Minutes, ts.Seconds));
            }
            else
            {
                this.AddDetailsText("Connection Status", "Not Connected");
            }
        }

        private void AddDetailsText(string caption, object item)
        {
            if (item != null)
                this.lbDetails1.TopIndex = this.lbDetails1.Items.Add(caption + ":  " + item.ToString());
        }

        private void rASConnection_OnLog(string Entry)
        {
            this.Entry = Entry;
            lock (this.logMiv)
            {
                this.Invoke(this.logMiv);
            }
        }

        #endregion
    }
}
