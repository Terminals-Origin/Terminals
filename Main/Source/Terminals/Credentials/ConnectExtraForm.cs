using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Data.Credentials;

namespace Terminals.Credentials
{
    internal partial class ConnectExtraForm : Form
    {
        private readonly IPersistence persistence;

        internal bool Console
        {
            get { return this.consoleCheckBox.Checked; }
        }

        internal bool NewWindow
        {
            get { return this.newWindowCheckBox.Checked; }
        }

        private ICredentialSet customCredentials;
        internal ICredentialSet Credentials
        {
            get
            {
                if (!this.IsCustomSelected)
                    return this.credentialsComboBox.SelectedItem as ICredentialSet;

                var guarded = new GuardedCredential(this.customCredentials, this.persistence.Security);
                guarded.UserName = this.userTextBox.Text;
                guarded.Password = this.passwordTextBox.Text;
                guarded.Domain = this.domainTextBox.Text;
                return this.customCredentials;
            }
        }

        private bool IsCustomSelected
        {
            get { return this.customCredentials == this.credentialsComboBox.SelectedItem; }
        }

        internal ConnectExtraForm(IPersistence persistence)
        {
            this.persistence = persistence;
            InitializeComponent();
        }

        // dont do it in constructor to prevent wrong acces to the persistence from designer
        private void UserSelectForm_Shown(object sender, EventArgs e)
        {
            // it always has to be present
            this.customCredentials = this.persistence.Factory.CreateCredentialSet();
            this.customCredentials.Name = "(custom)";
            List<ICredentialSet> credentials = this.persistence.Credentials.ToList();
            credentials.Insert(0, this.customCredentials);
            this.credentialsComboBox.DataSource = credentials;
            this.credentialsComboBox.SelectedItem = this.customCredentials;
        }

        private void CredentialsComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            bool customSelected = this.IsCustomSelected;
            this.domainTextBox.Enabled = customSelected;
            this.userTextBox.Enabled = customSelected;
            this.passwordTextBox.Enabled = customSelected;
        }
    }
}
