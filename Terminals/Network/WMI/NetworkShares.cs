using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network
{
    public partial class NetworkShares : UserControl
    {
        public NetworkShares()
        {
            InitializeComponent();
        }
        private void LoadShares(string Username, string Password, string Computer) {
            List<Share> shares = new List<Share>();
            System.Text.StringBuilder sb = new StringBuilder();
            string qry = "select * from win32_share";
            System.Management.ManagementObjectSearcher searcher;
            System.Management.ObjectQuery query = new System.Management.ObjectQuery(qry);

            if(Username != "" && Password != "" && Computer != "" && !Computer.StartsWith(@"\\localhost")) {
                System.Management.ConnectionOptions oConn = new System.Management.ConnectionOptions();
                oConn.Username = Username;
                oConn.Password = Password;
                if(!Computer.StartsWith(@"\\")) Computer = @"\\" + Computer;
                if(!Computer.ToLower().EndsWith(@"\root\cimv2")) Computer = Computer + @"\root\cimv2";
                System.Management.ManagementScope oMs = new System.Management.ManagementScope(Computer, oConn);

                searcher = new System.Management.ManagementObjectSearcher(oMs, query);
            } else {
                searcher = new System.Management.ManagementObjectSearcher(query);
            }

            foreach(System.Management.ManagementObject share in searcher.Get()) {
                Share s = new Share();
                foreach(System.Management.PropertyData p in share.Properties) {
                    switch(p.Name) {
                        case "AccessMask":
                            if(p.Value != null) s.AccessMask = p.Value.ToString();
                            break;
                        case "MaximumAllowed":
                            if(p.Value != null) s.MaximumAllowed = p.Value.ToString();
                            break;
                        case "InstallDate":
                            if(p.Value != null) s.InstallDate = p.Value.ToString();
                            break;
                        case "Description":
                            if(p.Value != null) s.Description = p.Value.ToString();
                            break;
                        case "Caption":
                            if(p.Value != null) s.Caption = p.Value.ToString();
                            break;
                        case "AllowMaximum":
                            if(p.Value != null) s.AllowMaximum = p.Value.ToString();
                            break;
                        case "Name":
                            if(p.Value != null) s.Name = p.Value.ToString();
                            break;
                        case "Path":
                            if(p.Value != null) s.Path = p.Value.ToString();
                            break;
                        case "Status":
                            if(p.Value != null) s.Status = p.Value.ToString();
                            break;
                        case "Type":
                            if(p.Value != null) s._Type = p.Value.ToString();
                            break;
                        default:
                            break;
                    }
                }
                shares.Add(s);
            }
            this.dataGridView1.DataSource = shares;
        }
        private void NetworkShares_Load(object sender, EventArgs e)
        {

            this.LoadShares("", "", "");

        }

        private void button1_Click(object sender, EventArgs e) {
            this.LoadShares(this.wmiServerCredentials1.Username, this.wmiServerCredentials1.Password, this.wmiServerCredentials1.SelectedServer);
        }

        private void wmiServerCredentials1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
    public class Share
    {
        private string accessMask;
        public string AccessMask
        {
            get { return accessMask; }
            set { accessMask = value; }
        }
        private string maximumAllowed;
        public string MaximumAllowed
        {
            get { return maximumAllowed; }
            set { maximumAllowed = value; }
        }
        private string installDate;
        public string InstallDate
        {
            get { return installDate; }
            set { installDate = value; }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string caption;
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        private string allowMaximum;
        public string AllowMaximum
        {
            get { return allowMaximum; }
            set { allowMaximum = value; }
        }


        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public string _Type;
        public string Type
        {
            get {
                switch (_Type)
                {
                    case "0":
                        return "Disk Drive";
                    case "1":
                        return "Print Queue";
                    case "2":
                        return "Device";
                    case "3":
                        return "IPC";
                    case "2147483648":
                        return "Disk Drive Admin";
                    case "2147483649":
                        return "Print Queue Admin";
                    case "2147483650":
                        return "Device Admin";
                    case "2147483651":
                        return "IPC Admin";
                    default:
                        return _Type;
                }                
            }
        }        
    }
}
