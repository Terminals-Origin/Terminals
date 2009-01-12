using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Terminals.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TabControl;
using System.IO;

namespace Terminals.Connections {
    public class VNCConnection : Connection {
        #region IConnection Members
        private bool connected = false;
        public override void ChangeDesktopSize(Terminals.DesktopSize Size) {
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

                string pass = Favorite.Password;
                if(pass == null || pass == "") pass = Settings.DefaultPassword;

                this.vncPassword = pass;

                if(pass == null || pass == "") {
                    Terminals.InputBoxResult result = Terminals.InputBox.Show("VNC Password", "Please specify your password now", '*');
                    if(result.ReturnCode == DialogResult.OK) {
                        if(result.Text != null && result.Text != "") {
                            this.vncPassword = result.Text;
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                }
                //rd.SendSpecialKeys(VncSharp.SpecialKeys);            
                rd.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;
                rd.Dock = DockStyle.Fill;              

                rd.ConnectComplete += new VncSharp.ConnectCompleteHandler(rd_ConnectComplete);
                rd.ConnectionLost += new EventHandler(rd_ConnectionLost);
                rd.GetPassword = VNCPassword;
                Text = "Connecting to VNC Server...";
                rd.Connect(Favorite.ServerName,Favorite.VncDisplayNumber, Favorite.VncViewOnly, Favorite.VncAutoScale);

                rd.BringToFront();
                return true;

            } catch(Exception exc) {
                Terminals.Logging.Log.Fatal("Connecting to VNC", exc);
                return false;
            }
        }

        void rd_ConnectionLost(object sender, EventArgs e)
        {
            Terminals.Logging.Log.Fatal("VNC Connection Lost" + this.Favorite.Name);
            this.connected = false;

            TabControlItem selectedTabPage = (TabControlItem)(this.Parent);
            bool wasSelected = selectedTabPage.Selected;
            ParentForm.tcTerminals.RemoveTab(selectedTabPage);
            ParentForm.tcTerminals_TabControlItemClosed(null, EventArgs.Empty);
            if(wasSelected)
                NativeApi.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
            ParentForm.UpdateControls();

        }
        private string vncPassword = "";
        string VNCPassword() {
            return vncPassword;
        }

        void rd_ConnectComplete(object sender, VncSharp.ConnectEventArgs e) {
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
                Terminals.Logging.Log.Fatal("ConnectComplete to VNC", Exc);
            }
            // Change the Form's title to match desktop name
        }

        public override void Disconnect() {
            try {
                connected = false;
                rd.Disconnect();
            } catch(Exception e) {
                Terminals.Logging.Log.Info("Disconnect", e);
            }
        }

        #endregion
    }
}