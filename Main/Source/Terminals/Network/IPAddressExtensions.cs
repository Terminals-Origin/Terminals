using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace Terminals.Network
{
    internal static class IPAddressExtensions
    {
        #region Public methods
        
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            Byte[] ipAdressBytes = address.GetAddressBytes();
            Byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            Byte[] broadcastAddress = new Byte[ipAdressBytes.Length];
            for (Int32 i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (Byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }

            return new IPAddress(broadcastAddress);
        }

        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            Byte[] ipAdressBytes = address.GetAddressBytes();
            Byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            Byte[] broadcastAddress = new Byte[ipAdressBytes.Length];
            for (Int32 i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (Byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }

            return new IPAddress(broadcastAddress);
        }

        public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }

        /// <summary>
        /// Get all IP addresses between a start and an end IP address
        /// </summary>
        /// <param name="startIP">Starting IP address.</param>
        /// <param name="endIP">Ending IP address</param>
        /// <returns>IP address collection.</returns>
        /// <example>
        /// foreach (String address in GetIPRange(startIP, endIP))
        ///     Console.WriteLine(address);
        /// </example>
        public static IEnumerable<IPAddress> GetIPRange(IPAddress startIP, IPAddress endIP)
        {
            UInt32 sIP = IpToUint(startIP.GetAddressBytes());
            UInt32 eIP = IpToUint(endIP.GetAddressBytes());

            while (sIP <= eIP)
            {
                yield return new IPAddress(ReverseBytesArray(sIP));
                sIP++;
            }
        }

        /// <summary>
        /// Get the next IP address, adding one hop.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static IPAddress GetNextIPAddress(IPAddress address)
        {
            UInt32 sIP = IpToUint(address.GetAddressBytes());
            sIP++;
            return new IPAddress(ReverseBytesArray(sIP));
        }

        /// <summary>
        /// Get the previous IP address, substracting one hop.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static IPAddress GetPreviousIPAddress(IPAddress address)
        {
            UInt32 sIP = IpToUint(address.GetAddressBytes());
            sIP--;
            return new IPAddress(ReverseBytesArray(sIP));
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Reverse byte order in array
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static uint ReverseBytesArray(UInt32 ip)
        {
            Byte[] bytes = BitConverter.GetBytes(ip);
            bytes = bytes.Reverse().ToArray();
            return (UInt32)BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Convert bytes array to 32 bit long value
        /// </summary>
        /// <param name="ipBytes"></param>
        /// <returns></returns>
        private static UInt32 IpToUint(IEnumerable<Byte> ipBytes)
        {
            ByteConverter bConvert = new ByteConverter();
            UInt32 ipUint = 0;

            Int32 shift = 24; // indicates number of bits left for shifting
            foreach (Byte b in ipBytes)
            {
                if (ipUint == 0)
                {
                    object convertTo = bConvert.ConvertTo(b, typeof(UInt32));
                    if (convertTo != null)
                        ipUint = (UInt32)convertTo << shift;

                    shift -= 8;
                    continue;
                }

                if (shift >= 8)
                {
                    object convertTo = bConvert.ConvertTo(b, typeof(UInt32));
                    if (convertTo != null)
                        ipUint += (UInt32)convertTo << shift;
                }
                else
                {
                    object to = bConvert.ConvertTo(b, typeof(UInt32));
                    if (to != null)
                        ipUint += (UInt32)to;
                }

                shift -= 8;
            }

            return ipUint;
        }

        #endregion
    }
}
