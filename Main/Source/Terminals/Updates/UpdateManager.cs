using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Terminals.Configuration;
using Unified;
using Unified.Network.HTTP;
using Unified.Rss;

namespace Terminals.Updates
{
    internal class UpdateManager
    {
        /// <summary>
        /// Url to releases Rss feed, where the Terminals releases are published
        /// </summary>
        private const string RSS_URL = "http://terminals.codeplex.com/project/feeds/rss?ProjectRSSFeed=codeplex%3A%2F%2Frelease%2FTerminals&ProjectName=terminals";

        /// <summary>
        /// Check for available application updates.
        /// </summary>
        internal static Task<ReleaseInfo> CheckForUpdates(bool automaticallyUpdate)
        {
            return Task<ReleaseInfo>.Factory.StartNew((autoUpdate) => PerformCheck((bool)autoUpdate), automaticallyUpdate);
        }

        private static ReleaseInfo PerformCheck(bool automaticallyUpdate)
        {
            ReleaseInfo downLoaded = CheckForCodeplexRelease(Program.Info.BuildDate);

            // todo the automatic updates point to wrong URL this feature is not working
            bool autoUpdate = automaticallyUpdate; // obtain from command line arguments
            if (autoUpdate)
            {
                DownloadLatestRelease();
            }

            return downLoaded;
        }

        private static ReleaseInfo CheckForCodeplexRelease(DateTime buildDate)
        {
            try
            {
                return TryCheckForCodeplexRelease(buildDate);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Failed during CheckForCodeplexRelease.", exception);
                return ReleaseInfo.NotAvailable;
            }
        }

        /// <summary>
        /// check codeplex's rss feed to see if we have a new release available.
        /// Returns not null info about obtained current release.
        /// ReleaseInfo.NotAvailable in a case, new version was not checked or current version is the latest.
        /// </summary>
        private static ReleaseInfo TryCheckForCodeplexRelease(DateTime buildDate)
        {
            var checksFile = new UpdateChecksFile();
            if (!checksFile.ShouldCheckForUpdate)
                return ReleaseInfo.NotAvailable;

            ReleaseInfo downLoaded = DownLoadLatestReleaseInfo(buildDate);
            checksFile.WriteLastCheck();
            return downLoaded;
        }

        private static ReleaseInfo DownLoadLatestReleaseInfo(DateTime buildDate)
        {
            RssFeed feed = RssFeed.Read(RSS_URL);
            if (feed != null)
            {
                RssItem newvestRssItem = SelectNewvestRssRssItem(feed, buildDate);
                if (newvestRssItem != null)
                    return new ReleaseInfo(newvestRssItem.PubDate, newvestRssItem.Title);
            }

            return ReleaseInfo.NotAvailable;
        }

        private static RssItem SelectNewvestRssRssItem(RssFeed feed, DateTime buildDate)
        {
            return feed.Channels.OfType<RssChannel>()
                       .SelectMany(chanel => chanel.Items.OfType<RssItem>())
                       .Where(item => IsNewerThanCurrent(item, buildDate))
                       .OrderByDescending(selected => selected.PubDate)
                       .FirstOrDefault();
        }

        /// <summary>
        /// Inteligent resolution of latest build by real "release date", not only by the date used to publish the feed.
        /// This filters the updates on release page after the release was published.
        /// Obtains the real date from the title.
        /// </summary>
        private static bool IsNewerThanCurrent(RssItem item, DateTime buildDate)
        {
            // rss item published date
            if (item.PubDate < buildDate)
                return false;

            const string DATE_FILTER = @"([^\(]+)\((?<Date>[^\(\)]+)\)"; // Select string iside brackets as "Date" group
            string titleDate = Regex.Match(item.Title, DATE_FILTER).Groups["Date"].Value;
            DateTime releaseDate;
            // there is no culture specification when downloading the feed, so it is always EN
            DateTime.TryParse(titleDate, out releaseDate);
            return releaseDate > buildDate;
        }

        private static void DownloadLatestRelease()
        {
            try
            {
                String url = Settings.UpdateSource;
                String contents = Web.HTTPAsString(url);

                if (!String.IsNullOrEmpty(contents))
                {
                    TerminalsUpdates updates = (TerminalsUpdates)Serialize.DeSerializeXML(contents, typeof (TerminalsUpdates));
                    if (updates != null)
                    {
                        String currentMd5 = GetMd5HashFromFile("Terminals.exe");
                        if (currentMd5 != null)
                        {
                            //get the latest build
                            System.Data.DataRow row = updates.Tables[0].Rows[0];
                            String md5 = (row["MD5"] as string);
                            if (!md5.Equals(currentMd5))
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
                                    downloaded = Web.SaveHTTPToFile((row["Url"] as String), filename);

                                if (downloaded && File.Exists(filename))
                                {
                                    //ICSharpCode.SharpZipLib.Zip.FastZipEvents evts = new ICSharpCode.SharpZipLib.Zip.FastZipEvents();
                                    FastZip fz = new FastZip();
                                    fz.ExtractZip(filename, finalFolder, null);

                                    if (MessageBox.Show("A new build is available, would you like to install it now", "New Build", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                    {
                                        DirectoryInfo parent = FindFileInFolder(new DirectoryInfo(finalFolder), "Terminals.exe");
                                        if (parent == null)
                                            return;

                                        finalFolder = parent.FullName;

                                        File.Copy(FileLocations.CONFIG_FILENAME, Path.Combine(finalFolder, FileLocations.CONFIG_FILENAME), true);
                                        File.Copy("Terminals.log4net.config", Path.Combine(finalFolder, "Terminals.log4net.config"), true);

                                        String temp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                                        String updaterExe = Path.Combine(temp, "TerminalsUpdater.exe");
                                        if (File.Exists(Path.Combine(finalFolder, "TerminalsUpdater.exe")))
                                            File.Copy(Path.Combine(finalFolder, "TerminalsUpdater.exe"), updaterExe, true);

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
            catch (Exception exc)
            {
                Logging.Log.Error("Failed during update.", exc);
            }
        }

        private static DirectoryInfo FindFileInFolder(DirectoryInfo path, String filename)
        {
            if (path.GetFiles(filename).Length > 0)
                return path;

            foreach (DirectoryInfo dir in path.GetDirectories())
            {
                DirectoryInfo found = FindFileInFolder(dir, filename);
                if (found != null)
                    return found;
            }

            return null;
        }

        private static string GetMd5HashFromFile(string fileName)
        {
            String tmpFile = fileName + ".tmp";
            File.Copy(fileName, tmpFile, true);
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
    }
}