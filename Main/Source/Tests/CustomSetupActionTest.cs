using System;
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
    [DeploymentItem(@"Terminals\" + LOG4_NET_FILE)]
    [DeploymentItem(@"Tests\Data\" + APPCONFIG_FILE)]
    public class CustomSetupActionTest
    {
        private const string LOG4_NET_FILE = "Terminals.log4net.config";
        private const string APPCONFIG_FILE = "Terminals.exe.config";
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
            var log4Net = this.PrepareFile(LOG4_NET_FILE);
            this.log4NetFile = log4Net.Item1;
            this.log4NetContent = log4Net.Item2;

            var appConfig = this.PrepareFile(APPCONFIG_FILE);
            this.appConfigFile = appConfig.Item1;
            this.appConfigContent = appConfig.Item2;
        }

        private Tuple<string, string> PrepareFile(string file)
        {
            string targetFile = Path.Combine(this.targetDir, file);
            File.Copy(Path.Combine(DeployDirectory, file), targetFile);
            File.SetAttributes(targetFile, FileAttributes.Normal);
            string originalContent = File.ReadAllText(targetFile);
            return new Tuple<string, string>(targetFile, originalContent);
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
