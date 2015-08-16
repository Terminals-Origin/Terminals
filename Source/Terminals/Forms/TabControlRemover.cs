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

        private readonly TerminalTabsSelectionControler selectionControler;

        private readonly TabControl.TabControl tcTerminals;

        internal TabControlRemover(Settings settings, MainForm mainForm,
            TerminalTabsSelectionControler selectionControler, TabControl.TabControl tcTerminals)
        {
            this.settings = settings;
            this.mainForm = mainForm;
            this.selectionControler = selectionControler;
            this.tcTerminals = tcTerminals;
            this.tcTerminals.TabControlItemClosing += new TabControlItemClosingHandler(this.TcTerminals_TabControlItemClosing);
            this.tcTerminals.TabControlItemClosed += new TabControlItemClosedHandler(this.TcTerminals_TabControlItemClosed);
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

        private void TcTerminals_TabControlItemClosed(object sender, TabControlItemClosedEventArgs e)
        {
            this.CloseTabControlItem(e.Item);
        }

        private void TcTerminals_TabControlItemClosing(TabControlItemClosingEventArgs e)
        {
            // we have to check the connection state in case tab is closing because of lost connection
            IConnection currentConnection = this.selectionControler.CurrentConnection;
            if (currentConnection != null && currentConnection.Connected)
            {
                if (!this.AskToClose())
                    e.Cancel = true;
            }
        }

        internal void OnDisconnected(Connection connection)
        {
            // unregister not the method here, but the registered method
            connection.OnDisconnected -= this.mainForm.OnDisconnected;
            TerminalTabControlItem tabPage = new TabControlFilter(this.tcTerminals).FindTabToClose(connection);
            this.InvokeCloseTab(tabPage);
        }

        /// <summary>
        /// Because tabControl is cast internaly to TabControlItem, all connections should send their tab,
        /// which we already have in the connection.
        /// </summary>
        private void InvokeCloseTab(TerminalTabControlItem tabPage)
        {
            if (this.mainForm.InvokeRequired)
            {
                var d = new InvokeCloseTabPage(this.CloseTabPage);
                this.mainForm.Invoke(d, new object[] { tabPage });
            }
            else
            {
                this.CloseTabPage(tabPage);
            }
        }

        private void CloseTabPage(object tabObject)
        {
            var tabPage = tabObject as TabControlItem;
            if (tabPage == null)
                return;

            bool wasSelected = tabPage.Selected;
            this.tcTerminals.RemoveTab(tabPage);
            this.CloseTabControlItem(tabPage);

            if (wasSelected)
                this.mainForm.OnLeavingFullScreen();

            this.mainForm.UpdateControls();
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

        private void CloseTabControlItem(TabControlItem tabPage)
        {
            DisposeConnection(tabPage);
            this.RestoreWindowAfterLastClosed();
        }

        private static void DisposeConnection(TabControlItem tabPage)
        {
            var toDispose = tabPage as TerminalTabControlItem;
            if (toDispose != null && toDispose.Connection != null)
            {
                // Expecting all connections are disposable, because derive from Connection
                toDispose.Connection.Dispose();
            }
        }

        private void RestoreWindowAfterLastClosed()
        {
            if (this.settings.RestoreWindowOnLastTerminalDisconnect)
            {
                if (this.tcTerminals.Items.Count == 0)
                    this.mainForm.FullScreen = false;
            }
        }
    }
}