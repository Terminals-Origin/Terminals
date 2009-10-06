using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Credentials
{
    public partial class CredentialManager : Form
    {
        public CredentialManager()
        {
            InitializeComponent();

        }

        public void BindList()
        {
            CredentialsListView.Items.Clear();
            
            List<CredentialSet> list = Settings.SavedCredentials;

            foreach (CredentialSet s in list)
            {
                ListViewItem item = new ListViewItem(s.Name);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, s.Domain));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, s.Username));
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
            ManageCredentialForm frm = new ManageCredentialForm(null);
            frm.ShowDialog();
            BindList();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (CredentialsListView.SelectedItems != null && CredentialsListView.SelectedItems.Count > 0)
            {
                string name = CredentialsListView.SelectedItems[0].Text;
                List<CredentialSet> list = Settings.SavedCredentials;
                foreach (CredentialSet set in list)
                {
                    if (set.Name == name)
                    {
                        ManageCredentialForm mgr = new ManageCredentialForm(set);
                        mgr.ShowDialog();
                        BindList();
                        break;
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (CredentialsListView.SelectedItems != null && CredentialsListView.SelectedItems.Count > 0)
            {
                string name = CredentialsListView.SelectedItems[0].Text;
                List<CredentialSet> list = Settings.SavedCredentials;

                CredentialSet foundSet = null;

                foreach (CredentialSet set in list)
                {
                    if (set.Name == name)
                    {
                        foundSet = set;
                        break;
                    }
                }
                if (foundSet != null)
                {
                    if (MessageBox.Show("Are you sure you want to delete credential "+name+"?", "Confirmation Required", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        list.Remove(foundSet);
                        Settings.SavedCredentials = list;
                        BindList();
                    }
                }
            }

        }

        

    }
}
