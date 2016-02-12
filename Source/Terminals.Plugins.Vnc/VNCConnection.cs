using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections
{
    internal class VNCConnection : Connection
    {
        private bool connected = false;

        public override bool Connected { get { return this.connected; } }

        private VncSharp.RemoteDesktop rd;

        private string vncPassword = string.Empty;

        public void SendSpecialKeys(VncSharp.SpecialKeys Keys)
        {
            rd.SendSpecialKeys(Keys);
        }

        public override bool Connect()
        {
            try
            {
                rd = new VncSharp.RemoteDesktop();
                Controls.Add(rd);

                string pass = this.Favorite.Security.GetResolvedCredentials().Password;
                this.vncPassword = pass;

                if (string.IsNullOrEmpty(vncPassword)) return false;

                //rd.SendSpecialKeys(VncSharp.SpecialKeys);            
                rd.Parent = this.Parent;
                rd.Dock = DockStyle.Fill;

                rd.VncPort = Favorite.Port;
                rd.ConnectComplete += new VncSharp.ConnectCompleteHandler(rd_ConnectComplete);
                rd.ConnectionLost += new EventHandler(rd_ConnectionLost);
                rd.GetPassword = VNCPassword;
                Text = "Connecting to VNC Server...";

                VncOptions options = this.Favorite.ProtocolProperties as VncOptions;
                rd.Connect(Favorite.ServerName, options.DisplayNumber, options.ViewOnly, options.AutoScale);

                rd.BringToFront();
                return true;

            }
            catch (Exception exc)
            {
                //TODO Logging.Error("Connecting to VNC", exc);
                return false;
            }
        }

        private void rd_ConnectionLost(object sender, EventArgs e)
        {
            //TODO Terminals.Logging.Log.Fatal("VNC Connection Lost" + this.Favorite.Name);
            this.connected = false;
            this.FireDisconnected();
        }

        string VNCPassword()
        {
            return vncPassword;
        }

        private void rd_ConnectComplete(object sender, VncSharp.ConnectEventArgs e)
        {
            // Update Form to match geometry of remote desktop
            //ClientSize = new Size(e.DesktopWidth, e.DesktopHeight);
            try
            {
                connected = true;
                VncSharp.RemoteDesktop rd = (VncSharp.RemoteDesktop)sender;
                rd.Visible = true;
                rd.BringToFront();
                rd.FullScreenUpdate();
                rd.Enabled = true;
            }
            catch (Exception Exc)
            {
                //TODO Logging.Error("ConnectComplete to VNC", Exc);
            }
            // Change the Form's title to match desktop name
        }
    }
}