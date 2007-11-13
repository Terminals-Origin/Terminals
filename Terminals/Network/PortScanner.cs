using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network {
    public partial class PortScanner : UserControl {
        public PortScanner() {
            InitializeComponent();

            miv = new MethodInvoker(this.UpdateConnections);
        }

        MethodInvoker miv;
        List<Metro.Scanning.TcpSynScanner> scanners;
        private void StartButton_Click(object sender, EventArgs e) {
            scanners = new List<Metro.Scanning.TcpSynScanner>();
            Results = new List<ScanResult>();
            this.StartButton.Enabled = false;
            //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ScanSubnet), null);
            ScanSubnet();
        }
        private void ScanSubnet() {

            string startPort = pa.Text;
            string endPort = pb.Text;
            int iStartPort = 0;
            int iEndPort = 0;
            if(int.TryParse(startPort, out iStartPort) && int.TryParse(endPort, out iEndPort)) {

                if(iStartPort > iEndPort) {
                    int iPortTemp = iStartPort;
                    iStartPort = iEndPort;
                    iEndPort = iPortTemp;
                }
                ushort[] ports = new ushort[iEndPort - iStartPort+1];
                int counter = 0;
                for(int y = iStartPort;y <= iEndPort;y++) {
                    ports[counter] = (ushort)y;
                    counter++;
                }
                portCount = ports.Length;
                string initial = string.Format("{0}.{1}.{2}.", a.Text, b.Text, c.Text);
                string start = d.Text;
                string end = e.Text;
                int iStart = 0;
                int iEnd = 0;
                if(int.TryParse(start, out iStart) && int.TryParse(end, out iEnd)) {
                    if(iStart > iEnd) {
                        int iTemp = iStart;
                        iStart = iEnd;
                        iEnd = iTemp;
                    }
                    for(int x = iStart;x <= iEnd;x++) {
                        System.Net.IPAddress finalAddress;
                        if(System.Net.IPAddress.TryParse(initial + x.ToString(), out finalAddress)) {
                            try {
                                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ScanMachine), new object[] { finalAddress, ports});
                            } catch(Exception exc) {
                                exc.ToString();
                            }
                        }
                    }
                }
            }
        }
        private int Counter = 0;
        private int portCount = 0;
        private void ScanMachine(object state) {
            try {
                object[] states = (object[])state;
                System.Net.IPAddress address = (System.Net.IPAddress)states[0];
                ushort[] ports = (ushort[])states[1];

                Metro.Scanning.TcpSynScanner scanner;
                scanner = new Metro.Scanning.TcpSynScanner(new System.Net.IPEndPoint(endPointAddress, 0));
                scanner.PortReply += new Metro.Scanning.TcpPortReplyHandler(scanner_PortReply);
                scanner.ScanComplete += new Metro.Scanning.TcpPortScanComplete(scanner_ScanComplete);
                scanners.Add(scanner);
                scanner.StartScan(address, ports, 1000, 100, true);
                Counter = Counter + ports.Length;
            } catch(Exception e) {
                Terminals.Logging.Log.Info("Scanner caught an exception", e);
            }
            if(!this.IsDisposed) this.Invoke(miv);
            Application.DoEvents();

        }

        System.Net.IPAddress endPointAddress = null;
        private void PortScanner_Load(object sender, EventArgs e) {
            Metro.NetworkInterfaceList nil = new Metro.NetworkInterfaceList();
            try {
                foreach(Metro.NetworkInterface face in nil.Interfaces) {
                    if(face.IsEnabled && !face.isLoopback) {
                        endPointAddress = face.Address;
                        break;
                    }
                }

            } catch(Exception exc) { Terminals.Logging.Log.Info("", exc); }
        }

        void scanner_ScanComplete() {
            if(Counter>0) Counter = Counter - portCount;
            this.Invoke(miv);
            this.StartButton.Enabled = true;
        }
        public List<ScanResult> Results;
        void scanner_PortReply(System.Net.IPEndPoint remoteEndPoint, Metro.Scanning.TcpPortState state) {
            Counter--;
            ScanResult r = new ScanResult();
            r.RemoteEndPoint = remoteEndPoint;
            r.State = state;
            Results.Add(r);
            this.Invoke(miv);
        }

        private void UpdateConnections() {
            this.resultsGridView.DataSource = null;
            this.resultsGridView.DataSource = Results;
            this.ScanResultsLabel.Text = string.Format("Outsanding Requests:{0}", Counter);
        }

        private void StopButton_Click(object sender, EventArgs e) {
            foreach(Metro.Scanning.TcpSynScanner scanner in scanners) {
                if(scanner.Running) scanner.CancelScan();
            }
            this.Invoke(miv);
        }
    }
    public class ScanResult {
        System.Net.IPEndPoint remoteEndPoint;
        public System.Net.IPEndPoint RemoteEndPoint {
            get {
                return remoteEndPoint;
            }
            set {
                remoteEndPoint = value;
            }
        }
        Metro.Scanning.TcpPortState state;
        public Metro.Scanning.TcpPortState State {
            get {
                return state;
            }
            set {
                state = value;
            }
        }
    }
}