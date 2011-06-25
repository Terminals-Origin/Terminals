using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Connections
{
    public partial class MiniBrowser : UserControl
    {
        public MiniBrowser()
        {
            InitializeComponent();
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
