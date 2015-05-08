using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;

namespace Terminals.Network
{
    internal partial class NetworkShares : UserControl
    {
        public NetworkShares()
        {
            InitializeComponent();
        }

        private void LoadShares(string username, string password, string computer)
        {
            try
            {
                this.connectButton.Enabled = false;
                this.connectButton.Text = "Connecting...";
                this.dataGridView1.DataSource = TryLoadShares(username, password, computer);
            }
            catch
            {
                MessageBox.Show("Unable to loads shares for remote compuer.", "Loading shares",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.connectButton.Enabled = true;
                this.connectButton.Text = "Connect";
            }
        }

        private static List<Share> TryLoadShares(string username, string password, string computer)
        {
            var shares = new List<Share>();
            ManagementObjectSearcher searcher = CreateSearcher(username, password, computer);

            foreach (ManagementObject share in searcher.Get())
            {
                var s = new Share();
                FillShareByManagementObject(share, s);
                shares.Add(s);
            }
            return shares;
        }

        private static void FillShareByManagementObject(ManagementObject share, Share s)
        {
            foreach (PropertyData property in share.Properties)
            {
                switch (property.Name)
                {
                    case "AccessMask":
                        s.AccessMask = ResolvePropertyValue(property);
                        break;
                    case "MaximumAllowed":
                        s.MaximumAllowed = ResolvePropertyValue(property);
                        break;
                    case "InstallDate":
                        s.InstallDate = ResolvePropertyValue(property);
                        break;
                    case "Description":
                        s.Description = ResolvePropertyValue(property);
                        break;
                    case "Caption":
                        s.Caption = ResolvePropertyValue(property);
                        break;
                    case "AllowMaximum":
                        s.AllowMaximum = ResolvePropertyValue(property);
                        break;
                    case "Name":
                        s.Name = ResolvePropertyValue(property);
                        break;
                    case "Path":
                        s.Path = ResolvePropertyValue(property);
                        break;
                    case "Status":
                        s.Status = ResolvePropertyValue(property);
                        break;
                    case "Type":
                        s.TypeId = ResolvePropertyValue(property);
                        break;
                }
            }
        }

        private static string ResolvePropertyValue(PropertyData property)
        {
            if (property.Value != null)
                return property.Value.ToString();
            return string.Empty;
        }

        private static ManagementObjectSearcher CreateSearcher(string username, string password, string computer)
        {
            var query = new ObjectQuery("select * from win32_share");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(computer) || computer.StartsWith(@"\\localhost"))
                return new ManagementObjectSearcher(query);

            return CreateRemoteSearcher(username, password, computer, query);
        }

        private static ManagementObjectSearcher CreateRemoteSearcher(string username, string password,
            string computer, ObjectQuery query)
        {
            var oConn = new ConnectionOptions();
            oConn.Username = username;
            oConn.Password = password;
            if (!computer.StartsWith(@"\\"))
                computer = @"\\" + computer;
            if (!computer.ToLower().EndsWith(@"\root\cimv2"))
                computer = computer + @"\root\cimv2";
            var oMs = new ManagementScope(computer, oConn);
            return new ManagementObjectSearcher(oMs, query);
        }

        private void NetworkShares_Load(object sender, EventArgs e)
        {
            this.LoadShares(string.Empty, string.Empty, string.Empty);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            this.LoadShares(this.wmiServerCredentials1.Username, this.wmiServerCredentials1.Password, this.wmiServerCredentials1.SelectedServer);
        }

        private void WmiServerCredentials1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ConnectButton_Click(this, EventArgs.Empty);
            }
        }
    }
}
