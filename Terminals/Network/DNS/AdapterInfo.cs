using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Network.DNS
{
    public class AdapterInfo
    {
        public static List<Adapter> GetAdapters()
        {
            List<Adapter> adapterList = new List<Adapter>();
            string query = "SELECT * FROM Win32_NetworkAdapterConfiguration";

            System.Management.ManagementObjectSearcher searcher;
            System.Management.ObjectQuery q = new System.Management.ObjectQuery(query);
            searcher = new System.Management.ManagementObjectSearcher(q);
            foreach(System.Management.ManagementObject share in searcher.Get()) {
                Adapter ad = new Adapter();
                ad.PropertyData = share;
                adapterList.Add(ad);
            }
            return adapterList;
        }

        public static List<string> DNSServers {
            get {
                List<string> servers = new List<string>();
                try {
                    List<Adapter> adapters = AdapterInfo.GetAdapters();
                    foreach(Adapter a in adapters) {
                        if(a.IPEnabled) {
                            if(a.DNSServerSearchOrder != null) {
                                foreach(string server in a.DNSServerSearchOrder) {
                                    servers.Add(server);
                                }
                            }
                        }
                    }
                } catch(Exception exc) {
                    Terminals.Logging.Log.Error("DNS Server Lookup Failed (WMI)", exc);
                }
                return servers;
            }
        }
    }
}
