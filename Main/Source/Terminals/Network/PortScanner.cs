using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Terminals.Network
{
    internal partial class PortScanner : UserControl
    {
        public PortScanner()
        {
            InitializeComponent();

            miv = new MethodInvoker(this.UpdateConnections);
        }

        MethodInvoker miv;
        List<Metro.Scanning.TcpSynScanner> scanners;
        private void StartButton_Click(object sender, EventArgs e)
        {
            scanners = new List<Metro.Scanning.TcpSynScanner>();
            Results = new List<ScanResult>();
            this.StartButton.Enabled = false;
            //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ScanSubnet), null);
            ScanSubnet();
        }
        private void ScanSubnet()
        {

            string startPort = pa.Text;
            string endPort = pb.Text;
            int iStartPort = 0;
            int iEndPort = 0;
            if (int.TryParse(startPort, out iStartPort) && int.TryParse(endPort, out iEndPort))
            {

                if (iStartPort > iEndPort)
                {
                    int iPortTemp = iStartPort;
                    iStartPort = iEndPort;
                    iEndPort = iPortTemp;
                }
                ushort[] ports = new ushort[iEndPort - iStartPort + 1];
                int counter = 0;
                for (int y = iStartPort; y <= iEndPort; y++)
                {
                    ports[counter] = (ushort)y;
                    counter++;
                }
                portCount = ports.Length;
                string initial = string.Format("{0}.{1}.{2}.", a.Text, b.Text, c.Text);
                string start = d.Text;
                string end = e.Text;
                int iStart = 0;
                int iEnd = 0;
                if (int.TryParse(start, out iStart) && int.TryParse(end, out iEnd))
                {
                    if (iStart > iEnd)
                    {
                        int iTemp = iStart;
                        iStart = iEnd;
                        iEnd = iTemp;
                    }
                    for (int x = iStart; x <= iEnd; x++)
                    {
                        System.Net.IPAddress finalAddress;
                        if (System.Net.IPAddress.TryParse(initial + x.ToString(), out finalAddress))
                        {
                            try
                            {
                                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ScanMachine), new object[] { finalAddress, ports });
                            }
                            catch (Exception exc)
                            {
                                Terminals.Logging.Log.Error("Threaded Scan Machine Call", exc);
                            }
                        }
                    }
                }
            }
        }
        private int Counter = 0;
        private int portCount = 0;
        private void ScanMachine(object state)
        {
            try
            {
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
            }
            catch (NotSupportedException) // thrown by constructor of packet sniffer
            {
                MessageBox.Show("Port scanner requires administrative priviledges to run!", "Terminals - port scanner",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception exception)
            {
                Logging.Log.Info("Scanner caught an exception", exception);
            }
            if (!this.IsDisposed) this.Invoke(miv);
            Application.DoEvents();

        }

        System.Net.IPAddress endPointAddress = null;
        private void PortScanner_Load(object sender, EventArgs eargs)
        {
            Metro.NetworkInterfaceList nil = new Metro.NetworkInterfaceList();
            try
            {
                foreach (Metro.NetworkInterface face in nil.Interfaces)
                {
                    if (face.IsEnabled && !face.isLoopback)
                    {
                        endPointAddress = face.Address;
                        string[] parts = endPointAddress.ToString().Split('.');
                        a.Text = parts[0];
                        b.Text = parts[1];
                        c.Text = parts[2];
                        d.Text = parts[3];
                        e.Text = parts[3];
                        break;
                    }
                }

            }
            catch (Exception exc) { Terminals.Logging.Log.Error("Connecting to the network interfaces", exc); }
        }

        void scanner_ScanComplete()
        {
            if (Counter > 0) Counter = Counter - portCount;
            this.Invoke(miv);
            this.StartButton.Enabled = true;
        }
        public List<ScanResult> Results;
        void scanner_PortReply(System.Net.IPEndPoint remoteEndPoint, Metro.Scanning.TcpPortState state)
        {
            Counter--;
            ScanResult r = new ScanResult();
            r.RemoteEndPoint = new System.Net.IPEndPoint(remoteEndPoint.Address, remoteEndPoint.Port);
            r.State = state;
            lock (resultsLock)
            {
                Results.Add(r);
            }
            this.Invoke(miv);
        }

        object resultsLock = new object();
        private void UpdateConnections()
        {
            this.resultsGridView.Rows.Clear();
            if (this.resultsGridView.Columns == null || this.resultsGridView.Columns.Count == 0)
            {
                this.resultsGridView.Columns.Add("EndPoint", "End Point");
                this.resultsGridView.Columns.Add("State", "State");
            }
            lock (resultsLock)
            {
                foreach (ScanResult result in Results)
                {
                    if (result.State == Metro.Scanning.TcpPortState.Opened)
                    {
                        this.resultsGridView.Rows.Add(new object[] { result.RemoteEndPoint, result.State });
                    }
                }
            }
            if (Counter <= 0)
            {
                this.StopButton.Enabled = false;
                this.StartButton.Enabled = true;
                Counter = 0;
            }
            this.ScanResultsLabel.Text = string.Format("Outsanding Requests:{0}", Counter);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            foreach (Metro.Scanning.TcpSynScanner scanner in scanners)
            {
                if (scanner.Running) scanner.CancelScan();
            }
            this.Invoke(miv);
        }

        private void a_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                b.Focus();
            }
        }

        private void b_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                c.Focus();
            }
        }

        private void c_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                d.Focus();
            }
        }

        private void d_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                this.e.Focus();
            }
        }

        private void e_KeyUp(object sender, KeyEventArgs earg)
        {
            if (earg.KeyCode == Keys.Enter || earg.KeyCode == Keys.OemPeriod)
            {
                pa.Focus();
            }
        }

        private void pa_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                pb.Focus();
            }
        }

        private void pb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StartButton_Click(null, null);
            }
        }

        private void copyRemoteAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.resultsGridView.SelectedCells.Count > 0 && this.resultsGridView.SelectedCells[0].RowIndex <= this.resultsGridView.Rows.Count)
            {
                string ip = this.resultsGridView.Rows[this.resultsGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                if (ip != null && ip.IndexOf(":") > 0)
                {
                    ip = ip.Substring(0, ip.IndexOf(":"));
                    Clipboard.SetText(ip, TextDataFormat.Text);
                }
            }
        }
    }
    public class ScanResult
    {
        System.Net.IPEndPoint remoteEndPoint;
        public System.Net.IPEndPoint RemoteEndPoint
        {
            get
            {
                return remoteEndPoint;
            }
            set
            {
                remoteEndPoint = value;
            }
        }
        Metro.Scanning.TcpPortState state;
        public Metro.Scanning.TcpPortState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
    }
}