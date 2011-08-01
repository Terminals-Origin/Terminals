using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Connections {
    public class VNCConnection : Connection {
        #region IConnection Members
        private bool connected = false;
        public override void ChangeDesktopSize(DesktopSize Size) {
        }

        public void SendSpecialKeys(VncSharp.SpecialKeys Keys) {

            rd.SendSpecialKeys(Keys);
        }
        public override bool Connected { get { return connected; } }
        VncSharp.RemoteDesktop rd;
        public override bool Connect() {
            try {
                rd = new VncSharp.RemoteDesktop();
                Controls.Add(rd);

                string pass = null;
                if (!string.IsNullOrEmpty(Favorite.Credential))
                {
                  CredentialSet set = StoredCredentials.Instance.GetByName(Favorite.Credential);
                    if (set != null) 
                      pass = set.SecretKey;
                }
                else
                {
                    pass = Favorite.Password;
                }

                if(string.IsNullOrEmpty(pass))
                  pass = Settings.DefaultPassword;
                this.vncPassword = pass;

                if (string.IsNullOrEmpty(vncPassword)) return false;

                //rd.SendSpecialKeys(VncSharp.SpecialKeys);            
                rd.Parent = TerminalTabPage;
                this.Parent = TerminalTabPage;
                rd.Dock = DockStyle.Fill;

                rd.VncPort = Favorite.Port;
                rd.ConnectComplete += new VncSharp.ConnectCompleteHandler(rd_ConnectComplete);
                rd.ConnectionLost += new EventHandler(rd_ConnectionLost);
                rd.GetPassword = VNCPassword;
                Text = "Connecting to VNC Server...";
                rd.Connect(Favorite.ServerName,Favorite.VncDisplayNumber, Favorite.VncViewOnly, Favorite.VncAutoScale);

                rd.BringToFront();
                return true;

            } catch(Exception exc) {
                Logging.Log.Error("Connecting to VNC", exc);
                return false;
            }
        }

        private void rd_ConnectionLost(object sender, EventArgs e)
        {
            //Terminals.Logging.Log.Fatal("VNC Connection Lost" + this.Favorite.Name);
            this.connected = false;

            if (ParentForm.InvokeRequired)
            {
                InvokeCloseTabPage d = new InvokeCloseTabPage(CloseTabPage);
                this.Invoke(d, new object[] { rd.Parent });
            }
            else
                CloseTabPage(rd.Parent);
        }
        private string vncPassword = "";
        string VNCPassword() {
            return vncPassword;
        }

        private void rd_ConnectComplete(object sender, VncSharp.ConnectEventArgs e) {
            // Update Form to match geometry of remote desktop
            //ClientSize = new Size(e.DesktopWidth, e.DesktopHeight);
            try {
                connected = true;
                VncSharp.RemoteDesktop rd = (VncSharp.RemoteDesktop)sender;
                rd.Visible = true;
                rd.BringToFront();
                rd.FullScreenUpdate();
                rd.Enabled = true;
            } catch(Exception Exc) {
                Logging.Log.Error("ConnectComplete to VNC", Exc);
            }
            // Change the Form's title to match desktop name
        }

        public override void Disconnect() {
            try {
                connected = false;
                rd.Disconnect();
            } catch(Exception e) {
                Logging.Log.Error("Disconnect", e);
            }
        }

        #endregion
    }
}