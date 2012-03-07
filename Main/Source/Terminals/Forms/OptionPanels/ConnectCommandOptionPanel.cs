using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class ConnectCommandOptionPanel : UserControl, IOptionPanel
    {
        public ConnectCommandOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkExecuteBeforeConnect.Checked = Settings.ExecuteBeforeConnect;
            this.txtCommand.Text = Settings.ExecuteBeforeConnectCommand;
            this.txtArguments.Text = Settings.ExecuteBeforeConnectArgs;
            this.txtInitialDirectory.Text = Settings.ExecuteBeforeConnectInitialDirectory;
            this.chkWaitForExit.Checked = Settings.ExecuteBeforeConnectWaitForExit;

            this.txtCommand.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtArguments.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtInitialDirectory.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.chkWaitForExit.Enabled = this.chkExecuteBeforeConnect.Checked;
        }

        public void SaveSettings()
        {
            Settings.ExecuteBeforeConnect = this.chkExecuteBeforeConnect.Checked;
            Settings.ExecuteBeforeConnectCommand = this.txtCommand.Text;
            Settings.ExecuteBeforeConnectArgs = this.txtArguments.Text;
            Settings.ExecuteBeforeConnectInitialDirectory = this.txtInitialDirectory.Text;
            Settings.ExecuteBeforeConnectWaitForExit = this.chkWaitForExit.Checked;
        }

        private void chkExecuteBeforeConnect_CheckedChanged(object sender, EventArgs e)
        {
            Boolean enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtCommand.Enabled = enabled;
            this.txtArguments.Enabled = enabled;
            this.txtInitialDirectory.Enabled = enabled;
            this.chkWaitForExit.Enabled = enabled;
        }
    }
}
