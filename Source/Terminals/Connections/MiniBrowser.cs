using System;
using System.Windows.Forms;

namespace Terminals.Connections
{
    internal partial class MiniBrowser : UserControl
    {
        public MiniBrowser()
        {
            InitializeComponent();
            this.webBrowser1.ScriptErrorsSuppressed = true;
        }
        public WebBrowser Browser
        {
            get
            {
                return this.webBrowser1;
            }
        }
        public string Home { get; set; }
        private void backButton_Click(object sender, EventArgs e)
        {
            Browser.GoBack();
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            Browser.Navigate(Home);
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            Browser.GoForward();
        }
    }
}
