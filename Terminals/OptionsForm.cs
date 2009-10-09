using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMSTSCLib;
using FlickrNet;

namespace Terminals
{
    public partial class OptionsForm : Form
    {
        private AxMsRdpClient2 _currentTerminal;
        private string _tempFrob;
        private static string _amazonBucket = "Terminals";
        private static string _amazonConfigKeyName = "Terminals.config";

        public OptionsForm(AxMsRdpClient2 terminal) {
            InitializeComponent();
            chkShowInformationToolTips.Checked = Settings.ShowInformationToolTips;
            chkShowUserNameInTitle.Checked = Settings.ShowUserNameInTitle;
            chkShowFullInfo.Checked = Settings.ShowFullInformationToolTips;
            txtDefaultDesktopShare.Text = Settings.DefaultDesktopShare;
            chkExecuteBeforeConnect.Checked = Settings.ExecuteBeforeConnect;
            txtArguments.Text = Settings.ExecuteBeforeConnectArgs;
            txtCommand.Text = Settings.ExecuteBeforeConnectCommand;
            txtInitialDirectory.Text = Settings.ExecuteBeforeConnectInitialDirectory;
            chkWaitForExit.Checked = Settings.ExecuteBeforeConnectWaitForExit;
            autoSwitchToCaptureCheckbox.Checked=Settings.AutoSwitchOnCapture;
            chkSingleInstance.Checked = Settings.SingleInstance;
            chkShowConfirmDialog.Checked = Settings.ShowConfirmDialog;
            chkSaveConnections.Checked = Settings.SaveConnectionsOnClose;
            validateServerNamesCheckbox.Checked= Settings.ForceComputerNamesAsURI;
            warnDisconnectCheckBox.Checked = Settings.WarnOnConnectionClose;

            if(Settings.Office2007BlueFeel)
                RenderBlueRadio.Checked = true;
            else if(Settings.Office2007BlackFeel)
                RenderBlackRadio.Checked = true;
            else
                RenderNormalRadio.Checked = true;


            _currentTerminal = terminal;
            this.PortscanTimeoutTextBox.Text = Settings.PortScanTimeoutSeconds.ToString();

            ClearMasterButton.Enabled = false;
            if (Settings.TerminalsPassword != string.Empty) {
                PasswordProtectTerminalsCheckbox.Checked = true;
                PasswordProtectTerminalsCheckbox.Enabled = false;
                PasswordTextbox.Enabled = false;
                ConfirmPasswordTextBox.Enabled = false;
                ClearMasterButton.Enabled = true;
            }
            this.MinimizeToTrayCheckbox.Checked = Settings.MinimizeToTray;

            this.AutoExapandTagsPanelCheckBox.Checked = Settings.AutoExapandTagsPanel;

            ProxyRadionButton.Checked = Settings.UseProxy;
            if(Settings.UseProxy) {
                ProxyAddressTextbox.Text = Settings.ProxyAddress;
                ProxyPortTextbox.Text = Settings.ProxyPort.ToString();
            }
            this.EnableFavoritesPanel.Checked = Settings.EnableFavoritesPanel;
            this.EnableGroupsMenu.Checked=Settings.EnableGroupsMenu;


            this.AmazonBackupCheckbox.Checked = Settings.UseAmazon;
            this.AccessKeyTextbox.Text = Settings.AmazonAccessKey;
            this.SecretKeyTextbox.Text = Settings.AmazonSecretKey;

            this.AccessKeyTextbox.Enabled = AmazonBackupCheckbox.Checked;
            this.SecretKeyTextbox.Enabled = AmazonBackupCheckbox.Checked;
            this.TestButton.Enabled = AmazonBackupCheckbox.Checked;
            this.BackupButton.Enabled = AmazonBackupCheckbox.Checked;
            this.RestoreButton.Enabled = AmazonBackupCheckbox.Checked;

            this.autoCaseTagsCheckbox.Checked = Settings.AutoCaseTags;
            this.domainTextbox.Text = Settings.DefaultDomain;
            this.usernameTextbox.Text = Settings.DefaultUsername;
            this.passwordTxtBox.Text = Settings.DefaultPassword;
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

            if(ServerNameRadio.Checked) {
                Settings.DefaultSortProperty = Settings.SortProperties.ServerName;
            } else if(NoneRadioButton.Checked) {
                Settings.DefaultSortProperty = Settings.SortProperties.None;
            } else if(ConnectionNameRadioButton.Checked) {
                Settings.DefaultSortProperty = Settings.SortProperties.ConnectionName;
            } else {
                Settings.DefaultSortProperty = Settings.SortProperties.Protocol;
            }

            int.TryParse(this.PortscanTimeoutTextBox.Text, out timeout);

            if (Settings.PortScanTimeoutSeconds <= 0 || Settings.PortScanTimeoutSeconds >= 60)
                timeout = 5;
            
            Settings.PortScanTimeoutSeconds = timeout;
            Settings.ShowInformationToolTips = chkShowInformationToolTips.Checked;
            Settings.ShowUserNameInTitle = chkShowUserNameInTitle.Checked;
            Settings.ShowFullInformationToolTips = chkShowFullInfo.Checked;
            Settings.DefaultDesktopShare = txtDefaultDesktopShare.Text;
            Settings.ExecuteBeforeConnect = chkExecuteBeforeConnect.Checked;
            Settings.ExecuteBeforeConnectArgs = txtArguments.Text;
            Settings.ExecuteBeforeConnectCommand = txtCommand.Text;
            Settings.ExecuteBeforeConnectInitialDirectory = txtInitialDirectory.Text;
            Settings.ExecuteBeforeConnectWaitForExit = chkWaitForExit.Checked;
            Settings.SingleInstance = chkSingleInstance.Checked;
            Settings.AutoSwitchOnCapture = autoSwitchToCaptureCheckbox.Checked;
            Settings.ShowConfirmDialog = chkShowConfirmDialog.Checked;
            Settings.SaveConnectionsOnClose = chkSaveConnections.Checked;
            Settings.ForceComputerNamesAsURI = validateServerNamesCheckbox.Checked;
            Settings.WarnOnConnectionClose = warnDisconnectCheckBox.Checked;

            Settings.Office2007BlackFeel = false;
            Settings.Office2007BlueFeel = false;
            if(RenderBlueRadio.Checked)
                Settings.Office2007BlueFeel = true;
            
            if(RenderBlackRadio.Checked)
                Settings.Office2007BlackFeel = true;            
            
            
            if (this.PasswordProtectTerminalsCheckbox.Checked
                && !string.IsNullOrEmpty(PasswordTextbox.Text)
                && !string.IsNullOrEmpty(ConfirmPasswordTextBox.Text)
                && PasswordTextbox.Text.Equals(ConfirmPasswordTextBox.Text))
            {
                Settings.TerminalsPassword = PasswordTextbox.Text;
            }

            Settings.MinimizeToTray = this.MinimizeToTrayCheckbox.Checked;
            Settings.EnableFavoritesPanel = this.EnableFavoritesPanel.Checked;
            Settings.EnableGroupsMenu = this.EnableGroupsMenu.Checked;
            Settings.AutoExapandTagsPanel = this.AutoExapandTagsPanelCheckBox.Checked;
            
            Settings.UseProxy = ProxyRadionButton.Checked;
            
            if(Settings.UseProxy)
            {
                Settings.ProxyAddress = ProxyAddressTextbox.Text;
                Settings.ProxyPort = Convert.ToInt32(ProxyPortTextbox.Text);
            }

            Settings.UseAmazon = this.AmazonBackupCheckbox.Checked;

            Settings.AmazonAccessKey = this.AccessKeyTextbox.Text;
            Settings.AmazonSecretKey = this.SecretKeyTextbox.Text;

            Settings.DelayConfigurationSave = false;

            Settings.AutoCaseTags = this.autoCaseTagsCheckbox.Checked;

            Settings.DefaultDomain = this.domainTextbox.Text;
            Settings.DefaultUsername = this.usernameTextbox.Text;
            if (!string.IsNullOrEmpty(passwordTxtBox.Text))
                Settings.DefaultPassword = this.passwordTxtBox.Text;            
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
        private void PasswordProtectTerminalsCheckbox_CheckedChanged(object sender, EventArgs e) {
            PasswordTextbox.Enabled = PasswordProtectTerminalsCheckbox.Checked;
            ConfirmPasswordTextBox.Enabled = PasswordProtectTerminalsCheckbox.Checked;
            PasswordsMatchLabel.Visible = PasswordProtectTerminalsCheckbox.Checked;            
        }
        private void PasswordTextbox_TextChanged(object sender, EventArgs e) {
            CheckPasswords();
        }
        private void ConfirmPasswordTextBox_TextChanged(object sender, EventArgs e) {
            CheckPasswords();
        }               
        private void AuthorizeFlickrButton_Click(object sender, EventArgs e) {
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
        private void CompleteAuthButton_Click(object sender, EventArgs e) {
            // Create Flickr instance
            Flickr flickr = new Flickr(Program.FlickrAPIKey, Program.FlickrSharedSecretKey);    
            try {
                // use the temporary Frob to get the authentication
                Auth auth = flickr.AuthGetToken(_tempFrob);
                // Store this Token for later usage,
                // or set your Flickr instance to use it.
                System.Windows.Forms.MessageBox.Show("User authenticated successfully");
                Terminals.Logging.Log.Info("User authenticated successfully. Authentication token is " + auth.Token + ".User id is " + auth.User.UserId + ", username is" + auth.User.Username);
                flickr.AuthToken = auth.Token;
                Settings.FlickrToken = auth.Token;
            } catch(FlickrException ex) {
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

                Settings.TerminalsPassword = "";
                ClearMasterButton.Enabled = false;

                PasswordProtectTerminalsCheckbox.Checked = false;
                PasswordProtectTerminalsCheckbox.Enabled = true;
                PasswordTextbox.Enabled = true;
                ConfirmPasswordTextBox.Enabled = true;
                PasswordTextbox.Text = "";
                ConfirmPasswordTextBox.Text = "";
            }
        }
        private void label10_DoubleClick(object sender, EventArgs e) {
            System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog(this) == DialogResult.OK) {
                string md5 = Terminals.Updates.UpdateManager.GetMD5HashFromFile(ofd.FileName);
                System.Windows.Forms.Clipboard.SetText(string.Format("{0}\r\n{1}", ofd.FileName, md5));
                System.Windows.Forms.MessageBox.Show(string.Format("MD5 of {0}, value:{1}, was copied to the clipboard;", ofd.FileName, md5));
            }
        }
        private void ProxyRadionButton_CheckedChanged(object sender, EventArgs e) {
            this.panel1.Enabled = ProxyRadionButton.Checked;
        }
        private void AutoProxyRadioButton_CheckedChanged(object sender, EventArgs e) {
            this.panel1.Enabled = ProxyRadionButton.Checked;
        }
        private void OptionsForm_Load(object sender, EventArgs e) {
            switch(Settings.DefaultSortProperty) {
                case Settings.SortProperties.ConnectionName:
                    ConnectionNameRadioButton.Checked = true;
                    break;
                case Settings.SortProperties.None:
                    NoneRadioButton.Checked = true;
                    break;
                case Settings.SortProperties.Protocol:
                    ProtocolRadionButton.Checked = true;
                    break;
                case Settings.SortProperties.ServerName:
                    ServerNameRadio.Checked = true;
                    break;

            }
        }
        private void TestButton_Click(object sender, EventArgs e)
        {
            try
            {
                Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
                string testBucket = Guid.NewGuid().ToString();
                wrapper.AddBucket(testBucket);
                string bucket = wrapper.ListBucket(testBucket);
                wrapper.DeleteBucket(testBucket);

                try
                {
                    string terminals = wrapper.ListBucket(_amazonBucket);
                }
                catch (Exception exc)
                {
                    if (exc.Message == "The specified bucket does not exist")
                    {
                        wrapper.AddBucket(_amazonBucket);
                        string terminals = wrapper.ListBucket(_amazonBucket);
                    }
                }

                this.ErrorLabel.Text = "Test was successful!";
                this.ErrorLabel.ForeColor = Color.Black;
            }
            catch (Exception exc)
            {
                this.ErrorLabel.ForeColor = Color.Red;
                this.ErrorLabel.Text = exc.Message;
            }

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
            if (System.Windows.Forms.MessageBox.Show("Are you sure you want to upload your current configuration?", "Amazon S3 Backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
                string url = null;
                try
                {
                    string terminals = wrapper.ListBucket(_amazonBucket);
                }
                catch (Exception exc)
                {
                    wrapper.AddBucket(_amazonBucket);
                }
                try
                {
                    wrapper.AddFileObject(_amazonBucket, _amazonConfigKeyName, Terminals.Program.ConfigurationFileLocation);
                    url = wrapper.GetUrl(_amazonBucket, _amazonConfigKeyName);
                }
                catch (Exception exc)
                {
                    this.ErrorLabel.ForeColor = Color.Red;
                    this.ErrorLabel.Text = exc.Message;
                    return;
                }
                this.ErrorLabel.ForeColor = Color.Black;
                this.ErrorLabel.Text = "The backup was a success!";
            }
        }
        private void RestoreButton_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Are you sure you want to restore your current configuration?", "Amazon S3 Backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper wrapper = new Affirma.ThreeSharp.Wrapper.ThreeSharpWrapper(this.AccessKeyTextbox.Text, this.SecretKeyTextbox.Text);
                try
                {
                    string terminals = wrapper.ListBucket(_amazonBucket);
                }
                catch (Exception exc)
                {
                    this.ErrorLabel.ForeColor = Color.Red;
                    this.ErrorLabel.Text = exc.Message;
                    return;
                }
                try
                {
                    wrapper.GetFileObject(_amazonBucket, _amazonConfigKeyName, Terminals.Program.ConfigurationFileLocation);
                }
                catch (Exception exc)
                {
                    this.ErrorLabel.ForeColor = Color.Red;
                    this.ErrorLabel.Text = exc.Message;
                    return;
                }
                this.ErrorLabel.ForeColor = Color.Black;
                this.ErrorLabel.Text = "The restore was a success!";
            }
        }
        #endregion
    }
}