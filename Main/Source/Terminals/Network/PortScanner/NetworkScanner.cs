using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Forms.Controls;
using Terminals.Network;
using Terminals.Scanner;

namespace Terminals
{
    internal partial class NetworkScanner : Form
    {
        private readonly IPersistence persistence;

        private NetworkScanManager manager;
        private bool validation;

        private readonly Server server;

        internal NetworkScanner(IPersistence persistence)
        {
            InitializeComponent();

            this.checkListPorts.DataSource = ConnectionManager.GetAvailableProtocols();
            this.CheckAllPorts();
            this.persistence = persistence;
            this.server = new Server(persistence);
            FillTextBoxesFromLocalIp();
            InitScanManager();
            this.gridScanResults.AutoGenerateColumns = false;
            Client.OnServerConnection += new ServerConnectionHandler(Client_OnServerConnection);
            this.bsScanResults.DataSource = new SortableList<NetworkScanResult>();
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
                Logging.Error("Network Scanner Failed to Initialize", e);
            }
            return localIP;
        }

        private void InitScanManager()
        {
            this.manager = new NetworkScanManager();
            this.manager.OnAddressScanHit += new NetworkScanHandler(this.manager_OnScanHit);
            this.manager.OnAddressScanFinished += new NetworkScanHandler(this.manager_OnAddresScanFinished);
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            scanProgressBar.Value = 0;

            if (ScanButton.Text == "&Scan")
            {
                this.StartScan();
            }
            else
            {
                StopScan();
            }
        }

        private void StopScan()
        {
            this.manager.StopScan();
            this.ScanStatusLabel.Text = "Scan Stopped.";
            this.ScanButton.Text = "&Scan";
        }

        private void StartScan()
        {
            this.bsScanResults.Clear();
            ScanStatusLabel.Text = "Initiating Scan...";
            ScanButton.Text = "Stop";
            List<int> ports = GetSelectedPorts();
            this.manager.StartScan(this.ATextbox.Text, this.BTextbox.Text, this.CTextbox.Text,
                                   this.DTextbox.Text, this.ETextbox.Text, ports);
        }

        private List<int> GetSelectedPorts()
        {
            return checkListPorts.CheckedItems.OfType<string>()
                .Select(ConnectionManager.GetPort)
                .ToList();
        }

        private void manager_OnAddresScanFinished(ScanItemEventArgs args)
        {
            this.Invoke(new MethodInvoker(UpdateScanStatus));
        }

        /// <summary>
        /// Updates the status bar, button state and progress bar.
        /// The last who sends "is done" autoamticaly informs about the compleated state.
        /// </summary>
        private void UpdateScanStatus()
        {
            this.scanProgressBar.Maximum = this.manager.AllAddressesToScan;
            scanProgressBar.Value = this.manager.DoneAddressScans;
            Int32 pendingAddresses = this.manager.AllAddressesToScan - scanProgressBar.Value;
            Debug.WriteLine("updating status with pending ({0}): {1}", 
                            this.manager.ScanIsRunning, pendingAddresses);


            ScanStatusLabel.Text = String.Format("Pending items:{0}", pendingAddresses);
            if (scanProgressBar.Value >= scanProgressBar.Maximum)
                scanProgressBar.Value = 0;

            if (pendingAddresses == 0)
            {
                this.ScanButton.Text = "&Scan";
                ScanStatusLabel.Text = String.Format("Completed scan, found: {0} items.", this.bsScanResults.Count);
                scanProgressBar.Value = 0;
            }
        }

        private void manager_OnScanHit(ScanItemEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NetworkScanHandler(manager_OnScanHit), new object[] { args });
            }
            else
            {
                this.bsScanResults.Add(args.ScanResult);
                this.gridScanResults.Refresh();
            }
        }

        private void CheckListPorts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Unchecked)
                this.checkBoxAll.Checked = false;
        }

        private void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxAll.Checked)
                this.CheckAllPorts();
        }

        private void CheckAllPorts()
        {
            for (int index = 0; index < this.checkListPorts.Items.Count; index++)
            {
                this.checkListPorts.SetItemChecked(index, true);
            }
        }

        private void AddAllButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            String tags = GetTagsToApply();
            List<FavoriteConfigurationElement> favoritesToImport = GetFavoritesFromBindingSource(tags);
            ImportSelectedItems(favoritesToImport);
        }

        private List<FavoriteConfigurationElement> GetFavoritesFromBindingSource(String tags)
        {
            List<FavoriteConfigurationElement> favoritesToImport = new List<FavoriteConfigurationElement>();
            foreach (DataGridViewRow scanResultRow in this.gridScanResults.SelectedRows)
            {
                    var computer = scanResultRow.DataBoundItem as NetworkScanResult;
                    var favorite = computer.ToFavorite(tags);
                    favoritesToImport.Add(favorite);
            }
            return favoritesToImport;
        }

        private string GetTagsToApply()
        {
            String tags = this.TagsTextbox.Text;
            tags = tags.Replace("Groups...", String.Empty).Trim();
            return tags;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (this.server.ServerOnline)
            {
                this.button1.Text = "Start Server";
                this.server.Stop();
            }
            else
            {
                this.button1.Text = "Stop Server";
                this.server.Start();
            }
            if (this.server.ServerOnline)
            {
                this.ServerStatusLabel.Text = "Server is ONLINE";
            }
            else
            {
                this.ServerStatusLabel.Text = "Server is OFFLINE";
            }
        }

        private void ImportSelectedItems(List<FavoriteConfigurationElement> favoritesToImport)
        {
            var managedImport = new ImportWithDialogs(this, this.persistence);
            managedImport.Import(favoritesToImport);
        }

        private void Client_OnServerConnection(ShareFavoritesEventArgs args)
        {
            ImportSelectedItems(args.Favorites);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Client.Start(this.ServerAddressTextbox.Text);
        }

        private void NetworkScanner_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Client.OnServerConnection -= new ServerConnectionHandler(Client_OnServerConnection);
                this.manager.StopScan();
                this.server.Stop();
                Client.Stop();
            }
            catch (Exception exc)
            {
                Logging.Info("Network Scanner failed to stop server and client at close", exc);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Validate text boxes to allow inser only byte.
        /// </summary>
        private void IPTextbox_TextChanged(object sender, EventArgs e)
        {
            if (validation)
                return; // prevent stack overflow

            byte testValue;
            validation = true;
            TextBox textBox = sender as TextBox;
            bool isValid = Byte.TryParse(textBox.Text, NumberStyles.None, null,  out testValue);

            if (!isValid && validation)
                textBox.Text = textBox.Tag.ToString();
            else
                textBox.Tag = textBox.Text;

            validation = false;
        }

        private void GridScanResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn lastSortedColumn = this.gridScanResults.FindLastSortedColumn();
            DataGridViewColumn column = this.gridScanResults.Columns[e.ColumnIndex];

            SortOrder newSortDirection = SortableUnboundGrid.GetNewSortDirection(lastSortedColumn, column);
            var data = this.bsScanResults.DataSource as SortableList<NetworkScanResult>;
            this.bsScanResults.DataSource = data.SortByProperty(column.DataPropertyName, newSortDirection);
            column.HeaderCell.SortGlyphDirection = newSortDirection;
        }
    }
}
