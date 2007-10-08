using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Network
{
    public partial class DNSLookup : UserControl
    {
        public DNSLookup()
        {
            InitializeComponent();
        }
        private void lookupButton_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<KnownAlias> aliases = new List<KnownAlias>();
            System.Collections.Generic.List<IPRender> addresses = new List<IPRender>();
            try
            {
                System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(this.hostnameTextBox.Text);
                foreach (System.Net.IPAddress address in entry.AddressList)
                {
                    IPRender r = new IPRender();
                    r.address = address;
                    addresses.Add(r);
                }
                foreach (string a in entry.Aliases)
                {
                    KnownAlias ka = new KnownAlias();
                    ka.Alias = a;
                }
            }
            catch (Exception exc)
            {
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
            this.dataGridView1.DataSource = addresses;
            this.dataGridView2.DataSource = aliases;
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
