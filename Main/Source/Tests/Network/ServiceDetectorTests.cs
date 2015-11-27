using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;
using Terminals.Connections.VMRC;
using Terminals.Connections.VNC;
using Terminals.Network;

namespace Tests.Network
{
    [TestClass]
    public class ServiceDetectorTests
    {
        private const string IRRELEVANT_IP_ADDRESS = "IrrelevantIp";

        [TestMethod]
        public void UnknownPlugin_ResolveServiceName_ReturnsRdp()
        {
            var dummyPlugin = new DummyPlugin();
            string resolved = Resolve(0, dummyPlugin);
            const string MESSAGE = "If only one plugin represents required port, than its portName is resolved.";
            Assert.AreEqual(dummyPlugin.PortName, resolved, MESSAGE);
        }

        [TestMethod]
        public void WrokingVncVmrcPlugins_ResolveServiceName_ReturnsVnc()
        {
            IConnectionPlugin vnc = new VncConnectionPlugin((ip, port) => {});
            var resolved = Resolve(vnc.Port, new VmrcConnectionPlugin(), vnc);
            Assert.AreEqual(vnc.PortName, resolved, "If extra check is successfull, than that plugins is resolved.");
        }
        
        [TestMethod]
        public void VmrcFailingVncPlugin_ResolveServiceName_ReturnsVmrc()
        {
            IConnectionPlugin vnc = new VncConnectionPlugin((ip, port) =>
            {
                throw new Exception();
            });
            var vmrc = new VmrcConnectionPlugin();
            var resolved = Resolve(vnc.Port, vmrc, vnc);
            Assert.AreEqual(vmrc.PortName, resolved, "If extra check fails standard plugin is resolved.");
        }

        private static string Resolve(int port, params IConnectionPlugin[] plugins)
        {
            Mock<IConnectionManager> mockConnectionManager = CreateConnecitonManager(plugins);
            var detector = new ServiceDetector(mockConnectionManager.Object);
            return detector.ResolveServiceName(IRRELEVANT_IP_ADDRESS, port);
        }

        private static Mock<IConnectionManager> CreateConnecitonManager(params IConnectionPlugin[] plugins)
        {
            var mockConnectionManager = new Mock<IConnectionManager>();
            mockConnectionManager.Setup(m => m.GetPluginsByPort(It.IsAny<int>()))
                .Returns(new List<IConnectionPlugin>(plugins));
            return mockConnectionManager;
        }
    }
}
