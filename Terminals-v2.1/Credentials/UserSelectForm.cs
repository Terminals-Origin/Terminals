using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Credentials
{
    internal partial class UserSelectForm : Form
    {
        internal ICredentialSet Credentials { get; private set; }

        internal UserSelectForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Credentials = Persistance.Instance.Credentials.CreateCredentialSet();
            this.Credentials.Name = string.Empty;
            this.Credentials.Username = userTextBox.Text;
            this.Credentials.SecretKey = passwordTextBox.Text;
            this.Credentials.Domain = domainTextBox.Text;
            Close();
        }
    }
}
