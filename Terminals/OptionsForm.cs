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
        public OptionsForm(AxMsRdpClient2 terminal)
        {
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
            currentTerminal = terminal;
        }

        private AxMsRdpClient2 currentTerminal;

        private void btnOk_Click(object sender, EventArgs e)
        {
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
    }
}