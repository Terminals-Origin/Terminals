using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Terminals.Data;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{
    [TestClass]
    public class ArgumentBuilderTelnetTests
    {
        [TestMethod]
        public void SessionWithServerName_Build_UsesSessionAndServerNameArgument()
        {
            Mock<IGuardedSecurity> credentials = SetupCredentials();
            Mock<IFavorite> favorite = SetupFavorite();

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.AreEqual(" -load \"session\" -telnet server", arguments, "Both serverName and session need to be used when provided.");
        }

        [ExpectedException(typeof(ArgumentException), "The builder cant parse argument, if not supported options are provided.")]
        [TestMethod]
        public void InvalidProtocol_Build_Throws()
        {
            Mock<IGuardedSecurity> credentials = SetupCredentials();
            Mock<IFavorite> favorite = SetupFavorite();
            favorite.Setup(p => p.Protocol).Returns("Unknown");

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            builder.Build();
        }

        [TestMethod]
        public void VerboseTrue_Build_AddsVerboseArgument()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new TelnetOptions() { SessionName = "session", Verbose = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -v "), "-v should be present.");
        }

        private static Mock<IFavorite> SetupFavorite()
        {
            var favorite = new Mock<IFavorite>();

            favorite.Setup(p => p.Protocol).Returns(TelnetConnectionPlugin.TELNET);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new TelnetOptions() { SessionName = "session" });

            return favorite;
        }

        private static Mock<IGuardedSecurity> SetupCredentials()
        {
            var credentials = new Mock<IGuardedSecurity>();
            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");
            return credentials;
        }
    }
}