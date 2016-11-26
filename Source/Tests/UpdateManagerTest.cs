using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Configuration;
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
        public void CheckReleaseNotAvailableTest()
        {
            ReleaseInfo checkResult = RunUpdateCheck();

            Assert.AreEqual(ReleaseInfo.NotAvailable, checkResult, "New release noticed");
            AssertLastUpdateCheck();
        }

        [TestMethod]
        public void CheckAvailableReleaseTest()
        {
            buildDate = DateTime.MinValue;
            ReleaseInfo checkResult = RunUpdateCheck();

            Assert.AreNotEqual(ReleaseInfo.NotAvailable, checkResult, "Didn't notice new release");
            AssertLastUpdateCheck();
        }

        [TestMethod]
        public void AlreadyReportedReleaseTest()
        {
            var previousCheck = DateTime.Today;
            File.WriteAllText(FileLocations.LastUpdateCheck, previousCheck.ToString());
            ReleaseInfo checkResult = RunUpdateCheck();

            Assert.AreEqual(ReleaseInfo.NotAvailable, checkResult, "New release noticed");
            DateTime lastNoticed = ParseLastUpdateDate();
            Assert.AreEqual(lastNoticed.Date, previousCheck.Date, "Last update check date wasn't saved");
        }

        private ReleaseInfo RunUpdateCheck()
        {
            var updateManager = new PrivateType(typeof (UpdateManager));
            object releaseInfo = updateManager.InvokeStatic("CheckForCodeplexRelease", new object[] { buildDate });
            return releaseInfo as ReleaseInfo;
        }

        private void AssertLastUpdateCheck()
        {
            DateTime lastNoticed = ParseLastUpdateDate();
            Assert.IsTrue(lastNoticed.Date > this.yesterDay, "Last update check date wasn't saved");
        }

        private DateTime ParseLastUpdateDate()
        {
            var checkFileAccessor = new PrivateObject(new UpdateChecksFile());
            return (DateTime)checkFileAccessor.Invoke("ReadLastUpdate");
        }
    }
}
