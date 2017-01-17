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
        public void ValidFavoriteAndCredentials_Build_ReturnsAllSshArguments()
        {
            var credentials = new Mock<IGuardedSecurity>();
            var favorite = new Mock<IFavorite>();

            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");

            favorite.Setup(p => p.Protocol).Returns(SshConnectionPlugin.SSH);
            favorite.Setup(p => p.ServerName).Returns("server");
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", EnableCompression = false, X11Forwarding = false });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.AreEqual(" -load \"session\" -ssh -l user -pw password -x server", arguments, "All arguments need to be used from favorite and its credentials");
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
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", EnableCompression = false, X11Forwarding = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -X ") > -1, "-X for X11 should be present.");
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
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", EnableCompression = false, X11Forwarding = false });

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
            favorite.Setup(p => p.ProtocolProperties).Returns(new SshOptions() { SessionName = "session", EnableCompression = true, X11Forwarding = true });

            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);

            string arguments = builder.Build();
            Assert.IsTrue(arguments.IndexOf(" -C ") > -1, "-C for compression should be present.");
        }
    }
}

