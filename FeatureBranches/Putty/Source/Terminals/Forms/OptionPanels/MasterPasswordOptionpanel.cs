using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.Controls;

namespace Terminals.Forms
{
    internal partial class MasterPasswordOptionPanel : UserControl, IOptionPanel
    {
        internal PersistenceSecurity Security { get; set; }

        private bool PasswordsMatch
        {
            get { return this.PasswordTextbox.Text.Equals(this.ConfirmPasswordTextBox.Text); }
        }

        private bool PasswordsAreEntered
        {
            get
            {
                return !String.IsNullOrEmpty(this.PasswordTextbox.Text) &&
                       !String.IsNullOrEmpty(this.ConfirmPasswordTextBox.Text);
            }
        }
        

        public MasterPasswordOptionPanel()
        {
            InitializeComponent();
            this.revealPwdButton.Click += this.RevealOrHidePwd;
            this.lblPasswordsMatch.Text = string.Empty;
        }

        public void LoadSettings()
        {
            bool isMasterPasswordDefined = this.Security.IsMasterPasswordDefined;
            this.chkPasswordProtectTerminals.Checked = isMasterPasswordDefined;
            this.PasswordTextbox.Enabled = isMasterPasswordDefined;
            this.ConfirmPasswordTextBox.Enabled = isMasterPasswordDefined;
            this.FillTextBoxesByMasterPassword(isMasterPasswordDefined);
        }

        private void FillTextBoxesByMasterPassword(bool isMasterPasswordDefined)
        {
            if (isMasterPasswordDefined)
            {
                this.PasswordTextbox.Text = CredentialsPanel.HIDDEN_PASSWORD;
                this.ConfirmPasswordTextBox.Text = this.PasswordTextbox.Text;
            }
            else
            {
                this.PasswordTextbox.Text = String.Empty;
                this.ConfirmPasswordTextBox.Text = String.Empty;
            }
        }

        public void SaveSettings()
        {
            if (!this.chkPasswordProtectTerminals.Checked && this.Security.IsMasterPasswordDefined)
            {
                this.Security.UpdateMasterPassword(string.Empty); // remove password
            }
            else // new password is defined
            {
                if (this.PasswordsMatch &&
                    !string.IsNullOrEmpty(this.PasswordTextbox.Text) &&
                    this.PasswordTextbox.Text != CredentialsPanel.HIDDEN_PASSWORD)
                {
                    this.Security.UpdateMasterPassword(this.PasswordTextbox.Text);
                }
            }
        }

        private void ChkPasswordProtectTerminals_CheckedChanged(object sender, EventArgs e)
        {
            this.PasswordTextbox.Enabled = this.chkPasswordProtectTerminals.Checked;
            this.ConfirmPasswordTextBox.Enabled = this.chkPasswordProtectTerminals.Checked;
            this.lblPasswordsMatch.Visible = this.chkPasswordProtectTerminals.Checked;
            this.revealPwdButton.Enabled = this.chkPasswordProtectTerminals.Checked;
            this.HidePassword();
            
            this.lblPasswordsMatch.Text = String.Empty;
        }

        private void PasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            this.CheckPasswords();
        }

        private void ConfirmPasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            this.CheckPasswords();
        }

        private void CheckPasswords()
        {
            if (this.PasswordsAreEntered)
            {
                if (this.PasswordsMatch)
                {
                    this.lblPasswordsMatch.Text = "Passwords match";
                    this.lblPasswordsMatch.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    this.lblPasswordsMatch.Text = "Passwords do not match";
                    this.lblPasswordsMatch.ForeColor = Color.Red;
                }
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
                this.PasswordTextbox.PasswordChar = '\0';
                this.ConfirmPasswordTextBox.PasswordChar = '\0';
                this.revealPwdButton.ImageIndex = 1;
            }
        }

        private void HidePassword() {
            this.PasswordTextbox.PasswordChar = '*';
            this.ConfirmPasswordTextBox.PasswordChar = '*';
            this.revealPwdButton.ImageIndex = 0;
        }
    }
}
