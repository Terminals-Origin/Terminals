using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            var builder = new ArgumentsBuilder(credentials.Object, favorite.Object);
            string arguments = builder.Build();
            Assert.AreEqual("commandline", arguments, "All arguments need to be used from favorite and its credentials");
        }
    }
}
