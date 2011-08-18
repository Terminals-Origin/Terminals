using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using Terminals.Configuration;
using Terminals.Connections;
using VncSharp;

namespace Terminals.Scanner
{
    internal class NetworkScanItem
    {
        internal event NetworkScanHandler OnScanHit;
        internal event NetworkScanHandler OnScanMiss;

        internal string IPAddress { get; set; }
        internal int Port { get; set; }
        internal string HostName { get; set; }
        internal bool IsVMRC { get; set; }

        private string vncPassword = string.Empty;
        private object vncConnection = new object();

        private static Dictionary<string, string> knownHostNames = new Dictionary<string, string>();

        internal void Scan(object data)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.BeginConnect(this.IPAddress, this.Port, new AsyncCallback(AttemptConnect), client);
                int x = 0;
                while (x <= Settings.PortScanTimeoutSeconds)
                {
                    System.Threading.Thread.Sleep(1000);
                    x++;
                }

                client.Close();
            }
            catch (Exception e)
            {
                Logging.Log.Error("Scan Failed", e);
            }
        }

        private string VNCPassword()
        {
            return this.vncPassword;
        }

        private bool IsPortVNC()
        {
            try
            {
                lock (this.vncConnection)
                {
                    RemoteDesktop rd = new RemoteDesktop();
                    rd.VncPort = Port;
                    rd.GetPassword = new AuthenticateDelegate(this.VNCPassword);
                    rd.Connect(this.IPAddress);
                    rd.Disconnect();
                }

                return true;
            }
            catch (CryptographicException ce)
            {
                Logging.Log.Info(string.Empty, ce);
                return true;
            }
            catch (Exception exc)
            {
                Logging.Log.Error("VNC Port Scan Failed", exc);
                exc.ToString();
            }

            return false;
        }

        private void AttemptConnect(IAsyncResult result)
        {
            TcpClient client = result.AsyncState as TcpClient;
            if (client.Client != null && client.Connected)
            {
                CheckVNCPport();
                CheckHostName();
                FireOnScanHit();
            }
            else
            {
                FireOnscanMiss();
            }
        }

        private void CheckHostName()
        {
            try
            {
                if (knownHostNames.ContainsKey(this.IPAddress))
                {
                    this.HostName = knownHostNames[this.IPAddress];
                }
                else
                {
                    IPHostEntry entry = Dns.GetHostEntry(this.IPAddress);
                    this.HostName = entry.HostName;
                    if (!knownHostNames.ContainsKey(this.IPAddress))
                        knownHostNames.Add(this.IPAddress, this.HostName);
                }
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Attempting to Resolve host named failed", exc);
                this.HostName = this.IPAddress;
                if (!knownHostNames.ContainsKey(this.IPAddress))
                    knownHostNames.Add(this.IPAddress, this.IPAddress);
            }
        }

        private void CheckVNCPport()
        {
            if (this.Port == ConnectionManager.VNCVMRCPort)
            {
                this.IsVMRC = !this.IsPortVNC();
            }
        }

        private void FireOnscanMiss()
        {
            if (this.OnScanMiss != null)
            {
                this.OnScanMiss(this.CreateNewEventArguments());
            }
        }

        private void FireOnScanHit()
        {
            if (this.OnScanHit != null)
            {
                ScanItemEventArgs args = this.CreateNewEventArguments();
                this.OnScanHit(args);
            }
        }

        internal ScanItemEventArgs CreateNewEventArguments()
        {
            ScanItemEventArgs args = new ScanItemEventArgs();
            args.DateTime = DateTime.Now;
            args.NetworkScanItem = this;
            return args;
        }
    }
}