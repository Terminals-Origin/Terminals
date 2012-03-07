using System;
using System.Net;

namespace Terminals.Network
{
    internal static class SubnetMask
    {
        public static readonly IPAddress ClassA = IPAddress.Parse("255.0.0.0");
        public static readonly IPAddress ClassB = IPAddress.Parse("255.255.0.0");
        public static readonly IPAddress ClassC = IPAddress.Parse("255.255.255.0");

        public static IPAddress CreateByHostBitLength(Int32 hostpartLength)
        {
            Int32 hostPartLength = hostpartLength;
            Int32 netPartLength = 32 - hostPartLength;

            if (netPartLength < 2)
                throw new ArgumentException("Number of hosts is to large for IPv4");

            Byte[] binaryMask = new Byte[4];

            for (Int32 i = 0; i < 4; i++)
            {
                if (i * 8 + 8 <= netPartLength)
                {
                    binaryMask[i] = (byte) 255;
                }
                else if (i * 8 > netPartLength)
                {
                    binaryMask[i] = (byte) 0;
                }
                else
                {
                    Int32 oneLength = netPartLength - i * 8;
                    String binaryDigit = String.Empty.PadLeft(oneLength, '1').PadRight(8, '0');
                    binaryMask[i] = Convert.ToByte(binaryDigit, 2);
                }
            }

            return new IPAddress(binaryMask);
        }

        public static IPAddress CreateByNetBitLength(Int32 netpartLength)
        {
            Int32 hostPartLength = 32 - netpartLength;
            return CreateByHostBitLength(hostPartLength);
        }

        public static IPAddress CreateByHostNumber(Int32 numberOfHosts)
        {
            Int32 maxNumber = numberOfHosts + 1;
            String b = Convert.ToString(maxNumber, 2);
            return CreateByHostBitLength(b.Length);
        }
    }
}
