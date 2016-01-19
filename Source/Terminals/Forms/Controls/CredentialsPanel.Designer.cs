namespace Terminals.Forms.Controls
{
    partial class CredentialsPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialsPanel));
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDomains = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.revealPwdButtonImages = new System.Windows.Forms.ImageList(this.components);
            this.revealPwdButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbUsers
            // 
            this.cmbUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbUsers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUsers.Location = new System.Drawing.Point(132, 37);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(351, 21);
            this.cmbUsers.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "&Domain:";
            // 
            // cmbDomains
            // 
            this.cmbDomains.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDomains.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDomains.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDomains.Location = new System.Drawing.Point(132, 6);
            this.cmbDomains.Name = "cmbDomains";
            this.cmbDomains.Size = new System.Drawing.Size(351, 21);
            this.cmbDomains.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "&User name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "&Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(132, 69);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(327, 20);
            this.txtPassword.TabIndex = 11;
            // 
            // revealPwdButtonImages
            // 
            this.revealPwdButtonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("revealPwdButtonImages.ImageStream")));
            this.revealPwdButtonImages.TransparentColor = System.Drawing.Color.Transparent;
            this.revealPwdButtonImages.Images.SetKeyName(0, "eye_hide.png");
            this.revealPwdButtonImages.Images.SetKeyName(1, "eye_reveal.png");
            // 
            // revealPwdButton
            // 
            this.revealPwdButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.revealPwdButton.FlatAppearance.BorderSize = 0;
            this.revealPwdButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.revealPwdButton.ImageIndex = 0;
            this.revealPwdButton.ImageList = this.revealPwdButtonImages;
            this.revealPwdButton.Location = new System.Drawing.Point(458, 64);
            this.revealPwdButton.Name = "revealPwdButton";
            this.revealPwdButton.Size = new System.Drawing.Size(25, 30);
            this.revealPwdButton.TabIndex = 12;
            this.revealPwdButton.UseVisualStyleBackColor = false;
            // 
            // CredentialsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.revealPwdButton);
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDomains);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPassword);
            this.Name = "CredentialsPanel";
            this.Size = new System.Drawing.Size(483, 94);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDomains;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ImageList revealPwdButtonImages;
        private System.Windows.Forms.Button revealPwdButton;
    }
}
