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
            this.Credentials = Persistance.Instance.Factory.CreateCredentialSet();
            this.Credentials.Name = string.Empty;
            this.Credentials.UserName = userTextBox.Text;
            this.Credentials.Password = passwordTextBox.Text;
            this.Credentials.Domain = domainTextBox.Text;
            Close();
        }
    }
}
