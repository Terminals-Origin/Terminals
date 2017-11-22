using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;

namespace Tests.UserInterface
{
    [TestClass]
    public class RdpToolbarVisitorTests
    {
        [TestMethod]
        public void EmptyToolBar_UpdateMenu_CreatesNewButtons()
        {
            var sut = new MenuVisitorSut();
            var menuVisitor = new RdpToolbarVisitor(sut.MockProvider.Object);
            menuVisitor.Visit(sut.Toolbar);
            // the sub menu is created dynamicaly
            var serveButton = sut.Toolbar.Items[0] as ToolStripDropDownButton;
            Assert.IsNotNull(serveButton, "First visit should update the menu by add its own drop donw menu button");
        }
    }
}
