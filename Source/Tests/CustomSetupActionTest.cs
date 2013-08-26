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
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.targetDir = this.TestContext.DeploymentDirectory;
            this.log4NetFile = Path.Combine(targetDir, "Terminals.log4net.config");
            this.appConfigFile = Path.Combine(targetDir, "Terminals.exe.config");
            File.Move(Path.Combine(targetDir, "app.config"), appConfigFile);
            File.SetAttributes(log4NetFile, FileAttributes.Normal);
            File.SetAttributes(appConfigFile, FileAttributes.Normal);
        }

        [TestMethod]
        public void UserProfileVersionTest()
        {
            // backup both into memory
            string appConfigContent = File.ReadAllText(appConfigFile);
            string log4NetContent = File.ReadAllText(log4NetFile);

            // first option without changes
            UpgradeConfigFiles.CheckPortableInstallType(targetDir, false);
            string upgradedLog4Net = File.ReadAllText(log4NetFile);
            Assert.AreEqual(log4NetContent, upgradedLog4Net, "Log4net config file is not expected default");
            string upgradedAppConfig = File.ReadAllText(appConfigFile);
            Assert.AreEqual(appConfigContent, upgradedAppConfig, "App config file is not expected default");

            // second option, which should contain the requried user profile options
            UpgradeConfigFiles.CheckPortableInstallType(targetDir, true);
            upgradedAppConfig = File.ReadAllText(appConfigFile);
            bool inUserProfile = upgradedAppConfig.Contains("<value>False</value>"); // the only option in the file
            Assert.IsTrue(inUserProfile, "App config doesnt contain user profile flag");

            upgradedLog4Net = File.ReadAllText(log4NetFile);
            bool oldPath = upgradedLog4Net.Contains(@"\Robert_Chartier\Terminals\Data\logs\"); // part of the upgraded path
            Assert.IsTrue(oldPath, "Log4net config doesnt contain user profile path");
        }
    }
}
