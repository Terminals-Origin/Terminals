using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Connections;
using Terminals.Forms.EditFavorite;

namespace Tests.Connections
{
    [TestClass]
    public class ConnectionManagerTests
    {
        private const string UNKNOWN_PROTOCOL = "UNKNOWN_PROTOCOL";

        public TestContext TestContext { get; set; }

        /// <summary>
        /// this is a prerequisite test, to ensure all tests use expected data.
        /// </summary>
        [TestMethod]
        public void GetAvailableProtocols_ReturnsAll()
        {
            int knownProtocols = ConnectionManager.GetAvailableProtocols().Distinct().Count();
            Assert.AreEqual(8, knownProtocols, "All other test in this SUT operate on wrong data.");
        }
        
        [TestMethod]
        public void AllKnownProtocols_IsKnownProtocol_ReturnTrue()
        {
            bool allKnownAreKnown = ConnectionManager.GetAvailableProtocols()
                .Distinct()
                .All(ConnectionManager.IsKnownProtocol);
            Assert.IsTrue(allKnownAreKnown, "GetAvailable protocols should match all of them as known.");
        }

        [TestMethod]
        public void NonHttpValidProtocols_IsPortWebbased_ReturnFalse()
        {
            var nonHttpPorts = new[]
            {
                ConnectionManager.RDP,
                ConnectionManager.VNC,
                ConnectionManager.SSH,
                ConnectionManager.TELNET,
                ConnectionManager.VMRC
            };
            
            bool nonHttp = nonHttpPorts.All(ConnectionManager.IsProtocolWebBased);
            Assert.IsFalse(nonHttp, "Only Http based protocols are known web based.");
        }
        
        [TestMethod]
        public void HttpBasedProtocols_IsPortWebbased_ReturnTrue()
        {
            var nonHttpPorts = new[] { ConnectionManager.HTTP, ConnectionManager.HTTPS };
            bool nonHttp = nonHttpPorts.All(ConnectionManager.IsProtocolWebBased);
            Assert.IsTrue(nonHttp, "Only Http based protocols are known web based.");
        }
        
        [TestMethod]
        public void UnknownProtocol_IsPortWebbased_ReturnFalse()
        {
            bool nonHttp = ConnectionManager.IsProtocolWebBased(UNKNOWN_PROTOCOL);
            Assert.IsFalse(nonHttp, "Unknown protcol cant fit to any category, definitely is not web based.");
        }

        [TestMethod]
        public void UknownProtocol_IsKnownProtocol_ReturnFalse()
        {
            bool unknonw = ConnectionManager.IsKnownProtocol(UNKNOWN_PROTOCOL);
            Assert.IsFalse(unknonw, "We cant support unknown protocols.");
        }

        [TestMethod]
        public void UnknownProtocol_CreateControls_ReturnsEmpty()
        {
            int controlsCount = ConnectionManager.CreateControls(UNKNOWN_PROTOCOL).Length;
            const string MESSAGE = "For unknow protocol, dont fire exception, instead there is nothing to configure.";
            Assert.AreEqual(0, controlsCount, MESSAGE);
        }

        [TestMethod]
        public void WebProtocols_CreateControls_ReturnsEmpty()
        {
            int httpControls = ConnectionManager.CreateControls(ConnectionManager.HTTP).Length;
            int httpsControls = ConnectionManager.CreateControls(ConnectionManager.HTTPS).Length;
            const string MESSAGE = "Web based protocols have no configuration.";
            bool bothEmpty = httpControls == 0 && httpsControls == 0;
            Assert.IsTrue(bothEmpty, MESSAGE);
        }

        [TestMethod]
        public void KnownProtocols_CreateControls_ReturnsAllControls()
        {
            Tuple<string, int, Type>[] testData = CreateControlsTestData();
            bool allValid = testData.All(d => AssertCreatedControls(d.Item1, d.Item2, d.Item3));
            const string MESSAGE = "Created controls dont match expectations.";
            Assert.IsTrue(allValid, MESSAGE);
        }

        private Tuple<string, int, Type>[] CreateControlsTestData()
        {
            return new []{
                new Tuple<string, int, Type>(ConnectionManager.RDP, 5, typeof(RdpDisplayControl)),
                new Tuple<string, int, Type>(ConnectionManager.VNC, 1, typeof(VncControl)),
                new Tuple<string, int, Type>(ConnectionManager.VMRC, 1, typeof(VmrcControl)),
                new Tuple<string, int, Type>(ConnectionManager.TELNET, 1, typeof(ConsolePreferences)),
                new Tuple<string, int, Type>(ConnectionManager.SSH, 2, typeof(ConsolePreferences)),
                new Tuple<string, int, Type>(ConnectionManager.ICA_CITRIX, 1, typeof(CitrixControl))
            };
        }

        private bool AssertCreatedControls(string protocol, int expectedCount, Type firstControlExpected)
        {
            var controls = ConnectionManager.CreateControls(protocol);
            Type firstControl = controls[0].GetType();
            this.ReportAssertedResult(protocol, controls, firstControl);
            bool countMathes = expectedCount == controls.Length;
            bool controlTypeMatches = firstControl == firstControlExpected;
            return countMathes && controlTypeMatches;
        }

        private void ReportAssertedResult(string protocol, Control[] controls, Type firstControl)
        {
            const string MESSAGE_FORMAT = "Tested Protocol '{0}': {1} controls, first '{2}'";
            string messgae = string.Format(MESSAGE_FORMAT, protocol, controls.Length, firstControl.Name);
            this.TestContext.WriteLine(messgae);
        }
    }
}
