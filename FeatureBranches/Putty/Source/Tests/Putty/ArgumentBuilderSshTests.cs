using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{
    [TestClass]
    public class ArgumentBuilderTests : PuttyArgumentBuilderTests<SshOptions>
    {
        [TestInitialize]
        public void Setup()
        {
            this.Protocol = SshConnectionPlugin.SSH;

            this.Options = new SshOptions()
            {
                SessionName = "session",
                EnableCompression = false,
                X11Forwarding = false,
                EnablePagentAuthentication = false,
                EnablePagentForwarding = false,
                SshVersion = SshVersion.SshNegotiate,
                Verbose = false
            };
        }

        [TestMethod]
        public void ValidFavoriteAndCredentials__Build_GeneratesLoginAndPasswordAttributes()
        {
            string arguments = this.BuildArguments();
            Assert.AreEqual(" -load \"session\" -ssh -l user -pw password -noagent -a -x server", arguments, "Arguments weren't built as expected");
        }

        [TestMethod]
        public void X11ForwardingTrue_Build_GeneratesXAttribute()
        {
            this.Options.X11Forwarding = true;
            string arguments = this.BuildArguments();
            Assert.IsTrue(arguments.Contains(" -X "), "-X for X11 should be present.");
        }

        [TestMethod]
        public void EnablePagentAuthenticationTrue_Build_GeneratesAgentAttribute()
        {
            this.Options.EnablePagentAuthentication = true;
            string arguments = this.BuildArguments();
            Assert.IsTrue(arguments.Contains(" -agent"), "-agent should be present.");
        }

        [TestMethod]
        public void EnablePagentForwardingTrue_Build_GeneratesAAttribute()
        {
            this.Options.EnablePagentForwarding = true;
            string arguments = this.BuildArguments();
            Assert.IsTrue(arguments.Contains(" -A "), "-A should be present.");
        }

        [TestMethod]
        public void EnableCompressionTrue_Build_GeneratesCompressionAttribute()
        {
            this.Options.EnableCompression = true;
            string arguments = this.BuildArguments();
            Assert.IsTrue(arguments.Contains(" -C "), "-C for compression should be present.");
        }

        [TestMethod]
        public void SshVersion1_Build_Generates1Attribute()
        {
            this.Options.SshVersion = SshVersion.SshVersion1;
            string arguments = this.BuildArguments();
            Assert.IsTrue(arguments.Contains(" -1 "), "-1 should be present.");
        }

        [TestMethod]
        public void SshVersion2_Build_Generates2Attribute()
        {
            this.Options.SshVersion = SshVersion.SshVersion2;
            string arguments = this.BuildArguments();
            Assert.IsTrue(arguments.Contains(" -2 "), "-2 should be present.");
        }

        [TestMethod]
        public void SshNegotiate_Build_GeneratesNoVersionAttribute()
        {
            this.Options.SshVersion = SshVersion.SshNegotiate;
            string arguments = this.BuildArguments();
            Assert.IsTrue(!arguments.Contains(" -1 "), "-1 should NOT be present.");
            Assert.IsTrue(!arguments.Contains(" -2 "), "-2 should NOT be present.");
        }
    }
}

