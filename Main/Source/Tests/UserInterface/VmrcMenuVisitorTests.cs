using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;

namespace Tests.UserInterface
{
    [TestClass]
    public class VmrcMenuVisitorTests
    {
        private const string ACTIVE_BUTTONS_MESSAGE = "Its commands should be visible only in case the active connection is VMRC";

        private Mock<IConnection> mockConnection;

        private Mock<ICurrenctConnectionProvider> mockProvider;

        private ToolStrip toolbar;

        private VmrcMenuVisitor menuVisitor;

        [TestMethod]
        public void EmptyToolBar_UpdateMenu_CreatesNewButtons()
        {
            this.menuVisitor.UpdateMenu(this.toolbar);
            int itemsCount = this.toolbar.Items.Count;
            Assert.AreEqual(2, itemsCount, "First visit should update the menu by addint its own menu items");
        }

        [TestMethod]
        public void NoConnection_UpdateMenu_HidesButtons()
        {
            this.menuVisitor.UpdateMenu(this.toolbar);
            bool allHiden = this.toolbar.Items.OfType<ToolStripButton>().All(mi => !mi.Visible);
            Assert.IsTrue(allHiden, ACTIVE_BUTTONS_MESSAGE);
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
