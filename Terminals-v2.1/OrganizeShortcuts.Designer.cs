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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrganizeShortcuts));
            this.shortcutCombobox = new System.Windows.Forms.ComboBox();
            this.labelShortcut = new System.Windows.Forms.Label();
            this.labelExec = new System.Windows.Forms.Label();
            this.executableTextBox = new System.Windows.Forms.TextBox();
            this.executableBrowseButton = new System.Windows.Forms.Button();
            this.argumentsTextBox = new System.Windows.Forms.TextBox();
            this.labelArguments = new System.Windows.Forms.Label();
            this.workingFolderTextBox = new System.Windows.Forms.TextBox();
            this.labelWorkingDir = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.iconLabel = new System.Windows.Forms.Label();
            this.iconPicturebox = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.IconImageList = new System.Windows.Forms.ImageList(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.buttonsToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // shortcutCombobox
            // 
            this.shortcutCombobox.FormattingEnabled = true;
            this.shortcutCombobox.Location = new System.Drawing.Point(123, 12);
            this.shortcutCombobox.Name = "shortcutCombobox";
            this.shortcutCombobox.Size = new System.Drawing.Size(239, 21);
            this.shortcutCombobox.TabIndex = 0;
            this.buttonsToolTip.SetToolTip(this.shortcutCombobox, "Type new shortcut here or select one from list to change");
            this.shortcutCombobox.SelectedIndexChanged += new System.EventHandler(this.shortcutCombobox_SelectedIndexChanged);
            // 
            // labelShortcut
            // 
            this.labelShortcut.AutoSize = true;
            this.labelShortcut.Location = new System.Drawing.Point(55, 15);
            this.labelShortcut.Name = "labelShortcut";
            this.labelShortcut.Size = new System.Drawing.Size(50, 13);
            this.labelShortcut.TabIndex = 1;
            this.labelShortcut.Text = "Shortcut:";
            // 
            // labelExec
            // 
            this.labelExec.AutoSize = true;
            this.labelExec.Location = new System.Drawing.Point(42, 46);
            this.labelExec.Name = "labelExec";
            this.labelExec.Size = new System.Drawing.Size(63, 13);
            this.labelExec.TabIndex = 2;
            this.labelExec.Text = "Executable:";
            // 
            // executableTextBox
            // 
            this.executableTextBox.Location = new System.Drawing.Point(123, 43);
            this.executableTextBox.Name = "executableTextBox";
            this.executableTextBox.Size = new System.Drawing.Size(208, 20);
            this.executableTextBox.TabIndex = 3;
            this.buttonsToolTip.SetToolTip(this.executableTextBox, "Type full path to the executable");
            // 
            // executableBrowseButton
            // 
            this.executableBrowseButton.Location = new System.Drawing.Point(337, 41);
            this.executableBrowseButton.Name = "executableBrowseButton";
            this.executableBrowseButton.Size = new System.Drawing.Size(25, 23);
            this.executableBrowseButton.TabIndex = 4;
            this.executableBrowseButton.Text = "...";
            this.buttonsToolTip.SetToolTip(this.executableBrowseButton, "Show dialog to select a shortcut executable");
            this.executableBrowseButton.UseVisualStyleBackColor = true;
            this.executableBrowseButton.Click += new System.EventHandler(this.executableBrowseButton_Click);
            // 
            // argumentsTextBox
            // 
            this.argumentsTextBox.Location = new System.Drawing.Point(123, 105);
            this.argumentsTextBox.Name = "argumentsTextBox";
            this.argumentsTextBox.Size = new System.Drawing.Size(239, 20);
            this.argumentsTextBox.TabIndex = 6;
            this.buttonsToolTip.SetToolTip(this.argumentsTextBox, "Type here the executable startup command line parameters");
            // 
            // labelArguments
            // 
            this.labelArguments.AutoSize = true;
            this.labelArguments.Location = new System.Drawing.Point(45, 108);
            this.labelArguments.Name = "labelArguments";
            this.labelArguments.Size = new System.Drawing.Size(60, 13);
            this.labelArguments.TabIndex = 5;
            this.labelArguments.Text = "Arguments:";
            // 
            // workingFolderTextBox
            // 
            this.workingFolderTextBox.Location = new System.Drawing.Point(123, 136);
            this.workingFolderTextBox.Name = "workingFolderTextBox";
            this.workingFolderTextBox.Size = new System.Drawing.Size(239, 20);
            this.workingFolderTextBox.TabIndex = 8;
            this.buttonsToolTip.SetToolTip(this.workingFolderTextBox, "Type full path to the executable working directory");
            // 
            // labelWorkingDir
            // 
            this.labelWorkingDir.AutoSize = true;
            this.labelWorkingDir.Location = new System.Drawing.Point(12, 139);
            this.labelWorkingDir.Name = "labelWorkingDir";
            this.labelWorkingDir.Size = new System.Drawing.Size(93, 13);
            this.labelWorkingDir.TabIndex = 7;
            this.labelWorkingDir.Text = "Working directory:";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(206, 175);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "S&ave";
            this.buttonsToolTip.SetToolTip(this.saveButton, "Saves existing or adds new shortcut to the Shortcuts list");
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(287, 175);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 12;
            this.closeButton.Text = "C&lose";
            this.buttonsToolTip.SetToolTip(this.closeButton, "Closes this edit form without saving changes. You have to save your changes firs." +
        "");
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(123, 175);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 10;
            this.deleteButton.Text = "&Delete";
            this.buttonsToolTip.SetToolTip(this.deleteButton, "Deletes an item from Shortcuts list");
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // iconLabel
            // 
            this.iconLabel.AutoSize = true;
            this.iconLabel.Location = new System.Drawing.Point(74, 77);
            this.iconLabel.Name = "iconLabel";
            this.iconLabel.Size = new System.Drawing.Size(31, 13);
            this.iconLabel.TabIndex = 13;
            this.iconLabel.Text = "Icon:";
            // 
            // iconPicturebox
            // 
            this.iconPicturebox.Location = new System.Drawing.Point(123, 74);
            this.iconPicturebox.Name = "iconPicturebox";
            this.iconPicturebox.Size = new System.Drawing.Size(16, 16);
            this.iconPicturebox.TabIndex = 14;
            this.iconPicturebox.TabStop = false;
            this.buttonsToolTip.SetToolTip(this.iconPicturebox, "Selected shortcut icon");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Location = new System.Drawing.Point(221, 74);
            this.toolStrip1.MaximumSize = new System.Drawing.Size(100, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(100, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            this.buttonsToolTip.SetToolTip(this.toolStrip1, "Select an icon for the shortcut");
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
            this.label5.Location = new System.Drawing.Point(154, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Icon Picker:";
            // 
            // buttonsToolTip
            // 
            this.buttonsToolTip.ShowAlways = true;
            // 
            // OrganizeShortcuts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 210);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.iconPicturebox);
            this.Controls.Add(this.iconLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.workingFolderTextBox);
            this.Controls.Add(this.labelWorkingDir);
            this.Controls.Add(this.argumentsTextBox);
            this.Controls.Add(this.labelArguments);
            this.Controls.Add(this.executableBrowseButton);
            this.Controls.Add(this.executableTextBox);
            this.Controls.Add(this.labelExec);
            this.Controls.Add(this.labelShortcut);
            this.Controls.Add(this.shortcutCombobox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrganizeShortcuts";
            this.ShowIcon = false;
            this.Text = "Organize Shortcuts";
            this.Load += new System.EventHandler(this.OrganizeShortcuts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicturebox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox shortcutCombobox;
        private System.Windows.Forms.Label labelShortcut;
        private System.Windows.Forms.Label labelExec;
        private System.Windows.Forms.TextBox executableTextBox;
        private System.Windows.Forms.Button executableBrowseButton;
        private System.Windows.Forms.TextBox argumentsTextBox;
        private System.Windows.Forms.Label labelArguments;
        private System.Windows.Forms.TextBox workingFolderTextBox;
        private System.Windows.Forms.Label labelWorkingDir;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label iconLabel;
        private System.Windows.Forms.PictureBox iconPicturebox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ImageList IconImageList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip buttonsToolTip;
    }
}