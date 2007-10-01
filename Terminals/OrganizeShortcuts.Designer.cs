namespace Terminals
{
    partial class OrganizeShortcuts
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
            this.shortcutCombobox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.executableTextBox = new System.Windows.Forms.TextBox();
            this.executableBrowseButton = new System.Windows.Forms.Button();
            this.argumentsTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.workingFolderTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.iconLabel = new System.Windows.Forms.Label();
            this.iconPicturebox = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.IconImageList = new System.Windows.Forms.ImageList(this.components);
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.iconPicturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // shortcutCombobox
            // 
            this.shortcutCombobox.FormattingEnabled = true;
            this.shortcutCombobox.Location = new System.Drawing.Point(88, 6);
            this.shortcutCombobox.Name = "shortcutCombobox";
            this.shortcutCombobox.Size = new System.Drawing.Size(230, 21);
            this.shortcutCombobox.TabIndex = 0;
            this.shortcutCombobox.SelectedIndexChanged += new System.EventHandler(this.shortcutCombobox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Shortcut:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Executable:";
            // 
            // executableTextBox
            // 
            this.executableTextBox.Location = new System.Drawing.Point(88, 37);
            this.executableTextBox.Name = "executableTextBox";
            this.executableTextBox.Size = new System.Drawing.Size(208, 20);
            this.executableTextBox.TabIndex = 3;
            // 
            // executableBrowseButton
            // 
            this.executableBrowseButton.Location = new System.Drawing.Point(302, 35);
            this.executableBrowseButton.Name = "executableBrowseButton";
            this.executableBrowseButton.Size = new System.Drawing.Size(25, 23);
            this.executableBrowseButton.TabIndex = 4;
            this.executableBrowseButton.Text = "...";
            this.executableBrowseButton.UseVisualStyleBackColor = true;
            this.executableBrowseButton.Click += new System.EventHandler(this.executableBrowseButton_Click);
            // 
            // argumentsTextBox
            // 
            this.argumentsTextBox.Location = new System.Drawing.Point(88, 99);
            this.argumentsTextBox.Name = "argumentsTextBox";
            this.argumentsTextBox.Size = new System.Drawing.Size(208, 20);
            this.argumentsTextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Arguments:";
            // 
            // workingFolderTextBox
            // 
            this.workingFolderTextBox.Location = new System.Drawing.Point(88, 130);
            this.workingFolderTextBox.Name = "workingFolderTextBox";
            this.workingFolderTextBox.Size = new System.Drawing.Size(208, 20);
            this.workingFolderTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Working Folder:";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(252, 166);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "S&ave";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(3, 166);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "C&lose";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(171, 166);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 12;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // iconLabel
            // 
            this.iconLabel.AutoSize = true;
            this.iconLabel.Location = new System.Drawing.Point(0, 71);
            this.iconLabel.Name = "iconLabel";
            this.iconLabel.Size = new System.Drawing.Size(31, 13);
            this.iconLabel.TabIndex = 13;
            this.iconLabel.Text = "Icon:";
            // 
            // iconPicturebox
            // 
            this.iconPicturebox.Location = new System.Drawing.Point(88, 68);
            this.iconPicturebox.Name = "iconPicturebox";
            this.iconPicturebox.Size = new System.Drawing.Size(16, 16);
            this.iconPicturebox.TabIndex = 14;
            this.iconPicturebox.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Location = new System.Drawing.Point(186, 68);
            this.toolStrip1.MaximumSize = new System.Drawing.Size(100, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(34, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // IconImageList
            // 
            this.IconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.IconImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.IconImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(119, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Icon Picker:";
            // 
            // OrganizeShortcuts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 196);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.iconPicturebox);
            this.Controls.Add(this.iconLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.workingFolderTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.argumentsTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.executableBrowseButton);
            this.Controls.Add(this.executableTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shortcutCombobox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrganizeShortcuts";
            this.Text = "Organize Shortcuts";
            this.Load += new System.EventHandler(this.OrganizeShortcuts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicturebox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox shortcutCombobox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox executableTextBox;
        private System.Windows.Forms.Button executableBrowseButton;
        private System.Windows.Forms.TextBox argumentsTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox workingFolderTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label iconLabel;
        private System.Windows.Forms.PictureBox iconPicturebox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ImageList IconImageList;
        private System.Windows.Forms.Label label5;
    }
}