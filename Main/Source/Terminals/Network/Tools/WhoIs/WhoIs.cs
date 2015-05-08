using System;
using System.Windows.Forms;

namespace Terminals.Network.WhoIs
{
    internal partial class WhoIs : UserControl
    {
        public WhoIs()
        {
            InitializeComponent();
        }

        private void whoisButton_Click(object sender, EventArgs e)
        {
            String server = this.hostTextbox.Text.Trim();
            if (server != String.Empty)
            {
                if (!server.StartsWith("=") && !server.ToLower().EndsWith(".ca")) 
                    server = "=" + server;

                String result = WhoisResolver.Whois(server);
                result = result.Replace("\n", Environment.NewLine);
                Int32 pos = result.IndexOf("Whois Server:");
                if (pos > 0)
                {
                    String newServer = result.Substring(pos + 13, result.IndexOf("\r\n", pos) - pos - 13);
                    if (server.StartsWith("=")) 
                        server = this.hostTextbox.Text.Trim();

                    String newResults = WhoisResolver.Whois(server, newServer.Trim());
                    if (!String.IsNullOrEmpty(newResults)) 
                        newResults = newResults.Replace("\n", Environment.NewLine); ;

                    result = String.Format("{0}\r\n----------------------Sub Query:{1}--------------------------\r\n{2}", result, newServer, newResults);
                }

                this.textBox2.Text = result;
            }
        }

        private void hostTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                whoisButton_Click(null, null);
            }
        }
    }
}