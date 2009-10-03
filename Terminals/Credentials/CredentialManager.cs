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
            ReloadList();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            if (this.SaveCredentialsComboBox.SelectedItem != null)
                this.SelectedCredentials = (this.SaveCredentialsComboBox.SelectedItem as CredentialSet);
            this.Close();
        }

        public CredentialSet SelectedCredentials { get; set; }

        private void SaveCredentialsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SaveCredentialsComboBox.SelectedItem != null)
            {
                CredentialSet set = (this.SaveCredentialsComboBox.SelectedItem as CredentialSet);
                if (set != null)
                {
                    this.DomainTextbox.Text = set.Domain;
                    this.UsernameTextbox.Text = set.Username;
                    this.PasswordTextbox.Text = set.Password;
                }
            }
        }
        private void ReloadList()
        {
            this.SaveCredentialsComboBox.DataSource = Settings.SavedCredentials;

        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            CredentialSet set = new CredentialSet();
            set.Domain = this.DomainTextbox.Text;
            set.Username = this.UsernameTextbox.Text;
            set.Password = this.PasswordTextbox.Text;
            List<CredentialSet> list = Settings.SavedCredentials;

            CredentialSet dup = null;
            foreach(CredentialSet s in list) {
                if (s.Domain == set.Domain && s.Username == set.Username)
                {
                    dup = s;
                    break;
                }
            }

            if (dup != null) list.Remove(dup);

            list.Add(set);
            Settings.SavedCredentials = list;
            ReloadList();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            CredentialSet set = (this.SaveCredentialsComboBox.SelectedItem as CredentialSet);
            if (set != null)
            {
                List<CredentialSet> list = Settings.SavedCredentials;


                CredentialSet dup = null;
                foreach (CredentialSet s in list)
                {
                    if (s.Domain == set.Domain && s.Username == set.Username)
                    {
                        dup = s;
                        break;
                    }
                }

                if (dup != null) list.Remove(dup);

                Settings.SavedCredentials = list;
                ReloadList();
            }
        }
    }
}
