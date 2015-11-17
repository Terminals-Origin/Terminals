using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals;
using Terminals.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.Terminal;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Connections.Web;
using Terminals.Data;
using Terminals.Forms.EditFavorite;
using Terminals.Integration.Export;

namespace Tests.Connections
{
    [TestClass]
    public class ConnectionManagerTests
    {
        private readonly ConnectionManager connectionManager = new ConnectionManager();

        private const string PORTSMESSAGE = "All protocols have to be identified by default port and vice versa.";

        private const string UNKNOWN_PROTOCOL = "UNKNOWN_PROTOCOL";

        public TestContext TestContext { get; set; }

        /// <summary>
        /// this is a prerequisite test, to ensure all tests use expected data.
        /// </summary>
        [TestMethod]
        public void GetAvailableProtocols_ReturnsAll()
        {
            int knownProtocols = GetUniqueProtocols().Count();
            Assert.AreEqual(8, knownProtocols, "All other test in this SUT operate on wrong data.");
        }

        [TestMethod]
        public void AllKnownProtocols_CreateConnection_ReturnValidType()
        {
            var testData = new[]
            {
                new Tuple<string, Type>(KnownConnectionConstants.RDP, typeof(RDPConnection)),
                new Tuple<string, Type>(VncConnectionPlugin.VNC, typeof(VNCConnection)),
                new Tuple<string, Type>(VmrcConnectionPlugin.VMRC, typeof(VMRCConnection)),
                new Tuple<string, Type>(TelnetConnectionPlugin.TELNET, typeof(TerminalConnection)),
                new Tuple<string, Type>(SshConnectionPlugin.SSH, typeof(TerminalConnection)),
                new Tuple<string, Type>(KnownConnectionConstants.HTTP, typeof(HTTPConnection)),
                new Tuple<string, Type>(KnownConnectionConstants.HTTPS, typeof(HTTPConnection)),
                new Tuple<string, Type>(ICAConnectionPlugin.ICA_CITRIX, typeof(ICAConnection))
            };

            var allValid = testData.All(this.AssertCratedConnection);
            Assert.IsTrue(allValid, "User would be unable to connect.");
        }

        [TestMethod]
        public void UnknownProtocol_CreateConnection_CreatesRdpConnection()
        {
            var testCase = new Tuple<string, Type>("UnknownProtocol", typeof(RDPConnection));
            var isValid = AssertCratedConnection(testCase);
            Assert.IsTrue(isValid, "For unknown protocol we fall back to RdpConnection.");
        }

        private bool AssertCratedConnection(Tuple<string, Type> testCase)
        {
            var mockFavorite = new Mock<IFavorite>();
            mockFavorite.SetupGet(f => f.Protocol).Returns(testCase.Item1);
            Type created = connectionManager.CreateConnection(mockFavorite.Object).GetType();
            this.ReportCreatedConnection(testCase, created);
            return testCase.Item2 == created;
        }

        private void ReportCreatedConnection(Tuple<string, Type> testCase, Type created)
        {
            const string MESSAGE_FORMAT = "For protocol '{0}' created '{1}' connection, expected {2}.";
            string message = string.Format(MESSAGE_FORMAT, testCase.Item1, created.Name, testCase.Item2.Name);
            this.TestContext.WriteLine(message);
        }

        [TestMethod]
        public void UnknownProtocol_GetPort_Returns0()
        {
            var testData = new Tuple<string, int>(UNKNOWN_PROTOCOL, 0);
            var allValid = this.AssertResolvedPort(testData);
            Assert.IsTrue(allValid, "Unknow port we gues 0 as port number.");
        }

        [TestMethod]
        public void AllKnownProtocols_GetPort_ReturnValidPort()
        {
            var testData = new[]
            {
                new Tuple<string, int>(KnownConnectionConstants.RDP, KnownConnectionConstants.RDPPort),
                new Tuple<string, int>(VncConnectionPlugin.VNC, KnownConnectionConstants.VNCVMRCPort),
                new Tuple<string, int>(VmrcConnectionPlugin.VMRC, KnownConnectionConstants.VNCVMRCPort),
                new Tuple<string, int>(TelnetConnectionPlugin.TELNET, TelnetConnectionPlugin.TelnetPort),
                new Tuple<string, int>(SshConnectionPlugin.SSH, SshConnectionPlugin.SSHPort),
                new Tuple<string, int>(KnownConnectionConstants.HTTP, KnownConnectionConstants.HTTPPort),
                new Tuple<string, int>(KnownConnectionConstants.HTTPS, HttpsConnectionPlugin.HTTPSPort),
                new Tuple<string, int>(ICAConnectionPlugin.ICA_CITRIX, ICAConnectionPlugin.ICAPort)
            };

            var allValid = testData.All(this.AssertResolvedPort);
            Assert.IsTrue(allValid, PORTSMESSAGE);
        }

        private bool AssertResolvedPort(Tuple<string, int> testCase)
        {
            var port = connectionManager.GetPort(testCase.Item1);
            const string MESSAGE_FORAMT = "For '{0}' resolved '{1}' as port number, expected {2}";
            string message = string.Format(MESSAGE_FORAMT, testCase.Item1, port, testCase.Item2);
            this.TestContext.WriteLine(message);
            return port == testCase.Item2;
        }

        [TestMethod]
        public void UnknownPort_GetPortName_ReturnsRdp()
        {
            var testData = new Tuple<int, bool, string>(1111, false, KnownConnectionConstants.RDP);
            var allValid = this.AssertPortName(testData);
            Assert.IsTrue(allValid, "We gues RDP for unknonwn default port number.");
        }

        [TestMethod]
        public void AllKnownProtocols_GetPortName_ReturnValidPort()
        {
            var testData = new[]
            {
                new Tuple<int, bool, string>(KnownConnectionConstants.RDPPort, false, KnownConnectionConstants.RDP),
                new Tuple<int, bool, string>(KnownConnectionConstants.VNCVMRCPort, false, VncConnectionPlugin.VNC),
                new Tuple<int, bool, string>(KnownConnectionConstants.VNCVMRCPort, true, VmrcConnectionPlugin.VMRC),
                new Tuple<int, bool, string>(TelnetConnectionPlugin.TelnetPort, false, TelnetConnectionPlugin.TELNET),
                new Tuple<int, bool, string>(SshConnectionPlugin.SSHPort, false, SshConnectionPlugin.SSH),
                new Tuple<int, bool, string>(KnownConnectionConstants.HTTPPort, false, KnownConnectionConstants.HTTP),
                new Tuple<int, bool, string>(HttpsConnectionPlugin.HTTPSPort, false, KnownConnectionConstants.HTTPS),
                new Tuple<int, bool, string>(ICAConnectionPlugin.ICAPort, false, ICAConnectionPlugin.ICA_CITRIX)
            };

            var allValid = testData.All(this.AssertPortName);
            Assert.IsTrue(allValid, PORTSMESSAGE);
        }

        private bool AssertPortName(Tuple<int, bool, string> testCase)
        {
            var portName = connectionManager.GetPortName(testCase.Item1, testCase.Item2);
            this.ReportResolvedPort(testCase, portName);
            return testCase.Item3 == portName;
        }

        private void ReportResolvedPort(Tuple<int, bool, string> testCase, string portName)
        {
            const string MESSAGE_FORMAT = "Port {0} and isvmrc='{1}' resolved as {2}, expected '{3}'";
            string message = string.Format(MESSAGE_FORMAT, testCase.Item1, testCase.Item2, portName, testCase.Item3);
            this.TestContext.WriteLine(message);
        }

        [TestMethod]
        public void NonHttpValidProtocols_IsPortWebbased_ReturnFalse()
        {
            var nonHttpPorts = new[]
            {
                KnownConnectionConstants.RDP,
                VncConnectionPlugin.VNC,
                SshConnectionPlugin.SSH,
                TelnetConnectionPlugin.TELNET,
                VmrcConnectionPlugin.VMRC
            };
            
            bool nonHttp = nonHttpPorts.All(connectionManager.IsProtocolWebBased);
            Assert.IsFalse(nonHttp, "Only Http based protocols are known web based.");
        }
        
        [TestMethod]
        public void HttpBasedProtocols_IsPortWebbased_ReturnTrue()
        {
            var nonHttpPorts = new[] { KnownConnectionConstants.HTTP, KnownConnectionConstants.HTTPS };
            bool nonHttp = nonHttpPorts.All(connectionManager.IsProtocolWebBased);
            Assert.IsTrue(nonHttp, "Only Http based protocols are known web based.");
        }
        
        [TestMethod]
        public void UnknownProtocol_IsPortWebbased_ReturnFalse()
        {
            bool nonHttp = connectionManager.IsProtocolWebBased(UNKNOWN_PROTOCOL);
            Assert.IsFalse(nonHttp, "Unknown protcol cant fit to any category, definitely is not web based.");
        }

        [TestMethod]
        public void AllKnownProtocols_IsKnownProtocol_ReturnTrue()
        {
            bool allKnownAreKnown = GetUniqueProtocols()
                .All(connectionManager.IsKnownProtocol);
            Assert.IsTrue(allKnownAreKnown, "GetAvailable protocols should match all of them as known.");
        }

        [TestMethod]
        public void UknownProtocol_IsKnownProtocol_ReturnFalse()
        {
            bool unknonw = connectionManager.IsKnownProtocol(UNKNOWN_PROTOCOL);
            Assert.IsFalse(unknonw, "We cant support unknown protocols.");
        }

        [TestMethod]
        public void UnknownProtocol_CreateControls_ReturnsEmpty()
        {
            int controlsCount = connectionManager.CreateControls(UNKNOWN_PROTOCOL).Length;
            const string MESSAGE = "For unknow protocol, dont fire exception, instead there is nothing to configure.";
            Assert.AreEqual(0, controlsCount, MESSAGE);
        }

        [TestMethod]
        public void WebProtocols_CreateControls_ReturnsEmpty()
        {
            int httpControls = connectionManager.CreateControls(KnownConnectionConstants.HTTP).Length;
            int httpsControls = connectionManager.CreateControls(KnownConnectionConstants.HTTPS).Length;
            const string MESSAGE = "Web based protocols have no configuration.";
            bool bothEmpty = httpControls == 0 && httpsControls == 0;
            Assert.IsTrue(bothEmpty, MESSAGE);
        }

        [TestMethod]
        public void KnownProtocols_CreateControls_ReturnsAllControls()
        {
            Tuple<string, int, Type>[] testData = CreateControlsTestData();
            bool allValid = testData.All(this.AssertCreatedControls);
            const string MESSAGE = "Created controls dont match expectations.";
            Assert.IsTrue(allValid, MESSAGE);
        }

        private Tuple<string, int, Type>[] CreateControlsTestData()
        {
            return new []{
                new Tuple<string, int, Type>(KnownConnectionConstants.RDP, 5, typeof(RdpDisplayControl)),
                new Tuple<string, int, Type>(VncConnectionPlugin.VNC, 1, typeof(VncControl)),
                new Tuple<string, int, Type>(VmrcConnectionPlugin.VMRC, 1, typeof(VmrcControl)),
                new Tuple<string, int, Type>(TelnetConnectionPlugin.TELNET, 1, typeof(ConsolePreferences)),
                new Tuple<string, int, Type>(SshConnectionPlugin.SSH, 2, typeof(ConsolePreferences)),
                new Tuple<string, int, Type>(ICAConnectionPlugin.ICA_CITRIX, 1, typeof(CitrixControl))
            };
        }

        private bool AssertCreatedControls(Tuple<string, int, Type> testCase)
        {
            var controls = connectionManager.CreateControls(testCase.Item1);
            Type firstControl = controls[0].GetType();
            this.ReportAssertedResult(testCase, controls, firstControl);
            bool countMathes = testCase.Item2 == controls.Length;
            bool controlTypeMatches = firstControl == testCase.Item3;
            return countMathes && controlTypeMatches;
        }

        private void ReportAssertedResult(Tuple<string, int, Type> testCase, Control[] controls, Type firstControl)
        {
            const string MESSAGE_FORMAT = "Tested Protocol '{0}': {1} controls, first '{2}', expected ({3},'{4}')";
            string messgae = string.Format(MESSAGE_FORMAT, testCase.Item1,
                controls.Length, firstControl.Name, testCase.Item2, testCase.Item3);
            this.TestContext.WriteLine(messgae);
        }

        private IEnumerable<string> GetUniqueProtocols()
        {
            return connectionManager.GetAvailableProtocols().Distinct();
        }

        [TestMethod]
        public void DummyProvider_CreateToolbarExtensions_CreatesAll()
        {
            var mockProvider = new Mock<ICurrenctConnectionProvider>();
            var extensions = ConnectionManager.CreateToolbarExtensions(mockProvider.Object).Count();
            Assert.AreEqual(3, extensions, "All known extensions have to be registered");
        }
        
        [TestMethod]
        public void GetSupportedPorts_ReturnsAllNonWeb()
        {
            var resolved = connectionManager.SupportedPorts();
            var expected = new ushort[] { 22, 23, 1494, 3389, 5900 };
            Assert.AreEqual(expected.Length, resolved.Length, "We want to scan each unique port only once ignoring default web ports.");
            bool allResolved = resolved.All(port => expected.Contains(port));
            Assert.IsTrue(allResolved, "Port numbers have to obtained from plugins.");
        }

        [TestMethod]
        public void GetTerminalsOptionsExporters_ReturnsAllExporters()
        {
            var expected = new[]
            {
                typeof (TerminalsRdpExport),
                typeof (TerminalsVncExport),
                typeof (TerminalsIcaExport),
                typeof (TerminalsSshExport),
                typeof (TerminalsTelnetExport),
                typeof (TerminalsVmrcExport)
            };

            ITerminalsOptionsExport[] exporters = this.connectionManager.GetTerminalsOptionsExporters();
            bool allPresent = exporters.All(e => expected.Contains(e.GetType()));
            Assert.IsTrue(allPresent, "All supported plugins should cover all options to export.");
        }
    }
}
