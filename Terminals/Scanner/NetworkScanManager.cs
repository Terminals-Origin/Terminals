using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Scanner {
    public class NetworkScanManager {

        private System.Collections.Generic.List<NetworkScanItem> openPorts;

        public System.Collections.Generic.List<NetworkScanItem> OpenPorts {
            get { return openPorts; }
            set { openPorts = value; }
        }
        private System.Collections.Generic.List<NetworkScanItem> closedPorts;

        public System.Collections.Generic.List<NetworkScanItem> ClosedPorts {
            get { return closedPorts; }
            set { closedPorts = value; }
        }
	

        System.Collections.Generic.List<NetworkScanItem> scanItems = new List<NetworkScanItem>();

        public delegate void ScanHitHandler(ScanItemEventArgs args);
        public event ScanHitHandler OnScanHit;
        public delegate void ScanMissHandler(ScanItemEventArgs args);
        public event ScanMissHandler OnScanMiss;

        public delegate void ScanStartHandler(ScanItemEventArgs args);
        public event ScanStartHandler OnScanStart;

        public NetworkScanManager(string A, string B, string C, string D, string E, List<int> PortList) {
            string startIP = string.Format("{0}.{1}.{2}.", A, B, C);
            int start = 0;
            int end = 0;
            int.TryParse(D, out start);
            int.TryParse(E, out end);
            for (int x = start; x <= end; x++) {
                foreach (int port in PortList) {
                    NetworkScanItem item = new NetworkScanItem();
                    item.IPAddress = string.Format("{0}{1}", startIP, x);
                    item.Port = port;
                    scanItems.Add(item);
                }
            }
        }
        private bool scan=false;

        public bool Scan {
            get { return scan; }
            set { scan = value; }
        }
        public void StopScan() {
            scan = false;
        }
        public void StartScan() {
            openPorts = new List<NetworkScanItem>();
            closedPorts = new List<NetworkScanItem>();
            Scan = true;
            foreach (NetworkScanItem item in scanItems) {
                if (!Scan) break;
                item.OnScanHit += new NetworkScanItem.ScanHitHandler(item_OnScanHit);
                item.OnScanMiss += new NetworkScanItem.ScanMissHandler(item_OnScanMiss);
                ScanItemEventArgs args = new ScanItemEventArgs();
                args.NetworkScanItem = item;
                if (OnScanStart != null) OnScanStart(args);
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(item.Scan), null);
            }
        }

        void item_OnScanMiss(ScanItemEventArgs args) {
            ClosedPorts.Add(args.NetworkScanItem);
            if (OnScanMiss != null && scan) OnScanMiss(args);
            
        }

        void item_OnScanHit(ScanItemEventArgs args) {
            OpenPorts.Add(args.NetworkScanItem);
            if (OnScanHit != null && scan) OnScanHit(args);
        }
    }
}
