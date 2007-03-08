using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography; 

namespace Terminals
{
    internal static class Functions
    {
        internal static string DecryptPassword(string encryptedPassword)
        {
            if (String.IsNullOrEmpty(encryptedPassword))
                return encryptedPassword;

            try
            {
                return DPAPI.Decrypt(encryptedPassword);
            }
            catch
            {
                return "";
            }
        }

        internal static string EncryptPassword(string decryptedPassword)
        {
            return DPAPI.Encrypt(DPAPI.KeyType.UserKey, decryptedPassword);
        }

        internal static string UserDisplayName(string domain, string user)
        {
            return String.IsNullOrEmpty(domain) ? (user) : (domain + "\\" + user);
        }

        internal static string GetErrorMessage(int code)
        {
            //error messages from: http://msdn2.microsoft.com/en-us/aa382170.aspx
            switch (code)
            {
                case 260: return "DNS name lookup failure";
                case 262: return "Out of memory";
                case 264: return "Connection timed out";
                case 516: return "WinSock socket connect failure";
                case 518: return "Out of memory";
                case 520: return "Host not found error";
                case 772: return "WinSock send call failure";
                case 774: return "Out of memory";
                case 776: return "Invalid IP address specified";
                case 1028: return "WinSock recv call failure";
                case 1030: return "Invalid security data";
                case 1032: return "Internal error";
                case 1286: return "Invalid encryption method specified";
                case 1288: return "DNS lookup failed";
                case 1540: return "GetHostByName call failed";
                case 1542: return "Invalid server security data";
                case 1544: return "Internal timer error";
                case 1796: return "Time-out occurred";
                case 1798: return "Failed to unpack server certificate";
                case 2052: return "Bad IP address specified";
                case 2056: return "Internal security error";
                case 2308: return "Socket closed";
                case 2310: return "Internal security error";
                case 2312: return "Licensing time-out";
                case 2566: return "Internal security error";
                case 2822: return "Encryption error";
                case 3078: return "Decryption error";
            }
            return null;
        }
    }
}
