using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;

namespace Tests.UserInterface
{
    [TestClass]
    public class VmrcMenuVisitorTests
    {
        private Mock<IConnection> mockConnection;

        private Mock<ICurrenctConnectionProvider> mockProvider;

        private ToolStrip toolbar;

        private VmrcMenuVisitor menuVisitor;

        [TestMethod]
        public void EmptyToolBar_UpdateMenu_CreatesNewButtons()
        {
            this.menuVisitor.Visit(this.toolbar);
            int itemsCount = this.toolbar.Items.Count;
            Assert.AreEqual(2, itemsCount, "First visit should update the menu by addint its own menu items");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockConnection = new Mock<IConnection>();
            this.mockProvider = new Mock<ICurrenctConnectionProvider>();
            this.mockProvider.SetupGet(p => p.CurrentConnection)
                .Returns(() => this.mockConnection.Object); // other connection, than null
            this.toolbar = new ToolStrip();
            this.menuVisitor = new VmrcMenuVisitor(this.mockProvider.Object);
        }
    }
}
