using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FalafelSoftware.TransPort;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Credentials;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Terminals.Network.Servers;
using Terminals.TerminalServices;

namespace Terminals
{
    internal partial class NewTerminalForm : Form
    {
        private TerminalServerManager _terminalServerManager = new TerminalServerManager();
        private Dictionary<string, RASENTRY> _dialupList = new Dictionary<string, RASENTRY>();
        private String _currentToolBarFileName;
        private Guid editedId;
        private string oldName;

        private String favoritePassword = string.Empty;
        internal const String HIDDEN_PASSWORD = "****************";

        private static IGroups PersistedGroups
        {
            get { return Persistence.Instance.Groups; }
        }

        private static IFavorites PersistedFavorites
        {
            get { return Persistence.Instance.Favorites; }
        }

        #region Constructors

        public NewTerminalForm(String serverName)
            : this()
        {
            this.Init(null, serverName);
        }

        public NewTerminalForm(IFavorite favorite)
            : this()
        {
            this.Init(favorite, String.Empty);
        }

        private NewTerminalForm()
        {
            this.InitializeComponent();
            this.RedirectedDrives = new List<String>();
            this.RedirectDevices = false;
        }

        #endregion

        #region Properties

        public new TerminalFormDialogResult DialogResult { get; private set; }
        internal IFavorite Favorite { get; private set; }
        internal bool ShowOnToolbar { get; private set; }
        internal List<string> RedirectedDrives { get; set; }
        internal bool RedirectDevices { get; set; }

        #endregion

        #region Private form event handlers

        private void NewTerminalForm_Load(Object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.LoadCustomControlsState();
            BindGroupsToListView(this.AllTagsListView, PersistedGroups);
            this.ResumeLayout(true);
        }

        private void NewTerminalForm_Shown(object sender, EventArgs e)
        {
            this.cmbServers.Focus();
        }

        #endregion

        #region Public form functions

        /// <summary>
        /// Overload ShowDialog and return custom result.
        /// </summary>
        /// <returns>Returns custom dialogresult.</returns>
        public new TerminalFormDialogResult ShowDialog()
        {
            base.ShowDialog();

            return this.DialogResult;
        }

        #endregion

        #region Private form control event handler methods

        private void AllTagsAddButton_Click(object sender, EventArgs e)
        {
            this.AddGroupsToFavorite();
        }

        private void AllTagsListView_DoubleClick(object sender, EventArgs e)
        {
            this.AddGroupsToFavorite();
        }

        private void appPathBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();

            DialogResult result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ICAApplicationPath.Text = d.SelectedPath;
            }
        }

        private void AppWorkingFolderBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();

            DialogResult result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ICAWorkingFolder.Text = d.SelectedPath;
            }
        }

        private void btnAddNewTag_Click(object sender, EventArgs e)
        {
            this.AddGroup();
        }

        private void btnBrowseShare_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select Desktop Share:";
                dialog.ShowNewFolderButton = false;
                dialog.SelectedPath = @"\\" + this.cmbServers.Text;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.txtDesktopShare.Text = dialog.SelectedPath;
            }
        }

        private void btnDrives_Click(object sender, EventArgs e)
        {
            DiskDrivesForm drivesForm = new DiskDrivesForm(this);
            drivesForm.ShowDialog(this);
        }

        private void btnRemoveTag_Click(object sender, EventArgs e)
        {
            this.DeleteGroup();
        }

        /// <summary>
        /// Save favorite and close form. If the form isnt valid the form control is focused.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this.DialogResult = TerminalFormDialogResult.SaveAndClose;
                this.Close();
            }
        }

        private void btnSaveDefault_Click(object sender, EventArgs e)
        {
            this.contextMenuStripDefaults.Show(this.btnSaveDefault, 0, this.btnSaveDefault.Height);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlTSGWlogon.Enabled = this.chkTSGWlogin.Checked;
        }

        private void ClientINIBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "*.ini";
            d.CheckFileExists = true;

            DialogResult result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ICAClientINI.Text = d.FileName;
            }
        }

        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbResolution.Text == "Custom" || this.cmbResolution.Text == "Auto Scale")
                this.customSizePanel.Visible = true;
            else
                this.customSizePanel.Visible = false;
        }

        private void cmbServers_Leave(object sender, EventArgs e)
        {
            if (this.txtName.Text == String.Empty)
            {
                if (this.cmbServers.Text.Contains(":"))
                {
                    String server = String.Empty;
                    int port;
                    GetServerAndPort(this.cmbServers.Text, out server, out port);
                    this.cmbServers.Text = server;
                    this.txtPort.Text = port.ToString();
                    this.cmbServers.Text = server;
                }

                this.txtName.Text = this.cmbServers.Text;
            }
        }

        private void cmbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProtocolComboBox.Text == ConnectionManager.RAS)
            {
                this.LoadDialupConnections();
                this.RASDetailsListBox.Items.Clear();
                if (this._dialupList != null && this._dialupList.ContainsKey(cmbServers.Text))
                {
                    RASENTRY selectedRAS = this._dialupList[this.cmbServers.Text];
                    this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Connection", this.cmbServers.Text));
                    this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Area Code", selectedRAS.AreaCode));
                    this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Country Code", selectedRAS.CountryCode));
                    this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Device Name", selectedRAS.DeviceName));
                    this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Device Type", selectedRAS.DeviceType));
                    this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Local Phone Number", selectedRAS.LocalPhoneNumber));
                }
            }
        }

        private void cmbServers_TextChanged(object sender, EventArgs e)
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

            CredentialManager mgr = new CredentialManager();
            mgr.ShowDialog();
            this.FillCredentialsCombobox(selectedCredentialId);
        }

        private void httpUrlTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ConnectionManager.IsProtocolWebBased(this.ProtocolComboBox.Text))
            {
                Uri newUrl = GetFullUrlFromHttpTextBox();
                if (newUrl != null)
                {
                    this.cmbServers.Text = newUrl.Host;
                    this.txtPort.Text = newUrl.Port.ToString();
                }
                SetOkButtonState();
            }
        }

        private void ICAEnableEncryptionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.ICAEncryptionLevelCombobox.Enabled = this.ICAEnableEncryptionCheckbox.Checked;
        }

        private void lvConnectionTags_DoubleClick(object sender, EventArgs e)
        {
            this.DeleteGroup();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = CreateIconSelectionDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.TryToAssignNewToolbarIcon(openFileDialog.FileName);
            }
        }

        private void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetControlsProtocolIndependent();

            if (this.ProtocolComboBox.Text == ConnectionManager.RDP)
            {
                this.SetControlsForRdp();
            }
            else if (this.ProtocolComboBox.Text == ConnectionManager.VMRC)
            {
                this.VmrcGroupBox.Enabled = true;
            }
            else if (this.ProtocolComboBox.Text == ConnectionManager.RAS)
            {
                this.SetControlsForRas();
            }
            else if (this.ProtocolComboBox.Text == ConnectionManager.VNC)
            {
                this.VncGroupBox.Enabled = true;
            }
            else if (this.ProtocolComboBox.Text == ConnectionManager.ICA_CITRIX)
            {
                this.IcaGroupBox.Enabled = true;
            }
            else if (ConnectionManager.IsProtocolWebBased(this.ProtocolComboBox.Text))
            {
                this.SetControlsForWeb();
            }
            else if (this.ProtocolComboBox.Text == ConnectionManager.SSH || this.ProtocolComboBox.Text == ConnectionManager.TELNET)
            {
                this.ConsoleGroupBox.Enabled = true;

                if (this.ProtocolComboBox.Text == ConnectionManager.SSH)
                    this.SshGroupBox.Enabled = true;
            }

            int defaultPort = ConnectionManager.GetPort(this.ProtocolComboBox.Text);
            this.txtPort.Text = defaultPort.ToString();
            this.SetOkButtonState();
        }

        private void radTSGWenable_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlTSGWsettings.Enabled = this.radTSGWenable.Checked;
        }

        private void RDPSubTabPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.RDPSubTabPage.SelectedTab == this.RdpSessionTabPage && this.ProtocolComboBox.Text == ConnectionManager.RDP)
            {
                // Do not auto connect for sessions on tab select. This might take a moment and than the program hangs.
                //this._terminalServerManager.Connect(this.cmbServers.Text, true);
                //this._terminalServerManager.Invalidate();
            }
        }

        private void removeSavedDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.RemoveDefaultFavorite();
        }

        /// <summary>
        /// Save favorite, close form and immediatly connect to the favorite.
        /// </summary>
        private void saveConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();

            if (this.FillFavorite(false))
                this.DialogResult = TerminalFormDialogResult.SaveAndConnect;

            this.Close();
        }

        /// <summary>
        /// Save favorite and copy the current favorite settings, except favorite and connection name.
        /// </summary>
        private void saveCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this.txtName.Text = this.Favorite.Name + "_(copy)";
                this.editedId = Guid.Empty;
                this.cmbServers.Text = String.Empty;
                this.cmbServers.Focus();
            }
        }

        private void saveCurrentSettingsAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FillFavorite(true);
        }

        /// <summary>
        /// Save favorite and clear form for a new favorite.
        /// </summary>
        private void saveNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this.editedId = Guid.Empty;
                this.Init(null, String.Empty);
                this.cmbServers.Focus();
            }
        }

        private void SecuritySettingsEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.panel2.Enabled = this.SecuritySettingsEnabledCheckbox.Checked;
        }

        private void ServerINIBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "*.ini";
            d.CheckFileExists = true;
            DialogResult result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ICAServerINI.Text = d.FileName;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.RDPTabPage)
            {
                this.RDPSubTabPage_SelectedIndexChanged(null, null);
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            this.SetOkButtonState();
        }

        #endregion

        #region Private form methods

        private void Init(IFavorite favorite, String serverName)
        {
            this.LoadMRUs();
            this.SetOkButtonState();
            this.SSHPreferences.Keys = Settings.SSHKeys;

            // move following line down to default value only once smart card access worked out.
            this.cmbTSGWLogonMethod.SelectedIndex = 0;

            if (favorite == null)
            {
                this.FillCredentialsCombobox(Guid.Empty);

                var defaultSavedFavorite = Settings.GetDefaultFavorite();
                if (defaultSavedFavorite != null)
                {
                    var defaultFavorite = ModelConverterV1ToV2.ConvertToFavorite(defaultSavedFavorite);
                    this.FillControls(defaultFavorite);
                }
                else
                {
                    this.cmbResolution.SelectedIndex = 7;
                    this.cmbColors.SelectedIndex = 1;
                    this.cmbSounds.SelectedIndex = 2;
                    this.ProtocolComboBox.SelectedIndex = 0;
                }

                String server;
                Int32 port;
                GetServerAndPort(serverName, out server, out port);
                this.cmbServers.Text = server;
                this.txtPort.Text = port.ToString();
            }
            else
            {
                this.editedId = favorite.Id;
                this.oldName = favorite.Name;
                this.Text = "Edit Connection";
                this.FillControls(favorite);
            }
        }

        private void FillCredentialsCombobox(Guid credential)
        {
            this.CredentialDropdown.Items.Clear();
            this.CredentialDropdown.Items.Add("(custom)");
            this.FillCredentialsComboboxWithStoredCredentials();
            this.CredentialDropdown.SelectedItem = Persistence.Instance.Credentials[credential];
        }

        private void FillCredentialsComboboxWithStoredCredentials()
        {
            IEnumerable<ICredentialSet> credentials = Persistence.Instance.Credentials;
            if (credentials != null)
            {
                foreach (ICredentialSet item in credentials)
                {
                    this.CredentialDropdown.Items.Add(item);
                }
            }
        }

        private void LoadCustomControlsState()
        {
            this._terminalServerManager.Dock = DockStyle.Fill;
            this._terminalServerManager.Location = new Point(0, 0);
            this._terminalServerManager.Name = "terminalServerManager1";
            this._terminalServerManager.Size = new Size(748, 309);
            this._terminalServerManager.TabIndex = 0;
            this._terminalServerManager.HostName = (!String.IsNullOrEmpty(this.cmbServers.Text)) ? this.cmbServers.Text : "localhost";
            this.RdpSessionTabPage.Controls.Add(this._terminalServerManager);
        }

        private void LoadDialupConnections()
        {
            this._dialupList = new Dictionary<String, RASENTRY>();
            System.Collections.ArrayList rasEntries = new System.Collections.ArrayList();
            RasError error = ras1.ListEntries(ref rasEntries);
            foreach (String item in rasEntries)
            {
                RASENTRY details = new RASENTRY();
                ras1.GetEntry(item, ref details);
                this._dialupList.Add(item, details);

                if (!cmbServers.Items.Contains(item))
                    this.cmbServers.Items.Add(item);
            }
        }

        private void LoadMRUs()
        {
            this.cmbServers.Items.AddRange(Settings.MRUServerNames);
            this.cmbDomains.Items.AddRange(Settings.MRUDomainNames);
            this.cmbUsers.Items.AddRange(Settings.MRUUserNames);
            string[] groupNames = PersistedGroups.Select(group => group.Name).ToArray();
            this.txtTag.AutoCompleteCustomSource.AddRange(groupNames);
        }

        private void SaveMRUs()
        {
            Settings.AddServerMRUItem(cmbServers.Text);
            Settings.AddDomainMRUItem(cmbDomains.Text);
            Settings.AddUserMRUItem(cmbUsers.Text);
        }

        private void FillControls(IFavorite favorite)
        {
            FillGeneralPropertiesControls(favorite);
            FillDescriptionPropertiesControls(favorite);
            FillSecurityControls(favorite);
            this.FillCredentialsCombobox(favorite.Security.Credential);
            FillDisplayControls(favorite);
            FillExecuteBeforeControls(favorite);
            this.consolePreferences.FillControls(favorite);
            FillVmrcControls(favorite);
            FillVNCControls(favorite);
            FillIcaControls(favorite);
            FillSshControls(favorite);
            FillRdpOptions(favorite);

            ReloadTagsListViewItems(favorite);
        }

        private void FillSecurityControls(IFavorite favorite)
        {
            this.cmbDomains.Text = favorite.Security.Domain;
            this.cmbUsers.Text = favorite.Security.UserName;
            this.favoritePassword = favorite.Security.Password;

            if (string.IsNullOrEmpty(this.favoritePassword) && !string.IsNullOrEmpty(favorite.Security.EncryptedPassword))
            {
                MessageBox.Show("There was an issue with decrypting your password.\n\nPlease provide a new password and save the favorite.");
                this.txtPassword.Text = "";
                this.favoritePassword = String.Empty;
                this.txtPassword.Focus();
                favorite.Security.Password = String.Empty;
            }

            if (!string.IsNullOrEmpty(this.favoritePassword))
            {
                this.txtPassword.Text = HIDDEN_PASSWORD;
                this.chkSavePassword.Checked = true;
            }
            else
            {
                this.txtPassword.Text = String.Empty;
                this.chkSavePassword.Checked = false;
            }
        }

        private void FillDescriptionPropertiesControls(IFavorite favorite)
        {
            this.NewWindowCheckbox.Checked = favorite.NewWindow;
            this.txtDesktopShare.Text = favorite.DesktopShare;
            this.chkAddtoToolbar.Checked = Settings.HasToolbarButton(favorite.Id);
            this.pictureBox2.Image = favorite.ToolBarIconImage;
            this._currentToolBarFileName = favorite.ToolBarIconFile;
            this.NotesTextbox.Text = favorite.Notes;
        }

        private void FillGeneralPropertiesControls(IFavorite favorite)
        {
            this.txtName.Text = favorite.Name;
            this.cmbServers.Text = favorite.ServerName;
            this.ProtocolComboBox.SelectedItem = favorite.Protocol;
            this.txtPort.Text = favorite.Port.ToString();
            this.httpUrlTextBox.Text = WebOptions.ExtractAbsoluteUrl(favorite);
        }

        private void FillExecuteBeforeControls(IFavorite favorite)
        {
            this.chkExecuteBeforeConnect.Checked = favorite.ExecuteBeforeConnect.Execute;
            this.txtCommand.Text = favorite.ExecuteBeforeConnect.Command;
            this.txtArguments.Text = favorite.ExecuteBeforeConnect.CommandArguments;
            this.txtInitialDirectory.Text = favorite.ExecuteBeforeConnect.InitialDirectory;
            this.chkWaitForExit.Checked = favorite.ExecuteBeforeConnect.WaitForExit;
        }

        private void FillRdpOptions(IFavorite favorite)
        {
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions == null)
                return;

            FillRdpSecurityControls(rdpOptions);
            FillRdpUserInterfaceControls(rdpOptions);
            FillRdpDisplayControls(rdpOptions);
            FillRdpRedirectControls(rdpOptions);
            FillTsGatewayControls(rdpOptions);
            FillRdpTimeOutControls(rdpOptions);
        }

        private void FillRdpDisplayControls(RdpOptions rdpOptions)
        {
            this.chkConnectToConsole.Checked = rdpOptions.ConnectToConsole;
            this.chkDisableWallpaper.Checked = rdpOptions.UserInterface.DisableWallPaper;
            this.chkDisableCursorBlinking.Checked = rdpOptions.UserInterface.DisableCursorBlinking;
            this.chkDisableCursorShadow.Checked = rdpOptions.UserInterface.DisableCursorShadow;
            this.chkDisableFullWindowDrag.Checked = rdpOptions.UserInterface.DisableFullWindowDrag;
            this.chkDisableMenuAnimations.Checked = rdpOptions.UserInterface.DisableMenuAnimations;
            this.chkDisableThemes.Checked = rdpOptions.UserInterface.DisableTheming;
        }

        private void FillRdpSecurityControls(RdpOptions rdpOptions)
        {
            this.EnableTLSAuthenticationCheckbox.Checked = rdpOptions.Security.EnableTLSAuthentication;
            this.EnableNLAAuthenticationCheckbox.Checked = rdpOptions.Security.EnableNLAAuthentication;
            this.EnableEncryptionCheckbox.Checked = rdpOptions.Security.EnableEncryption;
            this.SecuritySettingsEnabledCheckbox.Checked = rdpOptions.Security.Enabled;
            this.SecurityWorkingFolderTextBox.Text = rdpOptions.Security.WorkingFolder;
            this.SecuriytStartProgramTextbox.Text = rdpOptions.Security.StartProgram;
            this.SecurityStartFullScreenCheckbox.Checked = rdpOptions.FullScreen;
        }

        private void FillRdpRedirectControls(RdpOptions rdpOptions)
        {
            this.RedirectedDrives = rdpOptions.Redirect.Drives;
            this.chkSerialPorts.Checked = rdpOptions.Redirect.Ports;
            this.chkPrinters.Checked = rdpOptions.Redirect.Printers;
            this.chkRedirectClipboard.Checked = rdpOptions.Redirect.Clipboard;
            this.RedirectDevices = rdpOptions.Redirect.Devices;
            this.chkRedirectSmartcards.Checked = rdpOptions.Redirect.SmartCards;
            this.cmbSounds.SelectedIndex = (Int32)rdpOptions.Redirect.Sounds;
        }

        private void FillSshControls(IFavorite favorite)
        {
            var sshOptions = favorite.ProtocolProperties as SshOptions;
            if (sshOptions == null)
                return;

            this.SSHPreferences.AuthMethod = sshOptions.AuthMethod;
            this.SSHPreferences.KeyTag = sshOptions.CertificateKey;
            this.SSHPreferences.SSH1 = sshOptions.SSH1;
        }

        private void FillDisplayControls(IFavorite favorite)
        {
            this.cmbResolution.SelectedIndex = (Int32)favorite.Display.DesktopSize;
            this.cmbColors.SelectedIndex = (Int32)favorite.Display.Colors;
            this.widthUpDown.Value = favorite.Display.Width;
            this.heightUpDown.Value = favorite.Display.Height;
        }

        private void FillIcaControls(IFavorite favorite)
        {
            var icaOptions = favorite.ProtocolProperties as ICAOptions;
            if (icaOptions == null)
                return;

            this.ICAClientINI.Text = icaOptions.ClientINI;
            this.ICAServerINI.Text = icaOptions.ServerINI;
            this.ICAEncryptionLevelCombobox.Text = icaOptions.EncryptionLevel;
            this.ICAEnableEncryptionCheckbox.Checked = icaOptions.EnableEncryption;
            this.ICAEncryptionLevelCombobox.Enabled = icaOptions.EnableEncryption;

            this.ICAApplicationNameTextBox.Text = icaOptions.ApplicationName;
            this.ICAApplicationPath.Text = icaOptions.ApplicationPath;
            this.ICAWorkingFolder.Text = icaOptions.ApplicationWorkingFolder;
        }

        private void FillRdpUserInterfaceControls(RdpOptions rdpOptions)
        {
            var userInterface = rdpOptions.UserInterface;
            this.GrabFocusOnConnectCheckbox.Checked = rdpOptions.GrabFocusOnConnect;
            this.DisableWindowsKeyCheckbox.Checked = userInterface.DisableWindowsKey;
            this.DetectDoubleClicksCheckbox.Checked = userInterface.DoubleClickDetect;
            this.DisplayConnectionBarCheckbox.Checked = userInterface.DisplayConnectionBar;
            this.DisableControlAltDeleteCheckbox.Checked = userInterface.DisableControlAltDelete;
            this.AcceleratorPassthroughCheckBox.Checked = userInterface.AcceleratorPassthrough;
            this.EnableCompressionCheckbox.Checked = userInterface.EnableCompression;
            this.EnableBitmapPersistenceCheckbox.Checked = userInterface.BitmapPeristence;
            this.AllowBackgroundInputCheckBox.Checked = userInterface.AllowBackgroundInput;
            this.AllowFontSmoothingCheckbox.Checked = userInterface.EnableFontSmoothing;
            this.AllowDesktopCompositionCheckbox.Checked = userInterface.EnableDesktopComposition;
        }

        private void FillRdpTimeOutControls(RdpOptions rdpOptions)
        {
            this.ShutdownTimeoutTextBox.Text = rdpOptions.TimeOuts.ShutdownTimeout.ToString();
            this.OverallTimeoutTextbox.Text = rdpOptions.TimeOuts.OverallTimeout.ToString();
            this.SingleTimeOutTextbox.Text = rdpOptions.TimeOuts.ConnectionTimeout.ToString();
            this.IdleTimeoutMinutesTextBox.Text = rdpOptions.TimeOuts.IdleTimeout.ToString();
        }

        private void FillTsGatewayControls(RdpOptions rdpOptions)
        {
            var tsGateway = rdpOptions.TsGateway;
            switch (tsGateway.UsageMethod)
            {
                case 0:
                    this.radTSGWdisable.Checked = true;
                    this.chkTSGWlocalBypass.Checked = false;
                    break;
                case 1:
                    this.radTSGWenable.Checked = true;
                    this.chkTSGWlocalBypass.Checked = false;
                    break;
                case 2:
                    this.radTSGWenable.Checked = true;
                    this.chkTSGWlocalBypass.Checked = true;
                    break;
                case 4:
                    this.radTSGWdisable.Checked = true;
                    this.chkTSGWlocalBypass.Checked = true;
                    break;
            }

            this.txtTSGWServer.Text = tsGateway.HostName;
            this.txtTSGWDomain.Text = tsGateway.Security.Domain;
            this.txtTSGWUserName.Text = tsGateway.Security.UserName;
            this.txtTSGWPassword.Text = tsGateway.Security.Password;
            this.chkTSGWlogin.Checked = tsGateway.SeparateLogin;
            if (tsGateway.CredentialSource == 4)
            {
                this.cmbTSGWLogonMethod.SelectedIndex = 2;
            }
            else
            {
                this.cmbTSGWLogonMethod.SelectedIndex = tsGateway.CredentialSource;

            }
        }

        private void FillVmrcControls(IFavorite favorite)
        {
            VMRCOptions vncOptions = favorite.ProtocolProperties as VMRCOptions;
            if (vncOptions == null)
                return;
            this.VMRCAdminModeCheckbox.Checked = vncOptions.AdministratorMode;
            this.VMRCReducedColorsCheckbox.Checked = vncOptions.ReducedColorsMode;
        }

        private void FillVNCControls(IFavorite favorite)
        {
            VncOptions vncOptions = favorite.ProtocolProperties as VncOptions;
            if (vncOptions == null)
                return;
            this.vncAutoScaleCheckbox.Checked = vncOptions.AutoScale;
            this.vncDisplayNumberInput.Value = vncOptions.DisplayNumber;
            this.VncViewOnlyCheckbox.Checked = vncOptions.ViewOnly;
        }

        private void ReloadTagsListViewItems(IFavorite favorite)
        {
            this.lvConnectionTags.Items.Clear();
            BindGroupsToListView(this.lvConnectionTags, favorite.Groups);
        }

        private static void BindGroupsToListView(ListView listViewToFill, IEnumerable<IGroup> groups)
        {
            foreach (IGroup group in groups)
            {
                if (group.Name != Settings.UNTAGGED_NODENAME)
                {
                    var groupItem = new GroupListViewItem(group);
                    listViewToFill.Items.Add(groupItem);
                }
            }
        }

        private Boolean FillFavorite(Boolean defaultFav)
        {
            try
            {
                if (!this.IsServerNameValid() || !this.IsPortValid())
                    return false;

                ResolveFavortie();

                FillGeneralProrperties();
                FillDescriptionProperties();
                FillFavoriteSecurity();
                FillFavoriteDisplayOptions();
                FillFavoriteExecuteBeforeOptions();
                this.consolePreferences.FillFavorite(this.Favorite);
                this.FillFavoriteVmrcOptions();
                this.FillFavoriteVncOptions();
                FillFavoriteICAOPtions();
                FillFavoriteSSHOptions();
                FillFavoriteRdpOptions();

                if (defaultFav)
                    SaveDefaultFavorite();
                else
                    this.CommitFavoriteChanges();

                return true;
            }
            catch (Exception e)
            {
                Logging.Log.Info("Fill Favorite Failed", e);
                ShowErrorMessageBox(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Overwrites favortie property by favorite stored in persistence
        /// or newly created one
        /// </summary>
        private void ResolveFavortie()
        {
            this.Favorite = null; // force favorite property reset
            if (!this.editedId.Equals(Guid.Empty))
                this.Favorite = PersistedFavorites[this.editedId];
            if (this.Favorite == null)
                this.Favorite = Persistence.Instance.Factory.CreateFavorite();
        }

        private void FillDescriptionProperties()
        {
            this.Favorite.NewWindow = this.NewWindowCheckbox.Checked;
            this.Favorite.DesktopShare = this.txtDesktopShare.Text;
            this.ShowOnToolbar = this.chkAddtoToolbar.Checked;
            this.Favorite.ToolBarIconFile = this._currentToolBarFileName;
            this.Favorite.Notes = this.NotesTextbox.Text;
        }

        private void FillGeneralProrperties()
        {
            this.Favorite.Name = (String.IsNullOrEmpty(this.txtName.Text) ? this.cmbServers.Text : this.txtName.Text);
            this.Favorite.ServerName = this.cmbServers.Text;
            this.Favorite.Protocol = this.ProtocolComboBox.SelectedItem.ToString();
            this.Favorite.Port = Int32.Parse(this.txtPort.Text);
            this.FillWebProperties();
        }

        private void FillWebProperties()
        {
            WebOptions.UpdateFavoriteUrl(this.Favorite, this.httpUrlTextBox.Text);
        }

        private void FillFavoriteDisplayOptions()
        {
            this.Favorite.Display.DesktopSize = (DesktopSize)this.cmbResolution.SelectedIndex;
            this.Favorite.Display.Colors = (Colors)this.cmbColors.SelectedIndex;
            this.Favorite.Display.Width = (Int32)this.widthUpDown.Value;
            this.Favorite.Display.Height = (Int32)this.heightUpDown.Value;
        }

        private void FillFavoriteRdpSecurity(RdpOptions rdpOptions)
        {
            rdpOptions.Security.EnableTLSAuthentication = this.EnableTLSAuthenticationCheckbox.Checked;
            rdpOptions.Security.EnableNLAAuthentication = this.EnableNLAAuthenticationCheckbox.Checked;
            rdpOptions.Security.EnableEncryption = this.EnableEncryptionCheckbox.Checked;
            rdpOptions.Security.Enabled = this.SecuritySettingsEnabledCheckbox.Checked;
            if (this.SecuritySettingsEnabledCheckbox.Checked)
            {
                rdpOptions.Security.WorkingFolder = this.SecurityWorkingFolderTextBox.Text;
                rdpOptions.Security.StartProgram = this.SecuriytStartProgramTextbox.Text;
                rdpOptions.FullScreen = this.SecurityStartFullScreenCheckbox.Checked;
            }
        }

        private void FillFavoriteRdpTimeOutOptions(RdpOptions rdpOptions)
        {
            if (this.ShutdownTimeoutTextBox.Text.Trim() != String.Empty)
            {
                rdpOptions.TimeOuts.ShutdownTimeout = Convert.ToInt32(this.ShutdownTimeoutTextBox.Text.Trim());
            }

            if (this.OverallTimeoutTextbox.Text.Trim() != String.Empty)
            {
                rdpOptions.TimeOuts.OverallTimeout = Convert.ToInt32(this.OverallTimeoutTextbox.Text.Trim());
            }

            if (this.SingleTimeOutTextbox.Text.Trim() != String.Empty)
            {
                rdpOptions.TimeOuts.ConnectionTimeout = Convert.ToInt32(this.SingleTimeOutTextbox.Text.Trim());
            }

            if (this.IdleTimeoutMinutesTextBox.Text.Trim() != String.Empty)
            {
                rdpOptions.TimeOuts.IdleTimeout = Convert.ToInt32(this.IdleTimeoutMinutesTextBox.Text.Trim());
            }
        }

        private void FillFavoriteRdpInterfaceOptions(RdpOptions rdpOptions)
        {
            RdpUserInterfaceOptions userInterface = rdpOptions.UserInterface;
            rdpOptions.GrabFocusOnConnect = this.GrabFocusOnConnectCheckbox.Checked;
            userInterface.DisableWindowsKey = this.DisableWindowsKeyCheckbox.Checked;
            userInterface.DoubleClickDetect = this.DetectDoubleClicksCheckbox.Checked;
            userInterface.DisplayConnectionBar = this.DisplayConnectionBarCheckbox.Checked;
            userInterface.DisableControlAltDelete = this.DisableControlAltDeleteCheckbox.Checked;
            userInterface.AcceleratorPassthrough = this.AcceleratorPassthroughCheckBox.Checked;
            userInterface.EnableCompression = this.EnableCompressionCheckbox.Checked;
            userInterface.BitmapPeristence = this.EnableBitmapPersistenceCheckbox.Checked;
            userInterface.AllowBackgroundInput = this.AllowBackgroundInputCheckBox.Checked;
            userInterface.EnableFontSmoothing = this.AllowFontSmoothingCheckbox.Checked;
            userInterface.EnableDesktopComposition = this.AllowDesktopCompositionCheckbox.Checked;
        }

        private void FillFavoriteExecuteBeforeOptions()
        {
            var exucutionOptions = this.Favorite.ExecuteBeforeConnect;
            exucutionOptions.Execute = this.chkExecuteBeforeConnect.Checked;
            exucutionOptions.Command = this.txtCommand.Text;
            exucutionOptions.CommandArguments = this.txtArguments.Text;
            exucutionOptions.InitialDirectory = this.txtInitialDirectory.Text;
            exucutionOptions.WaitForExit = this.chkWaitForExit.Checked;
        }

        private void FillFavoriteSSHOptions()
        {
            var sshOptions = this.Favorite.ProtocolProperties as SshOptions;
            if (sshOptions == null)
                return;

            sshOptions.AuthMethod = this.SSHPreferences.AuthMethod;
            sshOptions.CertificateKey = this.SSHPreferences.KeyTag;
            sshOptions.SSH1 = this.SSHPreferences.SSH1;
        }

        private void FillFavoriteICAOPtions()
        {
            var icaOptions = this.Favorite.ProtocolProperties as ICAOptions;
            if (icaOptions == null)
                return;

            icaOptions.ClientINI = this.ICAClientINI.Text;
            icaOptions.ServerINI = this.ICAServerINI.Text;
            icaOptions.EncryptionLevel = this.ICAEncryptionLevelCombobox.Text;
            icaOptions.EnableEncryption = this.ICAEnableEncryptionCheckbox.Checked;

            icaOptions.ApplicationName = this.ICAApplicationNameTextBox.Text;
            icaOptions.ApplicationPath = this.ICAApplicationPath.Text;
            icaOptions.ApplicationWorkingFolder = this.ICAWorkingFolder.Text;
        }

        private void FillFavoriteTSgatewayOptions(RdpOptions rdpOptions)
        {
            TsGwOptions tsgwOptions = rdpOptions.TsGateway;
            tsgwOptions.HostName = this.txtTSGWServer.Text;
            tsgwOptions.Security.Domain = this.txtTSGWDomain.Text;
            tsgwOptions.Security.UserName = this.txtTSGWUserName.Text;
            tsgwOptions.Security.Password = this.txtTSGWPassword.Text;
            tsgwOptions.SeparateLogin = this.chkTSGWlogin.Checked;
            tsgwOptions.CredentialSource = this.cmbTSGWLogonMethod.SelectedIndex;
            if (tsgwOptions.CredentialSource == 2)
                tsgwOptions.CredentialSource = 4;

            if (this.radTSGWenable.Checked)
            {
                if (this.chkTSGWlocalBypass.Checked)
                    tsgwOptions.UsageMethod = 2;
                else
                    tsgwOptions.UsageMethod = 1;
            }
            else
            {
                if (this.chkTSGWlocalBypass.Checked)
                    tsgwOptions.UsageMethod = 4;
                else
                    tsgwOptions.UsageMethod = 0;
            }
        }

        private void FillFavoriteRdpOptions()
        {
            var rdpOptions = this.Favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions == null)
                return;

            FillFavoriteRdpSecurity(rdpOptions);
            FillFavoriteRdpInterfaceOptions(rdpOptions);
            FillFavoriteRdpDisplayOptions(rdpOptions);
            FillFavoriteRdpRedirectOptions(rdpOptions);
            FillFavoriteTSgatewayOptions(rdpOptions);
            FillFavoriteRdpTimeOutOptions(rdpOptions);
        }

        private void FillFavoriteRdpDisplayOptions(RdpOptions rdpOptions)
        {
            rdpOptions.ConnectToConsole = this.chkConnectToConsole.Checked;
            rdpOptions.UserInterface.DisableWallPaper = this.chkDisableWallpaper.Checked;
            rdpOptions.UserInterface.DisableCursorBlinking = this.chkDisableCursorBlinking.Checked;
            rdpOptions.UserInterface.DisableCursorShadow = this.chkDisableCursorShadow.Checked;
            rdpOptions.UserInterface.DisableFullWindowDrag = this.chkDisableFullWindowDrag.Checked;
            rdpOptions.UserInterface.DisableMenuAnimations = this.chkDisableMenuAnimations.Checked;
            rdpOptions.UserInterface.DisableTheming = this.chkDisableThemes.Checked;
        }

        private void FillFavoriteRdpRedirectOptions(RdpOptions rdpOptions)
        {
            rdpOptions.Redirect.Drives = this.RedirectedDrives;
            rdpOptions.Redirect.Ports = this.chkSerialPorts.Checked;
            rdpOptions.Redirect.Printers = this.chkPrinters.Checked;
            rdpOptions.Redirect.Clipboard = this.chkRedirectClipboard.Checked;
            rdpOptions.Redirect.Devices = this.RedirectDevices;
            rdpOptions.Redirect.SmartCards = this.chkRedirectSmartcards.Checked;
            rdpOptions.Redirect.Sounds = (RemoteSounds)this.cmbSounds.SelectedIndex;
        }

        private void FillFavoriteSecurity()
        {
            ICredentialSet selectedCredential = this.CredentialDropdown.SelectedItem as ICredentialSet;
            this.Favorite.Security.Credential = selectedCredential == null ? Guid.Empty : selectedCredential.Id;

            this.Favorite.Security.Domain = this.cmbDomains.Text;
            this.Favorite.Security.UserName = this.cmbUsers.Text;
            if (this.chkSavePassword.Checked)
            {
                if (this.txtPassword.Text != HIDDEN_PASSWORD)
                    this.Favorite.Security.Password = this.txtPassword.Text;
                else
                    this.Favorite.Security.Password = this.favoritePassword;
            }
            else
            {
                this.Favorite.Security.Password = String.Empty;
            }
        }

        private void FillFavoriteVncOptions()
        {
            var vncOptions = this.Favorite.ProtocolProperties as VncOptions;
            if (vncOptions == null)
                return;

            vncOptions.AutoScale = this.vncAutoScaleCheckbox.Checked;
            vncOptions.DisplayNumber = (Int32)this.vncDisplayNumberInput.Value;
            vncOptions.ViewOnly = this.VncViewOnlyCheckbox.Checked;
        }

        private void FillFavoriteVmrcOptions()
        {
            var vmrcOptions = this.Favorite.ProtocolProperties as VMRCOptions;
            if (vmrcOptions == null)
                return;

            vmrcOptions.AdministratorMode = this.VMRCAdminModeCheckbox.Checked;
            vmrcOptions.ReducedColorsMode = this.VMRCReducedColorsCheckbox.Checked;
        }

        private void SaveDefaultFavorite()
        {
            this.Favorite.Name = String.Empty;
            this.Favorite.ServerName = String.Empty;
            this.Favorite.Notes = String.Empty;
            this.Favorite.Security.Domain = String.Empty;
            this.Favorite.Security.UserName = String.Empty;
            this.Favorite.Security.Password = String.Empty;

            var rdpOptions = this.Favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions != null)
            {
                rdpOptions.Security.Enabled = false;
                rdpOptions.Security.WorkingFolder = String.Empty;
                rdpOptions.Security.StartProgram = String.Empty;
                rdpOptions.FullScreen = false;
            }

            var defaultFavorite = ModelConverterV2ToV1.ConvertToFavorite(this.Favorite);
            Settings.SaveDefaultFavorite(defaultFavorite);
        }

        private void CommitFavoriteChanges()
        {
            Settings.StartDelayedUpdate();
            Persistence.Instance.StartDelayedUpdate();
            if (this.editedId == Guid.Empty)
            {
                PersistedFavorites.Add(this.Favorite);
                if (this.ShowOnToolbar)
                    Settings.AddFavoriteButton(this.Favorite.Id);
            }
            else
            {
                OrganizeFavoritesForm.UpdateFavoritePreservingDuplicitNames(this.oldName, this.Favorite.Name, this.Favorite);
                Settings.EditFavoriteButton(this.editedId, this.Favorite.Id, this.ShowOnToolbar);
            }

            List<IGroup> updatedGroups = this.GetNewlySelectedGroups();
            PersistedFavorites.UpdateFavorite(this.Favorite, updatedGroups);
            Persistence.Instance.SaveAndFinishDelayedUpdate();
            Settings.SaveAndFinishDelayedUpdate();
        }

        private void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(this, message, Terminals.Program.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Confirms changes into the favorite tags and returns collection of newly assigned tags.
        /// </summary>
        private List<IGroup> GetNewlySelectedGroups()
        {
            return this.lvConnectionTags.Items.Cast<GroupListViewItem>()
                 .Select(candidate => candidate.FavoritesGroup)
                 .ToList();
        }

        private bool IsServerNameValid()
        {
            if (ConnectionManager.IsProtocolWebBased(this.ProtocolComboBox.Text))
                return true;

            string serverName = this.cmbServers.Text.Trim();
            return this.IsServerNameValid(serverName);
        }

        private bool IsServerNameValid(string serverName)
        {
            if (String.IsNullOrEmpty(serverName))
            {
                this.RerportErrorInServerName("Server name was not specified.");
                return false;
            }

            if (Settings.ForceComputerNamesAsURI &&
                Uri.CheckHostName(serverName) == UriHostNameType.Unknown)
            {
                this.RerportErrorInServerName("Server name is not in the correct format.");
                return false;
            }

            return true;
        }

        private void RerportErrorInServerName(string message)
        {
            ShowErrorMessageBox(message);
            this.cmbServers.Focus();
        }

        private bool IsPortValid()
        {
            Int32 result;
            if (Int32.TryParse(this.txtPort.Text, out result) && result > 0 && result < 65536)
                return true;

            ShowErrorMessageBox("Port must be a number between 0 and 65535");
            this.txtPort.Focus();
            return false;
        }

        private void SetOkButtonState()
        {
            if (this.httpUrlTextBox.Visible)
            {
                Uri testUri = GetFullUrlFromHttpTextBox();
                this.btnSave.Enabled = testUri != null;
            }
            else
            {
                this.btnSave.Enabled = this.cmbServers.Text != String.Empty;
            }
        }

        private static void GetServerAndPort(String Connection, out String Server, out Int32 Port)
        {
            Server = Connection;
            Port = ConnectionManager.RDPPort;
            if (Connection != null && Connection.Trim() != String.Empty && Connection.Contains(":"))
            {
                String server = Connection.Substring(0, Connection.IndexOf(":"));
                String rawPort = Connection.Substring(Connection.IndexOf(":") + 1);
                Int32 port = ConnectionManager.RDPPort;
                if (rawPort != null && rawPort.Trim() != String.Empty)
                {
                    rawPort = rawPort.Trim();
                    Int32.TryParse(rawPort, out port);
                }

                Server = server;
                Port = port;
            }
        }

        private void AddGroup()
        {
            string newGroupName = this.txtTag.Text;
            if (!String.IsNullOrEmpty(newGroupName))
            {
                IGroup candidate = FavoritesFactory.GetOrCreateNewGroup(newGroupName);
                this.AddGroupIfNotAlreadyThere(candidate);
            }
        }

        private void AddGroupsToFavorite()
        {
            foreach (GroupListViewItem groupItem in this.AllTagsListView.SelectedItems)
            {
                this.AddGroupIfNotAlreadyThere(groupItem.FavoritesGroup);
            }
        }

        private void AddGroupIfNotAlreadyThere(IGroup selectedGroup)
        {
            // this also prevents duplicities in newly created groups not stored in persistence yet
            bool containsName = SelectedGroupsContainGroupName(selectedGroup);
            if (!containsName)
            {
                var selectedGroupItem = new GroupListViewItem(selectedGroup);
                this.lvConnectionTags.Items.Add(selectedGroupItem);
            }
        }

        private bool SelectedGroupsContainGroupName(IGroup selectedGroup)
        {
            return this.lvConnectionTags.Items.Cast<ListViewItem>()
                .Any(candidate => candidate.Text == selectedGroup.Name);
        }

        private void DeleteGroup()
        {
            foreach (ListViewItem groupItem in this.lvConnectionTags.SelectedItems)
            {
                this.lvConnectionTags.Items.Remove(groupItem);
            }
        }

        private void SetControlsForWeb()
        {
            this.lblServerName.Text = "Url:";
            this.cmbServers.Enabled = false;
            this.txtPort.Enabled = false;
            this.httpUrlTextBox.Enabled = true;
            this.httpUrlTextBox.Visible = true;
        }

        private void SetControlsForRas()
        {
            this.cmbServers.Items.Clear();
            this.LoadDialupConnections();
            this.RASGroupBox.Enabled = true;
            this.txtPort.Enabled = false;
            this.RASDetailsListBox.Items.Clear();
        }

        private void SetControlsForRdp()
        {
            this.DisplaySettingsGroupBox.Enabled = true;
            this.LocalResourceGroupBox.Enabled = true;
            this.ExtendedSettingsGgroupBox.Enabled = true;
            this.SecuritySettingsGroupBox.Enabled = true;
            this.TerminalGwLoginSettingsGroupBox.Enabled = true;
            this.TerminalGwSettingsGroupBox.Enabled = true;

            this.chkConnectToConsole.Enabled = true;
        }

        private void SetControlsProtocolIndependent()
        {
            this.lblServerName.Text = "Computer:";
            this.cmbServers.Enabled = true;
            this.txtPort.Enabled = true;

            this.DisplaySettingsGroupBox.Enabled = false;
            this.LocalResourceGroupBox.Enabled = false;
            this.ExtendedSettingsGgroupBox.Enabled = false;
            this.SecuritySettingsGroupBox.Enabled = false;
            this.TerminalGwLoginSettingsGroupBox.Enabled = false;
            this.TerminalGwSettingsGroupBox.Enabled = false;

            this.VmrcGroupBox.Enabled = false;
            this.ConsoleGroupBox.Enabled = false;
            this.RASGroupBox.Enabled = false;
            this.IcaGroupBox.Enabled = false;
            this.VncGroupBox.Enabled = false;
            this.SshGroupBox.Enabled = false;

            this.httpUrlTextBox.Enabled = false;
            this.httpUrlTextBox.Visible = false;
            this.txtPort.Enabled = true;
        }

        private void TryToAssignNewToolbarIcon(string newImagefilePath)
        {
            try
            {
                this.TryToLoadSelectedImage(newImagefilePath);
            }
            catch (Exception exception)
            {
                Logging.Log.Info("Set Terminal Image Failed", exception);
                this._currentToolBarFileName = String.Empty;
                MessageBox.Show("You have chosen an invalid image. Try again.");
            }
        }

        private void TryToLoadSelectedImage(string newImagefilePath)
        {
            string newFileInThumbsDir = FavoriteIcons.CopySelectedImageToThumbsDir(newImagefilePath);
            this.pictureBox2.Image = Image.FromFile(newImagefilePath);
            this._currentToolBarFileName = newFileInThumbsDir;
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

        private Uri GetFullUrlFromHttpTextBox()
        {
            string newUrlText = this.httpUrlTextBox.Text.ToLower();
            string protocolPrefix = this.ProtocolComboBox.Text.ToLower();
            if (!newUrlText.StartsWith(ConnectionManager.HTTP.ToLower()) &&
                !newUrlText.StartsWith(ConnectionManager.HTTPS.ToLower()))
                newUrlText = String.Format("{0}://{1}", protocolPrefix, newUrlText);
            return WebOptions.TryParseUrl(newUrlText);
        }

        #endregion
    }
}
