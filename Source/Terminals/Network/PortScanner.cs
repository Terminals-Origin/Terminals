using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Metro.Scanning;

namespace Terminals.Network
{
    internal partial class PortScanner : UserControl
    {
        readonly MethodInvoker updateConnections;
        List<TcpSynScanner> scanners;

        private int counter;

        private int portCount;

        IPAddress endPointAddress;

        private List<ScanResult> results;

        private readonly object resultsLock = new object();

        public PortScanner()
        {
            this.InitializeComponent();

            this.updateConnections = new MethodInvoker(this.UpdateConnections);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            scanners = new List<TcpSynScanner>();
            this.results = new List<ScanResult>();
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
                int portsCounter = 0;
                for (int y = iStartPort; y <= iEndPort; y++)
                {
                    ports[portsCounter] = (ushort)y;
                    portsCounter++;
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
                        IPAddress finalAddress;
                        if (IPAddress.TryParse(initial + x.ToString(), out finalAddress))
                        {
                            try
                            {
                                ThreadPool.QueueUserWorkItem(new WaitCallback(ScanMachine), new object[] { finalAddress, ports });
                            }
                            catch (Exception exc)
                            {
                                Logging.Log.Error("Threaded Scan Machine Call", exc);
                            }
                        }
                    }
                }
            }
        }

        private void ScanMachine(object state)
        {
            try
            {
                object[] states = (object[])state;
                IPAddress address = (IPAddress)states[0];
                ushort[] ports = (ushort[])states[1];

                TcpSynScanner scanner = new TcpSynScanner(new IPEndPoint(this.endPointAddress, 0));
                scanner.PortReply += new TcpPortReplyHandler(this.Scanner_PortReply);
                scanner.ScanComplete += new TcpPortScanComplete(this.Scanner_ScanComplete);
                scanners.Add(scanner);
                scanner.StartScan(address, ports, 1000, 100, true);
                this.counter = this.counter + ports.Length;
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
            if (!this.IsDisposed) this.Invoke(this.updateConnections);
            Application.DoEvents();

        }

        private void PortScanner_Load(object sender, EventArgs eargs)
        {
            Metro.NetworkInterfaceList interfaceList = new Metro.NetworkInterfaceList();
            try
            {
                foreach (Metro.NetworkInterface face in interfaceList.Interfaces)
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
            catch (Exception exc)
            {
                Logging.Log.Error("Connecting to the network interfaces", exc);
            }
        }

        private void Scanner_ScanComplete()
        {
            if (this.counter > 0) this.counter = this.counter - portCount;
            this.Invoke(this.updateConnections);
            this.StartButton.Enabled = true;
        }

        private void Scanner_PortReply(IPEndPoint remoteEndPoint, TcpPortState state)
        {
            this.counter--;
            ScanResult r = new ScanResult();
            r.RemoteEndPoint = new IPEndPoint(remoteEndPoint.Address, remoteEndPoint.Port);
            r.State = state;
            lock (resultsLock)
            {
                this.results.Add(r);
            }
            this.Invoke(this.updateConnections);
        }

        private void UpdateConnections()
        {
            this.resultsGridView.Rows.Clear();
            if (this.resultsGridView.Columns.Count == 0)
            {
                this.resultsGridView.Columns.Add("EndPoint", "End Point");
                this.resultsGridView.Columns.Add("State", "State");
            }
            lock (resultsLock)
            {
                foreach (ScanResult result in this.results)
                {
                    if (result.State == TcpPortState.Opened)
                    {
                        this.resultsGridView.Rows.Add(new object[] { result.RemoteEndPoint, result.State });
                    }
                }
            }
            if (this.counter <= 0)
            {
                this.StopButton.Enabled = false;
                this.StartButton.Enabled = true;
                this.counter = 0;
            }
            this.ScanResultsLabel.Text = string.Format("Outsanding Requests:{0}", this.counter);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            foreach (TcpSynScanner scanner in scanners)
            {
                if (scanner.Running) scanner.CancelScan();
            }
            this.Invoke(this.updateConnections);
        }

        private void A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                b.Focus();
            }
        }

        private void B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                c.Focus();
            }
        }

        private void C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                d.Focus();
            }
        }

        private void D_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                this.e.Focus();
            }
        }

        private void E_KeyUp(object sender, KeyEventArgs earg)
        {
            if (earg.KeyCode == Keys.Enter || earg.KeyCode == Keys.OemPeriod)
            {
                pa.Focus();
            }
        }

        private void Pa_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.OemPeriod)
            {
                pb.Focus();
            }
        }

        private void Pb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StartButton_Click(null, null);
            }
        }

        private void CopyRemoteAddressToolStripMenuItem_Click(object sender, EventArgs e)
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

    internal class ScanResult
    {
        public IPEndPoint RemoteEndPoint { get; set; }

        public TcpPortState State { get; set; }
    }
}