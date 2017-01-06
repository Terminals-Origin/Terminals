using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class DefaultPasswordOptionPanel : UserControl, IOptionPanel
    {
        private readonly Settings settings = Settings.Instance;

        public DefaultPasswordOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.domainTextbox.Text = settings.DefaultDomain;
            this.usernameTextbox.Text = settings.DefaultUsername;
            this.passwordTextBox.Text = settings.DefaultPassword;
        }

        public void SaveSettings()
        {
            settings.DefaultDomain = this.domainTextbox.Text;
            settings.DefaultUsername = this.usernameTextbox.Text;
            settings.DefaultPassword = this.passwordTextBox.Text;
        }
    }
}
