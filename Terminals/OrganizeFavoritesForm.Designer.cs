namespace Terminals
{
    partial class OrganizeFavoritesForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrganizeFavoritesForm));
            this.lvConnections = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colProtocol = new System.Windows.Forms.ColumnHeader();
            this.colComputer = new System.Windows.Forms.ColumnHeader();
            this.colCredential = new System.Windows.Forms.ColumnHeader();
            this.colDomain = new System.Windows.Forms.ColumnHeader();
            this.colUser = new System.Windows.Forms.ColumnHeader();
            this.colTags = new System.Windows.Forms.ColumnHeader();
            this.colNotes = new System.Windows.Forms.ColumnHeader();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.activeDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.networkDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muRDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vRDBackupFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvConnections
            // 
            this.lvConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colProtocol,
            this.colComputer,
            this.colCredential,
            this.colDomain,
            this.colUser,
            this.colTags,
            this.colNotes});
            this.lvConnections.FullRowSelect = true;
            this.lvConnections.GridLines = true;
            this.lvConnections.HideSelection = false;
            this.lvConnections.LabelEdit = true;
            this.lvConnections.Location = new System.Drawing.Point(12, 25);
            this.lvConnections.MultiSelect = false;
            this.lvConnections.Name = "lvConnections";
            this.lvConnections.ShowItemToolTips = true;
            this.lvConnections.Size = new System.Drawing.Size(595, 278);
            this.lvConnections.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvConnections.TabIndex = 1;
            this.lvConnections.UseCompatibleStateImageBehavior = false;
            this.lvConnections.View = System.Windows.Forms.View.Details;
            this.lvConnections.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvConnections_AfterLabelEdit);
            this.lvConnections.SelectedIndexChanged += new System.EventHandler(this.lvConnections_SelectedIndexChanged);
            this.lvConnections.DoubleClick += new System.EventHandler(this.btnEdit_Click);
            this.lvConnections.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvConnections_ColumnClick);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 100;
            // 
            // colProtocol
            // 
            this.colProtocol.Text = "Protocol";
            // 
            // colComputer
            // 
            this.colComputer.Text = "Computer";
            this.colComputer.Width = 89;
            // 
            // colCredential
            // 
            this.colCredential.Text = "Credential";
            // 
            // colDomain
            // 
            this.colDomain.Text = "Domain";
            this.colDomain.Width = 63;
            // 
            // colUser
            // 
            this.colUser.Text = "User";
            this.colUser.Width = 51;
            // 
            // colTags
            // 
            this.colTags.Text = "Tags";
            this.colTags.Width = 88;
            // 
            // colNotes
            // 
            this.colNotes.Text = "Notes";
            this.colNotes.Width = 98;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(613, 278);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 25);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(613, 55);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(106, 24);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "&Edit...";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(613, 85);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(106, 24);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(613, 115);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(106, 24);
            this.btnCopy.TabIndex = 5;
            this.btnCopy.Text = "&Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnRename
            // 
            this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRename.Location = new System.Drawing.Point(613, 145);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(106, 24);
            this.btnRename.TabIndex = 6;
            this.btnRename.Text = "&Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Co&nnections";
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(613, 25);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(106, 24);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "&New...";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportButton.ContextMenuStrip = this.contextMenuStrip1;
            this.ImportButton.Location = new System.Drawing.Point(613, 175);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(106, 23);
            this.ImportButton.TabIndex = 8;
            this.ImportButton.Text = "&Import...";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activeDirectoryToolStripMenuItem,
            this.networkDetectionToolStripMenuItem,
            this.muRDToolStripMenuItem,
            this.rDPToolStripMenuItem,
            this.vRDBackupFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(174, 114);
            // 
            // activeDirectoryToolStripMenuItem
            // 
            this.activeDirectoryToolStripMenuItem.Name = "activeDirectoryToolStripMenuItem";
            this.activeDirectoryToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.activeDirectoryToolStripMenuItem.Text = "Active Directory";
            this.activeDirectoryToolStripMenuItem.Click += new System.EventHandler(this.activeDirectoryToolStripMenuItem_Click);
            // 
            // networkDetectionToolStripMenuItem
            // 
            this.networkDetectionToolStripMenuItem.Name = "networkDetectionToolStripMenuItem";
            this.networkDetectionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.networkDetectionToolStripMenuItem.Text = "Network Detection";
            this.networkDetectionToolStripMenuItem.Click += new System.EventHandler(this.networkDetectionToolStripMenuItem_Click);
            // 
            // muRDToolStripMenuItem
            // 
            this.muRDToolStripMenuItem.Name = "muRDToolStripMenuItem";
            this.muRDToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.muRDToolStripMenuItem.Text = "MuRD File";
            this.muRDToolStripMenuItem.Click += new System.EventHandler(this.muRDToolStripMenuItem_Click);
            // 
            // rDPToolStripMenuItem
            // 
            this.rDPToolStripMenuItem.Name = "rDPToolStripMenuItem";
            this.rDPToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.rDPToolStripMenuItem.Text = "RDP File";
            this.rDPToolStripMenuItem.Click += new System.EventHandler(this.rDPToolStripMenuItem_Click);
            // 
            // vRDBackupFileToolStripMenuItem
            // 
            this.vRDBackupFileToolStripMenuItem.Name = "vRDBackupFileToolStripMenuItem";
            this.vRDBackupFileToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.vRDBackupFileToolStripMenuItem.Text = "vRD Backup File";
            this.vRDBackupFileToolStripMenuItem.Click += new System.EventHandler(this.vRDBackupFileToolStripMenuItem_Click);
            // 
            // OrganizeFavoritesForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(726, 315);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvConnections);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrganizeFavoritesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Organize Favorites";
            this.Shown += new System.EventHandler(this.OrganizeFavoritesForm_Shown);
            this.Activated += new System.EventHandler(this.OrganizeFavoritesForm_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConnectionManager_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvConnections;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colComputer;
        private System.Windows.Forms.ColumnHeader colDomain;
        private System.Windows.Forms.ColumnHeader colUser;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.ColumnHeader colProtocol;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.OpenFileDialog ImportOpenFileDialog;
        private System.Windows.Forms.ColumnHeader colTags;
        private System.Windows.Forms.ColumnHeader colNotes;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem activeDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem networkDetectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem muRDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rDPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vRDBackupFileToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader colCredential;
    }
}