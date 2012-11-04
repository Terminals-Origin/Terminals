using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Credentials
{
    internal partial class ManageCredentialForm : Form
    {
        private string editedCredentialName = "";
        private readonly ICredentialSet editedCredential;

        private static ICredentials Credentials
        {
            get { return Persistence.Instance.Credentials; }
        }

        internal ManageCredentialForm(ICredentialSet editedCredential)
        {
            InitializeComponent();

            this.editedCredential = editedCredential;
            FillControlsFromCredential();
        }

        private void FillControlsFromCredential()
        {
            if (this.editedCredential != null)
            {
                this.NameTextbox.Text = editedCredential.Name;
                this.DomainTextbox.Text = editedCredential.Domain;
                this.UsernameTextbox.Text = editedCredential.UserName;
                if(!string.IsNullOrEmpty(editedCredential.EncryptedPassword))
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
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool UpdateCredential()
        {
            ICredentialSet conflicting = Credentials[this.NameTextbox.Text];
            if (conflicting != null && !conflicting.Equals(this.editedCredential) &&
                EditedNameHasChanged())
            {
                return UpdateConflicting(conflicting, this.editedCredential);
            }

            UpdateOldOrCreateNew();
            return true;
        }

        private void UpdateOldOrCreateNew()
        {
            bool nameChanded = EditedNameHasChanged();
            if (this.editedCredential == null || nameChanded)
            {
                ICredentialSet newCredential = this.CreateNewCredential();
                Credentials.Add(newCredential);
            }
            else
            {
                this.UpdateFromControls(this.editedCredential);
                Credentials.Update(this.editedCredential);
            }
        }

        private bool EditedNameHasChanged()
        {
            return !string.Equals(this.editedCredentialName, this.NameTextbox.Text,
                StringComparison.CurrentCultureIgnoreCase);
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
            toUpdate.UserName = this.UsernameTextbox.Text;
            if(this.PasswordTextbox.Text != NewTerminalForm.HIDDEN_PASSWORD)
                toUpdate.Password = this.PasswordTextbox.Text;
        }

        private ICredentialSet CreateNewCredential()
        {
            var newItem = Persistence.Instance.Factory.CreateCredentialSet();
            UpdateFromControls(newItem);
            return newItem;
        }
    }
}
