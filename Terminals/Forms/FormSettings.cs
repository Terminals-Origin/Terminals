using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Terminals.Forms
{
    internal class FormSettings
    {
        private System.Windows.Forms.Form _form;
        private Boolean _saveSettings = false;
        private Boolean _loadCalled = false;
        private System.Drawing.Size _lastNormalSize;
        private System.Drawing.Point _lastNormalLocation;
        private const String WindowStateSetting = "WindowState";
        private const String SizeSetting = "Size";
        private const String LocationSetting = "Location";

        public FormSettings(System.Windows.Forms.Form form)
        {
            _form = form;
            _form.HandleDestroyed += new EventHandler(FormHandleDestroyed);
            _form.HandleCreated += new EventHandler(FormHandleCreated);
            _form.Load += new EventHandler(FormLoad);
            _form.Resize += new EventHandler(FormResize);
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

        private void ValidateProperty(String propertyName, Type propertyType)
        {
            System.Configuration.SettingsProperty property = Properties.Settings.Default.Properties[propertyName];
            if (property == null)
            {
                System.Configuration.SettingsProperty propInitializer = Properties.Settings.Default.Properties["Initializer"];
                property = new System.Configuration.SettingsProperty(propInitializer);
                property.Name = propertyName;
                property.PropertyType = propertyType;
                property.DefaultValue = null;
                Properties.Settings.Default.Properties.Add(property);
            }
        }

        private T GetValue<T>(String settingName, T defaultValue)
        {
            String propertyName = BuildPropertyName(settingName);
            ValidateProperty(propertyName, typeof(T));
            Object result = Properties.Settings.Default[propertyName];
            if (result == null)
                result = defaultValue;
            return (T)result;
        }

        private void SetValue(String settingName, Object value)
        {
            String propertyName = BuildPropertyName(settingName);
            ValidateProperty(propertyName, value.GetType());
            Properties.Settings.Default[propertyName] = value;
        }

        private void LoadFormState()
        {
            if (this.Enabled)
            {
                _form.WindowState = GetValue<FormWindowState>(WindowStateSetting, _form.WindowState);
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
                        _form.Size = GetValue<System.Drawing.Size>(SizeSetting, _form.Size);
                        _lastNormalSize = _form.Size;
                    }

                    if (_form.StartPosition == FormStartPosition.Manual)
                    {
                        Point location = GetValue<Point>(LocationSetting, _form.Location);
                        //TODO: make sure the screen is moved to correct screen if it's on a non-displayed area
                        _form.Location = location;
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

                Properties.Settings.Default.Save();
            }
        }

        public Boolean Enabled { get; set; }
    }
}
