using System;

namespace Terminals.Data
{
    [Serializable]
    public class TsGwOptions
    {
        /// <summary>
        /// TSC_PROXY_MODE_NONE_DIRECT 0 (0x0)
        /// Do not use an RD Gateway server. In the Remote Desktop Connection (RDC) client UI, the Bypass RD Gateway server for local addresses check box is cleared.
        /// 
        /// TSC_PROXY_MODE_DIRECT 1 (0x1)
        /// Always use an RD Gateway server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is cleared.
        /// 
        /// TSC_PROXY_MODE_DETECT 2 (0x2)
        /// Use an RD Gateway server if a direct connection cannot be made to the RD Session Host server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is selected.
        /// 
        /// TSC_PROXY_MODE_DEFAULT 3 (0x3)
        /// Use the default RD Gateway server settings.
        /// 
        /// TSC_PROXY_MODE_NONE_DETECT 4 (0x4)
        /// Do not use an RD Gateway server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is selected.
        /// </summary>
        public Int32 UsageMethod { get; set; }
        public Int32 CredentialSource { get; set; }
        public Boolean SeparateLogin { get; set; }

        private string hostName;
        public String HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private SecurityOptions security = new SecurityOptions();
        public SecurityOptions Security
        {
            get { return security; }
            set { security = value; }
        }

        internal TsGwOptions Copy()
        {
            return new TsGwOptions
                {
                    UsageMethod = this.UsageMethod,
                    CredentialSource = this.CredentialSource,
                    SeparateLogin = this.SeparateLogin,
                    HostName = this.HostName,
                    Security = this.Security.Copy()
                };
        }

        internal void FromConfigFavorite(FavoriteConfigurationElement favorite)
        {
            this.CredentialSource = favorite.TsgwCredsSource;
            this.HostName = favorite.TsgwHostname;
            this.SeparateLogin = favorite.TsgwSeparateLogin;
            this.UsageMethod = favorite.TsgwUsageMethod;

            this.Security.Domain = favorite.TsgwDomain;
            this.Security.EncryptedPassword = favorite.TsgwEncryptedPassword;
            this.Security.UserName = favorite.TsgwUsername;
        }

        internal void FoConfigFavorite(FavoriteConfigurationElement favorite)
        {
            favorite.TsgwCredsSource = this.CredentialSource;
            favorite.TsgwHostname = this.HostName;
            favorite.TsgwSeparateLogin = this.SeparateLogin;
            favorite.TsgwUsageMethod = this.UsageMethod;

            favorite.TsgwDomain = this.Security.Domain;
            favorite.TsgwEncryptedPassword = this.Security.EncryptedPassword;
            favorite.TsgwUsername = this.Security.UserName;
        }
    }
}
