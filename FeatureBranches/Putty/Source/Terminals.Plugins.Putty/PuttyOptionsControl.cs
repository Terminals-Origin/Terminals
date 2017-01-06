using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Plugins.Putty
{
    public partial class PuttyOptionsControl : UserControl, IProtocolOptionsControl
    {

        public PuttyOptionsControl()
        {
            InitializeComponent();
        }

        public void LoadFrom(IFavorite favorite)
        {
            var puttyOptions = favorite.ProtocolProperties as PuttyOptions;

            if (null != puttyOptions)
            {
                this.checkBoxX11Forwarding.Checked = puttyOptions.X11Forwarding;
                this.checkBoxCompression.Checked = puttyOptions.EnableCompression;
                this.cmbSessionName.Text = puttyOptions.SessionName;
            }
        }

        public void SaveTo(IFavorite favorite)
        {
            var puttyOptions = favorite.ProtocolProperties as PuttyOptions;

            if (null != puttyOptions)
            {
                puttyOptions.X11Forwarding = this.checkBoxX11Forwarding.Checked;
                puttyOptions.EnableCompression = this.checkBoxCompression.Checked;
                puttyOptions.SessionName = this.cmbSessionName.Text;
            }
        }
    }
}
