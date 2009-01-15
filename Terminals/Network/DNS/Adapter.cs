using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Network.DNS
{
    public class Adapter
    {

        private System.Management.ManagementObject share;

        public System.Management.ManagementObject PropertyData {
            get { return share; }
            set { share = value; }
        }


        public Boolean ArpAlwaysSourceRoute { get { return (share.Properties["ArpAlwaysSourceRoute"].Value==null)?false:(Boolean)share.Properties["ArpAlwaysSourceRoute"].Value; } }
        public Boolean ArpUseEtherSNAP { get { return (share.Properties["ArpUseEtherSNAP"].Value == null) ? false : (Boolean)share.Properties["ArpUseEtherSNAP"].Value; } }
        public String Caption { get { return (share.Properties["Caption"].Value == null) ? "" : (string)share.Properties["Caption"].Value; } }
        public String DatabasePath { get { return (share.Properties["DatabasePath"].Value == null) ? "" : (string)share.Properties["DatabasePath"].Value; } }
        public Boolean DeadGWDetectEnabled { get { return (share.Properties["DeadGWDetectEnabled"].Value == null) ? false : (Boolean)share.Properties["DeadGWDetectEnabled"].Value; } }
        public String[] DefaultIPGateway { get { return (share.Properties["DefaultIPGateway"].Value == null) ? new string[] { } : (string[])share.Properties["DefaultIPGateway"].Value; } }
        public String DefaultIPGatewayList {
            get {
                int x = 1;
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(string s in DefaultIPGateway) {
                    sb.Append(s);
                    if(x<DefaultIPGateway.Length) sb.Append(",");
                    x++;
                }
                return sb.ToString();
            }
        }

        public uint DefaultTOS { get { return (share.Properties["DefaultTOS"].Value == null) ? 0 : (uint)share.Properties["DefaultTOS"].Value; } }
        public uint DefaultTTL { get { return (share.Properties["DefaultTTL"].Value == null) ? 0 : (uint)share.Properties["DefaultTTL"].Value; } }
        public String Description { get { return (share.Properties["Description"].Value == null) ? "" : (string)share.Properties["Description"].Value; } }
        public Boolean DHCPEnabled { get { return (share.Properties["DHCPEnabled"].Value == null) ? false : (Boolean)share.Properties["DHCPEnabled"].Value; } }
        public DateTime DHCPLeaseExpires { get { return (share.Properties["DHCPLeaseExpires"].Value == null) ? DateTime.MinValue : ToDateTime( share.Properties["DHCPLeaseExpires"].Value.ToString()); } }
        public DateTime DHCPLeaseObtained { get { return (share.Properties["DHCPLeaseObtained"].Value == null) ? DateTime.MinValue : ToDateTime(share.Properties["DHCPLeaseObtained"].Value.ToString()); } }
        public String DHCPServer { get { return (share.Properties["DHCPServer"].Value == null) ? "" : (string)share.Properties["DHCPServer"].Value; } }
        public String DNSDomain { get { return (share.Properties["DNSDomain"].Value == null) ? "" : (string)share.Properties["DNSDomain"].Value; } }
        public String DNSDomainSuffixSearchOrder { 
            get {
                if (share.Properties["DNSDomainSuffixSearchOrder"] == null) return "";
                object dns = share.Properties["DNSDomainSuffixSearchOrder"].Value;
                if(dns==null) return "";
                System.Array dnsList = (dns as System.Array);
                if(dnsList==null) return dns.ToString();
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (object o in dnsList)
                {
                    sb.Append(o.ToString());
                    sb.Append(",");
                }
                return sb.ToString();
            } 
        }
        public Boolean DNSEnabledForWINSResolution { get { return (share.Properties["DNSEnabledForWINSResolution"].Value == null) ? false : (Boolean)share.Properties["DNSEnabledForWINSResolution"].Value; } }
        public String DNSHostName { get { return (share.Properties["DNSHostName"].Value == null) ? "" : (string)share.Properties["DNSHostName"].Value; } }
        public String[] DNSServerSearchOrder { get { return (share.Properties["DNSServerSearchOrder"].Value == null) ? new string[] { } : (string[])share.Properties["DNSServerSearchOrder"].Value; } }
        public String DNSServerSearchOrderList {
            get {
                int x = 1;
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(string s in DNSServerSearchOrder) {
                    sb.Append(s);
                    if(x < DNSServerSearchOrder.Length) sb.Append(",");
                    x++;
                }
                return sb.ToString();
            }
        }
        public Boolean DomainDNSRegistrationEnabled { get { return (share.Properties["DomainDNSRegistrationEnabled"].Value == null) ? false : (Boolean)share.Properties["DomainDNSRegistrationEnabled"].Value; } }
        public UInt32 ForwardBufferMemory { get { return (share.Properties["ForwardBufferMemory"].Value == null) ? 0 : (UInt32)share.Properties["ForwardBufferMemory"].Value; } }
        public Boolean FullDNSRegistrationEnabled { get { return (share.Properties["FullDNSRegistrationEnabled"].Value == null) ? false : (Boolean)share.Properties["FullDNSRegistrationEnabled"].Value; } }
        public ushort[] GatewayCostMetric { get { return (share.Properties["GatewayCostMetric"].Value == null) ? new ushort[] { } : (ushort[])share.Properties["GatewayCostMetric"].Value; } }
        public String GatewayCostMetricList {
            get {
                int x = 1;
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(ushort s in GatewayCostMetric) {
                    sb.Append(s);
                    if(x < GatewayCostMetric.Length) sb.Append(",");
                    x++;
                }
                return sb.ToString();
            }
        }
        public uint IGMPLevel { get { return (share.Properties["IGMPLevel"].Value == null) ? 0 : (uint)share.Properties["IGMPLevel"].Value; } }
        public UInt32 Index { get { return (share.Properties["Index"].Value == null) ? 0 : (UInt32)share.Properties["Index"].Value; } }
        public String[] IPAddress { get { return (share.Properties["IPAddress"].Value == null) ? new string[] { } : (string[])share.Properties["IPAddress"].Value; } }
        public String IPAddressList {
            get {
                int x = 1;
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(string s in IPAddress) {
                    sb.Append(s);
                    if(x < IPAddress.Length) sb.Append(",");
                    x++;
                }
                return sb.ToString();
            }
        }   
        public UInt32 IPConnectionMetric { get { return (share.Properties["IPConnectionMetric"].Value == null) ? 0 : (UInt32)share.Properties["IPConnectionMetric"].Value; } }
        public Boolean IPEnabled { get { return (share.Properties["IPEnabled"].Value == null) ? false : (Boolean)share.Properties["IPEnabled"].Value; } }
        public Boolean IPFilterSecurityEnabled { get { return (share.Properties["IPFilterSecurityEnabled"].Value == null) ? false : (Boolean)share.Properties["IPFilterSecurityEnabled"].Value; } }
        public Boolean IPPortSecurityEnabled { get { return (share.Properties["IPPortSecurityEnabled"].Value == null) ? false : (Boolean)share.Properties["IPPortSecurityEnabled"].Value; } }
        public String[] IPSecPermitIPProtocols { get { return (share.Properties["IPSecPermitIPProtocols"].Value == null) ? new string[] { } : (string[])share.Properties["IPSecPermitIPProtocols"].Value; } }
        public String IPSecPermitIPProtocolsList {
            get {
                int x = 1;
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(string s in IPSecPermitIPProtocols) {
                    sb.Append(s);
                    if(x < IPSecPermitIPProtocols.Length) sb.Append(",");
                    x++;
                }
                return sb.ToString();
            }
        }
        public String[] IPSecPermitTCPPorts { get { return (share.Properties["IPSecPermitTCPPorts"].Value == null) ? new string[] { } : (string[])share.Properties["IPSecPermitTCPPorts"].Value; } }
        public String IPSecPermitTCPPortsList {
            get {
                int x = 1;
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(string s in IPSecPermitTCPPorts) {
                    sb.Append(s);
                    if(x < IPSecPermitTCPPorts.Length) sb.Append(",");
                    x++;
                }
                return sb.ToString();
            }
        }
        public String[] IPSecPermitUDPPorts { get { return (share.Properties["IPSecPermitUDPPorts"].Value == null) ? new string[] { } : (string[])share.Properties["IPSecPermitUDPPorts"].Value; } }
        public String IPSecPermitUDPPortsList {
            get {
                int x = 1;
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(string s in IPSecPermitUDPPorts) {
                    sb.Append(s);
                    if(x < IPSecPermitUDPPorts.Length) sb.Append(",");
                    x++;
                }
                return sb.ToString();
            }
        }
        public String[] IPSubnet { get { return (share.Properties["IPSubnet"].Value == null) ? new string[] { } : (string[])share.Properties["IPSubnet"].Value; } }
        public String IPSubnetList {
            get {
                int x = 1;
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(string s in IPSubnet) {
                    sb.Append(s);
                    if(x < IPSubnet.Length) sb.Append(",");
                    x++;
                }
                return sb.ToString();
            }
        }
        public Boolean IPUseZeroBroadcast { get { return (share.Properties["IPUseZeroBroadcast"].Value == null) ? false : (Boolean)share.Properties["IPUseZeroBroadcast"].Value; } }
        public String IPXAddress { get { return (share.Properties["IPXAddress"].Value == null) ? "" : (string)share.Properties["IPXAddress"].Value; } }
        public Boolean IPXEnabled { get { return (share.Properties["IPXEnabled"].Value == null) ? false : (Boolean)share.Properties["IPXEnabled"].Value; } }
        public UInt32 IPXFrameType { get { return (share.Properties["IPXFrameType"].Value == null) ? 0 : (UInt32)share.Properties["IPXFrameType"].Value; } }
        public UInt32 IPXMediaType { get { return (share.Properties["IPXMediaType"].Value == null) ? 0 : (UInt32)share.Properties["IPXMediaType"].Value; } }
        public String IPXNetworkNumber { get { return (share.Properties["IPXNetworkNumber"].Value == null) ? "" : (string)share.Properties["IPXNetworkNumber"].Value; } }
        public String IPXVirtualNetNumber { get { return (share.Properties["IPXVirtualNetNumber"].Value == null) ? "" : (string)share.Properties["IPXVirtualNetNumber"].Value; } }
        public UInt32 KeepAliveInterval { get { return (share.Properties["KeepAliveInterval"].Value == null) ? 0 : (UInt32)share.Properties["KeepAliveInterval"].Value; } }
        public UInt32 KeepAliveTime { get { return (share.Properties["KeepAliveTime"].Value == null) ? 0 : (UInt32)share.Properties["KeepAliveTime"].Value; } }
        public String MACAddress { get { return (share.Properties["MACAddress"].Value == null) ? "" : (string)share.Properties["MACAddress"].Value; } }
        public UInt32 MTU { get { return (share.Properties["MTU"].Value == null) ? 0 : (UInt32)share.Properties["MTU"].Value; } }
        public UInt32 NumForwardPackets { get { return (share.Properties["NumForwardPackets"].Value == null) ? 0 : (UInt32)share.Properties["NumForwardPackets"].Value; } }
        public Boolean PMTUBHDetectEnabled { get { return (share.Properties["PMTUBHDetectEnabled"].Value == null) ? false : (Boolean)share.Properties["PMTUBHDetectEnabled"].Value; } }
        public Boolean PMTUDiscoveryEnabled { get { return (share.Properties["PMTUDiscoveryEnabled"].Value == null) ? false : (Boolean)share.Properties["PMTUDiscoveryEnabled"].Value; } }
        public String ServiceName { get { return (share.Properties["ServiceName"].Value == null) ? "" : (string)share.Properties["ServiceName"].Value; } }
        public String SettingID { get { return (share.Properties["SettingID"].Value == null) ? "" : (string)share.Properties["SettingID"].Value; } }
        public UInt32 TcpipNetbiosOptions { get { return (share.Properties["TcpipNetbiosOptions"].Value == null) ? 0 : (UInt32)share.Properties["TcpipNetbiosOptions"].Value; } }
        public UInt32 TcpMaxConnectRetransmissions { get { return (share.Properties["TcpMaxConnectRetransmissions"].Value == null) ? 0 : (UInt32)share.Properties["TcpMaxConnectRetransmissions"].Value; } }
        public UInt32 TcpMaxDataRetransmissions { get { return (share.Properties["TcpMaxDataRetransmissions"].Value == null) ? 0 : (UInt32)share.Properties["TcpMaxDataRetransmissions"].Value; } }
        public UInt32 TcpNumConnections { get { return (share.Properties["TcpNumConnections"].Value == null) ? 0 : (UInt32)share.Properties["TcpNumConnections"].Value; } }
        public Boolean TcpUseRFC1122UrgentPointer { get { return (share.Properties["TcpUseRFC1122UrgentPointer"].Value == null) ? false : (Boolean)share.Properties["TcpUseRFC1122UrgentPointer"].Value; } }
        public ushort TcpWindowSize
        { 
            get {
                return (share.Properties["TcpWindowSize"].Value == null) ? (ushort)0 : (ushort)share.Properties["TcpWindowSize"].Value; 
            } 
        }
        public Boolean WINSEnableLMHostsLookup { get { return (share.Properties["WINSEnableLMHostsLookup"].Value == null) ? false : (Boolean)share.Properties["WINSEnableLMHostsLookup"].Value; } }
        public String WINSHostLookupFile { get { return (share.Properties["WINSHostLookupFile"].Value == null) ? "" : (string)share.Properties["WINSHostLookupFile"].Value; } }
        public String WINSPrimaryServer { get { return (share.Properties["WINSPrimaryServer"].Value == null) ? "" : (string)share.Properties["WINSPrimaryServer"].Value; } }
        public String WINSScopeID { get { return (share.Properties["WINSScopeID"].Value == null) ? "" : (string)share.Properties["WINSScopeID"].Value; } }
        public String WINSSecondaryServer { get { return (share.Properties["WINSSecondaryServer"].Value == null) ? "" : (string)share.Properties["WINSSecondaryServer"].Value; } }
        //There is a utility called mgmtclassgen that ships with the .NET SDK that
        //will generate managed code for existing WMI classes. It also generates
        // datetime conversion routines like this one.
        //Thanks to Chetan Parmar and dotnet247.com for the help.
        static System.DateTime ToDateTime(string dmtfDate) {
            int year = System.DateTime.Now.Year;
            int month = 1;
            int day = 1;
            int hour = 0;
            int minute = 0;
            int second = 0;
            int millisec = 0;
            string dmtf = dmtfDate;
            string tempString = System.String.Empty;

            if(((System.String.Empty == dmtf) || (dmtf == null))) {
                return System.DateTime.MinValue;
            }

            if((dmtf.Length != 25)) {
                return System.DateTime.MinValue;
            }

            tempString = dmtf.Substring(0, 4);
            if(("****" != tempString)) {
                year = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(4, 2);

            if(("**" != tempString)) {
                month = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(6, 2);

            if(("**" != tempString)) {
                day = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(8, 2);

            if(("**" != tempString)) {
                hour = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(10, 2);

            if(("**" != tempString)) {
                minute = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(12, 2);

            if(("**" != tempString)) {
                second = System.Int32.Parse(tempString);
            }

            tempString = dmtf.Substring(15, 3);

            if(("***" != tempString)) {
                millisec = System.Int32.Parse(tempString);
            }

            System.DateTime dateRet = new System.DateTime(year, month, day, hour, minute, second, millisec);

            return dateRet;
        }
    }
}
