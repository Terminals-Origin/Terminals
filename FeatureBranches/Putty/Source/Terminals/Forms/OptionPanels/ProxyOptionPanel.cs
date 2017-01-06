using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class ProxyOptionPanel : UserControl, IOptionPanel
    {
        private readonly Settings settings = Settings.Instance;

        public ProxyOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.AutoProxyRadioButton.Checked = !settings.UseProxy;
            this.ProxyRadionButton.Checked = settings.UseProxy;
            this.ProxyAddressTextbox.Text = settings.ProxyAddress;
            this.ProxyPortTextbox.Text = (settings.ProxyPort.ToString().Equals("0")) ? "80" : settings.ProxyPort.ToString();
            this.ProxyAddressTextbox.Enabled = settings.UseProxy;
            this.ProxyPortTextbox.Enabled = settings.UseProxy;
        }

        public void SaveSettings()
        {
            settings.UseProxy = this.ProxyRadionButton.Checked;
            settings.ProxyAddress = this.ProxyAddressTextbox.Text;
            settings.ProxyPort = Convert.ToInt32(this.ProxyPortTextbox.Text);
        }

        private void ProxyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.ProxyAddressTextbox.Enabled = this.ProxyRadionButton.Checked;
            this.ProxyPortTextbox.Enabled = this.ProxyRadionButton.Checked;
        }
    }
}
