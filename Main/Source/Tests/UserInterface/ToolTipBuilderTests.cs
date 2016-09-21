using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Configuration;
using Terminals.Data;
using Tests.FilePersisted;

namespace Tests.UserInterface
{
    [TestClass]
    public class ToolTipBuilderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            FilePersistedTestLab.SetDefaultFileLocations();
        }

        [TestMethod]
        public void NotShowFullToolTips_BuildToolTip_GeneratesWithNotesAndGroups()
        {
            // todo fix the missing userDisplayName
            string created = BuildFavoriteToolTip();
            const string EXPECTED_TOOLTIP = "Computer: TestServerName\r\nPort: 9999\r\nUser: \r\n";
            Assert.AreEqual(EXPECTED_TOOLTIP, created, "Created tooltip should reflect favorite serverName and port.");
        }

        [TestMethod]
        public void ShowFullToolTips_BuildToolTip_GeneratesWithNotesAndGroups()
        {

            var settings = Settings.Instance;
            settings.ShowFullInformationToolTips = true;
            string created = BuildFavoriteToolTip();
            const string EXPECTED_TOOLTIP = "Computer: TestServerName\r\nPort: 9999\r\nUser: \r\n" +
                                            "Groups: TestGroup\r\nConnect to Console: False\r\nNotes: Here are test notes.\r\n";
            Assert.AreEqual(EXPECTED_TOOLTIP, created, "Created tooltip should reflect favorite serverName and port.");
            settings.ShowFullInformationToolTips = false;
        }

        private static string BuildFavoriteToolTip()
        {
            IFavorite favorite = CreateTestFavorite();
            var bulder = new TooTipBuilder();
            return bulder.BuildTooTip(favorite);
        }

        private static IFavorite CreateTestFavorite()
        {
            List<IGroup> groups = CreateTestGroups();
            IFavorite favorite = TestFavoriteFactory.CreateFavorite(groups);
            favorite.ServerName = "TestServerName";
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
