using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Terminals.Scanner
{    
    internal delegate void NetworkScanHandler(ScanItemEventArgs args);
    
    internal class NetworkScanManager
    {
        internal event NetworkScanHandler OnAddressScanHit;
        internal event NetworkScanHandler OnAddressScanFinished;

        private List<NetworkScanItem> scanItems = new List<NetworkScanItem>();

        /// <summary>
        /// Gets count of pending ip addresses to scan during last or actualy running scan.
        /// </summary>
        internal Int32 PendingAddressesToScan
        {
            get
            {
                return this.AllAddressesToScan - this.DoneAddressScans;
            }
        }

        /// <summary>
        /// Gets count of all ip addresses to scan during last or actualy running scan.
        /// </summary>
        internal Int32 AllAddressesToScan { get; private set; }

        private object doneItems = new object();
        private Int32 doneAddressScans;
        /// <summary>
        /// Gets or sets count of already finished ipaddress scans during last or actualy running scan.
        /// </summary>
        internal Int32 DoneAddressScans
        {
            get
            {
                lock (doneItems)
                    return doneAddressScans;
            }
            private set
            {
                lock (doneItems)
                    doneAddressScans = value;
            }
        }

        private Boolean scanIsRunning = false;
        internal bool ScanIsRunning
        {
            get
            {
                lock(doneItems)
                    return this.scanIsRunning;
            }
            set
            {
                lock (doneItems)
                    this.scanIsRunning = value;
            }
        }

        public override string ToString()
        {
            return String.Format("NetworkScanManager:{0}{1}/{2}", 
                this.ScanIsRunning, this.DoneAddressScans, this.AllAddressesToScan);
        }

        internal void StartScan(String A, String B, String C, String D, String E, List<Int32> portList)
        {
            Debug.WriteLine("Starting scan with previous state" + this.ScanIsRunning);
            if (this.ScanIsRunning)
                return;

            this.ScanIsRunning = true;
            this.DoneAddressScans = 0;
            PrepareItemsToScan(A, B, C, D, E, portList);
            this.QueueBackgroundScans();
        }

        private void PrepareItemsToScan(String A, String B, String C, String D, String E, List<Int32> portList)
        {
            String ipBody = String.Format("{0}.{1}.{2}.", A, B, C);
            Int32 start = 0;
            Int32 end = 0;
            Int32.TryParse(D, out start);
            Int32.TryParse(E, out end);

            this.scanItems.Clear();
            this.AllAddressesToScan = end - start + 1;

            for (Int32 ipSuffix = start; ipSuffix <= end; ipSuffix++)
            {
                if (!this.ScanIsRunning)
                    break;
                String ipAdddress = String.Format("{0}{1}", ipBody, ipSuffix);
                this.AddItemToScan(portList, ipAdddress);
            }
        }

        private void AddItemToScan(List<Int32> portList, String ipAdddress)
        {
            NetworkScanItem item = new NetworkScanItem(ipAdddress, portList);
            this.scanItems.Add(item);
        }

        internal void StopScan()
        {
            Debug.WriteLine("Canceling scan with previous state" + this.ScanIsRunning);
            foreach (NetworkScanItem scanItem in this.scanItems)
            {
                scanItem.Stop();
            }
            this.ScanIsRunning = false;
            Debug.WriteLine("Scan stoped.");
        }

        private void QueueBackgroundScans()
        {
            foreach (NetworkScanItem item in scanItems)
            {
                if (!this.ScanIsRunning)
                    break;
                QueueBackgroundScan(item);
            }
        }

        private void QueueBackgroundScan(NetworkScanItem item)
        {
            // dont use events, otherwise we have to unregister
            item.OnScanHit = new NetworkScanHandler(this.item_OnScanHit);
            item.OnScanFinished = new NetworkScanHandler(this.item_OnScanFinished);
            ThreadPool.QueueUserWorkItem(new WaitCallback(item.Scan));
        }

        private void item_OnScanFinished(ScanItemEventArgs args)
        {
            Debug.WriteLine(String.Format("Scan finished: {0}", args.ScanResult));

            this.DoneAddressScans++;

            if (this.OnAddressScanFinished != null && this.ScanIsRunning)
                this.OnAddressScanFinished(args);

            if (this.PendingAddressesToScan <= 0)
                this.StopScan();
        }

        private void item_OnScanHit(ScanItemEventArgs args)
        {
            if (this.OnAddressScanHit != null && this.ScanIsRunning)
                this.OnAddressScanHit(args);
        }
    }
}
