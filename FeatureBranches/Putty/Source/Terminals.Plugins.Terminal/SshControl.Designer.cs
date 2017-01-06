using Terminals.Common.Configuration;

namespace Terminals.Forms.EditFavorite
{
    partial class SshControl
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
            this.SSHPreferences = new SSHClient.Preferences();
            this.SuspendLayout();
            // 
            // SSHPreferences
            // 
            this.SSHPreferences.AuthMethod = AuthMethod.PublicKey;
            this.SSHPreferences.KeyTag = "";
            this.SSHPreferences.Location = new System.Drawing.Point(0, 0);
            this.SSHPreferences.Margin = new System.Windows.Forms.Padding(4);
            this.SSHPreferences.Name = "SSHPreferences";
            this.SSHPreferences.Size = new System.Drawing.Size(516, 352);
            this.SSHPreferences.SSH1 = false;
            this.SSHPreferences.SSHKeyFile = "";
            this.SSHPreferences.TabIndex = 2;
            // 
            // SshControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SSHPreferences);
            this.Name = "SshControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.ResumeLayout(false);

        }

        #endregion

        private SSHClient.Preferences SSHPreferences;
    }
}
