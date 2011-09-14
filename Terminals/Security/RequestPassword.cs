using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Unified.Encryption.Hash;

namespace Terminals.Security
{
    public partial class RequestPassword : Form
    {
        public RequestPassword()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            String newPass = this.PasswordTextBox.Text;
            if (Settings.ValidateMasterPassword(newPass))
            {
                this.PasswordTextBox.Focus();
                this.PasswordTextBox.Text = "";
                this.label2.Visible = true;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }

        private void CancelPasswordButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            this.label2.Visible = false;
        }
    }
}