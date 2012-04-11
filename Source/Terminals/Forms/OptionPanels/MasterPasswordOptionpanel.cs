using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms
{
    internal partial class MasterPasswordOptionPanel : UserControl, IOptionPanel
    {
        private PersistenceSecurity security;

        public MasterPasswordOptionPanel()
        {
            InitializeComponent();

            this.lblPasswordsMatch.Text = string.Empty;
            this.security = Persistance.Instance.Security;
        }

        public void LoadSettings()
        {
            bool isMasterPasswordDefined = this.security.IsMasterPasswordDefined;
            this.chkPasswordProtectTerminals.Checked = isMasterPasswordDefined;
            this.PasswordTextbox.Enabled = isMasterPasswordDefined;
            this.ConfirmPasswordTextBox.Enabled = isMasterPasswordDefined;
            this.FillTextBoxesByMasterPassword(isMasterPasswordDefined);
        }

        private void FillTextBoxesByMasterPassword(bool isMasterPasswordDefined)
        {
            if (isMasterPasswordDefined)
            {
                this.PasswordTextbox.Text = NewTerminalForm.HIDDEN_PASSWORD;
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
            if (!this.chkPasswordProtectTerminals.Checked && this.security.IsMasterPasswordDefined)
            {
                this.security.UpdateMasterPassword(string.Empty); // remove password
            }
            else // new password is defined
            {
                if (this.PasswordsMatch &&
                    !string.IsNullOrEmpty(this.PasswordTextbox.Text) &&
                    this.PasswordTextbox.Text != NewTerminalForm.HIDDEN_PASSWORD)
                {
                    this.security.UpdateMasterPassword(this.PasswordTextbox.Text);
                }
            }
        }

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

        private void chkPasswordProtectTerminals_CheckedChanged(object sender, EventArgs e)
        {
            this.PasswordTextbox.Enabled = this.chkPasswordProtectTerminals.Checked;
            this.ConfirmPasswordTextBox.Enabled = this.chkPasswordProtectTerminals.Checked;
            this.lblPasswordsMatch.Visible = this.chkPasswordProtectTerminals.Checked;
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
    }
}
