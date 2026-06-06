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

        private bool viewOnly;

        /// <summary>
        /// Gets whether the live session currently ignores local mouse and keyboard input.
        /// </summary>
        public bool ViewOnly
        {
            get { return this.viewOnly; }
        }

        public void SendSpecialKeys(VncSharp.SpecialKeys Keys)
        {
            rd.SendSpecialKeys(Keys);
        }

        /// <summary>
        /// Toggles the View Only mode of the running session and returns the new state.
        /// In View Only mode local mouse and keyboard events are not sent to the host.
        /// </summary>
        public bool ToggleViewOnly()
        {
            if (rd == null)
                return this.viewOnly;

            this.viewOnly = !this.viewOnly;
            rd.SetInputMode(this.viewOnly);
            return this.viewOnly;
        }

        public override bool Connect()
        {
            try
            {
                rd = new VncSharp.RemoteDesktop();
                Controls.Add(rd);

                string pass = this.ResolveFavoriteCredentials().Password;
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
                this.viewOnly = options.ViewOnly;
                rd.Connect(Favorite.ServerName, options.DisplayNumber, options.ViewOnly, options.AutoScale);

                rd.BringToFront();
                return true;

            }
            catch (Exception exc)
            {
                Logging.Error("Connecting to VNC", exc);
                return false;
            }
        }

        private void rd_ConnectionLost(object sender, EventArgs e)
        {
            Logging.Fatal("VNC Connection Lost" + this.Favorite.Name);
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
                Logging.Error("ConnectComplete to VNC", Exc);
            }
            // Change the Form's title to match desktop name
        }
    }
}