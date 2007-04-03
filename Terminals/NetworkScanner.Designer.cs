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
            this.label1 = new System.Windows.Forms.Label();
            this.ATextbox = new System.Windows.Forms.TextBox();
            this.BTextbox = new System.Windows.Forms.TextBox();
            this.CTextbox = new System.Windows.Forms.TextBox();
            this.DTextbox = new System.Windows.Forms.TextBox();
            this.ETextbox = new System.Windows.Forms.TextBox();
            this.RDPCheckbox = new System.Windows.Forms.CheckBox();
            this.VNCCheckbox = new System.Windows.Forms.CheckBox();
            this.VMRCCheckbox = new System.Windows.Forms.CheckBox();
            this.TelnetCheckbox = new System.Windows.Forms.CheckBox();
            this.SSHCheckbox = new System.Windows.Forms.CheckBox();
            this.ScanResultsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AddAllButton = new System.Windows.Forms.Button();
            this.AllCheckbox = new System.Windows.Forms.CheckBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ScanButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ScanStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.scanProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Range:";
            // 
            // ATextbox
            // 
            this.ATextbox.Location = new System.Drawing.Point(64, 16);
            this.ATextbox.Name = "ATextbox";
            this.ATextbox.Size = new System.Drawing.Size(30, 20);
            this.ATextbox.TabIndex = 1;
            this.ATextbox.Text = "10";
            // 
            // BTextbox
            // 
            this.BTextbox.Location = new System.Drawing.Point(100, 16);
            this.BTextbox.Name = "BTextbox";
            this.BTextbox.Size = new System.Drawing.Size(30, 20);
            this.BTextbox.TabIndex = 2;
            this.BTextbox.Text = "0";
            // 
            // CTextbox
            // 
            this.CTextbox.Location = new System.Drawing.Point(136, 16);
            this.CTextbox.Name = "CTextbox";
            this.CTextbox.Size = new System.Drawing.Size(30, 20);
            this.CTextbox.TabIndex = 3;
            this.CTextbox.Text = "0";
            // 
            // DTextbox
            // 
            this.DTextbox.Location = new System.Drawing.Point(172, 4);
            this.DTextbox.Name = "DTextbox";
            this.DTextbox.Size = new System.Drawing.Size(30, 20);
            this.DTextbox.TabIndex = 4;
            this.DTextbox.Text = "99";
            // 
            // ETextbox
            // 
            this.ETextbox.Location = new System.Drawing.Point(172, 30);
            this.ETextbox.Name = "ETextbox";
            this.ETextbox.Size = new System.Drawing.Size(30, 20);
            this.ETextbox.TabIndex = 5;
            this.ETextbox.Text = "99";
            // 
            // RDPCheckbox
            // 
            this.RDPCheckbox.AutoSize = true;
            this.RDPCheckbox.Checked = true;
            this.RDPCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RDPCheckbox.Location = new System.Drawing.Point(221, 4);
            this.RDPCheckbox.Name = "RDPCheckbox";
            this.RDPCheckbox.Size = new System.Drawing.Size(49, 17);
            this.RDPCheckbox.TabIndex = 6;
            this.RDPCheckbox.Text = "RDP";
            this.RDPCheckbox.UseVisualStyleBackColor = true;
            // 
            // VNCCheckbox
            // 
            this.VNCCheckbox.AutoSize = true;
            this.VNCCheckbox.Checked = true;
            this.VNCCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VNCCheckbox.Location = new System.Drawing.Point(221, 27);
            this.VNCCheckbox.Name = "VNCCheckbox";
            this.VNCCheckbox.Size = new System.Drawing.Size(48, 17);
            this.VNCCheckbox.TabIndex = 7;
            this.VNCCheckbox.Text = "VNC";
            this.VNCCheckbox.UseVisualStyleBackColor = true;
            // 
            // VMRCCheckbox
            // 
            this.VMRCCheckbox.AutoSize = true;
            this.VMRCCheckbox.Checked = true;
            this.VMRCCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VMRCCheckbox.Location = new System.Drawing.Point(276, 4);
            this.VMRCCheckbox.Name = "VMRCCheckbox";
            this.VMRCCheckbox.Size = new System.Drawing.Size(57, 17);
            this.VMRCCheckbox.TabIndex = 8;
            this.VMRCCheckbox.Text = "VMRC";
            this.VMRCCheckbox.UseVisualStyleBackColor = true;
            // 
            // TelnetCheckbox
            // 
            this.TelnetCheckbox.AutoSize = true;
            this.TelnetCheckbox.Checked = true;
            this.TelnetCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TelnetCheckbox.Location = new System.Drawing.Point(276, 27);
            this.TelnetCheckbox.Name = "TelnetCheckbox";
            this.TelnetCheckbox.Size = new System.Drawing.Size(56, 17);
            this.TelnetCheckbox.TabIndex = 9;
            this.TelnetCheckbox.Text = "Telnet";
            this.TelnetCheckbox.UseVisualStyleBackColor = true;
            // 
            // SSHCheckbox
            // 
            this.SSHCheckbox.AutoSize = true;
            this.SSHCheckbox.Checked = true;
            this.SSHCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SSHCheckbox.Location = new System.Drawing.Point(339, 6);
            this.SSHCheckbox.Name = "SSHCheckbox";
            this.SSHCheckbox.Size = new System.Drawing.Size(48, 17);
            this.SSHCheckbox.TabIndex = 10;
            this.SSHCheckbox.Text = "SSH";
            this.SSHCheckbox.UseVisualStyleBackColor = true;
            // 
            // ScanResultsListView
            // 
            this.ScanResultsListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.ScanResultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2});
            this.ScanResultsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScanResultsListView.FullRowSelect = true;
            this.ScanResultsListView.GridLines = true;
            this.ScanResultsListView.HideSelection = false;
            this.ScanResultsListView.HotTracking = true;
            this.ScanResultsListView.HoverSelection = true;
            this.ScanResultsListView.Location = new System.Drawing.Point(0, 85);
            this.ScanResultsListView.MultiSelect = false;
            this.ScanResultsListView.Name = "ScanResultsListView";
            this.ScanResultsListView.ShowGroups = false;
            this.ScanResultsListView.Size = new System.Drawing.Size(418, 254);
            this.ScanResultsListView.TabIndex = 11;
            this.ScanResultsListView.UseCompatibleStateImageBehavior = false;
            this.ScanResultsListView.View = System.Windows.Forms.View.Details;
            this.ScanResultsListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ScanResultsListView_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Server";
            this.columnHeader1.Width = 196;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Service";
            this.columnHeader3.Width = 72;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Port";
            this.columnHeader2.Width = 52;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AddAllButton);
            this.panel1.Controls.Add(this.AllCheckbox);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.CancelButton);
            this.panel1.Controls.Add(this.ScanButton);
            this.panel1.Controls.Add(this.ETextbox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SSHCheckbox);
            this.panel1.Controls.Add(this.ATextbox);
            this.panel1.Controls.Add(this.TelnetCheckbox);
            this.panel1.Controls.Add(this.BTextbox);
            this.panel1.Controls.Add(this.VMRCCheckbox);
            this.panel1.Controls.Add(this.CTextbox);
            this.panel1.Controls.Add(this.VNCCheckbox);
            this.panel1.Controls.Add(this.DTextbox);
            this.panel1.Controls.Add(this.RDPCheckbox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 85);
            this.panel1.TabIndex = 12;
            // 
            // AddAllButton
            // 
            this.AddAllButton.Enabled = false;
            this.AddAllButton.Location = new System.Drawing.Point(6, 56);
            this.AddAllButton.Name = "AddAllButton";
            this.AddAllButton.Size = new System.Drawing.Size(75, 23);
            this.AddAllButton.TabIndex = 17;
            this.AddAllButton.Text = "Add All";
            this.AddAllButton.UseVisualStyleBackColor = true;
            this.AddAllButton.Click += new System.EventHandler(this.AddAllButton_Click);
            // 
            // AllCheckbox
            // 
            this.AllCheckbox.AutoSize = true;
            this.AllCheckbox.Checked = true;
            this.AllCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllCheckbox.Location = new System.Drawing.Point(339, 27);
            this.AllCheckbox.Name = "AllCheckbox";
            this.AllCheckbox.Size = new System.Drawing.Size(37, 17);
            this.AllCheckbox.TabIndex = 16;
            this.AllCheckbox.Text = "All";
            this.AllCheckbox.UseVisualStyleBackColor = true;
            this.AllCheckbox.CheckedChanged += new System.EventHandler(this.AllCheckbox_CheckedChanged);
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(258, 56);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 14;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(339, 56);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 15;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ScanButton
            // 
            this.ScanButton.Location = new System.Drawing.Point(177, 56);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(75, 23);
            this.ScanButton.TabIndex = 11;
            this.ScanButton.Text = "Scan";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScanStatusLabel,
            this.scanProgressBar});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 339);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(418, 22);
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
            // NetworkScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 361);
            this.Controls.Add(this.ScanResultsListView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NetworkScanner";
            this.Text = "Network Scanner";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ATextbox;
        private System.Windows.Forms.TextBox BTextbox;
        private System.Windows.Forms.TextBox CTextbox;
        private System.Windows.Forms.TextBox DTextbox;
        private System.Windows.Forms.TextBox ETextbox;
        private System.Windows.Forms.CheckBox RDPCheckbox;
        private System.Windows.Forms.CheckBox VNCCheckbox;
        private System.Windows.Forms.CheckBox VMRCCheckbox;
        private System.Windows.Forms.CheckBox TelnetCheckbox;
        private System.Windows.Forms.CheckBox SSHCheckbox;
        private System.Windows.Forms.ListView ScanResultsListView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ScanButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ScanStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar scanProgressBar;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.CheckBox AllCheckbox;
        private System.Windows.Forms.Button AddAllButton;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}