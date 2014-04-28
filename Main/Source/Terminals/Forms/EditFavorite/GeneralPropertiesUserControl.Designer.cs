namespace Terminals.Forms.EditFavorite
{
    partial class GeneralPropertiesUserControl
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
            this.GeneralGroupBox = new System.Windows.Forms.GroupBox();
            this.httpUrlTextBox = new System.Windows.Forms.TextBox();
            this.NotesTextbox = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.CredentialDropdown = new System.Windows.Forms.ComboBox();
            this.CredentialManagerPicturebox = new System.Windows.Forms.PictureBox();
            this.CredentialsPanel = new System.Windows.Forms.Panel();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDomains = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkSavePassword = new System.Windows.Forms.CheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.ProtocolComboBox = new System.Windows.Forms.ComboBox();
            this.ProtocolLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbServers = new System.Windows.Forms.ComboBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.GeneralGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CredentialManagerPicturebox)).BeginInit();
            this.CredentialsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // GeneralGroupBox
            // 
            this.GeneralGroupBox.Controls.Add(this.httpUrlTextBox);
            this.GeneralGroupBox.Controls.Add(this.NotesTextbox);
            this.GeneralGroupBox.Controls.Add(this.txtPort);
            this.GeneralGroupBox.Controls.Add(this.txtName);
            this.GeneralGroupBox.Controls.Add(this.label15);
            this.GeneralGroupBox.Controls.Add(this.CredentialDropdown);
            this.GeneralGroupBox.Controls.Add(this.CredentialManagerPicturebox);
            this.GeneralGroupBox.Controls.Add(this.CredentialsPanel);
            this.GeneralGroupBox.Controls.Add(this.label36);
            this.GeneralGroupBox.Controls.Add(this.pictureBox2);
            this.GeneralGroupBox.Controls.Add(this.lblPort);
            this.GeneralGroupBox.Controls.Add(this.ProtocolComboBox);
            this.GeneralGroupBox.Controls.Add(this.ProtocolLabel);
            this.GeneralGroupBox.Controls.Add(this.label5);
            this.GeneralGroupBox.Controls.Add(this.cmbServers);
            this.GeneralGroupBox.Controls.Add(this.lblServerName);
            this.GeneralGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GeneralGroupBox.Location = new System.Drawing.Point(0, 0);
            this.GeneralGroupBox.Name = "GeneralGroupBox";
            this.GeneralGroupBox.Size = new System.Drawing.Size(590, 365);
            this.GeneralGroupBox.TabIndex = 1;
            this.GeneralGroupBox.TabStop = false;
            // 
            // httpUrlTextBox
            // 
            this.httpUrlTextBox.Location = new System.Drawing.Point(133, 45);
            this.httpUrlTextBox.Name = "httpUrlTextBox";
            this.httpUrlTextBox.Size = new System.Drawing.Size(259, 20);
            this.httpUrlTextBox.TabIndex = 38;
            this.httpUrlTextBox.Text = "http://terminals.codeplex.com";
            this.httpUrlTextBox.Visible = false;
            this.httpUrlTextBox.TextChanged += new System.EventHandler(this.HttpUrlTextBox_TextChanged);
            // 
            // NotesTextbox
            // 
            this.NotesTextbox.Location = new System.Drawing.Point(133, 256);
            this.NotesTextbox.Multiline = true;
            this.NotesTextbox.Name = "NotesTextbox";
            this.NotesTextbox.Size = new System.Drawing.Size(356, 98);
            this.NotesTextbox.TabIndex = 33;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(443, 43);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(46, 20);
            this.txtPort.TabIndex = 26;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(133, 75);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(334, 20);
            this.txtName.TabIndex = 28;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 108);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(57, 13);
            this.label15.TabIndex = 29;
            this.label15.Text = "Credential:";
            // 
            // CredentialDropdown
            // 
            this.CredentialDropdown.DisplayMember = "Name";
            this.CredentialDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CredentialDropdown.FormattingEnabled = true;
            this.CredentialDropdown.Location = new System.Drawing.Point(133, 105);
            this.CredentialDropdown.Name = "CredentialDropdown";
            this.CredentialDropdown.Size = new System.Drawing.Size(334, 21);
            this.CredentialDropdown.TabIndex = 30;
            this.CredentialDropdown.SelectedIndexChanged += new System.EventHandler(this.CredentialDropdown_SelectedIndexChanged);
            // 
            // CredentialManagerPicturebox
            // 
            this.CredentialManagerPicturebox.Image = global::Terminals.Properties.Resources.computer_security;
            this.CredentialManagerPicturebox.Location = new System.Drawing.Point(473, 109);
            this.CredentialManagerPicturebox.Name = "CredentialManagerPicturebox";
            this.CredentialManagerPicturebox.Size = new System.Drawing.Size(16, 16);
            this.CredentialManagerPicturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CredentialManagerPicturebox.TabIndex = 37;
            this.CredentialManagerPicturebox.TabStop = false;
            this.CredentialManagerPicturebox.Click += new System.EventHandler(this.CredentialManagerPicturebox_Click);
            // 
            // CredentialsPanel
            // 
            this.CredentialsPanel.Controls.Add(this.cmbUsers);
            this.CredentialsPanel.Controls.Add(this.label1);
            this.CredentialsPanel.Controls.Add(this.cmbDomains);
            this.CredentialsPanel.Controls.Add(this.label3);
            this.CredentialsPanel.Controls.Add(this.label4);
            this.CredentialsPanel.Controls.Add(this.txtPassword);
            this.CredentialsPanel.Controls.Add(this.chkSavePassword);
            this.CredentialsPanel.Location = new System.Drawing.Point(0, 130);
            this.CredentialsPanel.Name = "CredentialsPanel";
            this.CredentialsPanel.Size = new System.Drawing.Size(489, 123);
            this.CredentialsPanel.TabIndex = 31;
            // 
            // cmbUsers
            // 
            this.cmbUsers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUsers.Location = new System.Drawing.Point(133, 37);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(334, 21);
            this.cmbUsers.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Domain:";
            // 
            // cmbDomains
            // 
            this.cmbDomains.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDomains.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDomains.Location = new System.Drawing.Point(133, 6);
            this.cmbDomains.Name = "cmbDomains";
            this.cmbDomains.Size = new System.Drawing.Size(334, 21);
            this.cmbDomains.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "&User name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "&Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(133, 69);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(334, 20);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // chkSavePassword
            // 
            this.chkSavePassword.AutoSize = true;
            this.chkSavePassword.Location = new System.Drawing.Point(133, 99);
            this.chkSavePassword.Name = "chkSavePassword";
            this.chkSavePassword.Size = new System.Drawing.Size(99, 17);
            this.chkSavePassword.TabIndex = 6;
            this.chkSavePassword.Text = "S&ave password";
            this.chkSavePassword.UseVisualStyleBackColor = true;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(6, 259);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(38, 13);
            this.label36.TabIndex = 32;
            this.label36.Text = "Notes:";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Terminals.Properties.Resources.terminalsicon;
            this.pictureBox2.InitialImage = global::Terminals.Properties.Resources.smallterm;
            this.pictureBox2.Location = new System.Drawing.Point(473, 79);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 36;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.PictureBox2_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(398, 46);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 25;
            this.lblPort.Text = "Port:";
            // 
            // ProtocolComboBox
            // 
            this.ProtocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProtocolComboBox.FormattingEnabled = true;
            this.ProtocolComboBox.Items.AddRange(new object[] {
            "RDP",
            "VNC",
            "VMRC",
            "SSH",
            "Telnet",
            "RAS",
            "ICA Citrix",
            "HTTP",
            "HTTPS"});
            this.ProtocolComboBox.Location = new System.Drawing.Point(133, 14);
            this.ProtocolComboBox.MaxDropDownItems = 10;
            this.ProtocolComboBox.Name = "ProtocolComboBox";
            this.ProtocolComboBox.Size = new System.Drawing.Size(356, 21);
            this.ProtocolComboBox.TabIndex = 35;
            this.ProtocolComboBox.SelectedIndexChanged += new System.EventHandler(this.ProtocolComboBox_SelectedIndexChanged);
            // 
            // ProtocolLabel
            // 
            this.ProtocolLabel.AutoSize = true;
            this.ProtocolLabel.Location = new System.Drawing.Point(6, 17);
            this.ProtocolLabel.Name = "ProtocolLabel";
            this.ProtocolLabel.Size = new System.Drawing.Size(49, 13);
            this.ProtocolLabel.TabIndex = 34;
            this.ProtocolLabel.Text = "&Protocol:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Connection na&me:";
            // 
            // cmbServers
            // 
            this.cmbServers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbServers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbServers.Location = new System.Drawing.Point(133, 45);
            this.cmbServers.Name = "cmbServers";
            this.cmbServers.Size = new System.Drawing.Size(259, 21);
            this.cmbServers.TabIndex = 24;
            this.cmbServers.SelectedIndexChanged += new System.EventHandler(this.CmbServers_SelectedIndexChanged);
            this.cmbServers.TextChanged += new System.EventHandler(this.CmbServers_TextChanged);
            this.cmbServers.Leave += new System.EventHandler(this.CmbServers_Leave);
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(6, 46);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(55, 13);
            this.lblServerName.TabIndex = 23;
            this.lblServerName.Text = "&Computer:";
            // 
            // GeneralPropertiesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GeneralGroupBox);
            this.Name = "GeneralPropertiesUserControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.GeneralGroupBox.ResumeLayout(false);
            this.GeneralGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CredentialManagerPicturebox)).EndInit();
            this.CredentialsPanel.ResumeLayout(false);
            this.CredentialsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GeneralGroupBox;
        private System.Windows.Forms.TextBox httpUrlTextBox;
        private System.Windows.Forms.TextBox NotesTextbox;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox CredentialDropdown;
        private System.Windows.Forms.PictureBox CredentialManagerPicturebox;
        private System.Windows.Forms.Panel CredentialsPanel;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDomains;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkSavePassword;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox ProtocolComboBox;
        private System.Windows.Forms.Label ProtocolLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbServers;
        private System.Windows.Forms.Label lblServerName;
    }
}
