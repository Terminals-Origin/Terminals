using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Connections
{
    public class HTTPConnection : Connection//System.Windows.Forms.Control
    {
    
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
        System.Windows.Forms.WebBrowser webBrowser1;
        public override bool Connect()
        {
            try
            {
                webBrowser1 = new System.Windows.Forms.WebBrowser();

                this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.webBrowser1.Location = new System.Drawing.Point(0, 0);
                this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
                this.webBrowser1.Name = "webBrowser1";
                this.webBrowser1.Size = new System.Drawing.Size(292, 273);
                this.webBrowser1.TabIndex = 0;
                
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                

                if(!String.IsNullOrEmpty(Favorite.UserName) && !String.IsNullOrEmpty(Favorite.Password)) {
                    string hdr = "Authorization: Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Favorite.UserName + ":" + Favorite.Password)) + System.Environment.NewLine;
                    this.webBrowser1.Navigate(Favorite.Url,null,null, hdr);
                } else {
                    this.webBrowser1.Navigate(Favorite.Url);
                }

                this.Controls.Add(this.webBrowser1);
                this.webBrowser1.Parent = this;
                this.Parent = TerminalTabPage;
                this.BringToFront();
                this.webBrowser1.BringToFront();
                
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Fatal("Connecting to HTTP", exc);
                return false;
            }
            return true;
        }

        public override void Disconnect()
        {
            
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}