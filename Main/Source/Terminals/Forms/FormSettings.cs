using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Terminals.Configuration;

namespace Terminals.Forms
{
    /// <summary>
    /// Manages saving and reloading Windows Form state in the configuration file
    /// </summary>
    internal class FormSettings
    {
        private readonly Settings settings = Configuration.Settings.Instance;
        private readonly Form form;
        private Boolean saveSettings;
        private Boolean loadCalled;
        private Size lastNormalSize;
        private Point lastNormalLocation;

        #region Constructors

        internal FormSettings(Form form)
        {
            this.form = form;
            this.form.HandleDestroyed += FormHandleDestroyed;
            this.form.HandleCreated += FormHandleCreated;
            this.form.Load += FormLoad;
            this.form.Resize += FormResize;
            this.form.Move += FormMove;
            this.Enabled = true;
        }

        #endregion

        #region Properties

        internal Boolean Enabled { get; set; }

        private FormsSection Settings
        {
            get
            {
                return settings.Forms;
            }
        }

        private FormStateConfigElement SavedState
        {
            get
            {
                return this.Settings.Forms[this.form.Name];
            }
        }

        #endregion

        #region Event handlers

        private void FormHandleDestroyed(object sender, EventArgs e)
        {
            if (!this.form.RecreatingHandle)
            {
                SaveFormState();
            }
        }

        private void FormHandleCreated(object sender, EventArgs e)
        {
            LoadFormSize();
        }

        private void FormMove(object sender, EventArgs e)
        {
            SaveSizeAndLocation();
            SaveFormState();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            LoadFormState();
        }

        private void FormResize(object sender, EventArgs e)
        {
            SaveSizeAndLocation();
            SaveFormState();
        }

        #endregion

        #region Internal methods

        internal void LoadFormSize()
        {
            if (this.loadCalled) 
                return;

            this.loadCalled = true;
            FormStateConfigElement lastFormState = this.SavedState;
            if (this.Enabled && lastFormState != null)
            {
                this.LoadFormState();
                this.ReloadSize(lastFormState);
                this.ReloadLocation(lastFormState);
            }
            this.saveSettings = true;
        }

        /// <summary>
        /// Restores form position to primary screen, if both of its check points 
        /// in window caption is out of visible bounds
        /// </summary>
        internal void EnsureVisibleScreenArea()
        {
            // because when comming back from minimalized state, this method is called firs yet with this state
            if (this.form.WindowState == FormWindowState.Minimized)
                return;

            Screen lastScreen = LastScreenOfCaptionPoint(GetLeftCaptionPoint());

            if (lastScreen == null)
                lastScreen = LastScreenOfCaptionPoint(GetRightCaptionPoint());

            if (lastScreen == null)
                this.form.Location = new Point(100, 100);
        }

        #endregion

        #region Private Methods

        private void SaveSizeAndLocation()
        {
            // If form is MainForm, check if it's switching between fullscreen mode.
            // Then don't save changes to the form size.
            Boolean mainFormFullScreen = false;
            if (form.GetType() == typeof (MainForm))
                mainFormFullScreen = ((MainForm)form).SwitchingFullScreen;

            if (this.form.WindowState != FormWindowState.Normal || mainFormFullScreen)
                return;

            this.lastNormalSize = this.form.Size;
            this.lastNormalLocation = this.form.Location;
        }

        private void LoadFormState()
        {
            if (!this.Enabled)
                return;

            FormStateConfigElement lastFormState = this.SavedState;
            if(lastFormState != null)
                this.form.WindowState = lastFormState.State;

            if (this.form.WindowState == FormWindowState.Minimized)
                this.form.WindowState = FormWindowState.Normal;
        }

        private void ReloadLocation(FormStateConfigElement lastFormState)
        {
            if (this.form.StartPosition != FormStartPosition.Manual)
                return;

            this.form.Location = lastFormState.Location;
            this.EnsureVisibleScreenArea();
            this.lastNormalLocation = this.form.Location;
        }

        private void ReloadSize(FormStateConfigElement lastFormState)
        {
            if (this.form.FormBorderStyle != FormBorderStyle.Sizable &&
                this.form.FormBorderStyle != FormBorderStyle.SizableToolWindow)
                return;

            this.form.Size = lastFormState.Size;
            this.lastNormalSize = this.form.Size;
        }

        /// <summary>
        /// Consider dont use on move and resize because of performance
        /// </summary>
        private void SaveFormState()
        {
            if (!this.saveSettings || !this.Enabled)
                return;

            FormStateConfigElement formSettings = this.PrepareStateToSave();
            this.Settings.AddForm(formSettings);
            settings.SaveAndFinishDelayedUpdate();
        }

        private FormStateConfigElement PrepareStateToSave()
        {
            var formSettings = new FormStateConfigElement(this.form.Name);
            this.SetFormSizeAndLocation(formSettings);

            if (this.form.WindowState != FormWindowState.Minimized)
            {
                formSettings.State = this.form.WindowState;
            }
            return formSettings;
        }

        private void SetFormSizeAndLocation(FormStateConfigElement formSettings)
        {
            if (this.form.WindowState != FormWindowState.Normal)
            {
                formSettings.Size = this.lastNormalSize;
                formSettings.Location = this.lastNormalLocation;
            }
            else
            {
                formSettings.Size = this.form.Size;
                formSettings.Location = this.form.Location;
            }
        }

        /// <summary>
        /// Gets the screen location of point in midle of form caption height,
        /// five buttons left from right side
        /// </summary>
        private Point GetRightCaptionPoint()
        {
            int captionButtonsWidth = SystemInformation.CaptionButtonSize.Width * 5;
            return new Point(this.form.Location.X + this.form.Width - captionButtonsWidth,
                             this.form.Location.Y + GetCaptionHeightMidle());
        }

        /// <summary>
        /// Gets the screen location of point in midle of form caption height,
        ///  five buttons left from side
        /// </summary>
        private Point GetLeftCaptionPoint()
        {
            return new Point(this.form.Location.X + GetCaptionHeightMidle(),
                             this.form.Location.Y + GetCaptionHeightMidle());
        }

        private static int GetCaptionHeightMidle()
        {
            return SystemInformation.CaptionHeight / 2;
        }

        /// <summary>
        /// Finds first screen on which checked point appears; or null if doesnt belong to any screen
        /// </summary>
        private static Screen LastScreenOfCaptionPoint(Point captionPoint)
        {
            return Screen.AllScreens.
                FirstOrDefault(candidate => candidate.Bounds.X <= captionPoint.X &&
                               captionPoint.X < candidate.Bounds.X + candidate.Bounds.Width &&
                               candidate.Bounds.Y <= captionPoint.Y &&
                               captionPoint.Y < candidate.Bounds.Y + candidate.Bounds.Height);
        }

        #endregion
    }
}
