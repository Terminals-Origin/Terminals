using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Wizard
{
    internal partial class EnterPassword : UserControl
    {
        public bool StorePassword
        {
            get
            {
                return EnableMasterPassword.Checked && !string.IsNullOrEmpty(masterPasswordTextbox.Text);
            }
        }

        public string Password
        {
            get
            {
                if(this.masterPasswordTextbox.Text == this.confirmTextBox.Text)
                    return this.masterPasswordTextbox.Text;
                
                return string.Empty;
            }
        }

        public EnterPassword()
        {
            this.InitializeComponent();

            this.EnableMasterPassword.Checked = true;
            this.EnableMasterPassword.Enabled = true;
            this.panel1.Enabled = true;
            this.revealPwdButton.Click += this.RevealOrHidePwd;
        }

        private void ConfirmTextBox_TextChanged(object sender, EventArgs e)
        {
            if(masterPasswordTextbox.Text != confirmTextBox.Text)
                ErrorLabel.Text = "Passwords do not match!";
            else
                ErrorLabel.Text = "Passwords match!";

            this.strengthProgress.Value = PasswordStrength.Strength(this.masterPasswordTextbox.Text);
            if(this.strengthProgress.Value <= 10)
            {
                this.strengthProgress.ForeColor = Color.Red;
            }
            else if(this.strengthProgress.Value <= 50)
            {
                this.strengthProgress.ForeColor = Color.Yellow;
            }
            else if(this.strengthProgress.Value <= 75)
            {
                this.strengthProgress.ForeColor = Color.Green;
            }
            else if(this.strengthProgress.Value <= 100)
            {
                this.strengthProgress.ForeColor = Color.Blue;
            }

        }

        private void EnableMasterPassword_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = EnableMasterPassword.Checked;
            this.HidePassword();
            this.SwitchMasterKeyWarning();
        }

        private void SwitchMasterKeyWarning()
        {
            this.warnLabel.Visible = !this.EnableMasterPassword.Checked;
            this.warnPicture.Visible = !this.EnableMasterPassword.Checked;
            this.strengthProgress.Visible = this.EnableMasterPassword.Checked;
            this.stregthLabel.Visible = this.EnableMasterPassword.Checked;
        }

        public void AssignPersistence(IPersistence persistence)
        {
            if (persistence.Security.IsMasterPasswordDefined)
            {
                this.EnableMasterPassword.Checked = true;
                this.EnableMasterPassword.Enabled = false;
                this.panel1.Enabled = false;
            }
        }

        private void RevealOrHidePwd(object sender, EventArgs e)
        {
            if (this.revealPwdButton.ImageIndex == 1)
            {
                this.HidePassword();
            }
            else
            {
                this.masterPasswordTextbox.PasswordChar = '\0';
                this.confirmTextBox.PasswordChar = '\0';
                this.revealPwdButton.ImageIndex = 1;
            }
        }

        private void HidePassword()
        {
            this.masterPasswordTextbox.PasswordChar = '*';
            this.confirmTextBox.PasswordChar = '*';
            this.revealPwdButton.ImageIndex = 0;
        }
    }
}
