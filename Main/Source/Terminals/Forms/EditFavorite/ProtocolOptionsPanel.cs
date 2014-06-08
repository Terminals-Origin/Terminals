using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Terminals.Forms.EditFavorite
{
    public partial class ProtocolOptionsPanel : UserControl
    {
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
            List<Control> currentControls = this.Controls.OfType<Control>().ToList();
            foreach (Control protocolControl in currentControls)
            {
                this.Controls.Remove(protocolControl);
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
        }

        // todo better resolution then by note title
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
    }
}
