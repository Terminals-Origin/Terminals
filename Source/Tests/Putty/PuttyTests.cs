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
            const string RESOURCES_PUTTY_EXE = @"Resources\" + Executables.PUTTY_BINARY;
            string resolvedPath = Executables.GetPuttyBinaryPath();
            Assert.IsTrue(resolvedPath.EndsWith(RESOURCES_PUTTY_EXE));
        }
    }
}
