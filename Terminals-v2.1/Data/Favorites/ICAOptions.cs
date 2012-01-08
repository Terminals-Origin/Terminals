using System;

namespace Terminals.Data
{
    [Serializable]
    public class ICAOptions : ProtocolOptions
    {
        public string ApplicationName { get; set; }
        public string ApplicationWorkingFolder { get; set; }
        public string ApplicationPath { get; set; }
        public string ServerINI { get; set; }
        public string ClientINI { get; set; }
        public bool EnableEncryption { get; set; }
        public string EncryptionLevel { get; set; }

        internal override ProtocolOptions Copy()
        {
            return new ICAOptions
                {
                    ApplicationName = this.ApplicationName,
                    ApplicationWorkingFolder = this.ApplicationWorkingFolder,
                    ApplicationPath = this.ApplicationPath,
                    ServerINI = this.ServerINI,
                    ClientINI = this.ClientINI,
                    EnableEncryption = this.EnableEncryption,
                    EncryptionLevel = this.EncryptionLevel
                };
        }

        internal override void FromCofigFavorite(FavoriteConfigurationElement favorite)
        {
            this.ApplicationName = favorite.ICAApplicationName;
            this.ApplicationPath = favorite.ICAApplicationPath;
            this.ApplicationWorkingFolder = favorite.ICAApplicationWorkingFolder;
            this.ClientINI = favorite.IcaClientINI;
            this.ServerINI = favorite.IcaServerINI;
            this.EnableEncryption = favorite.IcaEnableEncryption;
            this.EncryptionLevel = favorite.IcaEncryptionLevel;
        }

        internal override void ToConfigFavorite(FavoriteConfigurationElement favorite)
        {
            favorite.ICAApplicationName = this.ApplicationName;
            favorite.ICAApplicationPath = this.ApplicationPath;
            favorite.ICAApplicationWorkingFolder = this.ApplicationWorkingFolder;
            favorite.IcaClientINI = this.ClientINI;
            favorite.IcaServerINI = this.ServerINI;
            favorite.IcaEnableEncryption = this.EnableEncryption;
            favorite.IcaEncryptionLevel = this.EncryptionLevel;
        }
    }
}
