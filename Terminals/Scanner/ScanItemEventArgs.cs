using System;

namespace Terminals.Scanner
{
    internal class ScanItemEventArgs : EventArgs
    {
        public DateTime DateTime { get; set; }
        public NetworkScanItem NetworkScanItem { get; set; }
    }
}