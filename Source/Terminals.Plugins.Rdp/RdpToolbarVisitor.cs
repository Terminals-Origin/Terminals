using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Plugins.Rdp.Properties;
using Terminals.TerminalServices;

namespace Terminals.Connections
{
    internal class RdpToolbarVisitor : IToolbarExtender
    {
        internal const string TERMINAL_SERVER_MENU_BUTTON_NAME = "TerminalServerMenuButton";

        private readonly ICurrenctConnectionProvider connectionProvider;

        private ToolStripDropDownButton TerminalServerMenuButton;

        public RdpToolbarVisitor(ICurrenctConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public void Visit(ToolStrip standardToolbar)
        {
            this.EnusereMenuCreated(standardToolbar);

            bool commandsAvailable = this.connectionProvider.CurrentConnection is RDPConnection;
            this.TerminalServerMenuButton.Visible = commandsAvailable;
        }

        private void EnusereMenuCreated(ToolStrip standardToolbar)
        {
            if (standardToolbar.Items[TERMINAL_SERVER_MENU_BUTTON_NAME] == null)
                this.CreateAdminSwitchButton(standardToolbar);
        }

        private void CreateAdminSwitchButton(ToolStrip standardToolbar)
        {
            this.TerminalServerMenuButton = new ToolStripDropDownButton();
            this.TerminalServerMenuButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TerminalServerMenuButton.Image = Resources.server_network;
            this.TerminalServerMenuButton.ImageTransparentColor = Color.Magenta;
            this.TerminalServerMenuButton.Name = TERMINAL_SERVER_MENU_BUTTON_NAME;
            this.TerminalServerMenuButton.Size = new Size(29, 22);
            this.TerminalServerMenuButton.Text = "Terminal Server";
            this.TerminalServerMenuButton.DropDownOpening += new EventHandler(this.TerminalServerMenuButton_DropDownOpening);
            standardToolbar.Items.Add(this.TerminalServerMenuButton);
        }

        private void TerminalServerMenuButton_DropDownOpening(object sender, EventArgs e)
        {
            TerminalServerMenuButton.DropDownItems.Clear();
            var currentConnection = this.connectionProvider.CurrentConnection as RDPConnection;
            if (currentConnection != null && currentConnection.IsTerminalServer)
            {
                var sessions = new ToolStripMenuItem(Resources.Sessions);
                sessions.Tag = currentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(sessions);
                var svr = new ToolStripMenuItem(Resources.Server);
                svr.Tag = currentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(svr);
                var sd = new ToolStripMenuItem(Resources.Shutdown);
                sd.Click += new EventHandler(sd_Click);
                sd.Tag = currentConnection.Server;
                svr.DropDownItems.Add(sd);
                var rb = new ToolStripMenuItem(Resources.Reboot);
                rb.Click += new EventHandler(sd_Click);
                rb.Tag = currentConnection.Server;
                svr.DropDownItems.Add(rb);

                if (currentConnection.Server.Sessions != null)
                {
                    foreach (TerminalServices.Session session in currentConnection.Server.Sessions)
                    {
                        if (session.Client.ClientName != "")
                        {
                            var sess = new ToolStripMenuItem(String.Format("{1} - {2} ({0})", session.State.ToString().Replace("WTS", ""), session.Client.ClientName, session.Client.UserName));
                            sess.Tag = session;
                            sessions.DropDownItems.Add(sess);
                            var msg = new ToolStripMenuItem(Resources.SendMessage);
                            msg.Click += new EventHandler(sd_Click);
                            msg.Tag = session;
                            sess.DropDownItems.Add(msg);

                            var lo = new ToolStripMenuItem(Resources.Logoff);
                            lo.Click += new EventHandler(sd_Click);
                            lo.Tag = session;
                            sess.DropDownItems.Add(lo);

                            if (session.IsTheActiveSession)
                            {
                                var lo1 = new ToolStripMenuItem(Resources.Logoff);
                                lo1.Click += new EventHandler(sd_Click);
                                lo1.Tag = session;
                                svr.DropDownItems.Add(lo1);
                            }
                        }
                    }
                }
            }
            else
            {
                TerminalServerMenuButton.Visible = false;
            }
        }

        private void sd_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (menu != null)
            {
                if (menu.Text == Resources.Shutdown)
                {
                    var server = menu.Tag as TerminalServer;
                    if (server != null && MessageBox.Show(Resources.Areyousureyouwanttoshutthismachineoff, Resources.Confirmation, MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServicesAPI.ShutdownSystem(server, false);
                }
                else if (menu.Text == Resources.Reboot)
                {
                    var server = menu.Tag as TerminalServer;
                    if (server != null && MessageBox.Show(Resources.Areyousureyouwanttorebootthismachine, Resources.Confirmation, MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServicesAPI.ShutdownSystem(server, true);
                }
                else if (menu.Text == Resources.Logoff)
                {
                    var session = menu.Tag as Session;
                    if (session != null && MessageBox.Show(Resources.Areyousureyouwanttologthissessionoff, Resources.Confirmation, MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServicesAPI.LogOffSession(session, false);
                }
                else if (menu.Text == Resources.SendMessage)
                {
                    var session = menu.Tag as Session;
                    TerminalServer.SendMessageToSession(session);
                }
            }
        }
    }
}