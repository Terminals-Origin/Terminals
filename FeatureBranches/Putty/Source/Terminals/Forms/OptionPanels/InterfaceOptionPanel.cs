using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class InterfaceOptionPanel : UserControl, IOptionPanel
    {
        private readonly Settings settings = Settings.Instance;

        public InterfaceOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkEnableGroupsMenu.Checked = settings.EnableGroupsMenu;
            this.chkMinimizeToTrayCheckbox.Checked = settings.MinimizeToTray;
            this.chkShowUserNameInTitle.Checked = settings.ShowUserNameInTitle;
            this.chkShowInformationToolTips.Checked = settings.ShowInformationToolTips;
            this.chkShowFullInfo.Checked = settings.ShowFullInformationToolTips;

            if (settings.Office2007BlueFeel)
                this.RenderBlueRadio.Checked = true;
            else if (settings.Office2007BlackFeel)
                this.RenderBlackRadio.Checked = true;
            else
                this.RenderNormalRadio.Checked = true;
        }

        public void SaveSettings()
        {
            settings.EnableGroupsMenu = this.chkEnableGroupsMenu.Checked;
            settings.MinimizeToTray = this.chkMinimizeToTrayCheckbox.Checked;
            settings.ShowUserNameInTitle = this.chkShowUserNameInTitle.Checked;
            settings.ShowInformationToolTips = this.chkShowInformationToolTips.Checked;
            settings.ShowFullInformationToolTips = this.chkShowFullInfo.Checked;

            settings.Office2007BlackFeel = false;
            settings.Office2007BlueFeel = false;

            if (this.RenderBlueRadio.Checked)
                settings.Office2007BlueFeel = true;
            else if (this.RenderBlackRadio.Checked)
                settings.Office2007BlackFeel = true;
        }

        private void chkShowInformationToolTips_CheckedChanged(object sender, EventArgs e)
        {
            this.chkShowFullInfo.Enabled = this.chkShowInformationToolTips.Checked;
            if (!this.chkShowInformationToolTips.Checked)
            {
                this.chkShowFullInfo.Checked = false;
            }
        }
    }
}
