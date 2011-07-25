using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Connections;

namespace Terminals
{
  internal partial class PopupTerminal : Form
  {
    internal PopupTerminal()
    {
      InitializeComponent();
    }

    internal PopupTerminal(TerminalTabsSelectionControler mainTabsControler): this()
    {
      this.mainTabsControler = mainTabsControler;
    }

    private Timer timerHover;
    private Timer closeTimer;

    private TerminalTabsSelectionControler mainTabsControler;

    public void AddTerminal(TerminalTabControlItem TabControlItem)
    {
      this.tabControl1.AddTab(TabControlItem);
      this.Text = TabControlItem.Connection.Favorite.Name;
    }

    private void timerHover_Tick(object sender, EventArgs e)
    {
      if (timerHover.Enabled)
      {
        timerHover.Enabled = false;
        tabControl1.ShowTabs = true;
      }
    }

    private void tabControl1_TabControlItemClosing(TabControl.TabControlItemClosingEventArgs e)
    {
      if (this.tabControl1.Items.Count <= 1)
      {
        this.Close();
      }
    }

    private void tcTerminals_MouseHover(object sender, EventArgs e)
    {
      if (tabControl1 != null)
      {
        if (!tabControl1.ShowTabs)
        {
          timerHover.Enabled = true;
        }
      }
    }

    private void tcTerminals_MouseLeave(object sender, EventArgs e)
    {
      timerHover.Enabled = false;
      if (FullScreen && tabControl1.ShowTabs && !tabControl1.MenuOpen)
      {
        tabControl1.ShowTabs = false;
      }

    }

    private bool fullScreen = false;
    public bool FullScreen
    {
      get
      {
        return fullScreen;
      }
      set
      {
        fullScreen = value;
        UpdateWindowByFullScreen(value);
      }
    }

    private void UpdateWindowByFullScreen(bool fullScreen)
    {
      if (fullScreen)
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

    private void tabControl1_DoubleClick(object sender, EventArgs e)
    {
      FullScreen = !fullScreen;
    }

    private void PopupTerminal_Load(object sender, EventArgs e)
    {
      timerHover = new Timer();
      timerHover.Interval = 200;
      timerHover.Tick += new EventHandler(timerHover_Tick);
      timerHover.Start();

      closeTimer = new Timer();
      closeTimer.Interval = 500;
      closeTimer.Tick += new EventHandler(closeTimer_Tick);
      closeTimer.Start();
    }

    private object synLock = new object();
    
    private void closeTimer_Tick(object sender, EventArgs e)
    {
      lock (synLock)
      {
        closeTimer.Enabled = false;
        List<TabControl.TabControlItem> removeableTabs = new List<TabControl.TabControlItem>();
        foreach (TabControl.TabControlItem tab in this.tabControl1.Items)
        {
          if (tab.Controls.Count > 0)
          {
            if (!((IConnection) tab.Controls[0]).Connected)
            {
              removeableTabs.Add(tab);
            }
          }
        }
        try
        {
          foreach (TabControl.TabControlItem tab in removeableTabs)
          {
            tabControl1.CloseTab(tab);
            tab.Dispose();
          }
        }
        catch (Exception exc)
        {
          Logging.Log.Error("Error attempting to remove tab from window", exc);
        }
        closeTimer.Enabled = true;
      }
    }

    private void attachToTerminalsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      TerminalTabControlItem activeTab = this.tabControl1.SelectedItem as TerminalTabControlItem;
      if (activeTab != null)
      {
        this.mainTabsControler.AttachTabFromWindow(activeTab);
        this.tabControl1.CloseTab(activeTab);
      }
    }

    private void CaptureToolStripButton_Click(object sender, EventArgs e)
    {
      CaptureScreen();
    }

    private void CaptureScreen()
    {
      CaptureManager.CaptureManager.PerformScreenCapture(this.tabControl1);
      this.mainTabsControler.RefreshCaptureManagerAndCreateItsTab(false);
    }

    private void PopupTerminal_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.mainTabsControler.UnRegisterPopUp(this);
    }

    internal void UpdateCaptureButtonEnabled(bool newEnabledState)
    {
      this.CaptureToolStripButton.Enabled = newEnabledState;
    }

    private void PopupTerminal_KeyUp(object sender, KeyEventArgs e)
    {
      if(e.Control && e.KeyCode == Keys.F12)
      {
        CaptureScreen();
      }
    }
  }
}
