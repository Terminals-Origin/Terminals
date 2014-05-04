using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    public partial class RdpDisplayControl : UserControl, IProtocolOptionsControl
    {
        public RdpDisplayControl()
        {
            InitializeComponent();
        }

        private void CmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbResolution.Text == "Custom" || this.cmbResolution.Text == "Auto Scale")
                this.customSizePanel.Visible = true;
            else
                this.customSizePanel.Visible = false;
        }

        public void SetControls()
        {
            this.chkConnectToConsole.Enabled = true;
        }


        private void FillFavoriteRdpDisplayOptions(RdpOptions rdpOptions)
        {
            rdpOptions.ConnectToConsole = this.chkConnectToConsole.Checked;
            rdpOptions.UserInterface.DisableWallPaper = this.chkDisableWallpaper.Checked;
            rdpOptions.UserInterface.DisableCursorBlinking = this.chkDisableCursorBlinking.Checked;
            rdpOptions.UserInterface.DisableCursorShadow = this.chkDisableCursorShadow.Checked;
            rdpOptions.UserInterface.DisableFullWindowDrag = this.chkDisableFullWindowDrag.Checked;
            rdpOptions.UserInterface.DisableMenuAnimations = this.chkDisableMenuAnimations.Checked;
            rdpOptions.UserInterface.DisableTheming = this.chkDisableThemes.Checked;
            // todo move to extended settings control rdpOptions.UserInterface.LoadBalanceInfo = this.txtLoadBalanceInfo.Text;
        }

        private void FillFavoriteDisplayOptions(IFavorite favorite)
        {
            IDisplayOptions display = favorite.Display;
            if (this.cmbResolution.SelectedIndex >= 0)
                display.DesktopSize = (DesktopSize)this.cmbResolution.SelectedIndex;

            if (this.cmbColors.SelectedIndex >= 0)
                display.Colors = (Colors)this.cmbColors.SelectedIndex;

            display.Width = (Int32)this.widthUpDown.Value;
            display.Height = (Int32)this.heightUpDown.Value;
        }

        private void FillDisplayControls(IFavorite favorite)
        {
            this.cmbResolution.SelectedIndex = (Int32)favorite.Display.DesktopSize;
            this.cmbColors.SelectedIndex = (Int32)favorite.Display.Colors;
            this.widthUpDown.Value = favorite.Display.Width;
            this.heightUpDown.Value = favorite.Display.Height;
        }

        private void FillRdpDisplayControls(RdpOptions rdpOptions)
        {
            this.chkConnectToConsole.Checked = rdpOptions.ConnectToConsole;
            this.chkDisableWallpaper.Checked = rdpOptions.UserInterface.DisableWallPaper;
            this.chkDisableCursorBlinking.Checked = rdpOptions.UserInterface.DisableCursorBlinking;
            this.chkDisableCursorShadow.Checked = rdpOptions.UserInterface.DisableCursorShadow;
            this.chkDisableFullWindowDrag.Checked = rdpOptions.UserInterface.DisableFullWindowDrag;
            this.chkDisableMenuAnimations.Checked = rdpOptions.UserInterface.DisableMenuAnimations;
            this.chkDisableThemes.Checked = rdpOptions.UserInterface.DisableTheming;
        }

    }
}
