using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;

namespace Tests
{
    /// <summary>
    /// Test, if custom setup properly upgrades log4net and terminals.exe config files.
    /// Because this is the only one test set for Setup, we dont create separate Tests assembly for setup.
    /// </summary>
    [TestClass]
    public class CustomSetupActionTest
    {
        private string log4NetFile;
        private string appConfigFile;
        private string targetDir;

        // backup original content into memory
        private string appConfigContent;
        private string log4NetContent;

        public TestContext TestContext { get; set; }

        private string DeployDirectory
        {
            get { return this.TestContext.DeploymentDirectory; }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.PrepareTargetDir();
            // we always have to deploy both files, even if we test only one,
            // because the tested method requires both of them
            this.PrepareLo4NetFile();
            this.PrepareAppConfigFile();
        }

        private void PrepareAppConfigFile()
        {
            this.appConfigFile = Path.Combine(this.targetDir, "Terminals.exe.config");
            File.Copy(Path.Combine(DeployDirectory, "app.config"), this.appConfigFile);
            File.SetAttributes(this.appConfigFile, FileAttributes.Normal);
            this.appConfigContent = File.ReadAllText(this.appConfigFile);
        }

        private void PrepareLo4NetFile()
        {
            const string LO4_NET_FILENAME = "Terminals.log4net.config";
            this.log4NetFile = Path.Combine(this.targetDir, LO4_NET_FILENAME);
            File.Copy(Path.Combine(DeployDirectory, LO4_NET_FILENAME), this.log4NetFile);
            File.SetAttributes(this.log4NetFile, FileAttributes.Normal);
            this.log4NetContent = File.ReadAllText(this.log4NetFile);
        }

        private void PrepareTargetDir()
        {
            // use random directory name to allow concurrent run of both tests
            string testDir = Path.GetRandomFileName();
            this.targetDir = Path.Combine(this.DeployDirectory, testDir);
            Directory.CreateDirectory(this.targetDir);
        }

        [TestMethod]
        public void PortableVersionAppconfigTest()
        {
            // first option without changes
            UpgradeConfigFiles.CheckPortableInstallType(this.targetDir, false);
            string upgradedAppConfig = File.ReadAllText(appConfigFile);
            Assert.AreEqual(appConfigContent, upgradedAppConfig, "App config file is expected default");
        }

        [TestMethod]
        public void PortableVersionLog4NetTest()
        {
            UpgradeConfigFiles.CheckPortableInstallType(this.targetDir, false);
            string upgradedLog4Net = File.ReadAllText(log4NetFile);
            Assert.AreEqual(log4NetContent, upgradedLog4Net, "Log4net config file is expected default");
        }

        [TestMethod]
        public void UserProfileVersionAppConfigTest()
        {
            // second option, which should contain the requried user profile options
            UpgradeConfigFiles.CheckPortableInstallType(targetDir, true);
            string upgradedAppConfig = File.ReadAllText(appConfigFile);
            // the only option in the file
            bool inUserProfile = upgradedAppConfig.Contains("<value>False</value>");
            Assert.IsTrue(inUserProfile, "App config doesnt contain user profile flag");
        }

        [TestMethod]
        public void UserProfileVersionLog4NetTest()
        {
            UpgradeConfigFiles.CheckPortableInstallType(targetDir, true);
            string upgradedLog4Net = File.ReadAllText(log4NetFile);
            // part of the upgraded path
            bool oldPath = upgradedLog4Net.Contains(@"\Robert_Chartier\Terminals\Data\logs\");
            Assert.IsTrue(oldPath, "Log4net config doesnt contain user profile path");
        }
    }
}
