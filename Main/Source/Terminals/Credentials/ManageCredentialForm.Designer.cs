using Terminals.Forms.Controls;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageCredentialForm));
            this.CancelButton_cred = new System.Windows.Forms.Button();
            this.SaveButton_cred = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextbox = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.credentialsPanel1 = new Terminals.Forms.Controls.CredentialsPanel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton_cred
            // 
            this.CancelButton_cred.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton_cred.Location = new System.Drawing.Point(272, 145);
            this.CancelButton_cred.Name = "CancelButton_cred";
            this.CancelButton_cred.Size = new System.Drawing.Size(75, 23);
            this.CancelButton_cred.TabIndex = 5;
            this.CancelButton_cred.Text = "Cancel";
            this.CancelButton_cred.UseVisualStyleBackColor = true;
            // 
            // SaveButton_cred
            // 
            this.SaveButton_cred.Location = new System.Drawing.Point(191, 145);
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
            this.errorProvider.SetIconAlignment(this.NameTextbox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.NameTextbox.Location = new System.Drawing.Point(86, 12);
            this.NameTextbox.Name = "NameTextbox";
            this.NameTextbox.Size = new System.Drawing.Size(227, 20);
            this.NameTextbox.TabIndex = 0;
            this.toolTip1.SetToolTip(this.NameTextbox, "An alias for the credential.\r\nThis is a required field.");
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // credentialsPanel1
            // 
            this.credentialsPanel1.Location = new System.Drawing.Point(5, 35);
            this.credentialsPanel1.Name = "credentialsPanel1";
            this.credentialsPanel1.Settings = null;
            this.credentialsPanel1.Size = new System.Drawing.Size(342, 85);
            this.credentialsPanel1.TabIndex = 2;
            this.credentialsPanel1.TextEditsLeft = 3;
            // 
            // ManageCredentialForm
            // 
            this.AcceptButton = this.SaveButton_cred;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton_cred;
            this.ClientSize = new System.Drawing.Size(359, 180);
            this.Controls.Add(this.credentialsPanel1);
            this.Controls.Add(this.NameTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveButton_cred);
            this.Controls.Add(this.CancelButton_cred);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageCredentialForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Terminals - Manage Credential";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton_cred;
        private System.Windows.Forms.Button SaveButton_cred;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextbox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private CredentialsPanel credentialsPanel1;
    }
}