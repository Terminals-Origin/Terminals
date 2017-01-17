using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
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
            Assert.IsTrue(puttyConnection.GetPuttyBinaryPath().EndsWith(@"Resources\" + PuttyConnection.PUTTY_BINARY));
        }



    }
}
