using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.CaptureManager;
using Terminals.Configuration;
using Terminals.Data;
using Tests.FilePersisted;

namespace Tests.UserInterface
{
    [TestClass]
    public class CaptureManagerTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void NoTab_PerformScreenCapture_TakesScreenShot()
        {
            this.ConfigureSettings();
            var expectedFiles = this.GetFilesCountInCaptureDirectory() + 1;
            var tabControl = new TabControl.TabControl();
            IFavorite favorite = new Mock<IFavorite>().Object;
            CaptureManager.PerformScreenCapture(tabControl, favorite);
            var actualFiles = this.GetFilesCountInCaptureDirectory();
            Assert.AreEqual(expectedFiles, actualFiles, "Screen capture should create new file in capture direcotry.");
        }

        private void ConfigureSettings()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
            var settings = Settings.Instance;
            settings.CaptureRoot = this.TestContext.TestDeploymentDir;
            settings.EnableCaptureToFolder = true;
        }

        private int GetFilesCountInCaptureDirectory()
        {
            return Directory.GetFiles(this.TestContext.TestDeploymentDir).Length;
        }
    }
}
