using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Screen = System.Windows.Forms.Screen;

namespace Terminals
{
    internal partial class MainForm
    {
        private class MainFormFullScreenSwitch
        {
            private readonly MainForm mainForm;
            // restore properties after return from FullScreen
            private Boolean fullScreen;
            private Boolean stdToolbarState = true;
            private Boolean specialToolbarState = true;
            private Boolean favToolbarState = true;

            internal MainFormFullScreenSwitch(MainForm mainForm)
            {
                this.mainForm = mainForm;
            }

            internal bool FullScreen
            {
                get
                {
                    return this.fullScreen;
                }
                set
                {
                   SwitchFullScreen(value); 
                }
            }

            internal FormWindowState LastWindowState { get; set; }
            internal Point LastWindowStateNormalLocation { private get; set; }
            internal Size LastWindowStateNormalSize { private get; set; }
            internal Boolean SwitchingFullScreen { get; private set; }

            private void SwitchFullScreen(Boolean goFullScreen)
            {
                mainForm.SuspendLayout();
                this.SwitchingFullScreen = true;

                // When going fullscreen from minimized mode by clicking the trayicon menu option, 
                // immediately make the mainform visible.
                if (this.mainForm.WindowState == FormWindowState.Minimized)
                    this.mainForm.Visible = true;

                this.SetFullScreen(goFullScreen);
                mainForm.menuLoader.UpdateSwitchFullScreenMenuItemsVisibility(this.fullScreen);

                if (!goFullScreen)
                    mainForm.LoadWindowState();

                this.fullScreen = goFullScreen;
                this.SwitchingFullScreen = false;
                mainForm.ResumeLayout();
            }

            private void SetFullScreen(Boolean goFullScreen)
            {
                if (goFullScreen)
                {
                    BackUpToolBarsVisibility();
                    StoreMainFormState();
                    this.GoFullScreen();
                }
                else
                    RestoreMainFormState();

                HideToolBar(goFullScreen);
                mainForm.tcTerminals.ShowTabs = !goFullScreen;
                mainForm.Visible = true;
                mainForm.PerformLayout();
            }

            /// <summary>
            /// Put the main form window in fullscreen mode.
            /// </summary>
            private void GoFullScreen()
            {
                this.mainForm.FormBorderStyle = FormBorderStyle.None;
                if (this.mainForm.allScreens)
                {
                    var width = 0;
                    if (this.mainForm.allScreens)
                        width += Screen.AllScreens.Sum(screen => screen.Bounds.Width);

                    this.mainForm.Width = width;
                    this.mainForm.Location = new Point(0, 0);
                }
                else
                {
                    var screenBounds = Screen.FromControl(this.mainForm.tcTerminals).Bounds;
                    this.mainForm.Width = screenBounds.Width;
                    this.mainForm.Location = screenBounds.Location;
                }
                this.mainForm.Height = Screen.FromControl(this.mainForm.tcTerminals).Bounds.Height;
                this.mainForm.WindowState = FormWindowState.Normal;

                this.mainForm.menuStrip.Visible = false;
                this.mainForm.SetGrabInput(true);
                this.mainForm.BringToFront();
            }

            /// <summary>
            /// Store the current main form window state before fullscreen mode
            /// to be able to restore to this state.
            /// </summary>
            private void StoreMainFormState()
            {
                if (this.mainForm.WindowState == FormWindowState.Normal)
                {
                    this.LastWindowStateNormalLocation = this.mainForm.Location;
                    this.LastWindowStateNormalSize = this.mainForm.Size;
                }
                
                if (this.mainForm.WindowState == FormWindowState.Minimized)
                    return;

                this.LastWindowState = this.mainForm.WindowState;
            }

            /// <summary>
            /// Restore the main form window to the previous state before fullscreen mode.
            /// </summary>
            /// <remarks>
            /// Keep code in this order.
            /// If setting the borderstyle is not in the bottom, it will get the last known 
            /// form size from the formsettings which is the fullscreen size.
            /// </remarks>
            private void RestoreMainFormState()
            {
                this.mainForm.TopMost = false;
                this.mainForm.WindowState = this.LastWindowState;
                this.mainForm.FormBorderStyle = FormBorderStyle.Sizable;
                if (this.LastWindowState != FormWindowState.Minimized)
                {
                    // initial location and size isn't set yet
                    //if (this.LastWindowState == FormWindowState.Normal && this.LastWindowNormalStateLocation != Point.Empty)
                    //    this.mainForm.Location = this.LastWindowNormalStateLocation;
                    if (this.LastWindowStateNormalLocation != Point.Empty)
                        this.mainForm.Location = this.LastWindowStateNormalLocation;

                    if (this.LastWindowStateNormalSize != Size.Empty)
                    {
                        // Calculate the form's width and height substracting the last know border width
                        // and title height. This is because we are sizing the form before it has
                        // a borderstyle assigned.
                        var width = this.LastWindowStateNormalSize.Width;
                        var height = this.LastWindowStateNormalSize.Height;

                        this.mainForm.Size = new Size(width, height);
                    }
                }

                
                this.mainForm.menuStrip.Visible = true;
            }

            private void BackUpToolBarsVisibility()
            {
                this.stdToolbarState = this.mainForm.toolbarStd.Visible;
                this.specialToolbarState = this.mainForm.SpecialCommandsToolStrip.Visible;
                this.favToolbarState = this.mainForm.favoriteToolBar.Visible;
                mainForm.toolStripContainer.SaveLayout();
            }

            private void HideToolBar(Boolean goFullScreen)
            {
                if (!goFullScreen)
                {
                    mainForm.toolbarStd.Visible = this.stdToolbarState;
                    mainForm.SpecialCommandsToolStrip.Visible = this.specialToolbarState;
                    mainForm.favoriteToolBar.Visible = this.favToolbarState;
                }
                else
                {
                    mainForm.toolbarStd.Visible = false;
                    mainForm.SpecialCommandsToolStrip.Visible = false;
                    mainForm.favoriteToolBar.Visible = false;
                }
            }
        }
    }
}
