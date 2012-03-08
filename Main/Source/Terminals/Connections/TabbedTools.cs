using System;
using System.Windows.Forms;

namespace Terminals.Connections
{
    internal partial class TabbedTools : UserControl
    {
        private PacketCapture packetCapture1;
        public delegate void TabChanged(object sender, TabControlEventArgs e);
        public event TabChanged OnTabChanged;

        public TabbedTools()
        {
            InitializeComponent();

            try
            {
                packetCapture1 = new PacketCapture();

                // 
                // PcapTabPage
                // 
                this.PcapTabPage.Controls.Add(packetCapture1);
                this.PcapTabPage.Location = new System.Drawing.Point(4, 22);
                this.PcapTabPage.Name = "PcapTabPage";
                this.PcapTabPage.Padding = new System.Windows.Forms.Padding(3);
                this.PcapTabPage.Size = new System.Drawing.Size(886, 309);
                this.PcapTabPage.TabIndex = 15;
                this.PcapTabPage.Text = "Packets";
                this.PcapTabPage.UseVisualStyleBackColor = true;
                // 
                // packetCapture1
                // 
                this.packetCapture1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.packetCapture1.Location = new System.Drawing.Point(3, 3);
                this.packetCapture1.Name = "packetCapture1";
                this.packetCapture1.Size = new System.Drawing.Size(880, 303);
                this.packetCapture1.TabIndex = 0;
            }
            catch (Exception e)
            {
                this.PcapTabPage.Controls.Clear();
                Label l = new Label();
                this.PcapTabPage.Controls.Add(l);
                l.Text = "Packet Capture is either not install or not supported on this version of windows.";
                l.Dock = DockStyle.Top;
                Terminals.Logging.Log.Info(l.Text, e);
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (this.OnTabChanged != null) 
                this.OnTabChanged(sender, e);
        }

        public void HideTab(Int32 index)
        {
            if (this.tabControl1.TabCount > index) 
                this.tabControl1.TabPages[index].Hide();
        }

        public void Execute(String action, String host)
        {
            switch (action)
            {
                case "Ping":
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[0];
                    ping1.ForcePing(host);

                    break;
                case "DNS":
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[6];
                    this.dnsLookup1.ForceDNS(host);
                    break;

                case "Trace":
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[1];
                    traceRoute1.ForceTrace(host);
                    break;

                case "TSAdmin":
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[10];
                    this.terminalServerManager1.ForceTSAdmin(host);
                    break;

                default:
                    break;
            }
        }
    }
}
