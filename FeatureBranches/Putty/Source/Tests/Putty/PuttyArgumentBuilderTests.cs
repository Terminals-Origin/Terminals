using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Data;
using Terminals.Plugins.Putty;

namespace Tests.Putty
{
    [TestClass]
    public class PuttyArgumentBuilderTests<TOptions> where TOptions : PuttyOptions
    {
        protected TOptions Options { get; set; }
        protected string Protocol { get; set; }

        [TestMethod]
        public void VerboseTrue_Build_GeneratesVAttribute()
        {
            this.Options.Verbose = true;
            string arguments = this.BuildArguments();
            Assert.IsTrue(arguments.Contains(" -v "), "-v should be present.");
        }

        [ExpectedException(typeof(ArgumentException), "The builder cant parse argument, if not supported options are provided.")]
        [TestMethod]
        public void InvalidProtocol_Build_Throws()
        {
            this.Protocol = "Unknown";
            this.BuildArguments();
        }

        protected string BuildArguments()
        {
            var credentials = this.CreateCredentials();
            var favorite = this.CreateFavorite();
            favorite.Setup(p => p.ProtocolProperties).Returns(this.Options);
            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);
            return builder.Build();
        }

        protected Mock<IFavorite> CreateFavorite()
        {
            var favorite = new Mock<IFavorite>();
            favorite.Setup(p => p.Protocol).Returns(this.Protocol);
            favorite.Setup(p => p.ServerName).Returns("server");
            return favorite;
        }

        protected Mock<IGuardedSecurity> CreateCredentials()
        {
            var credentials = new Mock<IGuardedSecurity>();
            credentials.Setup(a => a.UserName).Returns("user");
            credentials.Setup(a => a.Password).Returns("password");
            return credentials;
        }
    }
}