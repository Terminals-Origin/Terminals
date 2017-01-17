using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{
    [TestClass]
    public class PuttyRegistryTests
    {
        [TestMethod]
        public void LoadSessions()
        {
            PuttyRegistry pr = new PuttyRegistry();
            var result = pr.GetSessions();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HandleNonExistentKey()
        {
            PuttyRegistry pr = new PuttyRegistry(@"nonexistent\path\in\registry");
            var result = pr.GetSessions();
            Assert.IsNull(result);
        }
    }
}
