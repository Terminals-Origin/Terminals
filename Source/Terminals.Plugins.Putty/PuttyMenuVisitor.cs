using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Plugins.Putty;

namespace Terminals
{
    internal class PuttyMenuVisitor : IMenuExtender
    {
        public void Visit(MenuStrip menuStrip)
        {
            ToolStripMenuItem openSshAgentToolStripMenuItem = new ToolStripMenuItem();
            openSshAgentToolStripMenuItem.Name = "openSshAgentToolStripMenuItem";
            openSshAgentToolStripMenuItem.Size = new Size(276, 22);
            openSshAgentToolStripMenuItem.Text = "Open SSH Agent";
            openSshAgentToolStripMenuItem.ToolTipText = "Open SSH Agent";
            openSshAgentToolStripMenuItem.Click += new EventHandler(this.OpenSshAgentToolStripMenuItem_Click);
            AddToMenu(menuStrip, openSshAgentToolStripMenuItem);

            ToolStripMenuItem openSshKeygenToolStripMenuItem = new ToolStripMenuItem();
            openSshKeygenToolStripMenuItem.Name = "openSshKeygenToolStripMenuItem";
            openSshKeygenToolStripMenuItem.Size = new Size(276, 22);
            openSshKeygenToolStripMenuItem.Text = "Open SSH Keygen";
            openSshKeygenToolStripMenuItem.ToolTipText = "Open SSH Keygen";
            openSshKeygenToolStripMenuItem.Click += new EventHandler(this.OpenSshKeygenToolStripMenuItem_Click);
            AddToMenu(menuStrip, openSshKeygenToolStripMenuItem);

        }

        private void AddToMenu(MenuStrip menuStrip, ToolStripMenuItem newMenu)
        {
            var toolsMenu = menuStrip.Items.OfType<ToolStripMenuItem>()
                .FirstOrDefault(tm => tm.Name.Contains("tools"));

            if (toolsMenu != null)
                toolsMenu.DropDownItems.Add(newMenu);
        }

        private void OpenSshAgentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Executables.LaunchPageant();
        }

        private void OpenSshKeygenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Executables.LaunchKeyGen();
        }
    }
}
