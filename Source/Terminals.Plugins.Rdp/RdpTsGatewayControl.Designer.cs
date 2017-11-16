using Terminals.Forms.Controls;

namespace Terminals.Forms.EditFavorite
{
    partial class RdpTsGatewayControl
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
            Terminals.TerminalServices.GatewayCredentialsSources gatewayCredentialsSources4 = new Terminals.TerminalServices.GatewayCredentialsSources();
            Terminals.TerminalServices.GatewayCredentialsSources gatewayCredentialsSources5 = new Terminals.TerminalServices.GatewayCredentialsSources();
            Terminals.TerminalServices.GatewayCredentialsSources gatewayCredentialsSources6 = new Terminals.TerminalServices.GatewayCredentialsSources();
            this.TerminalGwLoginSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.securityPanel1 = new Terminals.Forms.Controls.SecurityPanel();
            this.credentialsPanel1 = new Terminals.Forms.Controls.CredentialsPanel();
            this.chkTSGWlogin = new System.Windows.Forms.CheckBox();
            this.TerminalGwSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.radTSGWenable = new System.Windows.Forms.RadioButton();
            this.radTSGWdisable = new System.Windows.Forms.RadioButton();
            this.pnlTSGWsettings = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.cmbTSGWLogonMethod = new System.Windows.Forms.ComboBox();
            this.chkTSGWlocalBypass = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtTSGWServer = new System.Windows.Forms.TextBox();
            this.TerminalGwLoginSettingsGroupBox.SuspendLayout();
            this.TerminalGwSettingsGroupBox.SuspendLayout();
            this.pnlTSGWsettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // TerminalGwLoginSettingsGroupBox
            // 
            this.TerminalGwLoginSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TerminalGwLoginSettingsGroupBox.Controls.Add(this.securityPanel1);
            this.TerminalGwLoginSettingsGroupBox.Controls.Add(this.credentialsPanel1);
            this.TerminalGwLoginSettingsGroupBox.Controls.Add(this.chkTSGWlogin);
            this.TerminalGwLoginSettingsGroupBox.Location = new System.Drawing.Point(8, 346);
            this.TerminalGwLoginSettingsGroupBox.Margin = new System.Windows.Forms.Padding(6);
            this.TerminalGwLoginSettingsGroupBox.Name = "TerminalGwLoginSettingsGroupBox";
            this.TerminalGwLoginSettingsGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.TerminalGwLoginSettingsGroupBox.Size = new System.Drawing.Size(1140, 279);
            this.TerminalGwLoginSettingsGroupBox.TabIndex = 3;
            this.TerminalGwLoginSettingsGroupBox.TabStop = false;
            this.TerminalGwLoginSettingsGroupBox.Text = "Login Settings";
            // 
            // securityPanel1
            // 
            this.securityPanel1.Location = new System.Drawing.Point(425, 44);
            this.securityPanel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.securityPanel1.Name = "securityPanel1";
            this.securityPanel1.Size = new System.Drawing.Size(538, 223);
            this.securityPanel1.TabIndex = 2;
            // 
            // credentialsPanel1
            // 
            this.credentialsPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.credentialsPanel1.Location = new System.Drawing.Point(12, 88);
            this.credentialsPanel1.Margin = new System.Windows.Forms.Padding(12);
            this.credentialsPanel1.Name = "credentialsPanel1";
            this.credentialsPanel1.Settings = null;
            this.credentialsPanel1.Size = new System.Drawing.Size(347, 181);
            this.credentialsPanel1.TabIndex = 1;
            this.credentialsPanel1.TextEditsLeft = 6;
            // 
            // chkTSGWlogin
            // 
            this.chkTSGWlogin.AutoSize = true;
            this.chkTSGWlogin.Location = new System.Drawing.Point(10, 44);
            this.chkTSGWlogin.Margin = new System.Windows.Forms.Padding(6);
            this.chkTSGWlogin.Name = "chkTSGWlogin";
            this.chkTSGWlogin.Size = new System.Drawing.Size(349, 29);
            this.chkTSGWlogin.TabIndex = 0;
            this.chkTSGWlogin.Text = "Use Separate Login Credentials";
            this.chkTSGWlogin.UseVisualStyleBackColor = true;
            this.chkTSGWlogin.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // TerminalGwSettingsGroupBox
            // 
            this.TerminalGwSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TerminalGwSettingsGroupBox.Controls.Add(this.radTSGWenable);
            this.TerminalGwSettingsGroupBox.Controls.Add(this.radTSGWdisable);
            this.TerminalGwSettingsGroupBox.Controls.Add(this.pnlTSGWsettings);
            this.TerminalGwSettingsGroupBox.Location = new System.Drawing.Point(6, 6);
            this.TerminalGwSettingsGroupBox.Margin = new System.Windows.Forms.Padding(6);
            this.TerminalGwSettingsGroupBox.Name = "TerminalGwSettingsGroupBox";
            this.TerminalGwSettingsGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.TerminalGwSettingsGroupBox.Size = new System.Drawing.Size(1140, 335);
            this.TerminalGwSettingsGroupBox.TabIndex = 2;
            this.TerminalGwSettingsGroupBox.TabStop = false;
            this.TerminalGwSettingsGroupBox.Text = "Terminal Server Gateway Settings";
            // 
            // radTSGWenable
            // 
            this.radTSGWenable.AutoSize = true;
            this.radTSGWenable.Location = new System.Drawing.Point(12, 83);
            this.radTSGWenable.Margin = new System.Windows.Forms.Padding(6);
            this.radTSGWenable.Name = "radTSGWenable";
            this.radTSGWenable.Size = new System.Drawing.Size(417, 29);
            this.radTSGWenable.TabIndex = 2;
            this.radTSGWenable.Text = "Use the following TS Gateway settings:";
            this.radTSGWenable.UseVisualStyleBackColor = true;
            this.radTSGWenable.CheckedChanged += new System.EventHandler(this.RadTsgWenable_CheckedChanged);
            // 
            // radTSGWdisable
            // 
            this.radTSGWdisable.AutoSize = true;
            this.radTSGWdisable.Checked = true;
            this.radTSGWdisable.Location = new System.Drawing.Point(12, 44);
            this.radTSGWdisable.Margin = new System.Windows.Forms.Padding(6);
            this.radTSGWdisable.Name = "radTSGWdisable";
            this.radTSGWdisable.Size = new System.Drawing.Size(270, 29);
            this.radTSGWdisable.TabIndex = 1;
            this.radTSGWdisable.TabStop = true;
            this.radTSGWdisable.Text = "Do not use TS Gateway";
            this.radTSGWdisable.UseVisualStyleBackColor = true;
            // 
            // pnlTSGWsettings
            // 
            this.pnlTSGWsettings.Controls.Add(this.label20);
            this.pnlTSGWsettings.Controls.Add(this.cmbTSGWLogonMethod);
            this.pnlTSGWsettings.Controls.Add(this.chkTSGWlocalBypass);
            this.pnlTSGWsettings.Controls.Add(this.label17);
            this.pnlTSGWsettings.Controls.Add(this.txtTSGWServer);
            this.pnlTSGWsettings.Enabled = false;
            this.pnlTSGWsettings.Location = new System.Drawing.Point(12, 127);
            this.pnlTSGWsettings.Margin = new System.Windows.Forms.Padding(6);
            this.pnlTSGWsettings.Name = "pnlTSGWsettings";
            this.pnlTSGWsettings.Size = new System.Drawing.Size(880, 175);
            this.pnlTSGWsettings.TabIndex = 8;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(40, 71);
            this.label20.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(156, 25);
            this.label20.TabIndex = 10;
            this.label20.Text = "Logon Method:";
            // 
            // cmbTSGWLogonMethod
            // 
            this.cmbTSGWLogonMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTSGWLogonMethod.FormattingEnabled = true;
            gatewayCredentialsSources4.DisplayName = "Ask for Password (NTLM)";
            gatewayCredentialsSources4.ID = 0;
            gatewayCredentialsSources5.DisplayName = "Smart Card";
            gatewayCredentialsSources5.ID = 1;
            gatewayCredentialsSources6.DisplayName = "Allow user to select later";
            gatewayCredentialsSources6.ID = 4;
            this.cmbTSGWLogonMethod.Items.AddRange(new object[] {
            gatewayCredentialsSources4,
            gatewayCredentialsSources5,
            gatewayCredentialsSources6});
            this.cmbTSGWLogonMethod.Location = new System.Drawing.Point(256, 65);
            this.cmbTSGWLogonMethod.Margin = new System.Windows.Forms.Padding(6);
            this.cmbTSGWLogonMethod.Name = "cmbTSGWLogonMethod";
            this.cmbTSGWLogonMethod.Size = new System.Drawing.Size(444, 33);
            this.cmbTSGWLogonMethod.TabIndex = 9;
            // 
            // chkTSGWlocalBypass
            // 
            this.chkTSGWlocalBypass.AutoSize = true;
            this.chkTSGWlocalBypass.Location = new System.Drawing.Point(46, 125);
            this.chkTSGWlocalBypass.Margin = new System.Windows.Forms.Padding(6);
            this.chkTSGWlocalBypass.Name = "chkTSGWlocalBypass";
            this.chkTSGWlocalBypass.Size = new System.Drawing.Size(426, 29);
            this.chkTSGWlocalBypass.TabIndex = 8;
            this.chkTSGWlocalBypass.Text = "Bypass TS Gateway for local addresses";
            this.chkTSGWlocalBypass.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(40, 13);
            this.label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(143, 25);
            this.label17.TabIndex = 5;
            this.label17.Text = "Server Name:";
            // 
            // txtTSGWServer
            // 
            this.txtTSGWServer.Location = new System.Drawing.Point(256, 8);
            this.txtTSGWServer.Margin = new System.Windows.Forms.Padding(6);
            this.txtTSGWServer.Name = "txtTSGWServer";
            this.txtTSGWServer.Size = new System.Drawing.Size(444, 31);
            this.txtTSGWServer.TabIndex = 3;
            // 
            // RdpTsGatewayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TerminalGwLoginSettingsGroupBox);
            this.Controls.Add(this.TerminalGwSettingsGroupBox);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "RdpTsGatewayControl";
            this.Size = new System.Drawing.Size(1180, 702);
            this.TerminalGwLoginSettingsGroupBox.ResumeLayout(false);
            this.TerminalGwLoginSettingsGroupBox.PerformLayout();
            this.TerminalGwSettingsGroupBox.ResumeLayout(false);
            this.TerminalGwSettingsGroupBox.PerformLayout();
            this.pnlTSGWsettings.ResumeLayout(false);
            this.pnlTSGWsettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox TerminalGwLoginSettingsGroupBox;
        private System.Windows.Forms.CheckBox chkTSGWlogin;
        private System.Windows.Forms.GroupBox TerminalGwSettingsGroupBox;
        private System.Windows.Forms.RadioButton radTSGWenable;
        private System.Windows.Forms.RadioButton radTSGWdisable;
        private System.Windows.Forms.Panel pnlTSGWsettings;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cmbTSGWLogonMethod;
        private System.Windows.Forms.CheckBox chkTSGWlocalBypass;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtTSGWServer;
        private Controls.CredentialsPanel credentialsPanel1;
        private SecurityPanel securityPanel1;
    }
}
