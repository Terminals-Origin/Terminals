using System;
using System.Windows.Forms;
using Terminals.Data;
using Terminals.Plugins.Web;

namespace Terminals.Connections
{
    internal partial class MiniBrowser : UserControl
    {
        private string homeUrl;
        private WebOptions options;
        private string username;
        private string password;
        private bool EnableFormsAuth;

        public MiniBrowser()
        {
            InitializeComponent();
        }

        public void SetOptions(WebOptions options, string username, string password, bool EnableFormsAuth)
        {
            this.options = options;
            this.username = username;
            this.password = password;
            this.EnableFormsAuth = EnableFormsAuth;
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

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!EnableFormsAuth)
                return;

            if (webBrowser1.Document.GetElementById(options.UsernameID) != null)
                webBrowser1.Document.GetElementById(options.UsernameID).SetAttribute("Value", username);

            if (webBrowser1.Document.GetElementById(options.PasswordID) != null)
                webBrowser1.Document.GetElementById(options.PasswordID).SetAttribute("Value", password);

            if (webBrowser1.Document.GetElementById(options.OptionalID) != null)
                webBrowser1.Document.GetElementById(options.OptionalID).SetAttribute("Value", options.OptionalValue);

            if (webBrowser1.Document.GetElementById(options.SubmitID) != null)
                webBrowser1.Document.GetElementById(options.SubmitID).InvokeMember("click");
        }
    }
}
