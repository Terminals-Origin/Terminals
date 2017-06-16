using System;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Configuration;
using Terminals.Updates;
using Tests.FilePersisted;

namespace Tests
{
    /// <summary>
    /// Check for new release to be able parse release rss feeds, to be able to download and unpack the update package.
    /// Expected implementation of UpdateManager:
    /// - the update check file is written after all checks
    /// - release shouldn't be reported only, if there is a release with newer version, than current
    /// - new release should be reported once per day only
    /// </summary>
    [TestClass]
    public class UpdateManagerTest
    {
        private Version currentVersion = new Version(2, 0, 0);

        private readonly DateTime yesterDay = DateTime.UtcNow.Date.AddDays(-1);

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
            this.currentVersion = new Version(4, 0, 0);
            ReleaseInfo checkResult = this.RunUpdateCheck();

            Assert.AreEqual(ReleaseInfo.NotAvailable, checkResult, "New release noticed");
            this.AssertLastUpdateCheck();
        }

        [TestMethod]
        public void OldestBuildDate_CheckForCodeplexRelease_ReturnsValidRelease()
        {
            this.currentVersion = new Version(1, 0, 0);
            ReleaseInfo checkResult = this.RunUpdateCheck();

            Assert.AreNotEqual(ReleaseInfo.NotAvailable, checkResult, "Didn't notice new release");
            this.AssertLastUpdateCheck();
        }

        [TestMethod]
        public void TodayCheckedDate_CheckForCodeplexRelease_DoesnotUpdateCheckDate()
        {
            var previousCheck = DateTime.UtcNow.Date;
            File.WriteAllText(FileLocations.LastUpdateCheck, previousCheck.ToString(CultureInfo.InvariantCulture));
            ReleaseInfo checkResult = this.RunUpdateCheck();

            Assert.AreEqual(ReleaseInfo.NotAvailable, checkResult, "New release noticed");
            DateTime lastNoticed = this.ParseLastUpdateDate();
            Assert.AreEqual(lastNoticed.Date, previousCheck.Date, "Last update check date wasn't saved");
        }

        private ReleaseInfo RunUpdateCheck()
        {
            const string CreateRss = @"
[
{
    ""name"": ""Title"",
    ""tag_name"": ""3.0.0"",
    ""published_at"": ""2017-06-14T20:25:15Z""
}
]";

            var updateManager = new UpdateManager(() => CreateRss);
            return updateManager.CheckForPublishedRelease(this.currentVersion);
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
