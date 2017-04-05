using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{

    [TestClass]
    public class PuttyTests
    {
        [TestMethod]
        public void EnsurePuttyBinaryInResources()
        {
            PuttyConnection puttyConnection = new PuttyConnection();
            const string RESOURCES_PUTTY_EXE = @"Resources\" + PuttyConnection.PUTTY_BINARY;
            string resolvedPath = puttyConnection.GetPuttyBinaryPath();
            Assert.IsTrue(resolvedPath.EndsWith(RESOURCES_PUTTY_EXE));
        }
    }
}
