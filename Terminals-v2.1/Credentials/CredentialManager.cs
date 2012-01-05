using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Credentials
{
    internal partial class CredentialManager : Form
    {
        private static StoredCredentials Credentials
        {
            get { return Persistance.Instance.Credentials; }
        }

        internal CredentialManager()
        {
            InitializeComponent();
            Credentials.CredentialsChanged += new EventHandler(this.CredentialsChanged);
        }

        private void CredentialsChanged(object sender, EventArgs e)
        {
            BindList();
        }

        private void BindList()
        {
            CredentialsListView.Items.Clear();

            foreach (ICredentialSet credential in Credentials.Items)
            {
                ListViewItem item = new ListViewItem(credential.Name);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, credential.Username));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, credential.Domain));
                CredentialsListView.Items.Add(item);
            }
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CredentialManager_Load(object sender, EventArgs e)
        {
            BindList();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            EditCredential(null);
        }

        private ICredentialSet GetSelectedItemCredentials()
        {
            if (CredentialsListView.SelectedItems != null && CredentialsListView.SelectedItems.Count > 0)
            {
                string name = CredentialsListView.SelectedItems[0].Text;
                return Credentials.GetByName(name);
            }

            return null;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            ICredentialSet selected = GetSelectedItemCredentials();
            if (selected != null)
            {
                EditCredential(selected);
            }
        }

        private void EditCredential(ICredentialSet selected)
        {
            ManageCredentialForm mgr = new ManageCredentialForm(selected);
            if (mgr.ShowDialog() == DialogResult.OK)
                this.BindList();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            ICredentialSet toRemove = GetSelectedItemCredentials();
            if (toRemove != null)
            {
                if (MessageBox.Show("Are you sure you want to delete credential " + toRemove.Name + "?",
                                    "Credential manager", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Credentials.Remove(toRemove);
                    Credentials.Save();
                    BindList();
                }
            }
        }

        private void CredentialManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            Credentials.CredentialsChanged -= CredentialsChanged;
        }
    }
}
