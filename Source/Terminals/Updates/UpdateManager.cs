using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
                // DownloadLatestRelease();
            }

            return downLoaded;
        }

        internal static ReleaseInfo CheckForCodeplexRelease(DateTime buildDate)
        {
            try
            {
                return TryCheckForCodeplexRelease(buildDate);
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
    }
}