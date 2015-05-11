using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;

namespace Tests.UserInterface
{
    [TestClass]
    public class VncMenuVisitorTests
    {
        [TestMethod]
        public void EmptyToolBar_UpdateMenu_CreatesNewButtons()
        {
            var sut = new MenuVisitorSut();
            var menuVisitor = new VncMenuVisitor(sut.MockProvider.Object);
            menuVisitor.Visit(sut.Toolbar);
            int itemsCount = ((ToolStripDropDownButton)sut.Toolbar.Items[0]).DropDownItems.Count;
            Assert.AreEqual(5, itemsCount, "First visit should update the menu by add its own drop donw menu items");
        }
    }
}
