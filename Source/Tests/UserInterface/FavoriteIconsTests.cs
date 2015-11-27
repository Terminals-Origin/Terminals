using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.Rdp;
using Terminals.Connections.Terminal;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Connections.Web;
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
                new Tuple<string, string>(KnownConnectionConstants.RDP, "treeIcon_RDP"),
                new Tuple<string, string>(VncConnectionPlugin.VNC, "treeIcon_VNC"),
                new Tuple<string, string>(SshConnectionPlugin.SSH, "treeIcon_SSH"),
                new Tuple<string, string>(TelnetConnectionPlugin.TELNET, "treeIcon_Telnet"),
                new Tuple<string, string>(KnownConnectionConstants.HTTP, "treeIcon_HTTP"),
                new Tuple<string, string>(KnownConnectionConstants.HTTPS, "treeIcon_HTTPS"),

                // undefined icons use default icon
                new Tuple<string, string>(VmrcConnectionPlugin.VMRC, "treeIcon_VMRC"),
                new Tuple<string, string>(ICAConnectionPlugin.ICA_CITRIX, "treeIcon_ICA Citrix")     
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
                new Tuple<string, Image>(KnownConnectionConstants.RDP, RdpConnectionPlugin.TreeIconRdp),
                new Tuple<string, Image>(VncConnectionPlugin.VNC, VncConnectionPlugin.TreeIconVnc),
                new Tuple<string, Image>(SshConnectionPlugin.SSH, SshConnectionPlugin.TreeIconSsh),
                new Tuple<string, Image>(TelnetConnectionPlugin.TELNET, TelnetConnectionPlugin.TreeIconTelnet),
                new Tuple<string, Image>(KnownConnectionConstants.HTTP, HttpConnectionPlugin.TreeIconHttp),
                new Tuple<string, Image>(KnownConnectionConstants.HTTPS, HttpConnectionPlugin.TreeIconHttp),

                // undefined icons use default icon
                new Tuple<string, Image>(ICAConnectionPlugin.ICA_CITRIX, ConnectionManager.Terminalsicon),
                new Tuple<string, Image>(VmrcConnectionPlugin.VMRC, ConnectionManager.Terminalsicon)
            };

            bool iconsEquals = testData.All(this.AssertGetFavoriteIcon);
            Assert.IsTrue(iconsEquals, "All protocols have to return their plugin icon.");
        }

        [TestMethod]
        public void UnKnownProtocols_GetFavoriteIcon_ReturnDefaultIcon()
        {
            var testCase = new Tuple<string, Image>(UNKNOWNPROTOCOL, ConnectionManager.Terminalsicon);
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

        [TestMethod]
        public void AllKnownProtocols_IsDefaultProtocolImage_ReturnTrue()
        {
            IEnumerable<Image> testCase = FavoriteIcons.GetProtocolIcons().Values;
            bool allDefault = testCase.All(FavoriteIcons.IsDefaultProtocolImage);
            Assert.IsTrue(allDefault, "The produced default icons have to be identified as produced.");
        }
    }
}
