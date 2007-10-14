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

            this.masterPasswordTextbox.Text = Settings.TerminalsPassword;
            this.confirmTextBox.Text = Settings.TerminalsPassword;
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
    }
}
