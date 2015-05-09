using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Connections;

namespace Tests.UserInterface
{
    [TestClass]
    public class VmrcMenuVisitorTests
    {
        [TestMethod]
        public void EmptyToolBar_Visit_CreatesNewButtons()
        {
            ToolStrip standardToolbar = new ToolStrip();
            var menuVisitor = new VmrcMenuVisitor();
            menuVisitor.UpdateMenu(standardToolbar);
            int itemsCount = standardToolbar.Items.Count;
            Assert.AreEqual(2, itemsCount, "First visit should update the menu by addint its own menu items");
        }
    }
}
