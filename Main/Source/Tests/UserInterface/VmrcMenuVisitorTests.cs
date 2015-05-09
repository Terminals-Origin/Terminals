using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Terminals.Connections;

namespace Tests.UserInterface
{
    [TestClass]
    public class VmrcMenuVisitorTests
    {
        [TestMethod]
        public void EmptyToolBar_Visit_CreatesNewButtons()
        {
            var mockConnection = new Mock<IConnection>();
            var mockProvider = new Mock<ICurrenctConnectionProvider>();
            mockProvider.SetupGet(p => p.CurrentConnection).Returns(mockConnection.Object);

            var menuVisitor = new VmrcMenuVisitor(mockProvider.Object);
            
            ToolStrip standardToolbar = new ToolStrip();
            menuVisitor.UpdateMenu(standardToolbar);
            int itemsCount = standardToolbar.Items.Count;
            Assert.AreEqual(2, itemsCount, "First visit should update the menu by addint its own menu items");
        }
    }
}
