using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;

namespace Tests.Connections
{
    [TestClass]
    public class DesktopSharesTests
    {
        private const string EXPECTED = "Expected";
        private const string SERVER = EXPECTED + "Server";
        private const string USER = EXPECTED + "User";
        const string EXPECTED_USERSERVER = SERVER + USER;

        [TestMethod]
        public void NullConnection_EvaluateDesktopShare_ReturnsEmpty()
        {
            var desktopShares = new DesktopShares(null, "Irelevant");
            string result = desktopShares.EvaluateDesktopShare();
            Assert.AreEqual(string.Empty, result, "If connection is not available, share path cant be evaluated.");
        }

        [TestMethod]
        public void ValidCurrent_EvaluateDesktopShare_ReturnsGiven()
        {
            var desktopShares = new DesktopShares(null, "Irelevant");
            string result = desktopShares.EvaluateDesktopShare(EXPECTED);
            Assert.AreEqual(EXPECTED, result, "If current desktop share is available, it is not modified.");
        }

        [TestMethod]
        public void NullCurrentAndConnection_EvaluateDesktopShare_ReturnsDefault()
        {
            IConnectionExtra connection = CreateMockConnection();
            var desktopShares = new DesktopShares(connection, EXPECTED);
            string result = desktopShares.EvaluateDesktopShare(string.Empty);
            Assert.AreEqual(EXPECTED, result, "If current desktop share is available, it is not modified.");
        }

        [TestMethod]
        public void DefaultShareAndConnection_EvaluateDesktopShare_ReturnsEvaluated()
        {
            IConnectionExtra connection = CreateMockConnection();
            // check, that user both case to check case sensitivity.
            var desktopShares = new DesktopShares(connection, "%SERVER%%user%");
            string result = desktopShares.EvaluateDesktopShare();
            const string MESSAGE = "If default share contains 'server' or 'user', they are replaced case sensitve.";
            Assert.AreEqual(EXPECTED_USERSERVER, result, MESSAGE);
        }

        private static IConnectionExtra CreateMockConnection()
        {
            var connectionMock = new Mock<IConnectionExtra>(MockBehavior.Strict);
            connectionMock.SetupGet(c => c.Server).Returns(SERVER);
            connectionMock.SetupGet(c => c.UserName).Returns(USER);
            return connectionMock.Object;
        }
    }
}
