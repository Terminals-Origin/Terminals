using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class MasterPasswordOptionPanel : UserControl, IOptionPanel
    {
        public MasterPasswordOptionPanel()
        {
            InitializeComponent();

            this.lblPasswordsMatch.Text = string.Empty;
        }

        public void LoadSettings()
        {
            this.chkPasswordProtectTerminals.Checked = Settings.IsMasterPasswordDefined;
            this.PasswordTextbox.Enabled = Settings.IsMasterPasswordDefined;
            this.ConfirmPasswordTextBox.Enabled = Settings.IsMasterPasswordDefined;
            this.FillTextBoxesByMasterPassword(Settings.IsMasterPasswordDefined);
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
            if (!this.chkPasswordProtectTerminals.Checked && Settings.IsMasterPasswordDefined)
            {
                Settings.UpdateMasterPassword(string.Empty);
            }
            else
            {
                if (this.PasswordsMatch && this.PasswordTextbox.Text != NewTerminalForm.HIDDEN_PASSWORD)
                {
                    Settings.UpdateMasterPassword(this.PasswordTextbox.Text);
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
