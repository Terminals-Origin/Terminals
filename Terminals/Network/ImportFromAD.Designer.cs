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
            this.gridComputers = new System.Windows.Forms.DataGridView();
            this.Import = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ComputerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperatingSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.computerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsComputers = new System.Windows.Forms.BindingSource(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(this.gridComputers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsComputers)).BeginInit();
            this.SuspendLayout();
            // 
            // gridComputers
            // 
            this.gridComputers.AllowUserToAddRows = false;
            this.gridComputers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridComputers.AutoGenerateColumns = false;
            this.gridComputers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridComputers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridComputers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Import,
            this.ComputerName,
            this.OperatingSystem,
            this.importDataGridViewCheckBoxColumn,
            this.computerNameDataGridViewTextBoxColumn});
            this.gridComputers.DataSource = this.bsComputers;
            this.gridComputers.Location = new System.Drawing.Point(8, 76);
            this.gridComputers.Name = "gridComputers";
            this.gridComputers.RowHeadersVisible = false;
            this.gridComputers.Size = new System.Drawing.Size(572, 248);
            this.gridComputers.TabIndex = 0;
            // 
            // Import
            // 
            this.Import.DataPropertyName = "Import";
            this.Import.HeaderText = "Import";
            this.Import.Name = "Import";
            this.Import.Width = 50;
            // 
            // ComputerName
            // 
            this.ComputerName.DataPropertyName = "ComputerName";
            this.ComputerName.HeaderText = "Computer Name";
            this.ComputerName.Name = "ComputerName";
            this.ComputerName.ReadOnly = true;
            this.ComputerName.Width = 150;
            // 
            // OperatingSystem
            // 
            this.OperatingSystem.DataPropertyName = "OperatingSystem";
            this.OperatingSystem.HeaderText = "Operating System";
            this.OperatingSystem.Name = "OperatingSystem";
            this.OperatingSystem.ReadOnly = true;
            this.OperatingSystem.Width = 300;
            // 
            // importDataGridViewCheckBoxColumn
            // 
            this.importDataGridViewCheckBoxColumn.DataPropertyName = "Import";
            this.importDataGridViewCheckBoxColumn.HeaderText = "Import";
            this.importDataGridViewCheckBoxColumn.Name = "importDataGridViewCheckBoxColumn";
            // 
            // computerNameDataGridViewTextBoxColumn
            // 
            this.computerNameDataGridViewTextBoxColumn.DataPropertyName = "ComputerName";
            this.computerNameDataGridViewTextBoxColumn.HeaderText = "ComputerName";
            this.computerNameDataGridViewTextBoxColumn.Name = "computerNameDataGridViewTextBoxColumn";
            // 
            // bsComputers
            // 
            this.bsComputers.DataSource = typeof(Terminals.Network.ActiveDirectoryComputer);
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
            this.domainTextbox.Location = new System.Drawing.Point(60, 34);
            this.domainTextbox.Name = "domainTextbox";
            this.domainTextbox.Size = new System.Drawing.Size(145, 20);
            this.domainTextbox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Domain:";
            // 
            // ButtonScanAD
            // 
            this.ButtonScanAD.Location = new System.Drawing.Point(211, 32);
            this.ButtonScanAD.Name = "ButtonScanAD";
            this.ButtonScanAD.Size = new System.Drawing.Size(75, 23);
            this.ButtonScanAD.TabIndex = 4;
            this.ButtonScanAD.Text = "Scan";
            this.ButtonScanAD.UseVisualStyleBackColor = true;
            this.ButtonScanAD.Click += new System.EventHandler(this.ScanADButton_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(505, 338);
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
            this.ButtonImport.Location = new System.Drawing.Point(358, 338);
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
            this.progressBar1.Location = new System.Drawing.Point(12, 60);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(568, 10);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 7;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Location = new System.Drawing.Point(196, 338);
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
            this.btnSelectNone.Location = new System.Drawing.Point(277, 338);
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
            this.lblProgressStatus.Location = new System.Drawing.Point(303, 38);
            this.lblProgressStatus.Name = "lblProgressStatus";
            this.lblProgressStatus.Size = new System.Drawing.Size(57, 13);
            this.lblProgressStatus.TabIndex = 10;
            this.lblProgressStatus.Text = "Progress...";
            // 
            // ImportFromAD
            // 
            this.AcceptButton = this.ButtonImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 373);
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

        private System.Windows.Forms.DataGridView gridComputers;
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
        private System.Windows.Forms.DataGridViewCheckBoxColumn Import;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComputerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperatingSystem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn importDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn computerNameDataGridViewTextBoxColumn;
    }
}