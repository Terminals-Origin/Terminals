namespace Terminals
{
    partial class OrganizeGroupsForm
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridGroups = new System.Windows.Forms.DataGridView();
            this.colGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAddGroup = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteGroup = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridGroupFavorites = new System.Windows.Forms.DataGridView();
            this.colFavorite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsbAddConnection = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteConnection = new System.Windows.Forms.ToolStripButton();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGroups)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGroupFavorites)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 316);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(389, 37);
            this.panel3.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(304, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(389, 353);
            this.splitContainer1.SplitterDistance = 186;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridGroups);
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(186, 353);
            this.panel2.TabIndex = 4;
            // 
            // gridGroups
            // 
            this.gridGroups.AllowUserToAddRows = false;
            this.gridGroups.AllowUserToDeleteRows = false;
            this.gridGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGroupName});
            this.gridGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGroups.Location = new System.Drawing.Point(3, 28);
            this.gridGroups.MultiSelect = false;
            this.gridGroups.Name = "gridGroups";
            this.gridGroups.RowHeadersVisible = false;
            this.gridGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGroups.Size = new System.Drawing.Size(180, 322);
            this.gridGroups.TabIndex = 4;
            this.gridGroups.SelectionChanged += new System.EventHandler(this.GridGroups_SelectedRowChanged);
            // 
            // colGroupName
            // 
            this.colGroupName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGroupName.DataPropertyName = "Name";
            this.colGroupName.HeaderText = "Group name";
            this.colGroupName.Name = "colGroupName";
            this.colGroupName.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddGroup,
            this.tsbDeleteGroup});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(180, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAddGroup
            // 
            this.tsbAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddGroup.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.tsbAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddGroup.Name = "tsbAddGroup";
            this.tsbAddGroup.Size = new System.Drawing.Size(23, 22);
            this.tsbAddGroup.Text = "Add new Group";
            this.tsbAddGroup.Click += new System.EventHandler(this.TsbAddGroup_Click);
            // 
            // tsbDeleteGroup
            // 
            this.tsbDeleteGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteGroup.Image = global::Terminals.Properties.Resources.tag_blue_delete;
            this.tsbDeleteGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteGroup.Name = "tsbDeleteGroup";
            this.tsbDeleteGroup.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteGroup.ToolTipText = "Delete Group";
            this.tsbDeleteGroup.Click += new System.EventHandler(this.TsbDeleteGroup_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridGroupFavorites);
            this.panel1.Controls.Add(this.toolStrip2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(197, 353);
            this.panel1.TabIndex = 5;
            // 
            // gridGroupFavorites
            // 
            this.gridGroupFavorites.AllowUserToAddRows = false;
            this.gridGroupFavorites.AllowUserToDeleteRows = false;
            this.gridGroupFavorites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGroupFavorites.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFavorite});
            this.gridGroupFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGroupFavorites.Location = new System.Drawing.Point(3, 28);
            this.gridGroupFavorites.Name = "gridGroupFavorites";
            this.gridGroupFavorites.RowHeadersVisible = false;
            this.gridGroupFavorites.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGroupFavorites.Size = new System.Drawing.Size(191, 322);
            this.gridGroupFavorites.TabIndex = 7;
            // 
            // colFavorite
            // 
            this.colFavorite.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFavorite.DataPropertyName = "Name";
            this.colFavorite.HeaderText = "Group Favorites";
            this.colFavorite.Name = "colFavorite";
            this.colFavorite.ReadOnly = true;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddConnection,
            this.tsbDeleteConnection});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(191, 25);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tsbAddConnection
            // 
            this.tsbAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddConnection.Image = global::Terminals.Properties.Resources.add;
            this.tsbAddConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddConnection.Name = "tsbAddConnection";
            this.tsbAddConnection.Size = new System.Drawing.Size(23, 22);
            this.tsbAddConnection.ToolTipText = "Select favorites for selected group";
            this.tsbAddConnection.Click += new System.EventHandler(this.TsbAddConnection_Click);
            // 
            // tsbDeleteConnection
            // 
            this.tsbDeleteConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteConnection.Image = global::Terminals.Properties.Resources.delete;
            this.tsbDeleteConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteConnection.Name = "tsbDeleteConnection";
            this.tsbDeleteConnection.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteConnection.ToolTipText = "Remove selected favorites from selected group";
            this.tsbDeleteConnection.Click += new System.EventHandler(this.TsbDeleteConnection_Click);
            // 
            // OrganizeGroupsForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(389, 353);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 380);
            this.Name = "OrganizeGroupsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Organize Groups";
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGroups)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGroupFavorites)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView gridGroups;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroupName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddGroup;
        private System.Windows.Forms.ToolStripButton tsbDeleteGroup;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gridGroupFavorites;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFavorite;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsbAddConnection;
        private System.Windows.Forms.ToolStripButton tsbDeleteConnection;

    }
}