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
        public TabbedTools()
        {
            InitializeComponent();
        }
        public delegate void TabChanged(object sender, TabControlEventArgs e);
        public event TabChanged OnTabChanged;

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (OnTabChanged != null) OnTabChanged(sender, e);
        }

        private void terminalServerManager1_Load(object sender, EventArgs e)
        {

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
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[9];
                    this.terminalServerManager1.ForceTSAdmin(Host);
                    break;
                default:
                    break;
            }
        }
    }
}
