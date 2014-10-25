using System;
using System.Net.Sockets;
using System.Threading;
using Terminals.Data;

namespace Terminals.Connections
{
    /// <summary>
    /// Checks, if the connection port is available. Simulates reconnect feature of the RDP client.
    /// Doens use the port scanner, because it needs administrative priviledges.
    /// Is disposable because of used internal Timer.
    /// </summary>
    internal sealed class ConnectionStateDetector : IDisposable
    {
        /// <summary>
        /// Try reconnect max. 1 hour. Consider provide application configuration option for this value.
        /// </summary>
        private const int RECONNECT_MAX_DURATION = 1000 * 3600;

        /// <summary>
        /// Once per 20 seconds
        /// </summary>
        private const int TIMER_INTERVAL = 1000 * 20;

        private int retriesCount;
        private readonly Timer retriesTimer;
        private string serverName;
        private int port;

        private readonly object activityLock = new object();
        private bool disabled;
        private bool isRunning;

        private readonly Action<string, int> testAction;

        private readonly int reconnectMaxDuration;

        private readonly int timerInterval;

        internal bool IsRunning
        {
            get
            {
                lock (this.activityLock)
                {
                    return this.isRunning;
                }
            }
        }

        private bool CanTest
        {
            get
            {
                lock (this.activityLock)
                {
                    return this.isRunning && !this.disabled;
                }
            }
        }

        /// <summary>
        /// Connection to the favorite target service should be available again
        /// </summary>
        internal event EventHandler Reconnected;

        /// <summary>
        /// Detector stoped to try reconnect, because maximum amount of retries exceeded.
        /// </summary>
        internal event EventHandler ReconnectExpired;

        internal ConnectionStateDetector()
            : this(TestAction, RECONNECT_MAX_DURATION, TIMER_INTERVAL)
        {
        }

        internal ConnectionStateDetector(Action<string, int> testAction, int reconnectMaxDuration, int timerInterval)
        {
            if (timerInterval <= 0)
                throw new ArgumentOutOfRangeException("timerInterval", "Interval has to be non zero positive number.");

            this.testAction = testAction;
            this.reconnectMaxDuration = reconnectMaxDuration;
            this.timerInterval = timerInterval;
            this.retriesTimer = new Timer(TryReconnection);
        }

        private void TryReconnection(object state)
        {
            if (!this.CanTest)
                return;
            
            this.retriesCount++;
            bool success = this.TryReconnection();

            if (success)
            {
                this.ReportReconnected();
                return;
            }

            if (this.retriesCount > (reconnectMaxDuration / timerInterval))
                this.ReconnectionFail();
        }

        private bool TryReconnection()
        {
            try
            {
                // simulate reconnect, cant use port scanned, because it requires admin priviledges
                this.testAction(this.serverName, this.port);
                return true;
            }
            catch // exception is not necessary, simply is has to work
            {
                return false;
            }
        }

        private static void TestAction(string serverName, int port)
        {
            var portClient = new TcpClient(serverName, port);
        }

        private void ReconnectionFail()
        {
            if (this.ReconnectExpired != null)
                this.ReconnectExpired(this, EventArgs.Empty);
        }

        private void ReportReconnected()
        {
            if (this.Reconnected != null)
                this.Reconnected(this, EventArgs.Empty);
        }

        internal void AssignFavorite(IFavorite favorite)
        {
            this.serverName = favorite.ServerName;
            this.port = favorite.Port;
        }

        internal void Start()
        {
            lock (this.activityLock)
            {
                if (this.disabled)
                    return;

                this.isRunning = true;
                this.retriesCount = 0;
                this.retriesTimer.Change(0, timerInterval);
            }
        }

        internal void Stop()
        {
            lock (this.activityLock)
            {
                this.isRunning = false;
                this.retriesTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void Dispose()
        {
            this.Disable();
            this.retriesTimer.Dispose();
        }

        /// <summary>
        /// Fill space between disconnected request from GUI and real disconnect of the client.
        /// </summary>
        private void Disable()
        {
            lock (this.activityLock)
            {
                this.disabled = true;
            }
        }

        public override string ToString()
        {
            return string.Format("ConnectionStateDetector:IsRunning={0},Disabled={1}", this.isRunning, this.disabled);
        }
    }
}