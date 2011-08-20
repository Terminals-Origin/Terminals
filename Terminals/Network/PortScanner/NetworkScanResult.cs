using System;
using Terminals.Connections;

namespace Terminals.Scanner
{
    internal class NetworkScanResult
    {
        public Boolean Import { get; set; }
        public String IPAddress { get; set; }
        public String HostName { get; set; }
        internal Int32 Port { get; set; }
        internal Boolean IsVMRC { get; set; }

        public String ServiceName
        {
            get
            {
                return ConnectionManager.GetPortName(this.Port, this.IsVMRC);
            }
        }

        internal NetworkScanResult()
        {
            this.Import = true;
        }

        public override String ToString()
        {
            return String.Format("NetworkScanResult:{0},{1},{2},{3}",
                this.IPAddress, this.ServiceName, this.HostName, this.Import);
        }
    }
}
