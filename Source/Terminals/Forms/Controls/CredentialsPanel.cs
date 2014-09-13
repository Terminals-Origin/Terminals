using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    internal partial class CredentialsPanel : UserControl
    {
        private String favoritePassword = string.Empty;
        internal const String HIDDEN_PASSWORD = "****************";

        internal bool PasswordLoaded { get; private set; }

        internal event EventHandler PasswordChanged
        {
            add { this.txtPassword.TextChanged += value; }
            remove { this.txtPassword.TextChanged -= value; }
        }

        public int TextEditsLeft
        {
            get
            {
                return this.cmbUsers.Left;
            }
            set
            {
                int newWidth = this.cmbUsers.Left + this.cmbUsers.Width - value; 
                this.cmbUsers.Left = value;
                this.cmbDomains.Left = value;
                this.txtPassword.Left = value;
                this.cmbUsers.Width = newWidth;
                this.cmbDomains.Width = newWidth;
                this.txtPassword.Width = newWidth;
            }
        }

        internal CredentialsPanel()
        {
            InitializeComponent();
        }

        internal void SetUserNameError(ErrorProvider errorProvider, string errroMessage)
        {
            errorProvider.SetIconAlignment(this.cmbUsers, ErrorIconAlignment.MiddleRight);
            errorProvider.SetError(this.cmbUsers, errroMessage);
        }

        internal void LoadMRUs()
        {
            this.cmbDomains.Items.AddRange(Settings.MRUDomainNames);
            this.cmbUsers.Items.AddRange(Settings.MRUUserNames);
        }

        internal void SaveMRUs()
        {
            Settings.AddDomainMRUItem(cmbDomains.Text);
            Settings.AddUserMRUItem(cmbUsers.Text);
        }

        internal void LoadDirectlyFrom(ICredentialBase security)
        {
            this.LoadDomainAndUser(security);
            // here dont affect stored password
            this.txtPassword.Text = security.Password;
        }

        internal void LoadFrom(ICredentialBase security)
        {
            this.LoadDomainAndUser(security);
            this.favoritePassword = security.Password;
            this.CheckEncryptedPassword(security);
            this.LoadPassword();
        }

        private void LoadDomainAndUser(ICredentialBase security)
        {
            this.cmbDomains.Text = security.Domain;
            this.cmbUsers.Text = security.UserName;
        }

        private void CheckEncryptedPassword(ICredentialBase security)
        {
            if (!string.IsNullOrEmpty(this.favoritePassword) || string.IsNullOrEmpty(security.EncryptedPassword))
                return;

            MessageBox.Show("There was an issue with decrypting your password.\n\nPlease provide a new password and save the favorite.");
            this.txtPassword.Text = string.Empty;
            this.favoritePassword = string.Empty;
            this.txtPassword.Focus();
            security.Password = string.Empty;
        }

        private void LoadPassword()
        {
            this.PasswordLoaded = !string.IsNullOrEmpty(this.favoritePassword);
            if (this.PasswordLoaded)
                this.txtPassword.Text = HIDDEN_PASSWORD;
            else
                this.txtPassword.Text = string.Empty;
        }

        internal void SaveUserAndDomain(ICredentialBase security)
        {
            security.Domain = this.cmbDomains.Text;
            security.UserName = this.cmbUsers.Text;
        }

        internal void SavePassword(ICredentialBase security)
        {
            if (this.txtPassword.Text != HIDDEN_PASSWORD)
                security.Password = this.txtPassword.Text;
            else
                security.Password = this.favoritePassword;
        }
    }
}

