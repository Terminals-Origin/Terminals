using System.Linq;
using System.Windows.Forms;

namespace Terminals.Forms.EditFavorite
{
    internal partial class ProtocolOptionsPanel : UserControl
    {
        private NewTerminalFormValidator validator;

        // private readonly PluginManager pluginsManager = new PluginManager();

        public ProtocolOptionsPanel()
        {
            this.InitializeComponent();
        }

        internal void Load()
        {
            // this.pluginsManager.Load();
        }

        internal string[] Available
        {
            get
            {  // todo load available protocols from connections manager
                return new string[]
                {
                    "RDP",
                    "VNC",
                    "VMRC",
                    "SSH",
                    "Telnet",
                    "RAS",
                    "ICA Citrix",
                    "HTTP",
                    "HTTPS"
                }; //this.pluginsManager.Available; 
            }
        }

        internal void ReloadControls(string newProtocol)
        {
            //var selectedPlugin = this.pluginsManager[newProtocol];
            //if (selectedPlugin == null)
            //    return;

            //Control[] newControls = selectedPlugin.CreateControls();
            //this.ReloadControls(newControls);
        }

        private void ReloadControls(Control[] toAssign)
        {
            this.RemoveCurrentControls();
            this.AddControls(toAssign);
        }

        private void RemoveCurrentControls()
        {
            foreach (Control protocolControl in this.Controls)
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
            this.Controls.Add(protocolControl);
            protocolControl.Dock = DockStyle.Fill;
            this.RegisterIntegerValidation(protocolControl);
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

        internal void FocuControl(string controlName = "")
        {
            this.HideAllProtocolControls();
            var control = this.Controls[controlName];
            if (control != null)
                control.Show();
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
    }
}
