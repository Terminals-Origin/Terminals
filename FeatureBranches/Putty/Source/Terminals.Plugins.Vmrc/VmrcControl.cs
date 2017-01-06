using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class VmrcControl : UserControl, IProtocolOptionsControl
    {
        public VmrcControl()
        {
            InitializeComponent();
        }

        public void SaveTo(IFavorite favorite)
        {
            var vmrcOptions = favorite.ProtocolProperties as VMRCOptions;
            if (vmrcOptions == null)
                return;

            vmrcOptions.AdministratorMode = this.VMRCAdminModeCheckbox.Checked;
            vmrcOptions.ReducedColorsMode = this.VMRCReducedColorsCheckbox.Checked;
        }

        public void LoadFrom(IFavorite favorite)
        {
            var vncOptions = favorite.ProtocolProperties as VMRCOptions;
            if (vncOptions == null)
                return;

            this.VMRCAdminModeCheckbox.Checked = vncOptions.AdministratorMode;
            this.VMRCReducedColorsCheckbox.Checked = vncOptions.ReducedColorsMode;
        }
    }
}
