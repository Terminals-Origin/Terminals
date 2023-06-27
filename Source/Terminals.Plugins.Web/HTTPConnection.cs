using System;
using System.Text;
using System.Windows.Forms;
using Terminals.Common.Converters;
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
                string url = UrlConverter.ExtractAbsoluteUrl(this.Favorite);
                IGuardedSecurity security = this.ResolveFavoriteCredentials();

                if (!String.IsNullOrEmpty(security.UserName) && !String.IsNullOrEmpty(security.Password))
                {
                    string securityValues = string.Format("{0}: {1}", security.UserName, security.Password);
                    string securityHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(securityValues));
                    string authorizationHeader = string.Format("Basic {0}", securityHeader);
                    this.browser.Navigate(url, authorizationHeader);
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
                Logging.Fatal("Connecting to HTTP", exc);
                return false;
            }
            return true;
        }
    }
}