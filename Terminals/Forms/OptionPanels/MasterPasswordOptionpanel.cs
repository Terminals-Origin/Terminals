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
        }

        public void LoadSettings()
        {
            this.ClearMasterButton.Enabled = false;
            if (Settings.IsMasterPasswordDefined)
            {
                this.chkPasswordProtectTerminals.Checked = true;
                this.chkPasswordProtectTerminals.Enabled = false;
                this.PasswordTextbox.Enabled = false;
                this.ConfirmPasswordTextBox.Enabled = false;
                this.ClearMasterButton.Enabled = true;
            }
        }

        public void SaveSettings()
        {
            if (this.chkPasswordProtectTerminals.Checked &&
                !String.IsNullOrEmpty(this.PasswordTextbox.Text) &&
                !String.IsNullOrEmpty(this.ConfirmPasswordTextBox.Text) &&
                this.PasswordTextbox.Text.Equals(this.ConfirmPasswordTextBox.Text))
            {
                Settings.UpdateMasterPassword(this.PasswordTextbox.Text);
            }
        }

        private void ClearMasterButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the master password?\r\n\r\n**Please be advised that this will render ALL saved passwords inactive!**",
                Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Settings.UpdateMasterPassword(String.Empty);
                this.ClearMasterButton.Enabled = false;

                this.chkPasswordProtectTerminals.Checked = false;
                this.chkPasswordProtectTerminals.Enabled = true;
                this.PasswordTextbox.Enabled = true;
                this.ConfirmPasswordTextBox.Enabled = true;
                this.PasswordTextbox.Text = String.Empty;
                this.ConfirmPasswordTextBox.Text = String.Empty;
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
            if (!this.PasswordTextbox.Text.Equals(String.Empty) && !this.ConfirmPasswordTextBox.Text.Equals(String.Empty))
            {
                if (this.PasswordTextbox.Text.Equals(this.ConfirmPasswordTextBox.Text))
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
