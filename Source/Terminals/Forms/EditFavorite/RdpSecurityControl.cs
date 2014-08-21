using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class RdpSecurityControl : UserControl, IProtocolOptionsControl
    {
        public RdpSecurityControl()
        {
            InitializeComponent();
        }

        private void SecuritySettingsEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.panel2.Enabled = this.SecuritySettingsEnabledCheckbox.Checked;
        }

        public void SaveTo(IFavorite favorite)
        {
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions == null)
                return;

            rdpOptions.Security.Enabled = this.SecuritySettingsEnabledCheckbox.Checked;
            if (this.SecuritySettingsEnabledCheckbox.Checked)
            {
                rdpOptions.Security.WorkingFolder = this.SecurityWorkingFolderTextBox.Text;
                rdpOptions.Security.StartProgram = this.SecuriytStartProgramTextbox.Text;
                rdpOptions.FullScreen = this.SecurityStartFullScreenCheckbox.Checked;
            }
        }

        public void LoadFrom(IFavorite favorite)
        {
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions == null)
                return;

            this.SecuritySettingsEnabledCheckbox.Checked = rdpOptions.Security.Enabled;
            this.SecurityWorkingFolderTextBox.Text = rdpOptions.Security.WorkingFolder;
            this.SecuriytStartProgramTextbox.Text = rdpOptions.Security.StartProgram;
            this.SecurityStartFullScreenCheckbox.Checked = rdpOptions.FullScreen;
        }
    }
}
