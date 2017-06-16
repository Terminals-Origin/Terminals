using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terminals.Properties;

namespace Terminals.Updates
{
    internal class UpdateManager
    {
        private readonly Func<string> readReleases;

        public UpdateManager() : this(DownloadReleases)
        {
        }

        internal UpdateManager(Func<string> readReleases)
        {
            this.readReleases = readReleases;
        }

        private static string DownloadReleases()
        {
            using (var client = new WebClient())
            {
                const string agent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident / 6.0)";
                client.Headers.Add("Accept", "application/json");
                client.Headers.Add("User-Agent", agent);
                return client.DownloadString(Settings.Default.ReleasesUrl);
            }
        }

        /// <summary>
        /// Check for available application updates.
        /// </summary>
        internal Task<ReleaseInfo> CheckForUpdates(bool automaticallyUpdate)
        {
            return Task<ReleaseInfo>.Factory.StartNew(autoUpdate => this.PerformCheck((bool)autoUpdate), automaticallyUpdate);
        }

        private ReleaseInfo PerformCheck(bool automaticallyUpdate)
        {
            ReleaseInfo downLoaded = this.CheckForPublishedRelease(Program.Info.Version);

            // todo the automatic updates point to wrong URL this feature is not working
            bool autoUpdate = automaticallyUpdate; // obtain from command line arguments
            if (autoUpdate)
            {
                // DownloadLatestRelease();
            }

            return downLoaded;
        }

        internal ReleaseInfo CheckForPublishedRelease(Version currentVersion)
        {
            try
            {
                return this.TryCheckForPublishedRelease(currentVersion);
            }
            catch (Exception exception)
            {
                Logging.Error("Failed during Check for release.", exception);
                return ReleaseInfo.NotAvailable;
            }
        }

        /// <summary>
        /// check codeplex's rss feed to see if we have a new release available.
        /// Returns not null info about obtained current release.
        /// ReleaseInfo.NotAvailable in a case, new version was not checked or current version is the latest.
        /// </summary>
        private ReleaseInfo TryCheckForPublishedRelease(Version currentVersion)
        {
            var checksFile = new UpdateChecksFile();
            if (!checksFile.ShouldCheckForUpdate)
                return ReleaseInfo.NotAvailable;

            ReleaseInfo downLoaded = this.DownLoadLatestReleaseInfo(currentVersion);
            checksFile.WriteLastCheck();
            return downLoaded;
        }

        private ReleaseInfo DownLoadLatestReleaseInfo(Version currentVersion)
        {
            string downloaded = this.readReleases();
            Release[] feed = JsonConvert.DeserializeObject<Release[]>(downloaded);

            if (feed != null)
            {
                Release newvestRssItem = SelectNewvestRssItem(feed, currentVersion);
                if (newvestRssItem != null)
                    return new ReleaseInfo(newvestRssItem.Published, newvestRssItem.Version.ToString());
            }

            return ReleaseInfo.NotAvailable;
        }

        private static Release SelectNewvestRssItem(Release[] feed, Version currentVersion)
        {
            return feed.Where(item => item.Version > currentVersion)
                       .OrderByDescending(selected => selected.Version)
                       .FirstOrDefault();
        }
    }
}