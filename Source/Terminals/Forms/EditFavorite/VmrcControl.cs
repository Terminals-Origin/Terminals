using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class VmrcControl : UserControl
    {
        public VmrcControl()
        {
            InitializeComponent();
        }

        public void FillFavoriteVmrcOptions(IFavorite favorite)
        {
            var vmrcOptions = favorite.ProtocolProperties as VMRCOptions;
            if (vmrcOptions == null)
                return;

            vmrcOptions.AdministratorMode = this.VMRCAdminModeCheckbox.Checked;
            vmrcOptions.ReducedColorsMode = this.VMRCReducedColorsCheckbox.Checked;
        }

        private void FillVmrcControls(IFavorite favorite)
        {
            VMRCOptions vncOptions = favorite.ProtocolProperties as VMRCOptions;
            if (vncOptions == null)
                return;
            this.VMRCAdminModeCheckbox.Checked = vncOptions.AdministratorMode;
            this.VMRCReducedColorsCheckbox.Checked = vncOptions.ReducedColorsMode;
        }

    }
}
