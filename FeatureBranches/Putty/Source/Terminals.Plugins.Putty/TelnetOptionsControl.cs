using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Plugins.Putty
{
    public partial class TelnetOptionsControl : UserControl, IProtocolOptionsControl
    {

        internal string[] SessionList { get; set; }

        public TelnetOptionsControl()
        {
            InitializeComponent();
            this.SessionList = new PuttyRegistry().GetSessions();

            BindingSource bs = new BindingSource();
            bs.DataSource = this.SessionList;
            this.cmbSessionName.DataSource = bs;
        }

        public void LoadFrom(IFavorite favorite)
        {
            var puttyOptions = favorite.ProtocolProperties as PuttyOptions;

            if (null != puttyOptions)
            {
                this.cmbSessionName.Text = puttyOptions.SessionName;
            }
        }

        public void SaveTo(IFavorite favorite)
        {
            var puttyOptions = favorite.ProtocolProperties as PuttyOptions;

            if (null != puttyOptions)
            {
                puttyOptions.SessionName = this.cmbSessionName.Text;
            }
        }
    }
}
