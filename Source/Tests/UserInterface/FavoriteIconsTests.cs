using System;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;
using Terminals.Data;

namespace Tests.UserInterface
{
    [TestClass]
    public class FavoriteIconsTests
    {
        private const string UNKNOWN_ICON_KEY = "terminalsicon.png";

        private const string UNKNOWN_PROTOCOL_DEFUULT_ICON_MESSAGE = "Unknown protocols are handled all with default protocol icon.";

        private const string UNKNOWNPROTOCOL = "UnknownProtocol";

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void AllKnownProtocos_GetTreeviewImageListKey_ReturnKey()
        {
            var testData = new[]
            {
                new Tuple<string, string>(ConnectionManager.RDP, "treeIcon_rdp.png"),
                new Tuple<string, string>(ConnectionManager.VNC, "treeIcon_vnc.png"),
                new Tuple<string, string>(ConnectionManager.SSH, "treeIcon_ssh.png"),
                new Tuple<string, string>(ConnectionManager.TELNET, "treeIcon_telnet.png"),
                new Tuple<string, string>(ConnectionManager.HTTP, "treeIcon_http.png"),
                new Tuple<string, string>(ConnectionManager.HTTPS, "treeIcon_http.png"),

                // undefined icons use default icon
                new Tuple<string, string>(ConnectionManager.VMRC, UNKNOWN_ICON_KEY),
                new Tuple<string, string>(ConnectionManager.ICA_CITRIX, UNKNOWN_ICON_KEY)     
            };

            bool allEquals = testData.All(this.AssertGetTreeviewImageListKey);
            Assert.IsTrue(allEquals, "All known protocols have to respect the same image key rule.");
        }

        [TestMethod]
        public void UnKnownProtocos_GetTreeviewImageListKey_ReturnDefaultKey()
        {
            var testCase = new Tuple<string, string>(UNKNOWNPROTOCOL, UNKNOWN_ICON_KEY);
            bool keyEquals = this.AssertGetTreeviewImageListKey(testCase);
            Assert.IsTrue(keyEquals, UNKNOWN_PROTOCOL_DEFUULT_ICON_MESSAGE);
        }

        private bool AssertGetTreeviewImageListKey(Tuple<string, string> testCase)
        {
            var iconKey = FavoriteIcons.GetTreeviewImageListKey(testCase.Item1);
            const string FORMAT = "{0}: Expected '{1}', resolved '{2}'";
            this.TestContext.WriteLine(FORMAT, testCase.Item1, testCase.Item2, iconKey);
            return testCase.Item2 == iconKey;
        }

        [TestMethod]
        public void AllKnownProtocols_GetFavoriteIcon_ReturnPluginIcon()
        {
            var testData = new[]
            {
                new Tuple<string, Image>(ConnectionManager.RDP, FavoriteIcons.TreeIconRdp),
                new Tuple<string, Image>(ConnectionManager.VNC, FavoriteIcons.TreeIconVnc),
                new Tuple<string, Image>(ConnectionManager.SSH, FavoriteIcons.TreeIconSsh),
                new Tuple<string, Image>(ConnectionManager.TELNET, FavoriteIcons.TreeIconTelnet),
                new Tuple<string, Image>(ConnectionManager.HTTP, FavoriteIcons.TreeIconHttp),
                new Tuple<string, Image>(ConnectionManager.HTTPS, FavoriteIcons.TreeIconHttp),

                // undefined icons use default icon
                new Tuple<string, Image>(ConnectionManager.ICA_CITRIX, FavoriteIcons.Terminalsicon),
                new Tuple<string, Image>(ConnectionManager.VMRC, FavoriteIcons.Terminalsicon)
            };

            bool iconsEquals = testData.All(this.AssertGetFavoriteIcon);
            Assert.IsTrue(iconsEquals, "All protocols have to return their plugin icon.");
        }

        [TestMethod]
        public void UnKnownProtocols_GetFavoriteIcon_ReturnDefaultIcon()
        {
            var testCase = new Tuple<string, Image>(UNKNOWNPROTOCOL, FavoriteIcons.Terminalsicon);
            bool keyEquals = this.AssertGetFavoriteIcon(testCase);
            Assert.IsTrue(keyEquals, UNKNOWN_PROTOCOL_DEFUULT_ICON_MESSAGE);
        }

        private bool AssertGetFavoriteIcon(Tuple<string, Image> testCase)
        {
            var mockFavorite = new Mock<IFavorite>();
            mockFavorite.SetupGet(f => f.Protocol).Returns(testCase.Item1);
            var icon = FavoriteIcons.GetFavoriteIcon(mockFavorite.Object);
            bool iconEquals = icon == testCase.Item2;
            this.TestContext.WriteLine("{0} icon equals: {1}", testCase.Item1, iconEquals);
            return iconEquals;
        }
    }
}
