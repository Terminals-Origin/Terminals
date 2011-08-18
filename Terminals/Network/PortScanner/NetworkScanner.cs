using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Network;
using Terminals.Scanner;
using Unified;

namespace Terminals
{
    internal partial class NetworkScanner : Form
    {
        Int32 scanCount = 0;

        MethodInvoker updateScanMethodInvoker;
        MethodInvoker updateStatusMethodInvoker;

        NetworkScanManager manager = null;
        private NetworkScanItem selectedScanItem;
        private object updateListLock = new object();
        private object scanCountLock = new object();

        internal NetworkScanner()
        {
            InitializeComponent();

            this.updateScanMethodInvoker = new MethodInvoker(UpdateScanItemList);
            this.updateStatusMethodInvoker = new MethodInvoker(UpdateStatus);
  
            FillTextBoxesFromLocalIp();

            Server.OnClientConnection += new Server.ClientConnection(Server_OnClientConnection);
            Client.OnServerConnection += new Client.ServerConnection(Client_OnServerConnection);
        }

        private void FillTextBoxesFromLocalIp()
        {
            string localIP = TryGetLocalIP();
            string[] ipList = localIP.Split('.');
            this.ATextbox.Text = ipList[0];
            this.BTextbox.Text = ipList[1];
            this.CTextbox.Text = ipList[2];
            this.DTextbox.Text = "1";
            this.ETextbox.Text = "255";
            this.ServerAddressLabel.Text = localIP;
        }

        private static String TryGetLocalIP()
        {   
            string localIP = "127.0.0.1";
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface nic in nics)
                {
                    if (nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        localIP = nic.GetIPProperties().GatewayAddresses[0].Address.ToString();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Log.Error("Network Scanner Failed to Initialize", e);
            }
            return localIP;
        }

        private void AddPort(NetworkScanItem port)
        {
            Boolean add = true;
            foreach (NetworkScanItem item in this.OpenPorts)
            {
                if (item.IPAddress == port.IPAddress && item.Port == port.Port && item.IsVMRC == port.IsVMRC)
                {
                    add = false;
                    break;
                }
            }

            if (add)
                this.openPorts.Add(port);
        }

        List<NetworkScanItem> openPorts = new List<NetworkScanItem>();
        internal List<NetworkScanItem> OpenPorts
        {
            get { return openPorts; }
            set { openPorts = value; }
        }

        private void UpdateScanItemList()
        {
            lock (updateListLock)
            {
                this.ScanResultsListView.Items.Clear();
                foreach (NetworkScanItem item in this.OpenPorts)
                {
                    AddItemToResultsList(item);
                }
            }
            IncrementProgress();
        }

        private void AddItemToResultsList(NetworkScanItem item)
        {
            ListViewItem newItem = new ListViewItem(item.IPAddress);
            newItem.Tag = item;
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, item.HostName));
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, item.Port.ToString()));
            String port = ConnectionManager.GetPortName(item.Port, item.IsVMRC);
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, port));
            this.ScanResultsListView.Items.Add(newItem);
        }

        private void UpdateStatus()
        {
            IncrementProgress();
        }

        private void IncrementProgress()
        {
            scanProgressBar.Increment(1);
            ScanStatusLabel.Text = String.Format("Pending items:{0}", scanCount);
            if (scanProgressBar.Value >= scanProgressBar.Maximum) scanProgressBar.Value = 0;
            if (scanCount == 0)
            {
                this.ScanButton.Text = "Scan";
                ScanStatusLabel.Text = String.Format("Completed scan, found: {0} items.", manager.OpenPorts);
                scanProgressBar.Value = 0;
            }
            Application.DoEvents();
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            scanCount = 0;
            scanProgressBar.Value = 0;

            if (ScanButton.Text == "Scan")
            {
                ScanStatusLabel.Text = "Initiating Scan...";
                ScanButton.Text = "Stop";
                Application.DoEvents();
                StartNewScanManager();
            }
            else
            {
                ScanStatusLabel.Text = "Scan Stopped.";
                ScanButton.Text = "Scan";
                Application.DoEvents();

                if (manager != null)
                {
                    manager.StopScan();
                }
            }
        }

        private void StartNewScanManager()
        {
            List<Int32> ports = new List<Int32>();
            if (this.RDPCheckbox.Checked)
                ports.Add(ConnectionManager.RDPPort);
            if (this.VNCCheckbox.Checked || this.VMRCCheckbox.Checked)
                ports.Add(ConnectionManager.VNCVMRCPort);
            
            this.manager = new NetworkScanManager(this.ATextbox.Text, this.BTextbox.Text, this.CTextbox.Text,
                                                  this.DTextbox.Text, this.ETextbox.Text, ports);

            this.manager.OnScanHit += new NetworkScanHandler(this.manager_OnScanHit);
            this.manager.OnScanStart += new NetworkScanHandler(this.manager_OnScanStart);
            this.manager.OnScanMiss += new NetworkScanHandler(this.manager_OnScanMiss);
            this.manager.StartScan();
        }

        private void manager_OnScanMiss(ScanItemEventArgs args)
        {
            scanCount--;
            this.Invoke(this.updateStatusMethodInvoker);
        }

        private void manager_OnScanStart(ScanItemEventArgs args)
        {
            scanCount++;
            this.Invoke(this.updateStatusMethodInvoker);
        }

        private void manager_OnScanHit(ScanItemEventArgs args)
        {
            scanCount--;
            AddPort(args.NetworkScanItem);
            this.Invoke(this.updateScanMethodInvoker);
        }

        private void ScanResultsListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (ScanResultsListView.SelectedItems[0].Tag != null)
            {
                lock (updateListLock)
                {
                    selectedScanItem = (NetworkScanItem)ScanResultsListView.SelectedItems[0].Tag;
                }
            }
            else
            {
                selectedScanItem = null;
            }
        }

        private void AllCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.RDPCheckbox.Checked = AllCheckbox.Checked;
            this.VNCCheckbox.Checked = AllCheckbox.Checked;
            this.VMRCCheckbox.Checked = AllCheckbox.Checked;
            this.TelnetCheckbox.Checked = AllCheckbox.Checked;
            this.SSHCheckbox.Checked = AllCheckbox.Checked;
        }

        private void AddAllButton_Click(object sender, EventArgs e)
        {
            Int32 count = 0;
            foreach (ListViewItem item in this.ScanResultsListView.Items)
            {
                NetworkScanItem service = item.Tag as NetworkScanItem;
                this.ImportSelectedService(service);
                count++;
            }
            MessageBox.Show(String.Format("{0} items were added to your favorites.", count));
        }

        private void ImportSelectedService(NetworkScanItem service)
        {
            FavoriteConfigurationElement favorite = new FavoriteConfigurationElement();
            favorite.ServerName = service.IPAddress;
            favorite.Port = service.Port;
            favorite.Protocol = ConnectionManager.GetPortName(favorite.Port, service.IsVMRC);
            String tags = this.TagsTextbox.Text;
            tags = tags.Replace("Tags...", String.Empty).Trim();
            if (tags != String.Empty)
                favorite.Tags = tags;
            favorite.Name = String.Format("{0}_{1}", service.HostName, favorite.Protocol);
            favorite.DomainName = Environment.UserDomainName;
            favorite.UserName = Environment.UserName;
            Settings.AddFavorite(favorite, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Server.ServerOnline)
            {
                this.button1.Text = "Start Server";
                Server.Stop();
            }
            else
            {
                this.button1.Text = "Stop Server";
                Server.Start();
            }
            if (Server.ServerOnline)
            {
                this.ServerStatusLabel.Text = "Server is ONLINE";
            }
            else
            {
                this.ServerStatusLabel.Text = "Server is OFFLINE";
            }
        }

        private void Client_OnServerConnection(MemoryStream Response)
        {
            if (Response.Length == 0)
            {
                MessageBox.Show("The server has nothing to share with you.");
            }
            else
            {
                ArrayList favorites = (ArrayList)Serialize.DeSerializeBinary(Response);
                Int32 count = ImportSharedFavorites(favorites);
                MessageBox.Show(string.Format("Successfully imported {0} connections.", count));
            }
        }

        private static Int32 ImportSharedFavorites(ArrayList favorites)
        {
            Int32 count = 0;
                
            foreach (object item in favorites)
            {
                SharedFavorite favorite = item as SharedFavorite;
                if (favorite != null)
                {
                    ImportSharedFavorite(favorite);
                    count++; 
                }
            }
            return count;
        }

        private static void ImportSharedFavorite(SharedFavorite favorite)
        {
            FavoriteConfigurationElement newfav = SharedFavorite.ConvertFromFavorite(favorite);
            newfav.Name = newfav.Name + "_new";
            newfav.UserName = Environment.UserName;
            newfav.DomainName = Environment.UserDomainName;
            Settings.AddFavorite(newfav, false);
        }

        private void Server_OnClientConnection(String Username, Socket Socket)
        {
            ArrayList list = FavoritesToSharedList();
            Byte[] data = SharedListToBinaryData(list);
            Socket.Send(data);
            Server.FinishDisconnect(Socket);
        }

        private static Byte[] SharedListToBinaryData(ArrayList list)
        {
            MemoryStream favs = Serialize.SerializeBinary(list);

            if (favs != null && favs.Length > 0)
            {
                if (favs.CanRead && favs.Position > 0) 
                    favs.Position = 0;
                Byte[] data = favs.ToArray();
                favs.Close();
                favs.Dispose();
                return data;
            }

            return new byte[0];
        }

        private static ArrayList FavoritesToSharedList()
        {
            FavoriteConfigurationElementCollection favorites = Settings.GetFavorites();
            ArrayList list = new ArrayList();
            foreach (FavoriteConfigurationElement elem in favorites)
            {
                list.Add(SharedFavorite.ConvertFromFavorite(elem));
            }
            return list;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Client.Start(this.ServerAddressTextbox.Text);
        }

        private void NetworkScanner_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Server.Stop();
                Client.Stop();
            }
            catch (Exception exc) { Logging.Log.Info("Network Scanner failed to stop server and client at close", exc); }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
