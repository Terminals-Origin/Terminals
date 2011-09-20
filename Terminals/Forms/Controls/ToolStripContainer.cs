using System;
using System.Drawing;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Extens Windows Forms ToolStripContainer to provide load and save its layout
    /// </summary>
    internal class ToolStripContainer : System.Windows.Forms.ToolStripContainer
    {
        #region Menu and toolbars

        internal ToolStrip toolbarStd { get; set; }
        internal ToolStripMenuItem standardToolbarToolStripMenuItem { get; set; }
        internal ToolStrip favoriteToolBar { get; set; }
        internal ToolStripMenuItem toolStripMenuItemShowHideFavoriteToolbar { get; set; }
        internal ToolStrip SpecialCommandsToolStrip { get; set; }
        internal ToolStripMenuItem shortcutsToolStripMenuItem { get; set; }
        internal MenuStrip menuStrip { get; set; }
        internal ToolStrip tsRemoteToolbar { get; set; }

        #endregion

        internal void SaveLayout()
        {
            if (!Settings.ToolbarsLocked)
            {
                ToolStripSettings newSettings = new ToolStripSettings();
                SaveToolStripPanel(this.TopToolStripPanel, "Top", newSettings);
                SaveToolStripPanel(this.LeftToolStripPanel, "Left", newSettings);
                SaveToolStripPanel(this.RightToolStripPanel, "Right", newSettings);
                SaveToolStripPanel(this.BottomToolStripPanel, "Bottom", newSettings);
                Settings.ToolbarSettings = newSettings;
            }
        }

        private static void SaveToolStripPanel(ToolStripPanel Panel, String Position, ToolStripSettings newSettings)
        {
            for (Int32 rowIndex = 0; rowIndex < Panel.Rows.Length; rowIndex++)
            {
                ToolStripPanelRow row = Panel.Rows[rowIndex];
                SaveToolStripRow(row, newSettings, Position, rowIndex);
            }
        }

        private static void SaveToolStripRow(ToolStripPanelRow Row, ToolStripSettings newSettings, String Position, int rowIndex)
        {
            foreach (ToolStrip strip in Row.Controls)
            {
                ToolStripSetting setting = new ToolStripSetting();
                setting.Dock = Position;
                setting.Row = rowIndex;
                setting.Left = strip.Left;
                setting.Top = strip.Top;
                setting.Name = strip.Name;
                setting.Visible = strip.Visible;
                newSettings.Add(newSettings.Count, setting);
            }
        }

        internal void LoadToolStripsState()
        {
            ToolStripSettings newSettings = Settings.ToolbarSettings;
            if (newSettings != null && newSettings.Count > 0)
            {
                this.SuspendLayout();
                this.ClearAllPanels();
                ReJoinAllPanels(newSettings);
                
                // paranoic, because the previous join can reset the position
                // dont assign if already there. Because it can reorder the toolbars
                // http://www.visualbasicask.com/visual-basic-language/toolstrips-controls-becoming-desorganized.shtml
                AplyAllPanelPositions(newSettings);
                this.ResumeLayout(true);

                ChangeLockState();
            }
        }

        private void ClearAllPanels()
        {
            this.RightToolStripPanel.Controls.Clear();
            this.LeftToolStripPanel.Controls.Clear();
            this.TopToolStripPanel.Controls.Clear();
            this.BottomToolStripPanel.Controls.Clear();
        }

        private void AplyAllPanelPositions(ToolStripSettings newSettings)
        {
            foreach (ToolStripSetting setting in newSettings.Values)
            {
                ToolStrip strip = this.FindToolStripForSetting(setting);
                strip.GripStyle = ToolStripGripStyle.Visible;
                //ChangeToolStripLock(strip);
                ApplyLastPosition(setting, strip);
            }
        }

        private void ReJoinAllPanels(ToolStripSettings newSettings)
        {
            foreach (ToolStripSetting setting in newSettings.Values)
            {
                ToolStrip strip = this.FindToolStripForSetting(setting);
                ToolStripMenuItem menuItem = this.FindMenuForSetting(setting);

                if (menuItem != null)
                {
                    menuItem.Checked = setting.Visible;
                }

                this.RestoreStripLayout(setting, strip);
            }
        }

        internal void RestoreStripLayout(ToolStripSetting setting, ToolStrip strip)
        {
            if (strip != null)
            {
                strip.Visible = setting.Visible;
                this.JoinPanelOnLastPosition(strip, setting);
            }
        }

        private ToolStripMenuItem FindMenuForSetting(ToolStripSetting setting)
        {
            if (setting.Name == this.toolbarStd.Name)
                return this.standardToolbarToolStripMenuItem;

            if (setting.Name == this.favoriteToolBar.Name)
                return this.toolStripMenuItemShowHideFavoriteToolbar;

            if (setting.Name == this.SpecialCommandsToolStrip.Name)
                return this.shortcutsToolStripMenuItem;

            return null;
        }

        private ToolStrip FindToolStripForSetting(ToolStripSetting setting)
        {
            if (setting.Name == this.toolbarStd.Name)
                return this.toolbarStd;

            if (setting.Name == this.favoriteToolBar.Name)
                return this.favoriteToolBar;

            if (setting.Name == this.SpecialCommandsToolStrip.Name)
                return this.SpecialCommandsToolStrip;

            if (setting.Name == this.menuStrip.Name)
                return this.menuStrip;

            if (setting.Name == this.tsRemoteToolbar.Name)
                return this.tsRemoteToolbar;

            return null;
        }

        private void JoinPanelOnLastPosition(ToolStrip strip, ToolStripSetting setting)
        {
            ToolStripPanel toolStripPanel = GetToolStripPanelToJoin(setting);
            if (!toolStripPanel.Controls.Contains(strip))
            {
                Point lastPosition = new Point(setting.Left, setting.Top);
                toolStripPanel.Join(strip, lastPosition);
            }
            else // set position only when comming from fullscreen
            {
                ApplyLastPosition(setting, strip);
            }
        }

        private static void ApplyLastPosition(ToolStripSetting setting, ToolStrip strip)
        {
            strip.Left = setting.Left;
            strip.Top = setting.Top;
        }

        private ToolStripPanel GetToolStripPanelToJoin(ToolStripSetting setting)
        {
            switch (setting.Dock)
            {
                case "Left":
                    return this.LeftToolStripPanel;
                case "Right":
                    return this.RightToolStripPanel;
                case "Bottom":
                    return this.BottomToolStripPanel;
                default:  // defensive position
                    return this.TopToolStripPanel;
            }
        }

        private static void ChangeToolStripLock(ToolStrip strip)
        {
            if (Settings.ToolbarsLocked)
                strip.GripStyle = ToolStripGripStyle.Hidden;
            else
                strip.GripStyle = ToolStripGripStyle.Visible;
        }

        /// <summary>
        /// Locks or unlocks the toolstrip panesl
        /// </summary>
        internal void ChangeLockState()
        {
            ChangeToolStripPanelLockState(this.TopToolStripPanel);
            ChangeToolStripPanelLockState(this.RightToolStripPanel);
            ChangeToolStripPanelLockState(this.LeftToolStripPanel);
            ChangeToolStripPanelLockState(this.BottomToolStripPanel);
        }

        private static void ChangeToolStripPanelLockState(ToolStripPanel toolStripPanel)
        {
            foreach (ToolStripPanelRow row in toolStripPanel.Rows)
            {
                foreach (ToolStrip toolStrip in row.Controls)
                {
                    if (toolStrip != null)
                    {
                        ChangeToolStripLock(toolStrip);
                    }
                }
            }
        }
    }
}
