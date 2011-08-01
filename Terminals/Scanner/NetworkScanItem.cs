using System;
using System.Collections.Generic;
using Terminals.Configuration;

namespace Terminals.Scanner
{
    public class NetworkScanItem
    {
        public delegate void ScanHitHandler(ScanItemEventArgs args);

        public event ScanHitHandler OnScanHit;
        
        public delegate void ScanMissHandler(ScanItemEventArgs args);

        public event ScanMissHandler OnScanMiss;

        private static Dictionary<string, string> KnownHostNames = new Dictionary<string, string>();
        public string IPAddress;
        public int Port;
        public bool IsOpen = false;
        private bool isVMRC = false;
        public string HostName;
        private string vncPassword = string.Empty;
        private object vncConnection = new object();

        public void Scan(object data)
        {
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
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
                    VncSharp.RemoteDesktop rd = new VncSharp.RemoteDesktop();
                    rd.VncPort = Port;
                    rd.GetPassword = new VncSharp.AuthenticateDelegate(this.VNCPassword);
                    rd.Connect(IPAddress);
                    rd.Disconnect();
                }

                return true;
            }
            catch (System.Security.Cryptography.CryptographicException ce)
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

        public bool IsVMRC
        {
            get
            {
                return this.isVMRC;
            }

            set
            {
                this.isVMRC = value;
            }
        }
    
        private void AttemptConnect(IAsyncResult result)
        {
            System.Net.Sockets.TcpClient client = (System.Net.Sockets.TcpClient)result.AsyncState;
            if (client.Client!=null && client.Connected)
            {
                if (this.OnScanHit != null)
                {
                    ScanItemEventArgs args = new ScanItemEventArgs();
                    if (this.Port == Connections.ConnectionManager.VNCVMRCPort)
                    {
                        this.IsVMRC = !IsPortVNC();
                    }

                    args.DateTime = DateTime.Now;
                    args.NetworkScanItem = this;
                    try
                    {
                        if (KnownHostNames.ContainsKey(args.NetworkScanItem.IPAddress))
                        {
                            args.NetworkScanItem.HostName = KnownHostNames[args.NetworkScanItem.IPAddress];
                        }
                        else
                        {
                            //System.Net.IPHostEntry entry = System.Net.Dns.Resolve(args.NetworkScanItem.IPAddress);
                            System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(args.NetworkScanItem.IPAddress);
                            args.NetworkScanItem.HostName = entry.HostName;
                            if (!KnownHostNames.ContainsKey(args.NetworkScanItem.IPAddress)) 
                                KnownHostNames.Add(args.NetworkScanItem.IPAddress, args.NetworkScanItem.HostName);
                        }
                    }
                    catch (Exception exc)
                    {
                        Logging.Log.Error("Attempting to Resolve host named failed", exc);
                        args.NetworkScanItem.HostName = args.NetworkScanItem.IPAddress;
                        if(!KnownHostNames.ContainsKey(args.NetworkScanItem.IPAddress))
                            KnownHostNames.Add(args.NetworkScanItem.IPAddress, args.NetworkScanItem.IPAddress);
                    }

                    this.IsOpen = true;
                    OnScanHit(args);
                }
            }
            else
            {
                if (OnScanMiss != null)
                {
                    ScanItemEventArgs args = new ScanItemEventArgs();
                    args.DateTime = DateTime.Now;
                    args.NetworkScanItem = this;
                    OnScanMiss(args);
                }
            }
        }
    }
}