using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Connections;

namespace Terminals.Forms
{
    internal partial class ConnectionsOptionPanel : UserControl, IOptionPanel
    {
        private readonly Settings settings = Settings.Instance;
        internal IConnectionExtra CurrentTerminal { get; set; }

        public ConnectionsOptionPanel()
        {
            InitializeComponent();
            this.EvaluatedDesktopShareLabel.Text = String.Empty;
        }

        public void LoadSettings()
        {
            this.validateServerNamesCheckbox.Checked = settings.ForceComputerNamesAsURI;
            this.warnDisconnectCheckBox.Checked = settings.WarnOnConnectionClose;
            this.tryReconnectCheckBox.Checked = settings.AskToReconnect;
            this.txtDefaultDesktopShare.Text = settings.DefaultDesktopShare;
            this.PortscanTimeoutTextBox.Text = settings.PortScanTimeoutSeconds.ToString();
            this.restoreWindowCheckbox.Checked = settings.RestoreWindowOnLastTerminalDisconnect;
        }

        public void SaveSettings()
        {
            settings.ForceComputerNamesAsURI = this.validateServerNamesCheckbox.Checked;
            settings.WarnOnConnectionClose = this.warnDisconnectCheckBox.Checked;
            settings.AskToReconnect = this.tryReconnectCheckBox.Checked;
            settings.DefaultDesktopShare = this.txtDefaultDesktopShare.Text;
            settings.RestoreWindowOnLastTerminalDisconnect = this.restoreWindowCheckbox.Checked;
            settings.PortScanTimeoutSeconds = this.ResolveTimeOut();
        }

        private int ResolveTimeOut()
        {
            Int32 timeout = 5;
            Int32.TryParse(this.PortscanTimeoutTextBox.Text, out timeout);
            if (timeout <= 0 || timeout >= 60)
                timeout = 5;
            return timeout;
        }

        private void txtDefaultDesktopShare_TextChanged(object sender, EventArgs e)
        {
            var desktopShares = new DesktopShares(this.CurrentTerminal, this.txtDefaultDesktopShare.Text);
            this.EvaluatedDesktopShareLabel.Text = desktopShares.EvaluateDesktopShare();
        }
    }
}
