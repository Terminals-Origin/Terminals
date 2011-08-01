using System;
using System.Collections.Generic;
using System.Text;
using Terminals.Configuration;

namespace Terminals.Connections
{
    public class HTTPConnection : Connection
    {

        MiniBrowser browser = new MiniBrowser();
        public override bool Connected
        {
            get { return true; }
        }
        public override void ChangeDesktopSize(Terminals.DesktopSize Size)
        {
        }
        

        private void EnsureConnection()
        {
        }
        public override bool Connect()
        {
            try
            {
                this.Dock = System.Windows.Forms.DockStyle.Fill;


                string domainName = Favorite.DomainName;
                string pass = Favorite.Password;
                string userName = Favorite.UserName;

                if (string.IsNullOrEmpty(domainName)) domainName = Settings.DefaultDomain;
                if (string.IsNullOrEmpty(pass)) pass = Settings.DefaultPassword;
                if (string.IsNullOrEmpty(userName)) userName = Settings.DefaultUsername;


                this.browser.Home = Favorite.Url;

                if(!String.IsNullOrEmpty(Favorite.UserName) && !String.IsNullOrEmpty(Favorite.Password)) {
                    string hdr = "Authorization: Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + pass)) + Environment.NewLine;
                    this.browser.Browser.Navigate(Favorite.Url,null,null, hdr);
                } else {
                    this.browser.Browser.Navigate(Favorite.Url);
                }
                

                this.Controls.Add(this.browser);
                this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
                this.browser.Parent = this;
                this.Parent = TerminalTabPage;
                this.BringToFront();
                this.browser.BringToFront();
                
            }
            catch(Exception exc)
            {
                Logging.Log.Fatal("Connecting to HTTP", exc);
                return false;
            }
            return true;
        }

        public override void Disconnect()
        {
            if (ParentForm.InvokeRequired)
            {
                InvokeCloseTabPage d = new InvokeCloseTabPage(CloseTabPage);
                this.Invoke(d, new object[] { this.Parent });
            }
            else
                CloseTabPage(this.Parent);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}