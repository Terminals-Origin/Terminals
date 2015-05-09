using System;
using System.Collections.Generic;
using System.Linq;
using TabControl;
using Terminals.CaptureManager;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Forms;

namespace Terminals
{
    /// <summary>
    /// Adapter between all windows (including main window) and TabControl
    /// </summary>
    internal class TerminalTabsSelectionControler
    {
        private readonly Settings settings = Settings.Instance;
        private readonly List<PopupTerminal> detachedWindows = new List<PopupTerminal>();
        private readonly TabControl.TabControl mainTabControl;
        private ConnectionsUiFactory connectionsUiFactory;

        private readonly TabControlFilter filter;

        internal IConnection CurrentConnection
        {
            get { return this.filter.CurrentConnection; }
        }

        internal TerminalTabControlItem Selected
        {
            get { return this.filter.Selected; }
        }

        internal IFavorite SelectedFavorite
        {
            get { return this.filter.SelectedFavorite; }
        }

        internal bool HasSelected
        {
            get { return this.filter.HasSelected; }
        }

        internal TerminalTabsSelectionControler(TabControl.TabControl tabControl, IPersistence persistence)
        {
            this.mainTabControl = tabControl;
            this.filter = new TabControlFilter(tabControl);
            persistence.Dispatcher.FavoritesChanged += new FavoritesChangedEventHandler(this.OnFavoritesChanged);
        }

        internal void AssingUiFactory(ConnectionsUiFactory connectionsUiFactory)
        {
            this.connectionsUiFactory = connectionsUiFactory;
        }

        private void OnFavoritesChanged(FavoritesChangedEventArgs args)
        {
            foreach (IFavorite updated in args.Updated)
            {
                // dont update the rest of properties, because it doesnt reflect opened session
                UpdateDetachedWindowTitle(updated);
                UpdateAttachedTabTitle(updated);
            }
        }

        private void UpdateAttachedTabTitle(IFavorite updated)
        {
            TabControlItem attachedTab = this.filter.FindAttachedTab(updated);
            if (attachedTab != null)
                attachedTab.Title = updated.Name;
        }

        private void UpdateDetachedWindowTitle(IFavorite updated)
        {
            PopupTerminal detached = this.FindDetachedWindowByFavorite(updated);
            if (detached != null)
                detached.UpdateTitle();
        }

        private PopupTerminal FindDetachedWindowByFavorite(IFavorite updated)
        {
            return this.detachedWindows.FirstOrDefault(window => window.HasFavorite(updated));
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
        /// Releases actualy selected tab to the new window
        /// </summary>
        internal void DetachTabToNewWindow()
        {
            if (this.Selected != null)
                this.DetachTabToNewWindow(this.Selected);
        }

        internal void DetachTabToNewWindow(TerminalTabControlItem tabControlToOpen)
        {
            if (tabControlToOpen != null)
            {
                this.mainTabControl.Items.SuspendEvents();

                PopupTerminal pop = new PopupTerminal(this);
                mainTabControl.RemoveTab(tabControlToOpen);
                pop.AddTerminal(tabControlToOpen);

                this.mainTabControl.Items.ResumeEvents();
                this.detachedWindows.Add(pop);
                pop.Show();
            }
        }

        internal void AttachTabFromWindow(TerminalTabControlItem tabControlToAttach)
        {
            this.mainTabControl.AddTab(tabControlToAttach);
            PopupTerminal popupTerminal = tabControlToAttach.FindForm() as PopupTerminal;
            if (popupTerminal != null)
            {
                UnRegisterPopUp(popupTerminal);
            }
        }

        internal void UnRegisterPopUp(PopupTerminal popupTerminal)
        {
            if (this.detachedWindows.Contains(popupTerminal))
            {
                this.detachedWindows.Remove(popupTerminal);
            }
        }

        internal void CaptureScreen()
        {
            this.CaptureScreen(this.mainTabControl);
        }

        /// <summary>
        /// We need to provide the tab from outside, because it may be tab from PopUp window
        /// </summary>
        internal void CaptureScreen(TabControl.TabControl tabControl)
        {
            CaptureManager.CaptureManager.PerformScreenCapture(tabControl, this.SelectedFavorite);
            this.RefreshCaptureManagerAndCreateItsTab(false);
        }

        internal void FocusCaptureManager()
        {
            this.RefreshCaptureManagerAndCreateItsTab(true);
        }

        private void RefreshCaptureManagerAndCreateItsTab(bool openManagerTab)
        {
            Boolean refreshed = this.RefreshCaptureManager(openManagerTab);

            if (!refreshed && this.NeedsFocusCaptureManagerTab(openManagerTab))
                this.connectionsUiFactory.CreateCaptureManagerTab();
        }

        /// <summary>
        /// Updates the CaptureManager tabcontrol, focuses it and updates its content.
        /// </summary>
        /// <param name="openManagerTab"></param>
        /// <returns>true, Tab exists and was updated, otherwise false.</returns>
        private bool RefreshCaptureManager(bool openManagerTab)
        {
            CaptureManagerLayout captureManager = this.FindCaptureManagerControl();

            if (captureManager != null)
            {
                captureManager.RefreshView();
                this.FocusCaptureManager(captureManager, openManagerTab);
                return true;
            }

            return false;
        }

        private void FocusCaptureManager(CaptureManagerLayout connectionManager, bool openManagerTab)
        {
            if (this.NeedsFocusCaptureManagerTab(openManagerTab))
            {
                connectionManager.BringToFront();
                connectionManager.Update();
                // the connection manager was resolved as control on the tab
                var tab = connectionManager.Parent as TerminalTabControlItem;
                this.Select(tab);
            }
        }

        private bool NeedsFocusCaptureManagerTab(bool openManagerTab)
        {
            return openManagerTab || (this.settings.EnableCaptureToFolder && this.settings.AutoSwitchOnCapture);
        }

        private CaptureManagerLayout FindCaptureManagerControl()
        {
            TerminalTabControlItem tab = this.filter.FindCaptureManagerTab();
            if (tab != null)
            {
                // after the connection is removed, this index moves to zero
                return tab.Controls[CaptureManagerLayout.ControlName] as CaptureManagerLayout;
            }

            return null;
        }

        internal void UpdateCaptureButtonOnDetachedPopUps()
        {
            bool newEnable = settings.EnabledCaptureToFolderAndClipBoard;
            foreach (PopupTerminal detachedWindow in this.detachedWindows)
            {
                detachedWindow.UpdateCaptureButtonEnabled(newEnable);
            }
        }
    }
}
