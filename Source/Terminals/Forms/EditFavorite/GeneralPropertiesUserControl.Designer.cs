using Terminals.Forms.Controls;

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
            this.chkAddtoToolbar = new System.Windows.Forms.CheckBox();
            this.NewWindowCheckbox = new System.Windows.Forms.CheckBox();
            this.httpUrlTextBox = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkSavePassword = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.ProtocolComboBox = new System.Windows.Forms.ComboBox();
            this.ProtocolLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbServers = new System.Windows.Forms.ComboBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.securityPanel1 = new SecurityPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // chkAddtoToolbar
            // 
            this.chkAddtoToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAddtoToolbar.AutoSize = true;
            this.chkAddtoToolbar.Location = new System.Drawing.Point(136, 272);
            this.chkAddtoToolbar.Name = "chkAddtoToolbar";
            this.chkAddtoToolbar.Size = new System.Drawing.Size(96, 17);
            this.chkAddtoToolbar.TabIndex = 39;
            this.chkAddtoToolbar.Text = "Add to &Toolbar";
            this.chkAddtoToolbar.UseVisualStyleBackColor = true;
            // 
            // NewWindowCheckbox
            // 
            this.NewWindowCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.NewWindowCheckbox.AutoSize = true;
            this.NewWindowCheckbox.Location = new System.Drawing.Point(136, 252);
            this.NewWindowCheckbox.Name = "NewWindowCheckbox";
            this.NewWindowCheckbox.Size = new System.Drawing.Size(130, 17);
            this.NewWindowCheckbox.TabIndex = 40;
            this.NewWindowCheckbox.Text = "&Open in New Window";
            this.NewWindowCheckbox.UseVisualStyleBackColor = true;
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
            this.txtName.Size = new System.Drawing.Size(325, 20);
            this.txtName.TabIndex = 28;
            // 
            // chkSavePassword
            // 
            this.chkSavePassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSavePassword.AutoSize = true;
            this.chkSavePassword.Location = new System.Drawing.Point(136, 232);
            this.chkSavePassword.Name = "chkSavePassword";
            this.chkSavePassword.Size = new System.Drawing.Size(99, 17);
            this.chkSavePassword.TabIndex = 6;
            this.chkSavePassword.Text = "S&ave password";
            this.chkSavePassword.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Terminals.Properties.Resources.terminalsicon;
            this.pictureBox2.InitialImage = global::Terminals.Properties.Resources.smallterm;
            this.pictureBox2.Location = new System.Drawing.Point(465, 79);
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
            // securityPanel1
            // 
            this.securityPanel1.Location = new System.Drawing.Point(6, 101);
            this.securityPanel1.Name = "securityPanel1";
            this.securityPanel1.Size = new System.Drawing.Size(483, 128);
            this.securityPanel1.TabIndex = 41;
            // 
            // GeneralPropertiesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.securityPanel1);
            this.Controls.Add(this.chkSavePassword);
            this.Controls.Add(this.chkAddtoToolbar);
            this.Controls.Add(this.NewWindowCheckbox);
            this.Controls.Add(this.httpUrlTextBox);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.ProtocolComboBox);
            this.Controls.Add(this.ProtocolLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbServers);
            this.Controls.Add(this.lblServerName);
            this.Name = "GeneralPropertiesUserControl";
            this.Size = new System.Drawing.Size(590, 365);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox httpUrlTextBox;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkSavePassword;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox ProtocolComboBox;
        private System.Windows.Forms.Label ProtocolLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbServers;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.CheckBox chkAddtoToolbar;
        private System.Windows.Forms.CheckBox NewWindowCheckbox;
        private Controls.SecurityPanel securityPanel1;
    }
}
