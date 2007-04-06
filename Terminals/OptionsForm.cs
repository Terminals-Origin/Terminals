using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMSTSCLib;

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
            chkSingleInstance.Checked = Settings.SingleInstance;
            chkShowConfirmDialog.Checked = Settings.ShowConfirmDialog;
            chkSaveConnections.Checked = Settings.SaveConnectionsOnClose;
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
            Settings.ShowConfirmDialog = chkShowConfirmDialog.Checked;
            Settings.SaveConnectionsOnClose = chkSaveConnections.Checked;
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
    }
}