using System;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Unified.Network.HTTP
{
    public class Web
    {
        /// <summary>
        /// Generic HTTP String Reader
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Data"></param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="Domain"></param>
        /// <param name="ProxyAddress"></param>
        /// <param name="ProxyPort"></param>
        /// <param name="DoPOST"></param>
        /// <returns></returns>
        public static string HTTPAsString(string URL, byte[] Data, string Username, string Password, string Domain, string ProxyAddress, int ProxyPort, bool DoPOST)
        {
            return System.Text.ASCIIEncoding.ASCII.GetString(HTTPAsBytes(URL, Data, Username, Password, Domain, ProxyAddress, ProxyPort, DoPOST));
        }

        public static string HTTPAsString(string URL)
        {
            return HTTPAsString(URL, null, string.Empty, string.Empty, string.Empty, string.Empty, 0, false);
        }

        public static System.Net.WebResponse HTTPAsWebResponse(string URL)
        {
            return HTTPAsWebResponse(URL, null, string.Empty, string.Empty, string.Empty, string.Empty, 0, false);
        }

        public static System.Net.WebResponse HTTPAsWebResponse(string URL, byte[] Data, string Username, string Password, string Domain, string ProxyAddress, int ProxyPort, bool DoPOST)
        {
            if (Terminals.Settings.UseProxy)
            {
                ProxyAddress = Terminals.Settings.ProxyAddress;
                ProxyPort = Terminals.Settings.ProxyPort;
            }

            if (!DoPOST && Data != null && Data.Length > 0)
            {
                string restoftheurl = System.Text.ASCIIEncoding.ASCII.GetString(Data);
                if (URL.IndexOf("?") <= 0) 
                    URL = URL + "?";

                URL = URL + restoftheurl;
            }

            System.Net.WebRequest wreq = System.Net.HttpWebRequest.Create(URL);
            wreq.Method = "GET";
            if (DoPOST) 
                wreq.Method = "POST";

            if (ProxyAddress != null && ProxyAddress.Trim() != string.Empty && ProxyPort > 0)
            {
                WebProxy webProxy = new WebProxy(ProxyAddress, ProxyPort);
                webProxy.BypassProxyOnLocal = true;
                wreq.Proxy = webProxy;
            }
            else
            {
                // wreq.Proxy = WebProxy.GetDefaultProxy();
                wreq.Proxy = WebRequest.DefaultWebProxy;
            }

            if (Username != null && Password != null && Domain != null && Username.Trim() != string.Empty && Password.Trim() != null && Domain.Trim() != null)
                wreq.Credentials = new System.Net.NetworkCredential(Username, Password, Domain);
            else if (Username != null && Password != null && Username.Trim() != string.Empty && Password.Trim() != null)
                wreq.Credentials = new System.Net.NetworkCredential(Username, Password);

            if (DoPOST && Data != null && Data.Length > 0)
            {
                wreq.ContentType = "application/x-www-form-urlencoded";
                System.IO.Stream request = wreq.GetRequestStream();
                request.Write(Data, 0, Data.Length);
                request.Close();
            }

            WebResponse wrsp = wreq.GetResponse();
            return wrsp;
        }

        /// <summary>
        /// its a default of 1024 bytes for the buffer because of download speeds and such
        /// some arbritray number which to buffer the downloaded content
        /// too high of a number and the responsestream cant keep up
        /// too low and the more loops, and ore time it takes to get the content
        /// if your constantly tearing the same URL down you may want to test 
        /// with different buffersizes for optimal performance
        /// with my tests 1024 (1kb/s) was optimal for text and binary data
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private static int BufferSize = 1024;

        public static byte[] ConvertWebResponseToByteArray(System.Net.WebResponse res)
        {
            System.IO.BinaryReader br = new BinaryReader(res.GetResponseStream());

            // Download and buffer the binary stream into a memory stream
            System.IO.MemoryStream stm = new MemoryStream();
            int pos = 0;
            int maxread = BufferSize;
            while (true)
            {
                byte[] content = br.ReadBytes(maxread);
                if (content.Length <= 0) 
                    break;

                if (content.Length < maxread)
                    maxread = maxread - content.Length;

                stm.Write(content, 0, content.Length);
                pos += content.Length;
            }

            br.Close();
            stm.Position = 0;
            byte[] final = new byte[(int)stm.Length];
            stm.Read(final, 0, final.Length);
            stm.Close();
            return final;
        }

        /// <summary>
        /// Generic HTTP Byte Array Reader
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Data"></param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="Domain"></param>
        /// <param name="ProxyAddress"></param>
        /// <param name="ProxyPort"></param>
        /// <param name="DoPOST"></param>
        /// <returns></returns>
        public static byte[] HTTPAsBytes(string URL, byte[] Data, string Username, string Password, string Domain, string ProxyAddress, int ProxyPort, bool DoPOST)
        {
            System.Net.WebResponse res = HTTPAsWebResponse(URL, Data, Username, Password, Domain, ProxyAddress, ProxyPort, DoPOST);
            return ConvertWebResponseToByteArray(res);
        }

        public static byte[] HTTPAsBytes(string URL)
        {
            return HTTPAsBytes(URL, null, string.Empty, string.Empty, string.Empty, string.Empty, 0, false);
        }

        /// <summary>
        /// Save content at any URL to disk, either with a POST or a GET
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Data"></param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="Domain"></param>
        /// <param name="Filename"></param>
        /// <param name="ProxyAddress"></param>
        /// <param name="ProxyPort"></param>
        /// <param name="DoPost"></param>
        /// <returns></returns>
        public static bool SaveHTTPToFile(string URL, byte[] Data, string Username, string Password, string Domain, string Filename, string ProxyAddress, int ProxyPort, bool DoPost)
        {
            byte[] data = HTTPAsBytes(URL, Data, Username, Password, Domain, ProxyAddress, ProxyPort, DoPost);
            if (data != null)
            {
                System.IO.FileStream fs = new FileStream(Filename, System.IO.FileMode.Create);
                fs.Write(data, 0, data.Length);
                fs.Close();
                return true;
            }

            return false;
        }

        public static bool SaveHTTPToFile(string URL, string Filename)
        {
            return SaveHTTPToFile(URL, null, string.Empty, string.Empty, string.Empty, Filename, string.Empty, 0, false);
        }

        /// <summary>
        /// Upload content to HTTP
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="Domain"></param>
        /// <param name="Filename"></param>
        /// <param name="ProxyAddress"></param>
        /// <param name="ProxyPort"></param>
        /// <param name="DoPost"></param>
        /// <returns></returns>
        public static bool SendFileToHTTP(string URL, string Username, string Password, string Domain, string Filename, string ProxyAddress, int ProxyPort, bool DoPost)
        {
            if (!System.IO.File.Exists(Filename)) 
                return false;

            System.IO.FileStream fs = new FileStream(Filename, System.IO.FileMode.Open);
            byte[] d = new byte[(int)fs.Length];
            fs.Read(d, 0, d.Length);
            fs.Close();
            if (d != null)
            {
                HTTPAsBytes(URL, d, Username, Password, Domain, ProxyAddress, ProxyPort, DoPost);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
