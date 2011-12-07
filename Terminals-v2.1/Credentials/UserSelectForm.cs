using System;
using System.Windows.Forms;
using Terminals.Configuration;

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
            set.SecretKey = passwordTextBox.Text;
            set.Domain = domainTextBox.Text;
            Close();
        }

        private void UserSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FavsList.credSet = set;
        }
    }
}
