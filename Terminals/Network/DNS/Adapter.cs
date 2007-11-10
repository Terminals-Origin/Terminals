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


        public Boolean ArpAlwaysSourceRoute { get { return (Boolean)share.Properties["ArpAlwaysSourceRoute"].Value; } }
        public Boolean ArpUseEtherSNAP { get { return (Boolean)share.Properties["ArpUseEtherSNAP"].Value; } }
        public String Caption { get { return (string)share.Properties["Caption"].Value; } }
        public String DatabasePath { get { return (string)share.Properties["DatabasePath"].Value; } }
        public Boolean DeadGWDetectEnabled { get { return (Boolean)share.Properties["DeadGWDetectEnabled"].Value; } }
        public String DefaultIPGateway { get { return (string)share.Properties["DefaultIPGateway"].Value; } }
        public uint DefaultTOS { get { return (uint)share.Properties["DefaultTOS"].Value; } }
        public uint DefaultTTL { get { return (uint)share.Properties["DefaultTTL"].Value; } }
        public String Description { get { return (string)share.Properties["Description"].Value; } }
        public Boolean DHCPEnabled { get { return (Boolean)share.Properties["DHCPEnabled"].Value; } }
        public DateTime DHCPLeaseExpires { get { return (DateTime)share.Properties["DHCPLeaseExpires"].Value; } }
        public DateTime DHCPLeaseObtained { get { return (DateTime)share.Properties["DHCPLeaseObtained"].Value; } }
        public String DHCPServer { get { return (string)share.Properties["DHCPServer"].Value; } }
        public String DNSDomain { get { return (string)share.Properties["DNSDomain"].Value; } }
        public String DNSDomainSuffixSearchOrder { get { return (string)share.Properties["DNSDomainSuffixSearchOrder"].Value; } }
        public Boolean DNSEnabledForWINSResolution { get { return (Boolean)share.Properties["DNSEnabledForWINSResolution"].Value; } }
        public String DNSHostName { get { return (string)share.Properties["DNSHostName"].Value; } }
        public String[] DNSServerSearchOrder { get { return (string[])share.Properties["DNSServerSearchOrder"].Value; } }
        public Boolean DomainDNSRegistrationEnabled { get { return (Boolean)share.Properties["DomainDNSRegistrationEnabled"].Value; } }
        public UInt32 ForwardBufferMemory { get { return (UInt32)share.Properties["ForwardBufferMemory"].Value; } }
        public Boolean FullDNSRegistrationEnabled { get { return (Boolean)share.Properties["FullDNSRegistrationEnabled"].Value; } }
        public UInt16 GatewayCostMetric { get { return (UInt16)share.Properties["GatewayCostMetric"].Value; } }
        public uint IGMPLevel { get { return (uint)share.Properties["IGMPLevel"].Value; } }
        public UInt32 Index { get { return (UInt32)share.Properties["Index"].Value; } }
        public String IPAddress { get { return (string)share.Properties["IPAddress"].Value; } }
        public UInt32 IPConnectionMetric { get { return (UInt32)share.Properties["IPConnectionMetric"].Value; } }
        public Boolean IPEnabled { get { return (Boolean)share.Properties["IPEnabled"].Value; } }
        public Boolean IPFilterSecurityEnabled { get { return (Boolean)share.Properties["IPFilterSecurityEnabled"].Value; } }
        public Boolean IPPortSecurityEnabled { get { return (Boolean)share.Properties["IPPortSecurityEnabled"].Value; } }
        public String IPSecPermitIPProtocols { get { return (string)share.Properties["IPSecPermitIPProtocols"].Value; } }
        public String IPSecPermitTCPPorts { get { return (string)share.Properties["IPSecPermitTCPPorts"].Value; } }
        public String IPSecPermitUDPPorts { get { return (string)share.Properties["IPSecPermitUDPPorts"].Value; } }
        public String IPSubnet { get { return (string)share.Properties["IPSubnet"].Value; } }
        public Boolean IPUseZeroBroadcast { get { return (Boolean)share.Properties["IPUseZeroBroadcast"].Value; } }
        public String IPXAddress { get { return (string)share.Properties["IPXAddress"].Value; } }
        public Boolean IPXEnabled { get { return (Boolean)share.Properties["IPXEnabled"].Value; } }
        public UInt32 IPXFrameType { get { return (UInt32)share.Properties["IPXFrameType"].Value; } }
        public UInt32 IPXMediaType { get { return (UInt32)share.Properties["IPXMediaType"].Value; } }
        public String IPXNetworkNumber { get { return (string)share.Properties["IPXNetworkNumber"].Value; } }
        public String IPXVirtualNetNumber { get { return (string)share.Properties["IPXVirtualNetNumber"].Value; } }
        public UInt32 KeepAliveInterval { get { return (UInt32)share.Properties["KeepAliveInterval"].Value; } }
        public UInt32 KeepAliveTime { get { return (UInt32)share.Properties["KeepAliveTime"].Value; } }
        public String MACAddress { get { return (string)share.Properties["MACAddress"].Value; } }
        public UInt32 MTU { get { return (UInt32)share.Properties["MTU"].Value; } }
        public UInt32 NumForwardPackets { get { return (UInt32)share.Properties["NumForwardPackets"].Value; } }
        public Boolean PMTUBHDetectEnabled { get { return (Boolean)share.Properties["PMTUBHDetectEnabled"].Value; } }
        public Boolean PMTUDiscoveryEnabled { get { return (Boolean)share.Properties["PMTUDiscoveryEnabled"].Value; } }
        public String ServiceName { get { return (string)share.Properties["ServiceName"].Value; } }
        public String SettingID { get { return (string)share.Properties["SettingID"].Value; } }
        public UInt32 TcpipNetbiosOptions { get { return (UInt32)share.Properties["TcpipNetbiosOptions"].Value; } }
        public UInt32 TcpMaxConnectRetransmissions { get { return (UInt32)share.Properties["TcpMaxConnectRetransmissions"].Value; } }
        public UInt32 TcpMaxDataRetransmissions { get { return (UInt32)share.Properties["TcpMaxDataRetransmissions"].Value; } }
        public UInt32 TcpNumConnections { get { return (UInt32)share.Properties["TcpNumConnections"].Value; } }
        public Boolean TcpUseRFC1122UrgentPointer { get { return (Boolean)share.Properties["TcpUseRFC1122UrgentPointer"].Value; } }
        public UInt16 TcpWindowSize { get { return (UInt16)share.Properties["TcpWindowSize"].Value; } }
        public Boolean WINSEnableLMHostsLookup { get { return (Boolean)share.Properties["WINSEnableLMHostsLookup"].Value; } }
        public String WINSHostLookupFile { get { return (string)share.Properties["WINSHostLookupFile"].Value; } }
        public String WINSPrimaryServer { get { return (string)share.Properties["WINSPrimaryServer"].Value; } }
        public String WINSScopeID { get { return (string)share.Properties["WINSScopeID"].Value; } }
        public String WINSSecondaryServer { get { return (string)share.Properties["WINSSecondaryServer"].Value; } }

    }
}
