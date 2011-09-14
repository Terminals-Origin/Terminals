using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMSTSCLib;
using FlickrNet;
using Terminals.Configuration;

namespace Terminals
{
    public partial class OptionsForm : Form
    {
        private AxMsRdpClient6 _currentTerminal;
        private string _tempFrob;
        // private static string _amazonBucket = "Terminals";
        // private static string _amazonConfigKeyName = "Terminals.config";

        public OptionsForm(AxMsRdpClient6 terminal) 
        {
            InitializeComponent();

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
            
            // Security - Master Password tab
            ClearMasterButton.Enabled = false;
            if (Settings.TerminalsPassword != string.Empty)
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

            this.AccessKeyTextbox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.SecretKeyTextbox.Enabled = this.AmazonBackupCheckbox.Checked;
            this.TestButton.Enabled = this.AmazonBackupCheckbox.Checked;
            this.BackupButton.Enabled = this.AmazonBackupCheckbox.Checked;
            this.RestoreButton.Enabled = this.AmazonBackupCheckbox.Checked;

            // Proxy tab
            this.ProxyRadionButton.Checked = Settings.UseProxy;
            if (Settings.UseProxy)
            {
                this.ProxyAddressTextbox.Text = Settings.ProxyAddress;
                this.ProxyPortTextbox.Text = Settings.ProxyPort.ToString();
            }

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

            this._currentTerminal = terminal;
        }

        #region private

        private void CheckPasswords()
        {
            if (PasswordTextbox.Text != ConfirmPasswordTextBox.Text)
            {
                PasswordsMatchLabel.Text = "Passwords do not match";
            }
            else
            {
                PasswordsMatchLabel.Text = "Passwords match";
            }
        }

        private void EvaluateDesktopShare()
        {
            if (_currentTerminal != null)
            {
                lblEvaluatedDesktopShare.Text = txtDefaultDesktopShare.Text.Replace("%SERVER%", _currentTerminal.Server).Replace(
                    "%USER%", _currentTerminal.UserName);
            }
            else
            {
                lblEvaluatedDesktopShare.Text = String.Empty;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int timeout = 5;
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
            
            int.TryParse(this.PortscanTimeoutTextBox.Text, out timeout);
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
                && !string.IsNullOrEmpty(this.PasswordTextbox.Text)
                && !string.IsNullOrEmpty(this.ConfirmPasswordTextBox.Text)
                && this.PasswordTextbox.Text.Equals(this.ConfirmPasswordTextBox.Text))
            {
                Settings.TerminalsPassword = this.PasswordTextbox.Text;
            }

            // Security - Default Password tab
            Settings.DefaultDomain = this.domainTextbox.Text;
            Settings.DefaultUsername = this.usernameTextbox.Text;
            if (!string.IsNullOrEmpty(this.passwordTxtBox.Text))
                Settings.DefaultPassword = this.passwordTxtBox.Text;   

            // Security - Amazon tab
            Settings.UseAmazon = this.AmazonBackupCheckbox.Checked;
            Settings.AmazonAccessKey = this.AccessKeyTextbox.Text;
            Settings.AmazonSecretKey = this.SecretKeyTextbox.Text;

            // Proxy tab
            Settings.UseProxy = this.ProxyRadionButton.Checked;
            if (Settings.UseProxy)
            {
                Settings.ProxyAddress = this.ProxyAddressTextbox.Text;
                Settings.ProxyPort = Convert.ToInt32(this.ProxyPortTextbox.Text);
            }

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

            Settings.DelayConfigurationSave = false;
        }

        private void chkShowInformationToolTips_CheckedChanged(object sender, EventArgs e)
        {
            chkShowFullInfo.Enabled = chkShowInformationToolTips.Checked;
            if (!chkShowInformationToolTips.Checked)
            {
                chkShowFullInfo.Checked = false;                
            }
        }

        private void txtDefaultDesktopShare_TextChanged(object sender, EventArgs e)
        {
            EvaluateDesktopShare();
        }

        private void PasswordProtectTerminalsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTextbox.Enabled = PasswordProtectTerminalsCheckbox.Checked;
            ConfirmPasswordTextBox.Enabled = PasswordProtectTerminalsCheckbox.Checked;
            PasswordsMatchLabel.Visible = PasswordProtectTerminalsCheckbox.Checked;            
        }

        private void PasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            CheckPasswords();
        }

        private void ConfirmPasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckPasswords();
        }

        private void AuthorizeFlickrButton_Click(object sender, EventArgs e)
        {
            // Create Flickr instance    
            Flickr flickr = new Flickr(Program.FlickrAPIKey, Program.FlickrSharedSecretKey);    
            // Get Frob        
            _tempFrob = flickr.AuthGetFrob();
            // Calculate the URL at Flickr to redirect the user to    
            string flickrUrl = flickr.AuthCalcUrl(_tempFrob, AuthLevel.Write);    
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
                Auth auth = flickr.AuthGetToken(_tempFrob);
                // Store this Token for later usage,
                // or set your Flickr instance to use it.
                System.Windows.Forms.MessageBox.Show("User authenticated successfully");
                Terminals.Logging.Log.Info("User authenticated successfully. Authentication token is " + auth.Token + ".User id is " + auth.User.UserId + ", username is" + auth.User.Username);
                flickr.AuthToken = auth.Token;
                Settings.FlickrToken = auth.Token;
            }
            catch(FlickrException ex)
            {
                // If user did not authenticat your application
                // then a FlickrException will be thrown.
                Terminals.Logging.Log.Info("User not authenticated successfully", ex);
                System.Windows.Forms.MessageBox.Show("User did not authenticate you" +ex.Message);
            }
        }

        private void ClearMasterButton_Click(object sender, EventArgs e)
        {
            if(System.Windows.Forms.MessageBox.Show("Are you sure you want to remove the master password?\r\n\r\n**Please be advised that this will render ALL saved passwords inactive!**", Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Settings.TerminalsPassword = String.Empty;
                ClearMasterButton.Enabled = false;

                PasswordProtectTerminalsCheckbox.Checked = false;
                PasswordProtectTerminalsCheckbox.Enabled = true;
                PasswordTextbox.Enabled = true;
                ConfirmPasswordTextBox.Enabled = true;
                PasswordTextbox.Text = "";
                ConfirmPasswordTextBox.Text = "";
            }
        }

        private void label10_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string md5 = Terminals.Updates.UpdateManager.GetMD5HashFromFile(ofd.FileName);
                System.Windows.Forms.Clipboard.SetText(string.Format("{0}\r\n{1}", ofd.FileName, md5));
                System.Windows.Forms.MessageBox.Show(string.Format("MD5 of {0}, value:{1}, was copied to the clipboard;", ofd.FileName, md5));
            }
        }

        private void ProxyRadionButton_CheckedChanged(object sender, EventArgs e)
        {
            this.panel1.Enabled = ProxyRadionButton.Checked;
        }

        private void AutoProxyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.panel1.Enabled = ProxyRadionButton.Checked;
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
            //    string testBucket = Guid.NewGuid().ToString();
            //    wrapper.AddBucket(testBucket);
            //    string bucket = wrapper.ListBucket(testBucket);
            //    wrapper.DeleteBucket(testBucket);

            //    try
            //    {
            //        string terminals = wrapper.ListBucket(_amazonBucket);
            //    }
            //    catch (Exception exc)
            //    {
            //        if (exc.Message == "The specified bucket does not exist")
            //        {
            //            wrapper.AddBucket(_amazonBucket);
            //            string terminals = wrapper.ListBucket(_amazonBucket);
            //        }
            //    }

            //    this.ErrorLabel.Text = "Test was successful!";
            //    this.ErrorLabel.ForeColor = Color.Black;
            //}
            //catch (Exception exc)
            //{
            //    this.ErrorLabel.ForeColor = Color.Red;
            //    this.ErrorLabel.Text = exc.Message;
            //}
        }

        private void AmazonBackupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.AccessKeyTextbox.Enabled = AmazonBackupCheckbox.Checked;
            this.SecretKeyTextbox.Enabled = AmazonBackupCheckbox.Checked;
            this.TestButton.Enabled = AmazonBackupCheckbox.Checked;
            this.BackupButton.Enabled = AmazonBackupCheckbox.Checked;
            this.RestoreButton.Enabled = AmazonBackupCheckbox.Checked;
        }

        private void BackupButton_Click(object sender, EventArgs e)
        {
            //if (System.Windows.Forms.MessageBox.Show("Are you sure you want to upload your current configuration?", "Amazon S3 Backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
            //    string url = null;
            //    try
            //    {
            //        string terminals = wrapper.ListBucket(_amazonBucket);
            //    }
            //    catch (Exception)
            //    {
            //        wrapper.AddBucket(_amazonBucket);
            //    }

            //    try
            //    {
            //        wrapper.AddFileObject(_amazonBucket, _amazonConfigKeyName, Terminals.Program.ConfigurationFileLocation);
            //        url = wrapper.GetUrl(_amazonBucket, _amazonConfigKeyName);
            //    }
            //    catch (Exception exc)
            //    {
            //        this.ErrorLabel.ForeColor = Color.Red;
            //        this.ErrorLabel.Text = exc.Message;
            //        return;
            //    }

            //    this.ErrorLabel.ForeColor = Color.Black;
            //    this.ErrorLabel.Text = "The backup was a success!";
            //}
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            //if (System.Windows.Forms.MessageBox.Show("Are you sure you want to restore your current configuration?", "Amazon S3 Backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
            //    try
            //    {
            //        string terminals = wrapper.ListBucket(_amazonBucket);
            //    }
            //    catch (Exception exc)
            //    {
            //        this.ErrorLabel.ForeColor = Color.Red;
            //        this.ErrorLabel.Text = exc.Message;
            //        return;
            //    }

            //    try
            //    {
            //        wrapper.GetFileObject(_amazonBucket, _amazonConfigKeyName, Terminals.Program.ConfigurationFileLocation);
            //    }
            //    catch (Exception exc)
            //    {
            //        this.ErrorLabel.ForeColor = Color.Red;
            //        this.ErrorLabel.Text = exc.Message;
            //        return;
            //    }

            //    this.ErrorLabel.ForeColor = Color.Black;
            //    this.ErrorLabel.Text = "The restore was a success!";
            //}
        }

        private void ButtonBrowseCaptureFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select the screen capture folder";
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                string currentFld = this.txtScreenCaptureFolder.Text;
                if (!currentFld.Equals(string.Empty))
                    currentFld = (currentFld.EndsWith("\\")) ? currentFld : currentFld + "\\";

                dlg.SelectedPath = (currentFld.Equals(string.Empty)) ? 
                    Environment.GetFolderPath(dlg.RootFolder) : System.IO.Path.GetDirectoryName(currentFld);
                
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFld = dlg.SelectedPath;
                    if (!selectedFld.Equals(string.Empty))
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

        #endregion

    }
}