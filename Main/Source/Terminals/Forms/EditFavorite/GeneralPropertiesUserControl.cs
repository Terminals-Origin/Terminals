using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Credentials;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    public partial class GeneralPropertiesUserControl : UserControl
    {
        private String currentToolBarFileName;
        // todo initialize validator, rasControl and Persistence
        private NewTerminalFormValidator validator;
        private RasControl rasControl;
        private IPersistence persistence;

        public GeneralPropertiesUserControl()
        {
            InitializeComponent();
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            this.SetOkButtonState();
        }

        private void CredentialDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CredentialsPanel.Enabled = true;
            ICredentialSet set = this.CredentialDropdown.SelectedItem as ICredentialSet;

            if (set != null)
            {
                this.CredentialsPanel.Enabled = false;
                this.cmbDomains.Text = set.Domain;
                this.cmbUsers.Text = set.UserName;
                this.txtPassword.Text = set.Password;
                this.chkSavePassword.Checked = true;
            }
        }

        private void CredentialManagerPicturebox_Click(object sender, EventArgs e)
        {
            Guid selectedCredentialId = Guid.Empty;
            var selectedCredential = this.CredentialDropdown.SelectedItem as ICredentialSet;
            if (selectedCredential != null)
                selectedCredentialId = selectedCredential.Id;

            using (var mgr = new CredentialManager(this.persistence))
                mgr.ShowDialog();

            this.FillCredentialsCombobox(selectedCredentialId);
        }

        private void FillCredentialsCombobox(Guid credential)
        {
            this.CredentialDropdown.Items.Clear();
            this.CredentialDropdown.Items.Add("(custom)");
            this.FillCredentialsComboboxWithStoredCredentials();
            this.CredentialDropdown.SelectedItem = this.persistence.Credentials[credential];
        }

        private void FillCredentialsComboboxWithStoredCredentials()
        {
            IEnumerable<ICredentialSet> credentials = this.persistence.Credentials;
            if (credentials != null)
            {
                foreach (ICredentialSet item in credentials)
                {
                    this.CredentialDropdown.Items.Add(item);
                }
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = CreateIconSelectionDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.TryToAssignNewToolbarIcon(openFileDialog.FileName);
            }
        }

        private static OpenFileDialog CreateIconSelectionDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.InitialDirectory = FileLocations.ThumbsDirectoryFullPath;
            openFileDialog.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select Custom Terminal Image .";
            return openFileDialog;
        }

        private void TryToAssignNewToolbarIcon(string newImagefilePath)
        {
            try
            {
                this.TryToLoadSelectedImage(newImagefilePath);
            }
            catch (Exception exception)
            {
                Logging.Info("Set Terminal Image Failed", exception);
                this.currentToolBarFileName = String.Empty;
                MessageBox.Show("You have chosen an invalid image. Try again.");
            }
        }

        private void TryToLoadSelectedImage(string newImagefilePath)
        {
            string newFileInThumbsDir = FavoriteIcons.CopySelectedImageToThumbsDir(newImagefilePath);
            this.pictureBox2.Image = Image.FromFile(newImagefilePath);
            this.currentToolBarFileName = newFileInThumbsDir;
        }

        private void CmbServers_TextChanged(object sender, EventArgs e)
        {
            this.SetOkButtonState();
        }

        private void CmbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProtocolComboBox.Text == ConnectionManager.RAS)
                this.rasControl.FillRasControls(this.cmbServers.Text);
        }

        private void CmbServers_Leave(object sender, EventArgs e)
        {
            if (this.txtName.Text == String.Empty)
            {
                if (this.cmbServers.Text.Contains(":"))
                {
                    String server = String.Empty;
                    int port;
                    GetServerAndPort(this.cmbServers.Text, out server, out port);
                    this.cmbServers.Text = server;
                    this.txtPort.Text = port.ToString(CultureInfo.InvariantCulture);
                    this.cmbServers.Text = server;
                }

                this.txtName.Text = this.cmbServers.Text;
            }
        }

        internal static void GetServerAndPort(String connection, out String server, out Int32 port)
        {
            server = connection;
            port = ConnectionManager.RDPPort;
            if (connection != null && connection.Trim() != String.Empty && connection.Contains(":"))
            {
                int separatorIndex = connection.IndexOf(":", StringComparison.Ordinal);
                String parsedServer = connection.Substring(0, separatorIndex);
                String rawPort = connection.Substring(separatorIndex + 1);
                Int32 parsedPort = ConnectionManager.RDPPort;
                if (rawPort.Trim() != String.Empty)
                {
                    rawPort = rawPort.Trim();
                    Int32.TryParse(rawPort, out parsedPort);
                }

                server = parsedServer;
                port = parsedPort;
            }
        }

        private void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetControlsProtocolIndependent();
            this.validator.OnServerNameValidating(this.cmbServers, new CancelEventArgs());

            //todo switch (this.ProtocolComboBox.Text)
            //{
            //    case ConnectionManager.RDP:
            //        this.SetControlsForRdp();
            //        break;
            //    case ConnectionManager.VMRC:
            //        this.VmrcGroupBox.Enabled = true;
            //        break;
            //    case ConnectionManager.RAS:
                      this.cmbServers.Items.Clear();
                      this.txtPort.Enabled = false;
            //        this.SetControlsForRas();
            //        break;
            //    case ConnectionManager.VNC:
            //        this.VncGroupBox.Enabled = true;
            //        break;
            //    case ConnectionManager.ICA_CITRIX:
            //        this.IcaGroupBox.Enabled = true;
            //        break;
            //    case ConnectionManager.HTTP:
            //    case ConnectionManager.HTTPS:
                      this.SetControlsForWeb();
            //        break;
            //    case ConnectionManager.SSH:
            //    case ConnectionManager.TELNET:
            //        this.ConsoleGroupBox.Enabled = true;
            //        if (this.ProtocolComboBox.Text == ConnectionManager.SSH)
            //            this.SshGroupBox.Enabled = true;
            //        break;
            //}

            int defaultPort = ConnectionManager.GetPort(this.ProtocolComboBox.Text);
            this.txtPort.Text = defaultPort.ToString(CultureInfo.InvariantCulture);
            this.SetOkButtonState();
        }

        private void SetControlsForWeb()
        {
            // todo how to extract WebControls from general properties
            this.lblServerName.Text = "Url:";
            this.cmbServers.Enabled = false;
            this.txtPort.Enabled = false;
            this.httpUrlTextBox.Enabled = true;
            this.httpUrlTextBox.Visible = true;
        }

        private void SetControlsProtocolIndependent()
        {
            this.lblServerName.Text = "Computer:";
            this.cmbServers.Enabled = true;
            this.txtPort.Enabled = true;
            this.httpUrlTextBox.Enabled = false;
            this.httpUrlTextBox.Visible = false;
            this.txtPort.Enabled = true;
        }

        private void HttpUrlTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ConnectionManager.IsProtocolWebBased(this.ProtocolComboBox.Text))
            {
                Uri newUrl = GetFullUrlFromHttpTextBox();
                if (newUrl != null)
                {
                    this.cmbServers.Text = newUrl.Host;
                    this.txtPort.Text = newUrl.Port.ToString(CultureInfo.InvariantCulture);
                }
                
                SetOkButtonState();  
            }
        }

        private void SetOkButtonState()
        {
            // todo replace SetOkButtonState with event handler
        }

        internal Uri GetFullUrlFromHttpTextBox()
        {
            string newUrlText = this.httpUrlTextBox.Text.ToLower();
            string protocolPrefix = this.ProtocolComboBox.Text.ToLower();

            if (!newUrlText.StartsWith(ConnectionManager.HTTP.ToLower()) &&
                !newUrlText.StartsWith(ConnectionManager.HTTPS.ToLower()))
                newUrlText = String.Format("{0}://{1}", protocolPrefix, newUrlText);
            return WebOptions.TryParseUrl(newUrlText);
        }          
    }
}
