using System.Windows.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Plugins.Putty
{
    public partial class SshOptionsControl : UserControl, IProtocolOptionsControl
    {

        internal string[] SessionList { get; set; }

        public SshOptionsControl()
        {
            InitializeComponent();
            this.SessionList = new PuttyRegistry().GetSessions();

            BindingSource bs = new BindingSource();
            bs.DataSource = this.SessionList;
            this.cmbSessionName.DataSource = bs;
        }

        public void LoadFrom(IFavorite favorite)
        {
            var sshOptions = favorite.ProtocolProperties as SshOptions;

            if (null != sshOptions)
            {
                this.checkBoxX11Forwarding.Checked = sshOptions.X11Forwarding;
                this.checkBoxCompression.Checked = sshOptions.EnableCompression;
                this.cmbSessionName.Text = sshOptions.SessionName;
                this.checkBoxVerbose.Checked = sshOptions.Verbose;
            }
        }

        public void SaveTo(IFavorite favorite)
        {
            var sshOptions = favorite.ProtocolProperties as SshOptions;

            if (null != sshOptions)
            {
                sshOptions.X11Forwarding = this.checkBoxX11Forwarding.Checked;
                sshOptions.EnableCompression = this.checkBoxCompression.Checked;
                sshOptions.SessionName = this.cmbSessionName.Text;
                sshOptions.Verbose = this.checkBoxVerbose.Checked;
            }
        }
    }
}
