using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class InterfaceOptionPanel : UserControl, IOptionPanel
    {
        public InterfaceOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkEnableGroupsMenu.Checked = Settings.EnableGroupsMenu;
            this.chkMinimizeToTrayCheckbox.Checked = Settings.MinimizeToTray;
            this.chkShowUserNameInTitle.Checked = Settings.ShowUserNameInTitle;
            this.chkShowInformationToolTips.Checked = Settings.ShowInformationToolTips;
            this.chkShowFullInfo.Checked = Settings.ShowFullInformationToolTips;

            if (Settings.Office2007BlueFeel)
                this.RenderBlueRadio.Checked = true;
            else if (Settings.Office2007BlackFeel)
                this.RenderBlackRadio.Checked = true;
            else
                this.RenderNormalRadio.Checked = true;
        }

        public void SaveSettings()
        {
            Settings.EnableGroupsMenu = this.chkEnableGroupsMenu.Checked;
            Settings.MinimizeToTray = this.chkMinimizeToTrayCheckbox.Checked;
            Settings.ShowUserNameInTitle = this.chkShowUserNameInTitle.Checked;
            Settings.ShowInformationToolTips = this.chkShowInformationToolTips.Checked;
            Settings.ShowFullInformationToolTips = this.chkShowFullInfo.Checked;

            Settings.Office2007BlackFeel = false;
            Settings.Office2007BlueFeel = false;

            if (this.RenderBlueRadio.Checked)
                Settings.Office2007BlueFeel = true;
            else if (this.RenderBlackRadio.Checked)
                Settings.Office2007BlackFeel = true;
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
