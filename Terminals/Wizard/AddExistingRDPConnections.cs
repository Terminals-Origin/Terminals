using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Wizard
{
    public partial class AddExistingRDPConnections : UserControl
    {
        public AddExistingRDPConnections()
        {
            InitializeComponent();
            this.dataGridView1.Visible = false;
            miv = new MethodInvoker(this.UpdateConnections);
        }

        MethodInvoker miv;

        public FavoriteConfigurationElementCollection DiscoFavs = new FavoriteConfigurationElementCollection();
        private void AddExistingRDPConnections_Load(object sender, EventArgs e)
        {

        }
        public void StartImport()
        {
            try
            {
                //first look into the registry for existing rdp connetions
                //HKEY_CURRENT_USER\Software\Microsoft\Terminal Server Client\Default
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Terminal Server Client\Default");
                if(key != null)
                {
                    foreach(string name in key.GetValueNames())
                    {
                        string value = key.GetValue(name).ToString();
                        //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(AddFavorite), value);
                        AddFavorite(value);
                    }
                }
            }
            catch (Exception e) { Terminals.Logging.Log.Info("", e); }
            string f = "";
            //then kick up the port scan for the entire subnet

            try
            {
                foreach(Metro.NetworkInterface face in nil.Interfaces)
                {
                    if(face.IsEnabled && !face.isLoopback)
                    {
                        endPointAddress = face.Address;
                        break;
                    }
                }

                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ScanSubnet), null);
            }
            catch (Exception e) { Terminals.Logging.Log.Info("", e); }
        }
        Metro.NetworkInterfaceList nil = new Metro.NetworkInterfaceList();
        System.Net.IPAddress endPointAddress;
        private void ScanSubnet(object nullstate)
        {
            try
            {
                pendingRequests = 254 * 5;
                string ipAddress = endPointAddress.ToString();
                string start = ipAddress.Substring(0, ipAddress.LastIndexOf('.')) + ".";
                for(int x = 1; x < 255; x++)
                {
                    System.Net.IPAddress address = System.Net.IPAddress.Parse(start + x.ToString());
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ScanMachine), address);
                    
                }
            }
            catch (Exception e) { Terminals.Logging.Log.Info("", e); }
            this.Invoke(miv);
        }

        List<Metro.Scanning.TcpSynScanner> scannerList = new List<Metro.Scanning.TcpSynScanner>(1275);
        int scannerCount = 0;
        int pendingRequests = 0;
        private void ScanMachine(object machine)
        {
            try
            {
                Metro.Scanning.TcpSynScanner scanner;
                scanner = new Metro.Scanning.TcpSynScanner(new System.Net.IPEndPoint(endPointAddress, 0));
                scanner.PortReply += new Metro.Scanning.TcpPortReplyHandler(scanner_PortReply);
                scanner.ScanComplete += new Metro.Scanning.TcpPortScanComplete(scanner_ScanComplete);

                System.Net.IPAddress address = (System.Net.IPAddress)machine;
                scannerList.Add(scanner);
                scanner.StartScan(address, new ushort[] { 
                    Terminals.Connections.ConnectionManager.ICAPort, 
                    Terminals.Connections.ConnectionManager.RDPPort, 
                    Terminals.Connections.ConnectionManager.SSHPort, 
                    Terminals.Connections.ConnectionManager.TelnetPort, 
                    Terminals.Connections.ConnectionManager.VNCVMRCPort 
                } , 1000, 100, true);
                scannerCount++;
            }
            catch (Exception e) { 
                //its safe to ignore exceptions here as well  
                //Terminals.Logging.Log.Info("", e); 
                lock(uiElementsLock) {
                    pendingRequests = pendingRequests - 5;
                }
            }
            if(!this.IsDisposed) this.Invoke(miv);
            Application.DoEvents();

        }
        object uiElementsLock = new object();

        public delegate void DiscoveryCompleted();
        public event DiscoveryCompleted OnDiscoveryCompleted;

        public void CancelDiscovery()
        {
            try
            {
                if(scannerCount > 0)
                {
                    foreach(Metro.Scanning.TcpSynScanner scanner in scannerList)
                    {
                        if(scanner.Running) scanner.CancelScan();
                    }
                }
            }
            catch (Exception e) { Terminals.Logging.Log.Info("", e); }
        }
        void scanner_ScanComplete()
        {
            try
            {
                //pendingRequests = pendingRequests - 5;
                lock(uiElementsLock)
                {
                    scannerCount--;
                }
                this.Invoke(miv);
            }
            catch (Exception e) { Terminals.Logging.Log.Info("", e); }
        }

        void scanner_PortReply(System.Net.IPEndPoint remoteEndPoint, Metro.Scanning.TcpPortState state)
        {
            try
            {
                lock(uiElementsLock) pendingRequests--;
                if(state == Metro.Scanning.TcpPortState.Opened)
                {
                    string protocol = Terminals.Connections.ConnectionManager.GetPortName(remoteEndPoint.Port, true);
                    AddFavorite(remoteEndPoint.Address.ToString(), remoteEndPoint.Address.ToString() + "_" + protocol, remoteEndPoint.Port);
                }

                this.Invoke(miv);
            }
            catch (Exception e) { Terminals.Logging.Log.Info("", e); }
        }
        public void AddFavorite(object server) {
            string s = (string)server;
            AddFavorite(s, s, Terminals.Connections.ConnectionManager.RDPPort);
        }

        public void AddFavorite(string server)
        {
            AddFavorite(server, server, Terminals.Connections.ConnectionManager.RDPPort);
        }
        public void AddFavorite(string server, string name, int Port) {
           
            try {
                FavoriteConfigurationElement elm = new FavoriteConfigurationElement();

                try {
                    System.Net.IPAddress address;
                    if(System.Net.IPAddress.TryParse(server, out address)) {
                        name = System.Net.Dns.GetHostByAddress(address).HostName;
                    }
                    name = string.Format("{0}_{1}", name, Terminals.Connections.ConnectionManager.GetPortName(Port, true));
                } catch(Exception exc) {
                    //lets not log dns lookups!
                    //Terminals.Logging.Log.Info("", exc); 
                }

                elm.Name = name;
                elm.ServerName = server;
                elm.UserName = System.Environment.UserName;
                if(System.Environment.UserDomainName != System.Environment.MachineName) {
                    elm.DomainName = System.Environment.UserDomainName;
                } else {
                    elm.DomainName = server;
                }
                elm.Tags = "Discovered Connections";
                elm.Port = Port;
                elm.Protocol = Terminals.Connections.ConnectionManager.GetPortName(Port, true);
                lock(DiscoFavs) {
                    DiscoFavs.Add(elm);
                }
                //if(this.IsHandleCreated) this.Invoke(miv);
            } catch(Exception e) { Terminals.Logging.Log.Info("", e); }

        }
        private void UpdateConnections() {
            try {
                ConnectionsCountLabel.Text = DiscoFavs.Count.ToString();
                PendingRequestsLabel.Text = pendingRequests.ToString();

                if(pendingRequests <= 0) {
                    //this.progressBar1.Value = this.progressBar1.Maximum;
                    if(OnDiscoveryCompleted != null) {
                        OnDiscoveryCompleted();
                    }
                }

                Application.DoEvents();
            } catch(Exception e) { Terminals.Logging.Log.Info("", e); }
        }
        private void ConnectionsCountLabel_Click(object sender, EventArgs e)
        {
            try
            {

                //hidden egg to show the connections.  Just click on the connections count label to show and update the list
                List<BindingElement> list = new List<BindingElement>();
                foreach(FavoriteConfigurationElement elm in this.DiscoFavs)
                {
                    BindingElement be = new BindingElement();
                    be.Element = string.Format("{0}:{1}", elm.ServerName, elm.Protocol);
                    list.Add(be);
                }
                this.dataGridView1.DataSource = list;
                this.dataGridView1.Visible = true;
                Application.DoEvents();
            }
            catch (Exception exc) { Terminals.Logging.Log.Info("", exc); }
        }


    }
    public class BindingElement
    {
        private string Elm;

        public string Element
        {
            get { return Elm; }
            set { Elm = value; }
        }

    }
}