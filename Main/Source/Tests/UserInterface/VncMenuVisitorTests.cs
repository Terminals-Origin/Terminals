using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;

namespace Tests.UserInterface
{
    [TestClass]
    public class VncMenuVisitorTests
    {
        private Mock<IConnection> mockConnection;

        private Mock<ICurrenctConnectionProvider> mockProvider;

        private ToolStrip toolbar;

        private VncMenuVisitor menuVisitor;

        [TestMethod]
        public void EmptyToolBar_UpdateMenu_CreatesNewButtons()
        {
            this.menuVisitor.Visit(this.toolbar);
            int itemsCount = ((ToolStripDropDownButton)this.toolbar.Items[0]).DropDownItems.Count;
            Assert.AreEqual(5, itemsCount, "First visit should update the menu by add its own drop donw menu items");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockConnection = new Mock<IConnection>();
            this.mockProvider = new Mock<ICurrenctConnectionProvider>();
            this.mockProvider.SetupGet(p => p.CurrentConnection)
                .Returns(() => this.mockConnection.Object); // other connection, than null
            this.toolbar = new ToolStrip();
            this.menuVisitor = new VncMenuVisitor(this.mockProvider.Object);
        }
    }
}
