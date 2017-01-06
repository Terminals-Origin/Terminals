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
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.btnAddNewTag = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btnRemoveTag = new System.Windows.Forms.Button();
            this.lvConnectionTags = new System.Windows.Forms.ListView();
            this.AllTagsAddButton = new System.Windows.Forms.Button();
            this.AllTagsListView = new System.Windows.Forms.ListView();
            this.selectedLabel = new System.Windows.Forms.Label();
            this.allLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtGroupName
            // 
            this.txtGroupName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtGroupName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtGroupName.Location = new System.Drawing.Point(93, 19);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(422, 20);
            this.txtGroupName.TabIndex = 1;
            // 
            // btnAddNewTag
            // 
            this.btnAddNewTag.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.btnAddNewTag.Location = new System.Drawing.Point(521, 18);
            this.btnAddNewTag.Name = "btnAddNewTag";
            this.btnAddNewTag.Size = new System.Drawing.Size(21, 21);
            this.btnAddNewTag.TabIndex = 2;
            this.btnAddNewTag.UseVisualStyleBackColor = true;
            this.btnAddNewTag.Click += new System.EventHandler(this.BtnAddNewTag_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "New Group:";
            // 
            // btnRemoveTag
            // 
            this.btnRemoveTag.Image = global::Terminals.Properties.Resources.tag_blue_delete;
            this.btnRemoveTag.Location = new System.Drawing.Point(521, 74);
            this.btnRemoveTag.Name = "btnRemoveTag";
            this.btnRemoveTag.Size = new System.Drawing.Size(21, 21);
            this.btnRemoveTag.TabIndex = 1;
            this.btnRemoveTag.UseVisualStyleBackColor = true;
            this.btnRemoveTag.Click += new System.EventHandler(this.BtnRemoveTag_Click);
            // 
            // lvConnectionTags
            // 
            this.lvConnectionTags.HideSelection = false;
            this.lvConnectionTags.Location = new System.Drawing.Point(12, 74);
            this.lvConnectionTags.Name = "lvConnectionTags";
            this.lvConnectionTags.Size = new System.Drawing.Size(503, 66);
            this.lvConnectionTags.TabIndex = 0;
            this.lvConnectionTags.UseCompatibleStateImageBehavior = false;
            this.lvConnectionTags.View = System.Windows.Forms.View.List;
            this.lvConnectionTags.DoubleClick += new System.EventHandler(this.LvConnectionTags_DoubleClick);
            // 
            // AllTagsAddButton
            // 
            this.AllTagsAddButton.Image = global::Terminals.Properties.Resources.tag_blue_add;
            this.AllTagsAddButton.Location = new System.Drawing.Point(521, 176);
            this.AllTagsAddButton.Name = "AllTagsAddButton";
            this.AllTagsAddButton.Size = new System.Drawing.Size(21, 21);
            this.AllTagsAddButton.TabIndex = 1;
            this.AllTagsAddButton.UseVisualStyleBackColor = true;
            this.AllTagsAddButton.Click += new System.EventHandler(this.AllTagsAddButton_Click);
            // 
            // AllTagsListView
            // 
            this.AllTagsListView.HideSelection = false;
            this.AllTagsListView.Location = new System.Drawing.Point(9, 176);
            this.AllTagsListView.Name = "AllTagsListView";
            this.AllTagsListView.Size = new System.Drawing.Size(506, 150);
            this.AllTagsListView.TabIndex = 0;
            this.AllTagsListView.UseCompatibleStateImageBehavior = false;
            this.AllTagsListView.View = System.Windows.Forms.View.List;
            this.AllTagsListView.DoubleClick += new System.EventHandler(this.AllTagsListView_DoubleClick);
            // 
            // selectedLabel
            // 
            this.selectedLabel.AutoSize = true;
            this.selectedLabel.Location = new System.Drawing.Point(9, 58);
            this.selectedLabel.Name = "selectedLabel";
            this.selectedLabel.Size = new System.Drawing.Size(88, 13);
            this.selectedLabel.TabIndex = 3;
            this.selectedLabel.Text = "Assigned groups:";
            // 
            // allLabel
            // 
            this.allLabel.AutoSize = true;
            this.allLabel.Location = new System.Drawing.Point(9, 160);
            this.allLabel.Name = "allLabel";
            this.allLabel.Size = new System.Drawing.Size(101, 13);
            this.allLabel.TabIndex = 4;
            this.allLabel.Text = "All available groups:";
            // 
            // GroupsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.allLabel);
            this.Controls.Add(this.selectedLabel);
            this.Controls.Add(this.AllTagsListView);
            this.Controls.Add(this.AllTagsAddButton);
            this.Controls.Add(this.btnRemoveTag);
            this.Controls.Add(this.txtGroupName);
            this.Controls.Add(this.lvConnectionTags);
            this.Controls.Add(this.btnAddNewTag);
            this.Controls.Add(this.label14);
            this.Name = "GroupsControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Button btnAddNewTag;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnRemoveTag;
        private System.Windows.Forms.ListView lvConnectionTags;
        private System.Windows.Forms.Button AllTagsAddButton;
        private System.Windows.Forms.ListView AllTagsListView;
        private System.Windows.Forms.Label selectedLabel;
        private System.Windows.Forms.Label allLabel;

    }
}
