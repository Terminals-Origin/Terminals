namespace Terminals
{
  /// <summary>
  /// Adapter between all windows (including main window) and TabControl
  /// </summary>
  internal class TerminalTabsSelectionControler
  {
    private TabControl.TabControl mainTabControl;

    internal TerminalTabsSelectionControler(TabControl.TabControl tabControl)
    {
      this.mainTabControl = tabControl;
    }

    /// <summary>
    /// Markes the selected terminal as selected. If it is in mainTabControl,
    /// then directly selects it, otherwise marks the selected window
    /// </summary>
    /// <param name="toSelect">new terminal tabControl to assign as selected</param>
    internal void Select(TerminalTabControlItem toSelect)
    {
      this.mainTabControl.SelectedItem = toSelect;
    }

    /// <summary>
    /// Clears the selection of currently manipulated TabControl.
    /// This has the same result like to call Select(null).
    /// </summary>
    internal void UnSelect()
    {
      Select(null);
    }

    internal void AddAndSelect(TerminalTabControlItem toAdd)
    {
      this.mainTabControl.Items.Add(toAdd);
      this.Select(toAdd);
    }

    internal void RemoveAndUnSelect(TerminalTabControlItem toRemove)
    {
      this.mainTabControl.Items.Remove(toRemove);
      this.UnSelect();
    }

    /// <summary>
    /// Gets the actualy selected TabControl even if it is not in main window
    /// </summary>
    internal TerminalTabControlItem Selected
    {
      get
      {
        return this.mainTabControl.SelectedItem as TerminalTabControlItem;
      }
    }

    internal bool HasSelected
    {
      get
      {
        return this.mainTabControl.SelectedItem != null;
      }
    }

    /// <summary>
    /// Releases actualy selected tab to the new window
    /// </summary>
    internal void ReleaseTabToNewWindow()
    {
      this.ReleaseTabToNewWindow(this.Selected);
    }

    internal void ReleaseTabToNewWindow(TerminalTabControlItem tabControlToOpen)
    {
      if (tabControlToOpen != null)
      {
        this.mainTabControl.Items.SuspendEvents();

        PopupTerminal pop = new PopupTerminal(this);
        mainTabControl.RemoveTab(tabControlToOpen);
        pop.AddTerminal(tabControlToOpen);

        this.mainTabControl.Items.ResumeEvents();
        pop.Show();
      }
    }

    internal void AttachTabFromWindow(TerminalTabControlItem tabControlToAttach)
    {
       this.mainTabControl.AddTab(tabControlToAttach);
    }
  }
}
