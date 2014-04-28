using System;
using System.Windows.Forms;

namespace Terminals.Forms.EditFavorite
{
    public partial class GeneralPropertiesUserControl : UserControl
    {
        public GeneralPropertiesUserControl()
        {
            InitializeComponent();
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            // this.SetOkButtonState();
        }

        private void CredentialDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.CredentialsPanel.Enabled = true;
            //ICredentialSet set = this.CredentialDropdown.SelectedItem as ICredentialSet;

            //if (set != null)
            //{
            //    this.CredentialsPanel.Enabled = false;
            //    this.cmbDomains.Text = set.Domain;
            //    this.cmbUsers.Text = set.UserName;
            //    this.txtPassword.Text = set.Password;
            //    this.chkSavePassword.Checked = true;
            //}
        }

        private void CredentialManagerPicturebox_Click(object sender, EventArgs e)
        {
            //Guid selectedCredentialId = Guid.Empty;
            //var selectedCredential = this.CredentialDropdown.SelectedItem as ICredentialSet;
            //if (selectedCredential != null)
            //    selectedCredentialId = selectedCredential.Id;

            //using (var mgr = new CredentialManager(this.persistence))
            //    mgr.ShowDialog();

            //this.FillCredentialsCombobox(selectedCredentialId);
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog = CreateIconSelectionDialog();

            //if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    this.TryToAssignNewToolbarIcon(openFileDialog.FileName);
            //}
        }

        private void CmbServers_TextChanged(object sender, EventArgs e)
        {
            // this.SetOkButtonState();
        }

        private void CmbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ProtocolComboBox.Text == ConnectionManager.RAS)
            //{
            //    this.LoadDialupConnections();
            //    this.RASDetailsListBox.Items.Clear();
            //    if (this.dialupList != null && this.dialupList.ContainsKey(cmbServers.Text))
            //    {
            //        RASENTRY selectedRAS = this.dialupList[this.cmbServers.Text];
            //        this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Connection", this.cmbServers.Text));
            //        this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Area Code", selectedRAS.AreaCode));
            //        this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Country Code", selectedRAS.CountryCode));
            //        this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Device Name", selectedRAS.DeviceName));
            //        this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Device Type", selectedRAS.DeviceType));
            //        this.RASDetailsListBox.Items.Add(String.Format("{0}:{1}", "Local Phone Number", selectedRAS.LocalPhoneNumber));
            //    }
            //}
        }

        private void CmbServers_Leave(object sender, EventArgs e)
        {
            //if (this.txtName.Text == String.Empty)
            //{
            //    if (this.cmbServers.Text.Contains(":"))
            //    {
            //        String server = String.Empty;
            //        int port;
            //        GetServerAndPort(this.cmbServers.Text, out server, out port);
            //        this.cmbServers.Text = server;
            //        this.txtPort.Text = port.ToString(CultureInfo.InvariantCulture);
            //        this.cmbServers.Text = server;
            //    }

            //    this.txtName.Text = this.cmbServers.Text;
            //}
        }

        private void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.SetControlsProtocolIndependent();
            //this.validator.OnServerNameValidating(this.cmbServers, new CancelEventArgs());

            //switch (this.ProtocolComboBox.Text)
            //{
            //    case ConnectionManager.RDP:
            //        this.SetControlsForRdp();
            //        break;
            //    case ConnectionManager.VMRC:
            //        this.VmrcGroupBox.Enabled = true;
            //        break;
            //    case ConnectionManager.RAS:
            //        this.SetControlsForRas();
            //        break;
            //    case ConnectionManager.VNC:
            //        this.VncGroupBox.Enabled = true;
            //        break;
            //    case ConnectionManager.ICA_CITRIX:
            //        this.IcaGroupBox.Enabled = true;
            //        break;
            //    case ConnectionManager.HTTP:
            //    case ConnectionManager.HTTPS:
            //        this.SetControlsForWeb();
            //        break;
            //    case ConnectionManager.SSH:
            //    case ConnectionManager.TELNET:
            //        this.ConsoleGroupBox.Enabled = true;
            //        if (this.ProtocolComboBox.Text == ConnectionManager.SSH)
            //            this.SshGroupBox.Enabled = true;
            //        break;
            //}

            //int defaultPort = ConnectionManager.GetPort(this.ProtocolComboBox.Text);
            //this.txtPort.Text = defaultPort.ToString(CultureInfo.InvariantCulture);
            //this.SetOkButtonState();
        }

        private void HttpUrlTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (ConnectionManager.IsProtocolWebBased(this.ProtocolComboBox.Text))
            //{
            //    Uri newUrl = GetFullUrlFromHttpTextBox();
            //    if (newUrl != null)
            //    {
            //        this.cmbServers.Text = newUrl.Host;
            //        this.txtPort.Text = newUrl.Port.ToString(CultureInfo.InvariantCulture);
            //    }
            //    SetOkButtonState();
            //}
        }
    }
}
