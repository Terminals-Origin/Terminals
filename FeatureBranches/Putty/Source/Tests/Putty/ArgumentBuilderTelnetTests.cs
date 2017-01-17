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
        public void ValidFavoriteAndCredentials_Build_ReturnsAllTelnetArguments()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            favorite.Setup(p => p.Protocol).Returns(TelnetConnectionPlugin.TELNET);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new TelnetOptions() { SessionName = "session" });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.AreEqual(" -load \"session\" -telnet server", arguments, "All arguments need to be used from favorite and its credentials");
        }



        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExceptionWhenNoServer()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            favorite.Setup(p => p.Protocol).Returns(TelnetConnectionPlugin.TELNET);
            favorite.Setup(p => p.ServerName).Returns(default(string));
            favorite.Setup(p => p.ProtocolProperties).Returns(new TelnetOptions() { SessionName = "session" });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
        }

    }
}

