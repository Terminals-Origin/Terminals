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
using Terminals.Credentials;
using vRdImport;

namespace Terminals.Integration.Import
{
    public class ImportvRD : IImport
    {
        #region IImport Members

        public FavoriteConfigurationElementCollection ImportFavorites(string Filename)
        {
            FavoriteConfigurationElementCollection fav = null;
            InputBoxResult result = InputBox.Show("Password", "vRD Password", '*');

            if (result.ReturnCode == System.Windows.Forms.DialogResult.OK)
            {
                byte[] file = System.IO.File.ReadAllBytes(Filename);
                string xml = ImportvRD.a(file, result.Text).Replace(" encoding=\"utf-16\"", "");
                byte[] data = System.Text.ASCIIEncoding.Default.GetBytes(xml);
                using (System.IO.MemoryStream sw = new MemoryStream(data))
                {
                    if (sw.Position > 0 & sw.CanSeek) sw.Seek(0, SeekOrigin.Begin);
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(vRdImport.vRDConfigurationFile));
                    object results = x.Deserialize(sw);

                    List<vRdImport.Connection> connections = new List<vRdImport.Connection>();
                    List<vRdImport.vRDConfigurationFileConnectionsFolder> folders = new List<vRdImport.vRDConfigurationFileConnectionsFolder>();
                    Dictionary<string, vRdImport.vRDConfigurationFileCredentialsFolderCredentials> credentials = new Dictionary<string, vRdImport.vRDConfigurationFileCredentialsFolderCredentials>();

                    if (results == null)
                    {
                        return fav;
                    }
                    vRdImport.vRDConfigurationFile config = (results as vRdImport.vRDConfigurationFile);
                    if (config == null)
                    {
                        return fav;
                    }
                    //scan in config item
                    foreach (object item in config.Items)
                    {
                        if (item == null)
                        {
                            continue;
                        }
                        if (item is vRdImport.vRDConfigurationFileCredentialsFolder)
                        {
                            //scan in all credentials into a dictionary
                            vRdImport.vRDConfigurationFileCredentialsFolder credentialFolder = (item as vRdImport.vRDConfigurationFileCredentialsFolder);
                            if (credentialFolder != null && credentialFolder.Credentials != null)
                            {
                                foreach (vRdImport.vRDConfigurationFileCredentialsFolderCredentials cred in credentialFolder.Credentials)
                                {
                                    credentials.Add(cred.Guid, cred);
                                }
                            }
                        }
                        else if (item is vRdImport.vRDConfigurationFileCredentialsFolderCredentials)
                        {
                            vRdImport.vRDConfigurationFileCredentialsFolderCredentials cred = (item as vRdImport.vRDConfigurationFileCredentialsFolderCredentials);
                            credentials.Add(cred.Guid, cred);
                        }
                        else if (item is vRdImport.Connection)
                        {
                            //scan in the connections
                            vRdImport.Connection connection = (item as vRdImport.Connection);
                            if (connection != null)
                            {
                                connections.Add(connection);
                            }
                        }
                        else if (item is vRdImport.vRDConfigurationFileConnectionsFolder)
                        {
                            //scan in recurse folders
                            vRdImport.vRDConfigurationFileConnectionsFolder folder = (item as vRdImport.vRDConfigurationFileConnectionsFolder);
                            if (folder != null)
                            {
                                folders.Add(folder);
                            }
                        }
                    }
                    //save credential item to local
                    SaveCredentials(credentials);
                    //save VRD connection to local
                    fav = ConvertVRDConnectionCollectionToLocal(connections.ToArray(), folders.ToArray(), null, String.Empty, credentials, fav);
                }
            }
            return fav;
        }
        #region Convert vrd confirguration to local configuration

        private void SaveCredentials(Dictionary<string, vRdImport.vRDConfigurationFileCredentialsFolderCredentials> credentials)
        {
            List<CredentialSet> list = Settings.SavedCredentials;
            if (list == null)
            {
                list = new List<CredentialSet>();
            }
            foreach (string guid in credentials.Keys)
            {
                CredentialSet set = new CredentialSet();
                set.Domain = credentials[guid].Domain;
                set.Name = credentials[guid].Name;
                set.Password = credentials[guid].Password;
                set.Username = credentials[guid].UserName;
                //will store the last one if the same credential name
                CredentialSet foundSet = null;
                foreach (CredentialSet item in list)
                {
                    if (item.Name.ToLower() == set.Name.ToLower())
                    {
                        foundSet = item;
                        break;
                    }
                }
                if (foundSet != null)
                {
                    list.Remove(foundSet);
                }
                list.Add(set);
            }
            Settings.SavedCredentials = list;
        }

        private FavoriteConfigurationElementCollection ConvertVRDConnectionCollectionToLocal(vRdImport.Connection[] connections, vRDConfigurationFileConnectionsFolder[] folders, vRDConfigurationFileConnectionsFolderFolder[] subFolders, String connectionTag, Dictionary<string, vRdImport.vRDConfigurationFileCredentialsFolderCredentials> credentials, FavoriteConfigurationElementCollection coll)
        {
            if (coll == null)
            {
                coll = new FavoriteConfigurationElementCollection();
            }
            //covert vrd connection
            if (connections != null && connections.Length > 0)
            {
                foreach (vRdImport.Connection con in connections)
                {
                    FavoriteConfigurationElement fav = ConvertVRDConnectionToLocal(credentials, con);
                    if (connectionTag != null && connectionTag != String.Empty && !fav.TagList.Contains(connectionTag))
                    {
                        fav.TagList.Add(connectionTag);
                        fav.Tags = connectionTag;
                    }
                    coll.Add(fav);
                }
            }
            //get connection object from root folder
            if (folders != null && folders.Length > 0)
            {
                foreach (vRdImport.vRDConfigurationFileConnectionsFolder folder in folders)
                {
                    ConvertVRDConnectionCollectionToLocal(folder.Connection, null, folder.Folder, folder.Name, credentials, coll);
                }
            }
            //get connection object from sub folder
            if (subFolders != null && subFolders.Length > 0)
            {
                foreach (vRDConfigurationFileConnectionsFolderFolder folder in subFolders)
                {
                    ConvertVRDConnectionCollectionToLocal(folder.Connection, null, null, connectionTag + folder.Name, credentials, coll);
                }
            }
            return coll;
        }

        private static FavoriteConfigurationElement ConvertVRDConnectionToLocal(Dictionary<string, vRdImport.vRDConfigurationFileCredentialsFolderCredentials> credentials, vRdImport.Connection con)
        {
            FavoriteConfigurationElement fav = new FavoriteConfigurationElement();

            fav.ServerName = con.ServerName;

            int p = 3389;
            int.TryParse(con.Port, out p);
            fav.Port = p;

            if (credentials.ContainsKey(con.Credentials))
            {
                fav.Credential = credentials[con.Credentials].Name;
                fav.UserName = credentials[con.Credentials].UserName;
                fav.DomainName = credentials[con.Credentials].Domain;
                fav.Password = credentials[con.Credentials].Password;
            }

            switch (con.ColorDepth)
            {
                case "8":
                    fav.Colors = Colors.Bits8;
                    break;
                case "16":
                    fav.Colors = Colors.Bit16;
                    break;
                case "24":
                    fav.Colors = Colors.Bits24;
                    break;
                case "32":
                    fav.Colors = Colors.Bits32;
                    break;
                default:
                    fav.Colors = Colors.Bit16;
                    break;
            };

            fav.DesktopSize = DesktopSize.AutoScale;
            if (con.SeparateWindow == "true") fav.DesktopSize = DesktopSize.FullScreen;

            fav.ConnectToConsole = false;
            if (con.Console == "true") fav.ConnectToConsole = true;

            fav.DisableWallPaper = false;
            if (con.BitmapCaching == "false") fav.DisableWallPaper = true;

            fav.RedirectSmartCards = false;
            if (con.SmartCard == "true") fav.RedirectSmartCards = true;

            fav.RedirectDrives = false;
            if (con.LocalDrives == "true") fav.RedirectDrives = true;

            fav.RedirectPorts = false;
            //if (pValue == "1") fav.RedirectPorts = true;

            fav.RedirectPrinters = false;
            if (con.Printer == "true") fav.RedirectPrinters = true;

            if (con.Audio == "0") fav.Sounds = RemoteSounds.Redirect;
            if (con.Audio == "1") fav.Sounds = RemoteSounds.PlayOnServer;
            if (con.Audio == "2") fav.Sounds = RemoteSounds.DontPlay;

            fav.Name = con.Name;

            return fav;
        }

        #endregion

        private void ImportConnection(vRdImport.Connection Connection, List<string> FolderNames, Dictionary<string, vRdImport.vRDConfigurationFileCredentialsFolderCredentials> Credentials)
        {
        }
        private void ImportFolder(vRdImport.vRDConfigurationFileConnectionsFolder Folder, List<string> FolderNames, Dictionary<string, vRdImport.vRDConfigurationFileCredentialsFolderCredentials> Credentials)
        {
            foreach (vRdImport.Connection conn in Folder.Connection)
            {
                ImportConnection(conn, FolderNames, Credentials);
            }
            foreach (vRdImport.vRDConfigurationFileConnectionsFolderFolder folder in Folder.Folder)
            {
                FolderNames.Add(folder.Name);
            }
        }

        public string KnownExtension
        {
            get { return ".vrb"; }
        }

        #endregion


        #region ignore this stuff
        private static SymmetricAlgorithm a()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            uint num = 0;
            if (!ah.GetTokenInformation(current.Token, ab.a, null, 0, ref num))
            {
                int error = Marshal.GetLastWin32Error();
                if (error != 0x7a)
                {
                    throw new Win32Exception(error);
                }
            }
            byte[] buffer = new byte[num];
            if (!ah.GetTokenInformation(current.Token, ab.a, buffer, num, ref num))
            {
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

        private static SymmetricAlgorithm a(bool A_0, string Password)
        {

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


        public static byte[] a(string A_0, string Password)
        {
            SymmetricAlgorithm algorithm = a(true, Password);
            if (algorithm == null)
            {
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

        public static string a(byte[] A_0, string Password)
        {
            SymmetricAlgorithm algorithm = a(false, Password);
            if (algorithm == null)
            {
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

        public static byte[] b(string A_0)
        {
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

        public static string b(byte[] A_0)
        {
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
    public class ah
    {
        // Methods
        public ah() { }
        [DllImport("advapi32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool GetTokenInformation(IntPtr A_0, ab A_1, [Out] byte[] A_2, uint A_3, ref uint A_4);
    }
    public enum ab
    {
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