using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Scanner {
    public class NetworkScanItem {

        public delegate void ScanHitHandler(ScanItemEventArgs args);
        public event ScanHitHandler OnScanHit;

        public delegate void ScanMissHandler(ScanItemEventArgs args);
        public event ScanMissHandler OnScanMiss;

        public string IPAddress;
        public int Port;
        public bool IsOpen = false;

        public void Scan(object data) {
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            try {

                client.BeginConnect(this.IPAddress, this.Port, new AsyncCallback(AttemptConnect), client);
                int x = 0;
                while (x <= Settings.PortScanTimeoutSeconds) {
                    System.Threading.Thread.Sleep(1000);
                    x++;
                }
                client.Close();
            } catch (Exception e) {
            }

        }
        private string vncPassword = "";
        string VNCPassword() {
            return vncPassword;
        }
        object vncConnection = new object();
        private bool IsPortVNC() {
            try {
                lock (vncConnection) {
                    VncSharp.RemoteDesktop rd = new VncSharp.RemoteDesktop();
                    rd.VncPort = Port;
                    rd.GetPassword = new VncSharp.AuthenticateDelegate(VNCPassword);
                    rd.Connect(IPAddress);
                    rd.Disconnect();
                }
                return true;
            }catch(System.Security.Cryptography.CryptographicException) {
                return true;
            } catch (Exception exc) {
                exc.ToString();
            }
            return false;
        }
        private bool isVMRC = false;

        public bool IsVMRC {
            get { return isVMRC; }
            set { isVMRC = value; }
        }
	

        private void AttemptConnect(IAsyncResult result) {
            System.Net.Sockets.TcpClient client = (System.Net.Sockets.TcpClient)result.AsyncState;
            
            if (client.Client!=null && client.Connected) {
                if (OnScanHit != null) {


                    ScanItemEventArgs args = new ScanItemEventArgs();
                    if (Port == Connections.ConnectionManager.VNCVMRCPort) {
                        IsVMRC = !IsPortVNC();
                    }

                    
                    args.DateTime = DateTime.Now;
                    args.NetworkScanItem = this;
                    this.IsOpen = true;
                    OnScanHit(args);
                }
            } else {
                if (OnScanMiss != null) {
                    ScanItemEventArgs args = new ScanItemEventArgs();
                    args.DateTime = DateTime.Now;
                    args.NetworkScanItem = this;
                    OnScanMiss(args);
                }
            }
        }
    }
}