using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Connections {
    public class VNCConnection : Connection {
        #region IConnection Members
        private bool connected = false;
        public override void ChangeDesktopSize(Terminals.DesktopSize Size) {
        }

        public override bool Connected { get { return connected; } }
        VncSharp.RemoteDesktop rd = new VncSharp.RemoteDesktop();
        public override bool Connect() {           

            Controls.Add(rd);

            string pass = Favorite.Password;
            if(pass == null || pass == "") pass = Settings.DefaultPassword;
            if(pass!=null && pass!="") this.vncPassword = pass;

            //rd.SendSpecialKeys(VncSharp.SpecialKeys);            
            rd.Parent = base.TerminalTabPage;
            this.Parent = TerminalTabPage;
            rd.Dock = DockStyle.Fill;
            rd.VncPort = Favorite.Port;
            
            rd.ConnectComplete += new VncSharp.ConnectCompleteHandler(rd_ConnectComplete);
            rd.GetPassword = VNCPassword;
            Text = "Connecting to VNC Server...";
            rd.Connect(Favorite.ServerName);
            rd.BringToFront();
            return true;
        }
        private string vncPassword = "";
        string VNCPassword() {
            return vncPassword;
        }

        void rd_ConnectComplete(object sender, VncSharp.ConnectEventArgs e) {
            // Update Form to match geometry of remote desktop
            //ClientSize = new Size(e.DesktopWidth, e.DesktopHeight);

            connected = true;
            VncSharp.RemoteDesktop rd = (VncSharp.RemoteDesktop)sender;
            rd.Visible = true;
            rd.BringToFront();
            rd.FullScreenUpdate();
            rd.Enabled = true;
            // Change the Form's title to match desktop name
        }

        public override void Disconnect() {
            try {
                connected = false;
                rd.Disconnect();
            } catch (Exception e) {
                Terminals.Logging.Log.Info("", e);
            }
        }

        #endregion
    }
}