using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Terminals.CommandLine;
using Terminals.Configuration;
using Unified;
using Unified.Rss;

namespace Terminals.Updates
{
    internal class UpdateManager
    {
        /// <summary>
        /// Check for available application updates
        /// </summary>
        internal static void CheckForUpdates(CommandLineArgs commandLine)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(PerformCheck), commandLine);
        }

        private static string GetMD5HashFromFile(string file_name)
        {
            String tmpFile = file_name + ".tmp";
            File.Copy(file_name, tmpFile, true);
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

            return null;
        }

        /// <summary>
        /// check codeplex's rss feed to see if we have a new release available.
        /// </summary>
        private static void CheckForCodeplexRelease()
        {
            Boolean checkForUpdate = true;
            String releaseFile = FileLocations.GetFullPath("LastUpdateCheck.txt");
            if (File.Exists(releaseFile))
            {
                String text = File.ReadAllText(releaseFile).Trim();
                if (text != String.Empty)
                {
                    DateTime lastUpdate = DateTime.MinValue;
                    if (DateTime.TryParse(text, out lastUpdate))
                    {   //dont run the update if the file is today or later..if we have checked today or not
                        if(lastUpdate.Date >= DateTime.Now.Date) 
                        {
                            checkForUpdate = false;
                        }
                    }
                }
            }

            if (checkForUpdate)
            {
                RssFeed feed = RssFeed.Read(String.Format("{0}/project/feeds/rss?ProjectRSSFeed=codeplex%3A%2F%2Frelease%2FTerminals&ProjectName=terminals", Program.Resources.GetString("TerminalsURL")));
                if (feed != null)
                {
                    Boolean needsUpdate = false;
                    foreach (RssChannel chan in feed.Channels)
                    {
                        foreach (RssItem item in chan.Items)
                        {
                            //check the date the item was published.  
                            //Is it after the currently executing application BuildDate? if so, then its probably a new build!
                            if (item.PubDate > Program.Info.BuildDate)  
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
                bool autoUpdate = (state as CommandLineArgs).AutomaticallyUpdate;
                if (autoUpdate)
                {
                    {
                        String url = Settings.UpdateSource;
                        String contents = Download(url);

                        if (!String.IsNullOrEmpty(contents))
                        {
                            TerminalsUpdates updates = (TerminalsUpdates)Serialize.DeSerializeXML(contents, typeof(TerminalsUpdates));
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
                                            FastZip fz = new FastZip();
                                            fz.ExtractZip(filename, finalFolder, null);

                                            if (MessageBox.Show("A new build is available, would you like to install it now", "New Build", MessageBoxButtons.OKCancel) == DialogResult.OK) {

                                                DirectoryInfo parent = FindFileInFolder(new DirectoryInfo(finalFolder), "Terminals.exe");
                                                if (parent == null)
                                                    return;

                                                finalFolder = parent.FullName;

                                                File.Copy(Settings.CONFIG_FILE_NAME, Path.Combine(finalFolder, Settings.CONFIG_FILE_NAME), true);
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
                                                    String args = String.Format("\"{0}\" \"{1}\"", finalFolder, Program.Info.Location);
                                                    System.Diagnostics.Process.Start(updaterExe, args);
                                                    Application.Exit();
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