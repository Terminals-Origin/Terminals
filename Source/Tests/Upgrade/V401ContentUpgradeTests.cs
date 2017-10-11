using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Updates;
using Tests.Helpers;

namespace Tests.Upgrade
{
    [TestClass]
    public class V401ContentUpgradeTests
    {
        public TestContext TestContext { get; set; }

        [DeploymentItem(TestDataFiles.TESTDATA_DIRECTORY + "SshTelnet_Favorites_401.xml")]
        [Ignore()] //"Not implemented yet."
        [TestMethod]
        public void TelnetAndSsh_Upgrade_SetsFavoritePropertiesToNewType()
        {
            // TODO Upgrade both persistences ssh or telnet favorites to empty PuttyOptions.
            // Let user lost all properties.

            // Deploy Version 4.0 favorites file

            var upgrade = new V401ContentUpgrade();
            upgrade.Run();

            // Assert all ssh/telnet favorites are of type puttyoptions
        }
    }
}
