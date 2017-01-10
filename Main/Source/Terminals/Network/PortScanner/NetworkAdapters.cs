using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Terminals
{
    internal class NetworkAdapters
    {
        private const string LOCAL_IP = "127.0.0.1";

        internal static String TryGetIPv4LocalAddress()
        {   
            try
            {
                IPInterfaceProperties adapter = SelectFirstUpAdapterProperties();

                if (adapter != null)
                    return SelectFirstIPv4Addres(adapter);

                return LOCAL_IP;

            }
            catch (Exception e)
            {
                Logging.Error("Network Scanner Failed to Find any IP v4 address on local adapters", e);
                return LOCAL_IP;
            }
        }

        private static IPInterfaceProperties SelectFirstUpAdapterProperties()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            return adapters.Where(IsUpEthernet)
                .Select(a => a.GetIPProperties())
                .FirstOrDefault();
        }

        private static bool IsUpEthernet(NetworkInterface nic)
        {
            return nic.OperationalStatus == OperationalStatus.Up &&
                   nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet;
        }

        private static string SelectFirstIPv4Addres(IPInterfaceProperties adapter)
        {
            return adapter.UnicastAddresses.Where(IsIpv4)
                .Select(ip => ip.Address.ToString())
                .FirstOrDefault();
        }

        private static bool IsIpv4(UnicastIPAddressInformation ip)
        {
            return AddressFamily.InterNetwork.Equals(ip.Address.AddressFamily);
        }
    }
}