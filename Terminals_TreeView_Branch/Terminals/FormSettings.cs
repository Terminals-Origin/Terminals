using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;

namespace Terminals
{
    public class FormSettings
    {
        System.Windows.Forms.Form _form;
        bool _saveSettings = false;
        bool _loadCalled = false;
        bool _enabled = true;
        System.Drawing.Size _lastNormalSize;
        System.Drawing.Point _lastNormalLocation;
        private const string WindowStateSetting = "WindowState";
        private const string SizeSetting = "Size";
        private const string LocationSetting = "Location";

        public FormSettings(System.Windows.Forms.Form form)
        {
            _form = form;
            _form.HandleDestroyed += new EventHandler(FormHandleDestroyed);
            _form.HandleCreated += new EventHandler(FormHandleCreated);
            _form.Load += new EventHandler(FormLoad);
            _form.Resize += new EventHandler(FormResize);
        }

        void FormLoad(object sender, EventArgs e)
        {
            LoadFormState();
        }

        void FormResize(object sender, EventArgs e)
        {
            if (_form.WindowState == FormWindowState.Normal)
            {
                _lastNormalSize = _form.Size;
                _lastNormalLocation = _form.Location;
            }
        }

        void FormHandleCreated(object sender, EventArgs e)
        {
            LoadFormSize();
        }

        void FormHandleDestroyed(object sender, EventArgs e)
        {
            if (!_form.RecreatingHandle)
            {
                SaveFormState();
            }
        }

        private string BuildPropertyName(string settingName)
        {
            return _form.GetType().Name + "." + _form.Name + "." + settingName;
        }

        private void ValidateProperty(string propertyName, Type propertyType)
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

        private T GetValue<T>(string settingName, T defaultValue)
        {
            string propertyName = BuildPropertyName(settingName);
            ValidateProperty(propertyName, typeof(T));
            object result = Properties.Settings.Default[propertyName];
            if (result == null)
                result = defaultValue;
            return (T)result;
        }

        private void SetValue(string settingName, object value)
        {
            string propertyName = BuildPropertyName(settingName);
            ValidateProperty(propertyName, value.GetType());
            Properties.Settings.Default[propertyName] = value;
        }

        private void LoadFormState()
        {
            if (_enabled)
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

                if (_enabled)
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
            if (_saveSettings && _enabled)
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

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }
    }
}
