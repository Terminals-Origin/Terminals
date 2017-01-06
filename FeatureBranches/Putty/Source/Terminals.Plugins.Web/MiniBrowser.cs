using System;
using System.Windows.Forms;

namespace Terminals.Connections
{
    internal partial class MiniBrowser : UserControl
    {
        private string homeUrl;

        public MiniBrowser()
        {
            InitializeComponent();            
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.webBrowser1.GoBack();
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(homeUrl);
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            this.webBrowser1.GoForward();
        }

        internal void Navigate(string url, string additionalHeaders)
        {
            this.homeUrl = url;
            this.webBrowser1.Navigate(this.homeUrl, null, null, additionalHeaders);
        }

        internal void Navigate(string url)
        {
            this.homeUrl = url;
            this.webBrowser1.Navigate(this.homeUrl);
        }
    }
}
