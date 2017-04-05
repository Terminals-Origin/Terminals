using System.Windows.Forms;

namespace Terminals.Plugins.Putty
{
    partial class SshOptionsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        private Label labelSession;
        private CheckBox checkBoxX11Forwarding;
        private CheckBox checkBoxCompression;
        private ComboBox cmbSessionName;

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
            this.cmbSessionName = new System.Windows.Forms.ComboBox();
            this.labelSession = new System.Windows.Forms.Label();
            this.checkBoxX11Forwarding = new System.Windows.Forms.CheckBox();
            this.checkBoxCompression = new System.Windows.Forms.CheckBox();
            this.checkBoxVerbose = new System.Windows.Forms.CheckBox();
            this.cmbSshVersion = new System.Windows.Forms.ComboBox();
            this.labelSshVersion = new System.Windows.Forms.Label();
            this.checkBoxEnablePagentAuthentication = new System.Windows.Forms.CheckBox();
            this.checkBoxEnablePagentForwarding = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmbSessionName
            // 
            this.cmbSessionName.FormattingEnabled = true;
            this.cmbSessionName.Location = new System.Drawing.Point(76, 41);
            this.cmbSessionName.Name = "cmbSessionName";
            this.cmbSessionName.Size = new System.Drawing.Size(165, 21);
            this.cmbSessionName.TabIndex = 0;
            // 
            // labelSession
            // 
            this.labelSession.AutoSize = true;
            this.labelSession.Location = new System.Drawing.Point(3, 44);
            this.labelSession.Name = "labelSession";
            this.labelSession.Size = new System.Drawing.Size(44, 13);
            this.labelSession.TabIndex = 1;
            this.labelSession.Text = "Session";
            // 
            // checkBoxX11Forwarding
            // 
            this.checkBoxX11Forwarding.AutoSize = true;
            this.checkBoxX11Forwarding.Location = new System.Drawing.Point(6, 114);
            this.checkBoxX11Forwarding.Name = "checkBoxX11Forwarding";
            this.checkBoxX11Forwarding.Size = new System.Drawing.Size(97, 17);
            this.checkBoxX11Forwarding.TabIndex = 2;
            this.checkBoxX11Forwarding.Text = "X11 forwarding";
            this.checkBoxX11Forwarding.UseVisualStyleBackColor = true;
            // 
            // checkBoxCompression
            // 
            this.checkBoxCompression.AutoSize = true;
            this.checkBoxCompression.Location = new System.Drawing.Point(6, 137);
            this.checkBoxCompression.Name = "checkBoxCompression";
            this.checkBoxCompression.Size = new System.Drawing.Size(121, 17);
            this.checkBoxCompression.TabIndex = 3;
            this.checkBoxCompression.Text = "Enable compression";
            this.checkBoxCompression.UseVisualStyleBackColor = true;
            // 
            // checkBoxVerbose
            // 
            this.checkBoxVerbose.AutoSize = true;
            this.checkBoxVerbose.Location = new System.Drawing.Point(6, 160);
            this.checkBoxVerbose.Name = "checkBoxVerbose";
            this.checkBoxVerbose.Size = new System.Drawing.Size(65, 17);
            this.checkBoxVerbose.TabIndex = 4;
            this.checkBoxVerbose.Text = "Verbose";
            this.checkBoxVerbose.UseVisualStyleBackColor = true;
            // 
            // cmbSshVersion
            // 
            this.cmbSshVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSshVersion.FormattingEnabled = true;
            this.cmbSshVersion.Location = new System.Drawing.Point(76, 70);
            this.cmbSshVersion.Name = "cmbSshVersion";
            this.cmbSshVersion.Size = new System.Drawing.Size(165, 21);
            this.cmbSshVersion.TabIndex = 1;
            // 
            // labelSshVersion
            // 
            this.labelSshVersion.AutoSize = true;
            this.labelSshVersion.Location = new System.Drawing.Point(3, 73);
            this.labelSshVersion.Name = "labelSshVersion";
            this.labelSshVersion.Size = new System.Drawing.Size(67, 13);
            this.labelSshVersion.TabIndex = 6;
            this.labelSshVersion.Text = "SSH Version";
            // 
            // checkBoxEnablePagentAuthentication
            // 
            this.checkBoxEnablePagentAuthentication.AutoSize = true;
            this.checkBoxEnablePagentAuthentication.Location = new System.Drawing.Point(6, 183);
            this.checkBoxEnablePagentAuthentication.Name = "checkBoxEnablePagentAuthentication";
            this.checkBoxEnablePagentAuthentication.Size = new System.Drawing.Size(167, 17);
            this.checkBoxEnablePagentAuthentication.TabIndex = 5;
            this.checkBoxEnablePagentAuthentication.Text = "Enable Pagent Authentication";
            this.checkBoxEnablePagentAuthentication.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnablePagentForwarding
            // 
            this.checkBoxEnablePagentForwarding.AutoSize = true;
            this.checkBoxEnablePagentForwarding.Location = new System.Drawing.Point(6, 206);
            this.checkBoxEnablePagentForwarding.Name = "checkBoxEnablePagentForwarding";
            this.checkBoxEnablePagentForwarding.Size = new System.Drawing.Size(151, 17);
            this.checkBoxEnablePagentForwarding.TabIndex = 6;
            this.checkBoxEnablePagentForwarding.Text = "Enable Pagent Forwarding";
            this.checkBoxEnablePagentForwarding.UseVisualStyleBackColor = true;
            // 
            // SshOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxEnablePagentForwarding);
            this.Controls.Add(this.checkBoxEnablePagentAuthentication);
            this.Controls.Add(this.labelSshVersion);
            this.Controls.Add(this.cmbSshVersion);
            this.Controls.Add(this.checkBoxVerbose);
            this.Controls.Add(this.checkBoxCompression);
            this.Controls.Add(this.checkBoxX11Forwarding);
            this.Controls.Add(this.labelSession);
            this.Controls.Add(this.cmbSessionName);
            this.Name = "SshOptionsControl";
            this.Size = new System.Drawing.Size(650, 443);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private CheckBox checkBoxVerbose;
        private ComboBox cmbSshVersion;
        private Label labelSshVersion;
        private CheckBox checkBoxEnablePagentAuthentication;
        private CheckBox checkBoxEnablePagentForwarding;
    }
}
