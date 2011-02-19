using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Connections
{
    public partial class TabbedTools : UserControl
    {
        private WindowsFormsApplication2.PacketCapture packetCapture1;
        public TabbedTools()
        {
            InitializeComponent();

            try
            {
                packetCapture1 = new WindowsFormsApplication2.PacketCapture();

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
        public delegate void TabChanged(object sender, TabControlEventArgs e);
        public event TabChanged OnTabChanged;

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (OnTabChanged != null) OnTabChanged(sender, e);
        }
        public void HideTab(int Index)
        {
            if (this.tabControl1.TabCount > Index) this.tabControl1.TabPages[Index].Hide();
        }
        public void Execute(string Action, string Host)
        {
            switch(Action)
            {
                case "Ping":
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[0];
                    ping1.ForcePing(Host);
                    break;
                case "DNS":
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[6];
                    this.dnsLookup1.ForceDNS(Host);
                    break;
                case "Trace":
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[1];
                    traceRoute1.ForceTrace(Host);
                    break;
                case "TSAdmin":
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[10];
                    this.terminalServerManager1.ForceTSAdmin(Host);
                    break;
                default:
                    break;
            }
        }
    }
}
