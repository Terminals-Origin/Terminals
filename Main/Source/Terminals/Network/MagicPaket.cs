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

        public enum ShutdownCommands
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

        /// <summary>
        /// Send a shutdown command to a (remote) computer.
        /// </summary>
        /// <param name="machineName">The machinename or ip-address of computer to send shutdown command to.</param>
        /// <param name="shutdownCommand">Shutdown type command.</param>
        /// <param name="credentials">Optional network credentials for the (remote) computer.</param>
        /// <returns>0 if the shutdown was succesfully send, else another integer value.</returns>
        /// <exception cref="ManagementException">An unhandled managed error occured.</exception>
        /// <exception cref="UnauthorizedAccessException">Access was denied.</exception>
        public static Int32 ForceShutdown(String machineName, ShutdownCommands shutdownCommand, NetworkCredential credentials = null)
        {
            Int32 result = -1;

            ConnectionOptions options = new ConnectionOptions();
            if (credentials != null)
            {
                options.EnablePrivileges = true;
                options.Username = (String.IsNullOrEmpty(credentials.Domain)) ? credentials.UserName : String.Format("{0}\\{1}", credentials.Domain, credentials.UserName);
                options.Password = credentials.Password;
            }

            ManagementScope scope = new ManagementScope(String.Format("\\\\{0}\\root\\cimv2", machineName), options);
            scope.Connect();

            SelectQuery query = new SelectQuery("Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            foreach (ManagementObject os in searcher.Get())
            {
                ManagementBaseObject inParams = os.GetMethodParameters("Win32Shutdown");
                inParams["Flags"] = (Int32)shutdownCommand;

                ManagementBaseObject outParams = os.InvokeMethod("Win32Shutdown", inParams, null);
                result = Convert.ToInt32(outParams.Properties["returnValue"].Value);

                return result;
            }

            return result;
        }

        //public static int ForceReboot(string MachineName, ShutdownStyles ShutdownStyle)
        //{

        //    int result = -1;
        //    try
        //    {

        //        ObjectGetOptions options = new ObjectGetOptions();
        //        ManagementClass WMI_W32_OS = new ManagementClass(string.Format(@"\\{0}\root\cimv2", MachineName), "Win32_OperatingSystem", options);
        //        ManagementBaseObject inputParams, outputParams;
        //        foreach(ManagementObject oneInstance in WMI_W32_OS.GetInstances())
        //        {
        //            inputParams = oneInstance.GetMethodParameters("Win32Shutdown");
        //            inputParams["Flags"] = (int)ShutdownStyle;
        //            inputParams["Reserved"] = 0;

        //            outputParams = oneInstance.InvokeMethod("Win32Shutdown", inputParams, null);

        //            result = Convert.ToInt32(outputParams["returnValue"]);
        //            break;
        //        }
        //    }
        //    catch(Exception exc)
        //    {
        //        Terminals.Logging.Log.Info("Terminals was not able to reboot the remote machine.", exc);
        //        System.Windows.Forms.MessageBox.Show("Terminals was not able to reboot the remote machine.  Reason:\r\n" + exc.Message);
        //    }
        //    return result;
        //}

    }
}
