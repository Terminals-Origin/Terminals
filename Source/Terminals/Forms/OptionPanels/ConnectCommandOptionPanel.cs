using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class ConnectCommandOptionPanel : UserControl, IOptionPanel
    {
        private readonly Settings settings = Settings.Instance;

        public ConnectCommandOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkExecuteBeforeConnect.Checked = settings.ExecuteBeforeConnect;
            this.txtCommand.Text = settings.ExecuteBeforeConnectCommand;
            this.txtArguments.Text = settings.ExecuteBeforeConnectArgs;
            this.txtInitialDirectory.Text = settings.ExecuteBeforeConnectInitialDirectory;
            this.chkWaitForExit.Checked = settings.ExecuteBeforeConnectWaitForExit;

            this.txtCommand.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtArguments.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtInitialDirectory.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.chkWaitForExit.Enabled = this.chkExecuteBeforeConnect.Checked;
        }

        public void SaveSettings()
        {
            settings.ExecuteBeforeConnect = this.chkExecuteBeforeConnect.Checked;
            settings.ExecuteBeforeConnectCommand = this.txtCommand.Text;
            settings.ExecuteBeforeConnectArgs = this.txtArguments.Text;
            settings.ExecuteBeforeConnectInitialDirectory = this.txtInitialDirectory.Text;
            settings.ExecuteBeforeConnectWaitForExit = this.chkWaitForExit.Checked;
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
