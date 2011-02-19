using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace Terminals {
    public partial class NetworkScanner : Form {
        public NetworkScanner() {
            InitializeComponent();
            miv = new MethodInvoker(UpdateScanItemList);
            mivStatus = new MethodInvoker(UpdateStatus);
            string localIP = "127.0.0.1";
            try {
                NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                foreach(NetworkInterface nic in nics)
                {
                    if(nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        localIP = nic.GetIPProperties().GatewayAddresses[0].Address.ToString();
                        break;
                    }
                }
            }
            catch (Exception e) { Terminals.Logging.Log.Error("Network Scanner Failed to Initialize", e); }
            string[] ipList = localIP.Split('.');
            ATextbox.Text = ipList[0];
            BTextbox.Text = ipList[1];
            CTextbox.Text = ipList[2];
            DTextbox.Text = "1";
            ETextbox.Text = "255";
            ServerAddressLabel.Text = localIP;
            Network.Server.OnClientConnection += new Terminals.Network.Server.ClientConnection(Server_OnClientConnection);
            Network.Client.OnServerConnection += new Terminals.Network.Client.ServerConnection(Client_OnServerConnection);
        }




        int scanCount = 0;

        System.Windows.Forms.MethodInvoker miv;
        System.Windows.Forms.MethodInvoker mivStatus;

        Scanner.NetworkScanManager manager = null;
        private object updateListLock = new object();
        private object scanCountLock = new object();

        private void AddPort(Terminals.Scanner.NetworkScanItem port) {
            bool add = true;
            foreach (Scanner.NetworkScanItem item in this.OpenPorts) {
                if (item.IPAddress == port.IPAddress && item.Port == port.Port && item.IsVMRC == port.IsVMRC) {
                    add = false;
                    break;
                }
            }
            if (add) this.openPorts.Add(port);
        }

        System.Collections.Generic.List<Terminals.Scanner.NetworkScanItem> openPorts = new List<Terminals.Scanner.NetworkScanItem>();
        public System.Collections.Generic.List<Terminals.Scanner.NetworkScanItem> OpenPorts {
            get { return openPorts; }
            set { openPorts = value; }
        }

        private void UpdateScanItemList() {
            lock (updateListLock) {
                this.ScanResultsListView.Items.Clear();
                foreach (Scanner.NetworkScanItem item in this.OpenPorts) {
                    ListViewItem lvi = new ListViewItem(item.IPAddress);
                    lvi.Tag = item;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, item.HostName));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, item.Port.ToString()));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, Connections.ConnectionManager.GetPortName(item.Port, item.IsVMRC)));
                    this.ScanResultsListView.Items.Add(lvi);
                }
            }
            IncrementProgress();
        }
        private void UpdateStatus() {
            IncrementProgress();
        }
        private void IncrementProgress() {
            
            scanProgressBar.Increment(1);
            ScanStatusLabel.Text = string.Format("Pending items:{0}", scanCount);
            if (scanProgressBar.Value >= scanProgressBar.Maximum) scanProgressBar.Value = 0;
            if (scanCount == 0) {
                this.ScanButton.Text = "Scan";
                ScanStatusLabel.Text = string.Format("Completed scan, found: {0} items.", manager.OpenPorts.Count);
                scanProgressBar.Value = 0;
                //if(manager.OpenPorts.Count>0) this.AddAllButton.Enabled = true;
            }
            Application.DoEvents();
        }
        private void ScanButton_Click(object sender, EventArgs e) {
            scanCount = 0;
            scanProgressBar.Value = 0;
            //this.ScanResultsListView.Items.Clear();
            if (ScanButton.Text == "Scan") {
                ScanStatusLabel.Text = "Initiating Scan...";
                ScanButton.Text = "Stop";
                Application.DoEvents();
                List<int> ports = new List<int>();
                if (this.RDPCheckbox.Checked) ports.Add(Connections.ConnectionManager.RDPPort);
                if (this.VNCCheckbox.Checked || this.VMRCCheckbox.Checked) ports.Add(Connections.ConnectionManager.VNCVMRCPort);
                manager = new Terminals.Scanner.NetworkScanManager(ATextbox.Text, BTextbox.Text, CTextbox.Text, DTextbox.Text, ETextbox.Text, ports);
                manager.OnScanHit += new Terminals.Scanner.NetworkScanManager.ScanHitHandler(manager_OnScanHit);
                manager.OnScanStart += new Terminals.Scanner.NetworkScanManager.ScanStartHandler(manager_OnScanStart);
                manager.OnScanMiss += new Terminals.Scanner.NetworkScanManager.ScanMissHandler(manager_OnScanMiss);
                manager.StartScan();
            } else {
                ScanStatusLabel.Text = "Scan Stopped.";
                ScanButton.Text = "Scan";
                Application.DoEvents();
                if (manager != null) {
                    manager.StopScan();
                }
            }

        }
        void manager_OnScanMiss(Terminals.Scanner.ScanItemEventArgs args) {
            scanCount--;
            this.Invoke(mivStatus);
        }

        void manager_OnScanStart(Terminals.Scanner.ScanItemEventArgs args) {
            scanCount++;
            this.Invoke(mivStatus);
        }


        void manager_OnScanHit(Terminals.Scanner.ScanItemEventArgs args) {
            scanCount--;
            AddPort(args.NetworkScanItem);
            this.Invoke(miv);

        }

        private void ScanResultsListView_MouseClick(object sender, MouseEventArgs e) {
            if (ScanResultsListView.SelectedItems[0].Tag != null) {
                lock (updateListLock) {
                    selecctedScanItem = (Terminals.Scanner.NetworkScanItem)ScanResultsListView.SelectedItems[0].Tag;
                }
            } else {
                selecctedScanItem = null;
            }
        }
        private Terminals.Scanner.NetworkScanItem selecctedScanItem;

        public Terminals.Scanner.NetworkScanItem SelectedScanItem {
            get { return selecctedScanItem; }
            set { selecctedScanItem = value; }
        }

        private void AllCheckbox_CheckedChanged(object sender, EventArgs e) {
            this.RDPCheckbox.Checked = AllCheckbox.Checked;
            this.VNCCheckbox.Checked = AllCheckbox.Checked;
            this.VMRCCheckbox.Checked = AllCheckbox.Checked;
            this.TelnetCheckbox.Checked = AllCheckbox.Checked;
            this.SSHCheckbox.Checked = AllCheckbox.Checked;
        }

        private void AddAllButton_Click(object sender, EventArgs e) {
            int count = 0;
            foreach (ListViewItem lvi in this.ScanResultsListView.Items) {
                Terminals.Scanner.NetworkScanItem item = (Terminals.Scanner.NetworkScanItem)lvi.Tag;
                FavoriteConfigurationElement fav = new FavoriteConfigurationElement();
                fav.ServerName = item.IPAddress;
                fav.Port = item.Port;
                fav.Protocol = Connections.ConnectionManager.GetPortName(fav.Port, item.IsVMRC);
                string tags = TagsTextbox.Text;
                tags = tags.Replace("Tags...","").Trim();
                if (tags != string.Empty)  fav.Tags = tags;
                fav.Name = string.Format("{0}_{1}", item.HostName, fav.Protocol);
                fav.DomainName = System.Environment.UserDomainName;
                fav.UserName = System.Environment.UserName;
                Settings.AddFavorite(fav, false);
                count++;
            }
            MessageBox.Show(string.Format("{0} items were added to your favorites.", count));
        }

        private void button1_Click(object sender, EventArgs e) {
            if (Network.Server.ServerOnline) {
                this.button1.Text = "Start Server";
                Network.Server.Stop();
            } else {
                this.button1.Text = "Stop Server";
                Network.Server.Start();
            }
            if (Network.Server.ServerOnline) {
                this.ServerStatusLabel.Text = "Server is ONLINE";
            } else {
                this.ServerStatusLabel.Text = "Server is OFFLINE";
            }
            
        }

        void Client_OnServerConnection(System.IO.MemoryStream Response) {

            if(Response.Length==0) {
                System.Windows.Forms.MessageBox.Show("The server has nothing to share with you.");
            } else {
                int count = 0;
                System.Collections.ArrayList favs = (System.Collections.ArrayList)Unified.Serialize.DeSerializeBinary(Response);
                foreach (object fav in favs) {
                    FavoriteConfigurationElement newfav = Network.SharedFavorite.ConvertFromFavorite((Network.SharedFavorite)fav);
                    newfav.Name = newfav.Name + "_new";
                    newfav.UserName = System.Environment.UserName;
                    newfav.DomainName = System.Environment.UserDomainName;
                    Settings.AddFavorite(newfav, false);
                    count++;
                }

                System.Windows.Forms.MessageBox.Show(string.Format("Successfully imported {0} connections.", count));
            }
        }

        void Server_OnClientConnection(string Username, System.Net.Sockets.Socket Socket) {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (FavoriteConfigurationElement elem in favorites) {
                list.Add(Network.SharedFavorite.ConvertFromFavorite(elem));
            }

            System.IO.MemoryStream favs = Unified.Serialize.SerializeBinary(list);
            byte[] data = null;
            if (favs != null &&  favs.Length > 0) {
                if (favs.CanRead && favs.Position > 0) favs.Position = 0;
                data = favs.ToArray();
                favs.Close();
                favs.Dispose();
            } else {
                data = new byte[0];
            }
            Socket.Send(data);
            Network.Server.FinishDisconnect(Socket);

        }

        private void button2_Click(object sender, EventArgs e) {
            Network.Client.Start(this.ServerAddressTextbox.Text);
        }

        private void NetworkScanner_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                Network.Server.Stop();
                Network.Client.Stop();
            }
            catch (Exception exc) { Terminals.Logging.Log.Info("Network Scanner failed to stop server and client at close", exc); }
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	
    }
}
