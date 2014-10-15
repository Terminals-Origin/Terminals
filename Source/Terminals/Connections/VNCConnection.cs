using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Connections
{
    internal class VNCConnection : Connection
    {
        #region IConnection Members
        private bool connected = false;
        public override void ChangeDesktopSize(DesktopSize Size)
        {
        }

        public void SendSpecialKeys(VncSharp.SpecialKeys Keys)
        {

            rd.SendSpecialKeys(Keys);
        }
        public override bool Connected { get { return connected; } }
        VncSharp.RemoteDesktop rd;
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
                rd.Parent = TerminalTabPage;
                this.Parent = TerminalTabPage;
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
                Logging.Error("Connecting to VNC", exc);
                return false;
            }
        }

        private void rd_ConnectionLost(object sender, EventArgs e)
        {
            //Terminals.Logging.Log.Fatal("VNC Connection Lost" + this.Favorite.Name);
            this.connected = false;
            this.FireConnectionClosed();
        }

        private string vncPassword = "";
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

        public override void Disconnect()
        {
            try
            {
                connected = false;
                rd.Disconnect();
            }
            catch (Exception e)
            {
                Logging.Error("Disconnect", e);
            }
        }

        #endregion
    }
}