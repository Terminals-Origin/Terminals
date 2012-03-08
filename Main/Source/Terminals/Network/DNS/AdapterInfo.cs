using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace Terminals.Network.DNS
{
    internal class AdapterInfo
    {
        public static List<Adapter> GetAdapters()
        {
            List<Adapter> adapterList = new List<Adapter>();

            ManagementObjectSearcher searcher;
            ObjectQuery q = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            searcher = new ManagementObjectSearcher(q);
            foreach (ManagementObject share in searcher.Get())
            {
                Adapter ad = new Adapter();
                ad.PropertyData = share;
                adapterList.Add(ad);
            }

            return adapterList;
        }

        public static List<String> DNSServers
        {
            get
            {
                List<string> servers = new List<String>();
                try
                {
                    List<Adapter> adapters = AdapterInfo.GetAdapters();
                    foreach (Adapter a in adapters)
                    {
                        if (a.IPEnabled)
                        {
                            if (a.DNSServerSearchOrder != null)
                            {
                                foreach (String server in a.DNSServerSearchOrder)
                                {
                                    servers.Add(server);
                                }
                            }
                        }
                    }
                }
                catch(Exception exc)
                {
                    Terminals.Logging.Log.Error("DNS Server Lookup Failed (WMI)", exc);
                }

                return servers;
            }
        }
    }
}
