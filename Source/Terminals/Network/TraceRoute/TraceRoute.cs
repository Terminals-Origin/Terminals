using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Terminals.Network
{
    public class TraceRoute : Component
    {
        #region Fields

        private System.Net.NetworkInformation.Ping ping;
        private List<TraceRouteHopData> hopList;
        private IPAddress destinationIP;
        private PingOptions pingOptions;
        private Byte[] buffer;
        private Boolean cancel = false;
        private Byte counter;

        #endregion

        #region Events

        /// <summary>
        /// Occures when the route tracing is completed.
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// Occures when a hop is found in the tracing route.
        /// </summary>
        public event EventHandler<RouteHopFoundEventArgs> RouteHopFound;

        #endregion

        #region Constructors

        public TraceRoute()
        {
            // Assign default value for the component properties
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this))
            {
                DefaultValueAttribute myAttribute = (DefaultValueAttribute)property.Attributes[typeof(DefaultValueAttribute)];
                if (myAttribute != null)
                    property.SetValue(this, myAttribute.Value);
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the buffer data.
        /// </summary>
        [DefaultValue(32)]
        public Int32 BufferSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of hops to allow for a trace.
        /// </summary>
        [DefaultValue(128)]
        public Int32 HopLimit { get; set; }

        /// <summary>
        /// Gets the list of hops in the trace route.
        /// </summary>
        public List<TraceRouteHopData> Hops
        {
            get
            {
                lock (this.hopList)
                {
                    return this.hopList;
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of records in the Hop arrays.
        /// </summary>
        [DefaultValue(0)]
        public Int32 HopCount { get; set; }

        /// <summary>
        /// Gets or sets the address of a host to trace to.
        /// </summary>
        public String Destination { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of time to wait for an individual hop to complete in miliseconds.
        /// </summary>
        [DefaultValue(5000)]
        public Int32 HopTimeOut { get; set; }

        /// <summary>
        /// Indicates whether the route tracing is idle.
        /// </summary>
        public Boolean Idle { get; set; }

        /// <summary>
        /// Gets or sets wether whether the component resolves the host name for each host during the trace.
        /// </summary>
        [DefaultValue(true)]
        public Boolean ResolveNames { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts the route tracing process. The HostNameOrAddress field should already be set
        /// </summary>
        public void Start()
        {
            this.hopList = new List<TraceRouteHopData>();
            this.destinationIP = Dns.GetHostEntry(this.Destination).AddressList[0];

            if (IPAddress.IsLoopback(this.destinationIP))
            {
                this.ProcessHop(this.destinationIP, IPStatus.Success);
            }
            else
            {
                this.StartPing();
            }
        }

        public void Cancel()
        {
            this.cancel = true;
            this.OnCompleted();
        }

        #endregion

        #region Private methods

        private void StartPing()
        {
            this.counter = 1;
            this.cancel = false;

            this.ping = new System.Net.NetworkInformation.Ping();
            this.ping.PingCompleted += new PingCompletedEventHandler(this.OnPingCompleted);
            this.pingOptions = new PingOptions(1, true);
            this.buffer = Encoding.ASCII.GetBytes(new String('.', this.BufferSize));
            this.ping.SendAsync(this.destinationIP, this.HopTimeOut, this.buffer, this.pingOptions, null);
        }

        private void OnPingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (!this.cancel)
            {
                this.ProcessHop(e.Reply.Address, e.Reply.Status);

                if (e.Reply.Status != IPStatus.TimedOut)
                    this.pingOptions.Ttl++;

                if (!this.Idle)
                {
                    lock (this)
                    {
                        this.ping.SendAsync(this.destinationIP, this.HopTimeOut, this.buffer, this.pingOptions, null);
                    }
                }
            }
        }

        private void ProcessHop(IPAddress address, IPStatus status)
        {
            Int64 roundTripTime = 0;

            if (status == IPStatus.Success || status == IPStatus.TtlExpired)
            {
                System.Net.NetworkInformation.Ping ping2 = new System.Net.NetworkInformation.Ping();

                try
                {
                    // Do another ping to get the roundtrip time per address.
                    PingReply reply = ping2.Send(address, this.HopTimeOut);
                    roundTripTime = reply.RoundtripTime;
                    status = reply.Status;
                }
                catch (Exception ex)
                {
                     Terminals.Logging.Log.Info(String.Empty, ex);
                }
                finally
                {
                    ping2.Dispose();
                    ping2 = null;
                }
            }

            if (this.cancel)
                return;

            TraceRouteHopData hop = new TraceRouteHopData(counter++, address, roundTripTime, status);
            try
            {
                if (status == IPStatus.Success && this.ResolveNames)
                {
                    IPHostEntry entry = Dns.GetHostEntry(address);
                    hop.HostName = entry.HostName;
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                // No such host is known error.
                hop.HostName = String.Empty;
            }

            lock (this.hopList)
                this.hopList.Add(hop);

            if (RouteHopFound != null)
                RouteHopFound(this, new RouteHopFoundEventArgs(hop, this.Idle));

            this.Idle = address.Equals(this.destinationIP);

            lock (this.hopList)
            {
                if (!this.Idle && hopList.Count >= this.HopLimit - 1)
                    this.ProcessHop(this.destinationIP, IPStatus.Success);
            }

            if (this.Idle)
            {
                this.OnCompleted();
                this.Dispose();
            }
        }

        protected virtual void OnCompleted()
        {
            this.Completed(this, EventArgs.Empty);
        }

        protected override void Dispose(Boolean disposing)
        {
            try
            {
                lock (this)
                {
                    if (this.ping != null)
                    {
                        this.ping.Dispose();
                        this.ping = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
