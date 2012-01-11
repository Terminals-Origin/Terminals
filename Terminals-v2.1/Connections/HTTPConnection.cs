using System;
using System.Text;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections
{
    internal class HTTPConnection : Connection
    {
        private MiniBrowser browser = new MiniBrowser();
        public override bool Connected
        {
            get { return true; }
        }
        public override void ChangeDesktopSize(DesktopSize Size)
        {
        }

        public override bool Connect()
        {
            try
            {
                this.Dock = DockStyle.Fill;
                this.browser.Home = WebOptions.ExtractAbsoluteUrl(this.Favorite);
                SecurityOptions security = this.Favorite.Security.GetResolvedCredentials();

                if (!String.IsNullOrEmpty(security.UserName) && !String.IsNullOrEmpty(security.Password))
                {
                    string securityValues = string.Format("{0}: {1}", security.UserName, security.Password);
                    string securityHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(securityValues));
                    string additionalHeaders = string.Format("Authorization: Basic {0}{1}", securityHeader, Environment.NewLine);
                    this.browser.Browser.Navigate(this.browser.Home, null, null, additionalHeaders);
                }
                else
                {
                    this.browser.Browser.Navigate(this.browser.Home);
                }

                this.Controls.Add(this.browser);
                this.browser.Dock = DockStyle.Fill;
                this.browser.Parent = this;
                this.Parent = TerminalTabPage;
                this.BringToFront();
                this.browser.BringToFront();
            }
            catch (Exception exc)
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