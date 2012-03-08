using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace Terminals.Network.DNS
{
    internal class Adapter
    {
        #region Public Properties
        
        public System.Management.ManagementObject PropertyData { get; set; }

        public Boolean ArpAlwaysSourceRoute
        {
            get
            {
                return this.ToBoolean("ArpAlwaysSourceRoute");
            }
        }

        public Boolean ArpUseEtherSNAP
        {
            get
            {
                return this.ToBoolean("ArpUseEtherSNAP");
            }
        }

        public String Caption
        {
            get
            {
                return this.ToString("Caption");
            }
        }

        public String DatabasePath
        {
            get
            {
                return this.ToString("DatabasePath");
            }
        }

        public Boolean DeadGWDetectEnabled
        {
            get
            {
                return this.ToBoolean("DeadGWDetectEnabled");
            }
        }

        public String[] DefaultIPGateway
        {
            get
            {
                return this.ToStringArray("DefaultIPGateway");
            }
        }

        public String DefaultIPGatewayList
        {
            get
            {
                return ToStringList(this.DefaultIPGateway);
            }
        }

        public Byte DefaultTOS
        {
            get
            {
                return this.ToByte("DefaultTOS");
            }
        }

        public Byte DefaultTTL
        {
            get
            {
                return this.ToByte("DefaultTTL");
            }
        }

        public String Description
        {
            get
            {
                return this.ToString("Description");
            }
        }

        public Boolean DHCPEnabled
        {
            get
            {
                return this.ToBoolean("DHCPEnabled");
            }
        }

        public DateTime DHCPLeaseExpires
        {
            get
            {
                return this.ToDateTime("DHCPLeaseExpires");
            }
        }

        public DateTime DHCPLeaseObtained
        {
            get
            {
                return this.ToDateTime("DHCPLeaseObtained");
            }
        }

        public String DHCPServer
        {
            get
            {
                return this.ToString("DHCPServer");
            }
        }

        public String DNSDomain
        {
            get
            {
                return this.ToString("DNSDomain");
            }
        }

        public String DNSDomainSuffixSearchOrder
        {
            get
            {
                try
                {
                    if (this.PropertyData.Properties["DNSDomainSuffixSearchOrder"] == null)
                        return String.Empty;

                    Object dns = this.PropertyData.Properties["DNSDomainSuffixSearchOrder"].Value;
                    if (dns == null)
                        return String.Empty;

                    Array dnsList = (dns as Array);
                    if (dnsList == null)
                        return dns.ToString();

                    StringBuilder sb = new StringBuilder();
                    foreach (Object o in dnsList)
                    {
                        sb.Append(o.ToString());
                        sb.Append(",");
                    }

                    return sb.ToString().TrimEnd(',');
                }
                catch (Exception exc)
                {
                    Logging.Log.Error("see: http://terminals.codeplex.com/workitem/20748", exc);
                    return String.Empty;
                }

            }
        }

        public Boolean DNSEnabledForWINSResolution
        {
            get
            {
                return this.ToBoolean("DNSEnabledForWINSResolution");
            }
        }

        public String DNSHostName
        {
            get
            {
                return this.ToString("DNSHostName");
            }
        }

        public String[] DNSServerSearchOrder
        {
            get
            {
                return this.ToStringArray("DNSServerSearchOrder");
            }
        }

        public String DNSServerSearchOrderList
        {
            get
            {
                return ToStringList(this.DNSServerSearchOrder);
            }
        }

        public Boolean DomainDNSRegistrationEnabled
        {
            get
            {
                return this.ToBoolean("DomainDNSRegistrationEnabled");
            }
        }

        public UInt32 ForwardBufferMemory
        {
            get
            {
                return this.ToUInt32("ForwardBufferMemory");
            }
        }

        public Boolean FullDNSRegistrationEnabled
        {
            get
            {
                return this.ToBoolean("FullDNSRegistrationEnabled");
            }
        }

        public UInt16[] GatewayCostMetric
        {
            get
            {
                return this.ToUInt16Array("GatewayCostMetric");
            }
        }

        public String GatewayCostMetricList
        {
            get
            {
                return ToStringList(this.GatewayCostMetric);
            }
        }

        public UInt32 IGMPLevel
        {
            get
            {
                return this.ToUInt32("IGMPLevel");
            }
        }

        public UInt32 Index
        {
            get
            {
                return this.ToUInt32("Index");
            }
        }

        public String[] IPAddress
        {
            get
            {
                return this.ToStringArray("IPAddress");
            }
        }

        public String IPAddressList
        {
            get
            {
                return ToStringList(this.IPAddress);
            }
        }

        public UInt32 IPConnectionMetric
        {
            get
            {
                return this.ToUInt32("IPConnectionMetric");
            }
        }

        public Boolean IPEnabled
        {
            get
            {
                return this.ToBoolean("IPEnabled");
            }
        }

        public Boolean IPFilterSecurityEnabled
        {
            get
            {
                return this.ToBoolean("IPFilterSecurityEnabled");
            }
        }

        public Boolean IPPortSecurityEnabled
        {
            get
            {
                return this.ToBoolean("IPPortSecurityEnabled");
            }
        }

        public String[] IPSecPermitIPProtocols
        {
            get
            {
                return this.ToStringArray("IPSecPermitIPProtocols");
            }
        }

        public String IPSecPermitIPProtocolsList
        {
            get
            {
                return ToStringList(this.IPSecPermitIPProtocols);
            }
        }

        public String[] IPSecPermitTCPPorts
        {
            get
            {
                return this.ToStringArray("IPSecPermitTCPPorts");
            }
        }

        public String IPSecPermitTCPPortsList
        {
            get
            {
                return ToStringList(this.IPSecPermitTCPPorts);
            }
        }

        public String[] IPSecPermitUDPPorts
        {
            get
            {
                return this.ToStringArray("IPSecPermitUDPPorts");
            }
        }

        public String IPSecPermitUDPPortsList
        {
            get
            {
                return ToStringList(this.IPSecPermitUDPPorts);
            }
        }

        public String[] IPSubnet
        {
            get
            {
                return this.ToStringArray("IPSubnet");
            }
        }

        public String IPSubnetList
        {
            get
            {

                return ToStringList(this.IPSubnet);
            }
        }

        public Boolean IPUseZeroBroadcast
        {
            get
            {
                return this.ToBoolean("IPUseZeroBroadcast");
            }
        }

        public String IPXAddress
        {
            get
            {
                return this.ToString("IPXAddress");
            }
        }

        public Boolean IPXEnabled
        {
            get
            {
                return this.ToBoolean("IPXEnabled");
            }
        }

        public UInt32 IPXFrameType
        {
            get
            {
                return this.ToUInt32("IPXFrameType");
            }
        }

        public UInt32 IPXMediaType
        {
            get
            {
                return this.ToUInt32("IPXMediaType");
            }
        }

        public String IPXNetworkNumber
        {
            get
            {
                return this.ToString("IPXNetworkNumber");
            }
        }

        public String IPXVirtualNetNumber
        {
            get
            {
                return this.ToString("IPXVirtualNetNumber");
            }
        }

        public UInt32 KeepAliveInterval
        {
            get
            {
                return this.ToUInt32("KeepAliveInterval");
            }
        }

        public UInt32 KeepAliveTime
        {
            get
            {
                return this.ToUInt32("KeepAliveTime");
            }
        }

        public String MACAddress
        {
            get
            {
                return this.ToString("MACAddress");
            }
        }

        public UInt32 MTU
        {
            get
            {
                return this.ToUInt32("MTU");
            }
        }

        public UInt32 NumForwardPackets
        {
            get
            {
                return this.ToUInt32("NumForwardPackets");
            }
        }

        public Boolean PMTUBHDetectEnabled
        {
            get
            {
                return this.ToBoolean("PMTUBHDetectEnabled");
            }
        }

        public Boolean PMTUDiscoveryEnabled
        {
            get
            {
                return this.ToBoolean("PMTUDiscoveryEnabled");
            }
        }

        public String ServiceName
        {
            get
            {
                return this.ToString("ServiceName");
            }
        }

        public String SettingID
        {
            get
            {
                return this.ToString("SettingID");
            }
        }

        public UInt32 TcpipNetbiosOptions
        {
            get
            {
                return this.ToUInt32("TcpipNetbiosOptions");
            }
        }

        public UInt32 TcpMaxConnectRetransmissions
        {
            get
            {
                return this.ToUInt32("TcpMaxConnectRetransmissions");
            }
        }

        public UInt32 TcpMaxDataRetransmissions
        {
            get
            {
                return this.ToUInt32("TcpMaxDataRetransmissions");
            }
        }

        public UInt32 TcpNumConnections
        {
            get
            {
                return this.ToUInt32("TcpNumConnections");
            }
        }

        public Boolean TcpUseRFC1122UrgentPointer
        {
            get
            {
                return this.ToBoolean("TcpUseRFC1122UrgentPointer");
            }
        }

        public UInt16 TcpWindowSize
        {
            get
            {
                return this.ToUInt16("TcpWindowSize");
            }
        }

        public Boolean WINSEnableLMHostsLookup
        {
            get
            {
                return this.ToBoolean("WINSEnableLMHostsLookup");
            }
        }

        public String WINSHostLookupFile
        {
            get
            {
                return this.ToString("WINSHostLookupFile");
            }
        }

        public String WINSPrimaryServer
        {
            get
            {
                return this.ToString("WINSPrimaryServer");
            }
        }

        public String WINSScopeID
        {
            get
            {
                return this.ToString("WINSScopeID");
            }
        }

        public String WINSSecondaryServer
        {
            get
            {
                return this.ToString("WINSSecondaryServer");
            }
        }

#endregion

        #region Private methods developer made
        
        private String ToString(String property)
        {
            try
            {
                object value = this.PropertyData.Properties[property].Value;
                return (value == null) ? String.Empty : value.ToString();
            }
            catch (Exception ex)
            {
                Logging.Log.Error("", ex);
                return String.Empty;
            }
        }

        private Boolean ToBoolean(String property)
        {
            try
            {
                object value = this.PropertyData.Properties[property].Value;
                return (value == null) ? false : Convert.ToBoolean(value);
            }
            catch (Exception ex)
            {
                Logging.Log.Error("", ex);
                return false;
            }
        }

        private String[] ToStringArray(String property)
        {
            try
            {
                object value = this.PropertyData.Properties[property].Value;
                return (value == null) ? new String[] { } : (String[])value;
            }
            catch (Exception ex)
            {
                Logging.Log.Error("", ex);
                return new String[] { };
            }
        }

        private Byte ToByte(String property)
        {
            try
            {
                object value = this.PropertyData.Properties[property].Value;
                return (value == null) ? Convert.ToByte(0) : Convert.ToByte(value);
            }
            catch (Exception ex)
            {
                Logging.Log.Error("", ex);
                return Convert.ToByte(0);
            }
        }

        private DateTime ToDateTime(String property)
        {
            try
            {
                object value = this.PropertyData.Properties[property].Value;
                return (value == null) ? DateTime.MinValue : this.GetDateTime(value.ToString());
            }
            catch (Exception ex)
            {
                Logging.Log.Error("", ex);
                return DateTime.MinValue;
            }
        }

        private UInt16 ToUInt16(String property)
        {
            try
            {
                object value = this.PropertyData.Properties[property].Value;
                return (value == null) ? Convert.ToUInt16(0) : Convert.ToUInt16(value);
            }
            catch (Exception ex)
            {
                Logging.Log.Error("", ex);
                return Convert.ToUInt16(0);
            }
        }

        private UInt16[] ToUInt16Array(String property)
        {
            try
            {
                object value = this.PropertyData.Properties[property].Value;
                return (value == null) ? new UInt16[] { } : (UInt16[])value;
            }
            catch (Exception ex)
            {
                Logging.Log.Error("", ex);
                return new UInt16[] { };
            }
        }

        private UInt32 ToUInt32(String property)
        {
            try
            {
                object value = this.PropertyData.Properties[property].Value;
                return (value == null) ? 0 : Convert.ToUInt32(value);
            }
            catch (Exception ex)
            {
                Logging.Log.Error("", ex);
                return 0;
            }
        }

        private static String ToStringList(String[] stringArray)
        {
            Int32 x = 1;
            StringBuilder sb = new StringBuilder();
            foreach (String s in stringArray)
            {
                sb.Append(s);
                if (x < stringArray.Length)
                    sb.Append(",");

                x++;
            }

            return sb.ToString();
        }

        private static String ToStringList(UInt16[] stringArray)
        {
            Int32 x = 1;
            StringBuilder sb = new StringBuilder();
            foreach (UInt16 s in stringArray)
            {
                sb.Append(s);
                if (x < stringArray.Length)
                    sb.Append(",");

                x++;
            }

            return sb.ToString();
        }

        // There is a utility called mgmtclassgen that ships with the .NET SDK that
        // will generate managed code for existing WMI classes. It also generates
        // datetime conversion routines like this one.
        // Thanks to Chetan Parmar and dotnet247.com for the help.
        private DateTime GetDateTime(String dmtfDate)
        {
            Int32 year = DateTime.Now.Year;
            Int32 month = 1;
            Int32 day = 1;
            Int32 hour = 0;
            Int32 minute = 0;
            Int32 second = 0;
            Int32 millisec = 0;
            String dmtf = dmtfDate;
            String tempString = String.Empty;

            if (String.IsNullOrEmpty(dmtf))
                return DateTime.MinValue;

            if ((dmtf.Length != 25))
                return DateTime.MinValue;

            tempString = dmtf.Substring(0, 4);
            if (("****" != tempString))
                year = Int32.Parse(tempString);

            tempString = dmtf.Substring(4, 2);
            if (("**" != tempString))
                month = Int32.Parse(tempString);

            tempString = dmtf.Substring(6, 2);
            if (("**" != tempString))
                day = Int32.Parse(tempString);

            tempString = dmtf.Substring(8, 2);
            if (("**" != tempString))
                hour = Int32.Parse(tempString);

            tempString = dmtf.Substring(10, 2);
            if (("**" != tempString))
                minute = Int32.Parse(tempString);

            tempString = dmtf.Substring(12, 2);
            if (("**" != tempString))
                second = Int32.Parse(tempString);

            tempString = dmtf.Substring(15, 3);
            if (("***" != tempString))
                millisec = Int32.Parse(tempString);

            DateTime dateRet = new DateTime(year, month, day, hour, minute, second, millisec);
            return dateRet;
        }

        #endregion
    }
}
