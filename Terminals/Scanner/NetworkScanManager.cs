using System;
using System.Collections.Generic;
using System.Threading;

namespace Terminals.Scanner
{    
    internal delegate void NetworkScanHandler(ScanItemEventArgs args);
    
    internal class NetworkScanManager
    {
        private List<NetworkScanItem> openPorts;
        
        internal Int32 OpenPorts
        {
            get { return openPorts.Count; }
        }

        private bool scan = false;
        private List<NetworkScanItem> closedPorts;
        private List<NetworkScanItem> scanItems = new List<NetworkScanItem>();

        internal event NetworkScanHandler OnScanHit;
        internal event NetworkScanHandler OnScanMiss;
        internal event NetworkScanHandler OnScanStart;

        internal NetworkScanManager(String A, String B, String C, String D, String E, List<Int32> portList)
        {
            String ipBody = String.Format("{0}.{1}.{2}.", A, B, C);
            Int32 start = 0;
            Int32 end = 0;
            Int32.TryParse(D, out start);
            Int32.TryParse(E, out end);

            for (Int32 ipSuffix = start; ipSuffix <= end; ipSuffix++)
            {
                String ipAdddress = String.Format("{0}{1}", ipBody, ipSuffix);
                AddItemToScan(portList, ipAdddress);
            }
        }

        private void AddItemToScan(List<Int32> portList, String ipAdddress)
        {
            foreach (Int32 port in portList)
            {
                NetworkScanItem item = new NetworkScanItem();
                item.IPAddress = ipAdddress;
                item.Port = port;
                this.scanItems.Add(item);
            }
        }

        internal void StopScan()
        {
            scan = false;
        }

        internal void StartScan()
        {
            this.openPorts = new List<NetworkScanItem>();
            this.closedPorts = new List<NetworkScanItem>();
            this.scan = true;

            foreach (NetworkScanItem item in scanItems)
            {
                if (!this.scan)
                    break;
                QueueBackgroundScan(item);
            }
        }

        private void QueueBackgroundScan(NetworkScanItem item)
        {
            item.OnScanHit += new NetworkScanHandler(this.item_OnScanHit);
            item.OnScanMiss += new NetworkScanHandler(this.item_OnScanMiss);
            ScanItemEventArgs args = item.CreateNewEventArguments();
            
            if (this.OnScanStart != null)
                this.OnScanStart(args);

            ThreadPool.QueueUserWorkItem(new WaitCallback(item.Scan), null);
        }

        private void item_OnScanMiss(ScanItemEventArgs args)
        {
            this.closedPorts.Add(args.NetworkScanItem);
            if (OnScanMiss != null && scan)
                OnScanMiss(args);
        }

        private void item_OnScanHit(ScanItemEventArgs args)
        {
            this.openPorts.Add(args.NetworkScanItem);
            if (OnScanHit != null && scan)
                OnScanHit(args);
        }
    }
}
