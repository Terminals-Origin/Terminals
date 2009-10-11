using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Net;
using System.Windows.Forms;

using Metro;
using Metro.Scanning;
using Terminals.Connections;

namespace Terminals.Wizard
{
    public partial class AddExistingRDPConnections : UserControl
    {
        private MethodInvoker _miv;
        private FavoriteConfigurationElementCollection _discoFavs = new FavoriteConfigurationElementCollection();

        private NetworkInterfaceList _nil;
        private IPAddress _endPointAddress;
        private List<TcpSynScanner> _scannerList = new List<TcpSynScanner>(1275);
        private int _scannerCount = 0;
        private int _pendingRequests = 0;
        private object _uiElementsLock = new object();

        public delegate void DiscoveryCompleted();
        public event DiscoveryCompleted OnDiscoveryCompleted;

        public FavoriteConfigurationElementCollection DiscoFavs
        {
            get { return _discoFavs; }
            set { _discoFavs = value; }
        }
        public AddExistingRDPConnections()
        {
            InitializeComponent();
            this.dataGridView1.Visible = false;
            _miv = new MethodInvoker(this.UpdateConnections);
            try {
                _nil = new Metro.NetworkInterfaceList();
            } catch(Exception exc) {
                Terminals.Logging.Log.Error("Could not new up Metro.NetworkInterfaceList in AddExistingRDPConnections", exc);
            }
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
            catch (Exception e) 
            { 
                Terminals.Logging.Log.Info("StartImport", e);
            }
            
            //then kick up the port scan for the entire subnet
            if(_nil != null) {
                try {
                    foreach(Metro.NetworkInterface face in _nil.Interfaces) {
                        if(face.IsEnabled && !face.isLoopback) {
                            _endPointAddress = face.Address;
                            break;
                        }
                    }

                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ScanSubnet), null);
                } catch(Exception e) 
                { 
                    Terminals.Logging.Log.Info("", e); 
                }
            }
        }
        public void CancelDiscovery()
        {
            try
            {
                if(_scannerCount > 0)
                {
                    foreach(Metro.Scanning.TcpSynScanner scanner in _scannerList)
                    {
                        if(scanner.Running) 
                            scanner.CancelScan();
                    }
                }
            }
            catch (Exception e) 
            { 
                Terminals.Logging.Log.Info("", e); 
            }
        }
        public void AddFavorite(object server) 
        {
            AddFavorite((string)server);
        }
        public void AddFavorite(string server)
        {
            AddFavorite(server, server, ConnectionManager.RDPPort);
        }
        public void AddFavorite(string server, string name, int Port) 
        {           
            try {
                FavoriteConfigurationElement elm = new FavoriteConfigurationElement();
                try {
                    IPAddress address;
                    if(IPAddress.TryParse(server, out address)) 
                        name = Dns.GetHostEntry(address).HostName;

                    name = string.Format("{0}_{1}", name, ConnectionManager.GetPortName(Port, true));
                } catch(Exception exc) {
                    //lets not log dns lookups!
                    //Terminals.Logging.Log.Info("", exc); 
                }

                elm.Name = name;
                elm.ServerName = server;
                elm.UserName = Environment.UserName;
                
                if(Environment.UserDomainName != Environment.MachineName)
                    elm.DomainName = Environment.UserDomainName;
                else
                    elm.DomainName = server;
                
                elm.Tags = "Discovered Connections";
                elm.Port = Port;
                elm.Protocol = ConnectionManager.GetPortName(Port, true);
                lock(_discoFavs) {
                    _discoFavs.Add(elm);
                }
            } 
            catch(Exception e) 
            { 
                Terminals.Logging.Log.Info("", e); 
            }
        }

        private void Scanner_ScanComplete()
        {
            try
            {
                lock (_uiElementsLock)
                {
                    _scannerCount--;
                }
                this.Invoke(_miv);
            }
            catch (Exception e) 
            { 
                Terminals.Logging.Log.Info("", e); 
            }
        }
        private void AddExistingRDPConnections_Load(object sender, EventArgs e)
        {
        }
        private void ScanSubnet(object nullstate)
        {
            try
            {
                _pendingRequests = 254 * 5;
                string ipAddress = _endPointAddress.ToString();
                string start = ipAddress.Substring(0, ipAddress.LastIndexOf('.')) + ".";
                for (int x = 1; x < 255; x++)
                {
                    IPAddress address = System.Net.IPAddress.Parse(start + x.ToString());
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ScanMachine), address);
                }
            }
            catch (Exception e) 
            { 
                Terminals.Logging.Log.Info("", e); 
            }
            this.Invoke(_miv);
        }
        private void ScanMachine(object machine)
        {
            try
            {
                TcpSynScanner scanner;
                scanner = new TcpSynScanner(new IPEndPoint(_endPointAddress, 0));
                scanner.PortReply += new TcpPortReplyHandler(Scanner_PortReply);
                scanner.ScanComplete += new TcpPortScanComplete(Scanner_ScanComplete);

                IPAddress address = (IPAddress)machine;
                _scannerList.Add(scanner);
                scanner.StartScan(address, new ushort[] { ConnectionManager.ICAPort, ConnectionManager.RDPPort, ConnectionManager.SSHPort, 
                    ConnectionManager.TelnetPort, ConnectionManager.VNCVMRCPort }, 1000, 100, true);
                _scannerCount++;
            }
            catch (Exception e)
            {
                //its safe to ignore exceptions here as well  
                //Terminals.Logging.Log.Info("", e); 
                lock (_uiElementsLock)
                {
                    _pendingRequests = _pendingRequests - 5;
                }
            }
            if (!this.IsDisposed) 
                this.Invoke(_miv);
            Application.DoEvents();
        }
        private void Scanner_PortReply(IPEndPoint remoteEndPoint, TcpPortState state)
        {
            try
            {
                lock (_uiElementsLock) _pendingRequests--;
                if (state == TcpPortState.Opened)
                {
                    string protocol = ConnectionManager.GetPortName(remoteEndPoint.Port, true);
                    AddFavorite(remoteEndPoint.Address.ToString(), remoteEndPoint.Address.ToString() + "_" + protocol, remoteEndPoint.Port);
                }

                this.Invoke(_miv);
            }
            catch (Exception e) { Terminals.Logging.Log.Info("", e); }
        }
        private void UpdateConnections() {
            try {
                ConnectionsCountLabel.Text = _discoFavs.Count.ToString();
                PendingRequestsLabel.Text = _pendingRequests.ToString();

                if (_pendingRequests <= 0 && OnDiscoveryCompleted != null) 
                        OnDiscoveryCompleted();

                Application.DoEvents();
            } 
            catch(Exception e) 
            { 
                Terminals.Logging.Log.Info("", e); 
            }
        }
        private void ConnectionsCountLabel_Click(object sender, EventArgs e)
        {
            try
            {

                //hidden egg to show the connections.  Just click on the connections count label to show and update the list
                List<BindingElement> list = new List<BindingElement>();
                foreach(FavoriteConfigurationElement elm in this._discoFavs)
                {
                    BindingElement be = new BindingElement();
                    be.Element = string.Format("{0}:{1}", elm.ServerName, elm.Protocol);
                    list.Add(be);
                }
                this.dataGridView1.DataSource = list;
                this.dataGridView1.Visible = true;
                Application.DoEvents();
            }
            catch (Exception exc) 
            { 
                Terminals.Logging.Log.Info("", exc); 
            }
        }
    }
    public class BindingElement
    {
        private string _elm;
        public string Element
        {
            get { return _elm; }
            set { _elm = value; }
        }
    }
}