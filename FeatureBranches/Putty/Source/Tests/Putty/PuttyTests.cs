using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{

    [TestClass]
    public class PuttyTests
    {

        [TestMethod]
        public void EnsurePuttyBinary()
        {
            PuttyConnection puttyConnection = new PuttyConnection();
            Assert.IsTrue(File.Exists(puttyConnection.GetPuttyBinaryPath()));
        }



    }
}
