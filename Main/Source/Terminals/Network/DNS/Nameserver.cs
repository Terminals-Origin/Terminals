using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Network.DNS
{
    internal class Nameserver
    {
        public static List<Nameserver> DNSList
        {
            get
            {
                List<Nameserver> l = new List<Nameserver>();
                //DNS.AdapterInfo.GetAdapters();
                Nameserver ns = new Nameserver("198.41.0.4", "ns.internic.net", "VeriSign", "Dulles, Virginia, U.S.", "BIND");

                return l;
            }
        }
        public static List<Nameserver> RootNameserverList
        {
            get
            {
                List<Nameserver> l = new List<Nameserver>();

                Nameserver ns = new Nameserver("198.41.0.4", "ns.internic.net", "VeriSign", "Dulles, Virginia, U.S.", "BIND");
                Nameserver ns1 = new Nameserver("192.228.79.201", "ns1.isi.edu", "USC-ISI", "Marina Del Rey, California, U.S.", "BIND");
                Nameserver ns2 = new Nameserver("192.33.4.12", "c.psi.net", "Cogent Communications", "distributed using anycast", "BIND");
                Nameserver ns3 = new Nameserver("128.8.10.90", "terp.umd.edu", "University of Maryland", "College Park, Maryland, U.S.", "BIND");
                Nameserver ns4 = new Nameserver("192.203.230.10", "ns.nasa.gov", "NASA", "Mountain View, California, U.S.", "BIND");
                Nameserver ns5 = new Nameserver("192.5.5.241", "ns.isc.org", "ISC", "distributed using anycast", "BIND");
                Nameserver ns6 = new Nameserver("192.112.36.4", "ns.nic.ddn.mil", "Defense Information Systems Agency", "Columbus, Ohio, U.S.", "BIND");
                Nameserver ns7 = new Nameserver("128.63.2.53", "aos.arl.army.mil", "U.S. Army Research Lab", "Aberdeen Proving Ground, Maryland, U.S.", "NSD");
                Nameserver ns8 = new Nameserver("192.36.148.17", "nic.nordu.net", "Autonomica", "distributed using anycast", "BIND");
                Nameserver ns9 = new Nameserver("192.58.128.30", "", "VeriSign", "distributed using anycast", "BIND");
                Nameserver ns10 = new Nameserver("193.0.14.129", "", "RIPE NCC", "distributed using anycast", "NSD");
                Nameserver ns11 = new Nameserver("198.32.64.12", "", "ICANN", "Los Angeles, California, U.S.", "NSD");
                Nameserver ns12 = new Nameserver("202.12.27.33", "", "WIDE Project", "distributed using anycast", "BIND");
                l.Add(ns);
                l.Add(ns1);
                l.Add(ns2);
                l.Add(ns3);
                l.Add(ns4);
                l.Add(ns5);
                l.Add(ns6);
                l.Add(ns7);
                l.Add(ns8);
                l.Add(ns9);
                l.Add(ns10);
                l.Add(ns11);
                l.Add(ns12);
                return l;
            }
        }
        public Nameserver()
        {
        }
        public Nameserver(string IPAddress, string OldName, string OperatorName, string Location, string Software)
        {
            this.address = IPAddress;
            this.oldName = OldName;
            this.operatorName = OperatorName;
            this.location = Location;
            this.software = Software;
        }
        private string address;

        public string IPAddress
        {
            get { return address; }
            set { address = value; }
        }

        private string oldName;

        public string OldName
        {
            get { return oldName; }
            set { oldName = value; }
        }

        private string operatorName;

        public string OperatorName
        {
            get { return operatorName; }
            set { operatorName = value; }
        }
        private string location;

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        private string software;

        public string Software
        {
            get { return software; }
            set { software = value; }
        }
	
	
    }
}
