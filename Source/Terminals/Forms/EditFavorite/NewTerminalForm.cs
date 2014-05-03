using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using FalafelSoftware.TransPort;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Credentials;
using Terminals.Data;
using Terminals.Data.Validation;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Terminals.Network.Servers;

namespace Terminals
{
    /// <summary>
    /// Copy of related dialog, from which user controls will be extracted
    /// </summary>
    internal partial class NewTerminalFormCopy : Form
    {
        private NewTerminalFormValidator validator;
        private readonly TerminalServerManager terminalServerManager = new TerminalServerManager();
        private String currentToolBarFileName;
        internal Guid EditedId { get; set; }

        internal bool EditingNew { get { return this.EditedId == Guid.Empty; } }

        private String favoritePassword = string.Empty;
        internal const String HIDDEN_PASSWORD = "****************";

        private readonly IPersistence persistence;

        private IGroups PersistedGroups
        {
            get { return this.persistence.Groups; }
        }

        private IFavorites PersistedFavorites
        {
            get { return this.persistence.Favorites; }
        }

        internal string ServerNameText { get { return this.cmbServers.Text.Trim(); } }

        internal string ProtocolText { get { return this.ProtocolComboBox.Text; } }

        internal string PortText { get { return this.txtPort.Text; } }

        #region Constructors

        public NewTerminalFormCopy(IPersistence persistence, String serverName)
            : this()
        {
            this.persistence = persistence;
            this.InitializeValidations();
            this.Init(null, serverName);
        }

        public NewTerminalFormCopy(IPersistence persistence, IFavorite favorite)
            : this()
        {
            this.persistence = persistence;
            this.InitializeValidations();
            this.Init(favorite, String.Empty);
        }

        private NewTerminalFormCopy()
        {
            this.InitializeComponent();
            this.RedirectedDrives = new List<String>();
            this.RedirectDevices = false;
            this.SetErrorProviderIconsAlignment();
        }

        private void InitializeValidations()
        {
            // this.validator = new NewTerminalFormValidator(this.persistence, this);
            this.AssignValidatingEvents();
            this.RegisterValiationControls();
        }

        private void AssignValidatingEvents()
        {
            this.txtPort.Validating += this.validator.OnPortValidating;
            this.cmbServers.Validating += this.validator.OnServerNameValidating;
            this.httpUrlTextBox.Validating += this.validator.OnUrlValidating;
            this.ShutdownTimeoutTextBox.Validating += this.validator.OnValidateInteger;
            this.OverallTimeoutTextbox.Validating += this.validator.OnValidateInteger;
            this.SingleTimeOutTextbox.Validating += this.validator.OnValidateInteger;
            this.IdleTimeoutMinutesTextBox.Validating += this.validator.OnValidateInteger;
        }

        private void SetErrorProviderIconsAlignment()
        {
            this.errorProvider.SetIconAlignment(this.cmbServers, ErrorIconAlignment.MiddleLeft);
            this.errorProvider.SetIconAlignment(this.httpUrlTextBox, ErrorIconAlignment.MiddleLeft);
            this.errorProvider.SetIconAlignment(this.NotesTextbox, ErrorIconAlignment.TopLeft);
            this.errorProvider.SetIconAlignment(this.txtName, ErrorIconAlignment.MiddleLeft);
        }

        private void RegisterValiationControls()
        {
            this.validator.RegisterValidationControl("ServerName", this.cmbServers);
            this.validator.RegisterValidationControl(Validations.NAME_PROPERTY, this.txtName);
            this.validator.RegisterValidationControl("Port", this.txtPort);
            this.validator.RegisterValidationControl("Protocol", this.ProtocolComboBox);
            this.validator.RegisterValidationControl("Notes", this.NotesTextbox);
            this.validator.RegisterValidationControl("Command", this.txtCommand);
            this.validator.RegisterValidationControl("CommandArguments", this.txtArguments);
            this.validator.RegisterValidationControl("InitialDirectory", this.txtInitialDirectory);
        }

        #endregion

        #region Properties

        private new TerminalFormDialogResult DialogResult { get; set; }
        internal IFavorite Favorite { get; private set; }
        private bool ShowOnToolbar { get; set; }
        internal List<string> RedirectedDrives { get; set; }
        internal bool RedirectDevices { get; set; }

        #endregion

        #region Private form event handlers

        private void NewTerminalForm_Load(Object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.LoadCustomControlsState();
            IOrderedEnumerable<IGroup> sortedGroups = PersistedGroups.OrderBy(group => group.Name);
            BindGroupsToListView(this.AllTagsListView, sortedGroups);
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

        /// <summary>
        /// Save favorite and close form. If the form isnt valid the form control is focused.
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this.DialogResult = TerminalFormDialogResult.SaveAndClose;
                this.Close();
            }
        }

        private void BtnSaveDefault_Click(object sender, EventArgs e)
        {
            this.contextMenuStripDefaults.Show(this.btnSaveDefault, 0, this.btnSaveDefault.Height);
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

        private void RemoveSavedDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.RemoveDefaultFavorite();
        }

        /// <summary>
        /// Save favorite, close form and immediatly connect to the favorite.
        /// </summary>
        private void SaveConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();

            if (this.FillFavorite(false))
                this.DialogResult = TerminalFormDialogResult.SaveAndConnect;

            this.Close();
        }

        /// <summary>
        /// Save favorite and copy the current favorite settings, except favorite and connection name.
        /// </summary>
        private void SaveCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this.txtName.Text = this.Favorite.Name + "_(copy)";
                this.EditedId = Guid.Empty;
                this.cmbServers.Text = String.Empty;
                this.cmbServers.Focus();
            }
        }

        private void SaveCurrentSettingsAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FillFavorite(true);
        }

        /// <summary>
        /// Save favorite and clear form for a new favorite.
        /// </summary>
        private void SaveNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this.EditedId = Guid.Empty;
                this.Init(null, String.Empty);
                this.cmbServers.Focus();
            }
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.RDPTabPage)
            {
                this.RDPSubTabPage_SelectedIndexChanged(null, null);
            }
        }

        #endregion

        #region Private form methods

        private void Init(IFavorite favorite, String serverName)
        {
            this.LoadMRUs();
            this.SetOkButtonState();

            try { this.SSHPreferences.Keys = Settings.SSHKeys; }
            catch (System.Security.Cryptography.CryptographicException)
            {
                Logging.Error("A CryptographicException occured on decrypting SSH keys. Favorite credentials possibly encrypted by another user. Ignore and continue.");
            }

            // move following line down to default value only once smart card access worked out.
            this.cmbTSGWLogonMethod.SelectedIndex = 0;

            if (favorite == null)
            {
                this.FillCredentialsCombobox(Guid.Empty);

                var defaultSavedFavorite = Settings.GetDefaultFavorite();
                if (defaultSavedFavorite != null)
                {
                    var defaultFavorite = ModelConverterV1ToV2.ConvertToFavorite(defaultSavedFavorite, this.persistence);
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
                this.txtPort.Text = port.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                this.EditedId = favorite.Id;
                this.Text = "Edit Connection";
                this.FillControls(favorite);
            }
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

        private void LoadCustomControlsState()
        {
            this.terminalServerManager.Dock = DockStyle.Fill;
            this.terminalServerManager.Location = new Point(0, 0);
            this.terminalServerManager.Name = "terminalServerManager1";
            this.terminalServerManager.Size = new Size(748, 309);
            this.terminalServerManager.TabIndex = 0;
            this.terminalServerManager.HostName = !this.validator.IsServerNameEmpty() ? this.cmbServers.Text : "localhost";
            this.terminalServerManager.AssignPersistence(this.persistence);
            this.RdpSessionTabPage.Controls.Add(this.terminalServerManager);
        }

        private void LoadMRUs()
        {
            this.cmbServers.Items.AddRange(Settings.MRUServerNames);
            this.cmbDomains.Items.AddRange(Settings.MRUDomainNames);
            this.cmbUsers.Items.AddRange(Settings.MRUUserNames);
            string[] groupNames = PersistedGroups.Select(group => group.Name).ToArray();
            this.txtGroupName.AutoCompleteCustomSource.AddRange(groupNames);
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
            this.currentToolBarFileName = favorite.ToolBarIconFile;
            this.NotesTextbox.Text = favorite.Notes;
        }

        private void FillGeneralPropertiesControls(IFavorite favorite)
        {
            this.txtName.Text = favorite.Name;
            this.cmbServers.Text = favorite.ServerName;
            this.ProtocolComboBox.SelectedItem = favorite.Protocol;
            this.txtPort.Text = favorite.Port.ToString(CultureInfo.InvariantCulture);
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

            this.SSHPreferences.SSHKeyFile = sshOptions.SSHKeyFile;
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
            this.txtLoadBalanceInfo.Text = userInterface.LoadBalanceInfo;
        }

        private void FillRdpTimeOutControls(RdpOptions rdpOptions)
        {
            this.ShutdownTimeoutTextBox.Text = rdpOptions.TimeOuts.ShutdownTimeout.ToString(CultureInfo.InvariantCulture);
            this.OverallTimeoutTextbox.Text = rdpOptions.TimeOuts.OverallTimeout.ToString(CultureInfo.InvariantCulture);
            this.SingleTimeOutTextbox.Text = rdpOptions.TimeOuts.ConnectionTimeout.ToString(CultureInfo.InvariantCulture);
            this.IdleTimeoutMinutesTextBox.Text = rdpOptions.TimeOuts.IdleTimeout.ToString(CultureInfo.InvariantCulture);
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
                var groupItem = new GroupListViewItem(group);
                listViewToFill.Items.Add(groupItem);
            }
        }

        private Boolean FillFavorite(Boolean defaultFav)
        {
            try
            {
                var isValid = this.validator.Validate();
                if (!isValid)
                    return false;

                IFavorite favorite = ResolveFavortie();
                this.FillFavoriteFromControls(favorite);

                if (defaultFav)
                    SaveDefaultFavorite(favorite);
                else
                    this.CommitFavoriteChanges(favorite);

                return true;
            }
            catch (DbEntityValidationException entityValidation)
            {
                EntityLogValidationErrors(entityValidation);
                this.ShowErrorMessageBox("Unable to save favorite, because database constrains are not satisfied");
                return false;
            }
            catch (Exception e)
            {
                Logging.Info("Fill Favorite Failed", e);
                this.ShowErrorMessageBox(e.Message);
                return false;
            }
        }

        internal void FillFavoriteFromControls(IFavorite favorite)
        {
            this.FillGeneralProrperties(favorite);
            this.FillDescriptionProperties(favorite);
            this.FillFavoriteSecurity(favorite);
            this.FillFavoriteDisplayOptions(favorite);
            this.FillFavoriteExecuteBeforeOptions(favorite);
            this.consolePreferences.FillFavorite(favorite);
            this.FillFavoriteVmrcOptions(favorite);
            this.FillFavoriteVncOptions(favorite);
            this.FillFavoriteICAOPtions(favorite);
            this.FillFavoriteSSHOptions(favorite);
            this.FillFavoriteRdpOptions(favorite);
        }
        
        private static void EntityLogValidationErrors(DbEntityValidationException entityValidation)
        {
            Logging.Error("Entity exception", entityValidation);
            foreach (DbEntityValidationResult validationResult in entityValidation.EntityValidationErrors)
            {
                foreach (DbValidationError propertyError in validationResult.ValidationErrors)
                {
                    Logging.Error(string.Format("Validation error '{0}': {1}", propertyError.PropertyName, propertyError.ErrorMessage));
                }
            }
        }

        /// <summary>
        /// Overwrites favortie property by favorite stored in persistence
        /// or newly created one
        /// </summary>
        private IFavorite ResolveFavortie()
        {
            IFavorite favorite = null; // force favorite property reset
            if (!this.EditedId.Equals(Guid.Empty))
                favorite = PersistedFavorites[this.EditedId];
            if (favorite == null)
                favorite = this.persistence.Factory.CreateFavorite();
            this.Favorite = favorite;
            return favorite;
        }

        private void FillDescriptionProperties(IFavorite favorite)
        {
            favorite.NewWindow = this.NewWindowCheckbox.Checked;
            favorite.DesktopShare = this.txtDesktopShare.Text;
            favorite.ToolBarIconFile = this.currentToolBarFileName;
            favorite.Notes = this.NotesTextbox.Text;
            this.ShowOnToolbar = this.chkAddtoToolbar.Checked;
        }

        private void FillGeneralProrperties(IFavorite favorite)
        {
            favorite.Name = (String.IsNullOrEmpty(this.txtName.Text) ? this.cmbServers.Text : this.txtName.Text);
            favorite.ServerName = this.cmbServers.Text;
            favorite.Protocol = this.ProtocolComboBox.SelectedItem.ToString();
            Int32 port;
            Int32.TryParse(this.PortText, out port);
            favorite.Port = port;
            this.FillWebProperties(favorite);
        }

        private void FillWebProperties(IFavorite favorite)
        {
            WebOptions.UpdateFavoriteUrl(favorite, this.httpUrlTextBox.Text);
        }

        private void FillFavoriteDisplayOptions(IFavorite favorite)
        {
            IDisplayOptions display = favorite.Display;
            if (this.cmbResolution.SelectedIndex >= 0)
                display.DesktopSize = (DesktopSize)this.cmbResolution.SelectedIndex;

            if (this.cmbColors.SelectedIndex >= 0)
                display.Colors = (Colors)this.cmbColors.SelectedIndex;

            display.Width = (Int32)this.widthUpDown.Value;
            display.Height = (Int32)this.heightUpDown.Value;
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
            rdpOptions.TimeOuts.ShutdownTimeout = ParseInteger(this.ShutdownTimeoutTextBox);
            rdpOptions.TimeOuts.OverallTimeout = ParseInteger(this.OverallTimeoutTextbox);
            rdpOptions.TimeOuts.ConnectionTimeout = ParseInteger(this.SingleTimeOutTextbox);
            rdpOptions.TimeOuts.IdleTimeout = ParseInteger(this.IdleTimeoutMinutesTextBox);
        }

        private static int ParseInteger(TextBox textBox)
        {
            int parsed;
            int.TryParse(textBox.Text, out parsed);
            return parsed;
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

        private void FillFavoriteExecuteBeforeOptions(IFavorite favorite)
        {
            IBeforeConnectExecuteOptions exucutionOptions = favorite.ExecuteBeforeConnect;
            exucutionOptions.Execute = this.chkExecuteBeforeConnect.Checked;
            exucutionOptions.Command = this.txtCommand.Text;
            exucutionOptions.CommandArguments = this.txtArguments.Text;
            exucutionOptions.InitialDirectory = this.txtInitialDirectory.Text;
            exucutionOptions.WaitForExit = this.chkWaitForExit.Checked;
        }

        private void FillFavoriteSSHOptions(IFavorite favorite)
        {
            var sshOptions = favorite.ProtocolProperties as SshOptions;
            if (sshOptions == null)
                return;

            sshOptions.AuthMethod = this.SSHPreferences.AuthMethod;
            sshOptions.CertificateKey = this.SSHPreferences.KeyTag;
            sshOptions.SSH1 = this.SSHPreferences.SSH1;

            sshOptions.SSHKeyFile = this.SSHPreferences.SSHKeyFile;
        }

        private void FillFavoriteICAOPtions(IFavorite favorite)
        {
            var icaOptions = favorite.ProtocolProperties as ICAOptions;
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

        private void FillFavoriteRdpOptions(IFavorite favorite)
        {
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
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
            rdpOptions.UserInterface.LoadBalanceInfo = this.txtLoadBalanceInfo.Text;
        }

        private void FillFavoriteRdpRedirectOptions(RdpOptions rdpOptions)
        {
            rdpOptions.Redirect.Drives = this.RedirectedDrives;
            rdpOptions.Redirect.Ports = this.chkSerialPorts.Checked;
            rdpOptions.Redirect.Printers = this.chkPrinters.Checked;
            rdpOptions.Redirect.Clipboard = this.chkRedirectClipboard.Checked;
            rdpOptions.Redirect.Devices = this.RedirectDevices;
            rdpOptions.Redirect.SmartCards = this.chkRedirectSmartcards.Checked;

            // because of changing protocol the value of the combox doesnt have to be selected
            if(this.cmbSounds.SelectedIndex >= 0)
                rdpOptions.Redirect.Sounds = (RemoteSounds)this.cmbSounds.SelectedIndex;
        }

        private void FillFavoriteSecurity(IFavorite favorite)
        {
            ICredentialSet selectedCredential = this.CredentialDropdown.SelectedItem as ICredentialSet;
            ISecurityOptions security = favorite.Security;
            security.Credential = selectedCredential == null ? Guid.Empty : selectedCredential.Id;

            security.Domain = this.cmbDomains.Text;
            security.UserName = this.cmbUsers.Text;
            if (this.chkSavePassword.Checked)
            {
                if (this.txtPassword.Text != HIDDEN_PASSWORD)
                    security.Password = this.txtPassword.Text;
                else
                    security.Password = this.favoritePassword;
            }
            else
            {
                security.Password = String.Empty;
            }
        }

        private void FillFavoriteVncOptions(IFavorite favorite)
        {
            var vncOptions = favorite.ProtocolProperties as VncOptions;
            if (vncOptions == null)
                return;

            vncOptions.AutoScale = this.vncAutoScaleCheckbox.Checked;
            vncOptions.DisplayNumber = (Int32)this.vncDisplayNumberInput.Value;
            vncOptions.ViewOnly = this.VncViewOnlyCheckbox.Checked;
        }

        private void FillFavoriteVmrcOptions(IFavorite favorite)
        {
            var vmrcOptions = favorite.ProtocolProperties as VMRCOptions;
            if (vmrcOptions == null)
                return;

            vmrcOptions.AdministratorMode = this.VMRCAdminModeCheckbox.Checked;
            vmrcOptions.ReducedColorsMode = this.VMRCReducedColorsCheckbox.Checked;
        }

        private void SaveDefaultFavorite(IFavorite favorite)
        {
            favorite.Name = String.Empty;
            favorite.ServerName = String.Empty;
            favorite.Notes = String.Empty;
            favorite.Security.Domain = String.Empty;
            favorite.Security.UserName = String.Empty;
            favorite.Security.Password = String.Empty;

            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions != null)
            {
                rdpOptions.Security.Enabled = false;
                rdpOptions.Security.WorkingFolder = String.Empty;
                rdpOptions.Security.StartProgram = String.Empty;
                rdpOptions.FullScreen = false;
            }

            var defaultFavorite = ModelConverterV2ToV1.ConvertToFavorite(favorite, this.persistence);
            Settings.SaveDefaultFavorite(defaultFavorite);
        }

        private void CommitFavoriteChanges(IFavorite favorite)
        {
            Settings.StartDelayedUpdate();
            this.persistence.StartDelayedUpdate();

            if (this.EditingNew)
                this.AddToPersistence(favorite);
            else
                Settings.EditFavoriteButton(this.EditedId, favorite.Id, this.ShowOnToolbar);

            List<IGroup> updatedGroups = this.GetNewlySelectedGroups();
            PersistedFavorites.UpdateFavorite(favorite, updatedGroups);
            this.persistence.SaveAndFinishDelayedUpdate();
            Settings.SaveAndFinishDelayedUpdate();
        }

        private void AddToPersistence(IFavorite favorite)
        {
            this.PersistedFavorites.Add(favorite);
            if (this.ShowOnToolbar)
                Settings.AddFavoriteButton(favorite.Id);
        }

        private void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(this, message, Program.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        internal void SetErrorInfo(Control target, string message)
        {
            if (target == null)
                return;

            this.errorProvider.SetError(target, message);
        }

        private void SetOkButtonState()
        {
            if (this.httpUrlTextBox.Visible)
            {
                this.btnSave.Enabled = this.validator.IsUrlValid();
            }
            else
            {
                this.btnSave.Enabled = !this.validator.IsServerNameEmpty();
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
            // this.LoadDialupConnections();
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

        internal Uri GetFullUrlFromHttpTextBox()
        {
            string newUrlText = this.httpUrlTextBox.Text.ToLower();
            string protocolPrefix = this.ProtocolComboBox.Text.ToLower();

            if (!newUrlText.StartsWith(ConnectionManager.HTTP.ToLower()) &&
                !newUrlText.StartsWith(ConnectionManager.HTTPS.ToLower()))
                newUrlText = String.Format("{0}://{1}", protocolPrefix, newUrlText);
            return WebOptions.TryParseUrl(newUrlText);
        }

        #endregion

        internal void AssingSelectedGroup(IGroup group)
        {
            if (group != null)
                BindGroupsToListView(this.lvConnectionTags, new[] { group });
        }
    }
}
