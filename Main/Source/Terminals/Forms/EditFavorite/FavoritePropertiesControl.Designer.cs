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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Groups");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Execute before connect");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Protocol Options");
            this.treeView = new System.Windows.Forms.TreeView();
            this.titleLabel = new System.Windows.Forms.Label();
            this.executePanel1 = new Terminals.Forms.EditFavorite.ExecuteControl();
            this.groupsPanel1 = new Terminals.Forms.EditFavorite.GroupsControl();
            this.generalPanel1 = new Terminals.Forms.EditFavorite.GeneralPropertiesUserControl();
            this.protocolOptionsPanel1 = new Terminals.Forms.EditFavorite.ProtocolOptionsPanel();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.BackColor = System.Drawing.SystemColors.Control;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.Name = "generalNode";
            treeNode1.Text = "General";
            treeNode2.Name = "groupsNode";
            treeNode2.Text = "Groups";
            treeNode3.Name = "executeNode";
            treeNode3.Text = "Execute before connect";
            treeNode4.Name = "protocolOptionsNode";
            treeNode4.Text = "Protocol Options";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.treeView.Size = new System.Drawing.Size(167, 407);
            this.treeView.TabIndex = 1;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Location = new System.Drawing.Point(167, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(492, 32);
            this.titleLabel.TabIndex = 7;
            this.titleLabel.Text = "titleLabel";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // executePanel1
            // 
            this.executePanel1.Location = new System.Drawing.Point(173, 211);
            this.executePanel1.Name = "executePanel1";
            this.executePanel1.Size = new System.Drawing.Size(185, 168);
            this.executePanel1.TabIndex = 15;
            // 
            // groupsPanel1
            // 
            this.groupsPanel1.Location = new System.Drawing.Point(386, 35);
            this.groupsPanel1.Name = "groupsPanel1";
            this.groupsPanel1.Size = new System.Drawing.Size(237, 126);
            this.groupsPanel1.TabIndex = 14;
            // 
            // genearalPanel1
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
            // FavoritePropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.executePanel1);
            this.Controls.Add(this.groupsPanel1);
            this.Controls.Add(this.generalPanel1);
            this.Controls.Add(this.protocolOptionsPanel1);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.treeView);
            this.Name = "FavoritePropertiesControl";
            this.Size = new System.Drawing.Size(659, 407);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label titleLabel;
        private ProtocolOptionsPanel protocolOptionsPanel1;
        private GeneralPropertiesUserControl generalPanel1;
        private GroupsControl groupsPanel1;
        private ExecuteControl executePanel1;
    }
}
