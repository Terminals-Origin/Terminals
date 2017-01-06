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
            this.panelContainer = new System.Windows.Forms.Panel();
            this.lblLine = new System.Windows.Forms.Label();
            this.groupsPanel1 = new GroupsControl();
            this.executePanel1 = new ExecuteControl();
            this.rasControl1 = new RasControl(); 
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.treeIcons;
            this.treeView.Location = new System.Drawing.Point(4, 4);
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
            this.treeView.Size = new System.Drawing.Size(144, 397);
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
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(152, 4);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(598, 32);
            this.titleLabel.TabIndex = 7;
            this.titleLabel.Text = "titleLabel";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // generalPanel1
            // 
            this.generalPanel1.Location = new System.Drawing.Point(18, 4);
            this.generalPanel1.Name = "generalPanel1";
            this.generalPanel1.Size = new System.Drawing.Size(205, 154);
            this.generalPanel1.TabIndex = 13;
            // 
            // protocolOptionsPanel1
            // 
            this.protocolOptionsPanel1.Location = new System.Drawing.Point(376, 193);
            this.protocolOptionsPanel1.Name = "protocolOptionsPanel1";
            this.protocolOptionsPanel1.Size = new System.Drawing.Size(186, 141);
            this.protocolOptionsPanel1.TabIndex = 12;
            // 
            // notesControl1
            // 
            this.notesControl1.Location = new System.Drawing.Point(268, 9);
            this.notesControl1.Name = "notesControl1";
            this.notesControl1.Size = new System.Drawing.Size(294, 250);
            this.notesControl1.TabIndex = 17;
            // 
            // panelContainer
            // 
            this.panelContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContainer.Controls.Add(this.notesControl1);
            this.panelContainer.Controls.Add(this.groupsPanel1);
            this.panelContainer.Controls.Add(this.executePanel1);
            this.panelContainer.Controls.Add(this.generalPanel1);
            this.panelContainer.Controls.Add(this.protocolOptionsPanel1);
            this.panelContainer.Controls.Add(this.rasControl1);
            this.panelContainer.Location = new System.Drawing.Point(152, 38);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(595, 354);
            this.panelContainer.TabIndex = 18;
            // 
            // groupsPanel1
            // 
            this.groupsPanel1.Location = new System.Drawing.Point(98, 9);
            this.groupsPanel1.Name = "groupsPanel1";
            this.groupsPanel1.Size = new System.Drawing.Size(237, 126);
            this.groupsPanel1.TabIndex = 14;
            // 
            // executePanel1
            // 
            this.executePanel1.Location = new System.Drawing.Point(18, 191);
            this.executePanel1.Name = "executePanel1";
            this.executePanel1.Size = new System.Drawing.Size(185, 168);
            this.executePanel1.TabIndex = 15;
            // 
            // rasControl1
            // 
            this.rasControl1.Location = new System.Drawing.Point(214, 164);
            this.rasControl1.Name = "rasControl1";
            this.rasControl1.Size = new System.Drawing.Size(202, 202);
            this.rasControl1.TabIndex = 16;
            // 
            // lblLine
            // 
            this.lblLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLine.Location = new System.Drawing.Point(170, 404);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(570, 2);
            this.lblLine.TabIndex = 19;
            this.lblLine.Text = "label1";
            // 
            // FavoritePropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.treeView);
            this.Name = "FavoritePropertiesControl";
            this.Size = new System.Drawing.Size(750, 407);
            this.panelContainer.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.Label lblLine;
    }
}
