using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Credentials
{
    internal partial class CredentialManager : Form
    {
        internal CredentialManager()
        {
            InitializeComponent();
        }

        private void BindList()
        {
            CredentialsListView.Items.Clear();
            List<CredentialSet> credentials = StoredCredentials.Instance.Items;

            foreach (CredentialSet credential in credentials)
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

        private CredentialSet GetSelectedItemCredentials()
        {
            if (CredentialsListView.SelectedItems != null && CredentialsListView.SelectedItems.Count > 0)
            {
                string name = CredentialsListView.SelectedItems[0].Text;
                return StoredCredentials.Instance.GetByName(name);
            }

            return null;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            CredentialSet selected = GetSelectedItemCredentials();
            if (selected != null)
            {
                EditCredential(selected);
            }
        }

        private void EditCredential(CredentialSet selected)
        {
            ManageCredentialForm mgr = new ManageCredentialForm(selected);
            if (mgr.ShowDialog() == DialogResult.OK)
                this.BindList();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            CredentialSet toRemove = GetSelectedItemCredentials();
            if (toRemove != null)
            {
                if (MessageBox.Show("Are you sure you want to delete credential " + toRemove.Name + "?",
                                    "Credential manager", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    StoredCredentials.Instance.Remove(toRemove);
                    StoredCredentials.Instance.Save();
                    BindList();
                }
            }
        }
    }
}
