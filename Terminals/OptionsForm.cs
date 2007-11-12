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
            office2007FeelCheckbox.Checked = Settings.Office2007Feel;
            currentTerminal = terminal;
            this.PortscanTimeoutTextBox.Text = Settings.PortScanTimeoutSeconds.ToString();
            if (Settings.TerminalsPassword != string.Empty) {
                PasswordProtectTerminalsCheckbox.Checked = true;
                PasswordProtectTerminalsCheckbox.Enabled = false;
                PasswordTextbox.Enabled = false;
                ConfirmPasswordTextBox.Enabled = false;
            }
            this.MinimizeToTrayCheckbox.Checked = Settings.MinimizeToTray;
        }

        private AxMsRdpClient2 currentTerminal;

        private void btnOk_Click(object sender, EventArgs e)
        {
            int timeout = 5;
            int.TryParse(this.PortscanTimeoutTextBox.Text, out timeout);
            if (Settings.PortScanTimeoutSeconds <= 0 || Settings.PortScanTimeoutSeconds >= 60) timeout = 5;
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
            Settings.Office2007Feel = office2007FeelCheckbox.Checked;
            if (this.PasswordProtectTerminalsCheckbox.Checked && PasswordTextbox.Text!=string.Empty && ConfirmPasswordTextBox.Text!=string.Empty && PasswordTextbox.Text == ConfirmPasswordTextBox.Text) {
                Settings.TerminalsPassword = PasswordTextbox.Text;
            }
            Settings.MinimizeToTray = this.MinimizeToTrayCheckbox.Checked;
        }

        private void chkShowInformationToolTips_CheckedChanged(object sender, EventArgs e)
        {
            chkShowFullInfo.Enabled = chkShowInformationToolTips.Checked;
            if (!chkShowInformationToolTips.Checked)
            {
                chkShowFullInfo.Checked = false;                
            }
        }

        private void EvaluateDesktopShare()
        {
            if (currentTerminal != null)
            {
                lblEvaluatedDesktopShare.Text = txtDefaultDesktopShare.Text.Replace("%SERVER%", currentTerminal.Server).Replace(
                    "%USER%", currentTerminal.UserName);
            }
            else
            {
                lblEvaluatedDesktopShare.Text = String.Empty;
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
        private void CheckPasswords() {
            if (PasswordTextbox.Text != ConfirmPasswordTextBox.Text) {
                PasswordsMatchLabel.Text = "Passwords do not match";
            } else {
                PasswordsMatchLabel.Text = "Passwords match";
            }
        }
        string tempFrob;
        private void AuthorizeFlickrButton_Click(object sender, EventArgs e) {
            // Create Flickr instance    
            Flickr flickr = new Flickr(MainForm.FlickrAPIKey, MainForm.FlickrSharedSecretKey);    
            // Get Frob        
            tempFrob = flickr.AuthGetFrob();
            // Calculate the URL at Flickr to redirect the user to    
            string flickrUrl = flickr.AuthCalcUrl(tempFrob, AuthLevel.Write);    
            // The following line will load the URL in the users default browser.    
            System.Diagnostics.Process.Start(flickrUrl);
            CompleteAuthButton.Enabled = true;
        }

        private void CompleteAuthButton_Click(object sender, EventArgs e) {
            // Create Flickr instance
            Flickr flickr = new Flickr(MainForm.FlickrAPIKey, MainForm.FlickrSharedSecretKey);    
            try {
                // use the temporary Frob to get the authentication
                Auth auth = flickr.AuthGetToken(tempFrob);
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
    }
}