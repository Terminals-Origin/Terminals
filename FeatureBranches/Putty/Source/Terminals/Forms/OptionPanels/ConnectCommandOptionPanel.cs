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
            this.chkExecuteBeforeConnect.Checked = settings.Execute;
            this.txtCommand.Text = settings.Command;
            this.txtArguments.Text = settings.CommandArguments;
            this.txtInitialDirectory.Text = settings.InitialDirectory;
            this.chkWaitForExit.Checked = settings.WaitForExit;

            this.txtCommand.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtArguments.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.txtInitialDirectory.Enabled = this.chkExecuteBeforeConnect.Checked;
            this.chkWaitForExit.Enabled = this.chkExecuteBeforeConnect.Checked;
        }

        public void SaveSettings()
        {
            settings.Execute = this.chkExecuteBeforeConnect.Checked;
            settings.Command = this.txtCommand.Text;
            settings.CommandArguments = this.txtArguments.Text;
            settings.InitialDirectory = this.txtInitialDirectory.Text;
            settings.WaitForExit = this.chkWaitForExit.Checked;
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
