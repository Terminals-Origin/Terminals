namespace Terminals
{
    partial class TagsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagsForm));
            this.lvTagConnections = new System.Windows.Forms.ListView();
            this.chConnection = new System.Windows.Forms.ColumnHeader();
            this.cmsTagConnections = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTagConnections = new System.Windows.Forms.ImageList(this.components);
            this.lvTags = new System.Windows.Forms.ListView();
            this.chTag = new System.Windows.Forms.ColumnHeader();
            this.cmsTags = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTags = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tscbSearchTag = new System.Windows.Forms.ToolStripComboBox();
            this.cmsTagConnections.SuspendLayout();
            this.cmsTags.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvTagConnections
            // 
            this.lvTagConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chConnection});
            this.lvTagConnections.ContextMenuStrip = this.cmsTagConnections;
            this.lvTagConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTagConnections.Location = new System.Drawing.Point(200, 25);
            this.lvTagConnections.Name = "lvTagConnections";
            this.lvTagConnections.Size = new System.Drawing.Size(200, 214);
            this.lvTagConnections.SmallImageList = this.ilTagConnections;
            this.lvTagConnections.TabIndex = 3;
            this.lvTagConnections.UseCompatibleStateImageBehavior = false;
            this.lvTagConnections.View = System.Windows.Forms.View.Details;
            this.lvTagConnections.SelectedIndexChanged += new System.EventHandler(this.lvTagConnections_SelectedIndexChanged);
            // 
            // chConnection
            // 
            this.chConnection.Text = "Connection";
            this.chConnection.Width = 174;
            // 
            // cmsTagConnections
            // 
            this.cmsTagConnections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem});
            this.cmsTagConnections.Name = "cmsTagConnections";
            this.cmsTagConnections.Size = new System.Drawing.Size(115, 26);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Enabled = false;
            this.connectToolStripMenuItem.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.connectToolStripMenuItem.Text = "&Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // ilTagConnections
            // 
            this.ilTagConnections.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTagConnections.ImageStream")));
            this.ilTagConnections.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTagConnections.Images.SetKeyName(0, "");
            // 
            // lvTags
            // 
            this.lvTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTag});
            this.lvTags.ContextMenuStrip = this.cmsTags;
            this.lvTags.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvTags.Location = new System.Drawing.Point(0, 25);
            this.lvTags.MultiSelect = false;
            this.lvTags.Name = "lvTags";
            this.lvTags.Size = new System.Drawing.Size(200, 214);
            this.lvTags.SmallImageList = this.ilTags;
            this.lvTags.TabIndex = 2;
            this.lvTags.UseCompatibleStateImageBehavior = false;
            this.lvTags.View = System.Windows.Forms.View.Details;
            this.lvTags.SelectedIndexChanged += new System.EventHandler(this.lvTags_SelectedIndexChanged);
            // 
            // chTag
            // 
            this.chTag.Text = "Tag";
            this.chTag.Width = 173;
            // 
            // cmsTags
            // 
            this.cmsTags.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToAllToolStripMenuItem});
            this.cmsTags.Name = "cmsTags";
            this.cmsTags.Size = new System.Drawing.Size(144, 26);
            // 
            // connectToAllToolStripMenuItem
            // 
            this.connectToAllToolStripMenuItem.Image = global::Terminals.Properties.Resources.application_lightning;
            this.connectToAllToolStripMenuItem.Name = "connectToAllToolStripMenuItem";
            this.connectToAllToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.connectToAllToolStripMenuItem.Text = "&Connect To All";
            this.connectToAllToolStripMenuItem.Click += new System.EventHandler(this.connectToAllToolStripMenuItem_Click);
            // 
            // ilTags
            // 
            this.ilTags.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTags.ImageStream")));
            this.ilTags.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTags.Images.SetKeyName(0, "tag.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tscbSearchTag});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(400, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel1.Text = "Search:";
            // 
            // tscbSearchTag
            // 
            this.tscbSearchTag.Name = "tscbSearchTag";
            this.tscbSearchTag.Size = new System.Drawing.Size(200, 25);
            this.tscbSearchTag.TextChanged += new System.EventHandler(this.tscbSearchTag_TextChanged);
            // 
            // TagsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 239);
            this.Controls.Add(this.lvTagConnections);
            this.Controls.Add(this.lvTags);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "TagsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Tags";
            this.Deactivate += new System.EventHandler(this.TagsForm_Deactivate);
            this.Activated += new System.EventHandler(this.TagsForm_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TagsForm_KeyDown);
            this.cmsTagConnections.ResumeLayout(false);
            this.cmsTags.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvTagConnections;
        private System.Windows.Forms.ListView lvTags;
        private System.Windows.Forms.ColumnHeader chTag;
        private System.Windows.Forms.ColumnHeader chConnection;
        private System.Windows.Forms.ContextMenuStrip cmsTagConnections;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ImageList ilTagConnections;
        private System.Windows.Forms.ContextMenuStrip cmsTags;
        private System.Windows.Forms.ToolStripMenuItem connectToAllToolStripMenuItem;
        private System.Windows.Forms.ImageList ilTags;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tscbSearchTag;
    }
}