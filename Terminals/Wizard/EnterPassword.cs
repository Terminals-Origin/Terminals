using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Wizard
{
    public partial class EnterPassword : UserControl
    {
        public EnterPassword()
        {
            InitializeComponent();

            EnableMasterPassword.Checked = true;
            EnableMasterPassword.Enabled = true;
            panel1.Enabled = true;

            if(Settings.TerminalsPassword != "")
            {
                EnableMasterPassword.Checked = true;
                EnableMasterPassword.Enabled = false;
                panel1.Enabled = false;
            }
        }
        public bool StorePassword
        {
            get
            {
                if(EnableMasterPassword.Checked && masterPasswordTextbox.Text!="") return true;
                return false;
            }
        }
        private void confirmTextBox_TextChanged(object sender, EventArgs e)
        {
            if(masterPasswordTextbox.Text != confirmTextBox.Text)
                ErrorLabel.Text = "Passwords do not match!";
            else
                ErrorLabel.Text = "Passwords match!";

            this.progressBar1.Value = Wizard.PasswordStrength.Strength(this.masterPasswordTextbox.Text);
            if(this.progressBar1.Value <= 10)
            {
                this.progressBar1.ForeColor = Color.Red;
            }
            else if(this.progressBar1.Value <= 50)
            {
                this.progressBar1.ForeColor = Color.Yellow;
            }
            else if(this.progressBar1.Value <= 75)
            {
                this.progressBar1.ForeColor = Color.Green;
            }
            else if(this.progressBar1.Value <= 100)
            {
                this.progressBar1.ForeColor = Color.Blue;
            }

        }
        
        public string Password
        {
            get
            {
                if(this.masterPasswordTextbox.Text == confirmTextBox.Text)
                {
                    return this.masterPasswordTextbox.Text;
                }
                return "";
            }
        }

        private void EnableMasterPassword_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = EnableMasterPassword.Checked; ;
        }
    }
}
