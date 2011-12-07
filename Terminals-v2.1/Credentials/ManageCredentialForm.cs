﻿using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Credentials
{
    internal partial class ManageCredentialForm : Form
    {
        private string editedCredentialName = "";

        internal ManageCredentialForm(CredentialSet editedCredential)
        {
            InitializeComponent();

            FillControlsFromCredential(editedCredential);
        }

        private void FillControlsFromCredential(CredentialSet editedCredential)
        {
            if (editedCredential != null)
            {
                this.NameTextbox.Text = editedCredential.Name;
                this.DomainTextbox.Text = editedCredential.Domain;
                this.UsernameTextbox.Text = editedCredential.Username;
                if(!string.IsNullOrEmpty(editedCredential.Password))
                    this.PasswordTextbox.Text = NewTerminalForm.HIDDEN_PASSWORD;
                this.editedCredentialName = editedCredential.Name;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextbox.Text) || string.IsNullOrEmpty(UsernameTextbox.Text))
            {
                MessageBox.Show("You must enter both a Name and User Name for the credential",
                                "Credentail manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.NameTextbox.Focus();
                return;
            }

            if (UpdateCredential())
            {
                StoredCredentials.Instance.Save();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool UpdateCredential()
        {
            CredentialSet conflicting = StoredCredentials.Instance.GetByName(this.NameTextbox.Text);
            CredentialSet oldItem = StoredCredentials.Instance.GetByName(this.editedCredentialName);

            if (conflicting != null && this.editedCredentialName != this.NameTextbox.Text)
            {
                return UpdateConflicting(conflicting, oldItem);
            }

            UpdateOldOrCreateNew(oldItem);
            return true;
        }

        private void UpdateOldOrCreateNew(CredentialSet oldItem)
        {
            if (oldItem == null || this.editedCredentialName != this.NameTextbox.Text)
            {
                CredentialSet newCredential = this.CreateNewCredential();
                StoredCredentials.Instance.Add(newCredential);
            }
            else
            {
                this.UpdateFromControls(oldItem);
            }
        }

        private bool UpdateConflicting(CredentialSet conflicting, CredentialSet oldItem)
        {
            DialogResult result = MessageBox.Show("The Credential Name you entered already exists.\r\n" +
                                                  "Do you want to overwrite it?", "Credential manager",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return false;

            if (oldItem != null)
            {
                StoredCredentials.Instance.Remove(oldItem);
            }

            this.UpdateFromControls(conflicting);
            return true;
        }

        private void UpdateFromControls(CredentialSet toUpdate)
        {
            toUpdate.Domain = this.DomainTextbox.Text;
            toUpdate.Name = this.NameTextbox.Text;
            toUpdate.Username = this.UsernameTextbox.Text;
            if(this.PasswordTextbox.Text != NewTerminalForm.HIDDEN_PASSWORD)
                toUpdate.SecretKey = this.PasswordTextbox.Text;
        }

        private CredentialSet CreateNewCredential()
        {
            CredentialSet newItem = new CredentialSet();
            UpdateFromControls(newItem);
            return newItem;
        }
    }
}
