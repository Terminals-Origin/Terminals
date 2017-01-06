using System;
using FalafelSoftware.TransPort;
using Terminals.Data;

namespace Terminals.Connections
{
    internal class RASConnection : Connection
    {
        public override bool Connected
        {
            get { return ras.Connected; }
        }

        public Ras ras { get; set; }

        public override bool Connect()
        {
            try
            {
                ras = new Ras();
                RASProperties p = new RASProperties();
                p.RASConnection = this;
                p.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                Controls.Add(p);
                p.BringToFront();
                this.BringToFront();
                p.Parent = this.Parent;

                this.ras.SetModemSpeaker = false;
                this.ras.SetSoftwareCompression = false;
                this.ras.UsePrefixSuffix = false;
                ras.HangUpOnDestroy = true;

                ras.DialError += new DialErrorEventHandler(ras_DialError);
                ras.DialStatus += new DialStatusEventHandler(ras_DialStatus);
                ras.ConnectionChanged += new ConnectionChangedEventHandler(ras_ConnectionChanged);
                ras.EntryName = Favorite.ServerName;

                var security = this.ResolveFavoriteCredentials();
                RasError error;
                if (!string.IsNullOrEmpty(security.UserName) && !string.IsNullOrEmpty(security.Password))
                {
                    Log("Using Terminals Credentials, Dialing...");
                    ras.UserName = security.UserName;
                    ras.Password = security.Password;
                    ras.Domain = security.Domain;
                    error = ras.Dial();
                }
                else
                {
                    Log("Terminals has no credentials, Showing Dial Dialog...");
                    error = ras.DialDialog();
                }

                Log("Dial Result:" + error.ToString());
                return (error == RasError.Success);

            }
            catch(Exception exc)
            {
                Logging.Fatal("Connecting to RAS", exc);
                return false;
            }
        }

        private void ras_DialStatus(object sender, DialStatusEventArgs e)
        {
            Log("Status:" + e.ConnectionState.ToString());
        }

        private void ras_DialError(object sender, DialErrorEventArgs e)
        {
            if(e.RasError != RasError.Success)
            {
                Log("Error:" + e.RasError.ToString());
                System.Windows.Forms.MessageBox.Show("Could not connect to the server. Reason:" + e.RasError.ToString());
            }
        }

        private void ras_ConnectionChanged(object sender, ConnectionChangedEventArgs e)
        {
            Log("Connected:" + e.Connected.ToString());

            if (!e.Connected)
                this.FireDisconnected();
        }

        public void Disconnect()
        {
            Log("Hanging Up:" + ras.HangUp().ToString());
        }
    }
}