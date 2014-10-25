using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;
using Terminals.Data;

namespace Tests.Connections
{
    [TestClass]
    public class ConnecitonStateDetectorTests
    {
        private const string EXPECTEDSERVERNAME = "EXPECTEDSERVERNAME";
        private const int EXPECTEDPORT = 9999;

        private ConnectionStateDetector stateDetector;

        private int port;

        private string serverName;

        private bool reconnected;

        private readonly ManualResetEvent runEvent = new ManualResetEvent(false);

        [TestInitialize]
        public void Setup()
        {
            this.stateDetector = new ConnectionStateDetector(TestAction, 0, 1);
            IFavorite fakeFavorite = CreateFakeFavorite();
            this.stateDetector.AssignFavorite(fakeFavorite);
            this.stateDetector.Reconnected += this.StateDetector_Reconnected;
            this.stateDetector.Start();
        }

        private void StateDetector_Reconnected(object sender, EventArgs e)
        {
            this.reconnected = true;
            // not in the test action, we want to be sure, all properties we are waiting for were set.
            runEvent.Set();
        }

        private void TestAction(string serverName, int port)
        {
            this.serverName = serverName;
            this.port = port;
        }

        [TestCleanup]
        public void Teardown()
        {
            this.stateDetector.Dispose();
        }

        [TestMethod]
        public void Instance_AssignFavorite_UsesServerName()
        {
            this.runEvent.WaitOne();
            const string MESSAGE = "Assigned server name has to be used for connection detection";
            Assert.AreEqual(EXPECTEDSERVERNAME, this.serverName, MESSAGE);
        }

        [TestMethod]
        public void Instance_AssignFavorite_UsesPort()
        {
            this.runEvent.WaitOne();
            const string MESSAGE = "Assigned port has to be used for connection detection";
            Assert.AreEqual(EXPECTEDPORT, this.port, MESSAGE);
        }

        [TestMethod]
        public void Instance_Start_SetsIsRunning()
        {
            this.runEvent.WaitOne();
            Assert.IsTrue(stateDetector.IsRunning, "Running detection has to correspond with Start call");
        }

        [TestMethod]
        public void Instance_StartStop_SetsNotRunning()
        {
            this.runEvent.WaitOne();
            stateDetector.Stop();
            Assert.IsFalse(stateDetector.IsRunning, "Stoped detection has to correspond with Stop calls");
        }

        [TestMethod]
        public void Instance_TryReconnect_FiresReconnected()
        {
            this.runEvent.WaitOne();
            Assert.IsTrue(this.reconnected, "Reconnected connnection has to report the state change");
        }

        [TestMethod]
        public void FailingTest_TryReconnect_FiresReconnectExpired()
        {
            this.stateDetector = new ConnectionStateDetector((serverName, port) =>
            {
                throw new Exception("Failing test simulation");
            }, 0, 1);
            IFavorite fakeFavorite = CreateFakeFavorite();
            this.stateDetector.AssignFavorite(fakeFavorite);
            bool expired = false;
            var runExpired = new ManualResetEvent(false);
            this.stateDetector.ReconnectExpired += (sender, args) =>
            {
                expired = true;
                runExpired.Set();
            };
            
            this.stateDetector.Start();
            runExpired.WaitOne();
            Assert.IsTrue(expired, "Failing connection longer than retry count has to raise expired event.");
        }

        private static IFavorite CreateFakeFavorite()
        {
            var fakeFavorite = new Mock<IFavorite>(MockBehavior.Strict);
            fakeFavorite.SetupGet(fa => fa.ServerName)
                .Returns(() => EXPECTEDSERVERNAME);
            fakeFavorite.SetupGet(f => f.Port)
                .Returns(() => EXPECTEDPORT);
            return fakeFavorite.Object;
        }
    }
}
