using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class RdpDisplayControl : UserControl, IProtocolOptionsControl
    {
        public RdpDisplayControl()
        {
            InitializeComponent();

            this.cmbResolution.SelectedIndex = 7;
            this.cmbColors.SelectedIndex = 1;
        }

        private void CmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbResolution.Text == "Custom" || this.cmbResolution.Text == "Auto Scale")
                this.customSizePanel.Visible = true;
            else
                this.customSizePanel.Visible = false;
        }

        // todo where tomove the SetControls method, when protocol was changed
        public void SetControls()
        {
            this.chkConnectToConsole.Enabled = true;
        }

        public void LoadFrom(IFavorite favorite)
        {
            this.FillDisplayControls(favorite);
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions != null)
                this.FillRdpDisplayControls(rdpOptions);
        }

        public void SaveTo(IFavorite favorite)
        {
            this.FillFavoriteDisplayOptions(favorite);
            var rdpOptions = favorite.ProtocolProperties as RdpOptions;
            if (rdpOptions != null)
                this.FillFavoriteRdpDisplayOptions(rdpOptions);
        }

        private void FillFavoriteRdpDisplayOptions(RdpOptions rdpOptions)
        {
            rdpOptions.ConnectToConsole = this.chkConnectToConsole.Checked;
            var userInterface = rdpOptions.UserInterface;
            userInterface.DisableWallPaper = this.chkDisableWallpaper.Checked;
            userInterface.DisableCursorBlinking = this.chkDisableCursorBlinking.Checked;
            userInterface.DisableCursorShadow = this.chkDisableCursorShadow.Checked;
            userInterface.DisableFullWindowDrag = this.chkDisableFullWindowDrag.Checked;
            userInterface.DisableMenuAnimations = this.chkDisableMenuAnimations.Checked;
            userInterface.DisableTheming = this.chkDisableThemes.Checked;
            userInterface.EnableFontSmoothing = this.AllowFontSmoothingCheckbox.Checked;
            userInterface.EnableDesktopComposition = this.AllowDesktopCompositionCheckbox.Checked;
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
            var userInterface = rdpOptions.UserInterface;
            this.chkDisableWallpaper.Checked = userInterface.DisableWallPaper;
            this.chkDisableCursorBlinking.Checked = userInterface.DisableCursorBlinking;
            this.chkDisableCursorShadow.Checked = userInterface.DisableCursorShadow;
            this.chkDisableFullWindowDrag.Checked = userInterface.DisableFullWindowDrag;
            this.chkDisableMenuAnimations.Checked = userInterface.DisableMenuAnimations;
            this.chkDisableThemes.Checked = userInterface.DisableTheming;
            this.AllowFontSmoothingCheckbox.Checked = userInterface.EnableFontSmoothing;
            this.AllowDesktopCompositionCheckbox.Checked = userInterface.EnableDesktopComposition;
        }
    }
}
