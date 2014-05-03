using System;
using System.Windows.Forms;

namespace Terminals.Forms.EditFavorite
{
    public partial class RdpDisplayControl : UserControl, IProtocolOptionsControl
    {
        public RdpDisplayControl()
        {
            InitializeComponent();
        }

        private void CmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbResolution.Text == "Custom" || this.cmbResolution.Text == "Auto Scale")
                this.customSizePanel.Visible = true;
            else
                this.customSizePanel.Visible = false;
        }

        public void SetControls()
        {
            this.chkConnectToConsole.Enabled = true;
        }
    }
}
