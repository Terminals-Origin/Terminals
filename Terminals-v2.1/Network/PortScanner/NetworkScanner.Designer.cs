namespace Terminals {
    partial class NetworkScanner {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetworkScanner));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.AllCheckbox = new System.Windows.Forms.CheckBox();
            this.SSHCheckbox = new System.Windows.Forms.CheckBox();
            this.TelnetCheckbox = new System.Windows.Forms.CheckBox();
            this.VMRCCheckbox = new System.Windows.Forms.CheckBox();
            this.VNCCheckbox = new System.Windows.Forms.CheckBox();
            this.RDPCheckbox = new System.Windows.Forms.CheckBox();
            this.groupAddressRange = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ETextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ATextbox = new System.Windows.Forms.TextBox();
            this.BTextbox = new System.Windows.Forms.TextBox();
            this.CTextbox = new System.Windows.Forms.TextBox();
            this.DTextbox = new System.Windows.Forms.TextBox();
            this.ScanButton = new System.Windows.Forms.Button();
            this.TagsTextbox = new System.Windows.Forms.TextBox();
            this.AddAllButton = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ScanStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.scanProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridScanResults = new Terminals.SortableUnboundGrid();
            this.Computer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iPAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serviceNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsScanResults = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ServerAddressTextbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ServerAddressLabel = new System.Windows.Forms.Label();
            this.ServerStatusLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupAddressRange.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridScanResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsScanResults)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupAddressRange);
            this.panel1.Controls.Add(this.ScanButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 87);
            this.panel1.TabIndex = 12;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.AllCheckbox);
            this.groupBox3.Controls.Add(this.SSHCheckbox);
            this.groupBox3.Controls.Add(this.TelnetCheckbox);
            this.groupBox3.Controls.Add(this.VMRCCheckbox);
            this.groupBox3.Controls.Add(this.VNCCheckbox);
            this.groupBox3.Controls.Add(this.RDPCheckbox);
            this.groupBox3.Location = new System.Drawing.Point(204, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(183, 72);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Services to scan:";
            // 
            // AllCheckbox
            // 
            this.AllCheckbox.AutoSize = true;
            this.AllCheckbox.Checked = true;
            this.AllCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllCheckbox.Location = new System.Drawing.Point(129, 46);
            this.AllCheckbox.Name = "AllCheckbox";
            this.AllCheckbox.Size = new System.Drawing.Size(37, 17);
            this.AllCheckbox.TabIndex = 22;
            this.AllCheckbox.Text = "All";
            this.AllCheckbox.UseVisualStyleBackColor = true;
            this.AllCheckbox.CheckedChanged += new System.EventHandler(this.AllCheckbox_CheckedChanged);
            // 
            // SSHCheckbox
            // 
            this.SSHCheckbox.AutoSize = true;
            this.SSHCheckbox.Checked = true;
            this.SSHCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SSHCheckbox.Location = new System.Drawing.Point(129, 25);
            this.SSHCheckbox.Name = "SSHCheckbox";
            this.SSHCheckbox.Size = new System.Drawing.Size(48, 17);
            this.SSHCheckbox.TabIndex = 21;
            this.SSHCheckbox.Text = "SSH";
            this.SSHCheckbox.UseVisualStyleBackColor = true;
            this.SSHCheckbox.CheckedChanged += new System.EventHandler(this.PortCheckbox_CheckedChanged);
            // 
            // TelnetCheckbox
            // 
            this.TelnetCheckbox.AutoSize = true;
            this.TelnetCheckbox.Checked = true;
            this.TelnetCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TelnetCheckbox.Location = new System.Drawing.Point(66, 46);
            this.TelnetCheckbox.Name = "TelnetCheckbox";
            this.TelnetCheckbox.Size = new System.Drawing.Size(56, 17);
            this.TelnetCheckbox.TabIndex = 20;
            this.TelnetCheckbox.Text = "Telnet";
            this.TelnetCheckbox.UseVisualStyleBackColor = true;
            this.TelnetCheckbox.CheckedChanged += new System.EventHandler(this.PortCheckbox_CheckedChanged);
            // 
            // VMRCCheckbox
            // 
            this.VMRCCheckbox.AutoSize = true;
            this.VMRCCheckbox.Checked = true;
            this.VMRCCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VMRCCheckbox.Location = new System.Drawing.Point(66, 23);
            this.VMRCCheckbox.Name = "VMRCCheckbox";
            this.VMRCCheckbox.Size = new System.Drawing.Size(57, 17);
            this.VMRCCheckbox.TabIndex = 19;
            this.VMRCCheckbox.Text = "VMRC";
            this.VMRCCheckbox.UseVisualStyleBackColor = true;
            this.VMRCCheckbox.CheckedChanged += new System.EventHandler(this.PortCheckbox_CheckedChanged);
            // 
            // VNCCheckbox
            // 
            this.VNCCheckbox.AutoSize = true;
            this.VNCCheckbox.Checked = true;
            this.VNCCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VNCCheckbox.Location = new System.Drawing.Point(11, 46);
            this.VNCCheckbox.Name = "VNCCheckbox";
            this.VNCCheckbox.Size = new System.Drawing.Size(48, 17);
            this.VNCCheckbox.TabIndex = 18;
            this.VNCCheckbox.Text = "VNC";
            this.VNCCheckbox.UseVisualStyleBackColor = true;
            this.VNCCheckbox.CheckedChanged += new System.EventHandler(this.PortCheckbox_CheckedChanged);
            // 
            // RDPCheckbox
            // 
            this.RDPCheckbox.AutoSize = true;
            this.RDPCheckbox.Checked = true;
            this.RDPCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RDPCheckbox.Location = new System.Drawing.Point(11, 23);
            this.RDPCheckbox.Name = "RDPCheckbox";
            this.RDPCheckbox.Size = new System.Drawing.Size(49, 17);
            this.RDPCheckbox.TabIndex = 17;
            this.RDPCheckbox.Text = "RDP";
            this.RDPCheckbox.UseVisualStyleBackColor = true;
            this.RDPCheckbox.CheckedChanged += new System.EventHandler(this.PortCheckbox_CheckedChanged);
            // 
            // groupAddressRange
            // 
            this.groupAddressRange.Controls.Add(this.label7);
            this.groupAddressRange.Controls.Add(this.ETextbox);
            this.groupAddressRange.Controls.Add(this.label1);
            this.groupAddressRange.Controls.Add(this.ATextbox);
            this.groupAddressRange.Controls.Add(this.BTextbox);
            this.groupAddressRange.Controls.Add(this.CTextbox);
            this.groupAddressRange.Controls.Add(this.DTextbox);
            this.groupAddressRange.Location = new System.Drawing.Point(6, 6);
            this.groupAddressRange.Name = "groupAddressRange";
            this.groupAddressRange.Size = new System.Drawing.Size(192, 72);
            this.groupAddressRange.TabIndex = 17;
            this.groupAddressRange.TabStop = false;
            this.groupAddressRange.Text = "IP address range:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "To:";
            // 
            // ETextbox
            // 
            this.ETextbox.Location = new System.Drawing.Point(153, 45);
            this.ETextbox.Name = "ETextbox";
            this.ETextbox.Size = new System.Drawing.Size(30, 20);
            this.ETextbox.TabIndex = 10;
            this.ETextbox.Text = "99";
            this.ETextbox.TextChanged += new System.EventHandler(this.IPTextbox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "From:";
            // 
            // ATextbox
            // 
            this.ATextbox.Location = new System.Drawing.Point(45, 20);
            this.ATextbox.Name = "ATextbox";
            this.ATextbox.Size = new System.Drawing.Size(30, 20);
            this.ATextbox.TabIndex = 6;
            this.ATextbox.Text = "10";
            this.ATextbox.TextChanged += new System.EventHandler(this.IPTextbox_TextChanged);
            // 
            // BTextbox
            // 
            this.BTextbox.Location = new System.Drawing.Point(81, 20);
            this.BTextbox.Name = "BTextbox";
            this.BTextbox.Size = new System.Drawing.Size(30, 20);
            this.BTextbox.TabIndex = 7;
            this.BTextbox.Text = "0";
            this.BTextbox.TextChanged += new System.EventHandler(this.IPTextbox_TextChanged);
            // 
            // CTextbox
            // 
            this.CTextbox.Location = new System.Drawing.Point(117, 20);
            this.CTextbox.Name = "CTextbox";
            this.CTextbox.Size = new System.Drawing.Size(30, 20);
            this.CTextbox.TabIndex = 8;
            this.CTextbox.Text = "0";
            this.CTextbox.TextChanged += new System.EventHandler(this.IPTextbox_TextChanged);
            // 
            // DTextbox
            // 
            this.DTextbox.Location = new System.Drawing.Point(153, 20);
            this.DTextbox.Name = "DTextbox";
            this.DTextbox.Size = new System.Drawing.Size(30, 20);
            this.DTextbox.TabIndex = 9;
            this.DTextbox.Text = "99";
            this.DTextbox.TextChanged += new System.EventHandler(this.IPTextbox_TextChanged);
            // 
            // ScanButton
            // 
            this.ScanButton.Location = new System.Drawing.Point(393, 31);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(75, 23);
            this.ScanButton.TabIndex = 11;
            this.ScanButton.Text = "&Scan";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // TagsTextbox
            // 
            this.TagsTextbox.Location = new System.Drawing.Point(92, 8);
            this.TagsTextbox.Name = "TagsTextbox";
            this.TagsTextbox.Size = new System.Drawing.Size(138, 20);
            this.TagsTextbox.TabIndex = 18;
            this.TagsTextbox.Text = "Tags...";
            // 
            // AddAllButton
            // 
            this.AddAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddAllButton.Location = new System.Drawing.Point(283, 6);
            this.AddAllButton.Name = "AddAllButton";
            this.AddAllButton.Size = new System.Drawing.Size(121, 23);
            this.AddAllButton.TabIndex = 17;
            this.AddAllButton.Text = "&Import selected";
            this.AddAllButton.UseVisualStyleBackColor = true;
            this.AddAllButton.Click += new System.EventHandler(this.AddAllButton_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(410, 6);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 15;
            this.ButtonCancel.Text = "&Close";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(150, 53);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Connect to Server";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Start Server";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScanStatusLabel,
            this.scanProgressBar});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 374);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(489, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ScanStatusLabel
            // 
            this.ScanStatusLabel.Name = "ScanStatusLabel";
            this.ScanStatusLabel.Size = new System.Drawing.Size(171, 17);
            this.ScanStatusLabel.Text = "Modify the IP Range, and hit Scan";
            // 
            // scanProgressBar
            // 
            this.scanProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.scanProgressBar.Name = "scanProgressBar";
            this.scanProgressBar.Size = new System.Drawing.Size(150, 16);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(489, 340);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridScanResults);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(481, 314);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Port Scanner";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gridScanResults
            // 
            this.gridScanResults.AllowUserToAddRows = false;
            this.gridScanResults.AllowUserToOrderColumns = true;
            this.gridScanResults.AllowUserToResizeRows = false;
            this.gridScanResults.AutoGenerateColumns = false;
            this.gridScanResults.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridScanResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridScanResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridScanResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Computer,
            this.HostName,
            this.ServiceName,
            this.iPAddressDataGridViewTextBoxColumn,
            this.hostNameDataGridViewTextBoxColumn,
            this.serviceNameDataGridViewTextBoxColumn});
            this.gridScanResults.DataSource = this.bsScanResults;
            this.gridScanResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridScanResults.Location = new System.Drawing.Point(3, 90);
            this.gridScanResults.Name = "gridScanResults";
            this.gridScanResults.RowHeadersVisible = false;
            this.gridScanResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridScanResults.Size = new System.Drawing.Size(475, 221);
            this.gridScanResults.TabIndex = 13;
            this.gridScanResults.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridScanResults_ColumnHeaderMouseClick);
            // 
            // Computer
            // 
            this.Computer.DataPropertyName = "IPAddress";
            this.Computer.HeaderText = "IP address";
            this.Computer.Name = "Computer";
            this.Computer.ReadOnly = true;
            this.Computer.Width = 150;
            // 
            // HostName
            // 
            this.HostName.DataPropertyName = "HostName";
            this.HostName.HeaderText = "Host name";
            this.HostName.Name = "HostName";
            this.HostName.ReadOnly = true;
            this.HostName.Width = 200;
            // 
            // ServiceName
            // 
            this.ServiceName.DataPropertyName = "ServiceName";
            this.ServiceName.HeaderText = "Service";
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.ReadOnly = true;
            this.ServiceName.Width = 50;
            // 
            // iPAddressDataGridViewTextBoxColumn
            // 
            this.iPAddressDataGridViewTextBoxColumn.DataPropertyName = "IPAddress";
            this.iPAddressDataGridViewTextBoxColumn.HeaderText = "IPAddress";
            this.iPAddressDataGridViewTextBoxColumn.Name = "iPAddressDataGridViewTextBoxColumn";
            // 
            // hostNameDataGridViewTextBoxColumn
            // 
            this.hostNameDataGridViewTextBoxColumn.DataPropertyName = "HostName";
            this.hostNameDataGridViewTextBoxColumn.HeaderText = "HostName";
            this.hostNameDataGridViewTextBoxColumn.Name = "hostNameDataGridViewTextBoxColumn";
            // 
            // serviceNameDataGridViewTextBoxColumn
            // 
            this.serviceNameDataGridViewTextBoxColumn.DataPropertyName = "ServiceName";
            this.serviceNameDataGridViewTextBoxColumn.HeaderText = "ServiceName";
            this.serviceNameDataGridViewTextBoxColumn.Name = "serviceNameDataGridViewTextBoxColumn";
            this.serviceNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bsScanResults
            // 
            this.bsScanResults.DataSource = typeof(Terminals.Scanner.NetworkScanResult);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(481, 314);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Direct Connection";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.ServerAddressTextbox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(170, 143);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 158);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Client";
            // 
            // ServerAddressTextbox
            // 
            this.ServerAddressTextbox.Location = new System.Drawing.Point(73, 27);
            this.ServerAddressTextbox.Name = "ServerAddressTextbox";
            this.ServerAddressTextbox.Size = new System.Drawing.Size(180, 20);
            this.ServerAddressTextbox.TabIndex = 21;
            this.ServerAddressTextbox.Text = "127.0.0.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "IP Address:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ServerAddressLabel);
            this.groupBox1.Controls.Add(this.ServerStatusLabel);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(11, 143);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 158);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // ServerAddressLabel
            // 
            this.ServerAddressLabel.AutoSize = true;
            this.ServerAddressLabel.Location = new System.Drawing.Point(43, 100);
            this.ServerAddressLabel.Name = "ServerAddressLabel";
            this.ServerAddressLabel.Size = new System.Drawing.Size(52, 13);
            this.ServerAddressLabel.TabIndex = 21;
            this.ServerAddressLabel.Text = "127.0.0.1";
            // 
            // ServerStatusLabel
            // 
            this.ServerStatusLabel.AutoSize = true;
            this.ServerStatusLabel.Location = new System.Drawing.Point(20, 70);
            this.ServerStatusLabel.Name = "ServerStatusLabel";
            this.ServerStatusLabel.Size = new System.Drawing.Size(98, 13);
            this.ServerStatusLabel.TabIndex = 20;
            this.ServerStatusLabel.Text = "Server is OFFLINE.";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(415, 45);
            this.label4.TabIndex = 24;
            this.label4.Text = "Step 2: On the machine which you would like to share your connections with, place" +
    " your IP address in the \"IP Address\" box on the right, and then it the \"Connect " +
    "to Server\" button.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(336, 23);
            this.label3.TabIndex = 23;
            this.label3.Text = "Step 1: Start the \"Server\" by hitting the \"Start Server\" button below.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(421, 30);
            this.label2.TabIndex = 22;
            this.label2.Text = "You can use this form to establish a direct connection with another machine on th" +
    "e network in order to share connections.";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.AddAllButton);
            this.panel2.Controls.Add(this.ButtonCancel);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.TagsTextbox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 340);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(489, 34);
            this.panel2.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Optional Tags:";
            // 
            // NetworkScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 396);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(497, 423);
            this.Name = "NetworkScanner";
            this.ShowInTaskbar = false;
            this.Text = "Terminals - Network Scanner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetworkScanner_FormClosing);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupAddressRange.ResumeLayout(false);
            this.groupAddressRange.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridScanResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsScanResults)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ScanButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ScanStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar scanProgressBar;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button AddAllButton;
        private System.Windows.Forms.TextBox TagsTextbox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ServerAddressTextbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label ServerAddressLabel;
        private System.Windows.Forms.Label ServerStatusLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private Terminals.SortableUnboundGrid gridScanResults;
        private System.Windows.Forms.BindingSource bsScanResults;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox AllCheckbox;
        private System.Windows.Forms.CheckBox SSHCheckbox;
        private System.Windows.Forms.CheckBox TelnetCheckbox;
        private System.Windows.Forms.CheckBox VMRCCheckbox;
        private System.Windows.Forms.CheckBox VNCCheckbox;
        private System.Windows.Forms.CheckBox RDPCheckbox;
        private System.Windows.Forms.GroupBox groupAddressRange;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ETextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ATextbox;
        private System.Windows.Forms.TextBox BTextbox;
        private System.Windows.Forms.TextBox CTextbox;
        private System.Windows.Forms.TextBox DTextbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Computer;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn iPAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serviceNameDataGridViewTextBoxColumn;
    }
}