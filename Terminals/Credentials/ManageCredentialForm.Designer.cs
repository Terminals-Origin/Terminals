namespace Terminals.Credentials
{
    partial class ManageCredentialForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageCredentialForm));
            this.CancelButton_cred = new System.Windows.Forms.Button();
            this.SaveButton_cred = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextbox = new System.Windows.Forms.TextBox();
            this.PasswordTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.UsernameTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DomainTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CancelButton_cred
            // 
            this.CancelButton_cred.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton_cred.Location = new System.Drawing.Point(199, 116);
            this.CancelButton_cred.Name = "CancelButton_cred";
            this.CancelButton_cred.Size = new System.Drawing.Size(75, 23);
            this.CancelButton_cred.TabIndex = 5;
            this.CancelButton_cred.Text = "Cancel";
            this.CancelButton_cred.UseVisualStyleBackColor = true;
            this.CancelButton_cred.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SaveButton_cred
            // 
            this.SaveButton_cred.Location = new System.Drawing.Point(118, 116);
            this.SaveButton_cred.Name = "SaveButton_cred";
            this.SaveButton_cred.Size = new System.Drawing.Size(75, 23);
            this.SaveButton_cred.TabIndex = 4;
            this.SaveButton_cred.Text = "Save";
            this.SaveButton_cred.UseVisualStyleBackColor = true;
            this.SaveButton_cred.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // NameTextbox
            // 
            this.NameTextbox.Location = new System.Drawing.Point(70, 12);
            this.NameTextbox.Name = "NameTextbox";
            this.NameTextbox.Size = new System.Drawing.Size(204, 20);
            this.NameTextbox.TabIndex = 0;
            // 
            // PasswordTextbox
            // 
            this.PasswordTextbox.Location = new System.Drawing.Point(70, 64);
            this.PasswordTextbox.Name = "PasswordTextbox";
            this.PasswordTextbox.PasswordChar = '*';
            this.PasswordTextbox.Size = new System.Drawing.Size(204, 20);
            this.PasswordTextbox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password:";
            // 
            // UsernameTextbox
            // 
            this.UsernameTextbox.Location = new System.Drawing.Point(70, 38);
            this.UsernameTextbox.Name = "UsernameTextbox";
            this.UsernameTextbox.Size = new System.Drawing.Size(204, 20);
            this.UsernameTextbox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "User Name:";
            // 
            // DomainTextbox
            // 
            this.DomainTextbox.Location = new System.Drawing.Point(70, 90);
            this.DomainTextbox.Name = "DomainTextbox";
            this.DomainTextbox.Size = new System.Drawing.Size(204, 20);
            this.DomainTextbox.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Domain:";
            // 
            // ManageCredentialForm
            // 
            this.AcceptButton = this.SaveButton_cred;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton_cred;
            this.ClientSize = new System.Drawing.Size(284, 145);
            this.ControlBox = false;
            this.Controls.Add(this.DomainTextbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.UsernameTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PasswordTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NameTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveButton_cred);
            this.Controls.Add(this.CancelButton_cred);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManageCredentialForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Credential";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton_cred;
        private System.Windows.Forms.Button SaveButton_cred;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextbox;
        private System.Windows.Forms.TextBox PasswordTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UsernameTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DomainTextbox;
        private System.Windows.Forms.Label label4;
    }
}