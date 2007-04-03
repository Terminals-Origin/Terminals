using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Scanner {
    public class ScanItemEventArgs : EventArgs {

        private DateTime dateTime;

        public DateTime DateTime {
            get { return dateTime; }
            set { dateTime = value; }
        }


        private NetworkScanItem networkScanItem;
        public NetworkScanItem NetworkScanItem {
            get { return networkScanItem; }
            set { networkScanItem = value; }
        }

    }
}