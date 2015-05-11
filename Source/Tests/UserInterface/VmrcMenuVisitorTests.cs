using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;

namespace Tests.UserInterface
{
    [TestClass]
    public class VmrcMenuVisitorTests
    {
        [TestMethod]
        public void EmptyToolBar_UpdateMenu_CreatesNewButtons()
        {
            var sut = new MenuVisitorSut();
            var menuVisitor = new VmrcMenuVisitor(sut.MockProvider.Object);
            menuVisitor.Visit(sut.Toolbar);
            int itemsCount = sut.Toolbar.Items.Count;
            Assert.AreEqual(2, itemsCount, "First visit should update the menu by addint its own menu items");
        }
    }
}
