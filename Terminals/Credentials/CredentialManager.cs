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
            string selectedItem = "";
            if (CredentialsListView.SelectedItems != null && CredentialsListView.SelectedItems.Count > 0)
            {
                selectedItem = CredentialsListView.SelectedItems[0].Text;
            }
            CredentialsListView.Clear();
            CredentialsListView.Columns.Add("Name");
            CredentialsListView.Columns.Add("Domain");
            CredentialsListView.Columns.Add("Username");

            List<CredentialSet> list = Settings.SavedCredentials;


            foreach (CredentialSet s in list)
            {
                ListViewItem item = new ListViewItem(s.Name);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, s.Domain));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, s.Username));
                CredentialsListView.Items.Add(item);
                item.Selected = false;
                if (!string.IsNullOrEmpty(selectedItem))
                {
                    if (item.Text == selectedItem)
                    {
                        item.Selected = true;
                        item.Focused = true;
                    }
                }
            }
            CredentialsListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            CredentialsListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
            CredentialsListView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.ColumnContent);

            if (CredentialsListView.Columns[0].Width < 50) CredentialsListView.Columns[0].Width = 100;
            if (CredentialsListView.Columns[1].Width < 50) CredentialsListView.Columns[1].Width = 100;
            if (CredentialsListView.Columns[2].Width < 50) CredentialsListView.Columns[2].Width = 100;

        }


        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            ManageCredentialForm frm = new ManageCredentialForm();
            frm.ShowDialog();
            BindList();
        }

        private void CredentialManager_Load(object sender, EventArgs e)
        {
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
                        ManageCredentialForm mgr = new ManageCredentialForm();
                        mgr.EditedSet = set;
                        mgr.ShowDialog();
                        BindList();
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
                    if (System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this Credential?", "Confirmation Required", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
