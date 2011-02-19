using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using FalafelSoftware.TransPort;
using Terminals.Properties;
using Terminals.Network.Servers;

namespace Terminals
{
    internal partial class NewTerminalForm : Form
    {
        private TerminalServerManager _terminalServerManager = new TerminalServerManager();
        private Dictionary<string, RASENTRY> _dialupList = new Dictionary<string, RASENTRY>();
        private FavoriteConfigurationElement _favorite;
        private bool _showOnToolbar;
        private string _currentToolBarFileName;
        private string _oldName;
        internal List<string> _redirectedDrives = new List<string>();
        internal bool _redirectDevices = false;

        #region Public
        public NewTerminalForm(string server, bool connect)
        {
            InitializeComponent();
            Init(null, server, connect);
        }
        public NewTerminalForm(FavoriteConfigurationElement favorite)
        {
            InitializeComponent();
            Init(favorite, "", false);
        }
        public FavoriteConfigurationElement Favorite
        {
            get
            {
                return _favorite;
            }
        }
        public bool ShowOnToolbar
        {
            get
            {
                return _showOnToolbar;
            }
        }
        #endregion

        #region Private Developermade
        private void Init(FavoriteConfigurationElement favorite, string server, bool connect)
        {
            LoadMRUs();
            SetOkButtonState();
            SetOkTitle(connect);
            SSHPreferences.Keys = Settings.SSHKeys;

            // move following line down to default value only once smart card access worked out.
            cmbTSGWLogonMethod.SelectedIndex = 0;

            if(favorite == null)
            {
                FillCredentials(null);

                FavoriteConfigurationElement defaultFav = Settings.GetDefaultFavorite();
                if (defaultFav != null)
                {
                    FillControls(defaultFav);
                }
                else
                {
                    cmbResolution.SelectedIndex = 7;
                    cmbColors.SelectedIndex = 1;
                    cmbSounds.SelectedIndex = 2;
                    ProtocolComboBox.SelectedIndex = 0;
                }
                string Server = server;
                int port = 3389;
                GetServerAndPort(server, out Server, out port);
                cmbServers.Text = Server;
                txtPort.Text = port.ToString();
            }
            else
            {
                _oldName = favorite.Name;
                this.Text = "Edit Connection";
                FillControls(favorite);                
            }
        }
        private void FillCredentials(string CredentialName)
        {
            this.CredentialDropdown.Items.Clear();
            List<Credentials.CredentialSet> creds = Settings.SavedCredentials;
            this.CredentialDropdown.Items.Add("(custom)");

            int selIndex = 0;
            if (creds != null)
            {
                foreach (Credentials.CredentialSet item in creds)
                {
                    int index = this.CredentialDropdown.Items.Add(item);
                    if (!string.IsNullOrEmpty(CredentialName) && CredentialName == item.Name)
                        selIndex = index;
                }
            }
            this.CredentialDropdown.SelectedIndex = selIndex;
        }
        private void NewTerminalForm_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this._terminalServerManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this._terminalServerManager.Location = new System.Drawing.Point(0, 0);
            this._terminalServerManager.Name = "terminalServerManager1";
            this._terminalServerManager.Size = new System.Drawing.Size(748, 309);
            this._terminalServerManager.TabIndex = 0;
            this.tabPage10.Controls.Add(_terminalServerManager);

            foreach (string tag in Settings.Tags)
            {
                ListViewItem lvi = new ListViewItem(tag);
                this.AllTagsListView.Items.Add(lvi);
            }

            this.ResumeLayout(true);
        }
        private void LoadDialupConnections()
        {
            _dialupList = new Dictionary<string, RASENTRY>();
            System.Collections.ArrayList rasEntries = new System.Collections.ArrayList();
            RasError error = ras1.ListEntries(ref rasEntries);
            foreach(string item in rasEntries)
            {
                RASENTRY details = new RASENTRY();
                ras1.GetEntry(item, ref details);
                _dialupList.Add(item, details);
                if(!cmbServers.Items.Contains(item))
                    this.cmbServers.Items.Add(item);
            }
        }
        private void SetOkTitle(bool connect)
        {
            if(connect)
                btnOk.Text = "Co&nnect";
            else
                btnOk.Text = "OK";
        }        
        private void LoadMRUs()
        {
            cmbServers.Items.AddRange(Settings.MRUServerNames);
            cmbDomains.Items.AddRange(Settings.MRUDomainNames);
            cmbUsers.Items.AddRange(Settings.MRUUserNames);
            txtTag.AutoCompleteCustomSource.AddRange(Settings.Tags);
        }
        private void SaveMRUs()
        {
            Settings.AddServerMRUItem(cmbServers.Text);
            Settings.AddDomainMRUItem(cmbDomains.Text);
            Settings.AddUserMRUItem(cmbUsers.Text);
        }
        private void FillControls(FavoriteConfigurationElement favorite)
        {
            consolePreferences.FillControls(favorite);

            this.NewWindowCheckbox.Checked = favorite.NewWindow;

            ProtocolComboBox.SelectedItem = favorite.Protocol;
            VMRCAdminModeCheckbox.Checked = favorite.VMRCAdministratorMode;

            vncAutoScaleCheckbox.Checked = favorite.VncAutoScale;
            vncDisplayNumberInput.Value = favorite.VncDisplayNumber;
            VncViewOnlyCheckbox.Checked = favorite.VncViewOnly;

            VMRCReducedColorsCheckbox.Checked = favorite.VMRCReducedColorsMode;
            txtName.Text = favorite.Name;
            cmbServers.Text = favorite.ServerName;
            cmbDomains.Text = favorite.DomainName;
            cmbUsers.Text = favorite.UserName;
            txtPassword.Text = favorite.Password;
            chkSavePassword.Checked = favorite.Password != "";
            cmbResolution.SelectedIndex = (int)favorite.DesktopSize;
            cmbColors.SelectedIndex = (int)favorite.Colors;
            chkConnectToConsole.Checked = favorite.ConnectToConsole;

            chkAddtoToolbar.Checked = Settings.HasToolbarButton(favorite.Name);
            _redirectedDrives = favorite.RedirectedDrives;
            chkSerialPorts.Checked = favorite.RedirectPorts;
            chkPrinters.Checked = favorite.RedirectPrinters;
            chkRedirectClipboard.Checked = favorite.RedirectClipboard;
            _redirectDevices = favorite.RedirectDevices;
            chkRedirectSmartcards.Checked = favorite.RedirectSmartCards;
            cmbSounds.SelectedIndex = (int)favorite.Sounds;

            switch (favorite.TsgwUsageMethod)
            {
                case 0:
                    radTSGWdisable.Checked = true;
                    chkTSGWlocalBypass.Checked = false;
                    break;
                case 1:
                    radTSGWenable.Checked = true;
                    chkTSGWlocalBypass.Checked = false;
                    break;
                case 2:
                    radTSGWenable.Checked = true;
                    chkTSGWlocalBypass.Checked = true;
                    break;
                case 4:
                    radTSGWdisable.Checked = true;
                    chkTSGWlocalBypass.Checked = true;
                    break;
            }
            txtTSGWServer.Text = favorite.TsgwHostname;
            txtTSGWDomain.Text = favorite.TsgwDomain;
            txtTSGWUserName.Text = favorite.TsgwUsername;
            txtTSGWPassword.Text = favorite.TsgwPassword;
            chkTSGWlogin.Checked = favorite.TsgwSeparateLogin;
            cmbTSGWLogonMethod.SelectedIndex = favorite.TsgwCredsSource;

            txtPort.Text = favorite.Port.ToString();
            txtDesktopShare.Text = favorite.DesktopShare;
            chkExecuteBeforeConnect.Checked = favorite.ExecuteBeforeConnect;
            txtCommand.Text = favorite.ExecuteBeforeConnectCommand;
            txtArguments.Text = favorite.ExecuteBeforeConnectArgs;
            txtInitialDirectory.Text = favorite.ExecuteBeforeConnectInitialDirectory;
            chkWaitForExit.Checked = favorite.ExecuteBeforeConnectWaitForExit;

            string[] tagsArray = favorite.Tags.Split(',');
            if(!((tagsArray.Length == 1) && (String.IsNullOrEmpty(tagsArray[0]))))
            {
                foreach(string tag in tagsArray)
                {
                    lvConnectionTags.Items.Add(tag, tag, -1);
                }
            }

            if(favorite.ToolBarIcon != null && System.IO.File.Exists(favorite.ToolBarIcon))
            {
                this.pictureBox2.Load(favorite.ToolBarIcon);
                this._currentToolBarFileName = favorite.ToolBarIcon;
            }

            //extended settings
            this.ShutdownTimeoutTextBox.Text = favorite.ShutdownTimeout.ToString();
            this.OverallTimeoutTextbox.Text = favorite.OverallTimeout.ToString();
            this.SingleTimeOutTextbox.Text = favorite.ConnectionTimeout.ToString();
            this.IdleTimeoutMinutesTextBox.Text = favorite.IdleTimeout.ToString();
            this.SecurityWorkingFolderTextBox.Text = favorite.SecurityWorkingFolder;
            this.SecuriytStartProgramTextbox.Text = favorite.SecurityStartProgram;
            this.SecurityStartFullScreenCheckbox.Checked = favorite.SecurityFullScreen;
            this.SecuritySettingsEnabledCheckbox.Checked = favorite.EnableSecuritySettings;
            this.GrabFocusOnConnectCheckbox.Checked = favorite.GrabFocusOnConnect;
            this.EnableEncryptionCheckbox.Checked = favorite.EnableEncryption;
            this.DisableWindowsKeyCheckbox.Checked = favorite.DisableWindowsKey;
            this.DetectDoubleClicksCheckbox.Checked = favorite.DoubleClickDetect;
            this.DisplayConnectionBarCheckbox.Checked = favorite.DisplayConnectionBar;
            this.DisableControlAltDeleteCheckbox.Checked= favorite.DisableControlAltDelete;
            this.AcceleratorPassthroughCheckBox.Checked = favorite.AcceleratorPassthrough;
            this.EnableCompressionCheckbox.Checked = favorite.EnableCompression;
            this.EnableBitmapPersistanceCheckbox.Checked = favorite.BitmapPeristence;
            this.EnableTLSAuthenticationCheckbox.Checked = favorite.EnableTLSAuthentication;
            this.EnableNLAAuthenticationCheckbox.Checked = favorite.EnableNLAAuthentication;
            this.AllowBackgroundInputCheckBox.Checked = favorite.AllowBackgroundInput;

            chkDisableCursorShadow.Checked = false;
            chkDisableCursorBlinking.Checked = false;
            chkDisableFullWindowDrag.Checked = false;
            chkDisableMenuAnimations.Checked = false;
            chkDisableThemes.Checked = false;
            chkDisableWallpaper.Checked = false;

            if(favorite.PerformanceFlags > 0)
            {
                chkDisableCursorShadow.Checked = favorite.DisableCursorShadow;
                chkDisableCursorBlinking.Checked = favorite.DisableCursorBlinking;
                chkDisableFullWindowDrag.Checked = favorite.DisableFullWindowDrag;
                chkDisableMenuAnimations.Checked = favorite.DisableMenuAnimations;
                chkDisableThemes.Checked = favorite.DisableTheming;
                chkDisableWallpaper.Checked = favorite.DisableWallPaper;
                AllowFontSmoothingCheckbox.Checked = favorite.EnableFontSmoothing;
                AllowDesktopCompositionCheckbox.Checked = favorite.EnableDesktopComposition;
            }
            this.widthUpDown.Value = (decimal)favorite.DesktopSizeWidth;
            this.heightUpDown.Value = (decimal)favorite.DesktopSizeHeight;

            httpUrlTextBox.Text = favorite.Url;

            ICAClientINI.Text = favorite.IcaClientINI;
            ICAServerINI.Text = favorite.IcaServerINI;
            ICAEncryptionLevelCombobox.Text = favorite.IcaEncryptionLevel;
            ICAEnableEncryptionCheckbox.Checked = favorite.IcaEnableEncryption;
            ICAEncryptionLevelCombobox.Enabled = ICAEnableEncryptionCheckbox.Checked;

            NotesTextbox.Text = favorite.Notes;

            SSHPreferences.AuthMethod = favorite.AuthMethod;
            SSHPreferences.KeyTag = favorite.KeyTag;
            SSHPreferences.SSH1 = favorite.SSH1;

            FillCredentials(favorite.Credential);
        }
        private bool FillFavorite(bool defaultFav)
        {
            try {
                if (_favorite == null)
                    _favorite = new FavoriteConfigurationElement();
                
                consolePreferences.FillFavorite(_favorite);

                //if (defaultFav)
                //    _favorite.Name = "default";
                //else
                    _favorite.Name = (string.IsNullOrEmpty(txtName.Text) ? cmbServers.Text : txtName.Text);

                _favorite.VMRCAdministratorMode = VMRCAdminModeCheckbox.Checked;
                _favorite.VMRCReducedColorsMode = VMRCReducedColorsCheckbox.Checked;

                _favorite.VncAutoScale = vncAutoScaleCheckbox.Checked;
                _favorite.VncDisplayNumber = (int)vncDisplayNumberInput.Value;
                _favorite.VncViewOnly = VncViewOnlyCheckbox.Checked;

                _favorite.NewWindow = this.NewWindowCheckbox.Checked;

                _favorite.Protocol = ProtocolComboBox.SelectedItem.ToString();
                if (!defaultFav)
                    _favorite.ServerName = ValidateServer(cmbServers.Text);

                Credentials.CredentialSet set = (CredentialDropdown.SelectedItem as Credentials.CredentialSet);
                _favorite.Credential = (set == null ? "" : set.Name);

                _favorite.DomainName = cmbDomains.Text;
                _favorite.UserName = cmbUsers.Text;
                _favorite.Password = (chkSavePassword.Checked ? txtPassword.Text : "");
                
                _favorite.DesktopSize = (DesktopSize)cmbResolution.SelectedIndex;
                _favorite.Colors = (Colors)cmbColors.SelectedIndex;
                _favorite.ConnectToConsole = chkConnectToConsole.Checked;
                _favorite.DisableWallPaper = chkDisableWallpaper.Checked;
                _favorite.DisableCursorBlinking = chkDisableCursorBlinking.Checked;
                _favorite.DisableCursorShadow = chkDisableCursorShadow.Checked;
                _favorite.DisableFullWindowDrag = chkDisableFullWindowDrag.Checked;
                _favorite.DisableMenuAnimations = chkDisableMenuAnimations.Checked;
                _favorite.DisableTheming = chkDisableThemes.Checked;

                _favorite.RedirectedDrives = _redirectedDrives;
                _favorite.RedirectPorts = chkSerialPorts.Checked;
                _favorite.RedirectPrinters = chkPrinters.Checked;
                _favorite.RedirectClipboard = chkRedirectClipboard.Checked;
                _favorite.RedirectDevices = _redirectDevices;
                _favorite.RedirectSmartCards = chkRedirectSmartcards.Checked;
                _favorite.Sounds = (RemoteSounds)cmbSounds.SelectedIndex;
                _showOnToolbar = chkAddtoToolbar.Checked;

                if (radTSGWenable.Checked)
                {
                    if (chkTSGWlocalBypass.Checked)
                        _favorite.TsgwUsageMethod = 2;
                    else
                        _favorite.TsgwUsageMethod = 1;
                }
                else
                {
                    if (chkTSGWlocalBypass.Checked)
                        _favorite.TsgwUsageMethod = 4;
                    else
                        _favorite.TsgwUsageMethod = 0;
                }
                _favorite.TsgwHostname = txtTSGWServer.Text;
                _favorite.TsgwDomain = txtTSGWDomain.Text;
                _favorite.TsgwUsername = txtTSGWUserName.Text;
                _favorite.TsgwPassword = txtTSGWPassword.Text;
                _favorite.TsgwSeparateLogin = chkTSGWlogin.Checked;
                _favorite.TsgwCredsSource = cmbTSGWLogonMethod.SelectedIndex;

                _favorite.Port = ValidatePort(txtPort.Text);
                _favorite.DesktopShare = txtDesktopShare.Text;
                _favorite.ExecuteBeforeConnect = chkExecuteBeforeConnect.Checked;
                _favorite.ExecuteBeforeConnectCommand = txtCommand.Text;
                _favorite.ExecuteBeforeConnectArgs = txtArguments.Text;
                _favorite.ExecuteBeforeConnectInitialDirectory = txtInitialDirectory.Text;
                _favorite.ExecuteBeforeConnectWaitForExit = chkWaitForExit.Checked;
                _favorite.ToolBarIcon = _currentToolBarFileName;

                _favorite.ICAApplicationName = ICAApplicationNameTextBox.Text;
                _favorite.ICAApplicationPath = ICAApplicationPath.Text;
                _favorite.ICAApplicationWorkingFolder = ICAWorkingFolder.Text;

                List<string> tagList = new List<string>();
                foreach(ListViewItem listViewItem in lvConnectionTags.Items) {
                    tagList.Add(listViewItem.Text);
                }
                _favorite.Tags = String.Join(",", tagList.ToArray());


                //extended settings
                if(ShutdownTimeoutTextBox.Text.Trim() != "") {
                    _favorite.ShutdownTimeout = Convert.ToInt32(ShutdownTimeoutTextBox.Text.Trim());
                }
                
                if(OverallTimeoutTextbox.Text.Trim() != "") {
                    _favorite.OverallTimeout = Convert.ToInt32(OverallTimeoutTextbox.Text.Trim());
                }
                
                if(SingleTimeOutTextbox.Text.Trim() != "") {
                    _favorite.ConnectionTimeout = Convert.ToInt32(SingleTimeOutTextbox.Text.Trim());
                }
                
                if(IdleTimeoutMinutesTextBox.Text.Trim() != "") {
                    _favorite.IdleTimeout = Convert.ToInt32(IdleTimeoutMinutesTextBox.Text.Trim());
                }

                _favorite.EnableSecuritySettings = SecuritySettingsEnabledCheckbox.Checked;

                if(SecuritySettingsEnabledCheckbox.Checked) {
                    _favorite.SecurityWorkingFolder = SecurityWorkingFolderTextBox.Text;
                    _favorite.SecurityStartProgram = SecuriytStartProgramTextbox.Text;
                    _favorite.SecurityFullScreen = SecurityStartFullScreenCheckbox.Checked;
                }
                _favorite.GrabFocusOnConnect = GrabFocusOnConnectCheckbox.Checked;
                _favorite.EnableEncryption = EnableEncryptionCheckbox.Checked;
                _favorite.DisableWindowsKey = DisableWindowsKeyCheckbox.Checked;
                _favorite.DoubleClickDetect = DetectDoubleClicksCheckbox.Checked;
                _favorite.DisplayConnectionBar = DisplayConnectionBarCheckbox.Checked;
                _favorite.DisableControlAltDelete = DisableControlAltDeleteCheckbox.Checked;
                _favorite.AcceleratorPassthrough = AcceleratorPassthroughCheckBox.Checked;
                _favorite.EnableCompression = EnableCompressionCheckbox.Checked;
                _favorite.BitmapPeristence = EnableBitmapPersistanceCheckbox.Checked;
                _favorite.EnableTLSAuthentication = EnableTLSAuthenticationCheckbox.Checked;
                _favorite.EnableNLAAuthentication = EnableNLAAuthenticationCheckbox.Checked;

                _favorite.AllowBackgroundInput = AllowBackgroundInputCheckBox.Checked;

                _favorite.EnableFontSmoothing = AllowFontSmoothingCheckbox.Checked;
                _favorite.EnableDesktopComposition = AllowDesktopCompositionCheckbox.Checked;

                _favorite.DesktopSizeWidth = (int)this.widthUpDown.Value;
                _favorite.DesktopSizeHeight = (int)this.heightUpDown.Value;

                _favorite.Url = httpUrlTextBox.Text;

                _favorite.IcaClientINI = ICAClientINI.Text;
                _favorite.IcaServerINI = ICAServerINI.Text;
                _favorite.IcaEncryptionLevel = ICAEncryptionLevelCombobox.Text;
                _favorite.IcaEnableEncryption = ICAEnableEncryptionCheckbox.Checked;

                _favorite.Notes = NotesTextbox.Text;

                _favorite.KeyTag = SSHPreferences.KeyTag;
                _favorite.SSH1 = SSHPreferences.SSH1;
                _favorite.AuthMethod = SSHPreferences.AuthMethod;

                if (defaultFav)
                {
                    _favorite.Name = "";
                    _favorite.ServerName = "";
                    _favorite.DomainName = "";
                    _favorite.UserName = "";
                    _favorite.Password = "";
                    _favorite.Notes = "";
                    _favorite.EnableSecuritySettings = false;
                    _favorite.SecurityWorkingFolder = "";
                    _favorite.SecurityStartProgram = "";
                    _favorite.SecurityFullScreen = false;
                    _favorite.Url = "";
                    Settings.SaveDefaultFavorite(_favorite);
                }
                else
                {
                    if (string.IsNullOrEmpty(_oldName))
                        Settings.AddFavorite(_favorite, _showOnToolbar);
                    else
                        Settings.EditFavorite(_oldName, _favorite, _showOnToolbar);
                }

                return true;
            } catch(Exception e) {
                Terminals.Logging.Log.Info("", e);
                MessageBox.Show(this, e.Message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private string ValidateServer(string serverName)
        {
            serverName = serverName.Trim();
            if(Settings.ForceComputerNamesAsURI)
            {
                if(Uri.CheckHostName(serverName) == UriHostNameType.Unknown)
                    throw new ArgumentException("Server name is not valid");
            }
            else
            {
                if(serverName == null) throw new ArgumentException("Server name was not specified.");
                if(serverName.Length < 0) throw new ArgumentException("Server name was not specified.");
            }
            return serverName;
        }
        private int ValidatePort(string port)
        {
            if(txtPort.Text.Trim() != "")
            {
                int result;
                if(int.TryParse(txtPort.Text, out result) && result < 65536 && result > 0)
                    return result;
                else
                    throw new ArgumentException("Port must be a number between 0 and 65535");
            }
            else
                return 3389;
        }

        private void SetOkButtonState()
        {
            btnOk.Enabled = cmbServers.Text != String.Empty;
        }

        private void GetServerAndPort(string Connection, out string Server, out int Port)
        {
            Server = Connection;
            Port = 3389;
            if(Connection != null && Connection.Trim() != "" && Connection.Contains(":"))
            {
                string server = Connection.Substring(0, Connection.IndexOf(":"));
                string rawPort = Connection.Substring(Connection.IndexOf(":") + 1);
                int port = 3389;
                if(rawPort != null && rawPort.Trim() != "")
                {
                    rawPort = rawPort.Trim();
                    int.TryParse(rawPort, out port);
                }
                Server = server;
                Port = port;
            }
        }
        private void AddTag()
        {
            if (!String.IsNullOrEmpty(txtTag.Text))
            {
                string newTag = txtTag.Text.ToLower();
                foreach (string tag in Settings.Tags)
                {
                    if (tag.ToLower() == newTag)
                    {
                        txtTag.Text = tag;
                        break;
                    }
                }

                ListViewItem[] items = lvConnectionTags.Items.Find(txtTag.Text, false);
                if (items.Length == 0)
                {
                    Settings.AddTag(txtTag.Text);
                    lvConnectionTags.Items.Add(txtTag.Text);
                }
            }
        }
        private void DeleteTag()
        {
            foreach (ListViewItem item in lvConnectionTags.SelectedItems)
            {
                lvConnectionTags.Items.Remove(item);
            }
        }
        #endregion

        #region Private
        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveMRUs();
            
            if (FillFavorite(false))
                DialogResult = DialogResult.OK;
        }

        private void control_TextChanged(object sender, EventArgs e)
        {
            SetOkButtonState();
        }

        private void chkSavePassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.ReadOnly = !chkSavePassword.Checked;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            SetOkButtonState();
        }

        private void NewTerminalForm_Shown(object sender, EventArgs e)
        {
            cmbServers.Focus();
        }

        private void cmbServers_TextChanged(object sender, EventArgs e)
        {
            SetOkButtonState();
        }

        private void cmbServers_Leave(object sender, EventArgs e)
        {
            if (txtName.Text == String.Empty)
            {
                if (cmbServers.Text.Contains(":"))
                {
                    string server = "";
                    int port = 3389;
                    GetServerAndPort(cmbServers.Text, out server, out port);
                    cmbServers.Text = server;
                    txtPort.Text = port.ToString();
                    cmbServers.Text = server;
                }
                txtName.Text = cmbServers.Text;
            }
        }

        private void btnBrowseShare_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = @"\\" + cmbServers.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtDesktopShare.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnAddNewTag_Click(object sender, EventArgs e)
        {
            AddTag();
        }

        private void btnRemoveTag_Click(object sender, EventArgs e)
        {
            DeleteTag();
        }

        private void btnSaveDefault_Click(object sender, EventArgs e)
        {
            DefaultDialog frm = new DefaultDialog();
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Yes)
                FillFavorite(true);
            else if (dr == DialogResult.No)
                Settings.RemoveDefaultFavorite();
        }

        private void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string defaultPort = Connections.ConnectionManager.VNCVMRCPort.ToString();

            cmbServers.Enabled = true;
            txtPort.Enabled = true;

            vncAutoScaleCheckbox.Enabled = false;
            vncDisplayNumberInput.Enabled = false;
            VncViewOnlyCheckbox.Enabled = false;

            groupBox1.Enabled = false;
            chkConnectToConsole.Enabled = false;
            LocalResourceGroupBox.Enabled = false;
            VMRCReducedColorsCheckbox.Enabled = false;
            VMRCAdminModeCheckbox.Enabled = false;
            RASGroupBox.Enabled = false;

            ICAApplicationNameTextBox.Enabled = false;
            ICAApplicationPath.Enabled = false;
            httpUrlTextBox.Enabled = false;
            txtPort.Enabled = true;

            if (ProtocolComboBox.Text == "RDP")
            {
                defaultPort = Connections.ConnectionManager.RDPPort.ToString();
                groupBox1.Enabled = true;
                chkConnectToConsole.Enabled = true;
                LocalResourceGroupBox.Enabled = true;
            }
            else if (ProtocolComboBox.Text == "VMRC")
            {
                VMRCReducedColorsCheckbox.Enabled = true;
                VMRCAdminModeCheckbox.Enabled = true;
            }
            else if (ProtocolComboBox.Text == "RAS")
            {
                this.cmbServers.Items.Clear();
                LoadDialupConnections();
                RASGroupBox.Enabled = true;
                txtPort.Enabled = false;
                RASDetailsListBox.Items.Clear();
            }
            else if (ProtocolComboBox.Text == "VNC")
            {
                //vnc settings
                vncAutoScaleCheckbox.Enabled = true;
                vncDisplayNumberInput.Enabled = true;
                VncViewOnlyCheckbox.Enabled = true;
            }
            else if (ProtocolComboBox.Text == "Telnet")
            {
                defaultPort = Connections.ConnectionManager.TelnetPort.ToString();
            }
            else if (ProtocolComboBox.Text == "SSH")
            {
                defaultPort = Connections.ConnectionManager.SSHPort.ToString();
            }
            else if (ProtocolComboBox.Text == "ICA Citrix")
            {
                ICAApplicationNameTextBox.Enabled = true;
                ICAApplicationPath.Enabled = true;
                defaultPort = Connections.ConnectionManager.ICAPort.ToString();
            }
            else if (ProtocolComboBox.Text == "HTTP")
            {
                cmbServers.Enabled = false;
                txtPort.Enabled = false;
                txtPort.Text = "80";
                httpUrlTextBox.Enabled = true;
                defaultPort = Connections.ConnectionManager.HTTPPort.ToString();
                this.tabControl1.SelectTab(HTTPTabPage);
            }
            else if (ProtocolComboBox.Text == "HTTPS")
            {
                cmbServers.Enabled = false;
                txtPort.Text = "443";
                txtPort.Enabled = false;
                httpUrlTextBox.Enabled = true;
                defaultPort = Connections.ConnectionManager.HTTPSPort.ToString();
                this.tabControl1.SelectTab(HTTPTabPage);
            }

            txtPort.Text = defaultPort;
        }

        private void cmbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProtocolComboBox.Text == "RAS")
            {
                LoadDialupConnections();
                RASDetailsListBox.Items.Clear();
                if (_dialupList != null && _dialupList.ContainsKey(cmbServers.Text))
                {
                    RASENTRY selectedRAS = _dialupList[cmbServers.Text];
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Connection", cmbServers.Text));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Area Code", selectedRAS.AreaCode));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Country Code", selectedRAS.CountryCode));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Device Name", selectedRAS.DeviceName));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Device Type", selectedRAS.DeviceType));
                    RASDetailsListBox.Items.Add(string.Format("{0}:{1}", "Local Phone Number", selectedRAS.LocalPhoneNumber));
                }
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string appFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.InitialDirectory = appFolder;
            openFileDialog.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select Custom Terminal Image .";
            string thumbsFolder = Path.Combine(appFolder, "Thumbs");
            if(!Directory.Exists(thumbsFolder))
                Directory.CreateDirectory(thumbsFolder);

            _currentToolBarFileName = "";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //make it relative to the current application executable
                string filename = openFileDialog.FileName;
                try
                {
                    Image i = Image.FromFile(filename);
                    if(i != null)
                    {                        
                        this.pictureBox2.Image = i;
                        string newFile = Path.Combine(thumbsFolder, Path.GetFileName(filename));
                        if(newFile != filename && !File.Exists(newFile)) 
                            File.Copy(filename, newFile);
                        _currentToolBarFileName = newFile;
                    }
                }
                catch(Exception ex)
                {
                    Terminals.Logging.Log.Info("", ex);
                    _currentToolBarFileName = "";
                    System.Windows.Forms.MessageBox.Show("You have chosen an invalid image. Try again.");
                }
            }
        }

        private void SecuritySettingsEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = SecuritySettingsEnabledCheckbox.Checked;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab == RDPTabPage)
            {
                RDPSubTabPage_SelectedIndexChanged(null, null);
            }
        }

        private void RDPSubTabPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RDPSubTabPage.SelectedTab == tabPage10)
            {
                _terminalServerManager.Connect(this.cmbServers.Text, true);
                _terminalServerManager.Invalidate();
            }
        }

        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            customSizePanel.Visible = false;
            if(cmbResolution.Text == "Custom" || cmbResolution.Text == "Auto Scale")
                customSizePanel.Visible = true;            
        }

        private void AllTagsAddButton_Click(object sender, EventArgs e) 
        {
            foreach(ListViewItem lv in AllTagsListView.SelectedItems) {
                ListViewItem[] items = lvConnectionTags.Items.Find(lv.Text, false);
                if(items.Length == 0) {
                    Settings.AddTag(lv.Text);
                    lvConnectionTags.Items.Add(lv.Text);
                }
            }
        }

        private void ICAEnableEncryptionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ICAEncryptionLevelCombobox.Enabled = ICAEnableEncryptionCheckbox.Checked;
        }

        private void httpUrlTextBox_TextChanged(object sender, EventArgs e) 
        {
            string url = httpUrlTextBox.Text;
            try {
                System.Uri u = new Uri(url);
                cmbServers.Text = u.Host;
                txtPort.Text = u.Port.ToString();
            } catch(Exception ex) {
                Logging.Log.Error("Http URL Parse Failed", ex);
            }
        }

        private void AllTagsListView_DoubleClick(object sender, EventArgs e)
        {
            AllTagsAddButton_Click(null, null);
        }

        private void CredentialManagerPicturebox_Click(object sender, EventArgs e)
        {
            Credentials.CredentialManager mgr = new Terminals.Credentials.CredentialManager();
            mgr.ShowDialog();
        }

        private void CredentialDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            CredentialsPanel.Enabled = true;
            Credentials.CredentialSet set = (CredentialDropdown.SelectedItem as Credentials.CredentialSet);

            if (set != null)
            {
                CredentialsPanel.Enabled = false;
                cmbDomains.Text = set.Domain;
                cmbUsers.Text = set.Username;
                txtPassword.Text = set.Password;
                chkSavePassword.Checked = true;
            }
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pnlTSGWlogon.Enabled = chkTSGWlogin.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            pnlTSGWsettings.Enabled = radTSGWenable.Checked;
        }

        private void btnDrives_Click(object sender, EventArgs e)
        {
            DiskDrivesForm drivesForm = new DiskDrivesForm(this);
            drivesForm.ShowDialog(this);
        }
    }
}
