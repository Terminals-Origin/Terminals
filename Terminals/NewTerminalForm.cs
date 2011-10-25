using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using FalafelSoftware.TransPort;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Network.Servers;

namespace Terminals
{
    internal partial class NewTerminalForm : Form
    {
        private TerminalServerManager _terminalServerManager = new TerminalServerManager();
        private Dictionary<string, RASENTRY> _dialupList = new Dictionary<string, RASENTRY>();
        // todo dangerous usage of favorite field - it can be null for some cases
        private FavoriteConfigurationElement _favorite;
        private Boolean _showOnToolbar;
        private String _currentToolBarFileName;
        private String _oldName;
        private List<String> oldTags = new List<string>();
        private const String HIDDEN_PASSWORD = "****************";
        private String favoritePassword = string.Empty;
        internal List<String> _redirectedDrives = new List<String>();
        internal Boolean _redirectDevices = false;

        #region Constructors

        public NewTerminalForm(String serverName)
        {
            this.InitializeComponent();
            this.Init(null, serverName);
        }

        public NewTerminalForm(FavoriteConfigurationElement favorite)
        {
            this.InitializeComponent();
            this.Init(favorite, String.Empty);
        }

        #endregion

        #region Properties

        public new TerminalFormDialogResult DialogResult { get; private set; }

        public FavoriteConfigurationElement Favorite
        {
            get
            {
                return this._favorite;
            }
        }

        public Boolean ShowOnToolbar
        {
            get
            {
                return this._showOnToolbar;
            }
        }

        #endregion

        #region Private Developer made

        private void Init(FavoriteConfigurationElement favorite, String serverName)
        {
            this.LoadMRUs();
            this.SetOkButtonState();
            this.SSHPreferences.Keys = Settings.SSHKeys;

            // move following line down to default value only once smart card access worked out.
            this.cmbTSGWLogonMethod.SelectedIndex = 0;

            if (favorite == null)
            {
                this.FillCredentials(null);

                FavoriteConfigurationElement defaultFav = Settings.GetDefaultFavorite();
                if (defaultFav != null)
                {
                    this.FillControls(defaultFav);
                }
                else
                {
                    this.cmbResolution.SelectedIndex = 7;
                    this.cmbColors.SelectedIndex = 1;
                    this.cmbSounds.SelectedIndex = 2;
                    this.ProtocolComboBox.SelectedIndex = 0;
                }

                String server = serverName;
                Int32 port;
                GetServerAndPort(serverName, out server, out port);
                this.cmbServers.Text = server;
                this.txtPort.Text = port.ToString();
            }
            else
            {
                this._oldName = favorite.Name;
                this.oldTags = favorite.TagList;
                this.Text = "Edit Connection";
                this.FillControls(favorite);
            }
        }

        private void FillCredentials(String credentialName)
        {
            this.CredentialDropdown.Items.Clear();
            List<CredentialSet> creds = StoredCredentials.Instance.Items;
            this.CredentialDropdown.Items.Add("(custom)");

            Int32 selIndex = 0;
            if (creds != null)
            {
                foreach (CredentialSet item in creds)
                {
                    Int32 index = this.CredentialDropdown.Items.Add(item);
                    if (!String.IsNullOrEmpty(credentialName) && credentialName == item.Name)
                        selIndex = index;
                }
            }

            this.CredentialDropdown.SelectedIndex = selIndex;
        }

        private void NewTerminalForm_Load(Object sender, EventArgs e)
        {
            this.SuspendLayout();
            this._terminalServerManager.Dock = DockStyle.Fill;
            this._terminalServerManager.Location = new Point(0, 0);
            this._terminalServerManager.Name = "terminalServerManager1";
            this._terminalServerManager.Size = new Size(748, 309);
            this._terminalServerManager.TabIndex = 0;
            this.tabPage10.Controls.Add(_terminalServerManager);

            foreach (String tag in Settings.Tags)
            {
                ListViewItem lvi = new ListViewItem(tag);
                this.AllTagsListView.Items.Add(lvi);
            }

            this.ResumeLayout(true);
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
            this.txtTag.AutoCompleteCustomSource.AddRange(Settings.Tags);
        }

        private void SaveMRUs()
        {
            Settings.AddServerMRUItem(cmbServers.Text);
            Settings.AddDomainMRUItem(cmbDomains.Text);
            Settings.AddUserMRUItem(cmbUsers.Text);
        }

        private void FillControls(FavoriteConfigurationElement favorite)
        {
            this.consolePreferences.FillControls(favorite);

            this.NewWindowCheckbox.Checked = favorite.NewWindow;

            this.ProtocolComboBox.SelectedItem = favorite.Protocol;
            this.VMRCAdminModeCheckbox.Checked = favorite.VMRCAdministratorMode;

            this.vncAutoScaleCheckbox.Checked = favorite.VncAutoScale;
            this.vncDisplayNumberInput.Value = favorite.VncDisplayNumber;
            this.VncViewOnlyCheckbox.Checked = favorite.VncViewOnly;

            this.VMRCReducedColorsCheckbox.Checked = favorite.VMRCReducedColorsMode;
            this.txtName.Text = favorite.Name;
            this.cmbServers.Text = favorite.ServerName;
            this.cmbDomains.Text = favorite.DomainName;
            this.cmbUsers.Text = favorite.UserName;
            if (favorite.Password != String.Empty)
            {
                this.txtPassword.Text = HIDDEN_PASSWORD;
                this.favoritePassword = favorite.Password;
            }
            else
            {
                this.txtPassword.Text = String.Empty;
            }

            this.chkSavePassword.Checked = favorite.Password != String.Empty;
            this.cmbResolution.SelectedIndex = (Int32)favorite.DesktopSize;
            this.cmbColors.SelectedIndex = (Int32)favorite.Colors;
            this.chkConnectToConsole.Checked = favorite.ConnectToConsole;

            this.chkAddtoToolbar.Checked = Settings.HasToolbarButton(favorite.Name);
            this._redirectedDrives = favorite.RedirectedDrives;
            this.chkSerialPorts.Checked = favorite.RedirectPorts;
            this.chkPrinters.Checked = favorite.RedirectPrinters;
            this.chkRedirectClipboard.Checked = favorite.RedirectClipboard;
            this._redirectDevices = favorite.RedirectDevices;
            this.chkRedirectSmartcards.Checked = favorite.RedirectSmartCards;
            this.cmbSounds.SelectedIndex = (Int32)favorite.Sounds;

            switch (favorite.TsgwUsageMethod)
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

            this.txtTSGWServer.Text = favorite.TsgwHostname;
            this.txtTSGWDomain.Text = favorite.TsgwDomain;
            this.txtTSGWUserName.Text = favorite.TsgwUsername;
            this.txtTSGWPassword.Text = favorite.TsgwPassword;
            this.chkTSGWlogin.Checked = favorite.TsgwSeparateLogin;
            this.cmbTSGWLogonMethod.SelectedIndex = favorite.TsgwCredsSource;

            this.httpUrlTextBox.Text = favorite.Url;

            this.txtPort.Text = favorite.Port.ToString();
            this.txtDesktopShare.Text = favorite.DesktopShare;
            this.chkExecuteBeforeConnect.Checked = favorite.ExecuteBeforeConnect;
            this.txtCommand.Text = favorite.ExecuteBeforeConnectCommand;
            this.txtArguments.Text = favorite.ExecuteBeforeConnectArgs;
            this.txtInitialDirectory.Text = favorite.ExecuteBeforeConnectInitialDirectory;
            this.chkWaitForExit.Checked = favorite.ExecuteBeforeConnectWaitForExit;

            ReloadTagsListViewItems(favorite);

            if (favorite.ToolBarIcon != null && File.Exists(favorite.ToolBarIcon))
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
            this.DisableControlAltDeleteCheckbox.Checked = favorite.DisableControlAltDelete;
            this.AcceleratorPassthroughCheckBox.Checked = favorite.AcceleratorPassthrough;
            this.EnableCompressionCheckbox.Checked = favorite.EnableCompression;
            this.EnableBitmapPersistanceCheckbox.Checked = favorite.BitmapPeristence;
            this.EnableTLSAuthenticationCheckbox.Checked = favorite.EnableTLSAuthentication;
            this.EnableNLAAuthenticationCheckbox.Checked = favorite.EnableNLAAuthentication;
            this.AllowBackgroundInputCheckBox.Checked = favorite.AllowBackgroundInput;

            this.chkDisableCursorShadow.Checked = false;
            this.chkDisableCursorBlinking.Checked = false;
            this.chkDisableFullWindowDrag.Checked = false;
            this.chkDisableMenuAnimations.Checked = false;
            this.chkDisableThemes.Checked = false;
            this.chkDisableWallpaper.Checked = false;

            if (favorite.PerformanceFlags > 0)
            {
                this.chkDisableCursorShadow.Checked = favorite.DisableCursorShadow;
                this.chkDisableCursorBlinking.Checked = favorite.DisableCursorBlinking;
                this.chkDisableFullWindowDrag.Checked = favorite.DisableFullWindowDrag;
                this.chkDisableMenuAnimations.Checked = favorite.DisableMenuAnimations;
                this.chkDisableThemes.Checked = favorite.DisableTheming;
                this.chkDisableWallpaper.Checked = favorite.DisableWallPaper;
                this.AllowFontSmoothingCheckbox.Checked = favorite.EnableFontSmoothing;
                this.AllowDesktopCompositionCheckbox.Checked = favorite.EnableDesktopComposition;
            }

            this.widthUpDown.Value = favorite.DesktopSizeWidth;
            this.heightUpDown.Value = favorite.DesktopSizeHeight;

            this.ICAClientINI.Text = favorite.IcaClientINI;
            this.ICAServerINI.Text = favorite.IcaServerINI;
            this.ICAEncryptionLevelCombobox.Text = favorite.IcaEncryptionLevel;
            this.ICAEnableEncryptionCheckbox.Checked = favorite.IcaEnableEncryption;
            this.ICAEncryptionLevelCombobox.Enabled = ICAEnableEncryptionCheckbox.Checked;

            this.NotesTextbox.Text = favorite.Notes;

            this.SSHPreferences.AuthMethod = favorite.AuthMethod;
            this.SSHPreferences.KeyTag = favorite.KeyTag;
            this.SSHPreferences.SSH1 = favorite.SSH1;

            this.FillCredentials(favorite.Credential);
        }

        private void ReloadTagsListViewItems(FavoriteConfigurationElement favorite)
        {
            List<string> tagsArray = favorite.TagList;
            this.lvConnectionTags.Items.Clear();
            foreach (String tag in tagsArray)
            {
                this.lvConnectionTags.Items.Add(tag, tag, -1);
            }
        }

        private Boolean FillFavorite(Boolean defaultFav)
        {
            try
            {
                if (this._favorite == null)
                    this._favorite = new FavoriteConfigurationElement();

                this.consolePreferences.FillFavorite(_favorite);

                this._favorite.Name = (String.IsNullOrEmpty(this.txtName.Text) ? this.cmbServers.Text : this.txtName.Text);

                this._favorite.VMRCAdministratorMode = this.VMRCAdminModeCheckbox.Checked;
                this._favorite.VMRCReducedColorsMode = this.VMRCReducedColorsCheckbox.Checked;

                this._favorite.VncAutoScale = this.vncAutoScaleCheckbox.Checked;
                this._favorite.VncDisplayNumber = (Int32)this.vncDisplayNumberInput.Value;
                this._favorite.VncViewOnly = this.VncViewOnlyCheckbox.Checked;

                this._favorite.NewWindow = this.NewWindowCheckbox.Checked;

                this._favorite.Protocol = this.ProtocolComboBox.SelectedItem.ToString();
                this._favorite.Port = this.ValidatePort();

                if (!defaultFav)
                    this._favorite.ServerName = ValidateServer(this.cmbServers.Text);

                CredentialSet set = (this.CredentialDropdown.SelectedItem as CredentialSet);
                this._favorite.Credential = (set == null ? String.Empty : set.Name);

                this._favorite.DomainName = this.cmbDomains.Text;
                this._favorite.UserName = this.cmbUsers.Text;
                if (this.chkSavePassword.Checked)
                {
                    if (this.txtPassword.Text != HIDDEN_PASSWORD)
                        this._favorite.Password = this.txtPassword.Text;
                    else
                        this._favorite.Password = this.favoritePassword;
                }
                else
                {
                    this._favorite.Password = String.Empty;
                }

                this._favorite.DesktopSize = (DesktopSize)this.cmbResolution.SelectedIndex;
                this._favorite.Colors = (Colors)this.cmbColors.SelectedIndex;
                this._favorite.ConnectToConsole = this.chkConnectToConsole.Checked;
                this._favorite.DisableWallPaper = this.chkDisableWallpaper.Checked;
                this._favorite.DisableCursorBlinking = this.chkDisableCursorBlinking.Checked;
                this._favorite.DisableCursorShadow = this.chkDisableCursorShadow.Checked;
                this._favorite.DisableFullWindowDrag = this.chkDisableFullWindowDrag.Checked;
                this._favorite.DisableMenuAnimations = this.chkDisableMenuAnimations.Checked;
                this._favorite.DisableTheming = this.chkDisableThemes.Checked;

                this._favorite.RedirectedDrives = this._redirectedDrives;
                this._favorite.RedirectPorts = this.chkSerialPorts.Checked;
                this._favorite.RedirectPrinters = this.chkPrinters.Checked;
                this._favorite.RedirectClipboard = this.chkRedirectClipboard.Checked;
                this._favorite.RedirectDevices = this._redirectDevices;
                this._favorite.RedirectSmartCards = this.chkRedirectSmartcards.Checked;
                this._favorite.Sounds = (RemoteSounds)this.cmbSounds.SelectedIndex;
                this._showOnToolbar = this.chkAddtoToolbar.Checked;

                if (this.radTSGWenable.Checked)
                {
                    if (this.chkTSGWlocalBypass.Checked)
                        this._favorite.TsgwUsageMethod = 2;
                    else
                        this._favorite.TsgwUsageMethod = 1;
                }
                else
                {
                    if (this.chkTSGWlocalBypass.Checked)
                        this._favorite.TsgwUsageMethod = 4;
                    else
                        this._favorite.TsgwUsageMethod = 0;
                }

                this._favorite.TsgwHostname = this.txtTSGWServer.Text;
                this._favorite.TsgwDomain = this.txtTSGWDomain.Text;
                this._favorite.TsgwUsername = this.txtTSGWUserName.Text;
                this._favorite.TsgwPassword = this.txtTSGWPassword.Text;
                this._favorite.TsgwSeparateLogin = this.chkTSGWlogin.Checked;
                this._favorite.TsgwCredsSource = this.cmbTSGWLogonMethod.SelectedIndex;

                this._favorite.DesktopShare = this.txtDesktopShare.Text;
                this._favorite.ExecuteBeforeConnect = this.chkExecuteBeforeConnect.Checked;
                this._favorite.ExecuteBeforeConnectCommand = this.txtCommand.Text;
                this._favorite.ExecuteBeforeConnectArgs = this.txtArguments.Text;
                this._favorite.ExecuteBeforeConnectInitialDirectory = this.txtInitialDirectory.Text;
                this._favorite.ExecuteBeforeConnectWaitForExit = this.chkWaitForExit.Checked;
                this._favorite.ToolBarIcon = this._currentToolBarFileName;

                this._favorite.ICAApplicationName = this.ICAApplicationNameTextBox.Text;
                this._favorite.ICAApplicationPath = this.ICAApplicationPath.Text;
                this._favorite.ICAApplicationWorkingFolder = this.ICAWorkingFolder.Text;

                //extended settings
                if (this.ShutdownTimeoutTextBox.Text.Trim() != String.Empty)
                {
                    this._favorite.ShutdownTimeout = Convert.ToInt32(this.ShutdownTimeoutTextBox.Text.Trim());
                }

                if (this.OverallTimeoutTextbox.Text.Trim() != String.Empty)
                {
                    this._favorite.OverallTimeout = Convert.ToInt32(this.OverallTimeoutTextbox.Text.Trim());
                }

                if (this.SingleTimeOutTextbox.Text.Trim() != String.Empty)
                {
                    this._favorite.ConnectionTimeout = Convert.ToInt32(this.SingleTimeOutTextbox.Text.Trim());
                }

                if (this.IdleTimeoutMinutesTextBox.Text.Trim() != String.Empty)
                {
                    this._favorite.IdleTimeout = Convert.ToInt32(this.IdleTimeoutMinutesTextBox.Text.Trim());
                }

                this._favorite.EnableSecuritySettings = this.SecuritySettingsEnabledCheckbox.Checked;

                if (this.SecuritySettingsEnabledCheckbox.Checked)
                {
                    this._favorite.SecurityWorkingFolder = this.SecurityWorkingFolderTextBox.Text;
                    this._favorite.SecurityStartProgram = this.SecuriytStartProgramTextbox.Text;
                    this._favorite.SecurityFullScreen = this.SecurityStartFullScreenCheckbox.Checked;
                }

                this._favorite.GrabFocusOnConnect = this.GrabFocusOnConnectCheckbox.Checked;
                this._favorite.EnableEncryption = this.EnableEncryptionCheckbox.Checked;
                this._favorite.DisableWindowsKey = this.DisableWindowsKeyCheckbox.Checked;
                this._favorite.DoubleClickDetect = this.DetectDoubleClicksCheckbox.Checked;
                this._favorite.DisplayConnectionBar = this.DisplayConnectionBarCheckbox.Checked;
                this._favorite.DisableControlAltDelete = this.DisableControlAltDeleteCheckbox.Checked;
                this._favorite.AcceleratorPassthrough = this.AcceleratorPassthroughCheckBox.Checked;
                this._favorite.EnableCompression = this.EnableCompressionCheckbox.Checked;
                this._favorite.BitmapPeristence = this.EnableBitmapPersistanceCheckbox.Checked;
                this._favorite.EnableTLSAuthentication = this.EnableTLSAuthenticationCheckbox.Checked;
                this._favorite.EnableNLAAuthentication = this.EnableNLAAuthenticationCheckbox.Checked;
                this._favorite.AllowBackgroundInput = this.AllowBackgroundInputCheckBox.Checked;

                this._favorite.EnableFontSmoothing = this.AllowFontSmoothingCheckbox.Checked;
                this._favorite.EnableDesktopComposition = this.AllowDesktopCompositionCheckbox.Checked;

                this._favorite.DesktopSizeWidth = (Int32)this.widthUpDown.Value;
                this._favorite.DesktopSizeHeight = (Int32)this.heightUpDown.Value;

                this._favorite.Url = this.httpUrlTextBox.Text;

                this._favorite.IcaClientINI = this.ICAClientINI.Text;
                this._favorite.IcaServerINI = this.ICAServerINI.Text;
                this._favorite.IcaEncryptionLevel = this.ICAEncryptionLevelCombobox.Text;
                this._favorite.IcaEnableEncryption = this.ICAEnableEncryptionCheckbox.Checked;

                this._favorite.Notes = this.NotesTextbox.Text;

                this._favorite.KeyTag = this.SSHPreferences.KeyTag;
                this._favorite.SSH1 = this.SSHPreferences.SSH1;
                this._favorite.AuthMethod = this.SSHPreferences.AuthMethod;

                List<String> updatedTags = UpdateFavoriteTags();

                if (defaultFav)
                {
                    this._favorite.Name = String.Empty;
                    this._favorite.ServerName = String.Empty;
                    this._favorite.DomainName = String.Empty;
                    this._favorite.UserName = String.Empty;
                    this._favorite.Password = String.Empty;
                    this._favorite.Notes = String.Empty;
                    this._favorite.EnableSecuritySettings = false;
                    this._favorite.SecurityWorkingFolder = String.Empty;
                    this._favorite.SecurityStartProgram = String.Empty;
                    this._favorite.SecurityFullScreen = false;
                    this._favorite.Url = String.Empty;
                    Settings.SaveDefaultFavorite(this._favorite);
                }
                else
                {
                    if (String.IsNullOrEmpty(this._oldName))
                        Settings.AddFavorite(this._favorite, this._showOnToolbar);
                    else
                    {
                        Settings.EditFavorite(this._oldName, this._favorite, this._showOnToolbar);
                        UpdateTags(updatedTags);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Logging.Log.Info("Fill Favorite Failed", e);
                MessageBox.Show(this, e.Message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Call this after favorite was saved, otherwise the removed, otherwise not used tags wouldnt be removed
        /// </summary>
        private void UpdateTags(List<string> updatedTags)
        {
            List<String> tagsToRemove = ListStringHelper.GetMissingSourcesInTarget(this.oldTags, updatedTags);
            List<String> tagsToAdd = ListStringHelper.GetMissingSourcesInTarget(updatedTags, this.oldTags);
            Settings.AddTags(tagsToAdd);
            Settings.DeleteTags(tagsToRemove);
        }

        /// <summary>
        /// Confirms changes into the favorite tags and returns collection of newly assigned tags.
        /// </summary>
        private List<String> UpdateFavoriteTags()
        {
            List<String> updatedTags = new List<String>();
            foreach (ListViewItem listViewItem in this.lvConnectionTags.Items)
                updatedTags.Add(listViewItem.Text);

            this._favorite.Tags = String.Join(",", updatedTags.ToArray());
            return updatedTags;
        }

        private static String ValidateServer(String serverName)
        {
            serverName = serverName.Trim();
            if (Settings.ForceComputerNamesAsURI)
            {
                if (Uri.CheckHostName(serverName) == UriHostNameType.Unknown)
                    throw new ArgumentException("Server name is not valid");
            }
            else
            {
                if (String.IsNullOrEmpty(serverName))
                    throw new ArgumentException("Server name was not specified.");

                if (serverName.Length < 0)
                    throw new ArgumentException("Server name was not specified.");
            }

            return serverName;
        }

        private Int32 ValidatePort()
        {
            Int32 result;
            if (Int32.TryParse(this.txtPort.Text, out result) && result < 65536 && result > 0)
                return result;

            //string protocol = this.ProtocolComboBox.SelectedItem.ToString();
            //result = ConnectionManager.GetPort(protocol);
            //this.txtPort.Text = result.ToString();

            // todo replace Exceptions usage in case, where it isnt relevant
            throw new ArgumentException("Port must be a number between 0 and 65535");
        }

        private void SetOkButtonState()
        {
            this.btnSave.Enabled = this.cmbServers.Text != String.Empty;
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

        private void AddTag()
        {
            if (!String.IsNullOrEmpty(this.txtTag.Text))
            {
                AddTagIfNotAlreadyThere(this.txtTag.Text);
            }
        }

        private void AddTagsToFavorite()
        {
            foreach (ListViewItem lv in this.AllTagsListView.SelectedItems)
            {
                AddTagIfNotAlreadyThere(lv.Text);
            }
        }

        private void AddTagIfNotAlreadyThere(String selectedTag)
        {
            ListViewItem[] items = this.lvConnectionTags.Items.Find(selectedTag, false);
            if (items.Length == 0)
            {
                this.lvConnectionTags.Items.Add(selectedTag);
            }
        }

        private void DeleteTag()
        {
            foreach (ListViewItem item in this.lvConnectionTags.SelectedItems)
            {
                this.lvConnectionTags.Items.Remove(item);
            }
        }

        #endregion

        #region Private eventhandlers

        /// <summary>
        /// Overload ShowDialog and return custom result.
        /// </summary>
        /// <returns>Returns custom dialogresult.</returns>
        public new TerminalFormDialogResult ShowDialog()
        {
            base.ShowDialog();

            return this.DialogResult;
        }

        /// <summary>
        /// Save favorite and close form.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();

            if (this.FillFavorite(false))
                this.DialogResult = TerminalFormDialogResult.SaveAndClose;

            this.Close();
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
        /// Save favorite and clear form for a new favorite.
        /// </summary>
        private void saveNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this._favorite = null;
                this._oldName = String.Empty;
                this.Init(null, String.Empty);
                this.cmbServers.Focus();
            }
        }

        /// <summary>
        /// Save favorite and copy the current favorite settings, except favorite and connection name.
        /// </summary>
        private void saveCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this.txtName.Text = this._favorite.Name + "_(copy)";
                this._favorite = null;
                this._oldName = String.Empty;
                this.cmbServers.Text = String.Empty;
                this.cmbServers.Focus();
            }
        }

        private void control_TextChanged(object sender, EventArgs e)
        {
            this.SetOkButtonState();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            this.SetOkButtonState();
        }

        private void NewTerminalForm_Shown(object sender, EventArgs e)
        {
            this.cmbServers.Focus();
        }

        private void cmbServers_TextChanged(object sender, EventArgs e)
        {
            this.SetOkButtonState();
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

        private void btnAddNewTag_Click(object sender, EventArgs e)
        {
            this.AddTag();
        }

        private void btnRemoveTag_Click(object sender, EventArgs e)
        {
            this.DeleteTag();
        }

        private void lvConnectionTags_DoubleClick(object sender, EventArgs e)
        {
            this.DeleteTag();
        }

        private void btnSaveDefault_Click(object sender, EventArgs e)
        {
            this.contextMenuStripDefaults.Show(this.btnSaveDefault, 0, this.btnSaveDefault.Height);
        }

        private void saveCurrentSettingsAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FillFavorite(true);
        }

        private void removeSavedDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.RemoveDefaultFavorite();
        }

        private void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String defaultPort = ConnectionManager.VNCVMRCPort.ToString();

            this.cmbServers.Enabled = true;
            this.txtPort.Enabled = true;

            this.vncAutoScaleCheckbox.Enabled = false;
            this.vncDisplayNumberInput.Enabled = false;
            this.VncViewOnlyCheckbox.Enabled = false;

            this.groupBox1.Enabled = false;
            this.chkConnectToConsole.Enabled = false;
            this.LocalResourceGroupBox.Enabled = false;
            this.VMRCReducedColorsCheckbox.Enabled = false;
            this.VMRCAdminModeCheckbox.Enabled = false;
            this.RASGroupBox.Enabled = false;

            this.ICAApplicationNameTextBox.Enabled = false;
            this.ICAApplicationPath.Enabled = false;
            this.httpUrlTextBox.Enabled = false;
            this.txtPort.Enabled = true;

            if (this.ProtocolComboBox.Text == "RDP")
            {
                defaultPort = ConnectionManager.RDPPort.ToString();
                this.groupBox1.Enabled = true;
                this.chkConnectToConsole.Enabled = true;
                this.LocalResourceGroupBox.Enabled = true;
            }
            else if (this.ProtocolComboBox.Text == "VMRC")
            {
                this.VMRCReducedColorsCheckbox.Enabled = true;
                this.VMRCAdminModeCheckbox.Enabled = true;
            }
            else if (this.ProtocolComboBox.Text == "RAS")
            {
                this.cmbServers.Items.Clear();
                this.LoadDialupConnections();
                this.RASGroupBox.Enabled = true;
                this.txtPort.Enabled = false;
                this.RASDetailsListBox.Items.Clear();
            }
            else if (this.ProtocolComboBox.Text == "VNC")
            {
                //vnc settings
                this.vncAutoScaleCheckbox.Enabled = true;
                this.vncDisplayNumberInput.Enabled = true;
                this.VncViewOnlyCheckbox.Enabled = true;
            }
            else if (this.ProtocolComboBox.Text == "Telnet")
            {
                defaultPort = ConnectionManager.TelnetPort.ToString();
            }
            else if (this.ProtocolComboBox.Text == "SSH")
            {
                defaultPort = ConnectionManager.SSHPort.ToString();
            }
            else if (this.ProtocolComboBox.Text == "ICA Citrix")
            {
                this.ICAApplicationNameTextBox.Enabled = true;
                this.ICAApplicationPath.Enabled = true;
                defaultPort = ConnectionManager.ICAPort.ToString();
            }
            else if (ProtocolComboBox.Text == "HTTP")
            {
                this.cmbServers.Enabled = false;
                this.txtPort.Enabled = false;
                this.txtPort.Text = "80";
                this.httpUrlTextBox.Enabled = true;
                defaultPort = ConnectionManager.HTTPPort.ToString();
                this.tabControl1.SelectTab(HTTPTabPage);
            }
            else if (this.ProtocolComboBox.Text == "HTTPS")
            {
                this.cmbServers.Enabled = false;
                this.txtPort.Text = "443";
                this.txtPort.Enabled = false;
                this.httpUrlTextBox.Enabled = true;
                defaultPort = ConnectionManager.HTTPSPort.ToString();
                this.tabControl1.SelectTab(HTTPTabPage);
            }

            this.txtPort.Text = defaultPort;
        }

        private void cmbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProtocolComboBox.Text == "RAS")
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            String appFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.InitialDirectory = appFolder;
            openFileDialog.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select Custom Terminal Image .";
            String thumbsFolder = Path.Combine(appFolder, "Thumbs");
            if (!Directory.Exists(thumbsFolder))
                Directory.CreateDirectory(thumbsFolder);

            this._currentToolBarFileName = String.Empty;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //make it relative to the current application executable
                String filename = openFileDialog.FileName;
                try
                {
                    Image image = Image.FromFile(filename);
                    if (image != null)
                    {
                        this.pictureBox2.Image = image;
                        String newFile = Path.Combine(thumbsFolder, Path.GetFileName(filename));
                        if (newFile != filename && !File.Exists(newFile))
                            File.Copy(filename, newFile);
                        this._currentToolBarFileName = newFile;
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log.Info("Set Terminal Image Failed", ex);
                    this._currentToolBarFileName = String.Empty;
                    MessageBox.Show("You have chosen an invalid image. Try again.");
                }
            }
        }

        private void SecuritySettingsEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.panel2.Enabled = this.SecuritySettingsEnabledCheckbox.Checked;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.RDPTabPage)
            {
                this.RDPSubTabPage_SelectedIndexChanged(null, null);
            }
        }

        private void RDPSubTabPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.RDPSubTabPage.SelectedTab == this.tabPage10)
            {
                this._terminalServerManager.Connect(this.cmbServers.Text, true);
                this._terminalServerManager.Invalidate();
            }
        }

        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbResolution.Text == "Custom" || this.cmbResolution.Text == "Auto Scale")
                this.customSizePanel.Visible = true;
            else
                this.customSizePanel.Visible = false;
        }

        private void AllTagsAddButton_Click(object sender, EventArgs e)
        {
            AddTagsToFavorite();
        }

        private void ICAEnableEncryptionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.ICAEncryptionLevelCombobox.Enabled = this.ICAEnableEncryptionCheckbox.Checked;
        }

        private void httpUrlTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.ProtocolComboBox.Text == "HTTP" | this.ProtocolComboBox.Text == "HTTPS")
            {
                String url = this.httpUrlTextBox.Text;
                try
                {
                    Uri u = new Uri(url);
                    this.cmbServers.Text = u.Host;
                    this.txtPort.Text = u.Port.ToString();
                }
                catch (Exception ex)
                {
                    Logging.Log.Error("Http URL Parse Failed", ex);
                }
            }
        }

        private void AllTagsListView_DoubleClick(object sender, EventArgs e)
        {
            AddTagsToFavorite();
        }

        private void CredentialManagerPicturebox_Click(object sender, EventArgs e)
        {
            String cred = String.Empty;
            if (this.CredentialDropdown.SelectedItem.GetType() != typeof(string))
                cred = ((CredentialSet)this.CredentialDropdown.SelectedItem).Name;

            Credentials.CredentialManager mgr = new Credentials.CredentialManager();
            mgr.ShowDialog();
            this.FillCredentials(cred);
        }

        private void CredentialDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CredentialsPanel.Enabled = true;
            CredentialSet set = (this.CredentialDropdown.SelectedItem as CredentialSet);

            if (set != null)
            {
                this.CredentialsPanel.Enabled = false;
                this.cmbDomains.Text = set.Domain;
                this.cmbUsers.Text = set.Username;
                this.txtPassword.Text = set.SecretKey;
                this.chkSavePassword.Checked = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlTSGWlogon.Enabled = this.chkTSGWlogin.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlTSGWsettings.Enabled = this.radTSGWenable.Checked;
        }

        private void btnDrives_Click(object sender, EventArgs e)
        {
            DiskDrivesForm drivesForm = new DiskDrivesForm(this);
            drivesForm.ShowDialog(this);
        }

        #endregion
    }
}
