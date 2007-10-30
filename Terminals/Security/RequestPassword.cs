using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Security {
    public partial class RequestPassword : Form {
        public RequestPassword() {
            InitializeComponent();
        }
        string hashedPassword = Settings.TerminalsPassword;
        private void OkButton_Click(object sender, EventArgs e) {
            string newPass = this.PasswordTextBox.Text;
            string newHashed = Unified.Encryption.Hash.Hash.GetHash(newPass, Unified.Encryption.Hash.Hash.HashType.SHA512);
            if (newHashed != hashedPassword) {
                this.PasswordTextBox.Focus();
                this.PasswordTextBox.Text = "";
                this.label2.Visible = true;
                //System.Windows.Forms.MessageBox.Show("Invalid Password.");
            } else {
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }

        private void RequestPassword_Load(object sender, EventArgs e) {

        }

        private void CancelPasswordButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();

        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            this.label2.Visible = false;
        }
    }
}