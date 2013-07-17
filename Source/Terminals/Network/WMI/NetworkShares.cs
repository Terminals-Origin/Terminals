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
            List<Share> shares = new List<Share>();
            const string QRY = "select * from win32_share";
            ManagementObjectSearcher searcher;
            ObjectQuery query = new ObjectQuery(QRY);

            if (username != "" && password != "" && computer != "" && !computer.StartsWith(@"\\localhost"))
            {
                ConnectionOptions oConn = new ConnectionOptions();
                oConn.Username = username;
                oConn.Password = password;
                if (!computer.StartsWith(@"\\")) 
                    computer = @"\\" + computer;
                if (!computer.ToLower().EndsWith(@"\root\cimv2"))
                    computer = computer + @"\root\cimv2";
                ManagementScope oMs = new ManagementScope(computer, oConn);

                searcher = new ManagementObjectSearcher(oMs, query);
            }
            else
            {
                searcher = new ManagementObjectSearcher(query);
            }

            foreach (ManagementObject share in searcher.Get())
            {
                Share s = new Share();
                foreach (PropertyData p in share.Properties)
                {
                    switch (p.Name)
                    {
                        case "AccessMask":
                            if (p.Value != null) s.AccessMask = p.Value.ToString();
                            break;
                        case "MaximumAllowed":
                            if (p.Value != null) s.MaximumAllowed = p.Value.ToString();
                            break;
                        case "InstallDate":
                            if (p.Value != null) s.InstallDate = p.Value.ToString();
                            break;
                        case "Description":
                            if (p.Value != null) s.Description = p.Value.ToString();
                            break;
                        case "Caption":
                            if (p.Value != null) s.Caption = p.Value.ToString();
                            break;
                        case "AllowMaximum":
                            if (p.Value != null) s.AllowMaximum = p.Value.ToString();
                            break;
                        case "Name":
                            if (p.Value != null) s.Name = p.Value.ToString();
                            break;
                        case "Path":
                            if (p.Value != null) s.Path = p.Value.ToString();
                            break;
                        case "Status":
                            if (p.Value != null) s.Status = p.Value.ToString();
                            break;
                        case "Type":
                            if (p.Value != null) s.TypeId = p.Value.ToString();
                            break;
                    }
                }
                shares.Add(s);
            }
            this.dataGridView1.DataSource = shares;
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
