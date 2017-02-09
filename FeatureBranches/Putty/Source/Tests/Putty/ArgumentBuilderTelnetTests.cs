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


        [TestMethod]
        public void LoadSession()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.AreEqual(" -load \"session\" -telnet server", arguments, "Arguments weren't built as expected");
        }



        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExceptionWhenNoServer()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ServerName).Returns(default(string));

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
        }

        [TestMethod]
        public void Check_Verbose()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new TelnetOptions() { SessionName = "session", Verbose = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -v "), "-v should be present.");
        }
    }
}

