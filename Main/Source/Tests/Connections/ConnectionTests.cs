using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals;
using Terminals.Connections;

namespace Tests.Connections
{
    [TestClass]
    public class ConnectionTests
    {
        [TestMethod]
        public void Message_Log_FiresOnLogEvent()
        {
            var connection = new MockConnection();
            string toLog = string.Empty;
            connection.OnLog += entry =>
            {
                toLog = entry;
            };
            const string EXPECTEDLOGENTRY = "EXPECTEDLOGENTRY";
            connection.CallLog(EXPECTEDLOGENTRY);
            Assert.AreEqual(EXPECTEDLOGENTRY, toLog, "The connection has to forwared the required log entry");
        }
        
        [TestMethod]
        public void Connection_FireDisconnected_FiresEvent()
        {
            var connection = new MockConnection();
            bool eventFired = false;
            connection.OnDisconnected += c =>
            {
                eventFired = true;
            };
            connection.CallFireDisconnected();
            Assert.IsTrue(eventFired, "Derived connection have to fire disconnected event, instead of call main form");
        }
    }
}
