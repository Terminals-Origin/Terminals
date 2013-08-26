using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class DefaultPasswordOptionPanel : UserControl, IOptionPanel
    {
        public DefaultPasswordOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.domainTextbox.Text = Settings.DefaultDomain;
            this.usernameTextbox.Text = Settings.DefaultUsername;
            this.passwordTextBox.Text = Settings.DefaultPassword;
        }

        public void SaveSettings()
        {
            Settings.DefaultDomain = this.domainTextbox.Text;
            Settings.DefaultUsername = this.usernameTextbox.Text;
            Settings.DefaultPassword = this.passwordTextBox.Text;
        }
    }
}
