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
        [TestMethod]
        public void ValidFavoriteAndCredentials_NoCustomParameters()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() {
                SessionName = "session",
                EnableCompression = false,
                X11Forwarding = false,
                EnablePagentAuthentication = false,
                EnablePagentForwarding = false,
                SshVersion = SshVersion.SshNegotiate,
                Verbose = false
            });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.AreEqual(" -load \"session\" -ssh -l user -pw password -noagent -a -x server", arguments, "Arguments weren't built as expected");
        }

        [TestMethod]
        public void Check_X11Forwarding()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { X11Forwarding = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -X ") > -1, "-X for X11 should be present.");
        }

        [TestMethod]
        public void Check_PagentAuthentication()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { EnablePagentAuthentication = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -agent") > -1, "-agent should be present.");
        }

        [TestMethod]
        public void Check_PagentForwarding()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { EnablePagentForwarding = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -A ") > -1, "-A should be present.");
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExceptionWhenNoServer()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns(default(string));
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session" });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
        }
        [TestMethod]
        public void Check_Compression()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", EnableCompression = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -C ") > -1, "-C for compression should be present.");
        }

        [TestMethod]
        public void Check_SshVersion1()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", SshVersion = SshVersion.SshVersion1 });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -1 ") > -1, "-1 should be present.");
        }

        [TestMethod]
        public void Check_SshVersion2()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", SshVersion = SshVersion.SshVersion2 });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -2 ") > -1, "-2 should be present.");
        }

        [TestMethod]
        public void Check_SshVersionNegotiate()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", SshVersion = SshVersion.SshNegotiate });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -1 ") == -1, "-1 should NOT be present.");
            Assert.IsTrue(arguments.IndexOf(" -2 ") == -1, "-2 should NOT be present.");
        }

        [TestMethod]
        public void Check_Verbose()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", Verbose = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -v ") > -1, "-v should be present.");
        }
    }
}

