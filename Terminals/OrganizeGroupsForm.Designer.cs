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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvGroups = new System.Windows.Forms.ListView();
            this.chGroupName = new System.Windows.Forms.ColumnHeader();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lvConnections = new System.Windows.Forms.ListView();
            this.chConnectionName = new System.Windows.Forms.ColumnHeader();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tsbAddConnection = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteConnection = new System.Windows.Forms.ToolStripButton();
            this.tsbAddGroup = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteGroup = new System.Windows.Forms.ToolStripButton();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lvGroups);
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(168, 312);
            this.panel2.TabIndex = 3;
            // 
            // lvGroups
            // 
            this.lvGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chGroupName});
            this.lvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvGroups.LabelEdit = true;
            this.lvGroups.Location = new System.Drawing.Point(0, 25);
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.Size = new System.Drawing.Size(168, 287);
            this.lvGroups.TabIndex = 2;
            this.lvGroups.UseCompatibleStateImageBehavior = false;
            this.lvGroups.View = System.Windows.Forms.View.Details;
            this.lvGroups.SelectedIndexChanged += new System.EventHandler(this.lvGroups_SelectedIndexChanged);
            // 
            // chGroupName
            // 
            this.chGroupName.Text = "Group Name";
            this.chGroupName.Width = 139;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddGroup,
            this.tsbDeleteGroup});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(168, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lvConnections);
            this.panel1.Controls.Add(this.toolStrip2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(168, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 312);
            this.panel1.TabIndex = 4;
            // 
            // lvConnections
            // 
            this.lvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chConnectionName});
            this.lvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvConnections.Location = new System.Drawing.Point(0, 25);
            this.lvConnections.Name = "lvConnections";
            this.lvConnections.Size = new System.Drawing.Size(221, 287);
            this.lvConnections.TabIndex = 3;
            this.lvConnections.UseCompatibleStateImageBehavior = false;
            this.lvConnections.View = System.Windows.Forms.View.Details;
            // 
            // chConnectionName
            // 
            this.chConnectionName.Text = "Connection Name";
            this.chConnectionName.Width = 190;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddConnection,
            this.tsbDeleteConnection});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(221, 25);
            this.toolStrip2.TabIndex = 4;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 312);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(389, 37);
            this.panel3.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(304, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(168, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 312);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // tsbAddConnection
            // 
            this.tsbAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddConnection.Image = global::Terminals.Properties.Resources.add;
            this.tsbAddConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddConnection.Name = "tsbAddConnection";
            this.tsbAddConnection.Size = new System.Drawing.Size(23, 22);
            this.tsbAddConnection.ToolTipText = "Add Connection";
            this.tsbAddConnection.Click += new System.EventHandler(this.tsbAddConnection_Click);
            // 
            // tsbDeleteConnection
            // 
            this.tsbDeleteConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteConnection.Image = global::Terminals.Properties.Resources.delete;
            this.tsbDeleteConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteConnection.Name = "tsbDeleteConnection";
            this.tsbDeleteConnection.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteConnection.ToolTipText = "Delete Connection";
            this.tsbDeleteConnection.Click += new System.EventHandler(this.tsbDeleteConnection_Click);
            // 
            // tsbAddGroup
            // 
            this.tsbAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddGroup.Image = global::Terminals.Properties.Resources.add;
            this.tsbAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddGroup.Name = "tsbAddGroup";
            this.tsbAddGroup.Size = new System.Drawing.Size(23, 22);
            this.tsbAddGroup.Text = "toolStripButton1";
            this.tsbAddGroup.Click += new System.EventHandler(this.tsbAddGroup_Click);
            // 
            // tsbDeleteGroup
            // 
            this.tsbDeleteGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteGroup.Image = global::Terminals.Properties.Resources.delete;
            this.tsbDeleteGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteGroup.Name = "tsbDeleteGroup";
            this.tsbDeleteGroup.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteGroup.ToolTipText = "Delete Group";
            this.tsbDeleteGroup.Click += new System.EventHandler(this.tsbDeleteGroup_Click);
            // 
            // OrganizeGroupsForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(389, 349);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrganizeGroupsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Organize Groups";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView lvGroups;
        private System.Windows.Forms.ColumnHeader chGroupName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddGroup;
        private System.Windows.Forms.ToolStripButton tsbDeleteGroup;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lvConnections;
        private System.Windows.Forms.ColumnHeader chConnectionName;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsbAddConnection;
        private System.Windows.Forms.ToolStripButton tsbDeleteConnection;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Splitter splitter1;

    }
}