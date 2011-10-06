using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AxMSTSCLib;
using FlickrNet;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class OptionDialog : Form
    {
        private AxMsRdpClient6 currentTerminal;
        private String tempFrob;
        private Panel currentPanel = null;

        #region Constructors
        
        public OptionDialog(AxMsRdpClient6 terminal) 
        {
            // Set default font type by Windows theme to use for all controls on form
            this.Font = SystemFonts.IconTitleFont;
            this.AutoScaleMode = AutoScaleMode.Dpi;

            InitializeComponent();

            // Set the size of the options form
            this.SetFormSize();

            // Update the old treeview theme to the new theme
            Native.Methods.SetWindowTheme(this.OptionsTreeView.Handle, "Explorer", null);

            // General tab
            this.NeverShowTerminalsCheckbox.Checked = Settings.NeverShowTerminalsWindow;
            this.chkShowUserNameInTitle.Checked = Settings.ShowUserNameInTitle;
            this.chkShowInformationToolTips.Checked = Settings.ShowInformationToolTips;
            this.chkShowFullInfo.Checked = Settings.ShowFullInformationToolTips;
            this.chkSingleInstance.Checked = Settings.SingleInstance;
            this.chkShowConfirmDialog.Checked = Settings.ShowConfirmDialog;
            this.chkSaveConnections.Checked = Settings.SaveConnectionsOnClose;
            this.MinimizeToTrayCheckbox.Checked = Settings.MinimizeToTray;
            this.validateServerNamesCheckbox.Checked = Settings.ForceComputerNamesAsURI;
            this.warnDisconnectCheckBox.Checked = Settings.WarnOnConnectionClose;
            this.chkAutoSwitchToCaptureCheckbox.Checked = Settings.AutoSwitchOnCapture;
            this.autoCaseTagsCheckbox.Checked = Settings.AutoCaseTags;

            this.txtDefaultDesktopShare.Text = Settings.DefaultDesktopShare;
            this.PortscanTimeoutTextBox.Text = Settings.PortScanTimeoutSeconds.ToString();

            // Execute Before Connect tab
            this.chkExecuteBeforeConnect.Checked = Settings.ExecuteBeforeConnect;
            this.txtCommand.Text = Settings.ExecuteBeforeConnectCommand;
            this.txtArguments.Text = Settings.ExecuteBeforeConnectArgs;
            this.txtInitialDirectory.Text = Settings.ExecuteBeforeConnectInitialDirectory;
            this.chkWaitForExit.Checked = Settings.ExecuteBeforeConnectWaitForExit;
            this.txtCommand.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtArguments.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtInitialDirectory.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.chkWaitForExit.Enabled = this.chkExecuteBeforeConnect.Checked;
            
            // Security - Master Password tab
            ClearMasterButton.Enabled = false;
            if (Settings.IsMasterPasswordDefined)
            {
                this.PasswordProtectTerminalsCheckbox.Checked = true;
                this.PasswordProtectTerminalsCheckbox.Enabled = false;
                this.PasswordTextbox.Enabled = false;
                this.ConfirmPasswordTextBox.Enabled = false;
                this.ClearMasterButton.Enabled = true;
            }

            // Security - Default password tab
            this.domainTextbox.Text = Settings.DefaultDomain;
            this.usernameTextbox.Text = Settings.DefaultUsername;
            this.passwordTxtBox.Text = Settings.DefaultPassword;

            // Security - Amazon tab
            this.AmazonBackupCheckbox.Checked = Settings.UseAmazon;
            this.AccessKeyTextbox.Text = Settings.AmazonAccessKey;
            this.SecretKeyTextbox.Text = Settings.AmazonSecretKey;
            this.BucketName = Settings.AmazonBucketName;
            UpdateAmazonControlsEnabledState();

            // Proxy tab
            this.AutoProxyRadioButton.Checked = !Settings.UseProxy;
            this.ProxyRadionButton.Checked = Settings.UseProxy;
            this.ProxyAddressTextbox.Text = Settings.ProxyAddress;
            this.ProxyPortTextbox.Text = (Settings.ProxyPort.ToString().Equals("0")) ? "80" : Settings.ProxyPort.ToString();
            this.ProxyAddressTextbox.Enabled = Settings.UseProxy;
            this.ProxyPortTextbox.Enabled = Settings.UseProxy;

            // Screen capture tab
            this.chkEnableCaptureToClipboard.Checked = Settings.EnableCaptureToClipboard;
            this.chkEnableCaptureToFolder.Checked = Settings.EnableCaptureToFolder;

            this.txtScreenCaptureFolder.Text = Settings.CaptureRoot;
            this.txtScreenCaptureFolder.SelectionStart = this.txtScreenCaptureFolder.Text.Length;
            this.txtScreenCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.chkAutoSwitchToCaptureCheckbox.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.ButtonBrowseCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;

            // More tab
            this.EnableGroupsMenu.Checked = Settings.EnableGroupsMenu;
            this.EnableFavoritesPanel.Checked = Settings.EnableFavoritesPanel;
            this.AutoExapandTagsPanelCheckBox.Checked = Settings.AutoExapandTagsPanel;

            switch (Settings.DefaultSortProperty)
            {
                case SortProperties.ServerName:
                    this.ServerNameRadio.Checked = true;
                    break;
                case SortProperties.ConnectionName:
                    this.ConnectionNameRadioButton.Checked = true;
                    break;
                case SortProperties.Protocol:
                    this.ProtocolRadionButton.Checked = true;
                    break;
                case SortProperties.None:
                    this.NoneRadioButton.Checked = true;
                    break;
            }

            if (Settings.Office2007BlueFeel)
            {
                this.RenderBlueRadio.Checked = true;
            }
            else if (Settings.Office2007BlackFeel)
            {
                this.RenderBlackRadio.Checked = true;
            }
            else
                this.RenderNormalRadio.Checked = true;

            this.currentTerminal = terminal;

            // Edit appearance
            this.EvaluatedDesktopShareLabel.Visible = false;
            this.PasswordsMatchLabel.Text = String.Empty;
            this.currentPanel = this.panelStartupShutdown;
            this.OptionsTreeView.SelectedNode = this.OptionsTreeView.Nodes[0];
            this.OptionsTreeView.Select();
            this.OptionTitelLabel.BackColor = Color.FromArgb(17, 0, 252);

            this.DrawBottomLine();
        }

        #endregion

        #region Private Methods by developer

        private void SetFormSize()
        {
            // Hide tabpage control, only used in design time
            this.tabControl3.Hide();

            // Get all the panel control from the tabpages 
            // and add them to the form controls collection
            // and hide the controls
            foreach (TabPage tp in this.tabControl3.TabPages)
            {
                foreach (Control ctrl in tp.Controls)
                {
                    if (ctrl.GetType() == typeof(Panel))
                    {
                        ctrl.Hide();
                        this.Controls.Add(ctrl);
                    }
                }
            }

            

            // The option title label is the anchor for the form's width
            Int32 formWidth = this.OptionTitelLabel.Location.X + this.OptionTitelLabel.Width + 15;
            this.Width = formWidth;
        }

        private void DrawBottomLine()
        {
            Label lbl = new Label();
            lbl.AutoSize = false;
            lbl.BorderStyle = BorderStyle.Fixed3D;
            lbl.SetBounds(
                this.OptionTitelLabel.Left, 
                this.OptionsTreeView.Top + this.OptionsTreeView.Height - 1,
                this.OptionTitelLabel.Width,
                2);
            this.Controls.Add(lbl);
            lbl.Show();
        }

        private void CheckPasswords()
        {
            if (!this.PasswordTextbox.Text.Equals(String.Empty) && !this.ConfirmPasswordTextBox.Text.Equals(String.Empty))
            {
                if (this.PasswordTextbox.Text.Equals(this.ConfirmPasswordTextBox.Text))
                {
                    this.PasswordsMatchLabel.Text = "Passwords match";
                    this.PasswordsMatchLabel.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    this.PasswordsMatchLabel.Text = "Passwords do not match";
                    this.PasswordsMatchLabel.ForeColor = Color.Red;                    
                }
            }
        }

        private void EvaluateDesktopShare()
        {
            if (this.currentTerminal != null)
            {
                this.EvaluatedDesktopShareLabel.Text = 
                    this.txtDefaultDesktopShare.Text.Replace("%SERVER%", this.currentTerminal.Server).Replace(
                    "%USER%", this.currentTerminal.UserName);
            }
            else
            {
                this.EvaluatedDesktopShareLabel.Text = String.Empty;
            }
        }

        #endregion

        #region Private Event Methods

        private void OptionsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                // Show correct options panel on node selection
                this.currentPanel.Hide();
                this.currentPanel = (Panel)this.Controls["panel" + this.OptionsTreeView.SelectedNode.Tag.ToString()];

                Int32 x = this.OptionTitelLabel.Left;
                Int32 y = this.OptionTitelLabel.Top + this.OptionTitelLabel.Height + 3;
                this.currentPanel.Location = new Point(x, y);
                this.currentPanel.Show();

                this.OptionTitelLabel.Text = this.OptionsTreeView.SelectedNode.Name.Replace("&", "&&");

                if (e.Node.GetNodeCount(true) > 0)
                {
                    switch (e.Action)
                    {
                        case TreeViewAction.ByKeyboard:
                        case TreeViewAction.ByMouse:
                            if (e.Node.IsExpanded)
                                e.Node.Collapse();
                            else
                                e.Node.Expand();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Info(ex);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Int32 timeout = 5;
            Settings.DelayConfigurationSave = true;

            // General tab
            Settings.NeverShowTerminalsWindow = this.NeverShowTerminalsCheckbox.Checked;
            Settings.ShowUserNameInTitle = this.chkShowUserNameInTitle.Checked;
            Settings.ShowInformationToolTips = this.chkShowInformationToolTips.Checked;
            Settings.ShowFullInformationToolTips = this.chkShowFullInfo.Checked;
            Settings.SingleInstance = this.chkSingleInstance.Checked;
            Settings.ShowConfirmDialog = this.chkShowConfirmDialog.Checked;
            Settings.SaveConnectionsOnClose = this.chkSaveConnections.Checked;
            Settings.MinimizeToTray = this.MinimizeToTrayCheckbox.Checked;
            Settings.ForceComputerNamesAsURI = this.validateServerNamesCheckbox.Checked;
            Settings.WarnOnConnectionClose = this.warnDisconnectCheckBox.Checked;
            Settings.AutoSwitchOnCapture = this.chkAutoSwitchToCaptureCheckbox.Checked;
            Settings.AutoCaseTags = this.autoCaseTagsCheckbox.Checked;

            Settings.DefaultDesktopShare = this.txtDefaultDesktopShare.Text;
            
            Int32.TryParse(this.PortscanTimeoutTextBox.Text, out timeout);
            if (Settings.PortScanTimeoutSeconds <= 0 || Settings.PortScanTimeoutSeconds >= 60)
                timeout = 5;
            Settings.PortScanTimeoutSeconds = timeout;

            // Execute Before Connect tab
            Settings.ExecuteBeforeConnect = this.chkExecuteBeforeConnect.Checked;
            Settings.ExecuteBeforeConnectCommand = this.txtCommand.Text;
            Settings.ExecuteBeforeConnectArgs = this.txtArguments.Text;
            Settings.ExecuteBeforeConnectInitialDirectory = this.txtInitialDirectory.Text;
            Settings.ExecuteBeforeConnectWaitForExit = this.chkWaitForExit.Checked;

            // Security - Master Password tab
            if (this.PasswordProtectTerminalsCheckbox.Checked
                && !String.IsNullOrEmpty(this.PasswordTextbox.Text)
                && !String.IsNullOrEmpty(this.ConfirmPasswordTextBox.Text)
                && this.PasswordTextbox.Text.Equals(this.ConfirmPasswordTextBox.Text))
            {
                Settings.UpdateMasterPassword(this.PasswordTextbox.Text);
            }

            // Security - Default Password tab
            Settings.DefaultDomain = this.domainTextbox.Text;
            Settings.DefaultUsername = this.usernameTextbox.Text;
            if (!String.IsNullOrEmpty(this.passwordTxtBox.Text))
                Settings.DefaultPassword = this.passwordTxtBox.Text;   

            // Security - Amazon tab
            Settings.UseAmazon = this.AmazonBackupCheckbox.Checked;
            Settings.AmazonAccessKey = this.AccessKeyTextbox.Text;
            Settings.AmazonSecretKey = this.SecretKeyTextbox.Text;
            Settings.AmazonBucketName = this.BucketName;

            // Proxy tab
            Settings.UseProxy = this.ProxyRadionButton.Checked;
            Settings.ProxyAddress = this.ProxyAddressTextbox.Text;
            Settings.ProxyPort = Convert.ToInt32(this.ProxyPortTextbox.Text);

            // Screen capture tab
            Settings.EnableCaptureToClipboard = this.chkEnableCaptureToClipboard.Checked;
            Settings.EnableCaptureToFolder = this.chkEnableCaptureToFolder.Checked;
            Settings.CaptureRoot = this.txtScreenCaptureFolder.Text;

            // More tab            
            Settings.EnableFavoritesPanel = this.EnableFavoritesPanel.Checked;
            Settings.EnableGroupsMenu = this.EnableGroupsMenu.Checked;
            Settings.AutoExapandTagsPanel = this.AutoExapandTagsPanelCheckBox.Checked;

            if (this.ServerNameRadio.Checked)
            {
                Settings.DefaultSortProperty = SortProperties.ServerName;
            }
            else if (this.NoneRadioButton.Checked)
            {
                Settings.DefaultSortProperty = SortProperties.None;
            }
            else if (this.ConnectionNameRadioButton.Checked)
            {
                Settings.DefaultSortProperty = SortProperties.ConnectionName;
            }
            else
            {
                Settings.DefaultSortProperty = SortProperties.Protocol;
            }

            Settings.Office2007BlackFeel = false;
            Settings.Office2007BlueFeel = false;
            if (this.RenderBlueRadio.Checked)
            {
                Settings.Office2007BlueFeel = true;
            }
            else if (this.RenderBlackRadio.Checked)
            {
                Settings.Office2007BlackFeel = true;
            }

            try
            {
                Settings.Save();
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Error(ex);
                MessageBox.Show(String.Format("Error saving settings.\r\n{0}", ex.Message));
            }
            finally
            {
                Settings.DelayConfigurationSave = false;
            }
        }

        private void chkShowInformationToolTips_CheckedChanged(object sender, EventArgs e)
        {
            this.chkShowFullInfo.Enabled = this.chkShowInformationToolTips.Checked;
            if (!this.chkShowInformationToolTips.Checked)
            {
                this.chkShowFullInfo.Checked = false;                
            }
        }

        private void txtDefaultDesktopShare_TextChanged(object sender, EventArgs e)
        {
            this.EvaluateDesktopShare();
        }

        private void PasswordProtectTerminalsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.PasswordTextbox.Enabled = this.PasswordProtectTerminalsCheckbox.Checked;
            this.ConfirmPasswordTextBox.Enabled = this.PasswordProtectTerminalsCheckbox.Checked;
            this.PasswordsMatchLabel.Visible = this.PasswordProtectTerminalsCheckbox.Checked;
            this.PasswordsMatchLabel.Text = String.Empty;
        }

        private void PasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            this.CheckPasswords();
        }

        private void ConfirmPasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            this.CheckPasswords();
        }

        private void AuthorizeFlickrButton_Click(object sender, EventArgs e)
        {
            // Create Flickr instance    
            Flickr flickr = new Flickr(Program.FlickrAPIKey, Program.FlickrSharedSecretKey);    
            // Get Frob        
            this.tempFrob = flickr.AuthGetFrob();
            // Calculate the URL at Flickr to redirect the user to    
            String flickrUrl = flickr.AuthCalcUrl(this.tempFrob, AuthLevel.Write);    
            // The following line will load the URL in the users default browser.    
            System.Diagnostics.Process.Start(flickrUrl);
            CompleteAuthButton.Enabled = true;
        }

        private void CompleteAuthButton_Click(object sender, EventArgs e)
        {
            // Create Flickr instance
            Flickr flickr = new Flickr(Program.FlickrAPIKey, Program.FlickrSharedSecretKey);    
            try
            {
                // use the temporary Frob to get the authentication
                Auth auth = flickr.AuthGetToken(this.tempFrob);
                // Store this Token for later usage,
                // or set your Flickr instance to use it.
                MessageBox.Show("User authenticated successfully");
                Logging.Log.Info("User authenticated successfully. Authentication token is " + auth.Token + ".User id is " + auth.User.UserId + ", username is" + auth.User.Username);
                flickr.AuthToken = auth.Token;
                Settings.FlickrToken = auth.Token;
            }
            catch(FlickrException ex)
            {
                // If user did not authenticat your application
                // then a FlickrException will be thrown.
                Logging.Log.Info("User not authenticated successfully", ex);
                MessageBox.Show("User did not authenticate you" +ex.Message);
            }
        }

        private void ClearMasterButton_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to remove the master password?\r\n\r\n**Please be advised that this will render ALL saved passwords inactive!**",
                               Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Settings.UpdateMasterPassword(String.Empty);
                this.ClearMasterButton.Enabled = false;

                this.PasswordProtectTerminalsCheckbox.Checked = false;
                this.PasswordProtectTerminalsCheckbox.Enabled = true;
                this.PasswordTextbox.Enabled = true;
                this.ConfirmPasswordTextBox.Enabled = true;
                this.PasswordTextbox.Text = String.Empty;
                this.ConfirmPasswordTextBox.Text = String.Empty;
            }
        }

        private void label10_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    String md5 = Updates.UpdateManager.GetMD5HashFromFile(ofd.FileName);
                    Clipboard.SetText(String.Format("{0}\r\n{1}", ofd.FileName, md5));
                    MessageBox.Show(String.Format("MD5 of {0}, value:{1}, was copied to the clipboard;", ofd.FileName, md5));
                }
            }
        }

        private void ProxyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.ProxyAddressTextbox.Enabled = this.ProxyRadionButton.Checked;
            this.ProxyPortTextbox.Enabled = this.ProxyRadionButton.Checked;
        }

        private void ButtonBrowseCaptureFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select the screen capture folder";
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                String currentFld = this.txtScreenCaptureFolder.Text;
                if (!currentFld.Equals(String.Empty))
                    currentFld = (currentFld.EndsWith("\\")) ? currentFld : currentFld + "\\";

                dlg.SelectedPath = (currentFld.Equals(String.Empty)) ? 
                    Environment.GetFolderPath(dlg.RootFolder) : System.IO.Path.GetDirectoryName(currentFld);
                
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    String selectedFld = dlg.SelectedPath;
                    if (!selectedFld.Equals(String.Empty))
                        selectedFld = (selectedFld.EndsWith("\\")) ? selectedFld : selectedFld + "\\";

                    this.txtScreenCaptureFolder.Text = selectedFld;
                    this.txtScreenCaptureFolder.SelectionStart = this.txtScreenCaptureFolder.Text.Length;
                }
            }
        }

        private void chkDisableCaptureToFolder_CheckedChanged(object sender, EventArgs e)
        {
            this.chkAutoSwitchToCaptureCheckbox.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.txtScreenCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.ButtonBrowseCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
        }

        private void chkExecuteBeforeConnect_CheckedChanged(object sender, EventArgs e)
        {
            Boolean enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtCommand.Enabled = enabled;
            this.txtArguments.Enabled = enabled;
            this.txtInitialDirectory.Enabled = enabled;
            this.chkWaitForExit.Enabled = enabled;
        }

        #endregion

        #region Amazon S3

        private const String AMAZON_BUCKET = "Terminals";
        private const String AMAZON_FILE = "Terminals.config";
        private const String AMAZON_MESSAGETITLE = "Amazon S3 Backup";

        /// <summary>
        /// Gets or sets the bucket name into/from the text box (it is a proxy).
        /// Returns default "Terminals" bucket name or text filled into associated text box.
        /// </summary>
        private String BucketName
        {
            get
            {
                if (String.IsNullOrEmpty(this.BucketNameTextBox.Text))
                    this.BucketNameTextBox.Text = AMAZON_BUCKET;
                return this.BucketNameTextBox.Text;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    this.BucketNameTextBox.Text = AMAZON_BUCKET;
                else
                    this.BucketNameTextBox.Text = value;
            }
        }

        private void AmazonBackupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAmazonControlsEnabledState();
        }

        private void UpdateAmazonControlsEnabledState()
        {
            this.AccessKeyTextbox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.SecretKeyTextbox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.BucketNameTextBox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.TestButton.Enabled = this.AmazonBackupCheckbox.Checked;
            this.BackupButton.Enabled = this.AmazonBackupCheckbox.Checked;
            this.RestoreButton.Enabled = this.AmazonBackupCheckbox.Checked;
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Exception testError = null;
            using (AmazonS3 client = CreateClient())
            {
                testError = EnsureBucketExists(client);
            }

            this.ShowActionResult(testError, "Test was successful!");
            this.Cursor = Cursors.Default;
        }

        private void ShowActionResult(Exception testError, string successMessage)
        {
            if (testError == null)
            {
                this.ErrorLabel.Text = successMessage;
                this.ErrorLabel.ForeColor = Color.Black;
            }
            else
            {
                this.ErrorLabel.ForeColor = Color.Red;
                this.ErrorLabel.Text = testError.Message;
            }
        }

        /// <summary>
        /// Ceateates new S3 webservice client. Note, that the client is Disposable
        /// </summary>
        private AmazonS3 CreateClient()
        {
            return AWSClientFactory.CreateAmazonS3Client(
                this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
        }

        private void BackupButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to upload your current configuration?",
                AMAZON_MESSAGETITLE, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (AmazonS3 client = CreateClient())
                {
                    EnsureBucketExists(client);
                    BackUpToAmazon(client);
                }
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore your current configuration?",
                AMAZON_MESSAGETITLE, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (AmazonS3 client = CreateClient())
                {
                    RestoreFromAmazon(client);
                }
            }
        }

        private Exception EnsureBucketExists(AmazonS3 client)
        {
            try
            {
                S3Bucket bucket = GetBucket(client);
                if (bucket == null)
                {
                    CreateBucket(client);
                }

                return null;
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Amazon S3 exception occured", exception);
                return exception;
            }
        }

        private S3Bucket GetBucket(AmazonS3 client)
        {
            ListBucketsRequest listRequest = new ListBucketsRequest();
            ListBucketsResponse response = client.ListBuckets(listRequest);
            return response.Buckets
                .Where(candidate => candidate.BucketName == this.BucketName)
                .FirstOrDefault();
        }

        private void CreateBucket(AmazonS3 client)
        {
            PutBucketRequest request = new PutBucketRequest();
            request.BucketName = this.BucketName;
            client.PutBucket(request);
        }

        private void BackUpToAmazon(AmazonS3 client)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest();
                request.WithBucketName(this.BucketName).WithKey(AMAZON_FILE)
                    .WithFilePath(Program.ConfigurationFileLocation);

                client.PutObject(request);

                this.ErrorLabel.ForeColor = Color.Black;
                this.ErrorLabel.Text = "The backup was a success!";
            }
            catch (Exception exception)
            {
                this.ErrorLabel.ForeColor = Color.Red;
                this.ErrorLabel.Text = exception.Message;
            }
        }

        private void RestoreFromAmazon(AmazonS3 client)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest()
                    .WithBucketName(this.BucketName)
                    .WithKey(AMAZON_FILE);

                using (GetObjectResponse response = client.GetObject(request))
                {
                    response.WriteResponseStreamToFile(Program.ConfigurationFileLocation);
                    Settings.ForceReload();
                }

                this.ErrorLabel.ForeColor = Color.Black;
                this.ErrorLabel.Text = "The restore was a success!";
            }
            catch (Exception exc)
            {
                this.ErrorLabel.ForeColor = Color.Red;
                this.ErrorLabel.Text = exc.Message;
            }
        }

        #endregion
    }
}