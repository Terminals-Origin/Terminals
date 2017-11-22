using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Services;

namespace Terminals
{
    internal class PuttyMenuVisitor
    {
        private void InitializeToolsMenu()
        {
            ToolStripMenuItem openSshAgentToolStripMenuItem = new ToolStripMenuItem();
            openSshAgentToolStripMenuItem.Name = "openSshAgentToolStripMenuItem";
            openSshAgentToolStripMenuItem.Size = new Size(276, 22);
            openSshAgentToolStripMenuItem.Text = "Open SSH Agent";
            openSshAgentToolStripMenuItem.ToolTipText = "Open SSH Agent";
            openSshAgentToolStripMenuItem.Click += new EventHandler(this.OpenSshAgentToolStripMenuItem_Click);
            AddToMenu(openSshAgentToolStripMenuItem);

            ToolStripMenuItem openSshKeygenToolStripMenuItem = new ToolStripMenuItem();
            openSshKeygenToolStripMenuItem.Name = "openSshKeygenToolStripMenuItem";
            openSshKeygenToolStripMenuItem.Size = new Size(276, 22);
            openSshKeygenToolStripMenuItem.Text = "Open SSH Keygen";
            openSshKeygenToolStripMenuItem.ToolTipText = "Open SSH Keygen";
            openSshKeygenToolStripMenuItem.Click += new EventHandler(this.OpenSshKeygenToolStripMenuItem_Click);
            AddToMenu(openSshKeygenToolStripMenuItem);

        }

        private void AddToMenu(ToolStripMenuItem newMenu)
        {
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
            string path = Path.Combine(PluginsLoader.FindBasePluginDirectory(), "Putty", "Resources", name);
            ExternalLinks.OpenPath(path);
        }
    }
}
