using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Credentials;
using Terminals.Data;
using Terminals.Data.Credentials;

namespace Terminals.Forms.Controls
{
    internal partial class SecurityPanel : UserControl
    {
        private IPersistence persistence;

        internal bool PasswordLoaded { get { return this.credentialsPanel1.PasswordLoaded; } }

        internal event Action<bool> SelectedCredentailChanged;

        public event EventHandler PasswordChanged
        {
            add { this.credentialsPanel1.PasswordChanged += value; }
            remove { this.credentialsPanel1.PasswordChanged -= value; }
        }

        internal SecurityPanel()
        {
            InitializeComponent();
            this.credentialsPanel1.tableLayoutPanel1.Location = new Point(this.credentialDropdown.Location.X-3,0);
            this.credentialsPanel1.tableLayoutPanel1.Size = new Size(this.credentialDropdown.Width+36,80);
        }

        internal void AssignServices(IPersistence persistence, Settings settings)
        {
            this.persistence = persistence;
            this.credentialsPanel1.Settings = settings;
        }

        private void CredentialDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var set = this.credentialDropdown.SelectedItem as ICredentialSet;
            bool hasSelectedCredential = set != null;

            if (hasSelectedCredential)
            {
                var guarded = new GuardedCredential(set, this.persistence.Security);
                this.credentialsPanel1.LoadDirectlyFrom(guarded);
            }
            
            this.credentialsPanel1.Enabled = !hasSelectedCredential;
            this.FireSelectedCredentialChanged(hasSelectedCredential);
        }

        private void FireSelectedCredentialChanged(bool selectedCredential)
        {
            if (SelectedCredentailChanged != null)
                SelectedCredentailChanged(selectedCredential);
        }

        internal void SaveTo(ISecurityOptions security, bool savePassword)
        {
            ICredentialSet selectedCredential = this.credentialDropdown.SelectedItem as ICredentialSet;
            security.Credential = selectedCredential == null ? Guid.Empty : selectedCredential.Id;
            var guarded = new GuardedSecurity(this.persistence.Security, security);
            this.credentialsPanel1.SaveUserAndDomain(guarded);

            if (savePassword)
                this.credentialsPanel1.SavePassword(guarded);
            else
                security.EncryptedPassword = String.Empty;
        }

        private void CredentialManagerPicturebox_Click(object sender, EventArgs e)
        {
            // backup previously selected item
            Guid selectedCredentialId = Guid.Empty;
            var selectedCredential = this.credentialDropdown.SelectedItem as ICredentialSet;
            if (selectedCredential != null)
                selectedCredentialId = selectedCredential.Id;

            using (var mgr = new CredentialManager(this.persistence))
                mgr.ShowDialog();

            this.FillCredentialsCombobox(selectedCredentialId);
        }

        internal void FillCredentialsCombobox(Guid credential)
        {
            this.credentialDropdown.Items.Clear();
            this.credentialDropdown.Items.Add("(custom)");
            this.FillCredentialsComboboxWithStoredCredentials();
            this.credentialDropdown.SelectedItem = this.persistence.Credentials[credential];
        }

        private void FillCredentialsComboboxWithStoredCredentials()
        {
            IEnumerable<ICredentialSet> credentials = this.persistence.Credentials;
            if (credentials != null)
            {
                foreach (ICredentialSet item in credentials)
                {
                    this.credentialDropdown.Items.Add(item);
                }
            }
        }

        internal void SaveMRUs()
        {
            this.credentialsPanel1.SaveMRUs();
        }

        internal void LoadMRUs()
        {
            this.credentialsPanel1.LoadMRUs();
        }

        internal void LoadFrom(ISecurityOptions security)
        {
            var guarded = new GuardedSecurity(this.persistence.Security, security);
            this.credentialsPanel1.LoadFrom(guarded);
        }
    }
}
