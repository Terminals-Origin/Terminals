using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Terminals.Properties;

namespace Terminals.Forms
{
    /// <summary>
    /// Manages saving and reloading Windows Form state in the configuration file
    /// </summary>
    internal class FormSettings
    {
        private readonly Form form;
        private Boolean saveSettings;
        private Boolean loadCalled;
        private Size lastNormalSize;
        private Point lastNormalLocation;
        internal Boolean Enabled { get; set; }

        private FormsSection Settings
        {
            get
            {
                return Configuration.Settings.Forms;
            }
        }

        private FormStateConfigElement SavedState
        {
            get
            {
                return this.Settings.Forms[this.form.Name];
            }
        }

        internal FormSettings(Form form)
        {
            this.form = form;
            this.form.HandleDestroyed += new EventHandler(FormHandleDestroyed);
            this.form.HandleCreated += new EventHandler(FormHandleCreated);
            this.form.Load += new EventHandler(FormLoad);
            this.form.Resize += new EventHandler(FormResize);
            this.form.Move += new EventHandler(form_Move);
            this.Enabled = true;
        }

        void form_Move(object sender, EventArgs e)
        {
            SaveSizeAndLocation();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            LoadFormState();
        }

        private void FormResize(object sender, EventArgs e)
        {
            SaveSizeAndLocation();
        }
        private void SaveSizeAndLocation()
        {
            if (this.form.WindowState == FormWindowState.Normal)
            {
                this.lastNormalSize = this.form.Size;
                this.lastNormalLocation = this.form.Location;
            }
        }

        private void FormHandleCreated(object sender, EventArgs e)
        {
            LoadFormSize();
        }

        private void FormHandleDestroyed(object sender, EventArgs e)
        {
            if (!this.form.RecreatingHandle)
            {
                SaveFormState();
            }
        }

        private void LoadFormState()
        {
            if (this.Enabled)
            {
                FormStateConfigElement lastFormState = this.SavedState;
                if(lastFormState != null)
                    this.form.WindowState = lastFormState.State;

                if (this.form.WindowState == FormWindowState.Minimized)
                    this.form.WindowState = FormWindowState.Normal;
            }
        }

        internal void LoadFormSize()
        {
            if (!this.loadCalled)
            {
                this.loadCalled = true;

                FormStateConfigElement lastFormState = this.SavedState;
                if (this.Enabled && lastFormState != null)
                {
                    this.ReloadSize(lastFormState);
                    this.ReloadLocation(lastFormState);
                }
                this.saveSettings = true;
            }
        }

        private void ReloadLocation(FormStateConfigElement lastFormState)
        {
            if (this.form.StartPosition == FormStartPosition.Manual)
            {
                this.form.Location = lastFormState.Location;
                this.EnsureVisibleScreenArrea();
                this.lastNormalLocation = this.form.Location;
            }
        }

        private void ReloadSize(FormStateConfigElement lastFormState)
        {
            if (this.form.FormBorderStyle == FormBorderStyle.Sizable ||
                this.form.FormBorderStyle == FormBorderStyle.SizableToolWindow)
            {
                this.form.Size = lastFormState.Size;
                this.lastNormalSize = this.form.Size;
            }
        }

        private void SaveFormState()
        {
            if (this.saveSettings && this.Enabled)
            {
                FormStateConfigElement formSettings = this.PrepareStateToSave();
                Settings.AddForm(formSettings);
                Configuration.Settings.SaveAndFinishDelayedUpdate();
            }
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
        /// Restores form position to primary screen, if both of its check points 
        /// in window caption is out of visible bounds
        /// </summary>
        internal void EnsureVisibleScreenArrea()
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
    }
}
