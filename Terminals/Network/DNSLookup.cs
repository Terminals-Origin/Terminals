using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Bdev.Net.Dns;
using System.Net;

namespace Terminals.Network
{
    public partial class DNSLookup : UserControl
    {
        public DNSLookup()
        {
            InitializeComponent();
        }

        public void ForceDNS(string Host)
        {
            this.hostnameTextBox.Text = Host;
            this.lookupButton_Click(null, null);
        }

        private void lookupButton_Click(object sender, EventArgs e)
        {
            string serverIP = serverComboBox.Text.Trim();
            if(serverIP == "") serverIP = "128.8.10.90";
            serverComboBox.Text = serverIP.Trim();
            string domain = this.hostnameTextBox.Text.Trim();
            if(domain == "") domain = "codeplex.com";
            this.hostnameTextBox.Text = domain.Trim();

            try
            {
                List<Answer> responses = new List<Answer>();

                IPAddress dnsServer = IPAddress.Parse(serverIP);
                // create a DNS request
                Request request = new Request();
                request.AddQuestion(new Question(domain, DnsType.ANAME, DnsClass.IN));
                responses.Add(Resolver.Lookup(request, dnsServer).Answers[0]);

                request = new Request();
                request.AddQuestion(new Question(domain, DnsType.MX, DnsClass.IN));
                responses.Add(Resolver.Lookup(request, dnsServer).Answers[0]);

                request = new Request();
                request.AddQuestion(new Question(domain, DnsType.NS, DnsClass.IN));
                responses.Add(Resolver.Lookup(request, dnsServer).Answers[0]);

                request = new Request();
                request.AddQuestion(new Question(domain, DnsType.SOA, DnsClass.IN));
                responses.Add(Resolver.Lookup(request, dnsServer).Answers[0]);

                this.dataGridView1.DataSource = responses;
                //this.propertyGrid1.SelectedObject = records;
                // send it to the DNS server and get the response
                //
                //this.dataGridView1.DataSource = response.Answers;

            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                System.Windows.Forms.MessageBox.Show("Could not resolve host.");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DNSLookup_Load(object sender, EventArgs e)
        {
            this.serverComboBox.DataSource = Terminals.Network.DNS.AdapterInfo.DNSServers;
            
        }

        private void hostnameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lookupButton_Click(null, null);
            }
        }
    }
    public class IPRender
    {
        public System.Net.IPAddress address;
        public string Address
        {
            get { return address.ToString(); }
        }
        public string AddressFamily
        {
            get { return address.AddressFamily.ToString();  }
        }
        public bool IsIPv6LinkLocal
        {
            get { return address.IsIPv6LinkLocal; }
        }
        public bool IsIPv6Multicast
        {
            get { return address.IsIPv6Multicast; }
        }
        public bool IsIPv6SiteLocal
        {
            get { return address.IsIPv6SiteLocal; }
        }
    }
    public class KnownAlias
    {
        private string alias;

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }
	
    }
}
