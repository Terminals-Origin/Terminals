using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    public partial class SshControl : UserControl
    {
        public SshControl()
        {
            InitializeComponent();
        }

        private void FillFavoriteSSHOptions(IFavorite favorite)
        {
            var sshOptions = favorite.ProtocolProperties as SshOptions;
            if (sshOptions == null)
                return;

            sshOptions.AuthMethod = this.SSHPreferences.AuthMethod;
            sshOptions.CertificateKey = this.SSHPreferences.KeyTag;
            sshOptions.SSH1 = this.SSHPreferences.SSH1;

            sshOptions.SSHKeyFile = this.SSHPreferences.SSHKeyFile;
        }

        private void FillSshControls(IFavorite favorite)
        {
            var sshOptions = favorite.ProtocolProperties as SshOptions;
            if (sshOptions == null)
                return;

            this.SSHPreferences.AuthMethod = sshOptions.AuthMethod;
            this.SSHPreferences.KeyTag = sshOptions.CertificateKey;
            this.SSHPreferences.SSH1 = sshOptions.SSH1;

            this.SSHPreferences.SSHKeyFile = sshOptions.SSHKeyFile;
        }

    }
}
