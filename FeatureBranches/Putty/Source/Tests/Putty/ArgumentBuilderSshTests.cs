using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Terminals.Data;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{
    [TestClass]
    public class ArgumentBuilderTests
    {
        private static Mock<IFavorite> SetupFavorite()
        {
            var favorite = new Mock<IFavorite>();

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions()
            {
                SessionName = "session",
                EnableCompression = false,
                X11Forwarding = false,
                EnablePagentAuthentication = false,
                EnablePagentForwarding = false,
                SshVersion = SshVersion.SshNegotiate,
                Verbose = false
            });
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
        public void ValidFavoriteAndCredentials_NoCustomParameters()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.AreEqual(" -load \"session\" -ssh -l user -pw password -noagent -a -x server", arguments, "Arguments weren't built as expected");
        }

        [TestMethod]
        public void Check_X11Forwarding()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { X11Forwarding = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -X "), "-X for X11 should be present.");
        }

        [TestMethod]
        public void Check_PagentAuthentication()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { EnablePagentAuthentication = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -agent"), "-agent should be present.");
        }

        [TestMethod]
        public void Check_PagentForwarding()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { EnablePagentForwarding = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -A "), "-A should be present.");
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExceptionWhenNoServer()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ServerName).Returns(default(string));
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session" });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
        }
        [TestMethod]
        public void Check_Compression()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", EnableCompression = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -C "), "-C for compression should be present.");
        }

        [TestMethod]
        public void Check_SshVersion1()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", SshVersion = SshVersion.SshVersion1 });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -1 "), "-1 should be present.");
        }

        [TestMethod]
        public void Check_SshVersion2()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", SshVersion = SshVersion.SshVersion2 });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -2 "), "-2 should be present.");
        }

        [TestMethod]
        public void Check_SshVersionNegotiate()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", SshVersion = SshVersion.SshNegotiate });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(!arguments.Contains(" -1 "), "-1 should NOT be present.");
            Assert.IsTrue(!arguments.Contains(" -2 "), "-2 should NOT be present.");
        }

        [TestMethod]
        public void Check_Verbose()
        {
            var credentials = SetupCredentials();
            var favorite = SetupFavorite();

            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", Verbose = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.Contains(" -v "), "-v should be present.");
        }
    }
}

