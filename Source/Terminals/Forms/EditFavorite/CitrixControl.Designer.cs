namespace Terminals.Forms.EditFavorite
{
    partial class CitrixControl
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
            this.IcaGroupBox = new System.Windows.Forms.GroupBox();
            this.ClientINIBrowseButton = new System.Windows.Forms.Button();
            this.ServerINIBrowseButton = new System.Windows.Forms.Button();
            this.AppWorkingFolderBrowseButton = new System.Windows.Forms.Button();
            this.appPathBrowseButton = new System.Windows.Forms.Button();
            this.ICAEncryptionLevelCombobox = new System.Windows.Forms.ComboBox();
            this.ICAEnableEncryptionCheckbox = new System.Windows.Forms.CheckBox();
            this.ICAClientINI = new System.Windows.Forms.TextBox();
            this.ICAServerINI = new System.Windows.Forms.TextBox();
            this.ICAWorkingFolder = new System.Windows.Forms.TextBox();
            this.ICAApplicationPath = new System.Windows.Forms.TextBox();
            this.ICAApplicationNameTextBox = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.IcaGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // IcaGroupBox
            // 
            this.IcaGroupBox.Controls.Add(this.ClientINIBrowseButton);
            this.IcaGroupBox.Controls.Add(this.ServerINIBrowseButton);
            this.IcaGroupBox.Controls.Add(this.AppWorkingFolderBrowseButton);
            this.IcaGroupBox.Controls.Add(this.appPathBrowseButton);
            this.IcaGroupBox.Controls.Add(this.ICAEncryptionLevelCombobox);
            this.IcaGroupBox.Controls.Add(this.ICAEnableEncryptionCheckbox);
            this.IcaGroupBox.Controls.Add(this.ICAClientINI);
            this.IcaGroupBox.Controls.Add(this.ICAServerINI);
            this.IcaGroupBox.Controls.Add(this.ICAWorkingFolder);
            this.IcaGroupBox.Controls.Add(this.ICAApplicationPath);
            this.IcaGroupBox.Controls.Add(this.ICAApplicationNameTextBox);
            this.IcaGroupBox.Controls.Add(this.label35);
            this.IcaGroupBox.Controls.Add(this.label34);
            this.IcaGroupBox.Controls.Add(this.label23);
            this.IcaGroupBox.Controls.Add(this.label22);
            this.IcaGroupBox.Controls.Add(this.label21);
            this.IcaGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IcaGroupBox.Location = new System.Drawing.Point(0, 0);
            this.IcaGroupBox.Name = "IcaGroupBox";
            this.IcaGroupBox.Size = new System.Drawing.Size(590, 365);
            this.IcaGroupBox.TabIndex = 1;
            this.IcaGroupBox.TabStop = false;
            // 
            // ClientINIBrowseButton
            // 
            this.ClientINIBrowseButton.Location = new System.Drawing.Point(422, 137);
            this.ClientINIBrowseButton.Name = "ClientINIBrowseButton";
            this.ClientINIBrowseButton.Size = new System.Drawing.Size(28, 23);
            this.ClientINIBrowseButton.TabIndex = 31;
            this.ClientINIBrowseButton.Text = "...";
            this.ClientINIBrowseButton.UseVisualStyleBackColor = true;
            this.ClientINIBrowseButton.Click += new System.EventHandler(this.ClientINIBrowseButton_Click);
            // 
            // ServerINIBrowseButton
            // 
            this.ServerINIBrowseButton.Location = new System.Drawing.Point(422, 107);
            this.ServerINIBrowseButton.Name = "ServerINIBrowseButton";
            this.ServerINIBrowseButton.Size = new System.Drawing.Size(28, 23);
            this.ServerINIBrowseButton.TabIndex = 30;
            this.ServerINIBrowseButton.Text = "...";
            this.ServerINIBrowseButton.UseVisualStyleBackColor = true;
            this.ServerINIBrowseButton.Click += new System.EventHandler(this.ServerINIBrowseButton_Click);
            // 
            // AppWorkingFolderBrowseButton
            // 
            this.AppWorkingFolderBrowseButton.Location = new System.Drawing.Point(422, 77);
            this.AppWorkingFolderBrowseButton.Name = "AppWorkingFolderBrowseButton";
            this.AppWorkingFolderBrowseButton.Size = new System.Drawing.Size(28, 23);
            this.AppWorkingFolderBrowseButton.TabIndex = 29;
            this.AppWorkingFolderBrowseButton.Text = "...";
            this.AppWorkingFolderBrowseButton.UseVisualStyleBackColor = true;
            this.AppWorkingFolderBrowseButton.Click += new System.EventHandler(this.AppWorkingFolderBrowseButton_Click);
            // 
            // appPathBrowseButton
            // 
            this.appPathBrowseButton.Location = new System.Drawing.Point(422, 47);
            this.appPathBrowseButton.Name = "appPathBrowseButton";
            this.appPathBrowseButton.Size = new System.Drawing.Size(28, 23);
            this.appPathBrowseButton.TabIndex = 28;
            this.appPathBrowseButton.Text = "...";
            this.appPathBrowseButton.UseVisualStyleBackColor = true;
            this.appPathBrowseButton.Click += new System.EventHandler(this.AppPathBrowseButton_Click);
            // 
            // ICAEncryptionLevelCombobox
            // 
            this.ICAEncryptionLevelCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ICAEncryptionLevelCombobox.Enabled = false;
            this.ICAEncryptionLevelCombobox.FormattingEnabled = true;
            this.ICAEncryptionLevelCombobox.Items.AddRange(new object[] {
            "Encrypt / Basic (default)",
            "EncRC5-0 / RC5 128 bit-logon only",
            "EncRC5-40 / RC5 40 bit",
            "EncRC5-56 / RC5 56 bit",
            "EncRC5-128 / RC5 128 bit"});
            this.ICAEncryptionLevelCombobox.Location = new System.Drawing.Point(190, 167);
            this.ICAEncryptionLevelCombobox.Name = "ICAEncryptionLevelCombobox";
            this.ICAEncryptionLevelCombobox.Size = new System.Drawing.Size(226, 21);
            this.ICAEncryptionLevelCombobox.TabIndex = 27;
            // 
            // ICAEnableEncryptionCheckbox
            // 
            this.ICAEnableEncryptionCheckbox.AutoSize = true;
            this.ICAEnableEncryptionCheckbox.Location = new System.Drawing.Point(9, 169);
            this.ICAEnableEncryptionCheckbox.Name = "ICAEnableEncryptionCheckbox";
            this.ICAEnableEncryptionCheckbox.Size = new System.Drawing.Size(112, 17);
            this.ICAEnableEncryptionCheckbox.TabIndex = 26;
            this.ICAEnableEncryptionCheckbox.Text = "Enable Encryption";
            this.ICAEnableEncryptionCheckbox.UseVisualStyleBackColor = true;
            this.ICAEnableEncryptionCheckbox.CheckedChanged += new System.EventHandler(this.ICAEnableEncryptionCheckbox_CheckedChanged);
            // 
            // ICAClientINI
            // 
            this.ICAClientINI.Location = new System.Drawing.Point(190, 137);
            this.ICAClientINI.Name = "ICAClientINI";
            this.ICAClientINI.Size = new System.Drawing.Size(226, 20);
            this.ICAClientINI.TabIndex = 25;
            // 
            // ICAServerINI
            // 
            this.ICAServerINI.Location = new System.Drawing.Point(190, 107);
            this.ICAServerINI.Name = "ICAServerINI";
            this.ICAServerINI.Size = new System.Drawing.Size(226, 20);
            this.ICAServerINI.TabIndex = 23;
            // 
            // ICAWorkingFolder
            // 
            this.ICAWorkingFolder.Location = new System.Drawing.Point(190, 77);
            this.ICAWorkingFolder.Name = "ICAWorkingFolder";
            this.ICAWorkingFolder.Size = new System.Drawing.Size(226, 20);
            this.ICAWorkingFolder.TabIndex = 21;
            // 
            // ICAApplicationPath
            // 
            this.ICAApplicationPath.Location = new System.Drawing.Point(190, 47);
            this.ICAApplicationPath.Name = "ICAApplicationPath";
            this.ICAApplicationPath.Size = new System.Drawing.Size(226, 20);
            this.ICAApplicationPath.TabIndex = 19;
            this.ICAApplicationPath.Text = ".";
            // 
            // ICAApplicationNameTextBox
            // 
            this.ICAApplicationNameTextBox.Location = new System.Drawing.Point(190, 17);
            this.ICAApplicationNameTextBox.Name = "ICAApplicationNameTextBox";
            this.ICAApplicationNameTextBox.Size = new System.Drawing.Size(226, 20);
            this.ICAApplicationNameTextBox.TabIndex = 17;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 140);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(53, 13);
            this.label35.TabIndex = 24;
            this.label35.Text = "Client INI:";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(6, 110);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(58, 13);
            this.label34.TabIndex = 22;
            this.label34.Text = "Server INI:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 80);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(137, 13);
            this.label23.TabIndex = 20;
            this.label23.Text = "Application Working Folder:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 50);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(87, 13);
            this.label22.TabIndex = 18;
            this.label22.Text = "Application Path:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 20);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(38, 13);
            this.label21.TabIndex = 16;
            this.label21.Text = "Name:";
            // 
            // CitrixControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.IcaGroupBox);
            this.Name = "CitrixControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.IcaGroupBox.ResumeLayout(false);
            this.IcaGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox IcaGroupBox;
        private System.Windows.Forms.Button ClientINIBrowseButton;
        private System.Windows.Forms.Button ServerINIBrowseButton;
        private System.Windows.Forms.Button AppWorkingFolderBrowseButton;
        private System.Windows.Forms.Button appPathBrowseButton;
        private System.Windows.Forms.ComboBox ICAEncryptionLevelCombobox;
        private System.Windows.Forms.CheckBox ICAEnableEncryptionCheckbox;
        private System.Windows.Forms.TextBox ICAClientINI;
        private System.Windows.Forms.TextBox ICAServerINI;
        private System.Windows.Forms.TextBox ICAWorkingFolder;
        private System.Windows.Forms.TextBox ICAApplicationPath;
        private System.Windows.Forms.TextBox ICAApplicationNameTextBox;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
    }
}
