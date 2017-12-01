using System;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals
{
    internal partial class PopupTerminal : Form
    {
        private Timer closeTimer;
        private readonly TerminalTabsSelectionControler mainTabsControler;
        private bool fullScreen;

        private readonly TabControlFilter filter;

        private IFavorite SelectedOriginFavorite
        {
            get { return this.filter.SelectedOriginFavorite; }
        }

        internal PopupTerminal()
        {
            this.InitializeComponent();
        }

        internal PopupTerminal(TerminalTabsSelectionControler mainTabsControler)
            : this()
        {
            this.mainTabsControler = mainTabsControler;
            this.filter = new TabControlFilter(this.tabControl1);
        }

        internal void AddTerminal(TerminalTabControlItem tabControlItem)
        {
            this.tabControl1.AddTab(tabControlItem);
            this.Text = tabControlItem.Title;
        }

        internal bool HasFavorite(IFavorite updated)
        {
            return this.SelectedOriginFavorite != null && this.SelectedOriginFavorite.StoreIdEquals(updated);
        }

        internal void UpdateTitle()
        {
            if (this.SelectedOriginFavorite != null)
                this.Text = this.SelectedOriginFavorite.Name;
            this.tabControl1.Items[0].Title = this.Text;
        }

        private void PopupTerminal_Load(object sender, EventArgs e)
        {
            IConnection connection = this.filter.SelectedConnection;
            if (connection != null) // dont run the timer for not connected tab
            {
                this.closeTimer = new Timer();
                this.closeTimer.Interval = 500;
                this.closeTimer.Tick += new EventHandler(this.CloseTimer_Tick);
                this.closeTimer.Start();
            }
        }

        /// <summary>
        /// Check every 500 ms, to automaticaly close form, if connection is lost
        /// </summary>
        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            IConnection connection = this.filter.SelectedConnection;
            if (connection != null && !connection.Connected)
            {
                this.closeTimer.Stop();
                this.Close();
            }
        }

        private void AttachToTerminalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminalTabControlItem activeTab = this.tabControl1.SelectedItem as TerminalTabControlItem;
            if (activeTab != null)
            {
                this.mainTabsControler.AttachTabFromWindow(activeTab);
            }

            this.Close();
        }

        private void PopupTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.mainTabsControler.UnRegisterPopUp(this);
            TerminalTabControlItem activeTab = this.tabControl1.SelectedItem as TerminalTabControlItem;
            if (activeTab != null)
            {
                // doesnt metter yet. Nobody closes connections
                this.tabControl1.CloseTab(activeTab);
            }
        }

        private void CaptureToolStripButton_Click(object sender, EventArgs e)
        {
            this.CaptureScreen();
        }

        private void CaptureScreen()
        {
            this.mainTabsControler.CaptureScreen(this.tabControl1);
        }

        internal void UpdateCaptureButtonEnabled(bool newEnabledState)
        {
            this.CaptureToolStripButton.Enabled = newEnabledState;
        }

        private void PopupTerminal_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F12)
                this.CaptureScreen();

            if (e.KeyCode == Keys.F11)
                this.SwithFullScreen();
        }

        private void ToolStripButtonFullScreen_Click(object sender, EventArgs e)
        {
            this.SwithFullScreen();
        }

        private void SwithFullScreen()
        {
            this.fullScreen = !this.fullScreen;
            if (this.fullScreen)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
        }
    }
}
