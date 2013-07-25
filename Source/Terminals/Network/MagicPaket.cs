using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

/* (c)2003 M.Kruppa */
namespace NetTools
{
    public class MagicPacket
    {
        private const int HEADER = 6;
        private const int BYTELENGHT = 6;
        private const int MAGICPACKETLENGTH = 16;

        private System.Net.IPAddress wolIPAddr = System.Net.IPAddress.Broadcast;
        private int wolPortAddr = 7;
        private IPEndPoint wolEndPoint;				
        private byte[] wolMacAddr;
        private byte[] magicPacketPayload;

        public MagicPacket(String macAddress)
        {
            this.wolMacAddr = Mac2Byte(macAddress);
            this.magicPacketPayload = CreatePayload(wolMacAddr);
            this.wolEndPoint = new IPEndPoint(this.wolIPAddr, this.wolPortAddr);
        }
                
        public MagicPacket(String macAddress, String strPortAddress)
        {
            this.wolMacAddr = Mac2Byte(macAddress);
            this.magicPacketPayload = CreatePayload(this.wolMacAddr);
            this.wolPortAddr = Convert.ToInt16(strPortAddress, 10);
            this.wolEndPoint = new IPEndPoint(this.wolIPAddr, this.wolPortAddr);
        }
            
        public string macAddress 
        {
            get 
            {
                String strMacAdress = String.Empty;
                for (Int32 i = 0; i < this.wolMacAddr.Length; i++)
                {
                    strMacAdress += this.wolMacAddr[i].ToString("X2");
                }

                return strMacAdress;
            }
        }

        protected static Byte[] Mac2Byte(String strMacAddress)
        {
            String macAddr = String.Empty;
            Byte[] macBytes = new Byte[BYTELENGHT];
            //remove all non 0-9, A-F, a-f characters
            macAddr = Regex.Replace(strMacAddress, @"[^0-9A-Fa-f]", "");
            //check if it is now a valid mac adress
            if (!(macAddr.Length == BYTELENGHT*2))
                throw new ArgumentException("Mac Adress must be "+ (BYTELENGHT*2).ToString() +" digits of 0-9, A-F, a-f characters in length.");

            String hex;
            for (Int32 i=0; i < macBytes.Length;i++)
            {
                hex = new String(new Char[] {macAddr[i*2], macAddr[i*2+1]});
                macBytes[(i)] = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            }

            return macBytes;
        }
        
        protected static Byte[] CreatePayload(Byte[] macAddress)
        {
            Byte[] payloadData = new Byte[HEADER+MAGICPACKETLENGTH*BYTELENGHT];
            for (Int32 i=0; i < HEADER; i++) 
            {
                payloadData[i] = Byte.Parse("FF", System.Globalization.NumberStyles.HexNumber);
            }

            for (Int32 i=0; i < MAGICPACKETLENGTH; i++)
            {
                for (Int32 j=0;j < BYTELENGHT;j++)
                {
                    payloadData[((i * BYTELENGHT) + j) + HEADER] = macAddress[j];
                }
            }

            return payloadData;
        }
        
        public Int32 WakeUp()
        {
            return SendUDP(this.magicPacketPayload, this.wolEndPoint);
        }
        
        protected static Int32 SendUDP(Byte[] Payload, IPEndPoint EndPoint)
        {		
            Int32 byteSend;

            //create a new client socket...
            Socket socketClient = new Socket(EndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                //open connection...
                socketClient.Connect(EndPoint);
                //send MagicPacket(TM)...
                byteSend = socketClient.Send (Payload, 0, Payload.Length, SocketFlags.None);
            }
            finally
            {
                socketClient.Close();
            }

            return byteSend;
        }
    }
}
