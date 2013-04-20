using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Updates;
using Tests.FilePersisted;

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
        private bool releaseEventReceived;
        private DateTime buildDate = Program.Info.BuildDate;

        /// <summary>
        /// in debug there is never a newer version, because it is checked by build date
        /// </summary>
        [TestMethod]
        public void CheckReleaseNotAvailableTest()
        {
            var started = DateTime.Now;
            this.ConfigureTestLab();
            RunUpdateCheck();

            this.AssertNotReportedRelease();
            AssertLastUpdateCheck(started);
        }

        /// <summary>
        /// in case of concurrent test, fails because of not initialized persistence called from  MainForm.ReleaseAvailable.
        /// </summary>
        [TestMethod]
        public void CheckAvailableReleaseTest()
        {
            buildDate = DateTime.MinValue;
            var started = DateTime.Now;
            this.ConfigureTestLab();
            RunUpdateCheck();

            const string RELEASE_NOTICED = "Didn't notice new release";
            Assert.IsNotNull(MainForm.ReleaseDescription, RELEASE_NOTICED);
            Assert.IsTrue(this.releaseEventReceived, RELEASE_NOTICED);
            AssertLastUpdateCheck(started);
        }

        [TestMethod]
        public void AlreadyReportedReleaseTest()
        {
            this.ConfigureTestLab();
            var previousCheck = DateTime.Now;
            File.WriteAllText(FileLocations.LastUpdateCheck, previousCheck.ToString());
            RunUpdateCheck();

            this.AssertNotReportedRelease();
            DateTime lastNoticed = ParseLastUpdateDate();
            Assert.IsTrue(lastNoticed.Date == previousCheck.Date, "Last update check date wasn't saved");
        }

        private void ConfigureTestLab()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
            MainForm.OnReleaseIsAvailable += this.OnReleaseIsAvailable;
        }

        private void RunUpdateCheck()
        {
            var updateManager = new PrivateType(typeof (UpdateManager));
            updateManager.InvokeStatic("CheckForCodeplexRelease", new object[] { buildDate });
        }

        private void AssertNotReportedRelease()
        {
            const string RELEASE_NOTICED = "New release noticed";
            Assert.IsNull(MainForm.ReleaseDescription, RELEASE_NOTICED);
            Assert.IsFalse(this.releaseEventReceived, RELEASE_NOTICED);
        }

        private static void AssertLastUpdateCheck(DateTime started)
        {
            DateTime lastNoticed = ParseLastUpdateDate();
            Assert.IsTrue(lastNoticed > started, "Last update check date wasn't saved");
        }

        private static DateTime ParseLastUpdateDate()
        {
            String text = File.ReadAllText(FileLocations.LastUpdateCheck).Trim();
            DateTime lastNoticed = DateTime.MinValue;
            DateTime.TryParse(text, out lastNoticed);
            return lastNoticed;
        }

        private void OnReleaseIsAvailable(IFavorite releaseFavorite)
        {
            this.releaseEventReceived = true;
        }
    }
}
