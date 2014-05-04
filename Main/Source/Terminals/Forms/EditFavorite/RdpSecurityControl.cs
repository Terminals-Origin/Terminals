using System;
using System.Windows.Forms;
using Terminals.Data;

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

        private void FillFavoriteRdpSecurity(RdpOptions rdpOptions)
        {
            // todo move to extended settings control rdpOptions.Security.EnableTLSAuthentication = this.EnableTLSAuthenticationCheckbox.Checked;
            //rdpOptions.Security.EnableNLAAuthentication = this.EnableNLAAuthenticationCheckbox.Checked;
            //rdpOptions.Security.EnableEncryption = this.EnableEncryptionCheckbox.Checked;
            rdpOptions.Security.Enabled = this.SecuritySettingsEnabledCheckbox.Checked;
            if (this.SecuritySettingsEnabledCheckbox.Checked)
            {
                rdpOptions.Security.WorkingFolder = this.SecurityWorkingFolderTextBox.Text;
                rdpOptions.Security.StartProgram = this.SecuriytStartProgramTextbox.Text;
                rdpOptions.FullScreen = this.SecurityStartFullScreenCheckbox.Checked;
            }
        }


        private void FillRdpSecurityControls(RdpOptions rdpOptions)
        {
            // todo move to extended settings control this.EnableTLSAuthenticationCheckbox.Checked = rdpOptions.Security.EnableTLSAuthentication;
            //this.EnableNLAAuthenticationCheckbox.Checked = rdpOptions.Security.EnableNLAAuthentication;
            //this.EnableEncryptionCheckbox.Checked = rdpOptions.Security.EnableEncryption;
            this.SecuritySettingsEnabledCheckbox.Checked = rdpOptions.Security.Enabled;
            this.SecurityWorkingFolderTextBox.Text = rdpOptions.Security.WorkingFolder;
            this.SecuriytStartProgramTextbox.Text = rdpOptions.Security.StartProgram;
            this.SecurityStartFullScreenCheckbox.Checked = rdpOptions.FullScreen;
        }

    }
}
