using System;

namespace Terminals.Data
{
    [Serializable]
    public class RdpSecurityOptions
    {
        public Boolean Enabled { get; set; }
        public Boolean EnableEncryption { get; set; }
        
        public Boolean EnableTLSAuthentication { get; set; }
        public Boolean EnableNLAAuthentication { get; set; }

        private string workingFolder;
        public String WorkingFolder
        {
            get
            {
                return this.workingFolder;
            }
            set
            {
                this.workingFolder = value;
            }
        }

        private string startProgram;
        public String StartProgram
        {
            get
            {
                return this.startProgram;
            }
            set
            {
                this.startProgram = value;
            }
        }

        internal RdpSecurityOptions Copy()
        {
            return new RdpSecurityOptions
                {
                    Enabled = this.Enabled,
                    EnableEncryption = this.EnableEncryption,
                    EnableNLAAuthentication = this.EnableNLAAuthentication,
                    EnableTLSAuthentication = this.EnableTLSAuthentication,
                    WorkingFolder = this.WorkingFolder,
                    StartProgram = this.StartProgram
                };
        }

        internal void FromConfigFavorite(FavoriteConfigurationElement favorite)
        {
            this.EnableEncryption = favorite.EnableEncryption;
            this.EnableNLAAuthentication = favorite.EnableNLAAuthentication;
            this.Enabled = favorite.EnableSecuritySettings;
            this.EnableTLSAuthentication = favorite.EnableTLSAuthentication;
            this.StartProgram = favorite.SecurityStartProgram;
            this.WorkingFolder = favorite.SecurityWorkingFolder;
        }

        internal void ToConfigFavorite(FavoriteConfigurationElement favorite)
        {
            favorite.EnableEncryption = this.EnableEncryption;
            favorite.EnableNLAAuthentication = this.EnableNLAAuthentication;
            favorite.EnableSecuritySettings = this.Enabled;
            favorite.EnableTLSAuthentication = this.EnableTLSAuthentication;
            favorite.SecurityStartProgram = this.StartProgram;
            favorite.SecurityWorkingFolder = this.WorkingFolder;
        }
    }
}
