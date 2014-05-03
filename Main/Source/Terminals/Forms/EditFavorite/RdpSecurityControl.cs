using System;
using System.Windows.Forms;

namespace Terminals.Forms.EditFavorite
{
    public partial class RdpSecurityControl : UserControl
    {
        public RdpSecurityControl()
        {
            InitializeComponent();
        }

        private void SecuritySettingsEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.panel2.Enabled = this.SecuritySettingsEnabledCheckbox.Checked;
        }
    }
}
