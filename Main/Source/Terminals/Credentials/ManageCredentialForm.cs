using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Data.Validation;

namespace Terminals.Credentials
{
    internal partial class ManageCredentialForm : Form
    {
        private string editedCredentialName = string.Empty;

        private readonly IPersistence persistence;

        private readonly ICredentialSet editedCredential;

        private ICredentials Credentials
        {
            get { return this.persistence.Credentials; }
        }

        internal ManageCredentialForm(IPersistence persistence, ICredentialSet editedCredential)
        {
            InitializeComponent();

            this.persistence = persistence;
            this.editedCredential = editedCredential;
            this.credentialsPanel1.Settings = Settings.Instance;
            this.credentialsPanel1.LoadMRUs();
            FillControlsFromCredential();
        }

        private void FillControlsFromCredential()
        {
            if (this.editedCredential != null)
            {
                var guarded = new GuardedCredential(this.editedCredential, this.persistence.Security);
                this.credentialsPanel1.LoadFrom(guarded);
                this.NameTextbox.Text = editedCredential.Name;
                this.editedCredentialName = editedCredential.Name;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!this.ValidateNameAndUserName())
                return;

            if (UpdateCredential())
            {
                this.credentialsPanel1.SaveMRUs();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateNameAndUserName()
        {
            ICredentialSet prototype = this.CreateNewCredential();
            var results = Validations.Validate(prototype);
            string nameErrorMessage = results["Name"];
            this.errorProvider.SetError(this.NameTextbox, nameErrorMessage);
            // the validated object contains only encrypted properties.
            string userNameErrorMessage = results["EncryptedUserName"];
            this.credentialsPanel1.SetUserNameError(this.errorProvider, userNameErrorMessage);
            return results.Empty;
        }

        private bool UpdateCredential()
        {
            ICredentialSet conflicting = Credentials[this.NameTextbox.Text];
            bool hasConflicting = conflicting != null && !conflicting.Equals(this.editedCredential);
            
            if (hasConflicting && this.EditedNameHasChanged())
                return UpdateConflicting(conflicting, this.editedCredential);

            UpdateOldOrCreateNew();
            return true;
        }

        private void UpdateOldOrCreateNew()
        {
            if (this.editedCredential != null)
            {
                this.Update(this.editedCredential);
                return;
            }
            
            ICredentialSet newCredential = this.CreateNewCredential();
            Credentials.Add(newCredential);
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
                Credentials.Remove(oldItem);

            this.Update(conflicting);
            return true;
        }

        private void Update(ICredentialSet conflicting)
        {
            this.UpdateFromControls(conflicting);
            Credentials.Update(conflicting);
        }

        private void UpdateFromControls(ICredentialSet toUpdate)
        {
            toUpdate.Name = this.NameTextbox.Text;
            var guarded = new GuardedCredential(toUpdate, this.persistence.Security);
            this.credentialsPanel1.SaveTo(guarded);
        }

        private ICredentialSet CreateNewCredential()
        {
            var newItem = this.persistence.Factory.CreateCredentialSet();
            UpdateFromControls(newItem);
            return newItem;
        }
    }
}
