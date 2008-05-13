using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;

namespace Terminals.Integration.Import {
    public class ImportvRD : IImport {
        #region IImport Members

        public FavoriteConfigurationElementCollection ImportFavorites(string Filename) {
            FavoriteConfigurationElementCollection fav = null;
            InputBoxResult result = InputBox.Show("Password", "vRD Password", '*');

            if(result.ReturnCode == System.Windows.Forms.DialogResult.OK) {
                byte[] file = System.IO.File.ReadAllBytes(Filename);
                string xml = ImportvRD.a(file, result.Text).Replace(" encoding=\"utf-16\"","");
                byte[] data = System.Text.ASCIIEncoding.Default.GetBytes(xml);
                using(System.IO.MemoryStream sw = new MemoryStream(data)) {
                    if(sw.Position > 0 & sw.CanSeek) sw.Seek(0, SeekOrigin.Begin);
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(vRdImport.vRDConfigurationFile));
                    object results = x.Deserialize(sw);
                    if(results != null) {
                        vRdImport.vRDConfigurationFile config = (results as vRdImport.vRDConfigurationFile);

                    }
                }
            }

            return fav;
        }

        public string KnownExtension {
            get { return ".vrb"; }
        }

        #endregion


        #region ignore this stuff
        private static SymmetricAlgorithm a() {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            uint num = 0;
            if(!ah.GetTokenInformation(current.Token, ab.a, null, 0, ref num)) {
                int error = Marshal.GetLastWin32Error();
                if(error != 0x7a) {
                    throw new Win32Exception(error);
                }
            }
            byte[] buffer = new byte[num];
            if(!ah.GetTokenInformation(current.Token, ab.a, buffer, num, ref num)) {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            byte[] destinationArray = new byte[num - 8];
            Array.Copy(buffer, 8L, destinationArray, 0L, (long)(num - 8));
            byte[] buffer3 = new byte[0x10];
            Array.Copy(destinationArray, 0, buffer3, 0, 0x10);
            byte[] buffer4 = new byte[0x20];
            Array.Copy(destinationArray, destinationArray.Length - 0x10, buffer4, 0, 0x10);
            Array.Copy(destinationArray, destinationArray.Length - 0x10, buffer4, 0x10, 0x10);
            RijndaelManaged managed = new RijndaelManaged();
            managed.KeySize = 0x100;
            managed.IV = buffer3;
            managed.Key = buffer4;
            return managed;
        }

        private static SymmetricAlgorithm a(bool A_0, string Password) {

            byte[] bytes = new UnicodeEncoding().GetBytes(Password.PadRight(0x100, ' '));
            byte[] sourceArray = new SHA1Managed().ComputeHash(bytes);
            byte[] destinationArray = new byte[0x10];
            Array.Copy(sourceArray, 0, destinationArray, 0, 0x10);
            byte[] buffer4 = new byte[0x20];
            Array.Copy(sourceArray, 0, buffer4, 0, 20);
            Array.Copy(sourceArray, 0, buffer4, 20, 12);
            RijndaelManaged managed2 = new RijndaelManaged();
            managed2.KeySize = 0x100;
            managed2.IV = destinationArray;
            managed2.Key = buffer4;
            return managed2;
        }


        public static byte[] a(string A_0, string Password) {
            SymmetricAlgorithm algorithm = a(true, Password);
            if(algorithm == null) {
                return null;
            }
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] bytes = new UnicodeEncoding().GetBytes(A_0);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            stream.Position = 0L;
            byte[] buffer2 = stream.ToArray();
            stream2.Close();
            stream.Close();
            return buffer2;
        }

        public static string a(byte[] A_0, string Password) {
            SymmetricAlgorithm algorithm = a(false, Password);
            if(algorithm == null) {
                return "";
            }
            MemoryStream stream = new MemoryStream();
            stream.Write(A_0, 0, A_0.Length);
            stream.Position = 0L;
            CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            byte[] buffer = new byte[A_0.Length];
            int count = stream2.Read(buffer, 0, buffer.Length);
            stream2.Close();
            stream.Close();
            UnicodeEncoding encoding = new UnicodeEncoding();
            return encoding.GetString(buffer, 0, count);
        }

        public static byte[] b(string A_0) {
            SymmetricAlgorithm algorithm = a();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] bytes = new UnicodeEncoding().GetBytes(A_0.PadRight(0x100, ' '));
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            stream.Position = 0L;
            byte[] buffer2 = stream.ToArray();
            stream2.Close();
            stream.Close();
            return buffer2;
        }

        public static string b(byte[] A_0) {
            SymmetricAlgorithm algorithm = a();
            MemoryStream stream = new MemoryStream();
            stream.Write(A_0, 0, A_0.Length);
            stream.Position = 0L;
            CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            byte[] buffer = new byte[A_0.Length];
            int count = stream2.Read(buffer, 0, A_0.Length);
            stream2.Close();
            stream.Close();
            UnicodeEncoding encoding = new UnicodeEncoding();
            return encoding.GetString(buffer, 0, count).TrimEnd(new char[] { ' ' });
        }


    }
    public class ah {
        // Methods
        public ah() { }
        [DllImport("advapi32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool GetTokenInformation(IntPtr A_0, ab A_1, [Out] byte[] A_2, uint A_3, ref uint A_4);
    }
    public enum ab {
        a = 1,
        b = 2,
        c = 3,
        d = 4,
        e = 5,
        f = 6,
        g = 7,
        h = 8,
        i = 9,
        j = 10,
        k = 11,
        l = 12,
        m = 13,
        n = 14,
        o = 15,
        p = 0x10,
        q = 0x11
    }


        #endregion
}
