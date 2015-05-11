using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Network.Servers;

namespace Terminals.Connections
{
    internal class RdpMenuVisitor : IToolbarExtender
    {
        internal const string TERMINAL_SERVER_MENU_BUTTON_NAME = "TerminalServerMenuButton";

        private readonly ICurrenctConnectionProvider connectionProvider;

        private ToolStripDropDownButton TerminalServerMenuButton;

        public RdpMenuVisitor(ICurrenctConnectionProvider connectionProvider)
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
            this.TerminalServerMenuButton.Image = Properties.Resources.server_network;
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
                var sessions = new ToolStripMenuItem(Program.Resources.GetString("Sessions"));
                sessions.Tag = currentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(sessions);
                var svr = new ToolStripMenuItem(Program.Resources.GetString("Server"));
                svr.Tag = currentConnection.Server;
                TerminalServerMenuButton.DropDownItems.Add(svr);
                var sd = new ToolStripMenuItem(Program.Resources.GetString("Shutdown"));
                sd.Click += new EventHandler(sd_Click);
                sd.Tag = currentConnection.Server;
                svr.DropDownItems.Add(sd);
                var rb = new ToolStripMenuItem(Program.Resources.GetString("Reboot"));
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
                            var msg = new ToolStripMenuItem(Program.Resources.GetString("SendMessage"));
                            msg.Click += new EventHandler(sd_Click);
                            msg.Tag = session;
                            sess.DropDownItems.Add(msg);

                            var lo = new ToolStripMenuItem(Program.Resources.GetString("Logoff"));
                            lo.Click += new EventHandler(sd_Click);
                            lo.Tag = session;
                            sess.DropDownItems.Add(lo);

                            if (session.IsTheActiveSession)
                            {
                                var lo1 = new ToolStripMenuItem(Program.Resources.GetString("Logoff"));
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
                if (menu.Text == Program.Resources.GetString("Shutdown"))
                {
                    var server = menu.Tag as TerminalServices.TerminalServer;
                    if (server != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttoshutthismachineoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, false);
                }
                else if (menu.Text == Program.Resources.GetString("Reboot"))
                {
                    var server = menu.Tag as TerminalServices.TerminalServer;
                    if (server != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttorebootthismachine"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.ShutdownSystem(server, true);
                }
                else if (menu.Text == Program.Resources.GetString("Logoff"))
                {
                    var session = menu.Tag as TerminalServices.Session;
                    if (session != null && MessageBox.Show(Program.Resources.GetString("Areyousureyouwanttologthissessionoff"), Program.Resources.GetString("Confirmation"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        TerminalServices.TerminalServicesAPI.LogOffSession(session, false);
                }
                else if (menu.Text == Program.Resources.GetString("SendMessage"))
                {
                    var session = menu.Tag as TerminalServices.Session;
                    TerminalServerManager.SendMessageToSession(session);
                }
            }
        }
    }
}