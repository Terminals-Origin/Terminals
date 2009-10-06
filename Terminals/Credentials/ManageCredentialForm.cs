using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Credentials
{
    public partial class ManageCredentialForm : Form
    {
        private string editedCred = "";

        public ManageCredentialForm(CredentialSet editedSet)
        {
            InitializeComponent();
            if (editedSet != null)
            {
                NameTextbox.Enabled = false;
                DomainTextbox.Text = editedSet.Domain;
                NameTextbox.Text = editedSet.Name;
                PasswordTextbox.Text = editedSet.Password;
                UsernameTextbox.Text = editedSet.Username;
                editedCred = editedSet.Name;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextbox.Text) || string.IsNullOrEmpty(UsernameTextbox.Text))
            {
                MessageBox.Show("You must enter both a Name and User Name for the credential", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            List<CredentialSet> list = Settings.SavedCredentials;

            CredentialSet foundSet = null;
            foreach (CredentialSet item in list)
            {
                if (item.Name.ToLower() == NameTextbox.Text.ToLower())
                {
                    foundSet = item;
                    break;
                }
            }
            if (foundSet != null)
            {
                if (foundSet.Name == editedCred)
                    list.Remove(foundSet);
                else
                {
                    MessageBox.Show("The Credential Name you entered already exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            CredentialSet set = new CredentialSet();
            set.Domain = DomainTextbox.Text;
            set.Name = NameTextbox.Text;
            set.Password = PasswordTextbox.Text;
            set.Username = UsernameTextbox.Text;

            list.Add(set);
            Settings.SavedCredentials = list;

            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
