using System;
using System.Windows.Forms;

namespace Terminals.Forms.EditFavorite
{
    public partial class RdpTsGatewayControl : UserControl
    {
        public RdpTsGatewayControl()
        {
            InitializeComponent();
        }

        private void RadTsgWenable_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlTSGWsettings.Enabled = this.radTSGWenable.Checked;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlTSGWlogon.Enabled = this.chkTSGWlogin.Checked;
        }
    }
}
