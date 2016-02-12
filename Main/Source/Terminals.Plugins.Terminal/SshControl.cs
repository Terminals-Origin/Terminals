using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class SshControl : UserControl, IProtocolOptionsControl, ISettingsConsumer
    {
        public IConnectionSettings Settings { get; set; }

        public SshControl()
        {
            InitializeComponent();
        }

        public void SaveTo(IFavorite favorite)
        {
            var sshOptions = favorite.ProtocolProperties as SshOptions;
            if (sshOptions == null)
                return;

            sshOptions.AuthMethod = this.SSHPreferences.AuthMethod;
            sshOptions.CertificateKey = this.SSHPreferences.KeyTag;
            sshOptions.SSH1 = this.SSHPreferences.SSH1;

            sshOptions.SSHKeyFile = this.SSHPreferences.SSHKeyFile;
        }

        public void LoadFrom(IFavorite favorite)
        {
            this.TryLoadSshPreferences();

            var sshOptions = favorite.ProtocolProperties as SshOptions;
            if (sshOptions == null)
                return;

            this.SSHPreferences.AuthMethod = sshOptions.AuthMethod;
            this.SSHPreferences.KeyTag = sshOptions.CertificateKey;
            this.SSHPreferences.SSH1 = sshOptions.SSH1;

            this.SSHPreferences.SSHKeyFile = sshOptions.SSHKeyFile;
        }

        private void TryLoadSshPreferences()
        {
            try
            {
                this.SSHPreferences.Keys = Settings.SSHKeys;
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                // TODO Logging.Error(
                //    "A CryptographicException occured on decrypting SSH keys. Favorite credentials possibly encrypted by another user. Ignore and continue.");
            }
        }

    }
}
