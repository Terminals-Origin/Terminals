using System;
using System.Windows.Forms;
using AxMSTSCLib;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class ConnectionsOptionPanel : UserControl, IOptionPanel
    {
        internal AxMsRdpClient6 CurrentTerminal { get; set; }

        public ConnectionsOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.validateServerNamesCheckbox.Checked = Settings.ForceComputerNamesAsURI;
            this.warnDisconnectCheckBox.Checked = Settings.WarnOnConnectionClose;
            this.tryReconnectCheckBox.Checked = Settings.AskToReconnect;
            this.txtDefaultDesktopShare.Text = Settings.DefaultDesktopShare;
            this.PortscanTimeoutTextBox.Text = Settings.PortScanTimeoutSeconds.ToString();
            this.restoreWindowCheckbox.Checked = Settings.RestoreWindowOnLastTerminalDisconnect;
        }

        public void SaveSettings()
        {
            Settings.ForceComputerNamesAsURI = this.validateServerNamesCheckbox.Checked;
            Settings.WarnOnConnectionClose = this.warnDisconnectCheckBox.Checked;
            Settings.AskToReconnect = this.tryReconnectCheckBox.Checked;
            Settings.DefaultDesktopShare = this.txtDefaultDesktopShare.Text;
            Settings.RestoreWindowOnLastTerminalDisconnect = this.restoreWindowCheckbox.Checked;
            Settings.PortScanTimeoutSeconds = this.ResolveTimeOut();
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
            this.EvaluateDesktopShare();
        }

        private void EvaluateDesktopShare()
        {
            if (this.CurrentTerminal != null)
            {
                this.EvaluatedDesktopShareLabel.Text =
                    this.txtDefaultDesktopShare.Text.Replace("%SERVER%", this.CurrentTerminal.Server)
                                                    .Replace("%USER%", this.CurrentTerminal.UserName)
                                                    .Replace("%server%", this.CurrentTerminal.Server)
                                                    .Replace("%user%", this.CurrentTerminal.UserName);

            }
            else
            {
                this.EvaluatedDesktopShareLabel.Text = String.Empty;
            }
        }
    }
}
