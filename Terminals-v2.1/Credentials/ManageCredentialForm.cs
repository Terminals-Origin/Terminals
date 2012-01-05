using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Credentials
{
    internal partial class ManageCredentialForm : Form
    {
        private string editedCredentialName = "";

        private StoredCredentials Credentials
        {
            get { return Persistance.Instance.Credentials; }
        }

        internal ManageCredentialForm(ICredentialSet editedCredential)
        {
            InitializeComponent();

            FillControlsFromCredential(editedCredential);
        }

        private void FillControlsFromCredential(ICredentialSet editedCredential)
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
                Credentials.Save();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool UpdateCredential()
        {
            ICredentialSet conflicting = Credentials.GetByName(this.NameTextbox.Text);
            ICredentialSet oldItem = Credentials.GetByName(this.editedCredentialName);

            if (conflicting != null && this.editedCredentialName != this.NameTextbox.Text)
            {
                return UpdateConflicting(conflicting, oldItem);
            }

            UpdateOldOrCreateNew(oldItem);
            return true;
        }

        private void UpdateOldOrCreateNew(ICredentialSet oldItem)
        {
            if (oldItem == null || this.editedCredentialName != this.NameTextbox.Text)
            {
                ICredentialSet newCredential = this.CreateNewCredential();
                Credentials.Add(newCredential);
            }
            else
            {
                this.UpdateFromControls(oldItem);
            }
        }

        private bool UpdateConflicting(ICredentialSet conflicting, ICredentialSet oldItem)
        {
            DialogResult result = MessageBox.Show("The Credential Name you entered already exists.\r\n" +
                                                  "Do you want to overwrite it?", "Credential manager",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return false;

            if (oldItem != null)
            {
                Credentials.Remove(oldItem);
            }

            this.UpdateFromControls(conflicting);
            return true;
        }

        private void UpdateFromControls(ICredentialSet toUpdate)
        {
            toUpdate.Domain = this.DomainTextbox.Text;
            toUpdate.Name = this.NameTextbox.Text;
            toUpdate.Username = this.UsernameTextbox.Text;
            if(this.PasswordTextbox.Text != NewTerminalForm.HIDDEN_PASSWORD)
                toUpdate.SecretKey = this.PasswordTextbox.Text;
        }

        private ICredentialSet CreateNewCredential()
        {
            var newItem = Credentials.CreateCredentialSet();
            UpdateFromControls(newItem);
            return newItem;
        }
    }
}
