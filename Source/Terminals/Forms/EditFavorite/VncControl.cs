using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class VncControl : UserControl, IProtocolOptionsControl
    {
        public VncControl()
        {
            InitializeComponent();
        }

        public void SaveTo(IFavorite favorite)
        {
            var vncOptions = favorite.ProtocolProperties as VncOptions;
            if (vncOptions == null)
                return;

            vncOptions.AutoScale = this.vncAutoScaleCheckbox.Checked;
            vncOptions.DisplayNumber = (Int32)this.vncDisplayNumberInput.Value;
            vncOptions.ViewOnly = this.VncViewOnlyCheckbox.Checked;
        }

        public void LoadFrom(IFavorite favorite)
        {
            VncOptions vncOptions = favorite.ProtocolProperties as VncOptions;
            if (vncOptions == null)
                return;
            this.vncAutoScaleCheckbox.Checked = vncOptions.AutoScale;
            this.vncDisplayNumberInput.Value = vncOptions.DisplayNumber;
            this.VncViewOnlyCheckbox.Checked = vncOptions.ViewOnly;
        }
    }
}
