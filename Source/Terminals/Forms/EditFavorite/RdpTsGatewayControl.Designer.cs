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
            Terminals.TerminalServices.GatewayCredentialsSources gatewayCredentialsSources7 = new Terminals.TerminalServices.GatewayCredentialsSources();
            Terminals.TerminalServices.GatewayCredentialsSources gatewayCredentialsSources8 = new Terminals.TerminalServices.GatewayCredentialsSources();
            Terminals.TerminalServices.GatewayCredentialsSources gatewayCredentialsSources9 = new Terminals.TerminalServices.GatewayCredentialsSources();
            this.TerminalGwLoginSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.pnlTSGWlogon = new System.Windows.Forms.Panel();
            this.txtTSGWDomain = new System.Windows.Forms.TextBox();
            this.txtTSGWPassword = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtTSGWUserName = new System.Windows.Forms.TextBox();
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
            this.pnlTSGWlogon.SuspendLayout();
            this.TerminalGwSettingsGroupBox.SuspendLayout();
            this.pnlTSGWsettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // TerminalGwLoginSettingsGroupBox
            // 
            this.TerminalGwLoginSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TerminalGwLoginSettingsGroupBox.Controls.Add(this.pnlTSGWlogon);
            this.TerminalGwLoginSettingsGroupBox.Controls.Add(this.chkTSGWlogin);
            this.TerminalGwLoginSettingsGroupBox.Location = new System.Drawing.Point(4, 180);
            this.TerminalGwLoginSettingsGroupBox.Name = "TerminalGwLoginSettingsGroupBox";
            this.TerminalGwLoginSettingsGroupBox.Size = new System.Drawing.Size(570, 145);
            this.TerminalGwLoginSettingsGroupBox.TabIndex = 3;
            this.TerminalGwLoginSettingsGroupBox.TabStop = false;
            this.TerminalGwLoginSettingsGroupBox.Text = "Login Settings";
            // 
            // pnlTSGWlogon
            // 
            this.pnlTSGWlogon.Controls.Add(this.txtTSGWDomain);
            this.pnlTSGWlogon.Controls.Add(this.txtTSGWPassword);
            this.pnlTSGWlogon.Controls.Add(this.label18);
            this.pnlTSGWlogon.Controls.Add(this.label19);
            this.pnlTSGWlogon.Controls.Add(this.label16);
            this.pnlTSGWlogon.Controls.Add(this.txtTSGWUserName);
            this.pnlTSGWlogon.Enabled = false;
            this.pnlTSGWlogon.Location = new System.Drawing.Point(3, 43);
            this.pnlTSGWlogon.Name = "pnlTSGWlogon";
            this.pnlTSGWlogon.Size = new System.Drawing.Size(442, 92);
            this.pnlTSGWlogon.TabIndex = 1;
            // 
            // txtTSGWDomain
            // 
            this.txtTSGWDomain.Location = new System.Drawing.Point(102, 63);
            this.txtTSGWDomain.Name = "txtTSGWDomain";
            this.txtTSGWDomain.Size = new System.Drawing.Size(224, 20);
            this.txtTSGWDomain.TabIndex = 5;
            // 
            // txtTSGWPassword
            // 
            this.txtTSGWPassword.Location = new System.Drawing.Point(102, 33);
            this.txtTSGWPassword.Name = "txtTSGWPassword";
            this.txtTSGWPassword.PasswordChar = '*';
            this.txtTSGWPassword.Size = new System.Drawing.Size(224, 20);
            this.txtTSGWPassword.TabIndex = 4;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(22, 66);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(46, 13);
            this.label18.TabIndex = 3;
            this.label18.Text = "Domain:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(22, 36);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(56, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Password:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(22, 6);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "Username:";
            // 
            // txtTSGWUserName
            // 
            this.txtTSGWUserName.Location = new System.Drawing.Point(102, 3);
            this.txtTSGWUserName.Name = "txtTSGWUserName";
            this.txtTSGWUserName.Size = new System.Drawing.Size(224, 20);
            this.txtTSGWUserName.TabIndex = 0;
            // 
            // chkTSGWlogin
            // 
            this.chkTSGWlogin.AutoSize = true;
            this.chkTSGWlogin.Location = new System.Drawing.Point(5, 23);
            this.chkTSGWlogin.Name = "chkTSGWlogin";
            this.chkTSGWlogin.Size = new System.Drawing.Size(175, 17);
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
            this.TerminalGwSettingsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.TerminalGwSettingsGroupBox.Name = "TerminalGwSettingsGroupBox";
            this.TerminalGwSettingsGroupBox.Size = new System.Drawing.Size(570, 174);
            this.TerminalGwSettingsGroupBox.TabIndex = 2;
            this.TerminalGwSettingsGroupBox.TabStop = false;
            this.TerminalGwSettingsGroupBox.Text = "Terminal Server Gateway Settings";
            // 
            // radTSGWenable
            // 
            this.radTSGWenable.AutoSize = true;
            this.radTSGWenable.Location = new System.Drawing.Point(6, 43);
            this.radTSGWenable.Name = "radTSGWenable";
            this.radTSGWenable.Size = new System.Drawing.Size(210, 17);
            this.radTSGWenable.TabIndex = 2;
            this.radTSGWenable.Text = "Use the following TS Gateway settings:";
            this.radTSGWenable.UseVisualStyleBackColor = true;
            this.radTSGWenable.CheckedChanged += new System.EventHandler(this.RadTsgWenable_CheckedChanged);
            // 
            // radTSGWdisable
            // 
            this.radTSGWdisable.AutoSize = true;
            this.radTSGWdisable.Checked = true;
            this.radTSGWdisable.Location = new System.Drawing.Point(6, 23);
            this.radTSGWdisable.Name = "radTSGWdisable";
            this.radTSGWdisable.Size = new System.Drawing.Size(139, 17);
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
            this.pnlTSGWsettings.Location = new System.Drawing.Point(6, 66);
            this.pnlTSGWsettings.Name = "pnlTSGWsettings";
            this.pnlTSGWsettings.Size = new System.Drawing.Size(440, 91);
            this.pnlTSGWsettings.TabIndex = 8;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(20, 37);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(79, 13);
            this.label20.TabIndex = 10;
            this.label20.Text = "Logon Method:";
            // 
            // cmbTSGWLogonMethod
            // 
            this.cmbTSGWLogonMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTSGWLogonMethod.FormattingEnabled = true;
            gatewayCredentialsSources7.DisplayName = "Ask for Password (NTLM)";
            gatewayCredentialsSources7.ID = 0;
            gatewayCredentialsSources8.DisplayName = "Smart Card";
            gatewayCredentialsSources8.ID = 1;
            gatewayCredentialsSources9.DisplayName = "Allow user to select later";
            gatewayCredentialsSources9.ID = 4;
            this.cmbTSGWLogonMethod.Items.AddRange(new object[] {
            gatewayCredentialsSources7,
            gatewayCredentialsSources8,
            gatewayCredentialsSources9});
            this.cmbTSGWLogonMethod.Location = new System.Drawing.Point(128, 34);
            this.cmbTSGWLogonMethod.Name = "cmbTSGWLogonMethod";
            this.cmbTSGWLogonMethod.Size = new System.Drawing.Size(224, 21);
            this.cmbTSGWLogonMethod.TabIndex = 9;
            // 
            // chkTSGWlocalBypass
            // 
            this.chkTSGWlocalBypass.AutoSize = true;
            this.chkTSGWlocalBypass.Location = new System.Drawing.Point(23, 65);
            this.chkTSGWlocalBypass.Name = "chkTSGWlocalBypass";
            this.chkTSGWlocalBypass.Size = new System.Drawing.Size(213, 17);
            this.chkTSGWlocalBypass.TabIndex = 8;
            this.chkTSGWlocalBypass.Text = "Bypass TS Gateway for local addresses";
            this.chkTSGWlocalBypass.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(20, 7);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(72, 13);
            this.label17.TabIndex = 5;
            this.label17.Text = "Server Name:";
            // 
            // txtTSGWServer
            // 
            this.txtTSGWServer.Location = new System.Drawing.Point(128, 4);
            this.txtTSGWServer.Name = "txtTSGWServer";
            this.txtTSGWServer.Size = new System.Drawing.Size(224, 20);
            this.txtTSGWServer.TabIndex = 3;
            // 
            // RdpTsGatewayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TerminalGwLoginSettingsGroupBox);
            this.Controls.Add(this.TerminalGwSettingsGroupBox);
            this.Name = "RdpTsGatewayControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.TerminalGwLoginSettingsGroupBox.ResumeLayout(false);
            this.TerminalGwLoginSettingsGroupBox.PerformLayout();
            this.pnlTSGWlogon.ResumeLayout(false);
            this.pnlTSGWlogon.PerformLayout();
            this.TerminalGwSettingsGroupBox.ResumeLayout(false);
            this.TerminalGwSettingsGroupBox.PerformLayout();
            this.pnlTSGWsettings.ResumeLayout(false);
            this.pnlTSGWsettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox TerminalGwLoginSettingsGroupBox;
        private System.Windows.Forms.Panel pnlTSGWlogon;
        private System.Windows.Forms.TextBox txtTSGWDomain;
        private System.Windows.Forms.TextBox txtTSGWPassword;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtTSGWUserName;
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
    }
}
