using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    public partial class SshControl : UserControl
    {
        public SshControl()
        {
            InitializeComponent();
        }

        internal void ResetSshPreferences()
        {
            try
            {
                this.SSHPreferences.Keys = Settings.SSHKeys;
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                Logging.Error(
                    "A CryptographicException occured on decrypting SSH keys. Favorite credentials possibly encrypted by another user. Ignore and continue.");
            }
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
