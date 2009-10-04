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
        public ManageCredentialForm()
        {
            InitializeComponent();
        }

        CredentialSet editedSet = null;
        public CredentialSet EditedSet { get { return editedSet; } 
            set { 
                editedSet = value;
                DomainTextbox.Text = editedSet.Domain;
                NameTextbox.Text = editedSet.Name;
                PasswordTextbox.Text = editedSet.Password;
                UsernameTextbox.Text = editedSet.Username;
            } 
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            CredentialSet set = new CredentialSet();
            set.Domain = DomainTextbox.Text;
            set.Name = NameTextbox.Text;
            set.Password = PasswordTextbox.Text;
            set.Username = UsernameTextbox.Text;

            List<CredentialSet> list = Settings.SavedCredentials;

            CredentialSet foundSet = null;
            foreach (CredentialSet item in list)
            {
                if (item.Name == set.Name)
                {
                    foundSet = item;
                    break;
                }
            }
            if (foundSet != null) list.Remove(foundSet);
            list.Add(set);

            Settings.SavedCredentials = list;

            this.EditedSet = set;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
