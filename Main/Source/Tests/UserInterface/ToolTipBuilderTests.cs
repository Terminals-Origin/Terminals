using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.Credentials;
using Tests.FilePersisted;

namespace Tests.UserInterface
{
    [TestClass]
    public class ToolTipBuilderTests
    {
        private readonly PersistenceSecurity persistenceSecurity = new PersistenceSecurity();

        [TestInitialize]
        public void TestInitialize()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
        }

        [TestMethod]
        public void NotShowFullToolTips_BuildToolTip_GeneratesWithNotesAndGroups()
        {
            string created = this.BuildFavoriteToolTip();
            const string EXPECTED_TOOLTIP = "Computer: TestServerName\r\nPort: 9999\r\nUser: TestDomain\\TestUser\r\n";
            Assert.AreEqual(EXPECTED_TOOLTIP, created, "Created tooltip should reflect favorite serverName and port.");
        }

        [TestMethod]
        public void ShowFullToolTips_BuildToolTip_GeneratesWithNotesAndGroups()
        {

            var settings = Settings.Instance;
            settings.ShowFullInformationToolTips = true;
            string created = this.BuildFavoriteToolTip();
            const string EXPECTED_TOOLTIP = "Computer: TestServerName\r\nPort: 9999\r\nUser: TestDomain\\TestUser\r\n" +
                                            "Groups: TestGroup\r\nConnect to Console: False\r\nNotes: Here are test notes.\r\n";
            Assert.AreEqual(EXPECTED_TOOLTIP, created, "Created tooltip should reflect favorite serverName and port.");
            settings.ShowFullInformationToolTips = false;
        }

        private string BuildFavoriteToolTip()
        {
            IFavorite favorite = this.CreateTestFavorite();
            var bulder = new ToolTipBuilder(this.persistenceSecurity);
            return bulder.BuildTooTip(favorite);
        }

        private IFavorite CreateTestFavorite()
        {
            List<IGroup> groups = CreateTestGroups();
            IFavorite favorite = TestFavoriteFactory.CreateFavorite(groups);
            favorite.ServerName = "TestServerName";
            var guarded = new GuardedSecurity(this.persistenceSecurity, favorite.Security);
            guarded.Domain = "TestDomain";
            guarded.UserName = "TestUser";
            favorite.Port = 9999;
            favorite.Notes = "Here are test notes.";
            return favorite;
        }

        private static List<IGroup> CreateTestGroups()
        {
            var groups = new List<IGroup>();
            var groupMock = new Mock<IGroup>().SetupAllProperties();
            IGroup group = groupMock.Object;
            group.Name = "TestGroup";
            groups.Add(group);
            return groups;
        }
    }
}
