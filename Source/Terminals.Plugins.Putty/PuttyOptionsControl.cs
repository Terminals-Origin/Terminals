using System;
using System.Windows.Forms;
using Terminals.Common.Forms;
using Terminals.Data;
using Terminals.Forms.EditFavorite;

namespace Terminals.Plugins.Putty
{
    public partial class PuttyOptionsControl : UserControl, IProtocolOptionsControl
    {
        public PuttyOptionsControl()
        {
            this.InitializeComponent();
            this.RefreshSessions();
            this.cmbSessionName.DataSource = this.sessionsBindingSource;
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

        private void EditSessinsButton_Click(object sender, EventArgs e)
        {
            var puttyBinaryPath = PuttyConnection.GetPuttyBinaryPath();
            ExternalLinks.OpenPath(puttyBinaryPath);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            this.RefreshSessions();
        }

        private void RefreshSessions()
        {
            var puttyRegistry = new PuttyRegistry();
            this.sessionsBindingSource.DataSource = puttyRegistry.GetSessions();
        }
    }
}
