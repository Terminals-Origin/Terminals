using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Common.Data.Interfaces;

namespace Terminals.Forms.Controls
{
    public partial class SecurityPanel : UserControl
    {
        private ISecurityService _securityService;

        //private IPersistence persistence;

        public bool PasswordLoaded { get { return this.credentialsPanel1.PasswordLoaded; } }

        public event Action<bool> SelectedCredentailChanged;

        public event EventHandler PasswordChanged
        {
            add { this.credentialsPanel1.PasswordChanged += value; }
            remove { this.credentialsPanel1.PasswordChanged -= value; }
        }

        public SecurityPanel()
        {
            InitializeComponent();
            this.credentialsPanel1.tableLayoutPanel1.Location = new Point(this.credentialDropdown.Location.X - 3, 0);
            this.credentialsPanel1.tableLayoutPanel1.Size = new Size(this.credentialDropdown.Width + 36, 80);
        }

        public void AssignServices(ISecurityService securityService, IMRUSettings settings)
        {
            this.credentialsPanel1.Settings = settings;
            _securityService = securityService;
            //this.persistence = persistence;
            //this.credentialsPanel1.Settings = settings;
        }

        private void CredentialDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var set = this.credentialDropdown.SelectedItem as ICredentialSet;
            bool hasSelectedCredential = set != null;

            if (hasSelectedCredential)
            {
                //var guarded = new GuardedCredential(set, this.persistence.Security);
                var guarded = _securityService.FromCredentialSet(set);
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

        public void SaveTo(ISecurityOptions security, bool savePassword)
        {
            ICredentialSet selectedCredential = this.credentialDropdown.SelectedItem as ICredentialSet;
            security.Credential = selectedCredential == null ? Guid.Empty : selectedCredential.Id;
            //var guarded = new GuardedSecurity(this.persistence, security);
            var guarded = _securityService.FromSecurityOption(security);
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

            //using (var mgr = new CredentialManager(this.persistence))
            //    mgr.ShowDialog();
            using (var mgr = _securityService.GetCredentialForm())
                mgr.ShowDialog();

            this.FillCredentialsCombobox(selectedCredentialId);
        }

        public void FillCredentialsCombobox(Guid credential)
        {
            this.credentialDropdown.Items.Clear();
            this.credentialDropdown.Items.Add("(custom)");
            this.FillCredentialsComboboxWithStoredCredentials();
            //this.credentialDropdown.SelectedItem = this.persistence.Credentials[credential];
            this.credentialDropdown.SelectedItem = _securityService.Credentials.FirstOrDefault(p => p.Id == credential);
        }

        private void FillCredentialsComboboxWithStoredCredentials()
        {
            //IEnumerable<ICredentialSet> credentials = this.persistence.Credentials;
            IEnumerable<ICredentialSet> credentials = _securityService.Credentials;
            if (credentials != null)
            {
                foreach (ICredentialSet item in credentials)
                {
                    this.credentialDropdown.Items.Add(item);
                }
            }
        }

        public void SaveMRUs()
        {
            this.credentialsPanel1.SaveMRUs();
        }

        public void LoadMRUs()
        {
            this.credentialsPanel1.LoadMRUs();
        }

        public void LoadFrom(ISecurityOptions security)
        {
            //var guarded = new GuardedSecurity(this.persistence, security);
            var guarded = _securityService.FromSecurityOption(security);
            this.credentialsPanel1.LoadFrom(guarded);
        }
    }
}
