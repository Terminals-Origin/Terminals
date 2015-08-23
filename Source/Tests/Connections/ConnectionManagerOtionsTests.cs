using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;
using Terminals.Data;

namespace Tests.Connections
{
    [TestClass]
    public class ConnectionManagerOtionsTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void UnknownProtocol_UpdateProtocolPropertiesByProtocol_ReturnsEmptyOptions()
        {
            var returned = ConnectionManager.UpdateProtocolPropertiesByProtocol("UnknonwProtocol", new ConsoleOptions());
            Assert.IsInstanceOfType(returned, typeof(EmptyOptions), "There is no option, how to switch the properties.");
        }
        
        [TestMethod]
        public void NotChanged_UpdateProtocolPropertiesByProtocol_ReturnsExpectedOptions()
        {
            var testData = new[]
            {
                new Tuple<string, ProtocolOptions>(ConnectionManager.RDP, new RdpOptions()),
                new Tuple<string, ProtocolOptions>(ConnectionManager.VNC, new VncOptions()),
                new Tuple<string, ProtocolOptions>(ConnectionManager.VMRC, new VMRCOptions()),
                new Tuple<string, ProtocolOptions>(ConnectionManager.TELNET, new ConsoleOptions()),
                new Tuple<string, ProtocolOptions>(ConnectionManager.SSH, new SshOptions()),
                new Tuple<string, ProtocolOptions>(ConnectionManager.HTTP, new WebOptions()),
                new Tuple<string, ProtocolOptions>(ConnectionManager.HTTPS, new WebOptions()),
                new Tuple<string, ProtocolOptions>(ConnectionManager.ICA_CITRIX, new ICAOptions())
            };

            var allValid = testData.All(this.AssertTheSameInstance);
            Assert.IsTrue(allValid, "All protocols have to reflect related protocol.");
        }

        private bool AssertTheSameInstance(Tuple<string, ProtocolOptions> testCase)
        {
            var returned = ConnectionManager.UpdateProtocolPropertiesByProtocol(testCase.Item1, testCase.Item2);
            string expected = testCase.Item2.GetType().Name;
            string returnedType = returned.GetType().Name;
            ReportChangedOptions(testCase.Item1, expected, returnedType);
            return returned == testCase.Item2;  
        }

        [TestMethod]
        public void Changed_UpdateProtocolPropertiesByProtocol_ReturnsExpectedType()
        {
            var testData = new[]
            {
                new Tuple<string, Type>(ConnectionManager.RDP, typeof(RdpOptions)),
                new Tuple<string, Type>(ConnectionManager.VNC, typeof(VncOptions)),
                new Tuple<string, Type>(ConnectionManager.VMRC, typeof(VMRCOptions)),
                new Tuple<string, Type>(ConnectionManager.TELNET, typeof(ConsoleOptions)),
                new Tuple<string, Type>(ConnectionManager.SSH, typeof(SshOptions)),
                new Tuple<string, Type>(ConnectionManager.HTTP, typeof(WebOptions)),
                new Tuple<string, Type>(ConnectionManager.HTTPS, typeof(WebOptions)),
                new Tuple<string, Type>(ConnectionManager.ICA_CITRIX, typeof(ICAOptions))
            };

            var allValid = testData.All(this.AssertOptions);
            Assert.IsTrue(allValid, "All protocols have to reflect related protocol.");
        }

        private bool AssertOptions(Tuple<string, Type> testCase)
        {
            // No protocol uses EmptyOptions, so it is used as change from something else
            var returned = ConnectionManager.UpdateProtocolPropertiesByProtocol(testCase.Item1, new EmptyOptions());
            ReportCreated(returned, testCase);
            return returned.GetType() == testCase.Item2;
        }

        private void ReportCreated(ProtocolOptions returned, Tuple<string, Type> testCase)
        {
            string returnedType = returned.GetType().Name;
            string protocol = testCase.Item1;
            string expected = testCase.Item2.Name;
            this.ReportChangedOptions(protocol, expected, returnedType);
        }

        private void ReportChangedOptions(string protocol, string expected, string returned)
        {
            const string FORMAT = "For protocol {0}: Expected {1} Returned {2}";
            string message = string.Format(FORMAT, protocol, expected, returned);
            this.TestContext.WriteLine(message);
        }
    }
}
