using System;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Configuration;
using Terminals.Updates;
using Tests.FilePersisted;
using Unified.Rss;

namespace Tests
{
    /// <summary>
    /// Check for new release to be able parse release rss feeds, to be able to download and unpack the update package.
    /// Expected implementation of UpdateManager:
    /// - the update check file is written after all checks
    /// - release shouldn't be reported only, if there is a release with newer build date, than current
    /// - new release should be reported once per day only
    /// </summary>
    [TestClass]
    public class UpdateManagerTest
    {
        private DateTime buildDate = Program.Info.BuildDate;

        private readonly DateTime yesterDay = DateTime.Today.AddDays(-1);

        [TestInitialize]
        public void ConfigureTestLab()
        {
            FilePersistedTestLab.SetDefaultFileLocations();

            if (File.Exists(FileLocations.LastUpdateCheck))
                File.Delete(FileLocations.LastUpdateCheck);
        }

        /// <summary>
        /// in debug there is never a newer version, because it is checked by build date
        /// </summary>
        [TestMethod]
        public void CurrentBuild_CheckForCodeplexRelease_ReturnsNotAvailable()
        {
            ReleaseInfo checkResult = this.RunUpdateCheck();

            Assert.AreEqual(ReleaseInfo.NotAvailable, checkResult, "New release noticed");
            this.AssertLastUpdateCheck();
        }

        [TestMethod]
        public void OldestBuildDate_CheckForCodeplexRelease_ReturnsValidRelease()
        {
            this.buildDate = DateTime.MinValue;
            ReleaseInfo checkResult = this.RunUpdateCheck();

            Assert.AreNotEqual(ReleaseInfo.NotAvailable, checkResult, "Didn't notice new release");
            this.AssertLastUpdateCheck();
        }

        [TestMethod]
        public void TodayCheckedDate_CheckForCodeplexRelease_DoesnotUpdateCheckDate()
        {
            var previousCheck = DateTime.Today;
            File.WriteAllText(FileLocations.LastUpdateCheck, previousCheck.ToString(CultureInfo.InvariantCulture));
            ReleaseInfo checkResult = this.RunUpdateCheck();

            Assert.AreEqual(ReleaseInfo.NotAvailable, checkResult, "New release noticed");
            DateTime lastNoticed = this.ParseLastUpdateDate();
            Assert.AreEqual(lastNoticed.Date, previousCheck.Date, "Last update check date wasn't saved");
        }

        private ReleaseInfo RunUpdateCheck()
        {
            var updateManager = new UpdateManager(this.CreateRss);
            return updateManager.CheckForCodeplexRelease(this.buildDate);
        }

        private RssFeed CreateRss()
        {
            DateTime currentRelease = new DateTime(2010, 1, 3, 1, 0, 0, DateTimeKind.Utc);

            var feed = new RssFeed();
            var chanel = new RssChannel();
            var release = new RssItem()
            {
                PubDate = currentRelease,
                Title = string.Format("Title ({0})", currentRelease)
            };
            chanel.Items.Add(release);
            feed.Channels.Add(chanel);

            return feed;
        }

        private void AssertLastUpdateCheck()
        {
            DateTime lastNoticed = this.ParseLastUpdateDate();
            Assert.IsTrue(lastNoticed.Date > this.yesterDay, "Last update check date wasn't saved");
        }

        private DateTime ParseLastUpdateDate()
        {
            var updateChecksFile = new UpdateChecksFile();
            return updateChecksFile.ReadLastUpdate();
        }
    }
}
