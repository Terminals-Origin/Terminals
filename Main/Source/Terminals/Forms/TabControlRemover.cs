using System;
using System.Linq;
using System.Windows.Forms;
using TabControl;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Forms;

namespace Terminals
{
    /// <summary>
    /// Extract closing tab related method from MainForm
    /// </summary>
    internal class TabControlRemover
    {
        private delegate void InvokeCloseTabPage(TabControlItem tabPage);

        private readonly Settings settings;

        private readonly MainForm mainForm;

        private readonly TabControl.TabControl tcTerminals;

        internal TabControlRemover(Settings settings, MainForm mainForm, TabControl.TabControl tcTerminals)
        {
            this.settings = settings;
            this.mainForm = mainForm;
            this.tcTerminals = tcTerminals;
            this.tcTerminals.TabControlItemClosing += new TabControlItemClosingHandler(this.TcTerminals_TabControlItemClosing);
            this.tcTerminals.TabControlItemClosed += new EventHandler(this.TcTerminals_TabControlItemClosed);
        }

        internal void Disconnect()
        {
            try
            {
                TabControlItem tabToClose = this.tcTerminals.SelectedItem;
                if (this.tcTerminals.Items.Contains(tabToClose))
                    this.tcTerminals.CloseTab(tabToClose);
            }
            catch (Exception exc)
            {
                Logging.Error("Disconnecting a tab threw an exception", exc);
            }
        }

        private void TcTerminals_TabControlItemClosed(object sender, EventArgs e)
        {
            this.CloseTabControlItem();
        }

        private void TcTerminals_TabControlItemClosing(TabControlItemClosingEventArgs e)
        {
            IConnection currentConnection = this.mainForm.CurrentConnection;
            if (currentConnection != null && currentConnection.Connected)
            {
                if (AskToClose()) // ask only when tab is going to be closed by user
                {
                    // Expecting all connections are disposable, because derive from Connection
                    currentConnection.Dispose();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        internal void OnDisconnected(Connection connection)
        {
            // unregister not the method here, but the registered method
            connection.OnDisconnected -= this.mainForm.OnDisconnected;
            TerminalTabControlItem tabToClose = this.FindTabToClose(connection);
            this.InvokeCloseTab(tabToClose);
        }

        /// <summary>
        /// Because tabControl is cast internaly to TabControlItem, all connections should send their tab,
        /// which we already have in the connection.
        /// </summary>
        private void InvokeCloseTab(TerminalTabControlItem tabControl)
        {
            if (this.mainForm.InvokeRequired)
            {
                var d = new InvokeCloseTabPage(this.CloseTabPage);
                this.mainForm.Invoke(d, new object[] { tabControl });
            }
            else
            {
                this.CloseTabPage(tabControl);
            }
        }

        private void CloseTabPage(object tabObject)
        {
            var tabPage = tabObject as TabControlItem;
            if (tabPage == null)
                return;

            bool wasSelected = tabPage.Selected;
            IConnection lostConnection = this.mainForm.CurrentConnection;
            this.RemoveTabPage(tabPage);
            if (wasSelected)
                this.mainForm.OnLeavingFullScreen();

            this.mainForm.UpdateControls();
            lostConnection.Dispose();
        }

        private bool AskToClose()
        {
            if (this.settings.WarnOnConnectionClose)
            {
                string message = Program.Resources.GetString("Areyousurethatyouwanttodisconnectfromtheactiveterminal");
                string title = Program.Resources.GetString("Terminals");
                YesNoDisableResult answer = YesNoDisableForm.ShowDialog(title, message);
                if (answer.Disable)
                    this.settings.WarnOnConnectionClose = false;

                return answer.Result == DialogResult.Yes;
            }

            return true;
        }

        private void RemoveTabPage(TabControlItem tabControlToRemove)
        {
            this.tcTerminals.RemoveTab(tabControlToRemove);
            CloseTabControlItem();
        }

        private void CloseTabControlItem()
        {
            if (this.settings.RestoreWindowOnLastTerminalDisconnect)
            {
                if (this.tcTerminals.Items.Count == 0)
                    this.mainForm.FullScreen = false;
            }
        }

        private TerminalTabControlItem FindTabToClose(Connection connection)
        {
            return this.tcTerminals.Items
                .OfType<TerminalTabControlItem>()
                .FirstOrDefault(tab => tab.Connection == connection);
        }
    }
}