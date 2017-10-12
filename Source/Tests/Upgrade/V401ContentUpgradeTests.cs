using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Updates;
using Tests.Helpers;

namespace Tests.Upgrade
{
    [TestClass]
    public class V401ContentUpgradeTests
    {
        private const string FILE_NAME = "SshTelnet_Favorites_401.xml";

        private const string SSHTELNET_FAVORITES = TestDataFiles.TESTDATA_DIRECTORY + FILE_NAME;

        public TestContext TestContext { get; set; }

        [DeploymentItem(SSHTELNET_FAVORITES)]
        [TestMethod]
        public void TelnetAndSsh_Upgrade_SetsFavoritePropertiesToNewType()
        {
            string filePath = Path.Combine(this.TestContext.DeploymentDirectory, FILE_NAME);

            // TODO Upgrade SQL persistence ssh or telnet favorites to empty PuttyOptions.

            var upgrade = new V401ContentUpgrade();
            upgrade.Run(filePath);

            string updatedContent = File.ReadAllText(filePath);
            bool allpass = !updatedContent.Contains("SshOptions") &&
                           !updatedContent.Contains("ConsoleOptions") &&
                           Regex.Matches(updatedContent, "PuttyOptions").Count == 4;
            Assert.IsTrue(allpass, "Both ssh and telnet options were replaced by PuttyOptions");
        }
    }
}
