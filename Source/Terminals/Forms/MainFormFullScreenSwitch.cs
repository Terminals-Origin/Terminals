using System;
using System.Drawing;
using System.Windows.Forms;
using Screen = System.Windows.Forms.Screen;

namespace Terminals
{
    internal partial class MainForm
    {
        internal class MainFormFullScreenSwitch
        {
            private MainForm mainForm;
            // restore properties after return from FullScreen
            private Boolean _fullScreen;
            private Point _lastLocation;
            private Size _lastSize;
            private Boolean _stdToolbarState = true;
            private Boolean _specialToolbarState = true;
            private Boolean _favToolbarState = true;
            private FormWindowState _lastState;

            internal MainFormFullScreenSwitch(MainForm mainForm)
            {
                this.mainForm = mainForm;
            }

            internal bool FullScreen
            {
                get
                {
                    return this._fullScreen;
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
                mainForm.menuLoader.UpdateSwitchFullScreenMenuItemsVisibility(_fullScreen);

                if (!newfullScreen)
                    mainForm.LoadWindowState();

                this._fullScreen = newfullScreen;
                mainForm.ResumeLayout();
            }

            private void SetFullScreen(Boolean fullScreen)
            {
                mainForm.Visible = false;
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
                this._lastLocation = this.mainForm.Location;
                this._lastSize = this.mainForm.Size;

                if (this.mainForm.WindowState == FormWindowState.Minimized)
                    this._lastState = FormWindowState.Normal;
                else
                    this._lastState = this.mainForm.WindowState;

                this.mainForm.FormBorderStyle = FormBorderStyle.None;
                this.mainForm.WindowState = FormWindowState.Normal;
                if (this.mainForm._allScreens)
                {
                    Screen[] screenArr = Screen.AllScreens;
                    Int32 with = 0;
                    if (this.mainForm._allScreens)
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
                this.mainForm.WindowState = this._lastState;
                if (this._lastState != FormWindowState.Minimized)
                {
                    // initial location and size isn't set yet
                    if (this._lastState == FormWindowState.Normal && this._lastLocation != Point.Empty)
                        this.mainForm.Location = this._lastLocation;

                    if (this._lastSize != Size.Empty)
                       this.mainForm.Size = this._lastSize;
                }

                this.mainForm.menuStrip.Visible = true;
            }

            private void BackUpToolBarsVisibility(bool fullScreen)
            {
                if (fullScreen)
                {
                    this._stdToolbarState = this.mainForm.toolbarStd.Visible;
                    this._specialToolbarState = this.mainForm.SpecialCommandsToolStrip.Visible;
                    this._favToolbarState = this.mainForm.favoriteToolBar.Visible;
                }
            }

            private void HideToolBar(Boolean fullScreen)
            {
                if (!fullScreen)
                {
                    mainForm.toolbarStd.Visible = _stdToolbarState;
                    mainForm.SpecialCommandsToolStrip.Visible = _specialToolbarState;
                    mainForm.favoriteToolBar.Visible = _favToolbarState;
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
