using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Terminals.Common.Forms;
using Terminals.Connections;

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
            // TODO finish adding to the menu
            //separatorIndex = this.toolsToolStripMenuItem.DropDownItems.IndexOf(this.toolStripSeparator9);
            //this.toolsToolStripMenuItem.DropDownItems.Insert(separatorIndex, newMenu);
        }

        private void OpenSshAgentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenPuttyTool("pageant.exe");
        }

        private void OpenSshKeygenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenPuttyTool("puttygen.exe");
        }

        private void OpenPuttyTool(string name)
        {
            string pluginDirectory = typeof(PuttyMenuVisitor).Assembly.Location;
            string path = Path.Combine(pluginDirectory, "Putty", "Resources", name);
            ExternalLinks.OpenPath(path);
        }
    }
}
