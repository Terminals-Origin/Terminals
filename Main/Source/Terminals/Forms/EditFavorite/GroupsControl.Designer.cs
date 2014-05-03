namespace Terminals.Forms.EditFavorite
{
    partial class GroupsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.btnAddNewTag = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRemoveTag = new System.Windows.Forms.Button();
            this.lvConnectionTags = new System.Windows.Forms.ListView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.AllTagsAddButton = new System.Windows.Forms.Button();
            this.AllTagsListView = new System.Windows.Forms.ListView();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtGroupName);
            this.panel3.Controls.Add(this.btnAddNewTag);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(590, 50);
            this.panel3.TabIndex = 1;
            // 
            // txtGroupName
            // 
            this.txtGroupName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtGroupName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtGroupName.Location = new System.Drawing.Point(93, 19);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(419, 20);
            this.txtGroupName.TabIndex = 1;
            // 
            // btnAddNewTag
            // 
            this.btnAddNewTag.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.btnAddNewTag.Location = new System.Drawing.Point(518, 18);
            this.btnAddNewTag.Name = "btnAddNewTag";
            this.btnAddNewTag.Size = new System.Drawing.Size(21, 21);
            this.btnAddNewTag.TabIndex = 2;
            this.btnAddNewTag.UseVisualStyleBackColor = true;
            this.btnAddNewTag.Click += new System.EventHandler(this.BtnAddNewTag_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "New Group:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(590, 96);
            this.panel1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRemoveTag);
            this.groupBox3.Controls.Add(this.lvConnectionTags);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(590, 96);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Connection Groups";
            // 
            // btnRemoveTag
            // 
            this.btnRemoveTag.Image = global::Terminals.Properties.Resources.tag_blue_delete;
            this.btnRemoveTag.Location = new System.Drawing.Point(518, 23);
            this.btnRemoveTag.Name = "btnRemoveTag";
            this.btnRemoveTag.Size = new System.Drawing.Size(21, 21);
            this.btnRemoveTag.TabIndex = 1;
            this.btnRemoveTag.UseVisualStyleBackColor = true;
            this.btnRemoveTag.Click += new System.EventHandler(this.BtnRemoveTag_Click);
            // 
            // lvConnectionTags
            // 
            this.lvConnectionTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvConnectionTags.HideSelection = false;
            this.lvConnectionTags.Location = new System.Drawing.Point(8, 24);
            this.lvConnectionTags.Name = "lvConnectionTags";
            this.lvConnectionTags.Size = new System.Drawing.Size(503, 66);
            this.lvConnectionTags.TabIndex = 0;
            this.lvConnectionTags.UseCompatibleStateImageBehavior = false;
            this.lvConnectionTags.View = System.Windows.Forms.View.List;
            this.lvConnectionTags.DoubleClick += new System.EventHandler(this.LvConnectionTags_DoubleClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 146);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(590, 219);
            this.panel4.TabIndex = 15;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.AllTagsAddButton);
            this.groupBox4.Controls.Add(this.AllTagsListView);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(590, 219);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "All Available Groups";
            // 
            // AllTagsAddButton
            // 
            this.AllTagsAddButton.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.AllTagsAddButton.Location = new System.Drawing.Point(520, 20);
            this.AllTagsAddButton.Name = "AllTagsAddButton";
            this.AllTagsAddButton.Size = new System.Drawing.Size(21, 21);
            this.AllTagsAddButton.TabIndex = 1;
            this.AllTagsAddButton.UseVisualStyleBackColor = true;
            this.AllTagsAddButton.Click += new System.EventHandler(this.AllTagsAddButton_Click);
            // 
            // AllTagsListView
            // 
            this.AllTagsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AllTagsListView.HideSelection = false;
            this.AllTagsListView.Location = new System.Drawing.Point(8, 20);
            this.AllTagsListView.Name = "AllTagsListView";
            this.AllTagsListView.Size = new System.Drawing.Size(503, 176);
            this.AllTagsListView.TabIndex = 0;
            this.AllTagsListView.UseCompatibleStateImageBehavior = false;
            this.AllTagsListView.View = System.Windows.Forms.View.List;
            this.AllTagsListView.DoubleClick += new System.EventHandler(this.AllTagsListView_DoubleClick);
            // 
            // GroupsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "GroupsControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Button btnAddNewTag;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRemoveTag;
        private System.Windows.Forms.ListView lvConnectionTags;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button AllTagsAddButton;
        private System.Windows.Forms.ListView AllTagsListView;

    }
}
