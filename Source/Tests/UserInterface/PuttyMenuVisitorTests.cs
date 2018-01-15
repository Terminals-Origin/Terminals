using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;

namespace Tests.UserInterface
{
	[TestClass]
	public class PuttyMenuVisitorTests
    {
		[TestMethod]
		public void Adds_SshAgentMenuItem()
		{
            AssertMenuItemCreated("Agent");
		}

        [TestMethod]
        public void Adds_SshKeygenMenuItem()
        {
            AssertMenuItemCreated("Keygen");
        }

        private static void AssertMenuItemCreated(string expectedText)
        {
            MenuStrip menu = CreateMenu();
            var menuVisitor = new PuttyMenuVisitor();
            menuVisitor.Visit(menu);
            bool hasAnyMenu = menu.Items.OfType<ToolStripMenuItem>().Any(mi => mi.DropDownItems.OfType<ToolStripMenuItem>()
                .Any(chmi => chmi.Text.Contains(expectedText)));
            Assert.IsTrue(hasAnyMenu, "Menu items need to be added when populating menu.");
        }

        private static MenuStrip CreateMenu()
        {
            var menu = new MenuStrip();
            var toolsMenu = new ToolStripMenuItem()
            {
                Name = "tools"
            };

            menu.Items.Add(toolsMenu);
            return menu;
        }
    }
}
