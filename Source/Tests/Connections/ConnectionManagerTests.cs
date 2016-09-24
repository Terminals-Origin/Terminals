using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Connections.ICA;
using Terminals.Connections.Terminal;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Connections.Web;
using Terminals.Data;
using Terminals.Integration.Export;

namespace Tests.Connections
{
    [TestClass]
    public class ConnectionManagerTests : PluginBasedTests
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
                // VMRCConnection creation may fail in some test runners.
                //new Tuple<string, Type>(VmrcConnectionPlugin.VMRC, typeof(VMRCConnection)),
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
            var testCase = new Tuple<string, Type>("UnknownProtocol", typeof(Connection));
            var isValid = AssertCratedConnection(testCase);
            Assert.IsTrue(isValid, "For unknown protocol we fall back to RdpConnection.");
        }

        private bool AssertCratedConnection(Tuple<string, Type> testCase)
        {
            var mockFavorite = new Mock<IFavorite>();
            mockFavorite.SetupGet(f => f.Protocol).Returns(testCase.Item1);
            Type created = connectionManager.CreateConnection(mockFavorite.Object).GetType();
            this.ReportCreatedConnection(testCase, created);
            // because the runtime type differs
            return testCase.Item2.FullName == created.FullName;
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
            var testData = new Tuple<int, string>(1111, KnownConnectionConstants.RDP);
            var allValid = this.AssertPortName(testData);
            Assert.IsTrue(allValid, "We gues RDP for unknonwn default port number.");
        }

        [TestMethod]
        public void AllKnownProtocols_GetPortName_ReturnValidPort()
        {
            var testData = new[]
            {
                new Tuple<int, string>(KnownConnectionConstants.RDPPort, KnownConnectionConstants.RDP),
                new Tuple<int, string>(KnownConnectionConstants.VNCVMRCPort, VncConnectionPlugin.VNC),
                // imposible to distinquish vnc and vmrc, if both operate on the same port.
                // new Tuple<int, string>(KnownConnectionConstants.VNCVMRCPort, VmrcConnectionPlugin.VMRC),
                new Tuple<int, string>(TelnetConnectionPlugin.TelnetPort, TelnetConnectionPlugin.TELNET),
                new Tuple<int, string>(SshConnectionPlugin.SSHPort, SshConnectionPlugin.SSH),
                new Tuple<int, string>(KnownConnectionConstants.HTTPPort, KnownConnectionConstants.HTTP),
                new Tuple<int, string>(HttpsConnectionPlugin.HTTPSPort, KnownConnectionConstants.HTTPS),
                new Tuple<int, string>(ICAConnectionPlugin.ICAPort, ICAConnectionPlugin.ICA_CITRIX)
            };

            var allValid = testData.All(this.AssertPortName);
            Assert.IsTrue(allValid, PORTSMESSAGE);
        }

        private bool AssertPortName(Tuple<int, string> testCase)
        {
            var portName = connectionManager.GetPortName(testCase.Item1);
            this.ReportResolvedPort(testCase, portName);
            return testCase.Item2 == portName;
        }

        private void ReportResolvedPort(Tuple<int, string> testCase, string portName)
        {
            const string MESSAGE_FORMAT = "Port {0} resolved as {1}, expected '{2}'";
            string message = string.Format(MESSAGE_FORMAT, testCase.Item1, portName, testCase.Item2);
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
            Tuple<string, int, string>[] testData = CreateControlsTestData();
            bool allValid = testData.All(this.AssertCreatedControls);
            const string MESSAGE = "Created controls dont match expectations.";
            Assert.IsTrue(allValid, MESSAGE);
        }

        private Tuple<string, int, string>[] CreateControlsTestData()
        {
            return new []{
                new Tuple<string, int, string>(KnownConnectionConstants.RDP, 5, "Terminals.Forms.EditFavorite.RdpDisplayControl"),
                new Tuple<string, int, string>(VncConnectionPlugin.VNC, 1, "Terminals.Forms.EditFavorite.VncControl"),
                new Tuple<string, int, string>(VmrcConnectionPlugin.VMRC, 1, "Terminals.Forms.EditFavorite.VmrcControl"),
                new Tuple<string, int, string>(TelnetConnectionPlugin.TELNET, 1, "Terminals.ConsolePreferences"),
                new Tuple<string, int, string>(SshConnectionPlugin.SSH, 2, "Terminals.ConsolePreferences"),
                new Tuple<string, int, string>(ICAConnectionPlugin.ICA_CITRIX, 1, "Terminals.Forms.EditFavorite.CitrixControl")
            };
        }

        private bool AssertCreatedControls(Tuple<string, int, string> testCase)
        {
            var controls = connectionManager.CreateControls(testCase.Item1);
            string firstControl = controls[0].GetType().FullName;
            this.ReportAssertedResult(testCase, controls, firstControl);
            bool countMathes = testCase.Item2 == controls.Length;
            bool controlTypeMatches = firstControl == testCase.Item3;
            return countMathes && controlTypeMatches;
        }

        private void ReportAssertedResult(Tuple<string, int, string> testCase, Control[] controls, string firstControl)
        {
            const string MESSAGE_FORMAT = "Tested Protocol '{0}': {1} controls, first '{2}', expected ({3},'{4}')";
            string messgae = string.Format(MESSAGE_FORMAT, testCase.Item1,
                controls.Length, firstControl, testCase.Item2, testCase.Item3);
            this.TestContext.WriteLine(messgae);
        }

        private IEnumerable<string> GetUniqueProtocols()
        {
            return connectionManager.GetAvailableProtocols();
        }

        [TestMethod]
        public void DummyProvider_CreateToolbarExtensions_CreatesAll()
        {
            var mockProvider = new Mock<ICurrenctConnectionProvider>();
            var extensions = this.connectionManager.CreateToolbarExtensions(mockProvider.Object).Count();
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
            string[] expected = new[]
            {
                "Terminals.Integration.Export.TerminalsRdpExport",
                "Terminals.Integration.Export.TerminalsVncExport",
                "Terminals.Integration.Export.TerminalsIcaExport",
                "Terminals.Integration.Export.TerminalsSshExport",
                "Terminals.Integration.Export.TerminalsTelnetExport",
                "Terminals.Integration.Export.TerminalsVmrcExport"
            };

            ITerminalsOptionsExport[] exporters = this.connectionManager.GetTerminalsOptionsExporters();
            bool allPresent = exporters.All(e => expected.Contains(e.GetType().FullName));
            Assert.IsTrue(allPresent, "All supported plugins should cover all options to export.");
        }

        [TestMethod]
        public void GetProtocolOptionsTypes_ReturnsAll()
        {
            IEnumerable<Type> optionTypes = this.connectionManager.GetAllKnownProtocolOptionTypes()
                .Distinct();
            Assert.AreEqual(8, optionTypes.Count(), "To be able serialize all known protocols we have to list all.");
        }
        
        [TestMethod]
        public void GetProtocolOptionsTypes_ContainsEmptyOptions()
        {
            bool containsEmptyOtions = this.connectionManager.GetAllKnownProtocolOptionTypes()
                .Contains(typeof(EmptyOptions));
            Assert.IsTrue(containsEmptyOtions, "To support broken protocols and default values, empty options are required.");
        }

        [TestMethod]
        public void AllKnownPorts_GetPluginsByPort_ReturnExpectedPlugins()
        {
            var testCases = new List<Tuple<int, IEnumerable<string>>>()
            {
                new Tuple<int, IEnumerable<string>>(KnownConnectionConstants.RDPPort, 
                    new List<string>() { "Terminals.Connections.Rdp.RdpConnectionPlugin"}),
                new Tuple<int, IEnumerable<string>>(KnownConnectionConstants.VNCVMRCPort, 
                    new List<string>() { "Terminals.Connections.VNC.VncConnectionPlugin", "Terminals.Connections.VMRC.VmrcConnectionPlugin"}),
                new Tuple<int, IEnumerable<string>>(TelnetConnectionPlugin.TelnetPort, 
                    new List<string>() { "Terminals.Connections.Terminal.TelnetConnectionPlugin"}),
                new Tuple<int, IEnumerable<string>>(SshConnectionPlugin.SSHPort, 
                    new List<string>() { "Terminals.Connections.Terminal.SshConnectionPlugin"}),
                new Tuple<int, IEnumerable<string>>(KnownConnectionConstants.HTTPPort, 
                    new List<string>() { "Terminals.Connections.Web.HttpConnectionPlugin"}),
                new Tuple<int, IEnumerable<string>>(HttpsConnectionPlugin.HTTPSPort, 
                    new List<string>() { "Terminals.Connections.Web.HttpsConnectionPlugin"}),
            };

            bool allPortsResolved = testCases.All(this.AssertAllPortPluginsResolved);
            Assert.IsTrue(allPortsResolved, "Ports have identify their plugins");
        }

        [TestMethod]
        public void UnknownPort_GetPluginsByPort_ReturnsDummyPlugin()
        {
            string dummyPlugin = typeof (DummyPlugin).FullName;
            var testCase = new Tuple<int, IEnumerable<string>>(0, new List<string>() { dummyPlugin });
            var isValid = this.AssertAllPortPluginsResolved(testCase);
            Assert.IsTrue(isValid, "Unknown protocol has to return some dummy data to represent failed state.");
        }

        private bool AssertAllPortPluginsResolved(Tuple<int, IEnumerable<string>> testCase)
        {
            IEnumerable<IConnectionPlugin> resolved = this.connectionManager.GetPluginsByPort(testCase.Item1);
            return resolved.All(plugin => testCase.Item2.Contains(plugin.GetType().FullName));
        }

        [TestMethod]
        public void UnknownProtocol_GetOptionsConverterFactory_ReturnsDummyPlugin()
        {
            IOptionsConverterFactory converterFactory = this.connectionManager.GetOptionsConverterFactory(UNKNOWN_PROTOCOL);
            Assert.IsInstanceOfType(converterFactory,typeof(DummyPlugin), "Resolution of unknonw protocol cant fail.");
        }

        [TestMethod]
        public void AllKnownProtocol_GetOptionsConverterFactory_ReturnInstance()
        {
            string[] protocols = this.connectionManager.GetAvailableProtocols();
            var converterPlugins = protocols.Select(p => new Tuple<string, IOptionsConverterFactory>(p,
                this.connectionManager.GetOptionsConverterFactory(p)))
                .ToArray();

            ReportNoAvailableConverters(converterPlugins);
            bool allResolved = !converterPlugins.Any(p => p.Item2 is DummyPlugin);
            const string Message = "Till supported, all plugins have to be able convert their options to support lagacy import.";
            Assert.IsTrue(allResolved, Message);
        }

        private static void ReportNoAvailableConverters(Tuple<string, IOptionsConverterFactory>[] converterPlugins)
        {
            foreach (Tuple<string, IOptionsConverterFactory> resolved in converterPlugins)
            {
                Console.WriteLine("Converter for '{0}':{1}.", resolved.Item1, resolved.Item2.GetType());
            }
        }
    }
}
