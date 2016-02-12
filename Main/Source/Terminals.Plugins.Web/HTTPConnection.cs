using System;
using System.Text;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections
{
    internal class HTTPConnection : Connection
    {
        private readonly MiniBrowser browser = new MiniBrowser();
        public override bool Connected
        {
            get { return true; }
        }

        internal HTTPConnection()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Controls.Add(this.browser);
            this.browser.Dock = DockStyle.Fill;
            this.browser.Parent = this;
            this.ResumeLayout(false);
        }

        public override bool Connect()
        {
            try
            {
                this.Dock = DockStyle.Fill;
                string url = WebOptions.ExtractAbsoluteUrl(this.Favorite);
                ISecurityOptions security = this.Favorite.Security.GetResolvedCredentials();

                if (!String.IsNullOrEmpty(security.UserName) && !String.IsNullOrEmpty(security.Password))
                {
                    string securityValues = string.Format("{0}: {1}", security.UserName, security.Password);
                    string securityHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(securityValues));
                    string additionalHeaders = string.Format("Authorization: Basic {0}{1}", securityHeader, Environment.NewLine);
                    this.browser.Navigate(url, additionalHeaders);
                }
                else
                {
                    this.browser.Navigate(url);
                }

                this.BringToFront();
                this.browser.BringToFront();
            }
            catch (Exception exc)
            {
                //TODO Logging.Fatal("Connecting to HTTP", exc);
                return false;
            }
            return true;
        }
    }
}