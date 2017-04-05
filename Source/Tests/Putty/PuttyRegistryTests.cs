using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{
    [TestClass]
    public class PuttyRegistryTests
    {
        // TODO These registry integration tests will be removed.
        [Ignore]
        [TestMethod]
        public void LoadSessions()
        {
            var puttyRegistry = new PuttyRegistry();
            var result = puttyRegistry.GetSessions();
            // session names arent case sensitive and are unique
            Assert.IsNotNull(result);
        }
    }
}
