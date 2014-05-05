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
using Terminals.Forms.EditFavorite;
using Terminals.Network.Servers;

namespace Terminals
{
    /// <summary>
    /// Copy of related dialog, from which user controls will be extracted
    /// </summary>
    internal partial class NewTerminalFormCopy : Form
    {
        private NewTerminalFormValidator validator;
        internal Guid EditedId { get; set; }

        internal bool EditingNew { get { return this.EditedId == Guid.Empty; } }

        private readonly IPersistence persistence;

        private IGroups PersistedGroups
        {
            get { return this.persistence.Groups; }
        }

        private IFavorites PersistedFavorites
        {
            get { return this.persistence.Favorites; }
        }
 
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
            this.SetErrorProviderIconsAlignment();
        }

        private void InitializeValidations()
        {
            // todo move the validator and error provider initializations
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

        #endregion

        #region Private form event handlers

        private void NewTerminalForm_Load(Object sender, EventArgs e)
        {
            this.SuspendLayout();
            IOrderedEnumerable<IGroup> sortedGroups = PersistedGroups.OrderBy(group => group.Name);
            // BindGroupsToListView(this.AllTagsListView, sortedGroups);
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
            // this.SaveMRUs();
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

        private void RemoveSavedDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.RemoveDefaultFavorite();
        }

        /// <summary>
        /// Save favorite, close form and immediatly connect to the favorite.
        /// </summary>
        private void SaveConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // this.SaveMRUs();

            if (this.FillFavorite(false))
                this.DialogResult = TerminalFormDialogResult.SaveAndConnect;

            this.Close();
        }

        /// <summary>
        /// Save favorite and copy the current favorite settings, except favorite and connection name.
        /// </summary>
        private void SaveCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // this.SaveMRUs();
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
            // this.SaveMRUs();
            if (this.FillFavorite(false))
            {
                this.EditedId = Guid.Empty;
                this.Init(null, String.Empty);
                this.cmbServers.Focus();
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
                // todo generalcontrol this.FillCredentialsCombobox(Guid.Empty);

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
                // todo move to the general properties control
                GeneralPropertiesUserControl.GetServerAndPort(serverName, out server, out port);
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

        #endregion

        #region Refactoring finished

        internal void AssingSelectedGroup(IGroup group)
        {
            // todo this.groups.AssingSelectedGroup(group);
        }

        internal void LoadMRUs()
        {
            // todo this.generalProperties.LoadMRUs();
            // this.groups.LoadMRUs();
        }

        private void FillControls(IFavorite favorite)
        {
            // FillGeneralPropertiesControls(favorite);
            // FillDescriptionPropertiesControls(favorite);
            // FillSecurityControls(favorite);
            // todo generals control this.FillCredentialsCombobox(favorite.Security.Credential);
            // FillDisplayControls(favorite);
            // FillExecuteBeforeControls(favorite);
            // this.consolePreferences.FillControls(favorite);
            //FillVmrcControls(favorite);
            //FillVNCControls(favorite);
            //FillIcaControls(favorite);
            //FillSshControls(favorite);
            FillRdpOptions(favorite);

            // ReloadTagsListViewItems(favorite);
        }

        private void FillRdpOptions(IFavorite favorite)
        {
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions == null)
                return;

            //FillRdpSecurityControls(rdpOptions);
            //FillRdpUserInterfaceControls(rdpOptions);
            //FillRdpDisplayControls(rdpOptions);
            //FillRdpRedirectControls(rdpOptions);
            //FillTsGatewayControls(rdpOptions);
            //FillRdpTimeOutControls(rdpOptions);
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

        internal void FillFavoriteFromControls(IFavorite favorite)
        {
            // this.FillGeneralProrperties(favorite);
            // this.FillDescriptionProperties(favorite);
            // this.FillFavoriteSecurity(favorite);
            // this.FillFavoriteDisplayOptions(favorite);
            // this.FillFavoriteExecuteBeforeOptions(favorite);
            // this.consolePreferences.FillFavorite(favorite);
            // this.FillFavoriteVmrcOptions(favorite);
            // this.FillFavoriteVncOptions(favorite);
            // this.FillFavoriteICAOPtions(favorite);
            // this.FillFavoriteSSHOptions(favorite);
            this.FillFavoriteRdpOptions(favorite);
        }

        private void FillFavoriteRdpOptions(IFavorite favorite)
        {  
            // todo move to overall RdpControl
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions == null)
                return;

            // FillFavoriteRdpSecurity(rdpOptions);
            // FillFavoriteRdpInterfaceOptions(rdpOptions);
            // FillFavoriteRdpDisplayOptions(rdpOptions);
            // FillFavoriteRdpRedirectOptions(rdpOptions);
            // FillFavoriteTSgatewayOptions(rdpOptions);
            // FillFavoriteRdpTimeOutOptions(rdpOptions);
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

            // todo moved to Groups control List<IGroup> updatedGroups = this.GetNewlySelectedGroups();
            // PersistedFavorites.UpdateFavorite(favorite, updatedGroups);
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

        internal void SetErrorInfo(Control target, string message)
        {
            if (target == null)
                return;

            this.errorProvider.SetError(target, message);
        }

        private void SetOkButtonState()
        {
            if (this.httpUrlTextBox.Visible) // todo replace with general properties control property
            {
                this.btnSave.Enabled = this.validator.IsUrlValid();
            }
            else
            {
                this.btnSave.Enabled = !this.validator.IsServerNameEmpty();
            }
        }

        internal Uri GetFullUrlFromHttpTextBox()
        {
            // todo call general properties control
            return new Uri("nothing");
        }

        #endregion
    }
}
