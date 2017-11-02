using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Updates;
using Tests.Helpers;

namespace Tests.Upgrade
{
    [TestClass]
    public class V401ContentUpgradeTests : SqlPersisted.TestsLab
    {
        private const string FILE_NAME = "SshTelnet_Favorites_401.xml";

        private const string SSHTELNET_FAVORITES = TestDataFiles.TESTDATA_DIRECTORY + FILE_NAME;

        private const string ASSERT_QUERY = @"SELECT CAST(ProtocolProperties as nvarchar(max)) FROM Favorites";

        private const string SSH_PROPERTIES = @"
      <SshOptions>
        <SSH1>false</SSH1>
        <AuthMethod>PublicKey</AuthMethod>
        <SSHKeyFile />
        <CertificateKey />
        <Console>
          <Rows>38</Rows>
          <Columns>110</Columns>
          <Font>[Font: Name=Courier New, Size=10, Units=3, GdiCharSet=0, GdiVerticalFont=False, Style=0]</Font>
          <BackColor>Black</BackColor>
          <TextColor>White</TextColor>
          <CursorColor>Red</CursorColor>
        </Console>
      </SshOptions>
";

        private const string TELNET_PROPERTIES = @"
      <ConsoleOptions>
        <Rows>38</Rows>
        <Columns>110</Columns>
        <Font>[Font: Name=Courier New, Size=10, Units=3, GdiCharSet=0, GdiVerticalFont=False, Style=0]</Font>
        <BackColor>Black</BackColor>
        <TextColor>White</TextColor>
        <CursorColor>Red</CursorColor>
      </ConsoleOptions>
";

        private const string MESSAGE = "Both ssh and telnet options types were replaced by PuttyOptions";

        [DeploymentItem(SSHTELNET_FAVORITES)]
        [TestMethod]
        public void TelnetAndSshInFilePersitence_Upgrade_SetsFavoritePropertiesToNewType()
        {
            string filePath = Path.Combine(this.TestContext.DeploymentDirectory, FILE_NAME);
            var upgrade = new V401ContentUpgrade();
            upgrade.Run(filePath);

            string updatedContent = File.ReadAllText(filePath);
            bool allpass = AllUpgraded(updatedContent);
            Assert.IsTrue(allpass, MESSAGE);
        }

        [Ignore] // TODO Implement Upgrade also in the database
        [TestMethod]
        public void TelnetAndSshInDatabase_Upgrade_SetsFavoritePropertiesToNewType()
        {
            this.InitializeTestLab();
            this.InsertIntoFavoritesTable("SshFavorite", SSH_PROPERTIES);
            this.InsertIntoFavoritesTable("TelnetFavorite", TELNET_PROPERTIES);

            var upgrade = new V401ContentUpgrade();
            //upgrade.Run(filePath);

            var allProperties = this.CheckDatabase.Database.SqlQuery<string>(ASSERT_QUERY);
            string joinedProperties = string.Join(string.Empty, allProperties);
            bool allPass = AllUpgraded(joinedProperties);
            Assert.IsTrue(allPass, MESSAGE);
        }

        private void InsertIntoFavoritesTable(string sshfavorite, string properties)
        {
            // Cant use helper method, because it will insert properties in new format
            // and there would be nothing to upgrade
            string insertDummyData = @"
INSERT INTO Favorites (Name, Protocol, Port, ServerName, NewWindow, ProtocolProperties)
VALUES('{0}', 'SSH', 22, 'ServerName', 1, '{1}')";

            string fullQuery = string.Format(insertDummyData, sshfavorite, properties);
            this.CheckDatabase.Database.ExecuteSqlCommand(fullQuery);
        }

        private static bool AllUpgraded(string updatedContent)
        {
            return !updatedContent.Contains("SSHKeyFile") &&
                   !updatedContent.Contains("ConsoleOptions") &&
                   Regex.Matches(updatedContent, "SshOptions").Count == 2 &&
                   Regex.Matches(updatedContent, "TelnetOptions").Count == 2;
        }
    }
}
