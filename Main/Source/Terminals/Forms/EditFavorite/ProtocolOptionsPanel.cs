using System.Linq;
using System.Windows.Forms;
using Terminals.Common.Forms.EditFavorite;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class ProtocolOptionsPanel : UserControl
    {
        private NewTerminalFormValidator validator;

        public IGuardedCredentialFactory CredentialsFactory { get; set; }

        private readonly Settings settings = Settings.Instance;

        public ProtocolOptionsPanel()
        {
            this.InitializeComponent();
        }

        internal void ReloadControls(Control[] newProtocolControls)
        {
            this.RemoveCurrentControls();
            this.AddControls(newProtocolControls);
        }

        private void RemoveCurrentControls()
        {
            foreach (Control protocolControl in this.Controls.OfType<Control>().ToList())
            {
                this.Controls.Remove(protocolControl);
                UnRegisterIntegerValidation(protocolControl);
                protocolControl.Dispose();
            }
        }

        private void AddControls(Control[] toAssign)
        {
            foreach (Control protocolControl in toAssign)
            {
                this.AddNewControl(protocolControl);
            }
        }

        private void AddNewControl(Control protocolControl)
        {
            protocolControl.Dock = DockStyle.Fill;
            this.Controls.Add(protocolControl);
            this.RegisterIntegerValidation(protocolControl);
            this.AssignCredentialsFactory(protocolControl);
            this.AssignMruSettings(protocolControl);
        }

        private void AssignMruSettings(Control protocolControl)
        {
            var control = protocolControl as IRequiresMRUSettings;
            if (control != null)
            {
                control.Settings = this.settings;
            }
        }

        private void AssignCredentialsFactory(Control protocolControl)
        {
            var control = protocolControl as ISupportsSecurityControl;
            if (control != null)
            {
                control.CredentialFactory = this.CredentialsFactory;
            }
        }

        private void RegisterIntegerValidation(Control protocolControl)
        {
            var validatedControl = protocolControl as IValidatedProtocolControl;
            if (validatedControl != null)
                validatedControl.IntegerValidationRequested += this.validator.OnValidateInteger;
        }

        private void UnRegisterIntegerValidation(Control protocolControl)
        {
            var validatedControl = protocolControl as IValidatedProtocolControl;
            if (validatedControl != null)
                validatedControl.IntegerValidationRequested -= this.validator.OnValidateInteger;
        }

        internal void FocusControl(string controlName)
        {
            this.HideAllProtocolControls();
            Control control = this.ResolveChildByNameOrFirst(controlName);
            if (control != null)
                control.Show();
        }

        internal Control ResolveChildByNameOrFirst(string controlName)
        {
            Control control = this.Controls[controlName];
            if (control != null)
                return control;

            if (this.Controls.Count > 0)
                return this.Controls[0];

            return null;
        }

        private void HideAllProtocolControls()
        {
            foreach (Control protocolControl in this.Controls)
            {
                protocolControl.Hide();
            }
        }

        internal void RegisterValidations(NewTerminalFormValidator validator)
        {
            this.validator = validator;
        }

        internal void OnServerNameChanged(string newServerName)
        {
            foreach (var protocolControl in this.Controls.OfType<IProtocolObserver>())
            {
                protocolControl.OnServerNameChanged(newServerName);
            }
        }

        internal void LoadFrom(IFavorite favorite)
        {
            foreach (var protocolControl in this.Controls.OfType<IProtocolOptionsControl>())
            {
                var consumer = protocolControl as ISettingsConsumer;
                if (consumer != null)
                    consumer.Settings = this.settings;

                protocolControl.LoadFrom(favorite);
            }
        }

        internal void SaveTo(IFavorite favorite)
        {
            foreach (var protocolControl in this.Controls.OfType<IProtocolOptionsControl>())
            {
                protocolControl.SaveTo(favorite);
            }
        }
    }
}
