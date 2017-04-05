using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{
    [TestClass]
    public class ArgumentBuilderTelnetTests : PuttyArgumentBuilderTests<TelnetOptions>
    {
        [TestInitialize]
        public void Setup()
        {
            this.Protocol = TelnetConnectionPlugin.TELNET;
            this.Options = new TelnetOptions()
            {
                SessionName = "session"
            };
        }

        [TestMethod]
        public void SessionWithServerName_Build_UsesSessionAndServerNameArgument()
        {
            string arguments = this.BuildArguments();
            Assert.AreEqual(" -load \"session\" -telnet server", arguments, "Both serverName and session need to be used when provided.");
        }
    }
}