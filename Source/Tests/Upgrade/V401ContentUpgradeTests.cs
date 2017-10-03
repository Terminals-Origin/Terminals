using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Upgrade
{
    [TestClass]
    public class V401ContentUpgradeTests
    {
        [Ignore()] //"Not implemented yet."
        [TestMethod]
        public void TelnetAndSsh_Upgrade_SetsFavoritePropertiesToNewType()
        {
            // TODO Upgrade both persistences ssh or telnet favorites to empty PuttyOptions.
            // Let user lost all properties.
        }
    }
}
