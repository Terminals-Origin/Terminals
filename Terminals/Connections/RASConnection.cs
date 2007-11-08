using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Connections
{
    public class RASConnection : Connection
    {
        public override bool Connected
        {
            get { return ras.Connected; }
        }
        public override void ChangeDesktopSize(Terminals.DesktopSize Size)
        {
        }
        public FalafelSoftware.TransPort.Ras ras;

        private void EnsureConnection()
        {
        }

        public override bool Connect()
        {
            try
            {
                ras = new FalafelSoftware.TransPort.Ras();
                RASProperties p = new RASProperties();
                p.RASConnection = this;
                p.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                Controls.Add(p);
                p.BringToFront();
                this.BringToFront();
                p.Parent = TerminalTabPage;

                this.ras.SetModemSpeaker = false;
                this.ras.SetSoftwareCompression = false;
                this.ras.UsePrefixSuffix = false;
                ras.HangUpOnDestroy = true;

                ras.DialError += new FalafelSoftware.TransPort.DialErrorEventHandler(ras_DialError);
                ras.DialStatus += new FalafelSoftware.TransPort.DialStatusEventHandler(ras_DialStatus);
                ras.ConnectionChanged += new FalafelSoftware.TransPort.ConnectionChangedEventHandler(ras_ConnectionChanged);
                ras.EntryName = Favorite.ServerName;

                string domainName = Favorite.DomainName;
                if(domainName == null || domainName == "") domainName = Settings.DefaultDomain;
                string pass = Favorite.Password;
                if(pass == null || pass == "") pass = Settings.DefaultPassword;
                string userName = Favorite.UserName;
                if(userName == null || userName == "") userName = Settings.DefaultUsername;



                FalafelSoftware.TransPort.RasError error;
                if(Favorite.UserName != null && Favorite.UserName.Trim() != string.Empty && Favorite.Password != null && Favorite.Password.Trim() != string.Empty)
                {
                    Log("Using Terminals Credentials, Dialing...");
                    ras.UserName = userName;
                    ras.Password = pass;
                    ras.Domain = domainName;
                    error = ras.Dial();
                }
                else
                {
                    Log("Terminals has no credentials, Showing Dial Dialog...");
                    error = ras.DialDialog();
                }

                Log("Dial Result:" + error.ToString());
                return (error == FalafelSoftware.TransPort.RasError.Success);

            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Fatal("Connecting to VMRC", exc);
                return false;
            }
        }

        void ras_DialStatus(object sender, FalafelSoftware.TransPort.DialStatusEventArgs e)
        {
            Log("Status:" + e.ConnectionState.ToString());
        }

        void ras_DialError(object sender, FalafelSoftware.TransPort.DialErrorEventArgs e)
        {
            if(e.RasError != FalafelSoftware.TransPort.RasError.Success)
            {
                Log("Error:" + e.RasError.ToString());
                System.Windows.Forms.MessageBox.Show("Could not connect to the server. Reason:" + e.RasError.ToString());
            }
            else
            {
            }
        }

        void ras_ConnectionChanged(object sender, FalafelSoftware.TransPort.ConnectionChangedEventArgs e)
        {
            Log("Connected:" + e.Connected.ToString());
        }
        public override void Disconnect()
        {
            Log("Hanging Up:" + ras.HangUp().ToString());
        }
    }
}