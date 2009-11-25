using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Management;
namespace Terminals.Network.WMI {
    public partial class Services : UserControl {
        public Services() {
            InitializeComponent();
        }


        private List<System.Management.ManagementObject> list = new List<ManagementObject>();
        private void LoadServices(string Username, string Password, string Computer) {

            System.Text.StringBuilder sb = new StringBuilder();
            string qry = "select * from win32_service";
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

            System.Data.DataTable dt = new DataTable();
            bool needsSchema = true;
            int length = 0;
            object[] values = null;
            list.Clear();
            foreach(System.Management.ManagementObject share in searcher.Get()) {
                Share s = new Share();
                list.Add(share);
                if(needsSchema) {
                    foreach(System.Management.PropertyData p in share.Properties) {
                        System.Data.DataColumn col = new DataColumn(p.Name, ConvertCimType(p.Type));
                        dt.Columns.Add(col);
                    }
                    length = share.Properties.Count;
                    needsSchema = false;
                }

                if(values == null) values = new object[length];
                int x = 0;
                foreach(System.Management.PropertyData p in share.Properties) {
                    if (p != null)
                    {
                        values[x] = p.Value;
                        x++;
                    }
                }
                dt.Rows.Add(values);
                values = null;
            }

            this.dataGridView1.DataSource = dt;
        }

        private void Services_Load(object sender, EventArgs e) {
            LoadServices("", "", "");
        }

        private void button1_Click(object sender, EventArgs e) {
            LoadServices(this.wmiServerCredentials1.Username, this.wmiServerCredentials1.Password, this.wmiServerCredentials1.SelectedServer);
        }

        private System.Type ConvertCimType(CimType ctValue) {
            System.Type tReturnVal = null;
            switch(ctValue) {
                case CimType.Boolean:
                    tReturnVal = typeof(System.Boolean);
                    break;
                case CimType.Char16:
                    tReturnVal = typeof(System.String);
                    break;
                case CimType.DateTime:
                    tReturnVal = typeof(System.DateTime);
                    break;
                case CimType.Object:
                    tReturnVal = typeof(System.Object);
                    break;
                case CimType.Real32:
                    tReturnVal = typeof(System.Decimal);
                    break;
                case CimType.Real64:
                    tReturnVal = typeof(System.Decimal);
                    break;
                case CimType.Reference:
                    tReturnVal = typeof(System.Object);
                    break;
                case CimType.SInt16:
                    tReturnVal = typeof(System.Int16);
                    break;
                case CimType.SInt32:
                    tReturnVal = typeof(System.Int32);
                    break;
                case CimType.SInt8:
                    tReturnVal = typeof(System.Int16);
                    break;
                case CimType.String:
                    tReturnVal = typeof(System.String);
                    break;
                case CimType.UInt16:
                    tReturnVal = typeof(System.UInt16);
                    break;
                case CimType.UInt32:
                    tReturnVal = typeof(System.UInt32);
                    break;
                case CimType.UInt64:
                    tReturnVal = typeof(System.UInt64);
                    break;
                case CimType.UInt8:
                    tReturnVal = typeof(System.UInt16);
                    break;
            }
            return tReturnVal;
        }

        private System.Management.ManagementObject FindWMIObject(string name, string propname) {
            System.Management.ManagementObject foundObj = null;
            foreach(System.Management.ManagementObject obj in list) {
                if(obj.Properties[propname].Value.ToString() == name) {
                    foundObj = obj;
                    break;
                }
            }
            return foundObj;

        }
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e) {
            string name = this.dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Cells["Name"].Value.ToString();
            if(name != null && name != "") {
                System.Management.ManagementObject obj = FindWMIObject(name, "Name");
                if(obj != null) {
                    obj.InvokeMethod("PauseService", null);
                    LoadServices(this.wmiServerCredentials1.Username, this.wmiServerCredentials1.Password, this.wmiServerCredentials1.SelectedServer);
                }
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e) {
            string name = this.dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Cells["Name"].Value.ToString();
            if(name != null && name != "") {
                System.Management.ManagementObject obj = FindWMIObject(name, "Name");
                if(obj != null) {
                    obj.InvokeMethod("StopService", null);
                    LoadServices(this.wmiServerCredentials1.Username, this.wmiServerCredentials1.Password, this.wmiServerCredentials1.SelectedServer);
                }

            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e) {
            string name = this.dataGridView1.Rows[this.dataGridView1.SelectedCells[0].RowIndex].Cells["Name"].Value.ToString();
            if(name != null && name != "") {
                System.Management.ManagementObject obj = FindWMIObject(name, "Name");
                if(obj != null) {
                    obj.InvokeMethod("StartService", null);
                    LoadServices(this.wmiServerCredentials1.Username, this.wmiServerCredentials1.Password, this.wmiServerCredentials1.SelectedServer);
                }

            }
        }

        private void wmiServerCredentials1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}