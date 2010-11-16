using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Credentials
{
    public partial class UserSelectForm : Form
    {
        private CredentialSet set = null;
        public UserSelectForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            set = new CredentialSet();
            set.Name = "";
            set.Username = userTextBox.Text;
            set.Password = passwordTextBox.Text;
            set.Domain = domainTextBox.Text;
            Close();
        }

        private void UserSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FavsList.credSet = set;
        }
    }
}
