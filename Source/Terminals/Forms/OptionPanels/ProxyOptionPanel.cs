using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class ProxyOptionPanel : UserControl, IOptionPanel
    {
        public ProxyOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.AutoProxyRadioButton.Checked = !Settings.UseProxy;
            this.ProxyRadionButton.Checked = Settings.UseProxy;
            this.ProxyAddressTextbox.Text = Settings.ProxyAddress;
            this.ProxyPortTextbox.Text = (Settings.ProxyPort.ToString().Equals("0")) ? "80" : Settings.ProxyPort.ToString();
            this.ProxyAddressTextbox.Enabled = Settings.UseProxy;
            this.ProxyPortTextbox.Enabled = Settings.UseProxy;
        }

        public void SaveSettings()
        {
            Settings.UseProxy = this.ProxyRadionButton.Checked;
            Settings.ProxyAddress = this.ProxyAddressTextbox.Text;
            Settings.ProxyPort = Convert.ToInt32(this.ProxyPortTextbox.Text);
        }

        private void ProxyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.ProxyAddressTextbox.Enabled = this.ProxyRadionButton.Checked;
            this.ProxyPortTextbox.Enabled = this.ProxyRadionButton.Checked;
        }
    }
}
