namespace Terminals.Forms.Controls
{
    partial class SecurityPanel
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
            this.credentialsPanel1 = new CredentialsPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.credentialDropdown = new System.Windows.Forms.ComboBox();
            this.CredentialManagerPicturebox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CredentialManagerPicturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // credentialsPanel1
            // 
            this.credentialsPanel1.Location = new System.Drawing.Point(-4, 30);
            this.credentialsPanel1.Name = "credentialsPanel1";
            this.credentialsPanel1.Size = new System.Drawing.Size(484, 104);
            this.credentialsPanel1.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(57, 13);
            this.label15.TabIndex = 38;
            this.label15.Text = "Credential:";
            // 
            // credentialDropdown
            // 
            this.credentialDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.credentialDropdown.DisplayMember = "Name";
            this.credentialDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.credentialDropdown.FormattingEnabled = true;
            this.credentialDropdown.Location = new System.Drawing.Point(128, 6);
            this.credentialDropdown.Name = "credentialDropdown";
            this.credentialDropdown.Size = new System.Drawing.Size(352, 21);
            this.credentialDropdown.TabIndex = 39;
            this.credentialDropdown.SelectedIndexChanged += new System.EventHandler(this.CredentialDropdown_SelectedIndexChanged);
            // 
            // CredentialManagerPicturebox
            // 
            this.CredentialManagerPicturebox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CredentialManagerPicturebox.Image = global::Terminals.Properties.Resources.computer_security;
            this.CredentialManagerPicturebox.Location = new System.Drawing.Point(486, 9);
            this.CredentialManagerPicturebox.Name = "CredentialManagerPicturebox";
            this.CredentialManagerPicturebox.Size = new System.Drawing.Size(16, 16);
            this.CredentialManagerPicturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CredentialManagerPicturebox.TabIndex = 40;
            this.CredentialManagerPicturebox.TabStop = false;
            this.CredentialManagerPicturebox.Click += new System.EventHandler(this.CredentialManagerPicturebox_Click);
            // 
            // SecurityPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label15);
            this.Controls.Add(this.credentialDropdown);
            this.Controls.Add(this.CredentialManagerPicturebox);
            this.Controls.Add(this.credentialsPanel1);
            this.Name = "SecurityPanel";
            this.Size = new System.Drawing.Size(511, 128);
            ((System.ComponentModel.ISupportInitialize)(this.CredentialManagerPicturebox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CredentialsPanel credentialsPanel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox credentialDropdown;
        private System.Windows.Forms.PictureBox CredentialManagerPicturebox;
    }
}
