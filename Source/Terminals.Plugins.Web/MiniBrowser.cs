using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

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
            this.webBrowser1.CoreWebView2.Navigate(homeUrl);
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            this.webBrowser1.GoForward();
        }

        internal void Navigate(string url, string authorizationHeader)
        {
            this.homeUrl = url;
            var eventHandler = new EventHandler<CoreWebView2WebResourceRequestedEventArgs>(delegate (object o, CoreWebView2WebResourceRequestedEventArgs e)
            {
                e.Request.Headers.SetHeader("Authorization", authorizationHeader);
            });
            this.webBrowser1.EnsureCoreWebView2Async().ContinueWith(new Action<Task>((Action) =>
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    this.webBrowser1.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
                    this.webBrowser1.CoreWebView2.WebResourceRequested += eventHandler;
                    this.webBrowser1.CoreWebView2.Navigate(this.homeUrl);
                }));
            }));
        }
       
        internal void Navigate(string url)
        {
            this.homeUrl = url;
            this.webBrowser1.EnsureCoreWebView2Async().ContinueWith(new Action<Task>((Action) =>
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    this.webBrowser1.CoreWebView2.Navigate(this.homeUrl);

                }));
            }));
        }
    }
}
