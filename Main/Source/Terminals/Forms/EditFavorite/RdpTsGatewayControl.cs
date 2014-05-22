using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    public partial class RdpTsGatewayControl : UserControl
    {
        public RdpTsGatewayControl()
        {
            InitializeComponent();

            // move following line down to default value only once smart card access worked out.
            this.cmbTSGWLogonMethod.SelectedIndex = 0;
        }

        private void RadTsgWenable_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlTSGWsettings.Enabled = this.radTSGWenable.Checked;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlTSGWlogon.Enabled = this.chkTSGWlogin.Checked;
        }

        private void FillFavoriteTSgatewayOptions(RdpOptions rdpOptions)
        {
            TsGwOptions tsgwOptions = rdpOptions.TsGateway;
            tsgwOptions.HostName = this.txtTSGWServer.Text;
            tsgwOptions.Security.Domain = this.txtTSGWDomain.Text;
            tsgwOptions.Security.UserName = this.txtTSGWUserName.Text;
            tsgwOptions.Security.Password = this.txtTSGWPassword.Text;
            tsgwOptions.SeparateLogin = this.chkTSGWlogin.Checked;
            tsgwOptions.CredentialSource = this.cmbTSGWLogonMethod.SelectedIndex;
            if (tsgwOptions.CredentialSource == 2)
                tsgwOptions.CredentialSource = 4;

            if (this.radTSGWenable.Checked)
            {
                if (this.chkTSGWlocalBypass.Checked)
                    tsgwOptions.UsageMethod = 2;
                else
                    tsgwOptions.UsageMethod = 1;
            }
            else
            {
                if (this.chkTSGWlocalBypass.Checked)
                    tsgwOptions.UsageMethod = 4;
                else
                    tsgwOptions.UsageMethod = 0;
            }
        }
        
        private void FillTsGatewayControls(RdpOptions rdpOptions)
        {
            var tsGateway = rdpOptions.TsGateway;
            switch (tsGateway.UsageMethod)
            {
                case 0:
                    this.radTSGWdisable.Checked = true;
                    this.chkTSGWlocalBypass.Checked = false;
                    break;
                case 1:
                    this.radTSGWenable.Checked = true;
                    this.chkTSGWlocalBypass.Checked = false;
                    break;
                case 2:
                    this.radTSGWenable.Checked = true;
                    this.chkTSGWlocalBypass.Checked = true;
                    break;
                case 4:
                    this.radTSGWdisable.Checked = true;
                    this.chkTSGWlocalBypass.Checked = true;
                    break;
            }

            this.txtTSGWServer.Text = tsGateway.HostName;
            this.txtTSGWDomain.Text = tsGateway.Security.Domain;
            this.txtTSGWUserName.Text = tsGateway.Security.UserName;
            this.txtTSGWPassword.Text = tsGateway.Security.Password;
            this.chkTSGWlogin.Checked = tsGateway.SeparateLogin;
            if (tsGateway.CredentialSource == 4)
            {
                this.cmbTSGWLogonMethod.SelectedIndex = 2;
            }
            else
            {
                this.cmbTSGWLogonMethod.SelectedIndex = tsGateway.CredentialSource;

            }
        }

    }
}
