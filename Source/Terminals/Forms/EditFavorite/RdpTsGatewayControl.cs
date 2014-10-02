using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class RdpTsGatewayControl : UserControl, IProtocolOptionsControl
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
            this.credentialsPanel1.Enabled = this.chkTSGWlogin.Checked;
        }

        public void SaveTo(IFavorite favorite)
        {
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions == null)
                return;

            FillFavoriteTSgatewayOptions(rdpOptions);
        }

        private void FillFavoriteTSgatewayOptions(RdpOptions rdpOptions)
        {
            this.credentialsPanel1.SaveMRUs();
            TsGwOptions tsgwOptions = rdpOptions.TsGateway;
            tsgwOptions.HostName = this.txtTSGWServer.Text;
            this.credentialsPanel1.SaveTo(tsgwOptions.Security);
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

        public void LoadFrom(IFavorite favorite)
        {
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions == null)
                return;

            FillTsGatewayControls(rdpOptions);
        }

        private void FillTsGatewayControls(RdpOptions rdpOptions)
        {
            this.credentialsPanel1.LoadMRUs();
            TsGwOptions tsGateway = rdpOptions.TsGateway;

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
            this.chkTSGWlogin.Checked = tsGateway.SeparateLogin;
            this.credentialsPanel1.Enabled = tsGateway.SeparateLogin;

            if (tsGateway.SeparateLogin)
                this.credentialsPanel1.LoadFrom(tsGateway.Security);
            
            if (tsGateway.CredentialSource == 4)
                this.cmbTSGWLogonMethod.SelectedIndex = 2;
            else
                this.cmbTSGWLogonMethod.SelectedIndex = tsGateway.CredentialSource;
        }
    }
}
