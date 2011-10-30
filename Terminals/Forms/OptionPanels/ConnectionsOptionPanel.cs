using System;
using System.Windows.Forms;
using AxMSTSCLib;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class ConnectionsOptionPanel : UserControl, IOptionPanel
    {
        internal AxMsRdpClient6 CurrentTerminal { get; set; }
        private Int32 timeout = 5;

        public ConnectionsOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.validateServerNamesCheckbox.Checked = Settings.ForceComputerNamesAsURI;
            this.warnDisconnectCheckBox.Checked = Settings.WarnOnConnectionClose;
            this.txtDefaultDesktopShare.Text = Settings.DefaultDesktopShare;
            this.PortscanTimeoutTextBox.Text = Settings.PortScanTimeoutSeconds.ToString();
        }

        public void SaveSettings()
        {
            Settings.ForceComputerNamesAsURI = this.validateServerNamesCheckbox.Checked;
            Settings.WarnOnConnectionClose = this.warnDisconnectCheckBox.Checked;
            Settings.DefaultDesktopShare = this.txtDefaultDesktopShare.Text;

            Int32.TryParse(this.PortscanTimeoutTextBox.Text, out timeout);
            if (Settings.PortScanTimeoutSeconds <= 0 || Settings.PortScanTimeoutSeconds >= 60)
                timeout = 5;
            Settings.PortScanTimeoutSeconds = timeout;
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
                                                    .Replace("%USER%", this.CurrentTerminal.UserName);
            }
            else
            {
                this.EvaluatedDesktopShareLabel.Text = String.Empty;
            }
        }
    }
}
