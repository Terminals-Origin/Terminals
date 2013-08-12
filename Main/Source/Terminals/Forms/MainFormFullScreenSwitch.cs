using System;
using System.Drawing;
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
            private Point lastLocation;
            private Size lastSize;
            private Boolean stdToolbarState = true;
            private Boolean specialToolbarState = true;
            private Boolean favToolbarState = true;
            private FormWindowState lastState;

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

            private void SwitchFullScreen(Boolean newfullScreen)
            {
                mainForm.SuspendLayout();
               
                if (newfullScreen)
                    mainForm.toolStripContainer.SaveLayout(); //Save windows state before we do a fullscreen so we can restore it

                this.SetFullScreen(newfullScreen);
                mainForm.menuLoader.UpdateSwitchFullScreenMenuItemsVisibility(this.fullScreen);

                if (!newfullScreen)
                    mainForm.LoadWindowState();

                this.fullScreen = newfullScreen;
                mainForm.ResumeLayout();
            }

            private void SetFullScreen(Boolean fullScreen)
            {
                BackUpToolBarsVisibility(fullScreen);
                HideToolBar(fullScreen);

                if (fullScreen)
                    StoreMainFormState();
                else
                    RestoreMainFormState();

                mainForm.tcTerminals.ShowTabs = !fullScreen;
                mainForm.Visible = true;
                mainForm.PerformLayout();
            }

            private void StoreMainFormState()
            {
                this.mainForm.menuStrip.Visible = false;
                this.lastLocation = this.mainForm.Location;
                this.lastSize = this.mainForm.Size;

                if (this.mainForm.WindowState == FormWindowState.Minimized)
                    this.lastState = FormWindowState.Normal;
                else
                    this.lastState = this.mainForm.WindowState;

                this.mainForm.FormBorderStyle = FormBorderStyle.None;
                this.mainForm.WindowState = FormWindowState.Normal;
                if (this.mainForm.allScreens)
                {
                    Screen[] screenArr = Screen.AllScreens;
                    Int32 with = 0;
                    if (this.mainForm.allScreens)
                    {
                        foreach (Screen screen in screenArr)
                        {
                            with += screen.Bounds.Width;
                        }
                    }

                    this.mainForm.Width = with;
                    this.mainForm.Location = new Point(0, 0);
                }
                else
                {
                    Rectangle screenBounds = Screen.FromControl(this.mainForm.tcTerminals).Bounds;
                    this.mainForm.Width = screenBounds.Width;
                    this.mainForm.Location = screenBounds.Location;
                }

                this.mainForm.Height = Screen.FromControl(this.mainForm.tcTerminals).Bounds.Height;
                this.mainForm.SetGrabInput(true);
                this.mainForm.BringToFront();
            }

            private void RestoreMainFormState()
            {
                this.mainForm.TopMost = false;
                this.mainForm.FormBorderStyle = FormBorderStyle.Sizable;
                this.mainForm.WindowState = this.lastState;
                if (this.lastState != FormWindowState.Minimized)
                {
                    // initial location and size isn't set yet
                    if (this.lastState == FormWindowState.Normal && this.lastLocation != Point.Empty)
                        this.mainForm.Location = this.lastLocation;

                    if (this.lastSize != Size.Empty)
                       this.mainForm.Size = this.lastSize;
                }

                this.mainForm.menuStrip.Visible = true;
            }

            private void BackUpToolBarsVisibility(bool fullScreen)
            {
                if (fullScreen)
                {
                    this.stdToolbarState = this.mainForm.toolbarStd.Visible;
                    this.specialToolbarState = this.mainForm.SpecialCommandsToolStrip.Visible;
                    this.favToolbarState = this.mainForm.favoriteToolBar.Visible;
                }
            }

            private void HideToolBar(Boolean fullScreen)
            {
                if (!fullScreen)
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
