using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Terminals.Network
{
    /// <summary>
    /// Contains data of a trace routing hop.
    /// </summary>
    public class TraceRouteHopData
    {
        /// <summary>
        /// Constructs a new object from the IPAddress of the node and the round trip time taken
        /// </summary>
        /// <param name="address">The IP address of the hop.</param>
        /// <param name="roundTripTime">The roundtriptime it takes to the hop.</param>
        /// <param name="status">The hop IP status.</param>
        public TraceRouteHopData(Byte count, IPAddress address, Int64 roundTripTime, IPStatus status)
        {
            this.Count = count;
            this.Address = address;
            this.RoundTripTime = roundTripTime;
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets the hop count.
        /// </summary>
        public Byte Count { get; set; }

        /// <summary>
        /// Gets or sets the IP address for the hop.
        /// </summary>
        public IPAddress Address { get; set; }
        
        /// <summary>
        /// Gets or sets the time taken to go to the hop and come back to the originating node in milliseconds.
        /// </summary>
        public Int64 RoundTripTime { get; set; }
        
        /// <summary>
        /// Gets or sets the IPStatus of request send to the hope.
        /// </summary>
        public IPStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the resolved hostname for the IP address of the hop.
        /// </summary>
        public String HostName { get; set; }
    }
}

