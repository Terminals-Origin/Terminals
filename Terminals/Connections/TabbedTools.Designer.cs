namespace Terminals.Connections
{
    partial class TabbedTools
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ping1 = new Metro.Ping();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.traceRoute1 = new Metro.TraceRoute();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.wmiControl1 = new WMITestClient.WMIControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.localConnections1 = new Terminals.Network.LocalConnections();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.interfacesList1 = new Terminals.Network.InterfacesList();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.whoIs1 = new Terminals.Network.WhoIs.WhoIs();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.dnsLookup1 = new Terminals.Network.DNSLookup();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.networkShares1 = new Terminals.Network.NetworkShares();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.services1 = new Terminals.Network.WMI.Services();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.networkTime1 = new Terminals.Network.NTP.NetworkTime();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.terminalServerManager1 = new Terminals.Network.Servers.TerminalServerManager();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.wakeOnLan1 = new Terminals.Network.WakeOnLan();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.portScanner1 = new Terminals.Network.PortScanner();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.tabPage13.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage11);
            this.tabControl1.Controls.Add(this.tabPage10);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Controls.Add(this.tabPage12);
            this.tabControl1.Controls.Add(this.tabPage13);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(786, 335);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ping1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(748, 309);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Ping";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ping1
            // 
            this.ping1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ping1.Location = new System.Drawing.Point(3, 3);
            this.ping1.Name = "ping1";
            this.ping1.Size = new System.Drawing.Size(742, 303);
            this.ping1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.traceRoute1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(748, 309);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Trace Route";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // traceRoute1
            // 
            this.traceRoute1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.traceRoute1.Location = new System.Drawing.Point(3, 3);
            this.traceRoute1.Name = "traceRoute1";
            this.traceRoute1.Size = new System.Drawing.Size(742, 303);
            this.traceRoute1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.wmiControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(748, 309);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "WMI";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // wmiControl1
            // 
            this.wmiControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wmiControl1.Location = new System.Drawing.Point(0, 0);
            this.wmiControl1.Name = "wmiControl1";
            this.wmiControl1.Size = new System.Drawing.Size(748, 309);
            this.wmiControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.localConnections1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(748, 309);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Connections";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // localConnections1
            // 
            this.localConnections1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localConnections1.Location = new System.Drawing.Point(3, 3);
            this.localConnections1.Name = "localConnections1";
            this.localConnections1.Size = new System.Drawing.Size(742, 303);
            this.localConnections1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.interfacesList1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(748, 309);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Interfaces";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // interfacesList1
            // 
            this.interfacesList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interfacesList1.Location = new System.Drawing.Point(3, 3);
            this.interfacesList1.Name = "interfacesList1";
            this.interfacesList1.Size = new System.Drawing.Size(742, 303);
            this.interfacesList1.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.whoIs1);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(748, 309);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Whois";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // whoIs1
            // 
            this.whoIs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whoIs1.Location = new System.Drawing.Point(3, 3);
            this.whoIs1.Name = "whoIs1";
            this.whoIs1.Size = new System.Drawing.Size(742, 303);
            this.whoIs1.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.dnsLookup1);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(748, 309);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "DNS Lookup";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // dnsLookup1
            // 
            this.dnsLookup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dnsLookup1.Location = new System.Drawing.Point(3, 3);
            this.dnsLookup1.Name = "dnsLookup1";
            this.dnsLookup1.Size = new System.Drawing.Size(742, 303);
            this.dnsLookup1.TabIndex = 0;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.networkShares1);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(748, 309);
            this.tabPage9.TabIndex = 8;
            this.tabPage9.Text = "Shares";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // networkShares1
            // 
            this.networkShares1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.networkShares1.Location = new System.Drawing.Point(3, 3);
            this.networkShares1.Name = "networkShares1";
            this.networkShares1.Size = new System.Drawing.Size(742, 303);
            this.networkShares1.TabIndex = 0;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.services1);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(748, 309);
            this.tabPage11.TabIndex = 12;
            this.tabPage11.Text = "Services";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // services1
            // 
            this.services1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.services1.Location = new System.Drawing.Point(0, 0);
            this.services1.Name = "services1";
            this.services1.Size = new System.Drawing.Size(748, 309);
            this.services1.TabIndex = 0;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.networkTime1);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(748, 309);
            this.tabPage10.TabIndex = 9;
            this.tabPage10.Text = "Time";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // networkTime1
            // 
            this.networkTime1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.networkTime1.Location = new System.Drawing.Point(3, 3);
            this.networkTime1.Name = "networkTime1";
            this.networkTime1.Size = new System.Drawing.Size(742, 303);
            this.networkTime1.TabIndex = 0;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.terminalServerManager1);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(748, 309);
            this.tabPage8.TabIndex = 11;
            this.tabPage8.Text = "TS Admin";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // terminalServerManager1
            // 
            this.terminalServerManager1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.terminalServerManager1.Location = new System.Drawing.Point(0, 0);
            this.terminalServerManager1.Name = "terminalServerManager1";
            this.terminalServerManager1.Size = new System.Drawing.Size(748, 309);
            this.terminalServerManager1.TabIndex = 0;
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.wakeOnLan1);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(748, 309);
            this.tabPage12.TabIndex = 13;
            this.tabPage12.Text = "Wake on Lan";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // wakeOnLan1
            // 
            this.wakeOnLan1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wakeOnLan1.Location = new System.Drawing.Point(3, 3);
            this.wakeOnLan1.Name = "wakeOnLan1";
            this.wakeOnLan1.Size = new System.Drawing.Size(742, 303);
            this.wakeOnLan1.TabIndex = 0;
            // 
            // tabPage13
            // 
            this.tabPage13.Controls.Add(this.portScanner1);
            this.tabPage13.Location = new System.Drawing.Point(4, 22);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage13.Size = new System.Drawing.Size(778, 309);
            this.tabPage13.TabIndex = 14;
            this.tabPage13.Text = "Port Scanner";
            this.tabPage13.UseVisualStyleBackColor = true;
            // 
            // portScanner1
            // 
            this.portScanner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.portScanner1.Location = new System.Drawing.Point(3, 3);
            this.portScanner1.Name = "portScanner1";
            this.portScanner1.Size = new System.Drawing.Size(772, 303);
            this.portScanner1.TabIndex = 0;
            // 
            // TabbedTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "TabbedTools";
            this.Size = new System.Drawing.Size(786, 335);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.tabPage13.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private Metro.TraceRoute traceRoute1;
        private System.Windows.Forms.TabPage tabPage3;
        private WMITestClient.WMIControl wmiControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private Terminals.Network.LocalConnections localConnections1;
        private System.Windows.Forms.TabPage tabPage5;
        private Terminals.Network.InterfacesList interfacesList1;
        private System.Windows.Forms.TabPage tabPage6;
        private Terminals.Network.WhoIs.WhoIs whoIs1;
        private System.Windows.Forms.TabPage tabPage7;
        private Terminals.Network.DNSLookup dnsLookup1;
        private System.Windows.Forms.TabPage tabPage9;
        private Terminals.Network.NetworkShares networkShares1;
        private System.Windows.Forms.TabPage tabPage10;
        private Terminals.Network.NTP.NetworkTime networkTime1;
        private System.Windows.Forms.TabPage tabPage8;
        private Terminals.Network.Servers.TerminalServerManager terminalServerManager1;
        private System.Windows.Forms.TabPage tabPage1;
        private Metro.Ping ping1;
        private System.Windows.Forms.TabPage tabPage11;
        private Terminals.Network.WMI.Services services1;
        private System.Windows.Forms.TabPage tabPage12;
        private Terminals.Network.WakeOnLan wakeOnLan1;
        private System.Windows.Forms.TabPage tabPage13;
        private Terminals.Network.PortScanner portScanner1;
    }
}
