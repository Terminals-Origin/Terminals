using System;

namespace Terminals.Network
{
	/// <summary>
	/// Contains data for the Completed event of TraceRoute.
	/// </summary>
	public class RouteHopFoundEventArgs : EventArgs
	{
		public RouteHopFoundEventArgs(TraceRouteHopData hop, Boolean isLast)
		{
			this.Hop = hop;
			this.IsLastNode = isLast;
		}

		/// <summary>
		/// Gets or sets whether the value of the hop property is the last hop in the trace.
		/// </summary>
		public bool IsLastNode { get; set; }

		/// <summary>
		/// The hop encountered during the route tracing.
		/// </summary>
		public TraceRouteHopData Hop { get; set; }
	}
}

