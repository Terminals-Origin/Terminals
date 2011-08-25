using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Terminals.Properties;

namespace Terminals.Forms
{
    internal class FormSettings
    {
        private Form _form;
        private Boolean _saveSettings = false;
        private Boolean _loadCalled = false;
        private Size _lastNormalSize;
        private Point _lastNormalLocation;
        private const String WindowStateSetting = "WindowState";
        private const String SizeSetting = "Size";
        private const String LocationSetting = "Location";
        public Boolean Enabled { get; set; }

        public FormSettings(Form form)
        {
            _form = form;
            _form.HandleDestroyed += new EventHandler(FormHandleDestroyed);
            _form.HandleCreated += new EventHandler(FormHandleCreated);
            _form.Load += new EventHandler(FormLoad);
            _form.Resize += new EventHandler(FormResize);
            this.Enabled = true;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            LoadFormState();
        }

        private void FormResize(object sender, EventArgs e)
        {
            if (_form.WindowState == FormWindowState.Normal)
            {
                _lastNormalSize = _form.Size;
                _lastNormalLocation = _form.Location;
            }
        }

        private void FormHandleCreated(object sender, EventArgs e)
        {
            LoadFormSize();
        }

        private void FormHandleDestroyed(object sender, EventArgs e)
        {
            if (!_form.RecreatingHandle)
            {
                SaveFormState();
            }
        }

        private string BuildPropertyName(String settingName)
        {
            return String.Format("{0}.{1}.{2}", _form.GetType().Name, _form.Name, settingName);
        }

        private static void ValidateProperty(String propertyName, Type propertyType)
        {
            SettingsProperty property = Settings.Default.Properties[propertyName];
            if (property == null)
            {
                SettingsProperty propInitializer = Settings.Default.Properties["Initializer"];
                property = new SettingsProperty(propInitializer);
                property.Name = propertyName;
                property.PropertyType = propertyType;
                property.DefaultValue = null;
                Settings.Default.Properties.Add(property);
            }
        }

        private T GetValue<T>(String settingName, T defaultValue)
        {
            String propertyName = BuildPropertyName(settingName);
            ValidateProperty(propertyName, typeof(T));
            Object result = Settings.Default[propertyName];
            if (result == null)
                result = defaultValue;
            return (T)result;
        }

        private void SetValue(String settingName, Object value)
        {
            String propertyName = BuildPropertyName(settingName);
            ValidateProperty(propertyName, value.GetType());
            Settings.Default[propertyName] = value;
        }

        private void LoadFormState()
        {
            if (this.Enabled)
            {
                _form.WindowState = GetValue(WindowStateSetting, _form.WindowState);
                if (_form.WindowState == FormWindowState.Minimized)
                    _form.WindowState = FormWindowState.Normal;
            }
        }

        public void LoadFormSize()
        {
            if (!_loadCalled)
            {
                _loadCalled = true;

                if (this.Enabled)
                {
                    if (_form.FormBorderStyle == FormBorderStyle.Sizable ||
                      _form.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                    {
                        _form.Size = GetValue(SizeSetting, _form.Size);
                        _lastNormalSize = _form.Size;
                    }

                    if (_form.StartPosition == FormStartPosition.Manual)
                    {
                        Point location = GetValue(LocationSetting, _form.Location);
                        
                        _form.Location = location;
                        EnsureVisibleScreenArrea();
                        _lastNormalLocation = _form.Location;
                    }
                }
                _saveSettings = true;
            }
        }

        private void SaveFormState()
        {
            if (_saveSettings && this.Enabled)
            {
                if (_form.WindowState != FormWindowState.Normal)
                {
                    SetValue(SizeSetting, _lastNormalSize);
                    SetValue(LocationSetting, _lastNormalLocation);
                }
                else
                {
                    SetValue(SizeSetting, _form.Size);
                    SetValue(LocationSetting, _form.Location);
                }

                if (_form.WindowState != FormWindowState.Minimized)
                {
                    SetValue(WindowStateSetting, _form.WindowState);
                }

                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Restores form position to primary screen, if it is out of visible bounds
        /// </summary>
        internal void EnsureVisibleScreenArrea()
        {
            // Height - 10 = means to see atleast part of the window title
            Screen lastScreen = Screen.AllScreens.
                FirstOrDefault(candidate => candidate.Bounds.X <= _form.Location.X &&
                               _form.Location.X < candidate.Bounds.X + candidate.Bounds.Width - 10 &&
                               candidate.Bounds.Y <= _form.Location.Y &&
                               _form.Location.Y < candidate.Bounds.Y + candidate.Bounds.Height - 10);

            if (lastScreen == null)
                _form.Location = new Point(100, 100);
        }
    }
}
