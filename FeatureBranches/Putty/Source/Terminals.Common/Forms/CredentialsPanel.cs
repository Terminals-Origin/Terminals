using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    public partial class CredentialsPanel : UserControl
    {
        public IMRUSettings Settings { get; set; }

        private String favoritePassword = string.Empty;

        public const String HIDDEN_PASSWORD = "****************";

        public bool PasswordLoaded { get; private set; }

        public event EventHandler PasswordChanged
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

        public CredentialsPanel()
        {
            InitializeComponent();
            this.revealPwdButton.Click += this.RevealOrHidePwd;
        }

        public void SetUserNameError(ErrorProvider errorProvider, string errroMessage)
        {
            errorProvider.SetIconAlignment(this.cmbUsers, ErrorIconAlignment.MiddleLeft);
            errorProvider.SetError(this.cmbUsers, errroMessage);
        }

        public void LoadMRUs()
        {
            this.cmbDomains.Items.AddRange(this.Settings.MRUDomainNames);
            this.cmbUsers.Items.AddRange(this.Settings.MRUUserNames);
        }

        public void SaveMRUs()
        {
            this.Settings.AddDomainMRUItem(cmbDomains.Text);
            this.Settings.AddUserMRUItem(cmbUsers.Text);
        }

        public void LoadDirectlyFrom(IGuardedCredential security)
        {
            this.LoadDomainAndUser(security);
            // here dont affect stored password
            this.txtPassword.Text = security.Password;
        }

        public void LoadFrom(IGuardedCredential security)
        {
            this.LoadDomainAndUser(security);
            this.favoritePassword = security.Password;
            this.CheckEncryptedPassword(security);
            this.LoadPassword();
        }

        private void LoadDomainAndUser(IGuardedCredential security)
        {
            this.cmbDomains.Text = security.Domain;
            this.cmbUsers.Text = security.UserName;
        }

        private void CheckEncryptedPassword(IGuardedCredential security)
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
            this.txtPassword.Text = this.favoritePassword;
        }

        public void SaveTo(IGuardedCredential security)
        {
            this.SaveUserAndDomain(security);
            this.SavePassword(security);
        }

        public void SaveUserAndDomain(IGuardedCredential security)
        {
            security.Domain = this.cmbDomains.Text;
            security.UserName = this.cmbUsers.Text;
        }

        public void SavePassword(IGuardedCredential security)
        {
            if (this.txtPassword.Text != HIDDEN_PASSWORD)
                security.Password = this.txtPassword.Text;
            else
                security.Password = this.favoritePassword;
        }

        internal void RevealOrHidePwd(object sender, EventArgs e)
        {
            if (this.revealPwdButton.ImageIndex==1)
            {
                this.txtPassword.PasswordChar = '*';
                this.revealPwdButton.ImageIndex = 0;
            }
            else
            {
                this.txtPassword.PasswordChar = '\0';
                this.revealPwdButton.ImageIndex = 1;
            }
        }
    }
}

