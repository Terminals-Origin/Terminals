using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using Terminals.Configuration;
using Terminals.Connections;
using VncSharp;

namespace Terminals.Scanner
{
    internal class NetworkScanItem
    {
        // dont use events, otherwise we have to unregister
        internal NetworkScanHandler OnScanHit { get; set; }
        internal NetworkScanHandler OnScanFinished { get; set; }
        internal String HostName { get; private set; }
        
        private String iPAddress;
        private List<Int32> ports;
        private static readonly String vncPassword = String.Empty;
        private Boolean cancelationPending;
        private Boolean CancelationPending
        {
            get
            {
                lock (ports)
                {
                    return this.cancelationPending;
                }
            }
        }

        internal NetworkScanItem(String iPAddress, List<Int32> ports)
        {
            this.iPAddress = iPAddress;
            this.ports = ports;
        }

        public override string ToString()
        {
            string portsText = String.Empty;
            foreach (Int32 port in ports)
            {
                portsText += port.ToString() + ",";
            }
            return String.Format("NeworkScanItem:{0},{1}{{{2}}}", this.iPAddress, this.HostName, portsText);
        }

        internal void Scan(object data)
        {
            ResolveHostname();
            foreach (int port in this.ports)
            {
                if (this.CancelationPending)
                    return;

                ScanPort(port);
            }
            FireOnScanFinished();
        }

        internal void Stop()
        {
            lock (ports)
            {
               this.cancelationPending = true; 
            }
        }

        private void ResolveHostname()
        {
            try
            {
                if (this.CancelationPending)
                    return;

                IPHostEntry entry = Dns.GetHostEntry(this.iPAddress);
                this.HostName = entry.HostName;
            }
            catch (Exception exc)
            {
                Logging.Error("Attempting to Resolve host named failed", exc);
            }
        }

        private void ScanPort(int port)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var connectionState = new ConnectionState(port, client);
                    client.BeginConnect(this.iPAddress, port, new AsyncCallback(this.AttemptConnect), connectionState);
                    WaitUntilTimeOut(connectionState);
                    client.Client.Close();
                }
            }
            catch (Exception e)
            {
                Logging.Error("Scan Failed", e);
            }
        }

        private void FireOnScanFinished()
        {
            if (this.OnScanFinished != null)
            {
                // we dont have cancel here, because it already has no more work to do);
                this.OnScanFinished(this.CreateNewEventArguments());
            }
        }

        private void WaitUntilTimeOut(ConnectionState connectionState)
        {
            Int32 timeout = 0;
            Int32 maxTimeOut = Settings.Instance.PortScanTimeoutSeconds * 1000 / 50; // in seconds not in miliseconds
            while (timeout <= maxTimeOut && !this.CancelationPending && !connectionState.Done)
            {
                Thread.Sleep(50);
                timeout++;
            }
        }

        private void AttemptConnect(IAsyncResult result)
        {
            ConnectionState connectionState = result.AsyncState as ConnectionState;
            Socket socket = connectionState.Client.Client;

            if (socket != null && socket.Connected)
            {
                Boolean isVMRC = CheckVNCPport(connectionState.Port);
                FireOnScanHit(connectionState.Port, isVMRC);
            }
            else
                Debug.WriteLine(String.Format("Port {0} not openend at {1}",
                                              this.iPAddress, connectionState.Port));

            connectionState.Done = true;
        }

        private Boolean CheckVNCPport(Int32 port)
        {
            if (port == KnownConnectionConstants.VNCVMRCPort)
            {
                return !this.IsPortVNC(port);
            }

            return false;
        }

        private bool IsPortVNC(Int32 port)
        {
            try
            {
                RemoteDesktop rd = new RemoteDesktop();
                rd.VncPort = port;
                rd.GetPassword = new AuthenticateDelegate(this.GetVNCPassword);
                rd.Connect(this.iPAddress);
                rd.Disconnect();
            }
            catch (CryptographicException ce)
            {
                Logging.Info(string.Empty, ce);
            }
            catch (Exception exc)
            {
                Logging.Error("VNC Port Scan Failed", exc);
                return false;
            }

            return true;
        }
        
        private string GetVNCPassword()
        {
            return vncPassword;
        }

        private void FireOnScanHit(Int32 port, Boolean isVMRC)
        {
            if (this.OnScanHit != null)
            {
                ScanItemEventArgs args = this.CreateNewEventArguments(port, isVMRC);
                this.OnScanHit(args);
            }
        }

        private ScanItemEventArgs CreateNewEventArguments(Int32 port = 0, Boolean isVMRC = false)
        {
            ScanItemEventArgs args = new ScanItemEventArgs();
            args.DateTime = DateTime.Now;
            args.ScanResult = new NetworkScanResult
                {
                    HostName = this.HostName,
                    IPAddress = this.iPAddress,
                    Port = port,
                    IsVMRC = isVMRC
                };

            return args;
        }
    }
}