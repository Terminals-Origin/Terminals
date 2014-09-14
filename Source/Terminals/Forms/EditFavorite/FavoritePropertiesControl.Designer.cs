namespace Terminals.Forms.EditFavorite
{
    partial class FavoritePropertiesControl
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Groups");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Execute before connect");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Notes");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Protocol Options");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoritePropertiesControl));
            this.treeView = new System.Windows.Forms.TreeView();
            this.treeIcons = new System.Windows.Forms.ImageList(this.components);
            this.titleLabel = new System.Windows.Forms.Label();
            this.generalPanel1 = new Terminals.Forms.EditFavorite.GeneralPropertiesUserControl();
            this.protocolOptionsPanel1 = new Terminals.Forms.EditFavorite.ProtocolOptionsPanel();
            this.notesControl1 = new Terminals.Forms.EditFavorite.NotesControl();
            this.groupsPanel1 =new GroupsControl();
            this.executePanel1 = new ExecuteControl();
            this.rasControl1 = new RasControl();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.treeIcons;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.ImageKey = "computer_link.png";
            treeNode1.Name = "generalNode";
            treeNode1.SelectedImageKey = "computer_link.png";
            treeNode1.Text = "General";
            treeNode2.ImageKey = "tag_blue_add.png";
            treeNode2.Name = "groupsNode";
            treeNode2.SelectedImageKey = "tag_blue_add.png";
            treeNode2.Text = "Groups";
            treeNode3.ImageKey = "application_xp_terminal.png";
            treeNode3.Name = "executeNode";
            treeNode3.SelectedImageKey = "application_xp_terminal.png";
            treeNode3.Text = "Execute before connect";
            treeNode4.Name = "notesNode";
            treeNode4.Text = "Notes";
            treeNode5.ImageKey = "terminalsicon.png";
            treeNode5.Name = "protocolOptionsNode";
            treeNode5.SelectedImageKey = "terminalsicon.png";
            treeNode5.Text = "Protocol Options";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(150, 407);
            this.treeView.TabIndex = 1;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
            // 
            // treeIcons
            // 
            this.treeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
            this.treeIcons.TransparentColor = System.Drawing.Color.Magenta;
            this.treeIcons.Images.SetKeyName(0, "computer_link.png");
            this.treeIcons.Images.SetKeyName(1, "tag_blue_add.png");
            this.treeIcons.Images.SetKeyName(2, "application_xp_terminal.png");
            this.treeIcons.Images.SetKeyName(3, "terminalsicon.png");
            this.treeIcons.Images.SetKeyName(4, "treeIcon_rdp.png");
            this.treeIcons.Images.SetKeyName(5, "treeIcon_vnc.png");
            this.treeIcons.Images.SetKeyName(6, "treeIcon_ssh.png");
            this.treeIcons.Images.SetKeyName(7, "treeIcon_telnet.png");
            this.treeIcons.Images.SetKeyName(8, "treeIcon_http.png");
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Location = new System.Drawing.Point(150, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(600, 32);
            this.titleLabel.TabIndex = 7;
            this.titleLabel.Text = "titleLabel";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // generalPanel1
            // 
            this.generalPanel1.Location = new System.Drawing.Point(173, 35);
            this.generalPanel1.Name = "generalPanel1";
            this.generalPanel1.Size = new System.Drawing.Size(205, 154);
            this.generalPanel1.TabIndex = 13;
            // 
            // protocolOptionsPanel1
            // 
            this.protocolOptionsPanel1.Location = new System.Drawing.Point(437, 238);
            this.protocolOptionsPanel1.Name = "protocolOptionsPanel1";
            this.protocolOptionsPanel1.Size = new System.Drawing.Size(186, 141);
            this.protocolOptionsPanel1.TabIndex = 12;
            // 
            // notesControl1
            // 
            this.notesControl1.Location = new System.Drawing.Point(528, 86);
            this.notesControl1.Name = "notesControl1";
            this.notesControl1.Size = new System.Drawing.Size(294, 250);
            this.notesControl1.TabIndex = 17;
            // 
            // groupsPanel1
            // 
            this.groupsPanel1.Location = new System.Drawing.Point(386, 35);
            this.groupsPanel1.Name = "groupsPanel1";
            this.groupsPanel1.Size = new System.Drawing.Size(237, 126);
            this.groupsPanel1.TabIndex = 14;
            // 
            // executePanel1
            // 
            this.executePanel1.Location = new System.Drawing.Point(173, 211);
            this.executePanel1.Name = "executePanel1";
            this.executePanel1.Size = new System.Drawing.Size(185, 168);
            this.executePanel1.TabIndex = 15;
            // 
            // rasControl1
            // 
            this.rasControl1.Location = new System.Drawing.Point(290, 195);
            this.rasControl1.Name = "rasControl1";
            this.rasControl1.Size = new System.Drawing.Size(202, 202);
            this.rasControl1.TabIndex = 16;
            // 
            // FavoritePropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.notesControl1);
            this.Controls.Add(this.executePanel1);
            this.Controls.Add(this.groupsPanel1);
            this.Controls.Add(this.generalPanel1);
            this.Controls.Add(this.protocolOptionsPanel1);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.rasControl1);
            this.Name = "FavoritePropertiesControl";
            this.Size = new System.Drawing.Size(750, 407);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label titleLabel;
        private ProtocolOptionsPanel protocolOptionsPanel1;
        private GeneralPropertiesUserControl generalPanel1;
        private GroupsControl groupsPanel1;
        private ExecuteControl executePanel1;
        private RasControl rasControl1;
        private System.Windows.Forms.ImageList treeIcons;
        private NotesControl notesControl1;
    }
}
