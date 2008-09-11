using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network.WhoIs
{
    public partial class WhoIs : UserControl
    {
        public WhoIs()
        {
            InitializeComponent();
        }

        private void whoisButton_Click(object sender, EventArgs e)
        {
            string server = this.hostTextbox.Text.Trim();
            if (server != "")
            {
                if (!server.StartsWith("=")) server = "=" + server;
                string result = Org.Mentalis.Utilities.WhoisResolver.Whois(server);
                result = result.Replace("\n", "\r\n");
                int pos = result.IndexOf("Whois Server:");
                if (pos > 0)
                {
                    string newServer = result.Substring(pos + 13, result.IndexOf("\r\n", pos) - pos - 13);
                    if (server.StartsWith("=")) server = this.hostTextbox.Text.Trim();
                    string newResults = Org.Mentalis.Utilities.WhoisResolver.Whois(server, newServer.Trim());
                    if (newResults != null && newResults != "") newResults = newResults.Replace("\n", "\r\n"); ;
                    result = result + "\r\n----------------------Sub Query:"+newServer+"--------------------------\r\n" + newResults;
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