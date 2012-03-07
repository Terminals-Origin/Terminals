using System;
using System.Net;

namespace Terminals.Network
{
    /// <summary>
    /// Represents data from ping reply.
    /// </summary>
    public class PingReplyData
    {
        public PingReplyData(Int64 count, String status, String hostname, String destination, Int32 bytes, Int32 ttl, Int64 roundTripTime)
        {
            this.Count = count;
            this.Status = status;
            this.Hostname = hostname;
            this.Destination = destination;
            this.Bytes = bytes;
            this.TimeToLive = ttl;
            this.RoundTripTime = roundTripTime;
        }

        public Int64 Count { get; set; }
        public String Status { get; set; }
        public String Hostname { get; set; }
        public String Destination { get; set; }
        public Int32 Bytes { get; set; }
        public Int32 TimeToLive { get; set; }
        public Int64 RoundTripTime { get; set; }
    }
}
