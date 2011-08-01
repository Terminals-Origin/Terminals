using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using Terminals.Configuration;

namespace Terminals.Updates
{
    public class UpdateManager
    {
        public static void CheckForUpdates()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(PerformCheck), null);
            //PerformCheck(null);
        }

        public static string GetMD5HashFromFile(string file_name)
        {
            String tmpFile = file_name + ".tmp";
            System.IO.File.Copy(file_name, tmpFile, true);
            Byte[] retVal = null;

            using (FileStream file = new FileStream(tmpFile, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                retVal = md5.ComputeHash(file);
                file.Close();
            }

            if (retVal != null)
            {
                StringBuilder s = new StringBuilder();
                foreach (Byte b in retVal)
                {
                    s.Append(b.ToString("x2").ToLower());
                }

                return s.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// check codeplex's rss feed to see if we have a new release available.
        /// </summary>
        private static void CheckForCodeplexRelease()
        {
            Boolean checkForUpdate = true;
            String releaseFile = "LastUpdateCheck.txt";
            if (File.Exists(releaseFile))
            {
                String text = System.IO.File.ReadAllText(releaseFile).Trim();
                if (text != String.Empty)
                {
                    DateTime lastUpdate = DateTime.MinValue;
                    if (DateTime.TryParse(text, out lastUpdate))
                    {
                        if(lastUpdate.Date >= DateTime.Now.Date) //dont run the update if the file is today or later..if we have checked today or not
                        {
                            checkForUpdate = false;
                        }
                    }
                }
            }

            if (checkForUpdate)
            {
                Unified.Rss.RssFeed feed = Unified.Rss.RssFeed.Read(String.Format("{0}/project/feeds/rss?ProjectRSSFeed=codeplex%3A%2F%2Frelease%2FTerminals&ProjectName=terminals", Program.Resources.GetString("TerminalsURL")));
                if (feed != null)
                {
                    Boolean needsUpdate = false;
                    foreach (Unified.Rss.RssChannel chan in feed.Channels)
                    {
                        foreach (Unified.Rss.RssItem item in chan.Items)
                        {
                            if (item.PubDate > Program.Info.BuildDate)  //check the date the item was published.  is it after the currently executing application BuildDate? if so, then its probably a new build!
                            {
                                MainForm.ReleaseAvailable = true;
                                MainForm.ReleaseDescription = item;
                                needsUpdate = true;
                                break;
                            }
                        }

                        if(needsUpdate) 
                            break;
                    }
                }
                
                File.WriteAllText(releaseFile, DateTime.Now.ToString());
            }
        }

        private static void PerformCheck(object state)
        {
            try
            {
                CheckForCodeplexRelease();
            }
            catch(Exception exc)
            {
                Logging.Log.Error("Failed during CheckForCodeplexRelease.", exc);
            }

            try
            {
                String autoUpdate = MainForm.CommandLineArgs.AutomaticallyUpdate;
                if (autoUpdate != null)
                {
                    Boolean update = false;
                    if (Boolean.TryParse(autoUpdate, out update) && update == true)
                    {
                        String url = Settings.UpdateSource;
                        String contents = Download(url);

                        if (!String.IsNullOrEmpty(contents))
                        {
                            TerminalsUpdates updates = (TerminalsUpdates)Unified.Serialize.DeSerializeXML(contents, typeof(TerminalsUpdates));
                            if (updates != null)
                            {
                                String currentMD5 = GetMD5HashFromFile("Terminals.exe");
                                if (currentMD5 != null)
                                {
                                    //get the latest build
                                    System.Data.DataRow row = updates.Tables[0].Rows[0];
                                    String md5 = (row["MD5"] as string);
                                    if (!md5.Equals(currentMD5))
                                    {
                                        String version = (row["version"] as String);
                                        if (!Directory.Exists("Builds")) 
                                            Directory.CreateDirectory("Builds");

                                        String finalFolder = @"Builds\" + version;
                                        if (!Directory.Exists(finalFolder)) 
                                            Directory.CreateDirectory(finalFolder);

                                        String filename = String.Format("{0}.zip", version);
                                        filename = @"Builds\" + filename;
                                        Boolean downloaded = true;

                                        if (!File.Exists(filename)) 
                                            downloaded = DownloadNewBuild((row["Url"] as String), filename);

                                        if (downloaded && File.Exists(filename))
                                        {
                                            //ICSharpCode.SharpZipLib.Zip.FastZipEvents evts = new ICSharpCode.SharpZipLib.Zip.FastZipEvents();
                                            ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
                                            fz.ExtractZip(filename, finalFolder, null);

                                            if (System.Windows.Forms.MessageBox.Show("A new build is available, would you like to install it now", "New Build", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK) {

                                                DirectoryInfo parent = FindFileInFolder(new DirectoryInfo(finalFolder), "Terminals.exe");
                                                if (parent == null)
                                                {
                                                    return;
                                                }
                                                else
                                                {
                                                    finalFolder = parent.FullName;
                                                }

                                                File.Copy("Terminals.config", Path.Combine(finalFolder, "Terminals.config"), true);
                                                //System.IO.File.Copy(Settings.ToolStripSettingsFile, System.IO.Path.Combine(finalFolder, Settings.ToolStripSettingsFile), true);
                                                File.Copy("Terminals.log4net.config", Path.Combine(finalFolder, "Terminals.log4net.config"), true);

                                                String temp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                                                String updaterExe = Path.Combine(temp, "TerminalsUpdater.exe");
                                                if (File.Exists(Path.Combine(finalFolder, "TerminalsUpdater.exe"))) {
                                                    File.Copy(Path.Combine(finalFolder, "TerminalsUpdater.exe"), updaterExe, true);
                                                }

                                                //updaterExe = @"C:\Source\Terminals\Terminals\bin\Debug\";

                                                if (File.Exists(updaterExe))
                                                {
                                                    //String args = "\"" + finalFolder + "\" \"" + Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\"";
                                                    String args = String.Format("\"{0}\" \"{1}\"", finalFolder, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
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
            }
            catch(Exception exc)
            {
                Logging.Log.Error("Failed during update.", exc);
            }
        }

        private static DirectoryInfo FindFileInFolder(DirectoryInfo Path, String Filename)
        {
            if (Path.GetFiles(Filename).Length > 0) 
                return Path;

            foreach (DirectoryInfo dir in Path.GetDirectories())
            {
                DirectoryInfo found = FindFileInFolder(dir, Filename);
                if (found != null) 
                    return found;
            }
            
            return null;
        }

        private static bool DownloadNewBuild(String Url, String Filename)
        {
            return Unified.Network.HTTP.Web.SaveHTTPToFile(Url, Filename);
        }

        private static string Download(String Url)
        {
            return Unified.Network.HTTP.Web.HTTPAsString(Url);
        }
    }
}