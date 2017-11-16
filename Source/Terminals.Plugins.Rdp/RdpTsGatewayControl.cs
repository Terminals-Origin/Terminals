using System;
using System.Windows.Forms;
using Terminals.Common.Data.Interfaces;
using Terminals.Common.Forms.EditFavorite;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class RdpTsGatewayControl : UserControl, IProtocolOptionsControl,
        ISupportsSecurityControl, IRequiresMRUSettings, IGatewaySecurity
    {
        public IGuardedCredentialFactory CredentialFactory { get; set; }

        public IMRUSettings Settings
        {
            get { return this.credentialsPanel1.Settings; }
            set { this.credentialsPanel1.Settings = value; }
        }

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
            var guarded = this.CredentialFactory.CreateCredential(tsgwOptions.Security);
            this.credentialsPanel1.SaveTo(guarded);
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

            this.securityPanel1.LoadFrom(favorite.Security);
            this.securityPanel1.FillCredentialsCombobox(favorite.Security.Credential);
        }

        private void FillTsGatewayControls(RdpOptions rdpOptions)
        {
            this.credentialsPanel1.LoadMRUs();
            TsGwOptions tsGateway = rdpOptions.TsGateway;
            this.ApplyUsageMethod(tsGateway.UsageMethod);
            this.txtTSGWServer.Text = tsGateway.HostName;
            this.chkTSGWlogin.Checked = tsGateway.SeparateLogin;
            this.credentialsPanel1.Enabled = tsGateway.SeparateLogin;

            if (tsGateway.SeparateLogin)
            {
                var guarded = this.CredentialFactory.CreateCredential(tsGateway.Security);
                this.credentialsPanel1.LoadFrom(guarded);
            }

            if (tsGateway.CredentialSource == 4)
                this.cmbTSGWLogonMethod.SelectedIndex = 2;
            else
                this.cmbTSGWLogonMethod.SelectedIndex = tsGateway.CredentialSource;
        }

        private void ApplyUsageMethod(int usageMethod)
        {
            switch (usageMethod)
            {
                case 0:
                    this.ApplyUsageMethod(false, false);
                    break;
                case 1:
                    this.ApplyUsageMethod(true, false);
                    break;
                case 2:
                    this.ApplyUsageMethod(true, true);
                    break;
                case 4: // this setup doesnt make sence, keeping because of historical reasons
                    this.ApplyUsageMethod(false, true);
                    break;
            }
        }

        private void ApplyUsageMethod(bool tsgwEnabled, bool localBypasEnabled)
        {
            this.radTSGWdisable.Checked = !tsgwEnabled;
            this.radTSGWenable.Checked = tsgwEnabled;
            this.chkTSGWlocalBypass.Checked = localBypasEnabled;
        }

        public void InitSecurityPanel(ISecurityService service, IMRUSettings settings)
        {
            this.securityPanel1.AssignServices(service, settings);
            //this.securityPanel1.FillCredentialsCombobox(Guid.Empty);
        }
    }
}
