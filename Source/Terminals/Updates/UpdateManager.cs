using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terminals.Properties;
using Unified.Rss;

namespace Terminals.Updates
{
    public class Release
    {
        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        [JsonProperty("name")]

        public string Name { get; set; }

        [JsonProperty("published_at")]

        public DateTime Published { get; set; }
    }

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
            return Task<ReleaseInfo>.Factory.StartNew((autoUpdate) => this.PerformCheck((bool)autoUpdate), automaticallyUpdate);
        }

        private ReleaseInfo PerformCheck(bool automaticallyUpdate)
        {
            ReleaseInfo downLoaded = this.CheckForCodeplexRelease(Program.Info.BuildDate);

            // todo the automatic updates point to wrong URL this feature is not working
            bool autoUpdate = automaticallyUpdate; // obtain from command line arguments
            if (autoUpdate)
            {
                // DownloadLatestRelease();
            }

            return downLoaded;
        }

        internal ReleaseInfo CheckForCodeplexRelease(DateTime buildDate)
        {
            try
            {
                return this.TryCheckForCodeplexRelease(buildDate);
            }
            catch (Exception exception)
            {
                Logging.Error("Failed during CheckForCodeplexRelease.", exception);
                return ReleaseInfo.NotAvailable;
            }
        }

        /// <summary>
        /// check codeplex's rss feed to see if we have a new release available.
        /// Returns not null info about obtained current release.
        /// ReleaseInfo.NotAvailable in a case, new version was not checked or current version is the latest.
        /// </summary>
        private ReleaseInfo TryCheckForCodeplexRelease(DateTime buildDate)
        {
            var checksFile = new UpdateChecksFile();
            if (!checksFile.ShouldCheckForUpdate)
                return ReleaseInfo.NotAvailable;

            ReleaseInfo downLoaded = this.DownLoadLatestReleaseInfo(buildDate);
            checksFile.WriteLastCheck();
            return downLoaded;
        }

        private ReleaseInfo DownLoadLatestReleaseInfo(DateTime buildDate)
        {
            string downloaded = this.readReleases();
            Release[] feed = JsonConvert.DeserializeObject<Release[]>(downloaded);

            if (feed != null)
            {
                Release newvestRssItem = SelectNewvestRssRssItem(feed, buildDate);
                if (newvestRssItem != null)
                    return new ReleaseInfo(newvestRssItem.Published, newvestRssItem.TagName);
            }

            return ReleaseInfo.NotAvailable;
        }

        private static Release SelectNewvestRssRssItem(Release[] feed, DateTime buildDate)
        {
            return feed.Where(item => IsNewerThanCurrent(item, buildDate))
                       .OrderByDescending(selected => selected.Published)
                       .FirstOrDefault();
        }

        /// <summary>
        /// Inteligent resolution of latest build by real "release date", not only by the date used to publish the feed.
        /// This filters the updates on release page after the release was published.
        /// Obtains the real date from the title.
        /// </summary>
        private static bool IsNewerThanCurrent(Release item, DateTime buildDate)
        {
            // rss item published date
            if (item.Published < buildDate)
                return false;

            const string DATE_FILTER = @"([^\(]+)\((?<Date>[^\(\)]+)\)"; // Select string iside brackets as "Date" group
            string titleDate = Regex.Match(item.Name, DATE_FILTER).Groups["Date"].Value;
            DateTime releaseDate;
            // there is no culture specification when downloading the feed, so it is always EN
            DateTime.TryParse(titleDate, out releaseDate);
            return releaseDate > buildDate;
        }
    }
}