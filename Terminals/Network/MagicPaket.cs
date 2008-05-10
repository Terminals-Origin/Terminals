using System;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Management;

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

		public MagicPacket(string macAddress)
		{		
			wolMacAddr = Mac2Byte(macAddress);
			magicPacketPayload = CreatePayload(wolMacAddr);
			wolEndPoint = new System.Net.IPEndPoint(wolIPAddr, wolPortAddr);
  		}
				
		public MagicPacket(string macAddress, string strPortAddress)
		{		
			wolMacAddr = Mac2Byte(macAddress);
			magicPacketPayload = CreatePayload(wolMacAddr);
			wolPortAddr = Convert.ToInt16(strPortAddress, 10);
			wolEndPoint = new System.Net.IPEndPoint(wolIPAddr, wolPortAddr);
		}
			
		public string macAddress 
		{
			get 
			{
				string strMacAdress = "";
				for (int i=0; i<wolMacAddr.Length; i++)
				{
					strMacAdress += wolMacAddr[i].ToString("X2");
				}
				return strMacAdress;
			}
		}

		protected static byte[] Mac2Byte(string strMacAddress)
		{
			string macAddr;
			byte[] macBytes = new byte[BYTELENGHT];
			//remove all non 0-9, A-F, a-f characters
			macAddr = Regex.Replace(strMacAddress, @"[^0-9A-Fa-f]", "");
			//check if it is now a valid mac adress
			if (!(macAddr.Length == BYTELENGHT*2))
				throw new ArgumentException("Mac Adress must be "+ (BYTELENGHT*2).ToString() +" digits of 0-9, A-F, a-f characters in length.");
			string hex;
			for (int i=0; i<macBytes.Length;i++)
			{
				hex = new String(new Char[] {macAddr[i*2], macAddr[i*2+1]});
				macBytes[(i)] = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
			}
			return macBytes;
		}
		
		protected static byte[] CreatePayload(byte[] macAddress) 
		{
			byte[] payloadData = new byte[HEADER+MAGICPACKETLENGTH*BYTELENGHT];
			for (int i=0; i<HEADER; i++) 
			{
				payloadData[i] = byte.Parse("FF", System.Globalization.NumberStyles.HexNumber);
			}
			for(int i=0; i<MAGICPACKETLENGTH; i++)
			{
				for(int j=0;j<BYTELENGHT;j++)
				{
					payloadData[((i*BYTELENGHT)+j)+HEADER] = macAddress[j];
				}
			}
			return payloadData;
		}
		
		public int WakeUp() {
			return SendUDP(magicPacketPayload, wolEndPoint);
		}
		
		protected static int SendUDP(byte[] Payload, IPEndPoint EndPoint)
		{		
			int byteSend;
			//create a new client socket...
			Socket socketClient = new Socket(EndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
			try
			{
				//open connection...
				socketClient.Connect(EndPoint);
				//send MagicPacket(TM)...
				byteSend = socketClient.Send (Payload, 0, Payload.Length, SocketFlags.None);
			}
			catch (SocketException ex)
			{
				throw ex;
			}
			finally
			{
				socketClient.Close();
			}
			return byteSend;
  		}
        public enum ShutdownStyles
        {
            LogOff = 0,
            ForcedLogOff = 4,
            Shutdown = 1,
            ForcedShutdown = 5,
            Reboot = 2,
            ForcedReboot = 6,
            PowerOff = 8,
            ForcedPowerOff = 12
        }

        public static int ForceReboot(string MachineName, ShutdownStyles ShutdownStyle)
        {

            int result = -1;
            try
            {

                ObjectGetOptions options = new ObjectGetOptions();
                ManagementClass WMI_W32_OS = new ManagementClass(string.Format(@"\\{0}\root\cimv2", MachineName), "Win32_OperatingSystem", options);
                ManagementBaseObject inputParams, outputParams;
                foreach(ManagementObject oneInstance in WMI_W32_OS.GetInstances())
                {
                    inputParams = oneInstance.GetMethodParameters("Win32Shutdown");
                    inputParams["Flags"] = (int)ShutdownStyle;
                    inputParams["Reserved"] = 0;

                    outputParams = oneInstance.InvokeMethod("Win32Shutdown", inputParams, null);

                    result = Convert.ToInt32(outputParams["returnValue"]);
                    break;
                }
            }
            catch(Exception exc)
            {
                System.Windows.Forms.MessageBox.Show("Terminals was not able to reboot the remote machine.  Reason:\r\n" + exc.Message);
            }
            return result;
        }

 	}
}
