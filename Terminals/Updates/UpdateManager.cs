using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace Terminals.Updates {
    public class UpdateManager {
        public static void CheckForUpdates() {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(PerformCheck), null);
            //PerformCheck(null);
        }
        public static string GetMD5HashFromFile(string file_name) {
            string tmpFile = file_name + ".tmp";
            System.IO.File.Copy(file_name, tmpFile, true);
            byte[] retVal = null;
            using(FileStream file = new FileStream(tmpFile, FileMode.Open)) {
                MD5 md5 = new MD5CryptoServiceProvider();
                retVal = md5.ComputeHash(file);
                file.Close();
            }
            if(retVal != null) {
                System.Text.StringBuilder s = new StringBuilder();
                foreach(byte b in retVal) {
                    s.Append(b.ToString("x2").ToLower());
                }
                return s.ToString();
            } else
                return null;
        }
        private static void PerformCheck(object state) {
            try {
                string autoUpdate = Terminals.MainForm.CommandLineArgs.AutomaticallyUpdate;
                if(autoUpdate != null) {
                    bool update = false;
                    if(bool.TryParse(autoUpdate, out update) && update == true) {
                        string url = Settings.UpdateSource;
                        string contents = Download(url);
                        if(!String.IsNullOrEmpty(contents)) {
                            TerminalsUpdates updates = (TerminalsUpdates)Unified.Serialize.DeSerializeXML(contents, typeof(TerminalsUpdates));
                            if(updates != null) {
                                string currentMD5 = GetMD5HashFromFile("Terminals.exe");
                                if(currentMD5 != null) {
                                    //get the latest build
                                    System.Data.DataRow row = updates.Tables[0].Rows[0];
                                    string md5 = (row["MD5"] as string);
                                    if(!md5.Equals(currentMD5)) {
                                        string version = (row["version"] as string);
                                        if(!System.IO.Directory.Exists("Builds")) System.IO.Directory.CreateDirectory("Builds");
                                        string finalFolder = @"Builds\" + version;
                                        if(!System.IO.Directory.Exists(finalFolder)) System.IO.Directory.CreateDirectory(finalFolder);
                                        string filename = string.Format("{0}.zip", version);
                                        filename = @"Builds\" + filename;
                                        bool downloaded = true;
                                        if(!System.IO.File.Exists(filename)) downloaded = DownloadNewBuild((row["Url"] as string), filename);
                                        if(downloaded && System.IO.File.Exists(filename)) {
                                            //ICSharpCode.SharpZipLib.Zip.FastZipEvents evts = new ICSharpCode.SharpZipLib.Zip.FastZipEvents();
                                            ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
                                            fz.ExtractZip(filename, finalFolder, null);
                                            if(System.Windows.Forms.MessageBox.Show("A new build is available, would you like to install it now", "New Build", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK) {

                                                System.IO.DirectoryInfo parent = FindFileInFolder(new DirectoryInfo(finalFolder), "Terminals.exe");
                                                if(parent == null) {
                                                    return;
                                                } else {
                                                    finalFolder = parent.FullName;
                                                }

                                                System.IO.File.Copy("Terminals.config", System.IO.Path.Combine(finalFolder, "Terminals.config"), true);
                                                System.IO.File.Copy("ToolStrip.settings", System.IO.Path.Combine(finalFolder, "ToolStrip.settings"), true);
                                                System.IO.File.Copy("Terminals.log4net.config", System.IO.Path.Combine(finalFolder, "Terminals.log4net.config"), true);

                                                string temp = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                                                string updaterExe = System.IO.Path.Combine(temp, "TerminalsUpdater.exe");
                                                if(System.IO.File.Exists(System.IO.Path.Combine(finalFolder, "TerminalsUpdater.exe"))) {
                                                    System.IO.File.Copy(System.IO.Path.Combine(finalFolder, "TerminalsUpdater.exe"), updaterExe, true);
                                                }
                                                //updaterExe = @"C:\Source\Terminals\Terminals\bin\Debug\";

                                                if(System.IO.File.Exists(updaterExe)) {
                                                    string args = "\"" + finalFolder + "\" \"" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\"";
                                                    System.Diagnostics.Process.Start(updaterExe, args);
                                                    System.Windows.Forms.Application.Exit();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch(Exception exc) {
                Terminals.Logging.Log.Error("Failed during update.", exc);
            }
        }
        
        
        private static DirectoryInfo FindFileInFolder(DirectoryInfo Path, string Filename) {
            if(Path.GetFiles(Filename).Length > 0) return Path;
            foreach(System.IO.DirectoryInfo dir in Path.GetDirectories()) {
                DirectoryInfo found = FindFileInFolder(dir, Filename);
                if(found!=null) return found;
            }
            return null;
        }
        private static bool DownloadNewBuild(string Url, string Filename) {
                       
            return Unified.Network.HTTP.Web.SaveHTTPToFile(Url, Filename);
        }

        private static string Download(string Url) {
            return Unified.Network.HTTP.Web.HTTPAsString(Url);
        }
    }
}
