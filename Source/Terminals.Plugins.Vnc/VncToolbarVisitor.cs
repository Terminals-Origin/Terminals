using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Plugins.Vnc.Properties;

namespace Terminals.Connections
{
    internal class VncToolbarVisitor : IToolbarExtender
    {
        internal const string VNC_ACTION_BUTTON_NAME = "vncActionButton";

        private readonly ICurrenctConnectionProvider connectionProvider;
        private ToolStripDropDownButton vncActionButton;

        private ToolStripMenuItem sendALTKeyToolStripMenuItem;

        private ToolStripMenuItem sendALTF4KeyToolStripMenuItem;

        private ToolStripMenuItem sendCTRLKeyToolStripMenuItem;

        private ToolStripMenuItem sendCTRLESCKeysToolStripMenuItem;

        private ToolStripMenuItem sentCTRLALTDELETEKeysToolStripMenuItem;

        public VncToolbarVisitor(ICurrenctConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public void Visit(ToolStrip standardToolbar)
        {
            this.EnusereMenuCreated(standardToolbar);

            bool commandsAvailable = this.connectionProvider.CurrentConnection is VNCConnection;
            this.vncActionButton.Visible = commandsAvailable;
        }

        private void EnusereMenuCreated(ToolStrip standardToolbar)
        {
            if (standardToolbar.Items[VNC_ACTION_BUTTON_NAME] == null)
                this.CreateViewOnlyButton(standardToolbar);
        }

        private void CreateViewOnlyButton(ToolStrip standardToolbar)
        {
            this.CreateAltKeyMenuItem();
            this.CreateAltF4MenuItem();
            this.CreateCtrlKeyMenuItem();
            this.CreateCtrlEscKeysMenuItem();
            this.CreateCtrlAltDelKeyMenuItem();
            this.CreateActionButton();
            standardToolbar.Items.Add(this.vncActionButton);
        }

        private void CreateActionButton()
        {
            this.vncActionButton = new ToolStripDropDownButton();
            this.vncActionButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.vncActionButton.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.sendALTKeyToolStripMenuItem,
                this.sendALTF4KeyToolStripMenuItem,
                this.sendCTRLKeyToolStripMenuItem,
                this.sendCTRLESCKeysToolStripMenuItem,
                this.sentCTRLALTDELETEKeysToolStripMenuItem
            });
            this.vncActionButton.Image = Resources.vnc;
            this.vncActionButton.ImageTransparentColor = Color.Magenta;
            this.vncActionButton.Name = VNC_ACTION_BUTTON_NAME;
            this.vncActionButton.Size = new Size(29, 22);
            this.vncActionButton.Text = "VNC actions";
            this.vncActionButton.Visible = false;
        }

        private void CreateAltKeyMenuItem()
        {
            this.sendALTKeyToolStripMenuItem = new ToolStripMenuItem();
            this.sendALTKeyToolStripMenuItem.Name = "sendALTKeyToolStripMenuItem";
            this.sendALTKeyToolStripMenuItem.Size = new Size(202, 22);
            this.sendALTKeyToolStripMenuItem.Text = "Send ALT Key";
            this.sendALTKeyToolStripMenuItem.Click += new EventHandler(this.SendAltKeyToolStripMenuItem_Click);
        }

        private void CreateAltF4MenuItem()
        {
            this.sendALTF4KeyToolStripMenuItem = new ToolStripMenuItem();
            this.sendALTF4KeyToolStripMenuItem.Name = "sendALTF4KeyToolStripMenuItem";
            this.sendALTF4KeyToolStripMenuItem.Size = new Size(202, 22);
            this.sendALTF4KeyToolStripMenuItem.Text = "Send ALT-F4 Keys";
            this.sendALTF4KeyToolStripMenuItem.Click += new EventHandler(this.SendAltKeyToolStripMenuItem_Click);
        }

        private void CreateCtrlKeyMenuItem()
        {
            this.sendCTRLKeyToolStripMenuItem = new ToolStripMenuItem();
            this.sendCTRLKeyToolStripMenuItem.Name = "sendCTRLKeyToolStripMenuItem";
            this.sendCTRLKeyToolStripMenuItem.Size = new Size(202, 22);
            this.sendCTRLKeyToolStripMenuItem.Text = "Send CTRL Key";
            this.sendCTRLKeyToolStripMenuItem.Click += new EventHandler(this.SendAltKeyToolStripMenuItem_Click);
        }

        private void CreateCtrlEscKeysMenuItem()
        {
            this.sendCTRLESCKeysToolStripMenuItem = new ToolStripMenuItem();
            this.sendCTRLESCKeysToolStripMenuItem.Name = "sendCTRLESCKeysToolStripMenuItem";
            this.sendCTRLESCKeysToolStripMenuItem.Size = new Size(202, 22);
            this.sendCTRLESCKeysToolStripMenuItem.Text = "Send CTRL ESC Keys";
            this.sendCTRLESCKeysToolStripMenuItem.Click += new EventHandler(this.SendAltKeyToolStripMenuItem_Click);
        }

        private void CreateCtrlAltDelKeyMenuItem()
        {
            this.sentCTRLALTDELETEKeysToolStripMenuItem = new ToolStripMenuItem();
            this.sentCTRLALTDELETEKeysToolStripMenuItem.Name = "sentCTRLALTDELETEKeysToolStripMenuItem";
            this.sentCTRLALTDELETEKeysToolStripMenuItem.Size = new Size(202, 22);
            this.sentCTRLALTDELETEKeysToolStripMenuItem.Text = "Sent CTRL ALT DEL Keys";
            this.sentCTRLALTDELETEKeysToolStripMenuItem.Click += new EventHandler(this.SendAltKeyToolStripMenuItem_Click);
        }

        private void SendAltKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                String key = menuItem.Text;
                var vnc = this.connectionProvider.CurrentConnection as VNCConnection;
                if (vnc != null)
                {
                    if (key == sendALTF4KeyToolStripMenuItem.Text)
                    {
                        vnc.SendSpecialKeys(VncSharp.SpecialKeys.AltF4);
                    }
                    else if (key == sendALTKeyToolStripMenuItem.Text)
                    {
                        vnc.SendSpecialKeys(VncSharp.SpecialKeys.Alt);
                    }
                    else if (key == sendCTRLESCKeysToolStripMenuItem.Text)
                    {
                        vnc.SendSpecialKeys(VncSharp.SpecialKeys.CtrlEsc);
                    }
                    else if (key == sendCTRLKeyToolStripMenuItem.Text)
                    {
                        vnc.SendSpecialKeys(VncSharp.SpecialKeys.Ctrl);
                    }
                    else if (key == sentCTRLALTDELETEKeysToolStripMenuItem.Text)
                    {
                        vnc.SendSpecialKeys(VncSharp.SpecialKeys.CtrlAltDel);
                    }
                }
            }
        }
    }
}