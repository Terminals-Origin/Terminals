using System;
using SSHClient;

namespace Terminals.Data
{
    [Serializable]
    public class SshOptions : ProtocolOptions
    {
        /// <summary>
        /// Gets or sets flag, if the SSH version 1 should be used, instead of ssh version 2
        /// </summary>
        public Boolean SSH1 { get; set; }

        private AuthMethod authMethod = AuthMethod.Password;
        public AuthMethod AuthMethod
        {
            get { return authMethod; }
            set { authMethod = value; }
        }

        public string SSHKeyFile { get; set; }
        private string certificateKey;
        /// <summary>
        /// Security key used to authenticate
        /// </summary>
        public String CertificateKey
        {
            get
            {
                return this.certificateKey;
            }
            set
            {
                this.certificateKey = value;
            }
        }

        private ConsoleOptions console = new ConsoleOptions();
        public ConsoleOptions Console
        {
            get { return this.console; }
            set { this.console = value; }
        }

        public override ProtocolOptions Copy()
        {
            return new SshOptions
                {
                    AuthMethod = this.AuthMethod,
                    CertificateKey = this.CertificateKey,
                    SSH1 = this.SSH1,
                    Console = this.Console.Copy2(),
                    SSHKeyFile = this.SSHKeyFile
                };
        }

        public override void FromCofigFavorite(IFavorite destination, FavoriteConfigurationElement source)
        {
            throw new NotImplementedException("moved to converter");
        }

        public override void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination)
        {
            throw new NotImplementedException("moved to converter");
        }
    }
}
