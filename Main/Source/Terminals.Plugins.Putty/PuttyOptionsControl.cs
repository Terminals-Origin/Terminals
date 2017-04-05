using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Plugins.Putty
{
    public partial class PuttyOptionsControl : UserControl, IProtocolOptionsControl
    {
        public PuttyOptionsControl()
        {
            this.InitializeComponent();

            var puttyRegistry = new PuttyRegistry();
            var bindingSource = new BindingSource();
            bindingSource.DataSource = puttyRegistry.GetSessions();
            this.cmbSessionName.DataSource = bindingSource;
            this.cmbSessionName.SelectedItem = null;
        }

        public void LoadFrom(IFavorite favorite)
        {
            this.LoadFrom(favorite.ProtocolProperties);
        }

        protected virtual void LoadFrom(ProtocolOptions protocolOptions)
        {
            var puttyOptions = protocolOptions as PuttyOptions;

            if (null != puttyOptions)
            {
                this.cmbSessionName.Text = puttyOptions.SessionName;
                this.checkBoxVerbose.Checked = puttyOptions.Verbose;
            }
        }

        public void SaveTo(IFavorite favorite)
        {
            this.SaveTo(favorite.ProtocolProperties);
        }

        protected virtual void SaveTo(ProtocolOptions protocolOptions)
        {
            var puttyOptions = protocolOptions as PuttyOptions;

            if (null != puttyOptions)
            {
                puttyOptions.SessionName = this.cmbSessionName.Text;
                puttyOptions.Verbose = this.checkBoxVerbose.Checked;
            }
        }
    }
}
