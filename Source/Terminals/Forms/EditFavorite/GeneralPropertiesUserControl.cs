using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Data.Validation;

namespace Terminals.Forms.EditFavorite
{
    internal partial class GeneralPropertiesUserControl : UserControl
    {
        internal string ServerNameText { get { return this.cmbServers.Text.Trim(); } }

        internal string ProtocolText { get { return this.ProtocolComboBox.Text; } }

        internal string PortText { get { return this.txtPort.Text; } }

        internal bool UrlVisible { get { return this.httpUrlTextBox.Visible; } }

        internal bool ShowOnToolbar { get { return this.chkAddtoToolbar.Checked; } }
        
        internal const String HIDDEN_PASSWORD = "****************";
        
        private String currentToolBarFileName;
        
        private NewTerminalFormValidator validator;

        internal event EventHandler SetOkButtonRequested;

        private RasControl rasControl;

        public event Action<string> ProtocolChanged;

        public event Action<string> ServerNameChanged;

        public GeneralPropertiesUserControl()
        {
            InitializeComponent();

            this.securityPanel1.PasswordChanged += TxtPassword_TextChanged;
            this.securityPanel1.SelectedCredentailChanged += this.SecurityPanel1_SelectedCredentailChanged;
        }

        private void SecurityPanel1_SelectedCredentailChanged(bool hasSelectedCredential)
        {
            this.chkSavePassword.Enabled = !hasSelectedCredential;
        }

        internal void RegisterValidations(NewTerminalFormValidator validator)
        {
            this.validator = validator;
            this.RegisterValiationControls();
            this.txtPort.Validating += this.validator.OnPortValidating;
            this.cmbServers.Validating += this.validator.OnServerNameValidating;
            this.httpUrlTextBox.Validating += this.validator.OnUrlValidating;
        }

        private void RegisterValiationControls()
        {
            this.validator.RegisterValidationControl("ServerName", this.cmbServers);
            this.validator.RegisterValidationControl(Validations.NAME_PROPERTY, this.txtName);
            this.validator.RegisterValidationControl("Port", this.txtPort);
            this.validator.RegisterValidationControl("Protocol", this.ProtocolComboBox);
            this.validator.RegisterValidationControl("Notes", this.NotesTextbox);
        }

        internal void SetErrorProviderIconsAlignment(ErrorProvider errorProvider)
        {
            errorProvider.SetIconAlignment(this.cmbServers, ErrorIconAlignment.MiddleLeft);
            errorProvider.SetIconAlignment(this.httpUrlTextBox, ErrorIconAlignment.MiddleLeft);
            errorProvider.SetIconAlignment(this.NotesTextbox, ErrorIconAlignment.TopLeft);
            errorProvider.SetIconAlignment(this.txtName, ErrorIconAlignment.MiddleLeft);
        }

        internal void AssignPersistence(IPersistence persistence)
        {
            this.securityPanel1.AssignPersistence(persistence);
        }

        internal void AssignRasControl(RasControl rasControl)
        {
            this.rasControl = rasControl;
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            this.SetOkButtonState();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = CreateIconSelectionDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    this.TryToAssignNewToolbarIcon(openFileDialog.FileName);
            }
        }

        private static OpenFileDialog CreateIconSelectionDialog()
        {
            var openFileDialog = new OpenFileDialog();
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

            if (this.ServerNameChanged != null)
                this.ServerNameChanged(this.ServerNameText);
        }

        private void CmbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rasControl.OnServerNameChanged(this.ProtocolText, this.ServerNameText);
            // Setting the data source resets the already load text, additionaly the RAS control doesnt work
            // this.cmbServers.DataSource = this.rasControl.ConnectionNames;
        }

        private void CmbServers_Leave(object sender, EventArgs e)
        {
            if (this.txtName.Text == String.Empty)
            {
                if (this.cmbServers.Text.Contains(":"))
                {
                    this.FillServerName(this.cmbServers.Text);
                }

                this.txtName.Text = this.cmbServers.Text;
            }
        }

        private static void GetServerAndPort(String connection, out String server, out Int32 port)
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
            if(validator != null)// designer support
                 this.validator.OnServerNameValidating(this.cmbServers, new CancelEventArgs());

            if (ConnectionManager.IsProtocolWebBased(this.ProtocolText))
                this.SetControlsForWeb();

            this.FireProtocolChanged();
            int defaultPort = ConnectionManager.GetPort(this.ProtocolText);
            this.txtPort.Text = defaultPort.ToString(CultureInfo.InvariantCulture);
            this.SetOkButtonState();
        }

        private void FireProtocolChanged()
        {
            if (this.ProtocolChanged != null)
                this.ProtocolChanged(this.ProtocolComboBox.Text);
        }

        private void SetControlsForWeb()
        {
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
            if (SetOkButtonRequested != null)
                SetOkButtonRequested(this, EventArgs.Empty);
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

        public void SaveTo(IFavorite favorite)
        {
            this.FillGeneralProrperties(favorite);
            this.securityPanel1.SaveTo(favorite.Security, this.chkSavePassword.Checked);
            this.FillDescriptionProperties(favorite);
        }

        private void FillDescriptionProperties(IFavorite favorite)
        {
            favorite.NewWindow = this.NewWindowCheckbox.Checked;
            favorite.ToolBarIconFile = this.currentToolBarFileName;
            favorite.Notes = this.NotesTextbox.Text;
        }

        private void FillGeneralProrperties(IFavorite favorite)
        {
            favorite.Name = (String.IsNullOrEmpty(this.txtName.Text) ? this.cmbServers.Text : this.txtName.Text);
            favorite.ServerName = this.cmbServers.Text;
            favorite.Protocol = this.ProtocolComboBox.Text;
            Int32 port;
            Int32.TryParse(this.PortText, out port);
            favorite.Port = port;
            WebOptions.UpdateFavoriteUrl(favorite, this.httpUrlTextBox.Text);
        }

        internal void FillCredentialsCombobox(Guid credentialGuid)
        {
            this.securityPanel1.FillCredentialsCombobox(credentialGuid);
        }

        public void LoadFrom(IFavorite favorite)
        {
            this.FillGeneralPropertiesControls(favorite);
            this.securityPanel1.LoadFrom(favorite.Security);
            this.chkSavePassword.Checked = this.securityPanel1.PasswordLoaded;
            this.FillDescriptionPropertiesControls(favorite);
            this.securityPanel1.FillCredentialsCombobox(favorite.Security.Credential);
        }

        private void FillGeneralPropertiesControls(IFavorite favorite)
        {
            this.txtName.Text = favorite.Name;
            this.cmbServers.Text = favorite.ServerName;
            this.ProtocolComboBox.SelectedItem = favorite.Protocol;
            this.txtPort.Text = favorite.Port.ToString(CultureInfo.InvariantCulture);
            this.httpUrlTextBox.Text = WebOptions.ExtractAbsoluteUrl(favorite);
        }

        private void FillDescriptionPropertiesControls(IFavorite favorite)
        {
            this.NewWindowCheckbox.Checked = favorite.NewWindow;
            this.chkAddtoToolbar.Checked = Settings.HasToolbarButton(favorite.Id);
            this.pictureBox2.Image = favorite.ToolBarIconImage;
            this.currentToolBarFileName = favorite.ToolBarIconFile;
            this.NotesTextbox.Text = favorite.Notes;
        }

        internal void LoadMRUs()
        {
            this.cmbServers.Items.AddRange(Settings.MRUServerNames);
            this.securityPanel1.LoadMRUs();
        }

        internal void SaveMRUs()
        {
            Settings.AddServerMRUItem(cmbServers.Text);
            this.securityPanel1.SaveMRUs();
        }

        public void FillServerName(string serverName)
        {
            String server;
            Int32 port;
            GetServerAndPort(serverName, out server, out port);
            this.cmbServers.Text = server;
            this.txtPort.Text = port.ToString(CultureInfo.InvariantCulture);
        }

        internal void ResetServerNameControls(string favoriteName)
        {
            this.txtName.Text = favoriteName + "_(copy)";
            this.cmbServers.Text = String.Empty;
            this.cmbServers.Focus();
        }

        internal void FocusServers()
        {
            this.cmbServers.Focus();
        }

        internal void AssingAvailablePlugins(string[] protocols)
        {
            this.ProtocolComboBox.DataSource = protocols;
            // for dynamicaly loaded plugins, this needs to be done later
            this.ProtocolComboBox.SelectedIndex = 0;
        }
    }
}
