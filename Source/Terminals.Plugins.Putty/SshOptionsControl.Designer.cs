using System.Windows.Forms;

namespace Terminals.Plugins.Putty
{
    partial class SshOptionsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private CheckBox checkBoxX11Forwarding;
        private CheckBox checkBoxCompression;

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
            this.checkBoxX11Forwarding = new System.Windows.Forms.CheckBox();
            this.checkBoxCompression = new System.Windows.Forms.CheckBox();
            this.cmbSshVersion = new System.Windows.Forms.ComboBox();
            this.labelSshVersion = new System.Windows.Forms.Label();
            this.checkBoxEnablePagentAuthentication = new System.Windows.Forms.CheckBox();
            this.checkBoxEnablePagentForwarding = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxX11Forwarding
            // 
            this.checkBoxX11Forwarding.AutoSize = true;
            this.checkBoxX11Forwarding.Location = new System.Drawing.Point(107, 80);
            this.checkBoxX11Forwarding.Name = "checkBoxX11Forwarding";
            this.checkBoxX11Forwarding.Size = new System.Drawing.Size(97, 17);
            this.checkBoxX11Forwarding.TabIndex = 2;
            this.checkBoxX11Forwarding.Text = "X11 forwarding";
            this.checkBoxX11Forwarding.UseVisualStyleBackColor = true;
            // 
            // checkBoxCompression
            // 
            this.checkBoxCompression.AutoSize = true;
            this.checkBoxCompression.Location = new System.Drawing.Point(107, 103);
            this.checkBoxCompression.Name = "checkBoxCompression";
            this.checkBoxCompression.Size = new System.Drawing.Size(121, 17);
            this.checkBoxCompression.TabIndex = 3;
            this.checkBoxCompression.Text = "Enable compression";
            this.checkBoxCompression.UseVisualStyleBackColor = true;
            // 
            // cmbSshVersion
            // 
            this.cmbSshVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSshVersion.FormattingEnabled = true;
            this.cmbSshVersion.Location = new System.Drawing.Point(107, 172);
            this.cmbSshVersion.Name = "cmbSshVersion";
            this.cmbSshVersion.Size = new System.Drawing.Size(165, 21);
            this.cmbSshVersion.TabIndex = 1;
            // 
            // labelSshVersion
            // 
            this.labelSshVersion.AutoSize = true;
            this.labelSshVersion.Location = new System.Drawing.Point(18, 175);
            this.labelSshVersion.Name = "labelSshVersion";
            this.labelSshVersion.Size = new System.Drawing.Size(67, 13);
            this.labelSshVersion.TabIndex = 6;
            this.labelSshVersion.Text = "SSH Version";
            // 
            // checkBoxEnablePagentAuthentication
            // 
            this.checkBoxEnablePagentAuthentication.AutoSize = true;
            this.checkBoxEnablePagentAuthentication.Location = new System.Drawing.Point(107, 126);
            this.checkBoxEnablePagentAuthentication.Name = "checkBoxEnablePagentAuthentication";
            this.checkBoxEnablePagentAuthentication.Size = new System.Drawing.Size(167, 17);
            this.checkBoxEnablePagentAuthentication.TabIndex = 5;
            this.checkBoxEnablePagentAuthentication.Text = "Enable Pagent Authentication";
            this.checkBoxEnablePagentAuthentication.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnablePagentForwarding
            // 
            this.checkBoxEnablePagentForwarding.AutoSize = true;
            this.checkBoxEnablePagentForwarding.Location = new System.Drawing.Point(107, 149);
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
            this.Controls.Add(this.checkBoxCompression);
            this.Controls.Add(this.checkBoxX11Forwarding);
            this.Name = "SshOptionsControl";
            this.Controls.SetChildIndex(this.checkBoxX11Forwarding, 0);
            this.Controls.SetChildIndex(this.checkBoxCompression, 0);
            this.Controls.SetChildIndex(this.cmbSshVersion, 0);
            this.Controls.SetChildIndex(this.labelSshVersion, 0);
            this.Controls.SetChildIndex(this.checkBoxEnablePagentAuthentication, 0);
            this.Controls.SetChildIndex(this.checkBoxEnablePagentForwarding, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion
        private ComboBox cmbSshVersion;
        private Label labelSshVersion;
        private CheckBox checkBoxEnablePagentAuthentication;
        private CheckBox checkBoxEnablePagentForwarding;
    }
}
