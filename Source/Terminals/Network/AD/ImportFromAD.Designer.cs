namespace Terminals.Network
{
    partial class ImportFromAD
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
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportFromAD));
            this.label1 = new System.Windows.Forms.Label();
            this.domainTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonScanAD = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonImport = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.lblProgressStatus = new System.Windows.Forms.Label();
            this.ldapFilterTextbox = new System.Windows.Forms.TextBox();
            this.ldapFilterLabel = new System.Windows.Forms.Label();
            this.maxResultsTextBox = new System.Windows.Forms.TextBox();
            this.maxResultsLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gridComputers = new Terminals.SortableUnboundGrid();
            this.computerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operatingSystemDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsComputers = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridComputers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsComputers)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(476, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "This dialog allows you to import Computers from your Active Directory Domain dire" +
    "ctly into Terminals.";
            // 
            // domainTextbox
            // 
            this.domainTextbox.Location = new System.Drawing.Point(77, 33);
            this.domainTextbox.Name = "domainTextbox";
            this.domainTextbox.Size = new System.Drawing.Size(275, 20);
            this.domainTextbox.TabIndex = 2;
            this.toolTip.SetToolTip(this.domainTextbox, "Domain name to contact. Doesnt have to be fully qualified domain name.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Domain:";
            // 
            // ButtonScanAD
            // 
            this.ButtonScanAD.Location = new System.Drawing.Point(502, 31);
            this.ButtonScanAD.Name = "ButtonScanAD";
            this.ButtonScanAD.Size = new System.Drawing.Size(75, 23);
            this.ButtonScanAD.TabIndex = 4;
            this.ButtonScanAD.Text = "Scan";
            this.toolTip.SetToolTip(this.ButtonScanAD, "Start or interupt the scan");
            this.ButtonScanAD.UseVisualStyleBackColor = true;
            this.ButtonScanAD.Click += new System.EventHandler(this.ScanADButton_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(505, 546);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 5;
            this.ButtonCancel.Text = "Close";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ButtonImport
            // 
            this.ButtonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonImport.Location = new System.Drawing.Point(358, 546);
            this.ButtonImport.Name = "ButtonImport";
            this.ButtonImport.Size = new System.Drawing.Size(112, 23);
            this.ButtonImport.TabIndex = 6;
            this.ButtonImport.Text = "Import selected";
            this.ButtonImport.UseVisualStyleBackColor = true;
            this.ButtonImport.Click += new System.EventHandler(this.OnButtonImportClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(8, 102);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(572, 10);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 7;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Location = new System.Drawing.Point(196, 546);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 8;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.OnBtnSelectAllClick);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectNone.Location = new System.Drawing.Point(277, 546);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(75, 23);
            this.btnSelectNone.TabIndex = 9;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.OnBtnSelectNoneClick);
            // 
            // lblProgressStatus
            // 
            this.lblProgressStatus.AutoSize = true;
            this.lblProgressStatus.Location = new System.Drawing.Point(8, 86);
            this.lblProgressStatus.Name = "lblProgressStatus";
            this.lblProgressStatus.Size = new System.Drawing.Size(57, 13);
            this.lblProgressStatus.TabIndex = 10;
            this.lblProgressStatus.Text = "Progress...";
            // 
            // ldapFilterTextbox
            // 
            this.ldapFilterTextbox.Location = new System.Drawing.Point(77, 61);
            this.ldapFilterTextbox.Name = "ldapFilterTextbox";
            this.ldapFilterTextbox.Size = new System.Drawing.Size(471, 20);
            this.ldapFilterTextbox.TabIndex = 11;
            this.ldapFilterTextbox.Text = "(objectclass=computer)";
            this.toolTip.SetToolTip(this.ldapFilterTextbox, "LDAP search filter to use. Use default value to obtain all computers only.");
            // 
            // ldapFilterLabel
            // 
            this.ldapFilterLabel.AutoSize = true;
            this.ldapFilterLabel.Location = new System.Drawing.Point(8, 64);
            this.ldapFilterLabel.Name = "ldapFilterLabel";
            this.ldapFilterLabel.Size = new System.Drawing.Size(63, 13);
            this.ldapFilterLabel.TabIndex = 13;
            this.ldapFilterLabel.Text = "LDAP Filter:";
            // 
            // maxResultsTextBox
            // 
            this.maxResultsTextBox.Location = new System.Drawing.Point(440, 33);
            this.maxResultsTextBox.Name = "maxResultsTextBox";
            this.maxResultsTextBox.Size = new System.Drawing.Size(56, 20);
            this.maxResultsTextBox.TabIndex = 15;
            this.maxResultsTextBox.Text = "1000";
            this.toolTip.SetToolTip(this.maxResultsTextBox, "Maximum results to obtain.\r\nNumber in range 0-5000 (Default=1000).");
            // 
            // maxResultsLabel
            // 
            this.maxResultsLabel.AutoSize = true;
            this.maxResultsLabel.Location = new System.Drawing.Point(368, 37);
            this.maxResultsLabel.Name = "maxResultsLabel";
            this.maxResultsLabel.Size = new System.Drawing.Size(66, 13);
            this.maxResultsLabel.TabIndex = 16;
            this.maxResultsLabel.Text = "Max. results:";
            // 
            // resetButton
            // 
            this.resetButton.Image = global::Terminals.Properties.Resources.Refresh;
            this.resetButton.Location = new System.Drawing.Point(554, 58);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(23, 23);
            this.resetButton.TabIndex = 17;
            this.toolTip.SetToolTip(this.resetButton, "Set Filter, Domain and Maximum results to default values.\r\n");
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // gridComputers
            // 
            this.gridComputers.AllowUserToAddRows = false;
            this.gridComputers.AllowUserToOrderColumns = true;
            this.gridComputers.AllowUserToResizeRows = false;
            this.gridComputers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridComputers.AutoGenerateColumns = false;
            this.gridComputers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridComputers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridComputers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridComputers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.computerNameDataGridViewTextBoxColumn,
            this.operatingSystemDataGridViewTextBoxColumn});
            this.gridComputers.DataSource = this.bsComputers;
            this.gridComputers.Location = new System.Drawing.Point(8, 118);
            this.gridComputers.Name = "gridComputers";
            this.gridComputers.RowHeadersVisible = false;
            this.gridComputers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridComputers.Size = new System.Drawing.Size(572, 414);
            this.gridComputers.TabIndex = 0;
            this.gridComputers.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridComputers_ColumnHeaderMouseClick);
            // 
            // computerNameDataGridViewTextBoxColumn
            // 
            this.computerNameDataGridViewTextBoxColumn.DataPropertyName = "ComputerName";
            this.computerNameDataGridViewTextBoxColumn.HeaderText = "ComputerName";
            this.computerNameDataGridViewTextBoxColumn.Name = "computerNameDataGridViewTextBoxColumn";
            this.computerNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.computerNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // operatingSystemDataGridViewTextBoxColumn
            // 
            this.operatingSystemDataGridViewTextBoxColumn.DataPropertyName = "OperatingSystem";
            this.operatingSystemDataGridViewTextBoxColumn.HeaderText = "OperatingSystem";
            this.operatingSystemDataGridViewTextBoxColumn.Name = "operatingSystemDataGridViewTextBoxColumn";
            this.operatingSystemDataGridViewTextBoxColumn.ReadOnly = true;
            this.operatingSystemDataGridViewTextBoxColumn.Width = 200;
            // 
            // bsComputers
            // 
            this.bsComputers.DataSource = typeof(Terminals.Network.ActiveDirectoryComputer);
            // 
            // ImportFromAD
            // 
            this.AcceptButton = this.ButtonImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 581);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.maxResultsLabel);
            this.Controls.Add(this.maxResultsTextBox);
            this.Controls.Add(this.ldapFilterLabel);
            this.Controls.Add(this.ldapFilterTextbox);
            this.Controls.Add(this.lblProgressStatus);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ButtonImport);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonScanAD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.domainTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridComputers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "ImportFromAD";
            this.ShowInTaskbar = false;
            this.Text = "Terminals - Active Directory Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportFromAD_FormClosing);
            this.Load += new System.EventHandler(this.ImportFromAD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridComputers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsComputers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Terminals.SortableUnboundGrid gridComputers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox domainTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonScanAD;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonImport;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.BindingSource bsComputers;
        private System.Windows.Forms.Label lblProgressStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn computerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn operatingSystemDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox ldapFilterTextbox;
        private System.Windows.Forms.Label ldapFilterLabel;
        private System.Windows.Forms.TextBox maxResultsTextBox;
        private System.Windows.Forms.Label maxResultsLabel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.ToolTip toolTip;
    }
}